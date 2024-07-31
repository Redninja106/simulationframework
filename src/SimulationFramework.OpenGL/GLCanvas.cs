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

namespace SimulationFramework.OpenGL;
internal class GLCanvas : ICanvas
{
    public ITexture Target { get; }
    public ref readonly CanvasState State => ref currentState;
    public CanvasState currentState;

    private Stack<CanvasState> stateStack = [];

    private readonly PositionGeometryStream positionGeometryStream;
    private readonly ColorGeometryStream colorGeometryStream;
    private readonly TextureGeometryStream textureGeometryStream;
    private readonly TextureGeometryStream sdfGeometryStream;

    private readonly FillGeometryWriter fillGeometryWriter;
    private readonly HairlineGeometryWriter hairlineGeometryWriter;
    private readonly PathGeometryWriter pathGeometryWriter;

    private readonly ColorGeometryEffect colorGeometryEffect;
    private readonly TextureGeometryEffect textureGeometryEffect;
    private readonly SDFFontEffect sdfGeometryEffect;

    private GeometryStream currentGeometryStream;
    private GeometryWriter currentGeometryWriter;
    private GeometryBuffer geometryBuffer;
    // private unsafe List<SubmittedDrawCommand> submittedCommands = [];

    private GLGraphicsProvider graphics;
    private uint fbo;

    private GeometryWriter GetCurrentGeometryWriter()
    {
        if (currentState.Fill)
        {
            return fillGeometryWriter;
        }

        if (currentState.StrokeWidth == 0)
        {
            return hairlineGeometryWriter;
        }
        else
        {
            return pathGeometryWriter;
        }
    }

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

