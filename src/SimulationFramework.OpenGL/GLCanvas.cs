using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
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
    public CanvasState State { get; set; }

    private Stack<CanvasState> stateStack = [];
    private List<CanvasState> statePool = [];

    private PositionGeometryStream positionGeometryStream;
    private ColorGeometryStream colorGeometryStream;
    private TextureGeometryStream textureGeometryStream;

    private FillGeometryWriter fillGeometryWriter;
    private HairlineGeometryWriter hairlineGeometryWriter;
    private PathGeometryWriter pathGeometryWriter;

    private ColorGeometryEffect colorGeometryEffect;
    private TextureGeometryEffect textureGeometryEffect;

    private GeometryEffect currentGeometryEffect;

    private GeometryStream currentGeometryStream;
    private GeometryWriter currentGeometryWriter;
    private GeometryBuffer currentGeometryBuffer;

    private GLGraphicsProvider graphics;

    public GLCanvas(GLGraphicsProvider graphics, ITexture target)
    {
        this.graphics = graphics;
        this.Target = target;
        
        State = new();
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

        ResetState();
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
        glClearColor(color.R, color.G, color.B, color.A);
        glClear(GL_COLOR_BUFFER_BIT);
    }

    public void Clip(Rectangle? rectangle)
    {
        throw new NotImplementedException();
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
        SetupRegularGeometry();
        currentGeometryWriter.PushPolygon(currentGeometryStream, polygon, close);
    }

    public void DrawRect(Rectangle rect)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushRect(currentGeometryStream, rect);
    }

    public void DrawRoundedRect(Rectangle rect, float radius)
    {
        SetupRegularGeometry();
        currentGeometryWriter.PushRoundedRect(currentGeometryStream, rect, radius);
    }

    public void DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft, TextBounds origin = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Rectangle source, Rectangle destination)
    {
        if (textureGeometryEffect.texture != texture)
        {
            Flush();
        }

        textureGeometryEffect.texture = (GLTexture)texture;
        UpdateGeometryStream(textureGeometryStream);
        currentGeometryEffect = textureGeometryEffect;

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
            currentGeometryEffect = colorGeometryEffect;
        }
    }

    public void DrawTriangles(ReadOnlySpan<Vector2> triangles)
    {
        currentGeometryWriter.PushTriangles(currentGeometryStream, triangles);
    }

    public void Fill(ColorF color)
    {
        State.color = color;
        State.fill = true;

        colorGeometryStream.Color = color.ToColor();
        
        currentGeometryEffect = colorGeometryEffect;

        UpdateGeometryStream(colorGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);
    }

    public void Fill(CanvasShader shader)
    {
        var effect = graphics.GetShaderEffect(shader);
        effect.Shader = shader;
        currentGeometryEffect = effect;
        UpdateGeometryStream(positionGeometryStream);
        UpdateGeometryWriter(fillGeometryWriter);


    }

    public unsafe void Flush()
    {
        if (currentGeometryStream.GetVertexCount() == 0)
            return;

        glViewport(0, 0, Target.Width, Target.Height);

        Matrix3x2 viewportMatrix = Matrix3x2.CreateScale(2f / Target.Width, -2f / Target.Height) * Matrix3x2.CreateTranslation(-1, 1);

        Matrix3x2 transform = this.State.Transform * viewportMatrix;
        Matrix4x4 transform4x4 = new(
            transform.M11, transform.M12, 0, 0,
            transform.M21, transform.M22, 0, 0,
            0, 0, 1, 0,
            transform.M31, transform.M32, 0, 1
            );

        currentGeometryStream.Upload(currentGeometryBuffer);
        currentGeometryEffect.Use();
        currentGeometryEffect.ApplyState(State, transform4x4);
        currentGeometryStream.BindVertexArray();

        glDrawArrays(currentGeometryWriter.GetPrimitive(), 0, currentGeometryStream.GetVertexCount());
        currentGeometryStream.Clear();
    }


    public void Font(IFont font)
    {
        throw new NotImplementedException();
    }

    public void FontSize(float size)
    {
        throw new NotImplementedException();
    }

    public void FontStyle(FontStyle style)
    {
        throw new NotImplementedException();
    }

    public Vector2 MeasureText(ReadOnlySpan<char> text, float maxLength, out int charsMeasured, TextBounds bounds = TextBounds.BestFit)
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        if (stateStack.Count == 0)
            throw new InvalidOperationException("State stack is empty!");

        State = stateStack.Pop();
    }

    public void PushState()
    {
        stateStack.Push(State);
        State = State.Clone();
    }

    public void ResetState()
    {
        State.Reset();

        colorGeometryStream.Color = Color.White;
        currentGeometryEffect = colorGeometryEffect;
        currentGeometryWriter = fillGeometryWriter;
        currentGeometryStream = colorGeometryStream;
    }

    public void SetTransform(Matrix3x2 transform)
    {
        State.Transform = transform;
    }

    public void Stroke(Color color)
    {
        State.color = color.ToColorF();
        State.fill = false;

        colorGeometryStream.Color = color;

        if (currentGeometryWriter != hairlineGeometryWriter)
        {
            Flush();
        }

        if (State.strokeWidth == 0)
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
        State.strokeWidth = width;
        pathGeometryWriter.Width = width; 
        
        if (State.strokeWidth == 0)
        {
            UpdateGeometryWriter(hairlineGeometryWriter);
        }
        else
        {
            UpdateGeometryWriter(pathGeometryWriter);
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
    }
}