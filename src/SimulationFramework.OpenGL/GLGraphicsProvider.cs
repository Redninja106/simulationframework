using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Messaging;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

public unsafe class GLGraphicsProvider : IGraphicsProvider
{
    public IFont DefaultFont { get; }

    private GLCanvas frameCanvas;
    private GLFrame frame;

    private readonly Dictionary<Type, ComputeShaderEffect> computeShaders = [];
    private readonly Dictionary<(Type, Type?), ShaderGeometryEffect> shaderEffects = [];
    private readonly Dictionary<string, GLFont> systemFontCache = [];
    internal ShaderCompiler ShaderCompiler = new();

    public GLGraphicsProvider(GLFrame frame, Func<string, nint> getProcAddress)
    {
        this.frame = frame;
        global::OpenGL.Initialize(name => (delegate*<void>)getProcAddress(name));

        glDebugMessageCallback(&DebugCallback, null);

        DefaultFont = LoadSystemFont("Verdana");
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

    public ICanvas GetFrameCanvas()
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

    internal ShaderGeometryEffect GetShaderEffect(CanvasShader shader, VertexShader? vs = null)
    {
        if (shaderEffects.TryGetValue((shader.GetType(), vs?.GetType()), out var effect))
        {
            return effect;
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

        effect = new(compilation, src, vsCompilation, vsSrc);
        shaderEffects[(shader.GetType(), vs?.GetType())] = effect;
        return effect;
    }

    internal void RemoveCachedShader(Type type)
    {
        foreach (var (effect, _) in shaderEffects)
        {
            if (effect.Item1 == type || effect.Item2 == type || effect.Item1.IsSubclassOf(type) || (effect.Item2?.IsSubclassOf(type) ?? false))
            {
                shaderEffects.Remove(effect);
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

    // public IGeometry CreateGeometry<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices) where TVertex : unmanaged
    // {
    //     var result = new GLGeometry();
    //     result.AddVertexBuffer(vertices);
    //     return result;
    // }
}