            if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
            {
                Log.Warn("framebuffer is not complete!");
            }
            glBindFramebuffer(GL_FRAMEBUFFER, 0);
        }
        else
        {
            fbo = 0;
        }

        currentState = new();
        geometryBuffer = new();

        positionGeometryStream = new PositionGeometryStream();
        colorGeometryStream = new ColorGeometryStream();
        textureGeometryStream = new TextureGeometryStream();
        sdfGeometryStream = new TextureGeometryStream();

        fillGeometryWriter = new FillGeometryWriter();
        hairlineGeometryWriter = new HairlineGeometryWriter();
        pathGeometryWriter = new PathGeometryWriter();

        colorGeometryEffect = new ColorGeometryEffect();
        textureGeometryEffect = new TextureGeometryEffect();
        sdfGeometryEffect = new SDFFontEffect();

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
        var writer = GetCurrentGeometryWriter();
        writer.PushArc(GetCurrentGeometryStream(), bounds, begin, end, includeCenter);
    }

    private GeometryStream GetCurrentGeometryStream()
    {
        if (State.Shader is null)
        {
            return colorGeometryStream;
        }
        else
        {
            return positionGeometryStream;
        }
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        SetupRegularGeometry();
        if (State.Fill)
        {
            Stroke(State.Color.ToColor());
            currentGeometryWriter.PushLine(currentGeometryStream, p1, p2);
            Fill(State.Color);
        }
        else
        {
            currentGeometryWriter.PushLine(currentGeometryStream, p1, p2);
        }
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

    public Vector2 DrawText(ReadOnlySpan<char> text, float size, Vector2 baseline, TextStyle style = TextStyle.Regular)
    {
        for (int i = 0; i < text.Length; i++)
        {
            baseline = DrawCodepoint(text[i], size, baseline, style);
        }

        return baseline;
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination, ColorF tint)
    {
        if (textureGeometryEffect.texture != texture || textureGeometryEffect.tint != tint)
        {
            SubmitDrawCommands();
        }
        textureGeometryEffect.texture = (GLTexture)texture;
        textureGeometryEffect.texture.PrepareForRender();
        textureGeometryEffect.tint = tint;

        UpdateGeometryWriter(fillGeometryWriter);
        UpdateGeometryStream(textureGeometryStream);

        Vector2 uvScale = new(1f / texture.Width, 1f / texture.Height);
        textureGeometryStream.WriteVertexFlipUV(new(destination.X, destination.Y), uvScale * new Vector2(source.X, source.Y));
        textureGeometryStream.WriteVertexFlipUV(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        textureGeometryStream.WriteVertexFlipUV(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        textureGeometryStream.WriteVertexFlipUV(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        textureGeometryStream.WriteVertexFlipUV(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        textureGeometryStream.WriteVertexFlipUV(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));
    }

    private void SetupRegularGeometry()
    {
        if (currentGeometryStream == textureGeometryStream || currentGeometryStream == sdfGeometryStream)
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
        UpdateGeometryStream(colorGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);

        currentState.Color = color;
        currentState.Fill = true;

        colorGeometryStream.Color = color.ToColor();
    }

    public void Fill(CanvasShader shader)
    {
        UpdateGeometryStream(positionGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);
        var effect = graphics.GetShaderEffect(shader);
        if (effect.Shader != shader)
            SubmitDrawCommands();
        effect.Shader = shader;
        currentState.Shader = shader;
    }

    public Vector2 DrawCodepoint(int codepoint, float size, Vector2 baseline, TextStyle style = TextStyle.Regular)
    {
        GLFont font = (GLFont)currentState.Font;
        var atlas = font.GetAtlas(style);
        if (!font.SupportsBold && style.HasFlag(TextStyle.Bold))
        {
            sdfGeometryEffect.boldThreshold = true;
        }
        else
        {
            sdfGeometryEffect.boldThreshold = false;
        }

        if (!font.SupportsItalic && style.HasFlag(TextStyle.Italic))
        {
            // sdfGeometryEffect.slant = 1f;
        }

        sdfGeometryEffect.fontAtlas = atlas.GetTextureID();

        UpdateGeometryWriter(fillGeometryWriter);
        UpdateGeometryStream(sdfGeometryStream);

        baseline = atlas.GetCodepoint(codepoint, size, baseline, out Rectangle source, out Rectangle destination);

        Vector2 uvScale = new(1f / atlas.Width, 1f / atlas.Height);
        sdfGeometryStream.WriteVertex(new(destination.X, destination.Y), uvScale * new Vector2(source.X, source.Y));
        sdfGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        sdfGeometryStream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        sdfGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        sdfGeometryStream.WriteVertex(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        sdfGeometryStream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        return baseline;

        // Vector2 newPos = font.GetCodepointPosition(codepoint, position, currentState.FontSize, currentState.FontStyle, out Rectangle source, out Rectangle destination);
        // GLTexture atlas = font.GetAtlasTexture(currentState.FontSize, currentState.FontStyle);
        // DrawTexture(atlas, source, destination, currentState.Color);
        // return newPos;
    }

    public unsafe void Flush()
    {
        SubmitDrawCommands();
        glFinish();
        geometryBuffer.Reset();
    }

    private void SubmitDrawCommands()
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

        int vertexSize = currentGeometryStream.GetVertexSize();
        if (geometryBuffer.offset % vertexSize != 0)
        {
            geometryBuffer.offset += vertexSize - (geometryBuffer.offset % vertexSize);
        }

        int offset = geometryBuffer.offset;
        currentGeometryStream.Upload(geometryBuffer);

        geometryBuffer.Bind();
        currentGeometryStream.BindVertexArray();

        var effect = GetEffect(currentGeometryStream);
        effect.Use();
        effect.ApplyState(currentState, transform4x4);
        currentGeometryStream.BindVertexArray();

        glDrawArrays(currentGeometryWriter.GetPrimitive(), offset / vertexSize, currentGeometryStream.GetVertexCount());
        currentGeometryStream.Clear();
    }

    private GeometryEffect GetEffect(GeometryStream geometryStream)
    {
        if (geometryStream == textureGeometryStream)
        {
            return textureGeometryEffect;
        }

        if (geometryStream == sdfGeometryStream)
        {
            return sdfGeometryEffect;
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

    public void PopState()
    {
        if (stateStack.Count == 0)
            throw new InvalidOperationException("State stack is empty!");

        var newState = stateStack.Pop();
        if (currentState.Shader != newState.Shader ||
            currentState.Color != newState.Color ||
            currentState.Fill != newState.Fill ||
            currentState.StrokeWidth != newState.StrokeWidth)
        {
            SubmitDrawCommands();
        }
        currentState = newState;
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
        if (currentGeometryWriter != hairlineGeometryWriter)
        {
            SubmitDrawCommands();
        }

        if (currentState.StrokeWidth == 0)
        {
            UpdateGeometryWriter(hairlineGeometryWriter);
        }
        else
        {
            UpdateGeometryWriter(pathGeometryWriter);
        }
        UpdateGeometryStream(colorGeometryStream);

        currentState.Color = color.ToColorF();
        currentState.Fill = false;

        colorGeometryStream.Color = color;
    }

    public void StrokeWidth(float width)
    {
        if (!currentState.Fill)
        {
            if (width == 0)
            {
                UpdateGeometryWriter(hairlineGeometryWriter);
            }
            else
            {
                UpdateGeometryWriter(pathGeometryWriter);
            }
        }

        currentState.StrokeWidth = width;
        pathGeometryWriter.StrokeWidth = width;
    }

    private void UpdateGeometryWriter(GeometryWriter writer)
    {
        if (currentGeometryWriter.GetPrimitive() != writer.GetPrimitive())
        {
            SubmitDrawCommands();
        }

        currentGeometryWriter = writer;
    }

    private void UpdateGeometryStream(GeometryStream stream)
    {
        if (currentGeometryStream != stream && currentGeometryStream.GetVertexCount() > 0)
        {
            SubmitDrawCommands();
        }

        currentGeometryStream = stream;
        stream.TransformMatrix = currentState.Transform;
    }
}

class GLGeometry
{
    GLBuffer vertexBuffer;
    GLBuffer? indexBuffer;

    public void Draw(GLCanvas canvas)
    {
    }
}

class GLBuffer
{

}