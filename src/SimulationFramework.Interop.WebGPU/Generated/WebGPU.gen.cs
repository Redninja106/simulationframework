using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace WebGPU;
public enum AdapterType
{
    DiscreteGPU = 0x00000000,
    
    IntegratedGPU = 0x00000001,
    
    CPU = 0x00000002,
    
    Unknown = 0x00000003,
}

public enum AddressMode
{
    Repeat = 0x00000000,
    
    MirrorRepeat = 0x00000001,
    
    ClampToEdge = 0x00000002,
}

public enum BackendType
{
    Undefined = 0x00000000,
    
    Null = 0x00000001,
    
    WebGPU = 0x00000002,
    
    D3D11 = 0x00000003,
    
    D3D12 = 0x00000004,
    
    Metal = 0x00000005,
    
    Vulkan = 0x00000006,
    
    OpenGL = 0x00000007,
    
    OpenGLES = 0x00000008,
}

public enum BlendFactor
{
    Zero = 0x00000000,
    
    One = 0x00000001,
    
    Src = 0x00000002,
    
    OneMinusSrc = 0x00000003,
    
    SrcAlpha = 0x00000004,
    
    OneMinusSrcAlpha = 0x00000005,
    
    Dst = 0x00000006,
    
    OneMinusDst = 0x00000007,
    
    DstAlpha = 0x00000008,
    
    OneMinusDstAlpha = 0x00000009,
    
    SrcAlphaSaturated = 0x0000000A,
    
    Constant = 0x0000000B,
    
    OneMinusConstant = 0x0000000C,
}

public enum BlendOperation
{
    Add = 0x00000000,
    
    Subtract = 0x00000001,
    
    ReverseSubtract = 0x00000002,
    
    Min = 0x00000003,
    
    Max = 0x00000004,
}

public enum BufferBindingType
{
    Undefined = 0x00000000,
    
    Uniform = 0x00000001,
    
    Storage = 0x00000002,
    
    ReadOnlyStorage = 0x00000003,
}

public enum BufferMapAsyncStatus
{
    Success = 0x00000000,
    
    ValidationError = 0x00000001,
    
    Unknown = 0x00000002,
    
    DeviceLost = 0x00000003,
    
    DestroyedBeforeCallback = 0x00000004,
    
    UnmappedBeforeCallback = 0x00000005,
    
    MappingAlreadyPending = 0x00000006,
    
    OffsetOutOfRange = 0x00000007,
    
    SizeOutOfRange = 0x00000008,
}

public enum BufferMapState
{
    Unmapped = 0x00000000,
    
    Pending = 0x00000001,
    
    Mapped = 0x00000002,
}

public enum CompareFunction
{
    Undefined = 0x00000000,
    
    Never = 0x00000001,
    
    Less = 0x00000002,
    
    LessEqual = 0x00000003,
    
    Greater = 0x00000004,
    
    GreaterEqual = 0x00000005,
    
    Equal = 0x00000006,
    
    NotEqual = 0x00000007,
    
    Always = 0x00000008,
}

public enum CompilationInfoRequestStatus
{
    Success = 0x00000000,
    
    Error = 0x00000001,
    
    DeviceLost = 0x00000002,
    
    Unknown = 0x00000003,
}

public enum CompilationMessageType
{
    Error = 0x00000000,
    
    Warning = 0x00000001,
    
    Info = 0x00000002,
}

public enum CompositeAlphaMode
{
    Auto = 0x00000000,
    
    Opaque = 0x00000001,
    
    Premultiplied = 0x00000002,
    
    Unpremultiplied = 0x00000003,
    
    Inherit = 0x00000004,
}

public enum CreatePipelineAsyncStatus
{
    Success = 0x00000000,
    
    ValidationError = 0x00000001,
    
    InternalError = 0x00000002,
    
    DeviceLost = 0x00000003,
    
    DeviceDestroyed = 0x00000004,
    
    Unknown = 0x00000005,
}

public enum CullMode
{
    None = 0x00000000,
    
    Front = 0x00000001,
    
    Back = 0x00000002,
}

public enum DeviceLostReason
{
    Undefined = 0x00000000,
    
    Destroyed = 0x00000001,
}

public enum ErrorFilter
{
    Validation = 0x00000000,
    
    OutOfMemory = 0x00000001,
    
    Internal = 0x00000002,
}

public enum ErrorType
{
    NoError = 0x00000000,
    
    Validation = 0x00000001,
    
    OutOfMemory = 0x00000002,
    
    Internal = 0x00000003,
    
    Unknown = 0x00000004,
    
    DeviceLost = 0x00000005,
}

public enum FeatureName
{
    Undefined = 0x00000000,
    
    DepthClipControl = 0x00000001,
    
    Depth32FloatStencil8 = 0x00000002,
    
    TimestampQuery = 0x00000003,
    
    TextureCompressionBC = 0x00000004,
    
    TextureCompressionETC2 = 0x00000005,
    
    TextureCompressionASTC = 0x00000006,
    
    IndirectFirstInstance = 0x00000007,
    
    ShaderF16 = 0x00000008,
    
    RG11B10UfloatRenderable = 0x00000009,
    
    BGRA8UnormStorage = 0x0000000A,
    
    Float32Filterable = 0x0000000B,
}

public enum FilterMode
{
    Nearest = 0x00000000,
    
    Linear = 0x00000001,
}

public enum FrontFace
{
    CCW = 0x00000000,
    
    CW = 0x00000001,
}

public enum IndexFormat
{
    Undefined = 0x00000000,
    
    Uint16 = 0x00000001,
    
    Uint32 = 0x00000002,
}

public enum LoadOp
{
    Undefined = 0x00000000,
    
    Clear = 0x00000001,
    
    Load = 0x00000002,
}

public enum MipmapFilterMode
{
    Nearest = 0x00000000,
    
    Linear = 0x00000001,
}

public enum PowerPreference
{
    Undefined = 0x00000000,
    
    LowPower = 0x00000001,
    
    HighPerformance = 0x00000002,
}

public enum PresentMode
{
    Fifo = 0x00000000,
    
    FifoRelaxed = 0x00000001,
    
    Immediate = 0x00000002,
    
    Mailbox = 0x00000003,
}

public enum PrimitiveTopology
{
    PointList = 0x00000000,
    
    LineList = 0x00000001,
    
    LineStrip = 0x00000002,
    
    TriangleList = 0x00000003,
    
    TriangleStrip = 0x00000004,
}

public enum QueryType
{
    Occlusion = 0x00000000,
    
    Timestamp = 0x00000001,
}

public enum QueueWorkDoneStatus
{
    Success = 0x00000000,
    
    Error = 0x00000001,
    
    Unknown = 0x00000002,
    
    DeviceLost = 0x00000003,
}

public enum RequestAdapterStatus
{
    Success = 0x00000000,
    
    Unavailable = 0x00000001,
    
    Error = 0x00000002,
    
    Unknown = 0x00000003,
}

public enum RequestDeviceStatus
{
    Success = 0x00000000,
    
    Error = 0x00000001,
    
    Unknown = 0x00000002,
}

public enum SType
{
    Invalid = 0x00000000,
    
    SurfaceDescriptorFromMetalLayer = 0x00000001,
    
    SurfaceDescriptorFromWindowsHWND = 0x00000002,
    
    SurfaceDescriptorFromXlibWindow = 0x00000003,
    
    SurfaceDescriptorFromCanvasHTMLSelector = 0x00000004,
    
    ShaderModuleSPIRVDescriptor = 0x00000005,
    
    ShaderModuleWGSLDescriptor = 0x00000006,
    
    PrimitiveDepthClipControl = 0x00000007,
    
    SurfaceDescriptorFromWaylandSurface = 0x00000008,
    
    SurfaceDescriptorFromAndroidNativeWindow = 0x00000009,
    
    SurfaceDescriptorFromXcbWindow = 0x0000000A,
    
    RenderPassDescriptorMaxDrawCount = 0x0000000F,
}

public enum SamplerBindingType
{
    Undefined = 0x00000000,
    
    Filtering = 0x00000001,
    
    NonFiltering = 0x00000002,
    
    Comparison = 0x00000003,
}

public enum StencilOperation
{
    Keep = 0x00000000,
    
    Zero = 0x00000001,
    
    Replace = 0x00000002,
    
    Invert = 0x00000003,
    
    IncrementClamp = 0x00000004,
    
    DecrementClamp = 0x00000005,
    
    IncrementWrap = 0x00000006,
    
    DecrementWrap = 0x00000007,
}

public enum StorageTextureAccess
{
    Undefined = 0x00000000,
    
    WriteOnly = 0x00000001,
}

public enum StoreOp
{
    Undefined = 0x00000000,
    
    Store = 0x00000001,
    
    Discard = 0x00000002,
}

public enum SurfaceGetCurrentTextureStatus
{
    Success = 0x00000000,
    
    Timeout = 0x00000001,
    
    Outdated = 0x00000002,
    
    Lost = 0x00000003,
    
    OutOfMemory = 0x00000004,
    
    DeviceLost = 0x00000005,
}

public enum TextureAspect
{
    All = 0x00000000,
    
    StencilOnly = 0x00000001,
    
    DepthOnly = 0x00000002,
}

public enum TextureDimension
{
    _1D = 0x00000000,
    
    _2D = 0x00000001,
    
    _3D = 0x00000002,
}

public enum TextureFormat
{
    Undefined = 0x00000000,
    
    R8Unorm = 0x00000001,
    
    R8Snorm = 0x00000002,
    
    R8Uint = 0x00000003,
    
    R8Sint = 0x00000004,
    
    R16Uint = 0x00000005,
    
    R16Sint = 0x00000006,
    
    R16Float = 0x00000007,
    
    RG8Unorm = 0x00000008,
    
    RG8Snorm = 0x00000009,
    
    RG8Uint = 0x0000000A,
    
    RG8Sint = 0x0000000B,
    
    R32Float = 0x0000000C,
    
    R32Uint = 0x0000000D,
    
    R32Sint = 0x0000000E,
    
    RG16Uint = 0x0000000F,
    
    RG16Sint = 0x00000010,
    
    RG16Float = 0x00000011,
    
    RGBA8Unorm = 0x00000012,
    
    RGBA8UnormSrgb = 0x00000013,
    
    RGBA8Snorm = 0x00000014,
    
    RGBA8Uint = 0x00000015,
    
    RGBA8Sint = 0x00000016,
    
    BGRA8Unorm = 0x00000017,
    
    BGRA8UnormSrgb = 0x00000018,
    
    RGB10A2Unorm = 0x00000019,
    
    RG11B10Ufloat = 0x0000001A,
    
    RGB9E5Ufloat = 0x0000001B,
    
    RG32Float = 0x0000001C,
    
    RG32Uint = 0x0000001D,
    
    RG32Sint = 0x0000001E,
    
    RGBA16Uint = 0x0000001F,
    
    RGBA16Sint = 0x00000020,
    
    RGBA16Float = 0x00000021,
    
    RGBA32Float = 0x00000022,
    
    RGBA32Uint = 0x00000023,
    
    RGBA32Sint = 0x00000024,
    
    Stencil8 = 0x00000025,
    
    Depth16Unorm = 0x00000026,
    
    Depth24Plus = 0x00000027,
    
    Depth24PlusStencil8 = 0x00000028,
    
    Depth32Float = 0x00000029,
    
    Depth32FloatStencil8 = 0x0000002A,
    
    BC1RGBAUnorm = 0x0000002B,
    
    BC1RGBAUnormSrgb = 0x0000002C,
    
    BC2RGBAUnorm = 0x0000002D,
    
    BC2RGBAUnormSrgb = 0x0000002E,
    
    BC3RGBAUnorm = 0x0000002F,
    
    BC3RGBAUnormSrgb = 0x00000030,
    
    BC4RUnorm = 0x00000031,
    
    BC4RSnorm = 0x00000032,
    
    BC5RGUnorm = 0x00000033,
    
    BC5RGSnorm = 0x00000034,
    
    BC6HRGBUfloat = 0x00000035,
    
    BC6HRGBFloat = 0x00000036,
    
    BC7RGBAUnorm = 0x00000037,
    
    BC7RGBAUnormSrgb = 0x00000038,
    
    ETC2RGB8Unorm = 0x00000039,
    
    ETC2RGB8UnormSrgb = 0x0000003A,
    
    ETC2RGB8A1Unorm = 0x0000003B,
    
    ETC2RGB8A1UnormSrgb = 0x0000003C,
    
    ETC2RGBA8Unorm = 0x0000003D,
    
    ETC2RGBA8UnormSrgb = 0x0000003E,
    
    EACR11Unorm = 0x0000003F,
    
    EACR11Snorm = 0x00000040,
    
    EACRG11Unorm = 0x00000041,
    
    EACRG11Snorm = 0x00000042,
    
    ASTC4x4Unorm = 0x00000043,
    
    ASTC4x4UnormSrgb = 0x00000044,
    
    ASTC5x4Unorm = 0x00000045,
    
    ASTC5x4UnormSrgb = 0x00000046,
    
    ASTC5x5Unorm = 0x00000047,
    
    ASTC5x5UnormSrgb = 0x00000048,
    
    ASTC6x5Unorm = 0x00000049,
    
    ASTC6x5UnormSrgb = 0x0000004A,
    
    ASTC6x6Unorm = 0x0000004B,
    
    ASTC6x6UnormSrgb = 0x0000004C,
    
    ASTC8x5Unorm = 0x0000004D,
    
    ASTC8x5UnormSrgb = 0x0000004E,
    
    ASTC8x6Unorm = 0x0000004F,
    
    ASTC8x6UnormSrgb = 0x00000050,
    
    ASTC8x8Unorm = 0x00000051,
    
    ASTC8x8UnormSrgb = 0x00000052,
    
    ASTC10x5Unorm = 0x00000053,
    
    ASTC10x5UnormSrgb = 0x00000054,
    
    ASTC10x6Unorm = 0x00000055,
    
    ASTC10x6UnormSrgb = 0x00000056,
    
    ASTC10x8Unorm = 0x00000057,
    
    ASTC10x8UnormSrgb = 0x00000058,
    
    ASTC10x10Unorm = 0x00000059,
    
    ASTC10x10UnormSrgb = 0x0000005A,
    
    ASTC12x10Unorm = 0x0000005B,
    
    ASTC12x10UnormSrgb = 0x0000005C,
    
    ASTC12x12Unorm = 0x0000005D,
    
    ASTC12x12UnormSrgb = 0x0000005E,
}

public enum TextureSampleType
{
    Undefined = 0x00000000,
    
    Float = 0x00000001,
    
    UnfilterableFloat = 0x00000002,
    
    Depth = 0x00000003,
    
    Sint = 0x00000004,
    
    Uint = 0x00000005,
}

public enum TextureViewDimension
{
    Undefined = 0x00000000,
    
    _1D = 0x00000001,
    
    _2D = 0x00000002,
    
    _2DArray = 0x00000003,
    
    Cube = 0x00000004,
    
    CubeArray = 0x00000005,
    
    _3D = 0x00000006,
}

public enum VertexFormat
{
    Undefined = 0x00000000,
    
    Uint8x2 = 0x00000001,
    
    Uint8x4 = 0x00000002,
    
    Sint8x2 = 0x00000003,
    
    Sint8x4 = 0x00000004,
    
    Unorm8x2 = 0x00000005,
    
    Unorm8x4 = 0x00000006,
    
    Snorm8x2 = 0x00000007,
    
    Snorm8x4 = 0x00000008,
    
    Uint16x2 = 0x00000009,
    
    Uint16x4 = 0x0000000A,
    
    Sint16x2 = 0x0000000B,
    
    Sint16x4 = 0x0000000C,
    
    Unorm16x2 = 0x0000000D,
    
    Unorm16x4 = 0x0000000E,
    
    Snorm16x2 = 0x0000000F,
    
    Snorm16x4 = 0x00000010,
    
    Float16x2 = 0x00000011,
    
    Float16x4 = 0x00000012,
    
    Float32 = 0x00000013,
    
    Float32x2 = 0x00000014,
    
    Float32x3 = 0x00000015,
    
    Float32x4 = 0x00000016,
    
    Uint32 = 0x00000017,
    
    Uint32x2 = 0x00000018,
    
    Uint32x3 = 0x00000019,
    
    Uint32x4 = 0x0000001A,
    
    Sint32 = 0x0000001B,
    
    Sint32x2 = 0x0000001C,
    
    Sint32x3 = 0x0000001D,
    
    Sint32x4 = 0x0000001E,
}

public enum VertexStepMode
{
    Vertex = 0x00000000,
    
    Instance = 0x00000001,
    
    VertexBufferNotUsed = 0x00000002,
}

[Flags]
public enum BufferUsage
{
    None = 0x00000000,
    
    MapRead = 0x00000001,
    
    MapWrite = 0x00000002,
    
    CopySrc = 0x00000004,
    
    CopyDst = 0x00000008,
    
    Index = 0x00000010,
    
    Vertex = 0x00000020,
    
    Uniform = 0x00000040,
    
    Storage = 0x00000080,
    
    Indirect = 0x00000100,
    
    QueryResolve = 0x00000200,
}

[Flags]
public enum ColorWriteMask
{
    None = 0x00000000,
    
    Red = 0x00000001,
    
    Green = 0x00000002,
    
    Blue = 0x00000004,
    
    Alpha = 0x00000008,
    
    All = 0x0000000F,
}

[Flags]
public enum MapMode
{
    None = 0x00000000,
    
    Read = 0x00000001,
    
    Write = 0x00000002,
}

[Flags]
public enum ShaderStage
{
    None = 0x00000000,
    
    Vertex = 0x00000001,
    
    Fragment = 0x00000002,
    
    Compute = 0x00000004,
}

[Flags]
public enum TextureUsage
{
    None = 0x00000000,
    
    CopySrc = 0x00000001,
    
    CopyDst = 0x00000002,
    
    TextureBinding = 0x00000004,
    
    StorageBinding = 0x00000008,
    
    RenderAttachment = 0x00000010,
}

public delegate void BufferMapCallback(BufferMapAsyncStatus status);

public delegate void CompilationInfoCallback(CompilationInfoRequestStatus status, CompilationInfo? compilationInfo);

public delegate void CreateComputePipelineAsyncCallback(CreatePipelineAsyncStatus status, ComputePipeline pipeline, string message);

public delegate void CreateRenderPipelineAsyncCallback(CreatePipelineAsyncStatus status, RenderPipeline pipeline, string message);

public delegate void DeviceLostCallback(DeviceLostReason reason, string message);

public delegate void ErrorCallback(ErrorType type, string message);

public delegate void QueueWorkDoneCallback(QueueWorkDoneStatus status);

public delegate void RequestAdapterCallback(RequestAdapterStatus status, Adapter adapter, string message);

public delegate void RequestDeviceCallback(RequestDeviceStatus status, Device device, string message);

public delegate void ProcDeviceSetUncapturedErrorCallback(Device device, ErrorCallback callback);

public partial struct ChainedStructOut
{
    public IChainable? Next;
    
    public SType SType;
    
    public ChainedStructOut(SType sType, IChainable? next = null)
    {
        this.SType = sType;
        this.Next = next;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStructOut.Native* next;
        
        public SType sType;
    }
}

public partial struct ChainedStruct
{
    public IChainable? Next;
    
    public SType SType;
    
    public ChainedStruct(SType sType, IChainable? next = null)
    {
        this.SType = sType;
        this.Next = next;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* next;
        
        public SType sType;
    }
}

public partial struct RenderPipelineDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public PipelineLayout Layout;
    
    public VertexState Vertex;
    
    public PrimitiveState Primitive;
    
    public DepthStencilState? DepthStencil;
    
    public MultisampleState Multisample;
    
    public FragmentState? Fragment;
    
    public RenderPipelineDescriptor(string label, PipelineLayout layout, VertexState vertex, PrimitiveState primitive, DepthStencilState? depthStencil, MultisampleState multisample, FragmentState? fragment, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Layout = layout;
        this.Vertex = vertex;
        this.Primitive = primitive;
        this.DepthStencil = depthStencil;
        this.Multisample = multisample;
        this.Fragment = fragment;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public nint layout;
        
        public VertexState.Native vertex;
        
        public PrimitiveState.Native primitive;
        
        public DepthStencilState.Native* depthStencil;
        
        public MultisampleState.Native multisample;
        
        public FragmentState.Native* fragment;
    }
}

public partial struct FragmentState
{
    public IChainable? NextInChain;
    
    public ShaderModule Module;
    
    public string EntryPoint;
    
    public ConstantEntry[] Constants;
    
    public ColorTargetState[] Targets;
    
    public FragmentState(ShaderModule module, string entryPoint, ConstantEntry[] constants, ColorTargetState[] targets, IChainable? nextInChain = null)
    {
        this.Module = module;
        this.EntryPoint = entryPoint;
        this.Constants = constants;
        this.Targets = targets;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint module;
        
        public byte* entryPoint;
        
        public ulong constantCount;
        
        public ConstantEntry.Native* constants;
        
        public ulong targetCount;
        
        public ColorTargetState.Native* targets;
    }
}

public partial struct VertexState
{
    public IChainable? NextInChain;
    
    public ShaderModule Module;
    
    public string EntryPoint;
    
    public ConstantEntry[] Constants;
    
    public VertexBufferLayout[] Buffers;
    
    public VertexState(ShaderModule module, string entryPoint, ConstantEntry[] constants, VertexBufferLayout[] buffers, IChainable? nextInChain = null)
    {
        this.Module = module;
        this.EntryPoint = entryPoint;
        this.Constants = constants;
        this.Buffers = buffers;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint module;
        
        public byte* entryPoint;
        
        public ulong constantCount;
        
        public ConstantEntry.Native* constants;
        
        public ulong bufferCount;
        
        public VertexBufferLayout.Native* buffers;
    }
}

public partial struct RenderPassDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public RenderPassColorAttachment[] ColorAttachments;
    
    public RenderPassDepthStencilAttachment? DepthStencilAttachment;
    
    public QuerySet OcclusionQuerySet;
    
    public RenderPassTimestampWrites? TimestampWrites;
    
    public RenderPassDescriptor(string label, RenderPassColorAttachment[] colorAttachments, RenderPassDepthStencilAttachment? depthStencilAttachment, QuerySet occlusionQuerySet, RenderPassTimestampWrites? timestampWrites, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.ColorAttachments = colorAttachments;
        this.DepthStencilAttachment = depthStencilAttachment;
        this.OcclusionQuerySet = occlusionQuerySet;
        this.TimestampWrites = timestampWrites;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong colorAttachmentCount;
        
        public RenderPassColorAttachment.Native* colorAttachments;
        
        public RenderPassDepthStencilAttachment.Native* depthStencilAttachment;
        
        public nint occlusionQuerySet;
        
        public RenderPassTimestampWrites.Native* timestampWrites;
    }
}

