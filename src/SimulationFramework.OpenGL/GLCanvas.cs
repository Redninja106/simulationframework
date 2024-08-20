using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.OpenGL.Commands;
using SimulationFramework.OpenGL.Fonts;
using SimulationFramework.OpenGL.Geometry;
using SimulationFramework.OpenGL.Geometry.Streams;
using SimulationFramework.OpenGL.Geometry.Writers;
using SimulationFramework.OpenGL.Shaders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class GLCanvas : ICanvas
{
    public ITexture Target { get; }
    public ref readonly CanvasState State => ref currentState;
    public CanvasState currentState;

    private Stack<CanvasState> stateStack = [];
    private Queue<RenderCommand> commandQueue = [];
    private RenderCommand? currentCommand = null;

    GeometryBufferWriter bufferWriter;

    // TODO: multiple frames in flight

    private GeometryStreamCollection streams;
    private GeometryEffectCollection effects;
    private GeometryWriterCollection writers;

    private GLGraphics graphics;
    public readonly uint fbo;

    public unsafe GLCanvas(GLGraphics graphics, ITexture target)
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

        streams = new(graphics);
        effects = new(graphics);
        writers = new();

        bufferWriter = new();
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
        var writer = writers.GetWriter(in State);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushArc(stream, bounds, begin, end, includeCenter);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        var writer = writers.GetWriter(in State, true);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushLine(stream, p1, p2);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        if (polygon.Length < 2)
            return;

        var writer = writers.GetWriter(in State);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushPolygon(stream, polygon, close);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    public void DrawRect(Rectangle rect)
    {
        var writer = writers.GetWriter(in State);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushRect(stream, rect);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    private void SubmitStream(GeometryStream stream, GeometryEffect effect, bool triangles, ReadOnlySpan<byte> bytes = default)
    {
        int vertexSize = stream.GetVertexSize();
        if (bytes.IsEmpty)
        {
            bytes = stream.GetData();
        }

        int offset, count;

        if (currentCommand is StreamRenderCommand streamCommand)
        {
            if (streamCommand.State.WriteMask == State.WriteMask && streamCommand.State.WriteMaskValue == State.WriteMaskValue && streamCommand.State.Mask == State.Mask)
            {
                if (streamCommand.stream == stream && streamCommand.Effect.CheckStateCompatibility(ref currentState))
                {
                    if (streamCommand.buffer.TryWrite(bytes, vertexSize, out offset, out count))
                    {
                        streamCommand.AddCommand(triangles, offset / vertexSize, count / vertexSize);
                        stream.Clear();
                        return;
                    }
                }
            }
        }

        var buffer = bufferWriter.Write(bytes, vertexSize, out offset, out count);

        StreamRenderCommand cmd = new(effect, currentState)
        {
            buffer = buffer,
            stream = stream,
        };

        cmd.AddCommand(triangles, offset / vertexSize, count / vertexSize);
        stream.Clear();

        commandQueue.Enqueue(cmd);
        currentCommand = cmd;
    }

    public void DrawEllipse(Rectangle bounds)
    {
        var stream = streams.GetStream(in State);
        var writer = writers.GetWriter(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushEllipse(stream, bounds);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    public void DrawRoundedRect(Rectangle rect, Vector2 radii)
    {
        var writer = writers.GetWriter(in State);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushRoundedRect(stream, rect, radii);
        SubmitStream(stream, effect, writer.UsesTriangles);
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
        var stream = streams.GetTextureGeometryStream();
        stream.TransformMatrix = State.Transform;

        var effect = effects.GetTextureGeometryEffect(texture, tint);
        effect.texture = (GLTexture)texture;
        effect.texture.PrepareForRender();
        effect.tint = tint;

        Vector2 uvScale = new(1f / texture.Width, 1f / texture.Height);
        stream.WriteVertex(new(destination.X, destination.Y), uvScale * new Vector2(source.X, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        SubmitStream(stream, effect, true);
    }

    public void DrawTriangles(ReadOnlySpan<Vector2> triangles)
    {
        var writer = writers.GetWriter(in State);
        var stream = streams.GetStream(in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        writer.PushTriangles(stream, triangles);
        SubmitStream(stream, effect, writer.UsesTriangles);
    }

    public void DrawTriangles<TVertex>(ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        if (State.VertexShader is null)
        {
            throw new InvalidOperationException("Must have a vertex shader to accept custom vertices!");
        }
        var stream = streams.GetCustomVertexGeometryStream(typeof(TVertex), in State);
        var effect = effects.GetEffectFromCanvasState(in State);
        SubmitStream(stream, effect, true, MemoryMarshal.AsBytes(vertices));
    }

    public void Fill(ColorF color)
    {
        currentState.Color = color;
        currentState.Fill = true;
        currentState.Shader = null;
        currentState.VertexShader = null;
    }

    public void Fill(CanvasShader shader)
    {
        Fill(shader, vertexShader: null);
    }

    public void Fill(CanvasShader shader, VertexShader? vertexShader)
    {
        // ProgrammableShaderEffect effect = graphics.GetShaderProgram(shader, vertexShader);

        currentState.Fill = true;
        currentState.Shader = shader;
        currentState.VertexShader = vertexShader;
    }

    public void Stroke(ColorF color)
    {
        currentState.Color = color;
        currentState.Fill = false;
        currentState.Shader = null;
        currentState.VertexShader = null;
    }

    public void Stroke(CanvasShader shader)
    {
        Stroke(shader, vertexShader: null);
    }

    public void Stroke(CanvasShader shader, VertexShader? vertexShader)
    {
        // ProgrammableShaderEffect effect = graphics.GetShaderProgram(shader, vertexShader);

        currentState.Fill = false;
        currentState.Shader = shader;
        currentState.VertexShader = vertexShader;
    }

    public Vector2 DrawCodepoint(int codepoint, float size, Vector2 baseline, TextStyle style = TextStyle.Regular)
    {
        var stream = streams.GetTextureGeometryStream();
        stream.TransformMatrix = State.Transform;
        GLFont font = (GLFont)currentState.Font;
        var effect = effects.GetFontEffect(font, State.Color, style);
        var atlas = font.GetAtlas(style);
        if (!font.SupportsBold && (style & TextStyle.Bold) != 0)
        {
            effect.boldThreshold = true;
        }
        else
        {
            effect.boldThreshold = false;
        }

        if (!font.SupportsItalic && (style & TextStyle.Italic) != 0)
        {
            // sdfGeometryEffect.slant = 1f;
        }
        
        baseline = atlas.GetCodepoint(codepoint, size, baseline, out Rectangle source, out Rectangle destination);

        Vector2 uvScale = new(1f / atlas.Width, 1f / atlas.Height);
        stream.WriteVertex(new(destination.X, destination.Y), uvScale * new Vector2(source.X, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        SubmitStream(stream, effect, true);

        return baseline;
    }

    public unsafe void Flush()
    {
        Matrix3x2 viewportMatrix;

        if (fbo == 0)
        {
            viewportMatrix = Matrix3x2.CreateScale(2f / Target.Width, -2f / Target.Height) * Matrix3x2.CreateTranslation(-1, 1);
        }
        else
        {
            viewportMatrix = Matrix3x2.CreateScale(2f / Target.Width, 2f / Target.Height) * Matrix3x2.CreateTranslation(-1, -1);
        }

        Matrix3x2 transform = viewportMatrix;
        Matrix4x4 transform4x4 = new(
            transform.M11, transform.M12, 0, 0,
            transform.M21, transform.M22, 0, 0,
            0, 0, 1, 0,
            transform.M31, transform.M32, 0, 1
            );

        while (commandQueue.TryDequeue(out RenderCommand? command))
        {
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
            glEnable(GL_BLEND);
            glDisable(GL_CULL_FACE);
            glPolygonMode(GL_FRONT_AND_BACK, true ? GL_FILL : GL_LINE);
            glViewport(0, 0, Target.Width, Target.Height);
            glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);

            if (command.State.Mask is GLMask mask)
            {
                mask.BindRead(Target);
            }
            else
            {
                glDisable(GL_DEPTH_TEST);
                glDisable(GL_STENCIL_TEST);
            }

            if (command.State.WriteMask is GLMask writeMask)
            {
                writeMask.BindWrite(command.State.WriteMaskValue);
            }

            command.Effect.Apply(this, transform4x4);
            command.Submit();
        }
        currentCommand = null;
        glFlush();
        bufferWriter.Reset();
    }

    public void DrawGeometry(IGeometry geometry)
    {
        throw new NotImplementedException();
    }

    //public void DrawGeometryInstances(IGeometry geometry, ReadOnlySpan<Matrix3x2> instances)
    //{
    //    GLGeometry glGeometry = (GLGeometry)geometry;
    //    throw new NotImplementedException();
    //}

    //public void DrawGeometryInstances<TInstance>(IGeometry geometry, ReadOnlySpan<TInstance> instances)
    //    where TInstance : unmanaged
    //{
    //    GLGeometry glGeometry = (GLGeometry)geometry;
    //    throw new NotImplementedException();
    //}

    public void Font(IFont font)
    {
        currentState.Font = font;
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

    public void ResetState()
    {
        currentState.Reset();
    }

    public void SetTransform(Matrix3x2 transform)
    {
        currentState.Transform = transform;
    }

    public void StrokeWidth(float width)
    {
        currentState.StrokeWidth = width;
    }

    public void DrawLines(ReadOnlySpan<Vector2> vertices)
    {
        var writer = writers.GetWriter(in State, true);
        var stream = streams.GetStream(in State);
        for (int i = 0; i < vertices.Length; i += 2)
        {
            writer.PushLine(stream, vertices[i], vertices[i + 1]);
        }
        SubmitStream(stream, effects.GetEffectFromCanvasState(in State), writer.UsesTriangles);
    }

    public void DrawLines<TVertex>(ReadOnlySpan<TVertex> vertices) where TVertex : unmanaged
    {
        var stream = streams.GetCustomVertexGeometryStream(typeof(TVertex), in State);
        for (int i = 0; i < vertices.Length; i++)
        {
            stream.WriteVertex(vertices[i]);
        }
        SubmitStream(stream, effects.GetEffectFromCanvasState(in State), false);
    }

    public void Mask(IMask? mask)
    {
        currentState.Mask = mask;
    }

    public void WriteMask(IMask? mask, bool value)
    {
        currentState.WriteMask = mask;
        currentState.WriteMaskValue = value;
    }
}
