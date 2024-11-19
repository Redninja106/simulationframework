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
using SimulationFramework.OpenGL.Shaders.Compute;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimulationFramework.OpenGL;

public unsafe class GLGraphics : IGraphicsProvider
{
    public IFont DefaultFont { get; }

    private GLCanvas frameCanvas;
    private GLFrame frame;

    private readonly Dictionary<Type, ComputeShaderProgram> computeShaders = [];
    private readonly Dictionary<(Type, Type?), ProgrammableShaderProgram> shaderPrograms = [];
    private readonly Dictionary<string, GLFont> systemFontCache = [];
    internal ShaderCompiler ShaderCompiler = new();

    internal ShaderArrayManager arrayManager;

    internal GeometryStreamCollection streams;
    internal GeometryBufferPool bufferPool;
    internal GeometryEffectCollection effects;
    internal GeometryWriterCollection writers;
    internal int MaxTextureSize;

    private readonly string shaderVersion;

    public readonly bool HasGLES31 = false;

    public static bool ForceCompatability { get; set; } = false;

    public GLGraphics(GLFrame frame, string shaderVersion, Func<string, nint> getProcAddress)
    {
        this.shaderVersion = shaderVersion;
        this.frame = frame;
        Khronos.OpenGL.glInitialize(name => getProcAddress(name));

#if DEBUG
        glEnable(GL_DEBUG_OUTPUT);
        glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);
        glDebugMessageCallback(&DebugCallback, null);
#endif
        
        if (!ForceCompatability)
        {
            int major, minor;
            glGetIntegerv(GL_MAJOR_VERSION, &major);
            glGetIntegerv(GL_MINOR_VERSION, &minor);

            if (major > 3 || (major == 3 && minor >= 1))
            {
                HasGLES31 = true;
            }
        }

        fixed (int* maxTextureSizePtr = &MaxTextureSize)
        {
            glGetIntegerv(GL_MAX_TEXTURE_SIZE, maxTextureSizePtr);
        }
        Debug.Assert(MaxTextureSize != 0);

        var defaultFontStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("SimulationFramework.OpenGL.MartianMono.ttf");
        byte[] defaultFontData = new byte[defaultFontStream.Length];
        defaultFontStream.Read(defaultFontData);
        DefaultFont = LoadFont(defaultFontData);

        bufferPool = new();

        streams = new(this);
        effects = new(this, shaderVersion);
        writers = new();

        this.arrayManager = new(this);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static void DebugCallback(uint source, uint type, uint id, uint severity, int length, byte* message, void* userParam)
    {
        if (severity == GL_DEBUG_SEVERITY_NOTIFICATION)
            return;

        var str = Marshal.PtrToStringUTF8((nint)message, (int)length);
        Console.WriteLine(str);
    }

    public void GenerateMipmaps(ITexture texture)
    {
        var glTexture = (GLTexture)texture;
        glBindTexture(GL_TEXTURE_2D, glTexture.GetID());
        glGenerateMipmap(GL_TEXTURE_2D);
        glTexture.hasMipmaps = true;
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
        else if (OperatingSystem.IsAndroid())
        {
            string path = Path.Combine("/System/Fonts/", name + ".ttf");
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

        program = new(this.shaderVersion, compilation, src, vsCompilation, vsSrc);
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

    public void Dispatch(ComputeShader computeShader, int threadCountX, int threadCountY, int threadCountZ)
    {
        if (!computeShaders.TryGetValue(computeShader.GetType(), out var glShader))
        {
            var compilation = ShaderCompiler.Compile(computeShader);

            ThreadGroupSizeAttribute? groupSize = compilation.EntryPoint.BackingMethod?.GetCustomAttribute<ThreadGroupSizeAttribute>();
            if (groupSize != null)
            {
                if (groupSize.CountX < 1 || groupSize.CountY < 1 || groupSize.CountZ < 1)
                {
                    throw new Exception("thread count must be greater than or equal to 1 in all dimensions!");
                }

                glShader = new ExplicitGroupSizeComputeShaderProgram(shaderVersion, compilation, groupSize);
            }
            else
            {
                glShader = new AutoGroupSizeComputeShaderProgram(shaderVersion, compilation);
            }

            computeShaders[computeShader.GetType()] = glShader;
        }

        glShader.GetEffect(computeShader, threadCountX, threadCountY, threadCountZ).Dispatch(computeShader, threadCountX, threadCountY, threadCountZ);
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

    public void UploadArray(Array array)
    {
        ShaderArray shaderArray = arrayManager.Get(array);
        arrayManager.UploadArray(array);
        shaderArray.Synchronized = false;
    }

    public void SyncArray(Array array)
    {
        ShaderArray shaderArray = arrayManager.Get(array);
        arrayManager.SyncArray(array);
        shaderArray.Synchronized = true;
    }
}