public partial struct DeviceDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public FeatureName[] RequiredFeatures;
    
    public RequiredLimits? RequiredLimits;
    
    public QueueDescriptor DefaultQueue;
    
    public DeviceLostCallback DeviceLostCallback;
    
    public DeviceDescriptor(string label, FeatureName[] requiredFeatures, RequiredLimits? requiredLimits, QueueDescriptor defaultQueue, DeviceLostCallback deviceLostCallback, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.RequiredFeatures = requiredFeatures;
        this.RequiredLimits = requiredLimits;
        this.DefaultQueue = defaultQueue;
        this.DeviceLostCallback = deviceLostCallback;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong requiredFeatureCount;
        
        public FeatureName* requiredFeatures;
        
        public RequiredLimits.Native* requiredLimits;
        
        public QueueDescriptor.Native defaultQueue;
        
        public delegate*unmanaged[Cdecl]<DeviceLostReason, byte*, nint, void> deviceLostCallback;
        
        public nint deviceLostUserdata;
    }
}

public partial struct ComputePipelineDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public PipelineLayout Layout;
    
    public ProgrammableStageDescriptor Compute;
    
    public ComputePipelineDescriptor(string label, PipelineLayout layout, ProgrammableStageDescriptor compute, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Layout = layout;
        this.Compute = compute;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public nint layout;
        
        public ProgrammableStageDescriptor.Native compute;
    }
}

public partial struct ColorTargetState
{
    public IChainable? NextInChain;
    
    public TextureFormat Format;
    
    public BlendState? Blend;
    
    public ColorWriteMask WriteMask;
    
    public ColorTargetState(TextureFormat format, BlendState? blend, ColorWriteMask writeMask, IChainable? nextInChain = null)
    {
        this.Format = format;
        this.Blend = blend;
        this.WriteMask = writeMask;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public TextureFormat format;
        
        public BlendState.Native* blend;
        
        public ColorWriteMask writeMask;
    }
}

public partial struct BindGroupLayoutDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public BindGroupLayoutEntry[] Entries;
    
    public BindGroupLayoutDescriptor(string label, BindGroupLayoutEntry[] entries, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Entries = entries;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong entryCount;
        
        public BindGroupLayoutEntry.Native* entries;
    }
}

public partial struct VertexBufferLayout
{
    public ulong ArrayStride;
    
    public VertexStepMode StepMode;
    
    public VertexAttribute[] Attributes;
    
    public VertexBufferLayout(ulong arrayStride, VertexStepMode stepMode, VertexAttribute[] attributes)
    {
        this.ArrayStride = arrayStride;
        this.StepMode = stepMode;
        this.Attributes = attributes;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ulong arrayStride;
        
        public VertexStepMode stepMode;
        
        public ulong attributeCount;
        
        public VertexAttribute.Native* attributes;
    }
}

public partial struct TextureDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public TextureUsage Usage;
    
    public TextureDimension Dimension;
    
    public Extent3D Size;
    
    public TextureFormat Format;
    
    public uint MipLevelCount;
    
    public uint SampleCount;
    
    public TextureFormat[] ViewFormats;
    
    public TextureDescriptor(string label, TextureUsage usage, TextureDimension dimension, Extent3D size, TextureFormat format, uint mipLevelCount, uint sampleCount, TextureFormat[] viewFormats, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Usage = usage;
        this.Dimension = dimension;
        this.Size = size;
        this.Format = format;
        this.MipLevelCount = mipLevelCount;
        this.SampleCount = sampleCount;
        this.ViewFormats = viewFormats;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public TextureUsage usage;
        
        public TextureDimension dimension;
        
        public Extent3D.Native size;
        
        public TextureFormat format;
        
        public uint mipLevelCount;
        
        public uint sampleCount;
        
        public ulong viewFormatCount;
        
        public TextureFormat* viewFormats;
    }
}

public partial struct SupportedLimits
{
    public IChainable? NextInChain;
    
    public Limits Limits;
    
    public SupportedLimits(Limits limits, IChainable? nextInChain = null)
    {
        this.Limits = limits;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStructOut.Native* nextInChain;
        
        public Limits.Native limits;
    }
}

public partial struct ShaderModuleDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public ShaderModuleCompilationHint[] Hints;
    
    public ShaderModuleDescriptor(string label, ShaderModuleCompilationHint[] hints, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Hints = hints;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong hintCount;
        
        public ShaderModuleCompilationHint.Native* hints;
    }
}

public partial struct RequiredLimits
{
    public IChainable? NextInChain;
    
    public Limits Limits;
    
    public RequiredLimits(Limits limits, IChainable? nextInChain = null)
    {
        this.Limits = limits;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public Limits.Native limits;
    }
}

public partial struct RenderPassColorAttachment
{
    public IChainable? NextInChain;
    
    public TextureView View;
    
    public TextureView ResolveTarget;
    
    public LoadOp LoadOp;
    
    public StoreOp StoreOp;
    
    public Color ClearValue;
    
    public RenderPassColorAttachment(TextureView view, TextureView resolveTarget, LoadOp loadOp, StoreOp storeOp, Color clearValue, IChainable? nextInChain = null)
    {
        this.View = view;
        this.ResolveTarget = resolveTarget;
        this.LoadOp = loadOp;
        this.StoreOp = storeOp;
        this.ClearValue = clearValue;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint view;
        
        public nint resolveTarget;
        
        public LoadOp loadOp;
        
        public StoreOp storeOp;
        
        public Color.Native clearValue;
    }
}

public partial struct ProgrammableStageDescriptor
{
    public IChainable? NextInChain;
    
    public ShaderModule Module;
    
    public string EntryPoint;
    
    public ConstantEntry[] Constants;
    
    public ProgrammableStageDescriptor(ShaderModule module, string entryPoint, ConstantEntry[] constants, IChainable? nextInChain = null)
    {
        this.Module = module;
        this.EntryPoint = entryPoint;
        this.Constants = constants;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint module;
        
        public byte* entryPoint;
        
        public ulong constantCount;
        
        public ConstantEntry.Native* constants;
    }
}

public partial struct ImageCopyTexture
{
    public IChainable? NextInChain;
    
    public Texture Texture;
    
    public uint MipLevel;
    
    public Origin3D Origin;
    
    public TextureAspect Aspect;
    
    public ImageCopyTexture(Texture texture, uint mipLevel, Origin3D origin, TextureAspect aspect, IChainable? nextInChain = null)
    {
        this.Texture = texture;
        this.MipLevel = mipLevel;
        this.Origin = origin;
        this.Aspect = aspect;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint texture;
        
        public uint mipLevel;
        
        public Origin3D.Native origin;
        
        public TextureAspect aspect;
    }
}

public partial struct ImageCopyBuffer
{
    public IChainable? NextInChain;
    
    public TextureDataLayout Layout;
    
    public Buffer Buffer;
    
    public ImageCopyBuffer(TextureDataLayout layout, Buffer buffer, IChainable? nextInChain = null)
    {
        this.Layout = layout;
        this.Buffer = buffer;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public TextureDataLayout.Native layout;
        
        public nint buffer;
    }
}

public partial struct DepthStencilState
{
    public IChainable? NextInChain;
    
    public TextureFormat Format;
    
    public bool DepthWriteEnabled;
    
    public CompareFunction DepthCompare;
    
    public StencilFaceState StencilFront;
    
    public StencilFaceState StencilBack;
    
    public uint StencilReadMask;
    
    public uint StencilWriteMask;
    
    public int DepthBias;
    
    public float DepthBiasSlopeScale;
    
    public float DepthBiasClamp;
    
    public DepthStencilState(TextureFormat format, bool depthWriteEnabled, CompareFunction depthCompare, StencilFaceState stencilFront, StencilFaceState stencilBack, uint stencilReadMask, uint stencilWriteMask, int depthBias, float depthBiasSlopeScale, float depthBiasClamp, IChainable? nextInChain = null)
    {
        this.Format = format;
        this.DepthWriteEnabled = depthWriteEnabled;
        this.DepthCompare = depthCompare;
        this.StencilFront = stencilFront;
        this.StencilBack = stencilBack;
        this.StencilReadMask = stencilReadMask;
        this.StencilWriteMask = stencilWriteMask;
        this.DepthBias = depthBias;
        this.DepthBiasSlopeScale = depthBiasSlopeScale;
        this.DepthBiasClamp = depthBiasClamp;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public TextureFormat format;
        
        public WGPUBool depthWriteEnabled;
        
        public CompareFunction depthCompare;
        
        public StencilFaceState.Native stencilFront;
        
        public StencilFaceState.Native stencilBack;
        
        public uint stencilReadMask;
        
        public uint stencilWriteMask;
        
        public int depthBias;
        
        public float depthBiasSlopeScale;
        
        public float depthBiasClamp;
    }
}

public partial struct ComputePassDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public ComputePassTimestampWrites? TimestampWrites;
    
    public ComputePassDescriptor(string label, ComputePassTimestampWrites? timestampWrites, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.TimestampWrites = timestampWrites;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ComputePassTimestampWrites.Native* timestampWrites;
    }
}

public partial struct CompilationInfo
{
    public IChainable? NextInChain;
    
    public CompilationMessage[] Messages;
    
    public CompilationInfo(CompilationMessage[] messages, IChainable? nextInChain = null)
    {
        this.Messages = messages;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public ulong messageCount;
        
        public CompilationMessage.Native* messages;
    }
}

public partial struct BlendState
{
    public BlendComponent Color;
    
    public BlendComponent Alpha;
    
    public BlendState(BlendComponent color, BlendComponent alpha)
    {
        this.Color = color;
        this.Alpha = alpha;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public BlendComponent.Native color;
        
        public BlendComponent.Native alpha;
    }
}

public partial struct BindGroupLayoutEntry
{
    public IChainable? NextInChain;
    
    public uint Binding;
    
    public ShaderStage Visibility;
    
    public BufferBindingLayout Buffer;
    
    public SamplerBindingLayout Sampler;
    
    public TextureBindingLayout Texture;
    
    public StorageTextureBindingLayout StorageTexture;
    
    public BindGroupLayoutEntry(uint binding, ShaderStage visibility, BufferBindingLayout buffer, SamplerBindingLayout sampler, TextureBindingLayout texture, StorageTextureBindingLayout storageTexture, IChainable? nextInChain = null)
    {
        this.Binding = binding;
        this.Visibility = visibility;
        this.Buffer = buffer;
        this.Sampler = sampler;
        this.Texture = texture;
        this.StorageTexture = storageTexture;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public uint binding;
        
        public ShaderStage visibility;
        
        public BufferBindingLayout.Native buffer;
        
        public SamplerBindingLayout.Native sampler;
        
        public TextureBindingLayout.Native texture;
        
        public StorageTextureBindingLayout.Native storageTexture;
    }
}

public partial struct BindGroupDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public BindGroupLayout Layout;
    
    public BindGroupEntry[] Entries;
    
    public BindGroupDescriptor(string label, BindGroupLayout layout, BindGroupEntry[] entries, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Layout = layout;
        this.Entries = entries;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public nint layout;
        
        public ulong entryCount;
        
        public BindGroupEntry.Native* entries;
    }
}

public partial struct VertexAttribute
{
    public VertexFormat Format;
    
    public ulong Offset;
    
    public uint ShaderLocation;
    
    public VertexAttribute(VertexFormat format, ulong offset, uint shaderLocation)
    {
        this.Format = format;
        this.Offset = offset;
        this.ShaderLocation = shaderLocation;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public VertexFormat format;
        
        public ulong offset;
        
        public uint shaderLocation;
    }
}

public partial struct TextureViewDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public TextureFormat Format;
    
    public TextureViewDimension Dimension;
    
    public uint BaseMipLevel;
    
    public uint MipLevelCount;
    
    public uint BaseArrayLayer;
    
    public uint ArrayLayerCount;
    
    public TextureAspect Aspect;
    
    public TextureViewDescriptor(string label, TextureFormat format, TextureViewDimension dimension, uint baseMipLevel, uint mipLevelCount, uint baseArrayLayer, uint arrayLayerCount, TextureAspect aspect, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Format = format;
        this.Dimension = dimension;
        this.BaseMipLevel = baseMipLevel;
        this.MipLevelCount = mipLevelCount;
        this.BaseArrayLayer = baseArrayLayer;
        this.ArrayLayerCount = arrayLayerCount;
        this.Aspect = aspect;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public TextureFormat format;
        
        public TextureViewDimension dimension;
        
        public uint baseMipLevel;
        
        public uint mipLevelCount;
        
        public uint baseArrayLayer;
        
        public uint arrayLayerCount;
        
        public TextureAspect aspect;
    }
}

public partial struct TextureDataLayout
{
    public IChainable? NextInChain;
    
    public ulong Offset;
    
    public uint BytesPerRow;
    
    public uint RowsPerImage;
    
    public TextureDataLayout(ulong offset, uint bytesPerRow, uint rowsPerImage, IChainable? nextInChain = null)
    {
        this.Offset = offset;
        this.BytesPerRow = bytesPerRow;
        this.RowsPerImage = rowsPerImage;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public ulong offset;
        
        public uint bytesPerRow;
        
        public uint rowsPerImage;
    }
}

public partial struct TextureBindingLayout
{
    public IChainable? NextInChain;
    
    public TextureSampleType SampleType;
    
    public TextureViewDimension ViewDimension;
    
    public bool Multisampled;
    
    public TextureBindingLayout(TextureSampleType sampleType, TextureViewDimension viewDimension, bool multisampled, IChainable? nextInChain = null)
    {
        this.SampleType = sampleType;
        this.ViewDimension = viewDimension;
        this.Multisampled = multisampled;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public TextureSampleType sampleType;
        
        public TextureViewDimension viewDimension;
        
        public WGPUBool multisampled;
    }
}

public partial struct SurfaceTexture
{
    public Texture Texture;
    
    public bool Suboptimal;
    
    public SurfaceGetCurrentTextureStatus Status;
    
    public SurfaceTexture(Texture texture, bool suboptimal, SurfaceGetCurrentTextureStatus status)
    {
        this.Texture = texture;
        this.Suboptimal = suboptimal;
        this.Status = status;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public nint texture;
        
        public WGPUBool suboptimal;
        
        public SurfaceGetCurrentTextureStatus status;
    }
}

public partial struct SurfaceDescriptorFromXlibWindow : IChainable
{
    public IChainable? Chain;
    
    public nint Display;
    
    public uint Window;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromXlibWindow;
        native->display = this.Display;
        native->window = this.Window;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromXlibWindow(nint display, uint window, IChainable? chain = null)
    {
        this.Display = display;
        this.Window = window;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint display;
        
        public uint window;
    }
}

public partial struct SurfaceDescriptorFromXcbWindow : IChainable
{
    public IChainable? Chain;
    
    public nint Connection;
    
    public uint Window;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromXcbWindow;
        native->connection = this.Connection;
        native->window = this.Window;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromXcbWindow(nint connection, uint window, IChainable? chain = null)
    {
        this.Connection = connection;
        this.Window = window;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint connection;
        
        public uint window;
    }
}

public partial struct SurfaceDescriptorFromWindowsHWND : IChainable
{
    public IChainable? Chain;
    
    public nint Hinstance;
    
    public nint Hwnd;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromWindowsHWND;
        native->hinstance = this.Hinstance;
        native->hwnd = this.Hwnd;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromWindowsHWND(nint hinstance, nint hwnd, IChainable? chain = null)
    {
        this.Hinstance = hinstance;
        this.Hwnd = hwnd;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint hinstance;
        
        public nint hwnd;
    }
}

public partial struct SurfaceDescriptorFromWaylandSurface : IChainable
{
    public IChainable? Chain;
    
    public nint Display;
    
    public nint Surface;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromWaylandSurface;
        native->display = this.Display;
        native->surface = this.Surface;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromWaylandSurface(nint display, nint surface, IChainable? chain = null)
    {
        this.Display = display;
        this.Surface = surface;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint display;
        
        public nint surface;
    }
}

public partial struct SurfaceDescriptorFromMetalLayer : IChainable
{
    public IChainable? Chain;
    
    public nint Layer;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromMetalLayer;
        native->layer = this.Layer;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromMetalLayer(nint layer, IChainable? chain = null)
    {
        this.Layer = layer;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint layer;
    }
}

public partial struct SurfaceDescriptorFromCanvasHTMLSelector : IChainable
{
    public IChainable? Chain;
    
    public string Selector;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromCanvasHTMLSelector;
        native->selector = MarshalUtils.AllocString(this.Selector);
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        MarshalUtils.FreeString(native->selector);
    }
    
    
    public SurfaceDescriptorFromCanvasHTMLSelector(string selector, IChainable? chain = null)
    {
        this.Selector = selector;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public byte* selector;
    }
}

public partial struct SurfaceDescriptorFromAndroidNativeWindow : IChainable
{
    public IChainable? Chain;
    
    public nint Window;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.SurfaceDescriptorFromAndroidNativeWindow;
        native->window = this.Window;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public SurfaceDescriptorFromAndroidNativeWindow(nint window, IChainable? chain = null)
    {
        this.Window = window;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public nint window;
    }
}

public partial struct SurfaceDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public SurfaceDescriptor(string label, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
    }
}

public partial struct SurfaceConfiguration
{
    public IChainable? NextInChain;
    
    public Device Device;
    
    public TextureFormat Format;
    
    public TextureUsage Usage;
    
    public TextureFormat[] ViewFormats;
    
    public CompositeAlphaMode AlphaMode;
    
    public uint Width;
    
    public uint Height;
    
    public PresentMode PresentMode;
    
    public SurfaceConfiguration(Device device, TextureFormat format, TextureUsage usage, TextureFormat[] viewFormats, CompositeAlphaMode alphaMode, uint width, uint height, PresentMode presentMode, IChainable? nextInChain = null)
    {
        this.Device = device;
        this.Format = format;
        this.Usage = usage;
        this.ViewFormats = viewFormats;
        this.AlphaMode = alphaMode;
        this.Width = width;
        this.Height = height;
        this.PresentMode = presentMode;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint device;
        
        public TextureFormat format;
        
        public TextureUsage usage;
        
        public ulong viewFormatCount;
        
        public TextureFormat* viewFormats;
        
        public CompositeAlphaMode alphaMode;
        
        public uint width;
        
        public uint height;
        
        public PresentMode presentMode;
    }
}

public partial struct SurfaceCapabilities
{
    public IChainable? NextInChain;
    
    public TextureFormat[] Formats;
    
    public PresentMode[] PresentModes;
    
    public CompositeAlphaMode[] AlphaModes;
    
    public SurfaceCapabilities(TextureFormat[] formats, PresentMode[] presentModes, CompositeAlphaMode[] alphaModes, IChainable? nextInChain = null)
    {
        this.Formats = formats;
        this.PresentModes = presentModes;
        this.AlphaModes = alphaModes;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStructOut.Native* nextInChain;
        
        public ulong formatCount;
        
        public TextureFormat* formats;
        
        public ulong presentModeCount;
        
        public PresentMode* presentModes;
        
        public ulong alphaModeCount;
        
        public CompositeAlphaMode* alphaModes;
    }
}

public partial struct StorageTextureBindingLayout
{
    public IChainable? NextInChain;
    
    public StorageTextureAccess Access;
    
    public TextureFormat Format;
    
    public TextureViewDimension ViewDimension;
    
    public StorageTextureBindingLayout(StorageTextureAccess access, TextureFormat format, TextureViewDimension viewDimension, IChainable? nextInChain = null)
    {
        this.Access = access;
        this.Format = format;
        this.ViewDimension = viewDimension;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public StorageTextureAccess access;
        
        public TextureFormat format;
        
        public TextureViewDimension viewDimension;
    }
}

public partial struct StencilFaceState
{
    public CompareFunction Compare;
    
    public StencilOperation FailOp;
    
    public StencilOperation DepthFailOp;
    
    public StencilOperation PassOp;
    
    public StencilFaceState(CompareFunction compare, StencilOperation failOp, StencilOperation depthFailOp, StencilOperation passOp)
    {
        this.Compare = compare;
        this.FailOp = failOp;
        this.DepthFailOp = depthFailOp;
        this.PassOp = passOp;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public CompareFunction compare;
        
        public StencilOperation failOp;
        
        public StencilOperation depthFailOp;
        
        public StencilOperation passOp;
    }
}

public partial struct ShaderModuleWGSLDescriptor : IChainable
{
    public IChainable? Chain;
    
    public string Code;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.ShaderModuleWGSLDescriptor;
        native->code = MarshalUtils.AllocString(this.Code);
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        MarshalUtils.FreeString(native->code);
    }
    
    
    public ShaderModuleWGSLDescriptor(string code, IChainable? chain = null)
    {
        this.Code = code;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public byte* code;
    }
}

public partial struct ShaderModuleSPIRVDescriptor : IChainable
{
    public IChainable? Chain;
    
    public uint[] Code;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.ShaderModuleSPIRVDescriptor;
        native->codeSize = (uint)(this.Code?.Length ?? 0);
        native->code = MarshalUtils.AllocArray(this.Code);
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        MarshalUtils.FreeArray(native->code);
    }
    
    
    public ShaderModuleSPIRVDescriptor(uint[] code, IChainable? chain = null)
    {
        this.Code = code;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public uint codeSize;
        
        public uint* code;
    }
}

public partial struct ShaderModuleCompilationHint
{
    public IChainable? NextInChain;
    
    public string EntryPoint;
    
    public PipelineLayout Layout;
    
    public ShaderModuleCompilationHint(string entryPoint, PipelineLayout layout, IChainable? nextInChain = null)
    {
        this.EntryPoint = entryPoint;
        this.Layout = layout;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* entryPoint;
        
        public nint layout;
    }
}

public partial struct SamplerDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public AddressMode AddressModeU;
    
    public AddressMode AddressModeV;
    
    public AddressMode AddressModeW;
    
    public FilterMode MagFilter;
    
    public FilterMode MinFilter;
    
    public MipmapFilterMode MipmapFilter;
    
    public float LodMinClamp;
    
    public float LodMaxClamp;
    
    public CompareFunction Compare;
    
    public ushort MaxAnisotropy;
    
    public SamplerDescriptor(string label, AddressMode addressModeU, AddressMode addressModeV, AddressMode addressModeW, FilterMode magFilter, FilterMode minFilter, MipmapFilterMode mipmapFilter, float lodMinClamp, float lodMaxClamp, CompareFunction compare, ushort maxAnisotropy, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.AddressModeU = addressModeU;
        this.AddressModeV = addressModeV;
        this.AddressModeW = addressModeW;
        this.MagFilter = magFilter;
        this.MinFilter = minFilter;
        this.MipmapFilter = mipmapFilter;
        this.LodMinClamp = lodMinClamp;
        this.LodMaxClamp = lodMaxClamp;
        this.Compare = compare;
        this.MaxAnisotropy = maxAnisotropy;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public AddressMode addressModeU;
        
        public AddressMode addressModeV;
        
        public AddressMode addressModeW;
        
        public FilterMode magFilter;
        
        public FilterMode minFilter;
        
        public MipmapFilterMode mipmapFilter;
        
        public float lodMinClamp;
        
        public float lodMaxClamp;
        
        public CompareFunction compare;
        
        public ushort maxAnisotropy;
    }
}

