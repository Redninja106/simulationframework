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
    public static bool DumpShaders { get; set; }

    public IFont DefaultFont { get; }

    private GLCanvas frameCanvas;
    private GLFrame frame;

    private Dictionary<Type, ShaderGeometryEffect> shaderEffects = [];
    private CanvasShaderCompiler canvasShaderCompiler = new();

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

    internal ShaderGeometryEffect GetShaderEffect(CanvasShader shader)
    {
        if (shaderEffects.TryGetValue(shader.GetType(), out var effect))
        {
            return effect;
        }

        var compilation = canvasShaderCompiler.Compile(shader);

        var sw = new StringWriter();
        var emitter = new GLSLCanvasShaderEmitter(sw);
        emitter.Emit(compilation);

        var src = sw.ToString();
        effect = new(compilation, src);
        shaderEffects[shader.GetType()] = effect;
        return effect;
    }

    internal void RemoveCachedShader(Type type)
    {
        this.shaderEffects.Remove(type);
    }
}
