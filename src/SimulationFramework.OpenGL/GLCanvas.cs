using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.OpenGL.Fonts;
using SimulationFramework.OpenGL.Geometry;
using SimulationFramework.OpenGL.Geometry.Streams;
using SimulationFramework.OpenGL.Geometry.Writers;
using SimulationFramework.OpenGL.Shaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class GLCanvas : ICanvas, IDisposable
{
    public ITexture Target { get; }
    public ref readonly CanvasState State => ref currentState;
    public CanvasState currentState;

    private Stack<CanvasState> stateStack = [];
    private List<RenderCommand> commandList = [];

    GeometryBufferWriter vertexWriter;

    private GeometryStream CurrentStream => graphics.streams.GetStream(in State);
    private GeometryWriter CurrentWriter => graphics.writers.GetWriter(in State);

    // TODO: multiple frames in flight


    private GLGraphics graphics;
    public readonly uint fbo;

    // use one shared vao. Since we pool buffers, there's no way to know ahead
    // of time what buffer & vertex layout combination we'll be using when
    // rendering, so vao's storing this state only causes problems.
    // TODO: use the seperate vertex layout functions *if they are available*
    private readonly uint vao;

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

        fixed (uint* vaoPtr = &vao)
        {
            glGenVertexArrays(1, vaoPtr);
        }

        currentState = new();


        vertexWriter = new(graphics.bufferPool);
    }

    public void Antialias(bool antialias)
    {
        // if (antialias) 
        // {
        //     glEnable(GL_MULTI GL_MULTISAMPLE);
        // }
        // else
        // {
        //     glDisable(GL_MULTISAMPLE);
        // }
    }

    public void Clear(ColorF color)
    {
        SubmitCommands();
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
        CurrentWriter.PushArc(CurrentStream, bounds, begin, end, includeCenter);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
    }

    public void DrawLine(Vector2 p1, Vector2 p2)
    {
        graphics.writers.GetWriter(in State, true).PushLine(CurrentStream, p1, p2);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, false), effect);
    }

    public void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true)
    {
        if (polygon.Length < 2)
            return;

        CurrentWriter.PushPolygon(CurrentStream, polygon, close);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
    }

    public void DrawRect(Rectangle rect)
    {
        CurrentWriter.PushRect(CurrentStream, rect);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
    }

    private GeometryChunk CreateChunk(GeometryStream stream, bool triangles)
    {
        int vertexSize = stream.VertexLayout.VertexSize;
        var buffer = vertexWriter.Write(stream.GetData(), vertexSize, out int offsetInBytes, out int countInBytes);
        stream.Clear();
        return GeometryChunk.Create(buffer, stream.VertexLayout, offsetInBytes / vertexSize, countInBytes / vertexSize, triangles);
    }

    //private void SubmitStream(GeometryStream stream, GeometryEffect effect, bool triangles, ReadOnlySpan<byte> bytes = default)
    //{
    //    int vertexSize = stream.GetVertexSize();
    //    if (bytes.IsEmpty)
    //    {
    //        bytes = stream.GetData();
    //    }

    //    int offset, count;

    //    var buffer = vertexWriter.Write(bytes, vertexSize, out offset, out count);

    //    StreamRenderCommand cmd = new(effect, currentState)
    //    {
    //        buffer = buffer,
    //        stream = stream,
    //    };

    //    cmd.AddCommand(triangles, offset / vertexSize, count / vertexSize);
    //    stream.Clear();

    //    if (effect is ProgrammableShaderEffect)
    //    {
    //        Flush();
    //    }
    //}

    public void DrawEllipse(Rectangle bounds)
    {
        CurrentWriter.PushEllipse(CurrentStream, bounds);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
    }

    public void DrawRoundedRect(Rectangle rect, Vector2 radii)
    {
        CurrentWriter.PushRoundedRect(CurrentStream, rect, radii);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
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
        var stream = graphics.streams.GetTextureGeometryStream();
        stream.TransformMatrix = State.Transform;

        Vector2 uvScale = new(1f / texture.Width, 1f / texture.Height);
        stream.WriteVertex(new(destination.X, destination.Y), uvScale * new Vector2(source.X, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        stream.WriteVertex(new(destination.X + destination.Width, destination.Y), uvScale * new Vector2(source.X + source.Width, source.Y));
        stream.WriteVertex(new(destination.X + destination.Width, destination.Y + destination.Height), uvScale * new Vector2(source.X + source.Width, source.Y + source.Height));
        stream.WriteVertex(new(destination.X, destination.Y + destination.Height), uvScale * new Vector2(source.X, source.Y + source.Height));

        var glTexture = (GLTexture)texture;
        glTexture.PrepareForRender();
        var effect = new TextureGeometryEffect(glTexture, tint, graphics.effects.textureProgram);
        AddRenderCommand(CreateChunk(stream, true), effect);
    }

    public void DrawTriangles(ReadOnlySpan<Vector2> triangles)
    {
        CurrentWriter.PushTriangles(CurrentStream, triangles);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(CreateChunk(CurrentStream, CurrentWriter.UsesTriangles), effect);
    }

    public void DrawTriangles<TVertex>(ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged
    {
        if (State.VertexShader is null)
        {
            throw new InvalidOperationException("Must have a vertex shader to accept custom vertices!");
        }
        var stream = graphics.streams.GetCustomVertexGeometryStream(typeof(TVertex), in State);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);

        if (State.Fill)
        {
            int vertexSize = Unsafe.SizeOf<TVertex>();
            var vertexBuffer = vertexWriter.Write(MemoryMarshal.AsBytes(vertices), vertexSize, out int offsetBytes, out int countBytes);
            var chunk = GeometryChunk.Create(vertexBuffer, stream.VertexLayout, offsetBytes / vertexSize, countBytes / vertexSize, true);
            AddRenderCommand(chunk, effect);
        }
        else
        {
            int vertexSize = Unsafe.SizeOf<TVertex>();
            var buffer = this.graphics.bufferPool.GetLargeBuffer(vertexSize * vertices.Length * 2);
            TVertex[] wireframeVerts = new TVertex[vertices.Length * 2];
            for (int i = 0; i < vertices.Length / 3; i++)
            {
                TVertex a = vertices[i * 3 + 0];
                TVertex b = vertices[i * 3 + 1];
                TVertex c = vertices[i * 3 + 2];

                wireframeVerts[(i * 3 + 0) * 2 + 0] = a;
                wireframeVerts[(i * 3 + 0) * 2 + 1] = b;
                wireframeVerts[(i * 3 + 1) * 2 + 0] = b;
                wireframeVerts[(i * 3 + 1) * 2 + 1] = c;
                wireframeVerts[(i * 3 + 2) * 2 + 0] = c;
                wireframeVerts[(i * 3 + 2) * 2 + 1] = a;
            }
            buffer.Upload<TVertex>(wireframeVerts);
            var chunk = GeometryChunk.Create(buffer, stream.VertexLayout, 0, wireframeVerts.Length, false);
            AddRenderCommand(chunk, effect);
        }
    }

    public void DrawTriangles<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices)
       where TVertex : unmanaged
    {
        if (State.VertexShader is null)
        {
            throw new InvalidOperationException("Must have a vertex shader to accept custom vertices!");
        }

        var indexBuffer = vertexWriter.Write(MemoryMarshal.AsBytes(indices), sizeof(uint), out int offsetBytes, out int countBytes);
        var vertexBuffer = graphics.bufferPool.Rent(vertices.Length * Unsafe.SizeOf<TVertex>());
        vertexBuffer.Upload(vertices);

        var chunk = GeometryChunk.CreateIndexed(vertexBuffer, indexBuffer, VertexLayout.Get(typeof(TVertex)), offsetBytes / sizeof(uint), countBytes / sizeof(uint), true);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(chunk, effect);
    }
    
    public void DrawInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<TInstance> instances)
       where TVertex : unmanaged
       where TInstance : unmanaged
    {
        if (State.VertexShader is null)
        {
            throw new InvalidOperationException("Must have a vertex shader to accept custom vertices!");
        }
        var stream = graphics.streams.GetCustomVertexGeometryStream(typeof(TVertex), in State);
        var effect = graphics.effects.GetEffectFromCanvasState(in State);

        int vertexSize = Unsafe.SizeOf<TVertex>();
        var vertexBuffer = vertexWriter.Write(MemoryMarshal.AsBytes(vertices), vertexSize, out int offsetBytes, out int countBytes);
        var chunk = GeometryChunk.Create(vertexBuffer, stream.VertexLayout, offsetBytes / vertexSize, countBytes / vertexSize, true);

        var instanceBuffer = graphics.bufferPool.Rent(instances.Length * Unsafe.SizeOf<TInstance>());
        instanceBuffer.Upload(instances);
        chunk.instanceBuffer = instanceBuffer;
        chunk.instanceCount = instances.Length;
        chunk.instanceLayout = VertexLayout.Get(typeof(TInstance));

        AddRenderCommand(chunk, effect);
    }

    public void DrawInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices, ReadOnlySpan<TInstance> instances)
       where TVertex : unmanaged
       where TInstance : unmanaged
    {
        if (State.VertexShader is null)
        {
            throw new InvalidOperationException("Must have a vertex shader to accept custom vertices!");
        }

        var indexBuffer = vertexWriter.Write(MemoryMarshal.AsBytes(indices), sizeof(uint), out int offsetBytes, out int countBytes);
        var vertexBuffer = graphics.bufferPool.Rent(vertices.Length * Unsafe.SizeOf<TVertex>());
        vertexBuffer.Upload(vertices);

        var chunk = GeometryChunk.CreateIndexed(vertexBuffer, indexBuffer, VertexLayout.Get(typeof(TVertex)), offsetBytes / sizeof(uint), countBytes / sizeof(uint), true);

        var instanceBuffer = graphics.bufferPool.Rent(instances.Length * Unsafe.SizeOf<TInstance>());
        instanceBuffer.Upload(instances);
        chunk.instanceBuffer = instanceBuffer;
        chunk.instanceCount = instances.Length;
        chunk.instanceLayout = VertexLayout.Get(typeof(TInstance));

        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(chunk, effect);
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
        ArgumentNullException.ThrowIfNull(shader);

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
        var stream = graphics.streams.GetTextureGeometryStream();
        stream.TransformMatrix = State.Transform;
        GLFont font = (GLFont)currentState.Font;
        var effect = new SDFFontEffect(currentState.Color, font.GetAtlas(style), graphics.effects.fontProgram);
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

        AddRenderCommand(CreateChunk(stream, true), effect);
        
        return baseline;
    }

    public void SubmitCommands()
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

        vertexWriter.UploadCurrentBuffer();

        glBindVertexArray(vao); 
        glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
        glEnable(GL_BLEND);
        glDisable(GL_CULL_FACE);
        glViewport(0, 0, Target.Width, Target.Height);
        glBindFramebuffer(GL_FRAMEBUFFER, this.fbo);

        for (int i = 0; i < commandList.Count; i++)
        {
            commandList[i].Execute(this, transform4x4);
        }
        commandList.Clear();

        graphics.bufferPool.Reset();
        vertexWriter.Reset();
    }

    public unsafe void Flush()
    {
        SubmitCommands();
        glFlush();
    }

    public void DrawGeometry(IGeometry geometry)
    {
        ArgumentNullException.ThrowIfNull(geometry);

        var glGeometry = (GLGeometry)geometry;
        var effect = graphics.effects.GetEffectFromCanvasState(in State);
        AddRenderCommand(glGeometry.chunk with { wireframe = !State.Fill }, effect);
    }

    public void DrawInstancedGeometry(IGeometry geometry, ReadOnlySpan<Matrix3x2> instances)
    {
        var instanceBuffer = graphics.bufferPool.Rent(Unsafe.SizeOf<Matrix3x2>() * instances.Length);
        instanceBuffer.Upload(instances);

        var glGeometry = (GLGeometry)geometry;

        AddRenderCommand(
            glGeometry.chunk with
            {
                instanceBuffer = instanceBuffer,
                instanceLayout = VertexLayout.Get(typeof(Matrix3x2)),
                instanceCount = instances.Length,
            },
            graphics.effects.GetEffectFromCanvasState(in State)
            );
    }

    public void DrawInstancedGeometry<TInstance>(IGeometry geometry, ReadOnlySpan<TInstance> instances)
        where TInstance : unmanaged
    {
        var instanceBuffer = graphics.bufferPool.Rent(Unsafe.SizeOf<TInstance>() * instances.Length);
        instanceBuffer.Upload(instances);

        var glGeometry = (GLGeometry)geometry;
        
        AddRenderCommand(
            glGeometry.chunk with
            {
                instanceBuffer = instanceBuffer,
                instanceLayout = VertexLayout.Get(typeof(TInstance)),
                instanceCount = instances.Length,
            },
            graphics.effects.GetEffectFromCanvasState(in State)
            );
    }

    public void AddRenderCommand(GeometryChunk chunk, GeometryEffect effect)
    {
        Debug.Assert(chunk.count != 0);

        RenderCommand command = new()
        {
            chunk = chunk,
            effect = effect,
            clipRect = currentState.ClipRectangle,
            readMask = currentState.Mask,
            writeMask = currentState.WriteMask,
            writeMaskValue = currentState.WriteMaskValue
        };

        if (commandList.Count > 0)
        {
            RenderCommand lastCommand = commandList[^1];
            if (command.HasSameState(lastCommand))
            {
                GeometryChunk lastChunk = lastCommand.chunk;
                if (chunk.vertexBuffer == lastChunk.vertexBuffer && chunk.triangles == lastChunk.triangles)
                {
                    if (lastChunk.offset + lastChunk.count == chunk.offset)
                    {
                        lastChunk.count += chunk.count;
                        commandList[^1] = commandList[^1] with { chunk = lastChunk };
                        return;
                    }
                }
            }
        }
        
        commandList.Add(command);

        // TODO: implement proper uniform handling
        if (effect is ProgrammableShaderEffect)
        {
            SubmitCommands();
        }
    }


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
        var writer = graphics.writers.GetWriter(in State, true);
        var stream = graphics.streams.GetStream(in State);
        for (int i = 0; i < vertices.Length; i += 2)
        {
            writer.PushLine(stream, vertices[i], vertices[i + 1]);
        }
        AddRenderCommand(CreateChunk(stream, false), graphics.effects.GetEffectFromCanvasState(in State));
    }

    public void DrawLines<TVertex>(ReadOnlySpan<TVertex> vertices) where TVertex : unmanaged
    {
        var stream = graphics.streams.GetCustomVertexGeometryStream(typeof(TVertex), in State);
        for (int i = 0; i < vertices.Length; i++)
        {
            stream.WriteVertex(vertices[i]);
        }
        AddRenderCommand(CreateChunk(stream, false), graphics.effects.GetEffectFromCanvasState(in State));
    }

    public void Mask(IMask? mask)
    {
        currentState.Mask = mask;
    }

    public void WriteMask(IMask? mask, bool? value)
    {
        currentState.WriteMask = mask;
        currentState.WriteMaskValue = value;
    }

    public unsafe void Dispose()
    {
        fixed (uint* fboPtr = &fbo) 
        {
            glDeleteFramebuffers(1, fboPtr);
        }

        fixed (uint* vaoPtr = &vao)
        {
            glDeleteVertexArrays(1, vaoPtr);
        }
    }
}