public partial struct SamplerBindingLayout
{
    public IChainable? NextInChain;
    
    public SamplerBindingType Type;
    
    public SamplerBindingLayout(SamplerBindingType type, IChainable? nextInChain = null)
    {
        this.Type = type;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public SamplerBindingType type;
    }
}

public partial struct RequestAdapterOptions
{
    public IChainable? NextInChain;
    
    public Surface CompatibleSurface;
    
    public PowerPreference PowerPreference;
    
    public BackendType BackendType;
    
    public bool ForceFallbackAdapter;
    
    public RequestAdapterOptions(Surface compatibleSurface, PowerPreference powerPreference, BackendType backendType, bool forceFallbackAdapter, IChainable? nextInChain = null)
    {
        this.CompatibleSurface = compatibleSurface;
        this.PowerPreference = powerPreference;
        this.BackendType = backendType;
        this.ForceFallbackAdapter = forceFallbackAdapter;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public nint compatibleSurface;
        
        public PowerPreference powerPreference;
        
        public BackendType backendType;
        
        public WGPUBool forceFallbackAdapter;
    }
}

public partial struct RenderPassTimestampWrites
{
    public QuerySet QuerySet;
    
    public uint BeginningOfPassWriteIndex;
    
    public uint EndOfPassWriteIndex;
    
    public RenderPassTimestampWrites(QuerySet querySet, uint beginningOfPassWriteIndex, uint endOfPassWriteIndex)
    {
        this.QuerySet = querySet;
        this.BeginningOfPassWriteIndex = beginningOfPassWriteIndex;
        this.EndOfPassWriteIndex = endOfPassWriteIndex;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public nint querySet;
        
        public uint beginningOfPassWriteIndex;
        
        public uint endOfPassWriteIndex;
    }
}

public partial struct RenderPassDescriptorMaxDrawCount : IChainable
{
    public IChainable? Chain;
    
    public ulong MaxDrawCount;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.RenderPassDescriptorMaxDrawCount;
        native->maxDrawCount = this.MaxDrawCount;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public RenderPassDescriptorMaxDrawCount(ulong maxDrawCount, IChainable? chain = null)
    {
        this.MaxDrawCount = maxDrawCount;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public ulong maxDrawCount;
    }
}

public partial struct RenderPassDepthStencilAttachment
{
    public TextureView View;
    
    public LoadOp DepthLoadOp;
    
    public StoreOp DepthStoreOp;
    
    public float DepthClearValue;
    
    public bool DepthReadOnly;
    
    public LoadOp StencilLoadOp;
    
    public StoreOp StencilStoreOp;
    
    public uint StencilClearValue;
    
    public bool StencilReadOnly;
    
    public RenderPassDepthStencilAttachment(TextureView view, LoadOp depthLoadOp, StoreOp depthStoreOp, float depthClearValue, bool depthReadOnly, LoadOp stencilLoadOp, StoreOp stencilStoreOp, uint stencilClearValue, bool stencilReadOnly)
    {
        this.View = view;
        this.DepthLoadOp = depthLoadOp;
        this.DepthStoreOp = depthStoreOp;
        this.DepthClearValue = depthClearValue;
        this.DepthReadOnly = depthReadOnly;
        this.StencilLoadOp = stencilLoadOp;
        this.StencilStoreOp = stencilStoreOp;
        this.StencilClearValue = stencilClearValue;
        this.StencilReadOnly = stencilReadOnly;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public nint view;
        
        public LoadOp depthLoadOp;
        
        public StoreOp depthStoreOp;
        
        public float depthClearValue;
        
        public WGPUBool depthReadOnly;
        
        public LoadOp stencilLoadOp;
        
        public StoreOp stencilStoreOp;
        
        public uint stencilClearValue;
        
        public WGPUBool stencilReadOnly;
    }
}

public partial struct RenderBundleEncoderDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public TextureFormat[] ColorFormats;
    
    public TextureFormat DepthStencilFormat;
    
    public uint SampleCount;
    
    public bool DepthReadOnly;
    
    public bool StencilReadOnly;
    
    public RenderBundleEncoderDescriptor(string label, TextureFormat[] colorFormats, TextureFormat depthStencilFormat, uint sampleCount, bool depthReadOnly, bool stencilReadOnly, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.ColorFormats = colorFormats;
        this.DepthStencilFormat = depthStencilFormat;
        this.SampleCount = sampleCount;
        this.DepthReadOnly = depthReadOnly;
        this.StencilReadOnly = stencilReadOnly;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong colorFormatCount;
        
        public TextureFormat* colorFormats;
        
        public TextureFormat depthStencilFormat;
        
        public uint sampleCount;
        
        public WGPUBool depthReadOnly;
        
        public WGPUBool stencilReadOnly;
    }
}

public partial struct RenderBundleDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public RenderBundleDescriptor(string label, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
    }
}

public partial struct QueueDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public QueueDescriptor(string label, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
    }
}

public partial struct QuerySetDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public QueryType Type;
    
    public uint Count;
    
    public QuerySetDescriptor(string label, QueryType type, uint count, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Type = type;
        this.Count = count;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public QueryType type;
        
        public uint count;
    }
}

public partial struct PrimitiveState
{
    public IChainable? NextInChain;
    
    public PrimitiveTopology Topology;
    
    public IndexFormat StripIndexFormat;
    
    public FrontFace FrontFace;
    
    public CullMode CullMode;
    
    public PrimitiveState(PrimitiveTopology topology, IndexFormat stripIndexFormat, FrontFace frontFace, CullMode cullMode, IChainable? nextInChain = null)
    {
        this.Topology = topology;
        this.StripIndexFormat = stripIndexFormat;
        this.FrontFace = frontFace;
        this.CullMode = cullMode;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public PrimitiveTopology topology;
        
        public IndexFormat stripIndexFormat;
        
        public FrontFace frontFace;
        
        public CullMode cullMode;
    }
}

public partial struct PrimitiveDepthClipControl : IChainable
{
    public IChainable? Chain;
    
    public bool UnclippedDepth;
    
    readonly IChainable? IChainable.Next => Chain;
    
    unsafe readonly int IChainable.SizeInBytes => sizeof(Native);
    
    unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
        native->chain.sType = SType.PrimitiveDepthClipControl;
        native->unclippedDepth = this.UnclippedDepth;
    }
    
    
    unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) 
    {
        Native* native = (Native*)chainedStruct;
    }
    
    
    public PrimitiveDepthClipControl(bool unclippedDepth, IChainable? chain = null)
    {
        this.UnclippedDepth = unclippedDepth;
        this.Chain = chain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native chain;
        
        public WGPUBool unclippedDepth;
    }
}

public partial struct PipelineLayoutDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public BindGroupLayout[] BindGroupLayouts;
    
    public PipelineLayoutDescriptor(string label, BindGroupLayout[] bindGroupLayouts, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.BindGroupLayouts = bindGroupLayouts;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public ulong bindGroupLayoutCount;
        
        public nint* bindGroupLayouts;
    }
}

public partial struct Origin3D
{
    public uint X;
    
    public uint Y;
    
    public uint Z;
    
    public Origin3D(uint x, uint y, uint z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public uint x;
        
        public uint y;
        
        public uint z;
    }
}

public partial struct MultisampleState
{
    public IChainable? NextInChain;
    
    public uint Count;
    
    public uint Mask;
    
    public bool AlphaToCoverageEnabled;
    
    public MultisampleState(uint count, uint mask, bool alphaToCoverageEnabled, IChainable? nextInChain = null)
    {
        this.Count = count;
        this.Mask = mask;
        this.AlphaToCoverageEnabled = alphaToCoverageEnabled;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public uint count;
        
        public uint mask;
        
        public WGPUBool alphaToCoverageEnabled;
    }
}

public partial struct Limits
{
    public uint MaxTextureDimension1D;
    
    public uint MaxTextureDimension2D;
    
    public uint MaxTextureDimension3D;
    
    public uint MaxTextureArrayLayers;
    
    public uint MaxBindGroups;
    
    public uint MaxBindGroupsPlusVertexBuffers;
    
    public uint MaxBindingsPerBindGroup;
    
    public uint MaxDynamicUniformBuffersPerPipelineLayout;
    
    public uint MaxDynamicStorageBuffersPerPipelineLayout;
    
    public uint MaxSampledTexturesPerShaderStage;
    
    public uint MaxSamplersPerShaderStage;
    
    public uint MaxStorageBuffersPerShaderStage;
    
    public uint MaxStorageTexturesPerShaderStage;
    
    public uint MaxUniformBuffersPerShaderStage;
    
    public ulong MaxUniformBufferBindingSize;
    
    public ulong MaxStorageBufferBindingSize;
    
    public uint MinUniformBufferOffsetAlignment;
    
    public uint MinStorageBufferOffsetAlignment;
    
    public uint MaxVertexBuffers;
    
    public ulong MaxBufferSize;
    
    public uint MaxVertexAttributes;
    
    public uint MaxVertexBufferArrayStride;
    
    public uint MaxInterStageShaderComponents;
    
    public uint MaxInterStageShaderVariables;
    
    public uint MaxColorAttachments;
    
    public uint MaxColorAttachmentBytesPerSample;
    
    public uint MaxComputeWorkgroupStorageSize;
    
    public uint MaxComputeInvocationsPerWorkgroup;
    
    public uint MaxComputeWorkgroupSizeX;
    
    public uint MaxComputeWorkgroupSizeY;
    
    public uint MaxComputeWorkgroupSizeZ;
    
    public uint MaxComputeWorkgroupsPerDimension;
    
    public Limits(uint maxTextureDimension1D, uint maxTextureDimension2D, uint maxTextureDimension3D, uint maxTextureArrayLayers, uint maxBindGroups, uint maxBindGroupsPlusVertexBuffers, uint maxBindingsPerBindGroup, uint maxDynamicUniformBuffersPerPipelineLayout, uint maxDynamicStorageBuffersPerPipelineLayout, uint maxSampledTexturesPerShaderStage, uint maxSamplersPerShaderStage, uint maxStorageBuffersPerShaderStage, uint maxStorageTexturesPerShaderStage, uint maxUniformBuffersPerShaderStage, ulong maxUniformBufferBindingSize, ulong maxStorageBufferBindingSize, uint minUniformBufferOffsetAlignment, uint minStorageBufferOffsetAlignment, uint maxVertexBuffers, ulong maxBufferSize, uint maxVertexAttributes, uint maxVertexBufferArrayStride, uint maxInterStageShaderComponents, uint maxInterStageShaderVariables, uint maxColorAttachments, uint maxColorAttachmentBytesPerSample, uint maxComputeWorkgroupStorageSize, uint maxComputeInvocationsPerWorkgroup, uint maxComputeWorkgroupSizeX, uint maxComputeWorkgroupSizeY, uint maxComputeWorkgroupSizeZ, uint maxComputeWorkgroupsPerDimension)
    {
        this.MaxTextureDimension1D = maxTextureDimension1D;
        this.MaxTextureDimension2D = maxTextureDimension2D;
        this.MaxTextureDimension3D = maxTextureDimension3D;
        this.MaxTextureArrayLayers = maxTextureArrayLayers;
        this.MaxBindGroups = maxBindGroups;
        this.MaxBindGroupsPlusVertexBuffers = maxBindGroupsPlusVertexBuffers;
        this.MaxBindingsPerBindGroup = maxBindingsPerBindGroup;
        this.MaxDynamicUniformBuffersPerPipelineLayout = maxDynamicUniformBuffersPerPipelineLayout;
        this.MaxDynamicStorageBuffersPerPipelineLayout = maxDynamicStorageBuffersPerPipelineLayout;
        this.MaxSampledTexturesPerShaderStage = maxSampledTexturesPerShaderStage;
        this.MaxSamplersPerShaderStage = maxSamplersPerShaderStage;
        this.MaxStorageBuffersPerShaderStage = maxStorageBuffersPerShaderStage;
        this.MaxStorageTexturesPerShaderStage = maxStorageTexturesPerShaderStage;
        this.MaxUniformBuffersPerShaderStage = maxUniformBuffersPerShaderStage;
        this.MaxUniformBufferBindingSize = maxUniformBufferBindingSize;
        this.MaxStorageBufferBindingSize = maxStorageBufferBindingSize;
        this.MinUniformBufferOffsetAlignment = minUniformBufferOffsetAlignment;
        this.MinStorageBufferOffsetAlignment = minStorageBufferOffsetAlignment;
        this.MaxVertexBuffers = maxVertexBuffers;
        this.MaxBufferSize = maxBufferSize;
        this.MaxVertexAttributes = maxVertexAttributes;
        this.MaxVertexBufferArrayStride = maxVertexBufferArrayStride;
        this.MaxInterStageShaderComponents = maxInterStageShaderComponents;
        this.MaxInterStageShaderVariables = maxInterStageShaderVariables;
        this.MaxColorAttachments = maxColorAttachments;
        this.MaxColorAttachmentBytesPerSample = maxColorAttachmentBytesPerSample;
        this.MaxComputeWorkgroupStorageSize = maxComputeWorkgroupStorageSize;
        this.MaxComputeInvocationsPerWorkgroup = maxComputeInvocationsPerWorkgroup;
        this.MaxComputeWorkgroupSizeX = maxComputeWorkgroupSizeX;
        this.MaxComputeWorkgroupSizeY = maxComputeWorkgroupSizeY;
        this.MaxComputeWorkgroupSizeZ = maxComputeWorkgroupSizeZ;
        this.MaxComputeWorkgroupsPerDimension = maxComputeWorkgroupsPerDimension;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public uint maxTextureDimension1D;
        
        public uint maxTextureDimension2D;
        
        public uint maxTextureDimension3D;
        
        public uint maxTextureArrayLayers;
        
        public uint maxBindGroups;
        
        public uint maxBindGroupsPlusVertexBuffers;
        
        public uint maxBindingsPerBindGroup;
        
        public uint maxDynamicUniformBuffersPerPipelineLayout;
        
        public uint maxDynamicStorageBuffersPerPipelineLayout;
        
        public uint maxSampledTexturesPerShaderStage;
        
        public uint maxSamplersPerShaderStage;
        
        public uint maxStorageBuffersPerShaderStage;
        
        public uint maxStorageTexturesPerShaderStage;
        
        public uint maxUniformBuffersPerShaderStage;
        
        public ulong maxUniformBufferBindingSize;
        
        public ulong maxStorageBufferBindingSize;
        
        public uint minUniformBufferOffsetAlignment;
        
        public uint minStorageBufferOffsetAlignment;
        
        public uint maxVertexBuffers;
        
        public ulong maxBufferSize;
        
        public uint maxVertexAttributes;
        
        public uint maxVertexBufferArrayStride;
        
        public uint maxInterStageShaderComponents;
        
        public uint maxInterStageShaderVariables;
        
        public uint maxColorAttachments;
        
        public uint maxColorAttachmentBytesPerSample;
        
        public uint maxComputeWorkgroupStorageSize;
        
        public uint maxComputeInvocationsPerWorkgroup;
        
        public uint maxComputeWorkgroupSizeX;
        
        public uint maxComputeWorkgroupSizeY;
        
        public uint maxComputeWorkgroupSizeZ;
        
        public uint maxComputeWorkgroupsPerDimension;
    }
}

public partial struct InstanceDescriptor
{
    public IChainable? NextInChain;
    
    public InstanceDescriptor(IChainable? nextInChain = null)
    {
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
    }
}

public partial struct Extent3D
{
    public uint Width;
    
    public uint Height;
    
    public uint DepthOrArrayLayers;
    
    public Extent3D(uint width, uint height, uint depthOrArrayLayers)
    {
        this.Width = width;
        this.Height = height;
        this.DepthOrArrayLayers = depthOrArrayLayers;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public uint width;
        
        public uint height;
        
        public uint depthOrArrayLayers;
    }
}

public partial struct ConstantEntry
{
    public IChainable? NextInChain;
    
    public string Key;
    
    public double Value;
    
    public ConstantEntry(string key, double value, IChainable? nextInChain = null)
    {
        this.Key = key;
        this.Value = value;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* key;
        
        public double value;
    }
}

public partial struct ComputePassTimestampWrites
{
    public QuerySet QuerySet;
    
    public uint BeginningOfPassWriteIndex;
    
    public uint EndOfPassWriteIndex;
    
    public ComputePassTimestampWrites(QuerySet querySet, uint beginningOfPassWriteIndex, uint endOfPassWriteIndex)
    {
        this.QuerySet = querySet;
        this.BeginningOfPassWriteIndex = beginningOfPassWriteIndex;
        this.EndOfPassWriteIndex = endOfPassWriteIndex;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public nint querySet;
        
        public uint beginningOfPassWriteIndex;
        
        public uint endOfPassWriteIndex;
    }
}

public partial struct CompilationMessage
{
    public IChainable? NextInChain;
    
    public string Message;
    
    public CompilationMessageType Type;
    
    public ulong LineNum;
    
    public ulong LinePos;
    
    public ulong Offset;
    
    public ulong Length;
    
    public ulong Utf16LinePos;
    
    public ulong Utf16Offset;
    
    public ulong Utf16Length;
    
    public CompilationMessage(string message, CompilationMessageType type, ulong lineNum, ulong linePos, ulong offset, ulong length, ulong utf16LinePos, ulong utf16Offset, ulong utf16Length, IChainable? nextInChain = null)
    {
        this.Message = message;
        this.Type = type;
        this.LineNum = lineNum;
        this.LinePos = linePos;
        this.Offset = offset;
        this.Length = length;
        this.Utf16LinePos = utf16LinePos;
        this.Utf16Offset = utf16Offset;
        this.Utf16Length = utf16Length;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* message;
        
        public CompilationMessageType type;
        
        public ulong lineNum;
        
        public ulong linePos;
        
        public ulong offset;
        
        public ulong length;
        
        public ulong utf16LinePos;
        
        public ulong utf16Offset;
        
        public ulong utf16Length;
    }
}

public partial struct CommandEncoderDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public CommandEncoderDescriptor(string label, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
    }
}

public partial struct CommandBufferDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public CommandBufferDescriptor(string label, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
    }
}

public partial struct Color
{
    public double R;
    
    public double G;
    
    public double B;
    
    public double A;
    
    public Color(double r, double g, double b, double a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public double r;
        
        public double g;
        
        public double b;
        
        public double a;
    }
}

public partial struct BufferDescriptor
{
    public IChainable? NextInChain;
    
    public string Label;
    
    public BufferUsage Usage;
    
    public ulong Size;
    
    public bool MappedAtCreation;
    
    public BufferDescriptor(string label, BufferUsage usage, ulong size, bool mappedAtCreation, IChainable? nextInChain = null)
    {
        this.Label = label;
        this.Usage = usage;
        this.Size = size;
        this.MappedAtCreation = mappedAtCreation;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public byte* label;
        
        public BufferUsage usage;
        
        public ulong size;
        
        public WGPUBool mappedAtCreation;
    }
}

public partial struct BufferBindingLayout
{
    public IChainable? NextInChain;
    
    public BufferBindingType Type;
    
    public bool HasDynamicOffset;
    
    public ulong MinBindingSize;
    
    public BufferBindingLayout(BufferBindingType type, bool hasDynamicOffset, ulong minBindingSize, IChainable? nextInChain = null)
    {
        this.Type = type;
        this.HasDynamicOffset = hasDynamicOffset;
        this.MinBindingSize = minBindingSize;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public BufferBindingType type;
        
        public WGPUBool hasDynamicOffset;
        
        public ulong minBindingSize;
    }
}

public partial struct BlendComponent
{
    public BlendOperation Operation;
    
    public BlendFactor SrcFactor;
    
    public BlendFactor DstFactor;
    
    public BlendComponent(BlendOperation operation, BlendFactor srcFactor, BlendFactor dstFactor)
    {
        this.Operation = operation;
        this.SrcFactor = srcFactor;
        this.DstFactor = dstFactor;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public BlendOperation operation;
        
        public BlendFactor srcFactor;
        
        public BlendFactor dstFactor;
    }
}

public partial struct BindGroupEntry
{
    public IChainable? NextInChain;
    
    public uint Binding;
    
    public Buffer Buffer;
    
    public ulong Offset;
    
    public ulong Size;
    
    public Sampler Sampler;
    
    public TextureView TextureView;
    
    public BindGroupEntry(uint binding, Buffer buffer, ulong offset, ulong size, Sampler sampler, TextureView textureView, IChainable? nextInChain = null)
    {
        this.Binding = binding;
        this.Buffer = buffer;
        this.Offset = offset;
        this.Size = size;
        this.Sampler = sampler;
        this.TextureView = textureView;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStruct.Native* nextInChain;
        
        public uint binding;
        
        public nint buffer;
        
        public ulong offset;
        
        public ulong size;
        
        public nint sampler;
        
        public nint textureView;
    }
}

public partial struct AdapterProperties
{
    public IChainable? NextInChain;
    
    public uint VendorID;
    
    public string VendorName;
    
    public string Architecture;
    
    public uint DeviceID;
    
    public string Name;
    
    public string DriverDescription;
    
    public AdapterType AdapterType;
    
    public BackendType BackendType;
    
    public AdapterProperties(uint vendorID, string vendorName, string architecture, uint deviceID, string name, string driverDescription, AdapterType adapterType, BackendType backendType, IChainable? nextInChain = null)
    {
        this.VendorID = vendorID;
        this.VendorName = vendorName;
        this.Architecture = architecture;
        this.DeviceID = deviceID;
        this.Name = name;
        this.DriverDescription = driverDescription;
        this.AdapterType = adapterType;
        this.BackendType = backendType;
        this.NextInChain = nextInChain;
    }
    
    
    [Unmanaged<Native>]
    internal unsafe partial struct Native
    {
        public ChainedStructOut.Native* nextInChain;
        
        public uint vendorID;
        
        public byte* vendorName;
        
        public byte* architecture;
        
        public uint deviceID;
        
        public byte* name;
        
        public byte* driverDescription;
        
        public AdapterType adapterType;
        
