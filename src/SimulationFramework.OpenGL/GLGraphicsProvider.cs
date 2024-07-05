using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.Messaging;
using StbImageSharp;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

public unsafe class GLGraphicsProvider : IGraphicsProvider
{
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

    public bool TryLoadFont(ReadOnlySpan<byte> encodedData, [NotNullWhen(true)] out IFont? font)
    {
        throw new NotImplementedException();
    }

    public bool TryLoadSystemFont(string name, [NotNullWhen(true)] out IFont? font)
    {
        throw new NotImplementedException();
    }

    public ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options)
    {
        return new GLTexture(width, height, pixels, options);
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
}
