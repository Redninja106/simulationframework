using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Messaging;
using SimulationFramework.OpenGL.Fonts;
using SimulationFramework.OpenGL.Geometry;
using SimulationFramework.OpenGL.Geometry.Streams;
using SimulationFramework.OpenGL.Geometry.Writers;
using SimulationFramework.OpenGL.Shaders;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

public unsafe class GLGraphics : IGraphicsProvider
{
    public IFont DefaultFont { get; }

    private GLCanvas frameCanvas;
    private GLFrame frame;

    private readonly Dictionary<Type, ComputeShaderEffect> computeShaders = [];
    private readonly Dictionary<(Type, Type?), ProgrammableShaderProgram> shaderPrograms = [];
    private readonly Dictionary<string, GLFont> systemFontCache = [];
    internal ShaderCompiler ShaderCompiler = new();

    internal GeometryStreamCollection streams;
    internal GeometryBufferPool bufferPool;
    internal GeometryEffectCollection effects;
    internal GeometryWriterCollection writers;

    public GLGraphics(GLFrame frame, Func<string, nint> getProcAddress)
    {
        this.frame = frame;
        global::OpenGL.Initialize(name => (delegate*<void>)getProcAddress(name));

        glEnable(GL_DEBUG_OUTPUT);
        glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);
        glDebugMessageCallback(&DebugCallback, null);

        DefaultFont = LoadSystemFont("Verdana");

        bufferPool = new();

        streams = new(this);
        effects = new(this);
        writers = new();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static void DebugCallback(uint source, uint type, uint id, uint severity, long length, byte* message, void* userParam)
    {
        if (severity == GL_DEBUG_SEVERITY_NOTIFICATION)
            return;

        var str = Marshal.PtrToStringUTF8((nint)message, (int)length);
        Console.WriteLine(str);
    }

    public void Dispose()
    {
    }

    public ICanvas GetWindowCanvas()
    {
        return frameCanvas;
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        frameCanvas = new GLCanvas(this, frame);
    }

    public IFont LoadFont(ReadOnlySpan<byte> encodedData)
    {
        return new GLFont(this, encodedData);
    }

    public IFont LoadSystemFont(string name)
    {
        if (!systemFontCache.TryGetValue(name, out var font))
        {
            font = (GLFont)CreateSystemFont(name);
            systemFontCache.Add(name, font);
        }
     
        return font;
    }

    private IFont CreateSystemFont(string name)
    {
        if (OperatingSystem.IsWindows())
        {
            string path = Path.Combine(Environment.SystemDirectory, "../fonts", name + ".ttf");
            return LoadFont(File.ReadAllBytes(Path.GetFullPath(path)));
        }
        else if (OperatingSystem.IsLinux())
        {
            string path = Path.Combine("/usr/share/fonts", name + ".ttf");
            return LoadFont(File.ReadAllBytes(Path.GetFullPath(path)));
        }
        else if (OperatingSystem.IsMacOS())
        {
            string path = Path.Combine("/Library/Fonts/", name + ".ttf");
            return LoadFont(File.ReadAllBytes(Path.GetFullPath(path)));
        }
        else
        {
            throw new NotSupportedException("System fonts not supported on this platform!");
        }
    }

    public ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options)
    {
        return new GLTexture(this, width, height, pixels, options);
    }

    public ITexture LoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options)
    {
        fixed (byte* dataPtr = encodedData) 
        {
            using var stream = new UnmanagedMemoryStream(dataPtr, encodedData.Length);
            ImageResult result = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            ReadOnlySpan<Color> colors = MemoryMarshal.Cast<byte, Color>(result.Data);
            return CreateTexture(result.Width, result.Height, colors, options);
        }
    }

    internal ProgrammableShaderProgram GetShaderProgram(CanvasShader shader, VertexShader? vs = null)
    {
        if (shaderPrograms.TryGetValue((shader.GetType(), vs?.GetType()), out var program))
        {
            return program;
        }

        var compilation = ShaderCompiler.Compile(shader);

        ShaderCompilation vsCompilation = null;
        if (vs != null)
        {
            vsCompilation = ShaderCompiler.Compile(vs);
        }

        var sw = new StringWriter();
        var emitter = new GLSLShaderEmitter(sw);
        emitter.Emit(compilation);
        var src = sw.ToString();

        string? vsSrc = null;
        if (vsCompilation != null)
        {
            var vsSw = new StringWriter();
            var vsEmitter = new GLSLShaderEmitter(vsSw);
            vsEmitter.Emit(vsCompilation);
            vsSrc = vsSw.ToString();
        }

        program = new(compilation, src, vsCompilation, vsSrc);
        shaderPrograms[(shader.GetType(), vs?.GetType())] = program;
        return program;
    }

    internal void RemoveCachedShader(Type type)
    {
        foreach (var (effect, _) in shaderPrograms)
        {
            if (effect.Item1 == type || effect.Item2 == type || effect.Item1.IsSubclassOf(type) || (effect.Item2?.IsSubclassOf(type) ?? false))
            {
                shaderPrograms.Remove(effect);
            }
        }
    }

    public void Dispatch(ComputeShader computeShader, int threadCountI, int threadCountJ, int threadCountK)
    {
        if (!computeShaders.TryGetValue(computeShader.GetType(), out var glShader))
        {
            var compilation = ShaderCompiler.Compile(computeShader);
            glShader = new ComputeShaderEffect(computeShader.GetType(), compilation);
            computeShaders[computeShader.GetType()] = glShader;
        }

        glShader.Dispatch(computeShader, threadCountI, threadCountJ, threadCountK);
    }

    public IMask CreateMask(int width, int height)
    {
        return new GLMask(width, height);
    }

    public IDepthMask CreateDepthMask(int width, int height)
    {
        return new GLDepthMask(width, height);
    }

    public IGeometry CreateGeometry<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices) where TVertex : unmanaged
    {
        if (indices.IsEmpty)
        {
            return GLGeometry.Create(this, vertices);
        }
        else
        {
            return GLGeometry.CreateIndexed(this, vertices, indices);
        }
    }
}