        public BackendType backendType;
    }
}

public unsafe partial class TextureView : GraphicsObject
{
    public TextureView(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuTextureViewSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuTextureViewReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuTextureViewRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Texture : GraphicsObject
{
    public Texture(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public TextureView CreateView(TextureViewDescriptor? descriptor)
    {
        TextureViewDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc TextureViewDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->format = descriptor.Value.Format;
            native_descriptor->dimension = descriptor.Value.Dimension;
            native_descriptor->baseMipLevel = descriptor.Value.BaseMipLevel;
            native_descriptor->mipLevelCount = descriptor.Value.MipLevelCount;
            native_descriptor->baseArrayLayer = descriptor.Value.BaseArrayLayer;
            native_descriptor->arrayLayerCount = descriptor.Value.ArrayLayerCount;
            native_descriptor->aspect = descriptor.Value.Aspect;
        }
        var result = Native.wgpuTextureCreateView(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public void Destroy()
    {
        Native.wgpuTextureDestroy(this.NativeHandle);
    }
    
    public uint DepthOrArrayLayers
    {
        get
        {
            var result = Native.wgpuTextureGetDepthOrArrayLayers(this.NativeHandle);
            return result;
        }
    }
    
    public TextureDimension Dimension
    {
        get
        {
            var result = Native.wgpuTextureGetDimension(this.NativeHandle);
            return result;
        }
    }
    
    public TextureFormat Format
    {
        get
        {
            var result = Native.wgpuTextureGetFormat(this.NativeHandle);
            return result;
        }
    }
    
    public uint Height
    {
        get
        {
            var result = Native.wgpuTextureGetHeight(this.NativeHandle);
            return result;
        }
    }
    
    public uint MipLevelCount
    {
        get
        {
            var result = Native.wgpuTextureGetMipLevelCount(this.NativeHandle);
            return result;
        }
    }
    
    public uint SampleCount
    {
        get
        {
            var result = Native.wgpuTextureGetSampleCount(this.NativeHandle);
            return result;
        }
    }
    
    public TextureUsage Usage
    {
        get
        {
            var result = Native.wgpuTextureGetUsage(this.NativeHandle);
            return result;
        }
    }
    
    public uint Width
    {
        get
        {
            var result = Native.wgpuTextureGetWidth(this.NativeHandle);
            return result;
        }
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuTextureSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuTextureReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuTextureRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Surface : GraphicsObject
{
    public Surface(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void Configure(SurfaceConfiguration? config)
    {
        fixed (TextureFormat* native_config_viewFormats_ptr = config?.ViewFormats)
        {
            SurfaceConfiguration.Native* native_config = MarshalUtils.SpanToPointer(stackalloc SurfaceConfiguration.Native[config.HasValue ? 1 : 0]);
            if (config is not null)
            {
                native_config->nextInChain = MarshalUtils.AllocChain(config.Value.NextInChain);
                native_config->device = config.Value.Device?.NativeHandle ?? 0;
                native_config->format = config.Value.Format;
                native_config->usage = config.Value.Usage;
                native_config->viewFormatCount = (ulong)(config.Value.ViewFormats?.Length ?? 0);
                native_config->viewFormats = native_config_viewFormats_ptr;
                native_config->alphaMode = config.Value.AlphaMode;
                native_config->width = config.Value.Width;
                native_config->height = config.Value.Height;
                native_config->presentMode = config.Value.PresentMode;
            }
            Native.wgpuSurfaceConfigure(this.NativeHandle, native_config);
            if (config is not null)
            {
                MarshalUtils.FreeChain(config.Value.NextInChain, native_config->nextInChain);
            }
        }
    }
    
    public void GetCurrentTexture(out SurfaceTexture surfaceTexture)
    {
        SurfaceTexture.Native* native_surfaceTexture = MarshalUtils.SpanToPointer(stackalloc SurfaceTexture.Native[1]);
        Native.wgpuSurfaceGetCurrentTexture(this.NativeHandle, native_surfaceTexture);
        surfaceTexture = default;
        if (native_surfaceTexture is not null)
        {
            SurfaceTexture surfaceTexture_value = default;
            surfaceTexture_value.Texture = native_surfaceTexture->texture is 0 ? null : new(native_surfaceTexture->texture);
            surfaceTexture_value.Suboptimal = native_surfaceTexture->suboptimal;
            surfaceTexture_value.Status = native_surfaceTexture->status;
            surfaceTexture = surfaceTexture_value;
        }
    }
    
    public TextureFormat GetPreferredFormat(Adapter adapter)
    {
        nint native_adapter = adapter?.NativeHandle ?? 0;
        var result = Native.wgpuSurfaceGetPreferredFormat(this.NativeHandle, native_adapter);
        return result;
    }
    
    public void Present()
    {
        Native.wgpuSurfacePresent(this.NativeHandle);
    }
    
    public void Unconfigure()
    {
        Native.wgpuSurfaceUnconfigure(this.NativeHandle);
    }
    
    public void Reference()
    {
        Native.wgpuSurfaceReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuSurfaceRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class ShaderModule : GraphicsObject
{
    public ShaderModule(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void GetCompilationInfo(CompilationInfoCallback callback)
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_CompilationInfoCallback(CompilationInfoRequestStatus status, CompilationInfo.Native* compilationInfo, nint userdata)
        {
            CompilationInfo? managed_compilationInfo = default;
            if (compilationInfo is not null)
            {
                CompilationInfo managed_compilationInfo_value = default;
                managed_compilationInfo_value.Messages = new CompilationMessage[compilationInfo->messageCount];
                for (int Messages_i = 0; Messages_i < managed_compilationInfo_value.Messages.Length; Messages_i++)
                {
                    managed_compilationInfo_value.Messages[Messages_i] = default;
                    managed_compilationInfo_value.Messages[Messages_i].Message = MarshalUtils.StringFromPointer(compilationInfo->messages[Messages_i].message);
                    managed_compilationInfo_value.Messages[Messages_i].Type = compilationInfo->messages[Messages_i].type;
                    managed_compilationInfo_value.Messages[Messages_i].LineNum = compilationInfo->messages[Messages_i].lineNum;
                    managed_compilationInfo_value.Messages[Messages_i].LinePos = compilationInfo->messages[Messages_i].linePos;
                    managed_compilationInfo_value.Messages[Messages_i].Offset = compilationInfo->messages[Messages_i].offset;
                    managed_compilationInfo_value.Messages[Messages_i].Length = compilationInfo->messages[Messages_i].length;
                    managed_compilationInfo_value.Messages[Messages_i].Utf16LinePos = compilationInfo->messages[Messages_i].utf16LinePos;
                    managed_compilationInfo_value.Messages[Messages_i].Utf16Offset = compilationInfo->messages[Messages_i].utf16Offset;
                    managed_compilationInfo_value.Messages[Messages_i].Utf16Length = compilationInfo->messages[Messages_i].utf16Length;
                }
                managed_compilationInfo = managed_compilationInfo_value;
            }
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            CompilationInfoCallback callback = (CompilationInfoCallback)handle.Target!;
            callback(status, managed_compilationInfo);
            handle.Free();
        }
        GCHandle CompilationInfoCallback_callbackHandle = GCHandle.Alloc(callback);
        nint CompilationInfoCallback_userdata = GCHandle.ToIntPtr(CompilationInfoCallback_callbackHandle);
        Native.wgpuShaderModuleGetCompilationInfo(this.NativeHandle, &Native_CompilationInfoCallback, CompilationInfoCallback_userdata);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuShaderModuleSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuShaderModuleReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuShaderModuleRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Sampler : GraphicsObject
{
    public Sampler(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuSamplerSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuSamplerReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuSamplerRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class RenderPipeline : GraphicsObject
{
    public RenderPipeline(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public BindGroupLayout GetBindGroupLayout(uint groupIndex)
    {
        var result = Native.wgpuRenderPipelineGetBindGroupLayout(this.NativeHandle, groupIndex);
        return new(result);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuRenderPipelineSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuRenderPipelineReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuRenderPipelineRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class RenderPassEncoder : GraphicsObject
{
    public RenderPassEncoder(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void BeginOcclusionQuery(uint queryIndex)
    {
        Native.wgpuRenderPassEncoderBeginOcclusionQuery(this.NativeHandle, queryIndex);
    }
    
    public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
    {
        Native.wgpuRenderPassEncoderDraw(this.NativeHandle, vertexCount, instanceCount, firstVertex, firstInstance);
    }
    
    public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance)
    {
        Native.wgpuRenderPassEncoderDrawIndexed(this.NativeHandle, indexCount, instanceCount, firstIndex, baseVertex, firstInstance);
    }
    
    public void DrawIndexedIndirect(Buffer indirectBuffer, ulong indirectOffset)
    {
        nint native_indirectBuffer = indirectBuffer?.NativeHandle ?? 0;
        Native.wgpuRenderPassEncoderDrawIndexedIndirect(this.NativeHandle, native_indirectBuffer, indirectOffset);
    }
    
    public void DrawIndirect(Buffer indirectBuffer, ulong indirectOffset)
    {
        nint native_indirectBuffer = indirectBuffer?.NativeHandle ?? 0;
        Native.wgpuRenderPassEncoderDrawIndirect(this.NativeHandle, native_indirectBuffer, indirectOffset);
    }
    
    public void End()
    {
        Native.wgpuRenderPassEncoderEnd(this.NativeHandle);
    }
    
    public void EndOcclusionQuery()
    {
        Native.wgpuRenderPassEncoderEndOcclusionQuery(this.NativeHandle);
    }
    
    public void ExecuteBundles(RenderBundle[] bundles)
    {
        nint* native_bundles = MarshalUtils.SpanToPointer(stackalloc nint[bundles?.Length ?? 0]);
        for (int native_bundles_i = 0; native_bundles_i < (bundles?.Length ?? 0); native_bundles_i++)
        {
            native_bundles[native_bundles_i] = bundles[native_bundles_i]?.NativeHandle ?? 0;
        }
        Native.wgpuRenderPassEncoderExecuteBundles(this.NativeHandle, (nuint)bundles.Length, native_bundles);
    }
    
    public void InsertDebugMarker(string markerLabel)
    {
        byte* native_markerLabel = MarshalUtils.AllocString(markerLabel);
        Native.wgpuRenderPassEncoderInsertDebugMarker(this.NativeHandle, native_markerLabel);
        MarshalUtils.FreeString(native_markerLabel);
    }
    
    public void PopDebugGroup()
    {
        Native.wgpuRenderPassEncoderPopDebugGroup(this.NativeHandle);
    }
    
    public void PushDebugGroup(string groupLabel)
    {
        byte* native_groupLabel = MarshalUtils.AllocString(groupLabel);
        Native.wgpuRenderPassEncoderPushDebugGroup(this.NativeHandle, native_groupLabel);
        MarshalUtils.FreeString(native_groupLabel);
    }
    
    public void SetBindGroup(uint groupIndex, BindGroup group, uint[] dynamicOffsets)
    {
        fixed (uint* native_dynamicOffsets_ptr = dynamicOffsets)
        {
            nint native_group = group?.NativeHandle ?? 0;
            uint* native_dynamicOffsets = native_dynamicOffsets_ptr;
            Native.wgpuRenderPassEncoderSetBindGroup(this.NativeHandle, groupIndex, native_group, (nuint)dynamicOffsets.Length, native_dynamicOffsets);
        }
    }
    
    public void SetBlendConstant(Color? color)
    {
        Color.Native* native_color = MarshalUtils.SpanToPointer(stackalloc Color.Native[color.HasValue ? 1 : 0]);
        if (color is not null)
        {
            native_color->r = color.Value.R;
            native_color->g = color.Value.G;
            native_color->b = color.Value.B;
            native_color->a = color.Value.A;
        }
        Native.wgpuRenderPassEncoderSetBlendConstant(this.NativeHandle, native_color);
        if (color is not null)
        {
        }
    }
    
    public void SetIndexBuffer(Buffer buffer, IndexFormat format, ulong offset, ulong size)
    {
        nint native_buffer = buffer?.NativeHandle ?? 0;
        Native.wgpuRenderPassEncoderSetIndexBuffer(this.NativeHandle, native_buffer, format, offset, size);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuRenderPassEncoderSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void SetPipeline(RenderPipeline pipeline)
    {
        nint native_pipeline = pipeline?.NativeHandle ?? 0;
        Native.wgpuRenderPassEncoderSetPipeline(this.NativeHandle, native_pipeline);
    }
    
    public void SetScissorRect(uint x, uint y, uint width, uint height)
    {
        Native.wgpuRenderPassEncoderSetScissorRect(this.NativeHandle, x, y, width, height);
    }
    
    public void SetStencilReference(uint reference)
    {
        Native.wgpuRenderPassEncoderSetStencilReference(this.NativeHandle, reference);
    }
    
    public void SetVertexBuffer(uint slot, Buffer buffer, ulong offset, ulong size)
    {
        nint native_buffer = buffer?.NativeHandle ?? 0;
        Native.wgpuRenderPassEncoderSetVertexBuffer(this.NativeHandle, slot, native_buffer, offset, size);
    }
    
    public void SetViewport(float x, float y, float width, float height, float minDepth, float maxDepth)
    {
        Native.wgpuRenderPassEncoderSetViewport(this.NativeHandle, x, y, width, height, minDepth, maxDepth);
    }
    
    public void Reference()
    {
        Native.wgpuRenderPassEncoderReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuRenderPassEncoderRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class RenderBundleEncoder : GraphicsObject
{
    public RenderBundleEncoder(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance)
    {
        Native.wgpuRenderBundleEncoderDraw(this.NativeHandle, vertexCount, instanceCount, firstVertex, firstInstance);
    }
    
    public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance)
    {
        Native.wgpuRenderBundleEncoderDrawIndexed(this.NativeHandle, indexCount, instanceCount, firstIndex, baseVertex, firstInstance);
    }
    
    public void DrawIndexedIndirect(Buffer indirectBuffer, ulong indirectOffset)
    {
        nint native_indirectBuffer = indirectBuffer?.NativeHandle ?? 0;
        Native.wgpuRenderBundleEncoderDrawIndexedIndirect(this.NativeHandle, native_indirectBuffer, indirectOffset);
    }
    
    public void DrawIndirect(Buffer indirectBuffer, ulong indirectOffset)
    {
        nint native_indirectBuffer = indirectBuffer?.NativeHandle ?? 0;
        Native.wgpuRenderBundleEncoderDrawIndirect(this.NativeHandle, native_indirectBuffer, indirectOffset);
    }
    
    public RenderBundle Finish(RenderBundleDescriptor? descriptor)
    {
        RenderBundleDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc RenderBundleDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
        }
        var result = Native.wgpuRenderBundleEncoderFinish(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public void InsertDebugMarker(string markerLabel)
    {
        byte* native_markerLabel = MarshalUtils.AllocString(markerLabel);
        Native.wgpuRenderBundleEncoderInsertDebugMarker(this.NativeHandle, native_markerLabel);
        MarshalUtils.FreeString(native_markerLabel);
    }
    
    public void PopDebugGroup()
    {
        Native.wgpuRenderBundleEncoderPopDebugGroup(this.NativeHandle);
    }
    
    public void PushDebugGroup(string groupLabel)
    {
        byte* native_groupLabel = MarshalUtils.AllocString(groupLabel);
        Native.wgpuRenderBundleEncoderPushDebugGroup(this.NativeHandle, native_groupLabel);
        MarshalUtils.FreeString(native_groupLabel);
    }
    
    public void SetBindGroup(uint groupIndex, BindGroup group, uint[] dynamicOffsets)
    {
        fixed (uint* native_dynamicOffsets_ptr = dynamicOffsets)
        {
            nint native_group = group?.NativeHandle ?? 0;
            uint* native_dynamicOffsets = native_dynamicOffsets_ptr;
            Native.wgpuRenderBundleEncoderSetBindGroup(this.NativeHandle, groupIndex, native_group, (nuint)dynamicOffsets.Length, native_dynamicOffsets);
        }
    }
    
    public void SetIndexBuffer(Buffer buffer, IndexFormat format, ulong offset, ulong size)
    {
        nint native_buffer = buffer?.NativeHandle ?? 0;
        Native.wgpuRenderBundleEncoderSetIndexBuffer(this.NativeHandle, native_buffer, format, offset, size);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuRenderBundleEncoderSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void SetPipeline(RenderPipeline pipeline)
    {
        nint native_pipeline = pipeline?.NativeHandle ?? 0;
        Native.wgpuRenderBundleEncoderSetPipeline(this.NativeHandle, native_pipeline);
    }
    
    public void SetVertexBuffer(uint slot, Buffer buffer, ulong offset, ulong size)
    {
        nint native_buffer = buffer?.NativeHandle ?? 0;
        Native.wgpuRenderBundleEncoderSetVertexBuffer(this.NativeHandle, slot, native_buffer, offset, size);
    }
    
    public void Reference()
    {
        Native.wgpuRenderBundleEncoderReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuRenderBundleEncoderRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class RenderBundle : GraphicsObject
{
    public RenderBundle(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuRenderBundleSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuRenderBundleReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuRenderBundleRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Queue : GraphicsObject
{
    public Queue(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void OnSubmittedWorkDone(QueueWorkDoneCallback callback)
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_QueueWorkDoneCallback(QueueWorkDoneStatus status, nint userdata)
        {
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            QueueWorkDoneCallback callback = (QueueWorkDoneCallback)handle.Target!;
            callback(status);
            handle.Free();
        }
        GCHandle QueueWorkDoneCallback_callbackHandle = GCHandle.Alloc(callback);
        nint QueueWorkDoneCallback_userdata = GCHandle.ToIntPtr(QueueWorkDoneCallback_callbackHandle);
        Native.wgpuQueueOnSubmittedWorkDone(this.NativeHandle, &Native_QueueWorkDoneCallback, QueueWorkDoneCallback_userdata);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuQueueSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Submit(CommandBuffer[] commands)
    {
        nint* native_commands = MarshalUtils.SpanToPointer(stackalloc nint[commands?.Length ?? 0]);
        for (int native_commands_i = 0; native_commands_i < (commands?.Length ?? 0); native_commands_i++)
        {
            native_commands[native_commands_i] = commands[native_commands_i]?.NativeHandle ?? 0;
        }
        Native.wgpuQueueSubmit(this.NativeHandle, (nuint)commands.Length, native_commands);
    }
    
    public void WriteTexture(ImageCopyTexture? destination, byte[] data, TextureDataLayout? dataLayout, Extent3D? writeSize)
    {
        fixed (byte* native_data_ptr = data)
        {
            ImageCopyTexture.Native* native_destination = MarshalUtils.SpanToPointer(stackalloc ImageCopyTexture.Native[destination.HasValue ? 1 : 0]);
            if (destination is not null)
            {
                native_destination->nextInChain = MarshalUtils.AllocChain(destination.Value.NextInChain);
                native_destination->texture = destination.Value.Texture?.NativeHandle ?? 0;
                native_destination->mipLevel = destination.Value.MipLevel;
                native_destination->origin = default;
                native_destination->origin.x = destination.Value.Origin.X;
                native_destination->origin.y = destination.Value.Origin.Y;
                native_destination->origin.z = destination.Value.Origin.Z;
                native_destination->aspect = destination.Value.Aspect;
            }
            byte* native_data = native_data_ptr;
            TextureDataLayout.Native* native_dataLayout = MarshalUtils.SpanToPointer(stackalloc TextureDataLayout.Native[dataLayout.HasValue ? 1 : 0]);
            if (dataLayout is not null)
            {
                native_dataLayout->nextInChain = MarshalUtils.AllocChain(dataLayout.Value.NextInChain);
                native_dataLayout->offset = dataLayout.Value.Offset;
                native_dataLayout->bytesPerRow = dataLayout.Value.BytesPerRow;
                native_dataLayout->rowsPerImage = dataLayout.Value.RowsPerImage;
            }
            Extent3D.Native* native_writeSize = MarshalUtils.SpanToPointer(stackalloc Extent3D.Native[writeSize.HasValue ? 1 : 0]);
            if (writeSize is not null)
            {
                native_writeSize->width = writeSize.Value.Width;
                native_writeSize->height = writeSize.Value.Height;
                native_writeSize->depthOrArrayLayers = writeSize.Value.DepthOrArrayLayers;
            }
            Native.wgpuQueueWriteTexture(this.NativeHandle, native_destination, native_data, (nuint)data.Length, native_dataLayout, native_writeSize);
            if (destination is not null)
            {
                MarshalUtils.FreeChain(destination.Value.NextInChain, native_destination->nextInChain);
            }
            if (dataLayout is not null)
            {
                MarshalUtils.FreeChain(dataLayout.Value.NextInChain, native_dataLayout->nextInChain);
            }
            if (writeSize is not null)
            {
            }
        }
    }
    
    public void Reference()
    {
        Native.wgpuQueueReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuQueueRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class QuerySet : GraphicsObject
{
    public QuerySet(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void Destroy()
    {
        Native.wgpuQuerySetDestroy(this.NativeHandle);
    }
    
    public uint Count
    {
        get
        {
            var result = Native.wgpuQuerySetGetCount(this.NativeHandle);
            return result;
        }
    }
    
    public QueryType Type
    {
        get
        {
            var result = Native.wgpuQuerySetGetType(this.NativeHandle);
            return result;
        }
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuQuerySetSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuQuerySetReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuQuerySetRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class PipelineLayout : GraphicsObject
{
    public PipelineLayout(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuPipelineLayoutSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuPipelineLayoutReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuPipelineLayoutRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Instance : GraphicsObject
{
    public Instance(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public Surface CreateSurface(SurfaceDescriptor? descriptor)
    {
        SurfaceDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc SurfaceDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
        }
        var result = Native.wgpuInstanceCreateSurface(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public void ProcessEvents()
    {
        Native.wgpuInstanceProcessEvents(this.NativeHandle);
    }
    
    public void RequestAdapter(RequestAdapterOptions? options, RequestAdapterCallback callback)
    {
        RequestAdapterOptions.Native* native_options = MarshalUtils.SpanToPointer(stackalloc RequestAdapterOptions.Native[options.HasValue ? 1 : 0]);
        if (options is not null)
        {
            native_options->nextInChain = MarshalUtils.AllocChain(options.Value.NextInChain);
            native_options->compatibleSurface = options.Value.CompatibleSurface?.NativeHandle ?? 0;
            native_options->powerPreference = options.Value.PowerPreference;
            native_options->backendType = options.Value.BackendType;
            native_options->forceFallbackAdapter = options.Value.ForceFallbackAdapter;
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_RequestAdapterCallback(RequestAdapterStatus status, nint adapter, byte* message, nint userdata)
        {
            Adapter managed_adapter = adapter is 0 ? null : new(adapter);
            string managed_message = MarshalUtils.StringFromPointer(message);
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            RequestAdapterCallback callback = (RequestAdapterCallback)handle.Target!;
            callback(status, managed_adapter, managed_message);
            handle.Free();
        }
        GCHandle RequestAdapterCallback_callbackHandle = GCHandle.Alloc(callback);
        nint RequestAdapterCallback_userdata = GCHandle.ToIntPtr(RequestAdapterCallback_callbackHandle);
        Native.wgpuInstanceRequestAdapter(this.NativeHandle, native_options, &Native_RequestAdapterCallback, RequestAdapterCallback_userdata);
        if (options is not null)
        {
            MarshalUtils.FreeChain(options.Value.NextInChain, native_options->nextInChain);
        }
    }
    
    public void Reference()
    {
        Native.wgpuInstanceReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuInstanceRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Device : GraphicsObject
{
    public Device(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public BindGroup CreateBindGroup(BindGroupDescriptor? descriptor)
    {
        BindGroupDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc BindGroupDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->layout = descriptor.Value.Layout?.NativeHandle ?? 0;
            native_descriptor->entryCount = (ulong)(descriptor.Value.Entries?.Length ?? 0);
            native_descriptor->entries = MarshalUtils.SpanToPointer(stackalloc BindGroupEntry.Native[descriptor.Value.Entries?.Length ?? 0]);
            for (int entries_i = 0; entries_i < (descriptor.Value.Entries?.Length ?? 0); entries_i++)
            {
                native_descriptor->entries[entries_i] = default;
                native_descriptor->entries[entries_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].NextInChain);
                native_descriptor->entries[entries_i].binding = descriptor.Value.Entries[entries_i].Binding;
                native_descriptor->entries[entries_i].buffer = descriptor.Value.Entries[entries_i].Buffer?.NativeHandle ?? 0;
                native_descriptor->entries[entries_i].offset = descriptor.Value.Entries[entries_i].Offset;
                native_descriptor->entries[entries_i].size = descriptor.Value.Entries[entries_i].Size;
                native_descriptor->entries[entries_i].sampler = descriptor.Value.Entries[entries_i].Sampler?.NativeHandle ?? 0;
                native_descriptor->entries[entries_i].textureView = descriptor.Value.Entries[entries_i].TextureView?.NativeHandle ?? 0;
            }
        }
        var result = Native.wgpuDeviceCreateBindGroup(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public BindGroupLayout CreateBindGroupLayout(BindGroupLayoutDescriptor? descriptor)
    {
        BindGroupLayoutDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc BindGroupLayoutDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->entryCount = (ulong)(descriptor.Value.Entries?.Length ?? 0);
            native_descriptor->entries = MarshalUtils.SpanToPointer(stackalloc BindGroupLayoutEntry.Native[descriptor.Value.Entries?.Length ?? 0]);
            for (int entries_i = 0; entries_i < (descriptor.Value.Entries?.Length ?? 0); entries_i++)
            {
                native_descriptor->entries[entries_i] = default;
                native_descriptor->entries[entries_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].NextInChain);
                native_descriptor->entries[entries_i].binding = descriptor.Value.Entries[entries_i].Binding;
                native_descriptor->entries[entries_i].visibility = descriptor.Value.Entries[entries_i].Visibility;
                native_descriptor->entries[entries_i].buffer = default;
                native_descriptor->entries[entries_i].buffer.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].Buffer.NextInChain);
                native_descriptor->entries[entries_i].buffer.type = descriptor.Value.Entries[entries_i].Buffer.Type;
                native_descriptor->entries[entries_i].buffer.hasDynamicOffset = descriptor.Value.Entries[entries_i].Buffer.HasDynamicOffset;
                native_descriptor->entries[entries_i].buffer.minBindingSize = descriptor.Value.Entries[entries_i].Buffer.MinBindingSize;
                native_descriptor->entries[entries_i].sampler = default;
                native_descriptor->entries[entries_i].sampler.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].Sampler.NextInChain);
                native_descriptor->entries[entries_i].sampler.type = descriptor.Value.Entries[entries_i].Sampler.Type;
                native_descriptor->entries[entries_i].texture = default;
                native_descriptor->entries[entries_i].texture.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].Texture.NextInChain);
                native_descriptor->entries[entries_i].texture.sampleType = descriptor.Value.Entries[entries_i].Texture.SampleType;
                native_descriptor->entries[entries_i].texture.viewDimension = descriptor.Value.Entries[entries_i].Texture.ViewDimension;
                native_descriptor->entries[entries_i].texture.multisampled = descriptor.Value.Entries[entries_i].Texture.Multisampled;
                native_descriptor->entries[entries_i].storageTexture = default;
                native_descriptor->entries[entries_i].storageTexture.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Entries[entries_i].StorageTexture.NextInChain);
                native_descriptor->entries[entries_i].storageTexture.access = descriptor.Value.Entries[entries_i].StorageTexture.Access;
                native_descriptor->entries[entries_i].storageTexture.format = descriptor.Value.Entries[entries_i].StorageTexture.Format;
                native_descriptor->entries[entries_i].storageTexture.viewDimension = descriptor.Value.Entries[entries_i].StorageTexture.ViewDimension;
            }
        }
        var result = Native.wgpuDeviceCreateBindGroupLayout(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public Buffer CreateBuffer(BufferDescriptor? descriptor)
    {
        BufferDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc BufferDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->usage = descriptor.Value.Usage;
            native_descriptor->size = descriptor.Value.Size;
            native_descriptor->mappedAtCreation = descriptor.Value.MappedAtCreation;
        }
        var result = Native.wgpuDeviceCreateBuffer(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public CommandEncoder CreateCommandEncoder(CommandEncoderDescriptor? descriptor)
    {
        CommandEncoderDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc CommandEncoderDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
        }
        var result = Native.wgpuDeviceCreateCommandEncoder(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public ComputePipeline CreateComputePipeline(ComputePipelineDescriptor? descriptor)
    {
        ComputePipelineDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc ComputePipelineDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->layout = descriptor.Value.Layout?.NativeHandle ?? 0;
            native_descriptor->compute = default;
            native_descriptor->compute.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Compute.NextInChain);
            native_descriptor->compute.module = descriptor.Value.Compute.Module?.NativeHandle ?? 0;
            native_descriptor->compute.entryPoint = MarshalUtils.AllocString(descriptor.Value.Compute.EntryPoint);
            native_descriptor->compute.constantCount = (ulong)(descriptor.Value.Compute.Constants?.Length ?? 0);
            native_descriptor->compute.constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Compute.Constants?.Length ?? 0]);
            for (int constants_i = 0; constants_i < (descriptor.Value.Compute.Constants?.Length ?? 0); constants_i++)
            {
                native_descriptor->compute.constants[constants_i] = default;
                native_descriptor->compute.constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Compute.Constants[constants_i].NextInChain);
                native_descriptor->compute.constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Compute.Constants[constants_i].Key);
                native_descriptor->compute.constants[constants_i].value = descriptor.Value.Compute.Constants[constants_i].Value;
            }
        }
        var result = Native.wgpuDeviceCreateComputePipeline(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            MarshalUtils.FreeChain(descriptor.Value.Compute.NextInChain, native_descriptor->compute.nextInChain);
            MarshalUtils.FreeString(native_descriptor->compute.entryPoint);
        }
        return new(result);
    }
    
    public void CreateComputePipelineAsync(ComputePipelineDescriptor? descriptor, CreateComputePipelineAsyncCallback callback)
    {
        ComputePipelineDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc ComputePipelineDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->layout = descriptor.Value.Layout?.NativeHandle ?? 0;
            native_descriptor->compute = default;
            native_descriptor->compute.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Compute.NextInChain);
            native_descriptor->compute.module = descriptor.Value.Compute.Module?.NativeHandle ?? 0;
            native_descriptor->compute.entryPoint = MarshalUtils.AllocString(descriptor.Value.Compute.EntryPoint);
            native_descriptor->compute.constantCount = (ulong)(descriptor.Value.Compute.Constants?.Length ?? 0);
            native_descriptor->compute.constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Compute.Constants?.Length ?? 0]);
            for (int constants_i = 0; constants_i < (descriptor.Value.Compute.Constants?.Length ?? 0); constants_i++)
            {
                native_descriptor->compute.constants[constants_i] = default;
                native_descriptor->compute.constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Compute.Constants[constants_i].NextInChain);
                native_descriptor->compute.constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Compute.Constants[constants_i].Key);
                native_descriptor->compute.constants[constants_i].value = descriptor.Value.Compute.Constants[constants_i].Value;
            }
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_CreateComputePipelineAsyncCallback(CreatePipelineAsyncStatus status, nint pipeline, byte* message, nint userdata)
        {
            ComputePipeline managed_pipeline = pipeline is 0 ? null : new(pipeline);
            string managed_message = MarshalUtils.StringFromPointer(message);
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            CreateComputePipelineAsyncCallback callback = (CreateComputePipelineAsyncCallback)handle.Target!;
            callback(status, managed_pipeline, managed_message);
            handle.Free();
        }
        GCHandle CreateComputePipelineAsyncCallback_callbackHandle = GCHandle.Alloc(callback);
        nint CreateComputePipelineAsyncCallback_userdata = GCHandle.ToIntPtr(CreateComputePipelineAsyncCallback_callbackHandle);
        Native.wgpuDeviceCreateComputePipelineAsync(this.NativeHandle, native_descriptor, &Native_CreateComputePipelineAsyncCallback, CreateComputePipelineAsyncCallback_userdata);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            MarshalUtils.FreeChain(descriptor.Value.Compute.NextInChain, native_descriptor->compute.nextInChain);
            MarshalUtils.FreeString(native_descriptor->compute.entryPoint);
        }
    }
    
    public PipelineLayout CreatePipelineLayout(PipelineLayoutDescriptor? descriptor)
    {
        PipelineLayoutDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc PipelineLayoutDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->bindGroupLayoutCount = (ulong)(descriptor.Value.BindGroupLayouts?.Length ?? 0);
            native_descriptor->bindGroupLayouts = MarshalUtils.SpanToPointer(stackalloc nint[descriptor.Value.BindGroupLayouts?.Length ?? 0]);
            for (int bindGroupLayouts_i = 0; bindGroupLayouts_i < (descriptor.Value.BindGroupLayouts?.Length ?? 0); bindGroupLayouts_i++)
            {
                native_descriptor->bindGroupLayouts[bindGroupLayouts_i] = descriptor.Value.BindGroupLayouts[bindGroupLayouts_i]?.NativeHandle ?? 0;
            }
        }
        var result = Native.wgpuDeviceCreatePipelineLayout(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public QuerySet CreateQuerySet(QuerySetDescriptor? descriptor)
    {
        QuerySetDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc QuerySetDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->type = descriptor.Value.Type;
            native_descriptor->count = descriptor.Value.Count;
        }
        var result = Native.wgpuDeviceCreateQuerySet(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public RenderBundleEncoder CreateRenderBundleEncoder(RenderBundleEncoderDescriptor? descriptor)
    {
        fixed (TextureFormat* native_descriptor_colorFormats_ptr = descriptor?.ColorFormats)
        {
            RenderBundleEncoderDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc RenderBundleEncoderDescriptor.Native[descriptor.HasValue ? 1 : 0]);
            if (descriptor is not null)
            {
                native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
                native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
                native_descriptor->colorFormatCount = (ulong)(descriptor.Value.ColorFormats?.Length ?? 0);
                native_descriptor->colorFormats = native_descriptor_colorFormats_ptr;
                native_descriptor->depthStencilFormat = descriptor.Value.DepthStencilFormat;
                native_descriptor->sampleCount = descriptor.Value.SampleCount;
                native_descriptor->depthReadOnly = descriptor.Value.DepthReadOnly;
                native_descriptor->stencilReadOnly = descriptor.Value.StencilReadOnly;
            }
            var result = Native.wgpuDeviceCreateRenderBundleEncoder(this.NativeHandle, native_descriptor);
            if (descriptor is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
                MarshalUtils.FreeString(native_descriptor->label);
            }
            return new(result);
        }
    }
    
    public RenderPipeline CreateRenderPipeline(RenderPipelineDescriptor? descriptor)
    {
        RenderPipelineDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc RenderPipelineDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->layout = descriptor.Value.Layout?.NativeHandle ?? 0;
            native_descriptor->vertex = default;
            native_descriptor->vertex.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Vertex.NextInChain);
            native_descriptor->vertex.module = descriptor.Value.Vertex.Module?.NativeHandle ?? 0;
            native_descriptor->vertex.entryPoint = MarshalUtils.AllocString(descriptor.Value.Vertex.EntryPoint);
            native_descriptor->vertex.constantCount = (ulong)(descriptor.Value.Vertex.Constants?.Length ?? 0);
            native_descriptor->vertex.constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Vertex.Constants?.Length ?? 0]);
            for (int constants_i = 0; constants_i < (descriptor.Value.Vertex.Constants?.Length ?? 0); constants_i++)
            {
                native_descriptor->vertex.constants[constants_i] = default;
                native_descriptor->vertex.constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Vertex.Constants[constants_i].NextInChain);
                native_descriptor->vertex.constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Vertex.Constants[constants_i].Key);
                native_descriptor->vertex.constants[constants_i].value = descriptor.Value.Vertex.Constants[constants_i].Value;
            }
            native_descriptor->vertex.bufferCount = (ulong)(descriptor.Value.Vertex.Buffers?.Length ?? 0);
            native_descriptor->vertex.buffers = MarshalUtils.SpanToPointer(stackalloc VertexBufferLayout.Native[descriptor.Value.Vertex.Buffers?.Length ?? 0]);
            for (int buffers_i = 0; buffers_i < (descriptor.Value.Vertex.Buffers?.Length ?? 0); buffers_i++)
            {
                native_descriptor->vertex.buffers[buffers_i] = default;
                native_descriptor->vertex.buffers[buffers_i].arrayStride = descriptor.Value.Vertex.Buffers[buffers_i].ArrayStride;
                native_descriptor->vertex.buffers[buffers_i].stepMode = descriptor.Value.Vertex.Buffers[buffers_i].StepMode;
                native_descriptor->vertex.buffers[buffers_i].attributeCount = (ulong)(descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0);
                native_descriptor->vertex.buffers[buffers_i].attributes = MarshalUtils.SpanToPointer(stackalloc VertexAttribute.Native[descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0]);
                for (int attributes_i = 0; attributes_i < (descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0); attributes_i++)
                {
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i] = default;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].format = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].Format;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].offset = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].Offset;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].shaderLocation = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].ShaderLocation;
                }
            }
            native_descriptor->primitive = default;
            native_descriptor->primitive.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Primitive.NextInChain);
            native_descriptor->primitive.topology = descriptor.Value.Primitive.Topology;
            native_descriptor->primitive.stripIndexFormat = descriptor.Value.Primitive.StripIndexFormat;
            native_descriptor->primitive.frontFace = descriptor.Value.Primitive.FrontFace;
            native_descriptor->primitive.cullMode = descriptor.Value.Primitive.CullMode;
            native_descriptor->depthStencil = MarshalUtils.SpanToPointer(stackalloc DepthStencilState.Native[descriptor.Value.DepthStencil.HasValue ? 1 : 0]);
            if (descriptor.Value.DepthStencil is not null)
            {
                native_descriptor->depthStencil->nextInChain = MarshalUtils.AllocChain(descriptor.Value.DepthStencil.Value.NextInChain);
                native_descriptor->depthStencil->format = descriptor.Value.DepthStencil.Value.Format;
                native_descriptor->depthStencil->depthWriteEnabled = descriptor.Value.DepthStencil.Value.DepthWriteEnabled;
                native_descriptor->depthStencil->depthCompare = descriptor.Value.DepthStencil.Value.DepthCompare;
                native_descriptor->depthStencil->stencilFront = default;
                native_descriptor->depthStencil->stencilFront.compare = descriptor.Value.DepthStencil.Value.StencilFront.Compare;
                native_descriptor->depthStencil->stencilFront.failOp = descriptor.Value.DepthStencil.Value.StencilFront.FailOp;
                native_descriptor->depthStencil->stencilFront.depthFailOp = descriptor.Value.DepthStencil.Value.StencilFront.DepthFailOp;
                native_descriptor->depthStencil->stencilFront.passOp = descriptor.Value.DepthStencil.Value.StencilFront.PassOp;
                native_descriptor->depthStencil->stencilBack = default;
                native_descriptor->depthStencil->stencilBack.compare = descriptor.Value.DepthStencil.Value.StencilBack.Compare;
                native_descriptor->depthStencil->stencilBack.failOp = descriptor.Value.DepthStencil.Value.StencilBack.FailOp;
                native_descriptor->depthStencil->stencilBack.depthFailOp = descriptor.Value.DepthStencil.Value.StencilBack.DepthFailOp;
                native_descriptor->depthStencil->stencilBack.passOp = descriptor.Value.DepthStencil.Value.StencilBack.PassOp;
                native_descriptor->depthStencil->stencilReadMask = descriptor.Value.DepthStencil.Value.StencilReadMask;
                native_descriptor->depthStencil->stencilWriteMask = descriptor.Value.DepthStencil.Value.StencilWriteMask;
                native_descriptor->depthStencil->depthBias = descriptor.Value.DepthStencil.Value.DepthBias;
                native_descriptor->depthStencil->depthBiasSlopeScale = descriptor.Value.DepthStencil.Value.DepthBiasSlopeScale;
                native_descriptor->depthStencil->depthBiasClamp = descriptor.Value.DepthStencil.Value.DepthBiasClamp;
            }
            native_descriptor->multisample = default;
            native_descriptor->multisample.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Multisample.NextInChain);
            native_descriptor->multisample.count = descriptor.Value.Multisample.Count;
            native_descriptor->multisample.mask = descriptor.Value.Multisample.Mask;
            native_descriptor->multisample.alphaToCoverageEnabled = descriptor.Value.Multisample.AlphaToCoverageEnabled;
            native_descriptor->fragment = MarshalUtils.SpanToPointer(stackalloc FragmentState.Native[descriptor.Value.Fragment.HasValue ? 1 : 0]);
            if (descriptor.Value.Fragment is not null)
            {
                native_descriptor->fragment->nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.NextInChain);
                native_descriptor->fragment->module = descriptor.Value.Fragment.Value.Module?.NativeHandle ?? 0;
                native_descriptor->fragment->entryPoint = MarshalUtils.AllocString(descriptor.Value.Fragment.Value.EntryPoint);
                native_descriptor->fragment->constantCount = (ulong)(descriptor.Value.Fragment.Value.Constants?.Length ?? 0);
                native_descriptor->fragment->constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Fragment.Value.Constants?.Length ?? 0]);
                for (int constants_i = 0; constants_i < (descriptor.Value.Fragment.Value.Constants?.Length ?? 0); constants_i++)
                {
                    native_descriptor->fragment->constants[constants_i] = default;
                    native_descriptor->fragment->constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.Constants[constants_i].NextInChain);
                    native_descriptor->fragment->constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Fragment.Value.Constants[constants_i].Key);
                    native_descriptor->fragment->constants[constants_i].value = descriptor.Value.Fragment.Value.Constants[constants_i].Value;
                }
                native_descriptor->fragment->targetCount = (ulong)(descriptor.Value.Fragment.Value.Targets?.Length ?? 0);
                native_descriptor->fragment->targets = MarshalUtils.SpanToPointer(stackalloc ColorTargetState.Native[descriptor.Value.Fragment.Value.Targets?.Length ?? 0]);
                for (int targets_i = 0; targets_i < (descriptor.Value.Fragment.Value.Targets?.Length ?? 0); targets_i++)
                {
                    native_descriptor->fragment->targets[targets_i] = default;
                    native_descriptor->fragment->targets[targets_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.Targets[targets_i].NextInChain);
                    native_descriptor->fragment->targets[targets_i].format = descriptor.Value.Fragment.Value.Targets[targets_i].Format;
                    native_descriptor->fragment->targets[targets_i].blend = MarshalUtils.SpanToPointer(stackalloc BlendState.Native[descriptor.Value.Fragment.Value.Targets[targets_i].Blend.HasValue ? 1 : 0]);
                    if (descriptor.Value.Fragment.Value.Targets[targets_i].Blend is not null)
                    {
                        native_descriptor->fragment->targets[targets_i].blend->color = default;
                        native_descriptor->fragment->targets[targets_i].blend->color.operation = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.Operation;
                        native_descriptor->fragment->targets[targets_i].blend->color.srcFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.SrcFactor;
                        native_descriptor->fragment->targets[targets_i].blend->color.dstFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.DstFactor;
                        native_descriptor->fragment->targets[targets_i].blend->alpha = default;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.operation = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.Operation;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.srcFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.SrcFactor;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.dstFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.DstFactor;
                    }
                    native_descriptor->fragment->targets[targets_i].writeMask = descriptor.Value.Fragment.Value.Targets[targets_i].WriteMask;
                }
            }
        }
        var result = Native.wgpuDeviceCreateRenderPipeline(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            MarshalUtils.FreeChain(descriptor.Value.Vertex.NextInChain, native_descriptor->vertex.nextInChain);
            MarshalUtils.FreeString(native_descriptor->vertex.entryPoint);
            MarshalUtils.FreeChain(descriptor.Value.Primitive.NextInChain, native_descriptor->primitive.nextInChain);
            if (descriptor.Value.DepthStencil is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.DepthStencil.Value.NextInChain, native_descriptor->depthStencil->nextInChain);
            }
            MarshalUtils.FreeChain(descriptor.Value.Multisample.NextInChain, native_descriptor->multisample.nextInChain);
            if (descriptor.Value.Fragment is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.Fragment.Value.NextInChain, native_descriptor->fragment->nextInChain);
                MarshalUtils.FreeString(native_descriptor->fragment->entryPoint);
            }
        }
        return new(result);
    }
    
