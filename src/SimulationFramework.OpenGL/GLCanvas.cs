using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OpenGL;

namespace SimulationFramework.OpenGL;
internal class GLCanvas : ICanvas
{
    public ITexture Target { get; }
    public ref readonly CanvasState State => ref currentState;
    public CanvasState currentState;

    private Stack<CanvasState> stateStack = [];
    private List<CanvasState> statePool = [];

    private readonly PositionGeometryStream positionGeometryStream;
    private readonly ColorGeometryStream colorGeometryStream;
    private readonly TextureGeometryStream textureGeometryStream;

    private readonly FillGeometryWriter fillGeometryWriter;
    private readonly HairlineGeometryWriter hairlineGeometryWriter;
    private readonly PathGeometryWriter pathGeometryWriter;

    private readonly ColorGeometryEffect colorGeometryEffect;
    private readonly TextureGeometryEffect textureGeometryEffect;

    private GeometryStream currentGeometryStream;
    private GeometryWriter currentGeometryWriter;
    private GeometryBuffer currentGeometryBuffer;

    private GLGraphicsProvider graphics;
    private uint fbo;

    public unsafe GLCanvas(GLGraphicsProvider graphics, ITexture target)
    {
        this.graphics = graphics;
        this.Target = target;
        if (target is GLTexture texture)
        {
            fixed (uint* fboPtr = &fbo)
            {
                glGenFramebuffers(1, fboPtr);
            }

            glBindFramebuffer(GL_FRAMEBUFFER, fbo);
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, texture.GetID(), 0);
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
        }
        else
        {
            fbo = 0;
        }

        currentState = new();
        currentGeometryBuffer = new GeometryBuffer();
        currentGeometryBuffer.Bind();

        positionGeometryStream = new PositionGeometryStream();
        colorGeometryStream = new ColorGeometryStream();
        textureGeometryStream = new TextureGeometryStream();

        fillGeometryWriter = new FillGeometryWriter();
        hairlineGeometryWriter = new HairlineGeometryWriter();
        pathGeometryWriter = new PathGeometryWriter();

        colorGeometryEffect = new ColorGeometryEffect();
        textureGeometryEffect = new TextureGeometryEffect();