    public void CreateRenderPipelineAsync(RenderPipelineDescriptor? descriptor, CreateRenderPipelineAsyncCallback callback)
    {
        RenderPipelineDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc RenderPipelineDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->layout = descriptor.Value.Layout?.NativeHandle ?? 0;
            native_descriptor->vertex = default;
            native_descriptor->vertex.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Vertex.NextInChain);
            native_descriptor->vertex.module = descriptor.Value.Vertex.Module?.NativeHandle ?? 0;
            native_descriptor->vertex.entryPoint = MarshalUtils.AllocString(descriptor.Value.Vertex.EntryPoint);
            native_descriptor->vertex.constantCount = (ulong)(descriptor.Value.Vertex.Constants?.Length ?? 0);
            native_descriptor->vertex.constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Vertex.Constants?.Length ?? 0]);
            for (int constants_i = 0; constants_i < (descriptor.Value.Vertex.Constants?.Length ?? 0); constants_i++)
            {
                native_descriptor->vertex.constants[constants_i] = default;
                native_descriptor->vertex.constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Vertex.Constants[constants_i].NextInChain);
                native_descriptor->vertex.constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Vertex.Constants[constants_i].Key);
                native_descriptor->vertex.constants[constants_i].value = descriptor.Value.Vertex.Constants[constants_i].Value;
            }
            native_descriptor->vertex.bufferCount = (ulong)(descriptor.Value.Vertex.Buffers?.Length ?? 0);
            native_descriptor->vertex.buffers = MarshalUtils.SpanToPointer(stackalloc VertexBufferLayout.Native[descriptor.Value.Vertex.Buffers?.Length ?? 0]);
            for (int buffers_i = 0; buffers_i < (descriptor.Value.Vertex.Buffers?.Length ?? 0); buffers_i++)
            {
                native_descriptor->vertex.buffers[buffers_i] = default;
                native_descriptor->vertex.buffers[buffers_i].arrayStride = descriptor.Value.Vertex.Buffers[buffers_i].ArrayStride;
                native_descriptor->vertex.buffers[buffers_i].stepMode = descriptor.Value.Vertex.Buffers[buffers_i].StepMode;
                native_descriptor->vertex.buffers[buffers_i].attributeCount = (ulong)(descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0);
                native_descriptor->vertex.buffers[buffers_i].attributes = MarshalUtils.SpanToPointer(stackalloc VertexAttribute.Native[descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0]);
                for (int attributes_i = 0; attributes_i < (descriptor.Value.Vertex.Buffers[buffers_i].Attributes?.Length ?? 0); attributes_i++)
                {
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i] = default;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].format = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].Format;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].offset = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].Offset;
                    native_descriptor->vertex.buffers[buffers_i].attributes[attributes_i].shaderLocation = descriptor.Value.Vertex.Buffers[buffers_i].Attributes[attributes_i].ShaderLocation;
                }
            }
            native_descriptor->primitive = default;
            native_descriptor->primitive.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Primitive.NextInChain);
            native_descriptor->primitive.topology = descriptor.Value.Primitive.Topology;
            native_descriptor->primitive.stripIndexFormat = descriptor.Value.Primitive.StripIndexFormat;
            native_descriptor->primitive.frontFace = descriptor.Value.Primitive.FrontFace;
            native_descriptor->primitive.cullMode = descriptor.Value.Primitive.CullMode;
            native_descriptor->depthStencil = MarshalUtils.SpanToPointer(stackalloc DepthStencilState.Native[descriptor.Value.DepthStencil.HasValue ? 1 : 0]);
            if (descriptor.Value.DepthStencil is not null)
            {
                native_descriptor->depthStencil->nextInChain = MarshalUtils.AllocChain(descriptor.Value.DepthStencil.Value.NextInChain);
                native_descriptor->depthStencil->format = descriptor.Value.DepthStencil.Value.Format;
                native_descriptor->depthStencil->depthWriteEnabled = descriptor.Value.DepthStencil.Value.DepthWriteEnabled;
                native_descriptor->depthStencil->depthCompare = descriptor.Value.DepthStencil.Value.DepthCompare;
                native_descriptor->depthStencil->stencilFront = default;
                native_descriptor->depthStencil->stencilFront.compare = descriptor.Value.DepthStencil.Value.StencilFront.Compare;
                native_descriptor->depthStencil->stencilFront.failOp = descriptor.Value.DepthStencil.Value.StencilFront.FailOp;
                native_descriptor->depthStencil->stencilFront.depthFailOp = descriptor.Value.DepthStencil.Value.StencilFront.DepthFailOp;
                native_descriptor->depthStencil->stencilFront.passOp = descriptor.Value.DepthStencil.Value.StencilFront.PassOp;
                native_descriptor->depthStencil->stencilBack = default;
                native_descriptor->depthStencil->stencilBack.compare = descriptor.Value.DepthStencil.Value.StencilBack.Compare;
                native_descriptor->depthStencil->stencilBack.failOp = descriptor.Value.DepthStencil.Value.StencilBack.FailOp;
                native_descriptor->depthStencil->stencilBack.depthFailOp = descriptor.Value.DepthStencil.Value.StencilBack.DepthFailOp;
                native_descriptor->depthStencil->stencilBack.passOp = descriptor.Value.DepthStencil.Value.StencilBack.PassOp;
                native_descriptor->depthStencil->stencilReadMask = descriptor.Value.DepthStencil.Value.StencilReadMask;
                native_descriptor->depthStencil->stencilWriteMask = descriptor.Value.DepthStencil.Value.StencilWriteMask;
                native_descriptor->depthStencil->depthBias = descriptor.Value.DepthStencil.Value.DepthBias;
                native_descriptor->depthStencil->depthBiasSlopeScale = descriptor.Value.DepthStencil.Value.DepthBiasSlopeScale;
                native_descriptor->depthStencil->depthBiasClamp = descriptor.Value.DepthStencil.Value.DepthBiasClamp;
            }
            native_descriptor->multisample = default;
            native_descriptor->multisample.nextInChain = MarshalUtils.AllocChain(descriptor.Value.Multisample.NextInChain);
            native_descriptor->multisample.count = descriptor.Value.Multisample.Count;
            native_descriptor->multisample.mask = descriptor.Value.Multisample.Mask;
            native_descriptor->multisample.alphaToCoverageEnabled = descriptor.Value.Multisample.AlphaToCoverageEnabled;
            native_descriptor->fragment = MarshalUtils.SpanToPointer(stackalloc FragmentState.Native[descriptor.Value.Fragment.HasValue ? 1 : 0]);
            if (descriptor.Value.Fragment is not null)
            {
                native_descriptor->fragment->nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.NextInChain);
                native_descriptor->fragment->module = descriptor.Value.Fragment.Value.Module?.NativeHandle ?? 0;
                native_descriptor->fragment->entryPoint = MarshalUtils.AllocString(descriptor.Value.Fragment.Value.EntryPoint);
                native_descriptor->fragment->constantCount = (ulong)(descriptor.Value.Fragment.Value.Constants?.Length ?? 0);
                native_descriptor->fragment->constants = MarshalUtils.SpanToPointer(stackalloc ConstantEntry.Native[descriptor.Value.Fragment.Value.Constants?.Length ?? 0]);
                for (int constants_i = 0; constants_i < (descriptor.Value.Fragment.Value.Constants?.Length ?? 0); constants_i++)
                {
                    native_descriptor->fragment->constants[constants_i] = default;
                    native_descriptor->fragment->constants[constants_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.Constants[constants_i].NextInChain);
                    native_descriptor->fragment->constants[constants_i].key = MarshalUtils.AllocString(descriptor.Value.Fragment.Value.Constants[constants_i].Key);
                    native_descriptor->fragment->constants[constants_i].value = descriptor.Value.Fragment.Value.Constants[constants_i].Value;
                }
                native_descriptor->fragment->targetCount = (ulong)(descriptor.Value.Fragment.Value.Targets?.Length ?? 0);
                native_descriptor->fragment->targets = MarshalUtils.SpanToPointer(stackalloc ColorTargetState.Native[descriptor.Value.Fragment.Value.Targets?.Length ?? 0]);
                for (int targets_i = 0; targets_i < (descriptor.Value.Fragment.Value.Targets?.Length ?? 0); targets_i++)
                {
                    native_descriptor->fragment->targets[targets_i] = default;
                    native_descriptor->fragment->targets[targets_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Fragment.Value.Targets[targets_i].NextInChain);
                    native_descriptor->fragment->targets[targets_i].format = descriptor.Value.Fragment.Value.Targets[targets_i].Format;
                    native_descriptor->fragment->targets[targets_i].blend = MarshalUtils.SpanToPointer(stackalloc BlendState.Native[descriptor.Value.Fragment.Value.Targets[targets_i].Blend.HasValue ? 1 : 0]);
                    if (descriptor.Value.Fragment.Value.Targets[targets_i].Blend is not null)
                    {
                        native_descriptor->fragment->targets[targets_i].blend->color = default;
                        native_descriptor->fragment->targets[targets_i].blend->color.operation = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.Operation;
                        native_descriptor->fragment->targets[targets_i].blend->color.srcFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.SrcFactor;
                        native_descriptor->fragment->targets[targets_i].blend->color.dstFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Color.DstFactor;
                        native_descriptor->fragment->targets[targets_i].blend->alpha = default;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.operation = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.Operation;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.srcFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.SrcFactor;
                        native_descriptor->fragment->targets[targets_i].blend->alpha.dstFactor = descriptor.Value.Fragment.Value.Targets[targets_i].Blend.Value.Alpha.DstFactor;
                    }
                    native_descriptor->fragment->targets[targets_i].writeMask = descriptor.Value.Fragment.Value.Targets[targets_i].WriteMask;
                }
            }
        }
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_CreateRenderPipelineAsyncCallback(CreatePipelineAsyncStatus status, nint pipeline, byte* message, nint userdata)
        {
            RenderPipeline managed_pipeline = pipeline is 0 ? null : new(pipeline);
            string managed_message = MarshalUtils.StringFromPointer(message);
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            CreateRenderPipelineAsyncCallback callback = (CreateRenderPipelineAsyncCallback)handle.Target!;
            callback(status, managed_pipeline, managed_message);
            handle.Free();
        }
        GCHandle CreateRenderPipelineAsyncCallback_callbackHandle = GCHandle.Alloc(callback);
        nint CreateRenderPipelineAsyncCallback_userdata = GCHandle.ToIntPtr(CreateRenderPipelineAsyncCallback_callbackHandle);
        Native.wgpuDeviceCreateRenderPipelineAsync(this.NativeHandle, native_descriptor, &Native_CreateRenderPipelineAsyncCallback, CreateRenderPipelineAsyncCallback_userdata);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            MarshalUtils.FreeChain(descriptor.Value.Vertex.NextInChain, native_descriptor->vertex.nextInChain);
            MarshalUtils.FreeString(native_descriptor->vertex.entryPoint);
            MarshalUtils.FreeChain(descriptor.Value.Primitive.NextInChain, native_descriptor->primitive.nextInChain);
            if (descriptor.Value.DepthStencil is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.DepthStencil.Value.NextInChain, native_descriptor->depthStencil->nextInChain);
            }
            MarshalUtils.FreeChain(descriptor.Value.Multisample.NextInChain, native_descriptor->multisample.nextInChain);
            if (descriptor.Value.Fragment is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.Fragment.Value.NextInChain, native_descriptor->fragment->nextInChain);
                MarshalUtils.FreeString(native_descriptor->fragment->entryPoint);
            }
        }
    }
    
    public Sampler CreateSampler(SamplerDescriptor? descriptor)
    {
        SamplerDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc SamplerDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->addressModeU = descriptor.Value.AddressModeU;
            native_descriptor->addressModeV = descriptor.Value.AddressModeV;
            native_descriptor->addressModeW = descriptor.Value.AddressModeW;
            native_descriptor->magFilter = descriptor.Value.MagFilter;
            native_descriptor->minFilter = descriptor.Value.MinFilter;
            native_descriptor->mipmapFilter = descriptor.Value.MipmapFilter;
            native_descriptor->lodMinClamp = descriptor.Value.LodMinClamp;
            native_descriptor->lodMaxClamp = descriptor.Value.LodMaxClamp;
            native_descriptor->compare = descriptor.Value.Compare;
            native_descriptor->maxAnisotropy = descriptor.Value.MaxAnisotropy;
        }
        var result = Native.wgpuDeviceCreateSampler(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public ShaderModule CreateShaderModule(ShaderModuleDescriptor? descriptor)
    {
        ShaderModuleDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc ShaderModuleDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->hintCount = (ulong)(descriptor.Value.Hints?.Length ?? 0);
            native_descriptor->hints = MarshalUtils.SpanToPointer(stackalloc ShaderModuleCompilationHint.Native[descriptor.Value.Hints?.Length ?? 0]);
            for (int hints_i = 0; hints_i < (descriptor.Value.Hints?.Length ?? 0); hints_i++)
            {
                native_descriptor->hints[hints_i] = default;
                native_descriptor->hints[hints_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.Hints[hints_i].NextInChain);
                native_descriptor->hints[hints_i].entryPoint = MarshalUtils.AllocString(descriptor.Value.Hints[hints_i].EntryPoint);
                native_descriptor->hints[hints_i].layout = descriptor.Value.Hints[hints_i].Layout?.NativeHandle ?? 0;
            }
        }
        var result = Native.wgpuDeviceCreateShaderModule(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public Texture CreateTexture(TextureDescriptor? descriptor)
    {
        fixed (TextureFormat* native_descriptor_viewFormats_ptr = descriptor?.ViewFormats)
        {
            TextureDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc TextureDescriptor.Native[descriptor.HasValue ? 1 : 0]);
            if (descriptor is not null)
            {
                native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
                native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
                native_descriptor->usage = descriptor.Value.Usage;
                native_descriptor->dimension = descriptor.Value.Dimension;
                native_descriptor->size = default;
                native_descriptor->size.width = descriptor.Value.Size.Width;
                native_descriptor->size.height = descriptor.Value.Size.Height;
                native_descriptor->size.depthOrArrayLayers = descriptor.Value.Size.DepthOrArrayLayers;
                native_descriptor->format = descriptor.Value.Format;
                native_descriptor->mipLevelCount = descriptor.Value.MipLevelCount;
                native_descriptor->sampleCount = descriptor.Value.SampleCount;
                native_descriptor->viewFormatCount = (ulong)(descriptor.Value.ViewFormats?.Length ?? 0);
                native_descriptor->viewFormats = native_descriptor_viewFormats_ptr;
            }
            var result = Native.wgpuDeviceCreateTexture(this.NativeHandle, native_descriptor);
            if (descriptor is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
                MarshalUtils.FreeString(native_descriptor->label);
            }
            return new(result);
        }
    }
    
    public void Destroy()
    {
        Native.wgpuDeviceDestroy(this.NativeHandle);
    }
    
    public bool GetLimits(out SupportedLimits limits)
    {
        SupportedLimits.Native* native_limits = MarshalUtils.SpanToPointer(stackalloc SupportedLimits.Native[1]);
        var result = Native.wgpuDeviceGetLimits(this.NativeHandle, native_limits);
        limits = default;
        if (native_limits is not null)
        {
            SupportedLimits limits_value = default;
            limits_value.Limits = default;
            limits_value.Limits.MaxTextureDimension1D = native_limits->limits.maxTextureDimension1D;
            limits_value.Limits.MaxTextureDimension2D = native_limits->limits.maxTextureDimension2D;
            limits_value.Limits.MaxTextureDimension3D = native_limits->limits.maxTextureDimension3D;
            limits_value.Limits.MaxTextureArrayLayers = native_limits->limits.maxTextureArrayLayers;
            limits_value.Limits.MaxBindGroups = native_limits->limits.maxBindGroups;
            limits_value.Limits.MaxBindGroupsPlusVertexBuffers = native_limits->limits.maxBindGroupsPlusVertexBuffers;
            limits_value.Limits.MaxBindingsPerBindGroup = native_limits->limits.maxBindingsPerBindGroup;
            limits_value.Limits.MaxDynamicUniformBuffersPerPipelineLayout = native_limits->limits.maxDynamicUniformBuffersPerPipelineLayout;
            limits_value.Limits.MaxDynamicStorageBuffersPerPipelineLayout = native_limits->limits.maxDynamicStorageBuffersPerPipelineLayout;
            limits_value.Limits.MaxSampledTexturesPerShaderStage = native_limits->limits.maxSampledTexturesPerShaderStage;
            limits_value.Limits.MaxSamplersPerShaderStage = native_limits->limits.maxSamplersPerShaderStage;
            limits_value.Limits.MaxStorageBuffersPerShaderStage = native_limits->limits.maxStorageBuffersPerShaderStage;
            limits_value.Limits.MaxStorageTexturesPerShaderStage = native_limits->limits.maxStorageTexturesPerShaderStage;
            limits_value.Limits.MaxUniformBuffersPerShaderStage = native_limits->limits.maxUniformBuffersPerShaderStage;
            limits_value.Limits.MaxUniformBufferBindingSize = native_limits->limits.maxUniformBufferBindingSize;
            limits_value.Limits.MaxStorageBufferBindingSize = native_limits->limits.maxStorageBufferBindingSize;
            limits_value.Limits.MinUniformBufferOffsetAlignment = native_limits->limits.minUniformBufferOffsetAlignment;
            limits_value.Limits.MinStorageBufferOffsetAlignment = native_limits->limits.minStorageBufferOffsetAlignment;
            limits_value.Limits.MaxVertexBuffers = native_limits->limits.maxVertexBuffers;
            limits_value.Limits.MaxBufferSize = native_limits->limits.maxBufferSize;
            limits_value.Limits.MaxVertexAttributes = native_limits->limits.maxVertexAttributes;
            limits_value.Limits.MaxVertexBufferArrayStride = native_limits->limits.maxVertexBufferArrayStride;
            limits_value.Limits.MaxInterStageShaderComponents = native_limits->limits.maxInterStageShaderComponents;
            limits_value.Limits.MaxInterStageShaderVariables = native_limits->limits.maxInterStageShaderVariables;
            limits_value.Limits.MaxColorAttachments = native_limits->limits.maxColorAttachments;
            limits_value.Limits.MaxColorAttachmentBytesPerSample = native_limits->limits.maxColorAttachmentBytesPerSample;
            limits_value.Limits.MaxComputeWorkgroupStorageSize = native_limits->limits.maxComputeWorkgroupStorageSize;
            limits_value.Limits.MaxComputeInvocationsPerWorkgroup = native_limits->limits.maxComputeInvocationsPerWorkgroup;
            limits_value.Limits.MaxComputeWorkgroupSizeX = native_limits->limits.maxComputeWorkgroupSizeX;
            limits_value.Limits.MaxComputeWorkgroupSizeY = native_limits->limits.maxComputeWorkgroupSizeY;
            limits_value.Limits.MaxComputeWorkgroupSizeZ = native_limits->limits.maxComputeWorkgroupSizeZ;
            limits_value.Limits.MaxComputeWorkgroupsPerDimension = native_limits->limits.maxComputeWorkgroupsPerDimension;
            limits = limits_value;
        }
        return result;
    }
    
    public Queue Queue
    {
        get
        {
            var result = Native.wgpuDeviceGetQueue(this.NativeHandle);
            return new(result);
        }
    }
    
    public bool HasFeature(FeatureName feature)
    {
        var result = Native.wgpuDeviceHasFeature(this.NativeHandle, feature);
        return result;
    }
    
    public void PopErrorScope(ErrorCallback callback)
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_ErrorCallback(ErrorType type, byte* message, nint userdata)
        {
            string managed_message = MarshalUtils.StringFromPointer(message);
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            ErrorCallback callback = (ErrorCallback)handle.Target!;
            callback(type, managed_message);
            handle.Free();
        }
        GCHandle ErrorCallback_callbackHandle = GCHandle.Alloc(callback);
        nint ErrorCallback_userdata = GCHandle.ToIntPtr(ErrorCallback_callbackHandle);
        Native.wgpuDevicePopErrorScope(this.NativeHandle, &Native_ErrorCallback, ErrorCallback_userdata);
    }
    
    public void PushErrorScope(ErrorFilter filter)
    {
        Native.wgpuDevicePushErrorScope(this.NativeHandle, filter);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuDeviceSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void SetUncapturedErrorCallback(ErrorCallback callback)
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_ErrorCallback(ErrorType type, byte* message, nint userdata)
        {
            string managed_message = MarshalUtils.StringFromPointer(message);
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            ErrorCallback callback = (ErrorCallback)handle.Target!;
            callback(type, managed_message);
            handle.Free();
        }
        GCHandle ErrorCallback_callbackHandle = GCHandle.Alloc(callback);
        nint ErrorCallback_userdata = GCHandle.ToIntPtr(ErrorCallback_callbackHandle);
        Native.wgpuDeviceSetUncapturedErrorCallback(this.NativeHandle, &Native_ErrorCallback, ErrorCallback_userdata);
    }
    
    public void Reference()
    {
        Native.wgpuDeviceReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuDeviceRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class ComputePipeline : GraphicsObject
{
    public ComputePipeline(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public BindGroupLayout GetBindGroupLayout(uint groupIndex)
    {
        var result = Native.wgpuComputePipelineGetBindGroupLayout(this.NativeHandle, groupIndex);
        return new(result);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuComputePipelineSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuComputePipelineReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuComputePipelineRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class ComputePassEncoder : GraphicsObject
{
    public ComputePassEncoder(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void DispatchWorkgroups(uint workgroupCountX, uint workgroupCountY, uint workgroupCountZ)
    {
        Native.wgpuComputePassEncoderDispatchWorkgroups(this.NativeHandle, workgroupCountX, workgroupCountY, workgroupCountZ);
    }
    
    public void DispatchWorkgroupsIndirect(Buffer indirectBuffer, ulong indirectOffset)
    {
        nint native_indirectBuffer = indirectBuffer?.NativeHandle ?? 0;
        Native.wgpuComputePassEncoderDispatchWorkgroupsIndirect(this.NativeHandle, native_indirectBuffer, indirectOffset);
    }
    
    public void End()
    {
        Native.wgpuComputePassEncoderEnd(this.NativeHandle);
    }
    
    public void InsertDebugMarker(string markerLabel)
    {
        byte* native_markerLabel = MarshalUtils.AllocString(markerLabel);
        Native.wgpuComputePassEncoderInsertDebugMarker(this.NativeHandle, native_markerLabel);
        MarshalUtils.FreeString(native_markerLabel);
    }
    
    public void PopDebugGroup()
    {
        Native.wgpuComputePassEncoderPopDebugGroup(this.NativeHandle);
    }
    
    public void PushDebugGroup(string groupLabel)
    {
        byte* native_groupLabel = MarshalUtils.AllocString(groupLabel);
        Native.wgpuComputePassEncoderPushDebugGroup(this.NativeHandle, native_groupLabel);
        MarshalUtils.FreeString(native_groupLabel);
    }
    
    public void SetBindGroup(uint groupIndex, BindGroup group, uint[] dynamicOffsets)
    {
        fixed (uint* native_dynamicOffsets_ptr = dynamicOffsets)
        {
            nint native_group = group?.NativeHandle ?? 0;
            uint* native_dynamicOffsets = native_dynamicOffsets_ptr;
            Native.wgpuComputePassEncoderSetBindGroup(this.NativeHandle, groupIndex, native_group, (nuint)dynamicOffsets.Length, native_dynamicOffsets);
        }
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuComputePassEncoderSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void SetPipeline(ComputePipeline pipeline)
    {
        nint native_pipeline = pipeline?.NativeHandle ?? 0;
        Native.wgpuComputePassEncoderSetPipeline(this.NativeHandle, native_pipeline);
    }
    
    public void Reference()
    {
        Native.wgpuComputePassEncoderReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuComputePassEncoderRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class CommandEncoder : GraphicsObject
{
    public CommandEncoder(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public ComputePassEncoder BeginComputePass(ComputePassDescriptor? descriptor)
    {
        ComputePassDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc ComputePassDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->timestampWrites = MarshalUtils.SpanToPointer(stackalloc ComputePassTimestampWrites.Native[descriptor.Value.TimestampWrites.HasValue ? 1 : 0]);
            if (descriptor.Value.TimestampWrites is not null)
            {
                native_descriptor->timestampWrites->querySet = descriptor.Value.TimestampWrites.Value.QuerySet?.NativeHandle ?? 0;
                native_descriptor->timestampWrites->beginningOfPassWriteIndex = descriptor.Value.TimestampWrites.Value.BeginningOfPassWriteIndex;
                native_descriptor->timestampWrites->endOfPassWriteIndex = descriptor.Value.TimestampWrites.Value.EndOfPassWriteIndex;
            }
        }
        var result = Native.wgpuCommandEncoderBeginComputePass(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            if (descriptor.Value.TimestampWrites is not null)
            {
            }
        }
        return new(result);
    }
    
    public RenderPassEncoder BeginRenderPass(RenderPassDescriptor? descriptor)
    {
        RenderPassDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc RenderPassDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
            native_descriptor->colorAttachmentCount = (ulong)(descriptor.Value.ColorAttachments?.Length ?? 0);
            native_descriptor->colorAttachments = MarshalUtils.SpanToPointer(stackalloc RenderPassColorAttachment.Native[descriptor.Value.ColorAttachments?.Length ?? 0]);
            for (int colorAttachments_i = 0; colorAttachments_i < (descriptor.Value.ColorAttachments?.Length ?? 0); colorAttachments_i++)
            {
                native_descriptor->colorAttachments[colorAttachments_i] = default;
                native_descriptor->colorAttachments[colorAttachments_i].nextInChain = MarshalUtils.AllocChain(descriptor.Value.ColorAttachments[colorAttachments_i].NextInChain);
                native_descriptor->colorAttachments[colorAttachments_i].view = descriptor.Value.ColorAttachments[colorAttachments_i].View?.NativeHandle ?? 0;
                native_descriptor->colorAttachments[colorAttachments_i].resolveTarget = descriptor.Value.ColorAttachments[colorAttachments_i].ResolveTarget?.NativeHandle ?? 0;
                native_descriptor->colorAttachments[colorAttachments_i].loadOp = descriptor.Value.ColorAttachments[colorAttachments_i].LoadOp;
                native_descriptor->colorAttachments[colorAttachments_i].storeOp = descriptor.Value.ColorAttachments[colorAttachments_i].StoreOp;
                native_descriptor->colorAttachments[colorAttachments_i].clearValue = default;
                native_descriptor->colorAttachments[colorAttachments_i].clearValue.r = descriptor.Value.ColorAttachments[colorAttachments_i].ClearValue.R;
                native_descriptor->colorAttachments[colorAttachments_i].clearValue.g = descriptor.Value.ColorAttachments[colorAttachments_i].ClearValue.G;
                native_descriptor->colorAttachments[colorAttachments_i].clearValue.b = descriptor.Value.ColorAttachments[colorAttachments_i].ClearValue.B;
                native_descriptor->colorAttachments[colorAttachments_i].clearValue.a = descriptor.Value.ColorAttachments[colorAttachments_i].ClearValue.A;
            }
            native_descriptor->depthStencilAttachment = MarshalUtils.SpanToPointer(stackalloc RenderPassDepthStencilAttachment.Native[descriptor.Value.DepthStencilAttachment.HasValue ? 1 : 0]);
            if (descriptor.Value.DepthStencilAttachment is not null)
            {
                native_descriptor->depthStencilAttachment->view = descriptor.Value.DepthStencilAttachment.Value.View?.NativeHandle ?? 0;
                native_descriptor->depthStencilAttachment->depthLoadOp = descriptor.Value.DepthStencilAttachment.Value.DepthLoadOp;
                native_descriptor->depthStencilAttachment->depthStoreOp = descriptor.Value.DepthStencilAttachment.Value.DepthStoreOp;
                native_descriptor->depthStencilAttachment->depthClearValue = descriptor.Value.DepthStencilAttachment.Value.DepthClearValue;
                native_descriptor->depthStencilAttachment->depthReadOnly = descriptor.Value.DepthStencilAttachment.Value.DepthReadOnly;
                native_descriptor->depthStencilAttachment->stencilLoadOp = descriptor.Value.DepthStencilAttachment.Value.StencilLoadOp;
                native_descriptor->depthStencilAttachment->stencilStoreOp = descriptor.Value.DepthStencilAttachment.Value.StencilStoreOp;
                native_descriptor->depthStencilAttachment->stencilClearValue = descriptor.Value.DepthStencilAttachment.Value.StencilClearValue;
                native_descriptor->depthStencilAttachment->stencilReadOnly = descriptor.Value.DepthStencilAttachment.Value.StencilReadOnly;
            }
            native_descriptor->occlusionQuerySet = descriptor.Value.OcclusionQuerySet?.NativeHandle ?? 0;
            native_descriptor->timestampWrites = MarshalUtils.SpanToPointer(stackalloc RenderPassTimestampWrites.Native[descriptor.Value.TimestampWrites.HasValue ? 1 : 0]);
            if (descriptor.Value.TimestampWrites is not null)
            {
                native_descriptor->timestampWrites->querySet = descriptor.Value.TimestampWrites.Value.QuerySet?.NativeHandle ?? 0;
                native_descriptor->timestampWrites->beginningOfPassWriteIndex = descriptor.Value.TimestampWrites.Value.BeginningOfPassWriteIndex;
                native_descriptor->timestampWrites->endOfPassWriteIndex = descriptor.Value.TimestampWrites.Value.EndOfPassWriteIndex;
            }
        }
        var result = Native.wgpuCommandEncoderBeginRenderPass(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
            if (descriptor.Value.DepthStencilAttachment is not null)
            {
            }
            if (descriptor.Value.TimestampWrites is not null)
            {
            }
        }
        return new(result);
    }
    
    public void ClearBuffer(Buffer buffer, ulong offset, ulong size)
    {
        nint native_buffer = buffer?.NativeHandle ?? 0;
        Native.wgpuCommandEncoderClearBuffer(this.NativeHandle, native_buffer, offset, size);
    }
    
    public void CopyBufferToBuffer(Buffer source, ulong sourceOffset, Buffer destination, ulong destinationOffset, ulong size)
    {
        nint native_source = source?.NativeHandle ?? 0;
        nint native_destination = destination?.NativeHandle ?? 0;
        Native.wgpuCommandEncoderCopyBufferToBuffer(this.NativeHandle, native_source, sourceOffset, native_destination, destinationOffset, size);
    }
    
    public void CopyBufferToTexture(ImageCopyBuffer? source, ImageCopyTexture? destination, Extent3D? copySize)
    {
        ImageCopyBuffer.Native* native_source = MarshalUtils.SpanToPointer(stackalloc ImageCopyBuffer.Native[source.HasValue ? 1 : 0]);
        if (source is not null)
        {
            native_source->nextInChain = MarshalUtils.AllocChain(source.Value.NextInChain);
            native_source->layout = default;
            native_source->layout.nextInChain = MarshalUtils.AllocChain(source.Value.Layout.NextInChain);
            native_source->layout.offset = source.Value.Layout.Offset;
            native_source->layout.bytesPerRow = source.Value.Layout.BytesPerRow;
            native_source->layout.rowsPerImage = source.Value.Layout.RowsPerImage;
            native_source->buffer = source.Value.Buffer?.NativeHandle ?? 0;
        }
        ImageCopyTexture.Native* native_destination = MarshalUtils.SpanToPointer(stackalloc ImageCopyTexture.Native[destination.HasValue ? 1 : 0]);
        if (destination is not null)
        {
            native_destination->nextInChain = MarshalUtils.AllocChain(destination.Value.NextInChain);
            native_destination->texture = destination.Value.Texture?.NativeHandle ?? 0;
            native_destination->mipLevel = destination.Value.MipLevel;
            native_destination->origin = default;
            native_destination->origin.x = destination.Value.Origin.X;
            native_destination->origin.y = destination.Value.Origin.Y;
            native_destination->origin.z = destination.Value.Origin.Z;
            native_destination->aspect = destination.Value.Aspect;
        }
        Extent3D.Native* native_copySize = MarshalUtils.SpanToPointer(stackalloc Extent3D.Native[copySize.HasValue ? 1 : 0]);
        if (copySize is not null)
        {
            native_copySize->width = copySize.Value.Width;
            native_copySize->height = copySize.Value.Height;
            native_copySize->depthOrArrayLayers = copySize.Value.DepthOrArrayLayers;
        }
        Native.wgpuCommandEncoderCopyBufferToTexture(this.NativeHandle, native_source, native_destination, native_copySize);
        if (source is not null)
        {
            MarshalUtils.FreeChain(source.Value.NextInChain, native_source->nextInChain);
            MarshalUtils.FreeChain(source.Value.Layout.NextInChain, native_source->layout.nextInChain);
        }
        if (destination is not null)
        {
            MarshalUtils.FreeChain(destination.Value.NextInChain, native_destination->nextInChain);
        }
        if (copySize is not null)
        {
        }
    }
    
    public void CopyTextureToBuffer(ImageCopyTexture? source, ImageCopyBuffer? destination, Extent3D? copySize)
    {
        ImageCopyTexture.Native* native_source = MarshalUtils.SpanToPointer(stackalloc ImageCopyTexture.Native[source.HasValue ? 1 : 0]);
        if (source is not null)
        {
            native_source->nextInChain = MarshalUtils.AllocChain(source.Value.NextInChain);
            native_source->texture = source.Value.Texture?.NativeHandle ?? 0;
            native_source->mipLevel = source.Value.MipLevel;
            native_source->origin = default;
            native_source->origin.x = source.Value.Origin.X;
            native_source->origin.y = source.Value.Origin.Y;
            native_source->origin.z = source.Value.Origin.Z;
            native_source->aspect = source.Value.Aspect;
        }
        ImageCopyBuffer.Native* native_destination = MarshalUtils.SpanToPointer(stackalloc ImageCopyBuffer.Native[destination.HasValue ? 1 : 0]);
        if (destination is not null)
        {
            native_destination->nextInChain = MarshalUtils.AllocChain(destination.Value.NextInChain);
            native_destination->layout = default;
            native_destination->layout.nextInChain = MarshalUtils.AllocChain(destination.Value.Layout.NextInChain);
            native_destination->layout.offset = destination.Value.Layout.Offset;
            native_destination->layout.bytesPerRow = destination.Value.Layout.BytesPerRow;
            native_destination->layout.rowsPerImage = destination.Value.Layout.RowsPerImage;
            native_destination->buffer = destination.Value.Buffer?.NativeHandle ?? 0;
        }
        Extent3D.Native* native_copySize = MarshalUtils.SpanToPointer(stackalloc Extent3D.Native[copySize.HasValue ? 1 : 0]);
        if (copySize is not null)
        {
            native_copySize->width = copySize.Value.Width;
            native_copySize->height = copySize.Value.Height;
            native_copySize->depthOrArrayLayers = copySize.Value.DepthOrArrayLayers;
        }
        Native.wgpuCommandEncoderCopyTextureToBuffer(this.NativeHandle, native_source, native_destination, native_copySize);
        if (source is not null)
        {
            MarshalUtils.FreeChain(source.Value.NextInChain, native_source->nextInChain);
        }
        if (destination is not null)
        {
            MarshalUtils.FreeChain(destination.Value.NextInChain, native_destination->nextInChain);
            MarshalUtils.FreeChain(destination.Value.Layout.NextInChain, native_destination->layout.nextInChain);
        }
        if (copySize is not null)
        {
        }
    }
    
    public void CopyTextureToTexture(ImageCopyTexture? source, ImageCopyTexture? destination, Extent3D? copySize)
    {
        ImageCopyTexture.Native* native_source = MarshalUtils.SpanToPointer(stackalloc ImageCopyTexture.Native[source.HasValue ? 1 : 0]);
        if (source is not null)
        {
            native_source->nextInChain = MarshalUtils.AllocChain(source.Value.NextInChain);
            native_source->texture = source.Value.Texture?.NativeHandle ?? 0;
            native_source->mipLevel = source.Value.MipLevel;
            native_source->origin = default;
            native_source->origin.x = source.Value.Origin.X;
            native_source->origin.y = source.Value.Origin.Y;
            native_source->origin.z = source.Value.Origin.Z;
            native_source->aspect = source.Value.Aspect;
        }
        ImageCopyTexture.Native* native_destination = MarshalUtils.SpanToPointer(stackalloc ImageCopyTexture.Native[destination.HasValue ? 1 : 0]);
        if (destination is not null)
        {
            native_destination->nextInChain = MarshalUtils.AllocChain(destination.Value.NextInChain);
            native_destination->texture = destination.Value.Texture?.NativeHandle ?? 0;
            native_destination->mipLevel = destination.Value.MipLevel;
            native_destination->origin = default;
            native_destination->origin.x = destination.Value.Origin.X;
            native_destination->origin.y = destination.Value.Origin.Y;
            native_destination->origin.z = destination.Value.Origin.Z;
            native_destination->aspect = destination.Value.Aspect;
        }
        Extent3D.Native* native_copySize = MarshalUtils.SpanToPointer(stackalloc Extent3D.Native[copySize.HasValue ? 1 : 0]);
        if (copySize is not null)
        {
            native_copySize->width = copySize.Value.Width;
            native_copySize->height = copySize.Value.Height;
            native_copySize->depthOrArrayLayers = copySize.Value.DepthOrArrayLayers;
        }
        Native.wgpuCommandEncoderCopyTextureToTexture(this.NativeHandle, native_source, native_destination, native_copySize);
        if (source is not null)
        {
            MarshalUtils.FreeChain(source.Value.NextInChain, native_source->nextInChain);
        }
        if (destination is not null)
        {
            MarshalUtils.FreeChain(destination.Value.NextInChain, native_destination->nextInChain);
        }
        if (copySize is not null)
        {
        }
    }
    
    public CommandBuffer Finish(CommandBufferDescriptor? descriptor)
    {
        CommandBufferDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc CommandBufferDescriptor.Native[descriptor.HasValue ? 1 : 0]);
        if (descriptor is not null)
        {
            native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
            native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
        }
        var result = Native.wgpuCommandEncoderFinish(this.NativeHandle, native_descriptor);
        if (descriptor is not null)
        {
            MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
            MarshalUtils.FreeString(native_descriptor->label);
        }
        return new(result);
    }
    
    public void InsertDebugMarker(string markerLabel)
    {
        byte* native_markerLabel = MarshalUtils.AllocString(markerLabel);
        Native.wgpuCommandEncoderInsertDebugMarker(this.NativeHandle, native_markerLabel);
        MarshalUtils.FreeString(native_markerLabel);
    }
    
    public void PopDebugGroup()
    {
        Native.wgpuCommandEncoderPopDebugGroup(this.NativeHandle);
    }
    
    public void PushDebugGroup(string groupLabel)
    {
        byte* native_groupLabel = MarshalUtils.AllocString(groupLabel);
        Native.wgpuCommandEncoderPushDebugGroup(this.NativeHandle, native_groupLabel);
        MarshalUtils.FreeString(native_groupLabel);
    }
    
    public void ResolveQuerySet(QuerySet querySet, uint firstQuery, uint queryCount, Buffer destination, ulong destinationOffset)
    {
        nint native_querySet = querySet?.NativeHandle ?? 0;
        nint native_destination = destination?.NativeHandle ?? 0;
        Native.wgpuCommandEncoderResolveQuerySet(this.NativeHandle, native_querySet, firstQuery, queryCount, native_destination, destinationOffset);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuCommandEncoderSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void WriteTimestamp(QuerySet querySet, uint queryIndex)
    {
        nint native_querySet = querySet?.NativeHandle ?? 0;
        Native.wgpuCommandEncoderWriteTimestamp(this.NativeHandle, native_querySet, queryIndex);
    }
    
    public void Reference()
    {
        Native.wgpuCommandEncoderReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuCommandEncoderRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class CommandBuffer : GraphicsObject
{
    public CommandBuffer(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuCommandBufferSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuCommandBufferReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuCommandBufferRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Buffer : GraphicsObject
{
    public Buffer(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void Destroy()
    {
        Native.wgpuBufferDestroy(this.NativeHandle);
    }
    
    public nint GetConstMappedRange(ulong offset, ulong size)
    {
        var result = Native.wgpuBufferGetConstMappedRange(this.NativeHandle, offset, size);
        return result;
    }
    
    public BufferMapState MapState
    {
        get
        {
            var result = Native.wgpuBufferGetMapState(this.NativeHandle);
            return result;
        }
    }
    
    public ulong Size
    {
        get
        {
            var result = Native.wgpuBufferGetSize(this.NativeHandle);
            return result;
        }
    }
    
    public BufferUsage Usage
    {
        get
        {
            var result = Native.wgpuBufferGetUsage(this.NativeHandle);
            return result;
        }
    }
    
    public void MapAsync(MapMode mode, ulong offset, ulong size, BufferMapCallback callback)
    {
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        static void Native_BufferMapCallback(BufferMapAsyncStatus status, nint userdata)
        {
            GCHandle handle = GCHandle.FromIntPtr(userdata);
            BufferMapCallback callback = (BufferMapCallback)handle.Target!;
            callback(status);
            handle.Free();
        }
        GCHandle BufferMapCallback_callbackHandle = GCHandle.Alloc(callback);
        nint BufferMapCallback_userdata = GCHandle.ToIntPtr(BufferMapCallback_callbackHandle);
        Native.wgpuBufferMapAsync(this.NativeHandle, mode, offset, size, &Native_BufferMapCallback, BufferMapCallback_userdata);
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuBufferSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Unmap()
    {
        Native.wgpuBufferUnmap(this.NativeHandle);
    }
    
    public void Reference()
    {
        Native.wgpuBufferReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuBufferRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class BindGroupLayout : GraphicsObject
{
    public BindGroupLayout(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuBindGroupLayoutSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuBindGroupLayoutReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuBindGroupLayoutRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class BindGroup : GraphicsObject
{
    public BindGroup(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public void SetLabel(string label)
    {
        byte* native_label = MarshalUtils.AllocString(label);
        Native.wgpuBindGroupSetLabel(this.NativeHandle, native_label);
        MarshalUtils.FreeString(native_label);
    }
    
    public void Reference()
    {
        Native.wgpuBindGroupReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuBindGroupRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

public unsafe partial class Adapter : GraphicsObject
{
    public Adapter(nint nativeHandle) : base(nativeHandle)
    {
    }
    
    public bool GetLimits(out SupportedLimits limits)
    {
        SupportedLimits.Native* native_limits = MarshalUtils.SpanToPointer(stackalloc SupportedLimits.Native[1]);
        var result = Native.wgpuAdapterGetLimits(this.NativeHandle, native_limits);
        limits = default;
        if (native_limits is not null)
        {
            SupportedLimits limits_value = default;
            limits_value.Limits = default;
            limits_value.Limits.MaxTextureDimension1D = native_limits->limits.maxTextureDimension1D;
            limits_value.Limits.MaxTextureDimension2D = native_limits->limits.maxTextureDimension2D;
            limits_value.Limits.MaxTextureDimension3D = native_limits->limits.maxTextureDimension3D;
            limits_value.Limits.MaxTextureArrayLayers = native_limits->limits.maxTextureArrayLayers;
            limits_value.Limits.MaxBindGroups = native_limits->limits.maxBindGroups;
            limits_value.Limits.MaxBindGroupsPlusVertexBuffers = native_limits->limits.maxBindGroupsPlusVertexBuffers;
            limits_value.Limits.MaxBindingsPerBindGroup = native_limits->limits.maxBindingsPerBindGroup;
            limits_value.Limits.MaxDynamicUniformBuffersPerPipelineLayout = native_limits->limits.maxDynamicUniformBuffersPerPipelineLayout;
            limits_value.Limits.MaxDynamicStorageBuffersPerPipelineLayout = native_limits->limits.maxDynamicStorageBuffersPerPipelineLayout;
            limits_value.Limits.MaxSampledTexturesPerShaderStage = native_limits->limits.maxSampledTexturesPerShaderStage;
            limits_value.Limits.MaxSamplersPerShaderStage = native_limits->limits.maxSamplersPerShaderStage;
            limits_value.Limits.MaxStorageBuffersPerShaderStage = native_limits->limits.maxStorageBuffersPerShaderStage;
            limits_value.Limits.MaxStorageTexturesPerShaderStage = native_limits->limits.maxStorageTexturesPerShaderStage;
            limits_value.Limits.MaxUniformBuffersPerShaderStage = native_limits->limits.maxUniformBuffersPerShaderStage;
            limits_value.Limits.MaxUniformBufferBindingSize = native_limits->limits.maxUniformBufferBindingSize;
            limits_value.Limits.MaxStorageBufferBindingSize = native_limits->limits.maxStorageBufferBindingSize;
            limits_value.Limits.MinUniformBufferOffsetAlignment = native_limits->limits.minUniformBufferOffsetAlignment;
            limits_value.Limits.MinStorageBufferOffsetAlignment = native_limits->limits.minStorageBufferOffsetAlignment;
            limits_value.Limits.MaxVertexBuffers = native_limits->limits.maxVertexBuffers;
            limits_value.Limits.MaxBufferSize = native_limits->limits.maxBufferSize;
            limits_value.Limits.MaxVertexAttributes = native_limits->limits.maxVertexAttributes;
            limits_value.Limits.MaxVertexBufferArrayStride = native_limits->limits.maxVertexBufferArrayStride;
            limits_value.Limits.MaxInterStageShaderComponents = native_limits->limits.maxInterStageShaderComponents;
            limits_value.Limits.MaxInterStageShaderVariables = native_limits->limits.maxInterStageShaderVariables;
            limits_value.Limits.MaxColorAttachments = native_limits->limits.maxColorAttachments;
            limits_value.Limits.MaxColorAttachmentBytesPerSample = native_limits->limits.maxColorAttachmentBytesPerSample;
            limits_value.Limits.MaxComputeWorkgroupStorageSize = native_limits->limits.maxComputeWorkgroupStorageSize;
            limits_value.Limits.MaxComputeInvocationsPerWorkgroup = native_limits->limits.maxComputeInvocationsPerWorkgroup;
            limits_value.Limits.MaxComputeWorkgroupSizeX = native_limits->limits.maxComputeWorkgroupSizeX;
            limits_value.Limits.MaxComputeWorkgroupSizeY = native_limits->limits.maxComputeWorkgroupSizeY;
            limits_value.Limits.MaxComputeWorkgroupSizeZ = native_limits->limits.maxComputeWorkgroupSizeZ;
            limits_value.Limits.MaxComputeWorkgroupsPerDimension = native_limits->limits.maxComputeWorkgroupsPerDimension;
            limits = limits_value;
        }
        return result;
    }
    
    public bool HasFeature(FeatureName feature)
    {
        var result = Native.wgpuAdapterHasFeature(this.NativeHandle, feature);
        return result;
    }
    
    public void RequestDevice(DeviceDescriptor? descriptor, RequestDeviceCallback callback)
    {
        fixed (FeatureName* native_descriptor_requiredFeatures_ptr = descriptor?.RequiredFeatures)
        {
            DeviceDescriptor.Native* native_descriptor = MarshalUtils.SpanToPointer(stackalloc DeviceDescriptor.Native[descriptor.HasValue ? 1 : 0]);
            if (descriptor is not null)
            {
                native_descriptor->nextInChain = MarshalUtils.AllocChain(descriptor.Value.NextInChain);
                native_descriptor->label = MarshalUtils.AllocString(descriptor.Value.Label);
                native_descriptor->requiredFeatureCount = (ulong)(descriptor.Value.RequiredFeatures?.Length ?? 0);
                native_descriptor->requiredFeatures = native_descriptor_requiredFeatures_ptr;
                native_descriptor->requiredLimits = MarshalUtils.SpanToPointer(stackalloc RequiredLimits.Native[descriptor.Value.RequiredLimits.HasValue ? 1 : 0]);
                if (descriptor.Value.RequiredLimits is not null)
                {
                    native_descriptor->requiredLimits->nextInChain = MarshalUtils.AllocChain(descriptor.Value.RequiredLimits.Value.NextInChain);
                    native_descriptor->requiredLimits->limits = default;
                    native_descriptor->requiredLimits->limits.maxTextureDimension1D = descriptor.Value.RequiredLimits.Value.Limits.MaxTextureDimension1D;
                    native_descriptor->requiredLimits->limits.maxTextureDimension2D = descriptor.Value.RequiredLimits.Value.Limits.MaxTextureDimension2D;
                    native_descriptor->requiredLimits->limits.maxTextureDimension3D = descriptor.Value.RequiredLimits.Value.Limits.MaxTextureDimension3D;
                    native_descriptor->requiredLimits->limits.maxTextureArrayLayers = descriptor.Value.RequiredLimits.Value.Limits.MaxTextureArrayLayers;
                    native_descriptor->requiredLimits->limits.maxBindGroups = descriptor.Value.RequiredLimits.Value.Limits.MaxBindGroups;
                    native_descriptor->requiredLimits->limits.maxBindGroupsPlusVertexBuffers = descriptor.Value.RequiredLimits.Value.Limits.MaxBindGroupsPlusVertexBuffers;
                    native_descriptor->requiredLimits->limits.maxBindingsPerBindGroup = descriptor.Value.RequiredLimits.Value.Limits.MaxBindingsPerBindGroup;
                    native_descriptor->requiredLimits->limits.maxDynamicUniformBuffersPerPipelineLayout = descriptor.Value.RequiredLimits.Value.Limits.MaxDynamicUniformBuffersPerPipelineLayout;
                    native_descriptor->requiredLimits->limits.maxDynamicStorageBuffersPerPipelineLayout = descriptor.Value.RequiredLimits.Value.Limits.MaxDynamicStorageBuffersPerPipelineLayout;
                    native_descriptor->requiredLimits->limits.maxSampledTexturesPerShaderStage = descriptor.Value.RequiredLimits.Value.Limits.MaxSampledTexturesPerShaderStage;
                    native_descriptor->requiredLimits->limits.maxSamplersPerShaderStage = descriptor.Value.RequiredLimits.Value.Limits.MaxSamplersPerShaderStage;
                    native_descriptor->requiredLimits->limits.maxStorageBuffersPerShaderStage = descriptor.Value.RequiredLimits.Value.Limits.MaxStorageBuffersPerShaderStage;
                    native_descriptor->requiredLimits->limits.maxStorageTexturesPerShaderStage = descriptor.Value.RequiredLimits.Value.Limits.MaxStorageTexturesPerShaderStage;
                    native_descriptor->requiredLimits->limits.maxUniformBuffersPerShaderStage = descriptor.Value.RequiredLimits.Value.Limits.MaxUniformBuffersPerShaderStage;
                    native_descriptor->requiredLimits->limits.maxUniformBufferBindingSize = descriptor.Value.RequiredLimits.Value.Limits.MaxUniformBufferBindingSize;
                    native_descriptor->requiredLimits->limits.maxStorageBufferBindingSize = descriptor.Value.RequiredLimits.Value.Limits.MaxStorageBufferBindingSize;
                    native_descriptor->requiredLimits->limits.minUniformBufferOffsetAlignment = descriptor.Value.RequiredLimits.Value.Limits.MinUniformBufferOffsetAlignment;
                    native_descriptor->requiredLimits->limits.minStorageBufferOffsetAlignment = descriptor.Value.RequiredLimits.Value.Limits.MinStorageBufferOffsetAlignment;
                    native_descriptor->requiredLimits->limits.maxVertexBuffers = descriptor.Value.RequiredLimits.Value.Limits.MaxVertexBuffers;
                    native_descriptor->requiredLimits->limits.maxBufferSize = descriptor.Value.RequiredLimits.Value.Limits.MaxBufferSize;
                    native_descriptor->requiredLimits->limits.maxVertexAttributes = descriptor.Value.RequiredLimits.Value.Limits.MaxVertexAttributes;
                    native_descriptor->requiredLimits->limits.maxVertexBufferArrayStride = descriptor.Value.RequiredLimits.Value.Limits.MaxVertexBufferArrayStride;
                    native_descriptor->requiredLimits->limits.maxInterStageShaderComponents = descriptor.Value.RequiredLimits.Value.Limits.MaxInterStageShaderComponents;
                    native_descriptor->requiredLimits->limits.maxInterStageShaderVariables = descriptor.Value.RequiredLimits.Value.Limits.MaxInterStageShaderVariables;
                    native_descriptor->requiredLimits->limits.maxColorAttachments = descriptor.Value.RequiredLimits.Value.Limits.MaxColorAttachments;
                    native_descriptor->requiredLimits->limits.maxColorAttachmentBytesPerSample = descriptor.Value.RequiredLimits.Value.Limits.MaxColorAttachmentBytesPerSample;
                    native_descriptor->requiredLimits->limits.maxComputeWorkgroupStorageSize = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeWorkgroupStorageSize;
                    native_descriptor->requiredLimits->limits.maxComputeInvocationsPerWorkgroup = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeInvocationsPerWorkgroup;
                    native_descriptor->requiredLimits->limits.maxComputeWorkgroupSizeX = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeWorkgroupSizeX;
                    native_descriptor->requiredLimits->limits.maxComputeWorkgroupSizeY = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeWorkgroupSizeY;
                    native_descriptor->requiredLimits->limits.maxComputeWorkgroupSizeZ = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeWorkgroupSizeZ;
                    native_descriptor->requiredLimits->limits.maxComputeWorkgroupsPerDimension = descriptor.Value.RequiredLimits.Value.Limits.MaxComputeWorkgroupsPerDimension;
                }
                native_descriptor->defaultQueue = default;
                native_descriptor->defaultQueue.nextInChain = MarshalUtils.AllocChain(descriptor.Value.DefaultQueue.NextInChain);
                native_descriptor->defaultQueue.label = MarshalUtils.AllocString(descriptor.Value.DefaultQueue.Label);
                [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
                static void Native_DeviceLostCallback(DeviceLostReason reason, byte* message, nint userdata)
                {
                    string managed_message = MarshalUtils.StringFromPointer(message);
                    GCHandle handle = GCHandle.FromIntPtr(userdata);
                    DeviceLostCallback callback = (DeviceLostCallback)handle.Target!;
                    callback(reason, managed_message);
                    handle.Free();
                }
                GCHandle DeviceLostCallback_callbackHandle = GCHandle.Alloc(descriptor.Value.DeviceLostCallback);
                nint DeviceLostCallback_userdata = GCHandle.ToIntPtr(DeviceLostCallback_callbackHandle);
                native_descriptor->deviceLostUserdata = DeviceLostCallback_userdata;
            }
            [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
            static void Native_RequestDeviceCallback(RequestDeviceStatus status, nint device, byte* message, nint userdata)
            {
                Device managed_device = device is 0 ? null : new(device);
                string managed_message = MarshalUtils.StringFromPointer(message);
                GCHandle handle = GCHandle.FromIntPtr(userdata);
                RequestDeviceCallback callback = (RequestDeviceCallback)handle.Target!;
                callback(status, managed_device, managed_message);
                handle.Free();
            }
            GCHandle RequestDeviceCallback_callbackHandle = GCHandle.Alloc(callback);
            nint RequestDeviceCallback_userdata = GCHandle.ToIntPtr(RequestDeviceCallback_callbackHandle);
            Native.wgpuAdapterRequestDevice(this.NativeHandle, native_descriptor, &Native_RequestDeviceCallback, RequestDeviceCallback_userdata);
            if (descriptor is not null)
            {
                MarshalUtils.FreeChain(descriptor.Value.NextInChain, native_descriptor->nextInChain);
                MarshalUtils.FreeString(native_descriptor->label);
                if (descriptor.Value.RequiredLimits is not null)
                {
                    MarshalUtils.FreeChain(descriptor.Value.RequiredLimits.Value.NextInChain, native_descriptor->requiredLimits->nextInChain);
                }
                MarshalUtils.FreeChain(descriptor.Value.DefaultQueue.NextInChain, native_descriptor->defaultQueue.nextInChain);
                MarshalUtils.FreeString(native_descriptor->defaultQueue.label);
            }
        }
    }
    
    public void Reference()
    {
        Native.wgpuAdapterReference(this.NativeHandle);
    }
    
    public void Release()
    {
        Native.wgpuAdapterRelease(this.NativeHandle);
    }
    
    public override void Dispose() 
    {
        this.Release();
        base.Dispose();
    }
    
}

internal unsafe partial class Native
{
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuCreateInstance(InstanceDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern ulong wgpuAdapterEnumerateFeatures(nint adapter, FeatureName* features);
    
    [DllImport(LIBRARY_NAME)]
    public static extern WGPUBool wgpuAdapterGetLimits(nint adapter, SupportedLimits.Native* limits);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuAdapterGetProperties(nint adapter, AdapterProperties.Native* properties);
    
    [DllImport(LIBRARY_NAME)]
    public static extern WGPUBool wgpuAdapterHasFeature(nint adapter, FeatureName feature);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuAdapterRequestDevice(nint adapter, DeviceDescriptor.Native* descriptor, delegate*unmanaged[Cdecl]<RequestDeviceStatus, nint, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuAdapterReference(nint adapter);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuAdapterRelease(nint adapter);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupSetLabel(nint bindGroup, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupReference(nint bindGroup);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupRelease(nint bindGroup);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupLayoutSetLabel(nint bindGroupLayout, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupLayoutReference(nint bindGroupLayout);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBindGroupLayoutRelease(nint bindGroupLayout);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferDestroy(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuBufferGetConstMappedRange(nint buffer, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern BufferMapState wgpuBufferGetMapState(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuBufferGetMappedRange(nint buffer, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern ulong wgpuBufferGetSize(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern BufferUsage wgpuBufferGetUsage(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferMapAsync(nint buffer, MapMode mode, ulong offset, ulong size, delegate*unmanaged[Cdecl]<BufferMapAsyncStatus, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferSetLabel(nint buffer, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferUnmap(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferReference(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuBufferRelease(nint buffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandBufferSetLabel(nint commandBuffer, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandBufferReference(nint commandBuffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandBufferRelease(nint commandBuffer);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuCommandEncoderBeginComputePass(nint commandEncoder, ComputePassDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuCommandEncoderBeginRenderPass(nint commandEncoder, RenderPassDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderClearBuffer(nint commandEncoder, nint buffer, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderCopyBufferToBuffer(nint commandEncoder, nint source, ulong sourceOffset, nint destination, ulong destinationOffset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderCopyBufferToTexture(nint commandEncoder, ImageCopyBuffer.Native* source, ImageCopyTexture.Native* destination, Extent3D.Native* copySize);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderCopyTextureToBuffer(nint commandEncoder, ImageCopyTexture.Native* source, ImageCopyBuffer.Native* destination, Extent3D.Native* copySize);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderCopyTextureToTexture(nint commandEncoder, ImageCopyTexture.Native* source, ImageCopyTexture.Native* destination, Extent3D.Native* copySize);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuCommandEncoderFinish(nint commandEncoder, CommandBufferDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderInsertDebugMarker(nint commandEncoder, byte* markerLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderPopDebugGroup(nint commandEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderPushDebugGroup(nint commandEncoder, byte* groupLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderResolveQuerySet(nint commandEncoder, nint querySet, uint firstQuery, uint queryCount, nint destination, ulong destinationOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderSetLabel(nint commandEncoder, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderWriteTimestamp(nint commandEncoder, nint querySet, uint queryIndex);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderReference(nint commandEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuCommandEncoderRelease(nint commandEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderDispatchWorkgroups(nint computePassEncoder, uint workgroupCountX, uint workgroupCountY, uint workgroupCountZ);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderDispatchWorkgroupsIndirect(nint computePassEncoder, nint indirectBuffer, ulong indirectOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderEnd(nint computePassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderInsertDebugMarker(nint computePassEncoder, byte* markerLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderPopDebugGroup(nint computePassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderPushDebugGroup(nint computePassEncoder, byte* groupLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderSetBindGroup(nint computePassEncoder, uint groupIndex, nint group, ulong dynamicOffsetCount, uint* dynamicOffsets);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderSetLabel(nint computePassEncoder, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderSetPipeline(nint computePassEncoder, nint pipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderReference(nint computePassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePassEncoderRelease(nint computePassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuComputePipelineGetBindGroupLayout(nint computePipeline, uint groupIndex);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePipelineSetLabel(nint computePipeline, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePipelineReference(nint computePipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuComputePipelineRelease(nint computePipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateBindGroup(nint device, BindGroupDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateBindGroupLayout(nint device, BindGroupLayoutDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateBuffer(nint device, BufferDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateCommandEncoder(nint device, CommandEncoderDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateComputePipeline(nint device, ComputePipelineDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceCreateComputePipelineAsync(nint device, ComputePipelineDescriptor.Native* descriptor, delegate*unmanaged[Cdecl]<CreatePipelineAsyncStatus, nint, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreatePipelineLayout(nint device, PipelineLayoutDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateQuerySet(nint device, QuerySetDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateRenderBundleEncoder(nint device, RenderBundleEncoderDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateRenderPipeline(nint device, RenderPipelineDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceCreateRenderPipelineAsync(nint device, RenderPipelineDescriptor.Native* descriptor, delegate*unmanaged[Cdecl]<CreatePipelineAsyncStatus, nint, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateSampler(nint device, SamplerDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateShaderModule(nint device, ShaderModuleDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceCreateTexture(nint device, TextureDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceDestroy(nint device);
    
    [DllImport(LIBRARY_NAME)]
    public static extern ulong wgpuDeviceEnumerateFeatures(nint device, FeatureName* features);
    
    [DllImport(LIBRARY_NAME)]
    public static extern WGPUBool wgpuDeviceGetLimits(nint device, SupportedLimits.Native* limits);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuDeviceGetQueue(nint device);
    
    [DllImport(LIBRARY_NAME)]
    public static extern WGPUBool wgpuDeviceHasFeature(nint device, FeatureName feature);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDevicePopErrorScope(nint device, delegate*unmanaged[Cdecl]<ErrorType, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDevicePushErrorScope(nint device, ErrorFilter filter);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceSetLabel(nint device, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceSetUncapturedErrorCallback(nint device, delegate*unmanaged[Cdecl]<ErrorType, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceReference(nint device);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuDeviceRelease(nint device);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuInstanceCreateSurface(nint instance, SurfaceDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuInstanceProcessEvents(nint instance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuInstanceRequestAdapter(nint instance, RequestAdapterOptions.Native* options, delegate*unmanaged[Cdecl]<RequestAdapterStatus, nint, byte*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuInstanceReference(nint instance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuInstanceRelease(nint instance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuPipelineLayoutSetLabel(nint pipelineLayout, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuPipelineLayoutReference(nint pipelineLayout);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuPipelineLayoutRelease(nint pipelineLayout);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQuerySetDestroy(nint querySet);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuQuerySetGetCount(nint querySet);
    
    [DllImport(LIBRARY_NAME)]
    public static extern QueryType wgpuQuerySetGetType(nint querySet);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQuerySetSetLabel(nint querySet, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQuerySetReference(nint querySet);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQuerySetRelease(nint querySet);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueOnSubmittedWorkDone(nint queue, delegate*unmanaged[Cdecl]<QueueWorkDoneStatus, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueSetLabel(nint queue, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueSubmit(nint queue, ulong commandCount, nint* commands);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueWriteBuffer(nint queue, nint buffer, ulong bufferOffset, nint data, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueWriteTexture(nint queue, ImageCopyTexture.Native* destination, byte* data, ulong dataSize, TextureDataLayout.Native* dataLayout, Extent3D.Native* writeSize);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueReference(nint queue);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuQueueRelease(nint queue);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleSetLabel(nint renderBundle, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleReference(nint renderBundle);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleRelease(nint renderBundle);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderDraw(nint renderBundleEncoder, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderDrawIndexed(nint renderBundleEncoder, uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderDrawIndexedIndirect(nint renderBundleEncoder, nint indirectBuffer, ulong indirectOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderDrawIndirect(nint renderBundleEncoder, nint indirectBuffer, ulong indirectOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuRenderBundleEncoderFinish(nint renderBundleEncoder, RenderBundleDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderInsertDebugMarker(nint renderBundleEncoder, byte* markerLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderPopDebugGroup(nint renderBundleEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderPushDebugGroup(nint renderBundleEncoder, byte* groupLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderSetBindGroup(nint renderBundleEncoder, uint groupIndex, nint group, ulong dynamicOffsetCount, uint* dynamicOffsets);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderSetIndexBuffer(nint renderBundleEncoder, nint buffer, IndexFormat format, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderSetLabel(nint renderBundleEncoder, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderSetPipeline(nint renderBundleEncoder, nint pipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderSetVertexBuffer(nint renderBundleEncoder, uint slot, nint buffer, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderReference(nint renderBundleEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderBundleEncoderRelease(nint renderBundleEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderBeginOcclusionQuery(nint renderPassEncoder, uint queryIndex);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderDraw(nint renderPassEncoder, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderDrawIndexed(nint renderPassEncoder, uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderDrawIndexedIndirect(nint renderPassEncoder, nint indirectBuffer, ulong indirectOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderDrawIndirect(nint renderPassEncoder, nint indirectBuffer, ulong indirectOffset);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderEnd(nint renderPassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderEndOcclusionQuery(nint renderPassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderExecuteBundles(nint renderPassEncoder, ulong bundleCount, nint* bundles);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderInsertDebugMarker(nint renderPassEncoder, byte* markerLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderPopDebugGroup(nint renderPassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderPushDebugGroup(nint renderPassEncoder, byte* groupLabel);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetBindGroup(nint renderPassEncoder, uint groupIndex, nint group, ulong dynamicOffsetCount, uint* dynamicOffsets);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetBlendConstant(nint renderPassEncoder, Color.Native* color);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetIndexBuffer(nint renderPassEncoder, nint buffer, IndexFormat format, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetLabel(nint renderPassEncoder, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetPipeline(nint renderPassEncoder, nint pipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetScissorRect(nint renderPassEncoder, uint x, uint y, uint width, uint height);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetStencilReference(nint renderPassEncoder, uint reference);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetVertexBuffer(nint renderPassEncoder, uint slot, nint buffer, ulong offset, ulong size);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderSetViewport(nint renderPassEncoder, float x, float y, float width, float height, float minDepth, float maxDepth);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderReference(nint renderPassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPassEncoderRelease(nint renderPassEncoder);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuRenderPipelineGetBindGroupLayout(nint renderPipeline, uint groupIndex);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPipelineSetLabel(nint renderPipeline, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPipelineReference(nint renderPipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuRenderPipelineRelease(nint renderPipeline);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSamplerSetLabel(nint sampler, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSamplerReference(nint sampler);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSamplerRelease(nint sampler);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuShaderModuleGetCompilationInfo(nint shaderModule, delegate*unmanaged[Cdecl]<CompilationInfoRequestStatus, CompilationInfo.Native*, nint, void> callback, nint userdata);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuShaderModuleSetLabel(nint shaderModule, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuShaderModuleReference(nint shaderModule);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuShaderModuleRelease(nint shaderModule);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceConfigure(nint surface, SurfaceConfiguration.Native* config);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceGetCapabilities(nint surface, nint adapter, SurfaceCapabilities.Native* capabilities);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceGetCurrentTexture(nint surface, SurfaceTexture.Native* surfaceTexture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern TextureFormat wgpuSurfaceGetPreferredFormat(nint surface, nint adapter);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfacePresent(nint surface);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceUnconfigure(nint surface);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceReference(nint surface);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceRelease(nint surface);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuSurfaceCapabilitiesFreeMembers(SurfaceCapabilities.Native capabilities);
    
    [DllImport(LIBRARY_NAME)]
    public static extern nint wgpuTextureCreateView(nint texture, TextureViewDescriptor.Native* descriptor);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureDestroy(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuTextureGetDepthOrArrayLayers(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern TextureDimension wgpuTextureGetDimension(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern TextureFormat wgpuTextureGetFormat(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuTextureGetHeight(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuTextureGetMipLevelCount(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuTextureGetSampleCount(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern TextureUsage wgpuTextureGetUsage(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern uint wgpuTextureGetWidth(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureSetLabel(nint texture, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureReference(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureRelease(nint texture);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureViewSetLabel(nint textureView, byte* label);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureViewReference(nint textureView);
    
    [DllImport(LIBRARY_NAME)]
    public static extern void wgpuTextureViewRelease(nint textureView);
}