        currentGeometryStream = colorGeometryStream;
        currentGeometryWriter = fillGeometryWriter;
    }


    public void Antialias(bool antialias)
    {
        if (antialias) 
        {
            glEnable(GL_MULTISAMPLE);
        }
        else
        {
            glDisable(GL_MULTISAMPLE);
        }
    }

    public void Clear(ColorF color)
    {
        glBindFramebuffer(GL_FRAMEBUFFER, fbo);
        glClearColor(color.R, color.G, color.B, color.A);
        glClear(GL_COLOR_BUFFER_BIT);
    }

    public void Clip(Rectangle? rectangle)
    {
        currentState.ClipRectangle = rectangle;
    }

    public void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushArc(currentGeometryStream, bounds, begin, end, includeCenter);
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushLine(currentGeometryStream, p1, p2);
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        if (polygon.Length < 2)
            return;

        SetupRegularGeometry();
        currentGeometryWriter.PushPolygon(currentGeometryStream, polygon, close);
    }

    public void DrawRect(Rectangle rect)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushRect(currentGeometryStream, rect);
    }

    public void DrawEllipse(Rectangle bounds)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushEllipse(currentGeometryStream, bounds);
    }

    public void DrawRoundedRect(Rectangle rect, Vector2 radii)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushRoundedRect(currentGeometryStream, rect, radii);
    }

    public Vector2 DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        for (int i = 0; i < text.Length; i++)
        {
            position = DrawCodepoint(text[i], position, alignment);
        }

        return position;
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination, ColorF tint)
    {
        if (textureGeometryEffect.texture != texture || textureGeometryEffect.tint != tint)
        {
            Flush();
        }
        textureGeometryEffect.texture = (GLTexture)texture;
        textureGeometryEffect.tint = tint;

        UpdateGeometryStream(textureGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);

        Vector2 uvScale = new(1f/texture.Width, 1f/texture.Height);
        textureGeometryStream.WriteVertex(new(destination.X,                     destination.Y                     ), uvScale * new Vector2(source.X, source.Y));
        textureGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y                     ), uvScale * new Vector2(source.X + source.Width, source.Y));
        textureGeometryStream.WriteVertex(new(destination.X,                     destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));
        
        textureGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y                     ), uvScale * new Vector2(source.X + source.Width, source.Y));
        textureGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        textureGeometryStream.WriteVertex(new(destination.X,                     destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));
    }

    private void SetupRegularGeometry()
    {
        if (currentGeometryStream == textureGeometryStream)
        {
            UpdateGeometryStream(colorGeometryStream);
        }
        else
        {
            currentGeometryStream.TransformMatrix = this.currentState.Transform;
        }
    }

    public void DrawTriangles(ReadOnlySpan<Vector2> triangles)
    {
        currentGeometryWriter.PushTriangles(currentGeometryStream, triangles);
    }

    public void Fill(ColorF color)
    {
        currentState.Color = color;
        currentState.Fill = true;

        colorGeometryStream.Color = color.ToColor();
        
        UpdateGeometryStream(colorGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);
    }

    public void Fill(CanvasShader shader)
    {
        var effect = graphics.GetShaderEffect(shader);
        if (effect.Shader != shader)
            Flush();
        effect.Shader = shader;
        currentState.Shader = shader;
        UpdateGeometryStream(positionGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);
    }

    public Vector2 DrawCodepoint(int codepoint, Vector2 position, Alignment alignment)
    {
        GLFont font = (GLFont)currentState.Font;
        Vector2 newPos = font.GetCodepointPosition(codepoint, position, currentState.FontSize, currentState.FontStyle, out Rectangle source, out Rectangle destination);
        GLTexture atlas = font.GetAtlasTexture(currentState.FontSize, currentState.FontStyle);
        DrawTexture(atlas, source, destination, currentState.Color);
        return newPos;
    }

    public unsafe void Flush()
    {
        if (currentGeometryStream.GetVertexCount() == 0)
            return;

        glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
        glEnable(GL_BLEND);
        glDisable(GL_CULL_FACE);
        glViewport(0, 0, Target.Width, Target.Height);
        glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);

        Matrix3x2 viewportMatrix = Matrix3x2.CreateScale(2f / Target.Width, -2f / Target.Height) * Matrix3x2.CreateTranslation(-1, 1);

        Matrix3x2 transform = viewportMatrix;
        Matrix4x4 transform4x4 = new(
            transform.M11, transform.M12, 0, 0,
            transform.M21, transform.M22, 0, 0,
            0, 0, 1, 0,
            transform.M31, transform.M32, 0, 1
            );

        currentGeometryStream.Upload(currentGeometryBuffer);

        var effect = GetEffect(currentGeometryStream);
        effect.Use();
        effect.ApplyState(currentState, transform4x4);
        currentGeometryStream.BindVertexArray();

        glDrawArrays(currentGeometryWriter.GetPrimitive(), 0, currentGeometryStream.GetVertexCount());
        currentGeometryStream.Clear();
    }

    private GeometryEffect GetEffect(GeometryStream geometryStream)
    {
        if (geometryStream == textureGeometryStream)
        {
            return textureGeometryEffect;
        }

        if (currentState.Shader is not null)
        {
            var effect = graphics.GetShaderEffect(currentState.Shader);
            effect.Shader = currentState.Shader;
            return effect;
        }

        return colorGeometryEffect;
    }


    public void Font(IFont font)
    {
        currentState.Font = font;
    }

    public void FontSize(float size)
    {
        currentState.FontSize = size;
    }

    public void FontStyle(FontStyle style)
    {
        currentState.FontStyle = style;
    }

    public void PopState()
    {
        if (stateStack.Count == 0)
            throw new InvalidOperationException("State stack is empty!");

        currentState = stateStack.Pop();
    }

    public void PushState()
    {
        stateStack.Push(currentState);
    }


    [MemberNotNull(
        nameof(currentGeometryWriter),
        nameof(currentGeometryStream)
        )]
    public void ResetState()
    {
        currentState.Reset();

        colorGeometryStream.Color = Color.White;
        currentGeometryWriter = fillGeometryWriter;
        currentGeometryStream = colorGeometryStream;
    }

    public void SetTransform(Matrix3x2 transform)
    {
        currentState.Transform = transform;
    }

    public void Stroke(Color color)
    {
        currentState.Color = color.ToColorF();
        currentState.Fill = false;

        colorGeometryStream.Color = color;

        if (currentGeometryWriter != hairlineGeometryWriter)
        {
            Flush();
        }

        if (currentState.strokeWidth == 0)
        {
            UpdateGeometryWriter(hairlineGeometryWriter);
        }
        else
        {
            UpdateGeometryWriter(pathGeometryWriter);
        }
        UpdateGeometryStream(colorGeometryStream);
    }

    public void StrokeWidth(float width)
    {
        currentState.strokeWidth = width;
        pathGeometryWriter.StrokeWidth = width;

        if (!currentState.Fill)
        {
            if (currentState.strokeWidth == 0)
            {
                UpdateGeometryWriter(hairlineGeometryWriter);
            }
            else
            {
                UpdateGeometryWriter(pathGeometryWriter);
            }
        }
    }

    private void UpdateGeometryWriter(GeometryWriter writer)
    {
        if (currentGeometryWriter.GetPrimitive() != writer.GetPrimitive())
        {
            Flush();
        }

        currentGeometryWriter = writer;
    }

    private void UpdateGeometryStream(GeometryStream stream)
    {
        if (currentGeometryStream != stream && currentGeometryStream.GetVertexCount() > 0)
        {
            Flush();
        }

        currentGeometryStream = stream;
        stream.TransformMatrix = currentState.Transform;
    }
}