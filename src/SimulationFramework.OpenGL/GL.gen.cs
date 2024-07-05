using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable IDE0044 // Add readonly modifier

public static unsafe partial class OpenGL
{
    public delegate delegate*<void> GLFunctionLoader(string name);

    public static bool glInitialized = false;
    private static List<string> glExtensions;

    public static int Initialize(GLFunctionLoader functionLoader)
    {
        foreach (var field in typeof(OpenGL).GetFields(BindingFlags.Static | BindingFlags.NonPublic))
        {
            // only setup fields with 'pfn_' prefix
            if (!field.Name.StartsWith("pfn_"))
            {
                continue;
            }

            // remove the prefix
            var name = field.Name[4..];

            // get gl function pointer
            var pfn = functionLoader(name);

            if ((ulong)pfn == 0)
            {
                Console.WriteLine($"Unable to load OpenGl function '{name}'!");
                return 0;
            }

            field.SetValue(null, (IntPtr)pfn);
        }

        glInitialized = true;

        int len = 0;
        glGetIntegerv(GL_NUM_EXTENSIONS, &len);

        var exts = new List<string>(len);
        for (uint i = 0; i < len; i++)
        {
            exts.Add(Marshal.PtrToStringUTF8((IntPtr)glGetStringi(GL_EXTENSIONS, i)));
        }

        glExtensions = exts;

        return 1;
    }
    public const uint GL_VERSION_1_0 = 1;
    public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;
    public const uint GL_STENCIL_BUFFER_BIT = 0x00000400;
    public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
    public const uint GL_FALSE = 0;
    public const uint GL_TRUE = 1;
    public const uint GL_POINTS = 0x0000;
    public const uint GL_LINES = 0x0001;
    public const uint GL_LINE_LOOP = 0x0002;
    public const uint GL_LINE_STRIP = 0x0003;
    public const uint GL_TRIANGLES = 0x0004;
    public const uint GL_TRIANGLE_STRIP = 0x0005;
    public const uint GL_TRIANGLE_FAN = 0x0006;
    public const uint GL_QUADS = 0x0007;
    public const uint GL_NEVER = 0x0200;
    public const uint GL_LESS = 0x0201;
    public const uint GL_EQUAL = 0x0202;
    public const uint GL_LEQUAL = 0x0203;
    public const uint GL_GREATER = 0x0204;
    public const uint GL_NOTEQUAL = 0x0205;
    public const uint GL_GEQUAL = 0x0206;
    public const uint GL_ALWAYS = 0x0207;
    public const uint GL_ZERO = 0;
    public const uint GL_ONE = 1;
    public const uint GL_SRC_COLOR = 0x0300;
    public const uint GL_ONE_MINUS_SRC_COLOR = 0x0301;
    public const uint GL_SRC_ALPHA = 0x0302;
    public const uint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
    public const uint GL_DST_ALPHA = 0x0304;
    public const uint GL_ONE_MINUS_DST_ALPHA = 0x0305;
    public const uint GL_DST_COLOR = 0x0306;
    public const uint GL_ONE_MINUS_DST_COLOR = 0x0307;
    public const uint GL_SRC_ALPHA_SATURATE = 0x0308;
    public const uint GL_NONE = 0;
    public const uint GL_FRONT_LEFT = 0x0400;
    public const uint GL_FRONT_RIGHT = 0x0401;
    public const uint GL_BACK_LEFT = 0x0402;
    public const uint GL_BACK_RIGHT = 0x0403;
    public const uint GL_FRONT = 0x0404;
    public const uint GL_BACK = 0x0405;
    public const uint GL_LEFT = 0x0406;
    public const uint GL_RIGHT = 0x0407;
    public const uint GL_FRONT_AND_BACK = 0x0408;
    public const uint GL_NO_ERROR = 0;
    public const uint GL_INVALID_ENUM = 0x0500;
    public const uint GL_INVALID_VALUE = 0x0501;
    public const uint GL_INVALID_OPERATION = 0x0502;
    public const uint GL_OUT_OF_MEMORY = 0x0505;
    public const uint GL_CW = 0x0900;
    public const uint GL_CCW = 0x0901;
    public const uint GL_POINT_SIZE = 0x0B11;
    public const uint GL_POINT_SIZE_RANGE = 0x0B12;
    public const uint GL_POINT_SIZE_GRANULARITY = 0x0B13;
    public const uint GL_LINE_SMOOTH = 0x0B20;
    public const uint GL_LINE_WIDTH = 0x0B21;
    public const uint GL_LINE_WIDTH_RANGE = 0x0B22;
    public const uint GL_LINE_WIDTH_GRANULARITY = 0x0B23;
    public const uint GL_POLYGON_MODE = 0x0B40;
    public const uint GL_POLYGON_SMOOTH = 0x0B41;
    public const uint GL_CULL_FACE = 0x0B44;
    public const uint GL_CULL_FACE_MODE = 0x0B45;
    public const uint GL_FRONT_FACE = 0x0B46;
    public const uint GL_DEPTH_RANGE = 0x0B70;
    public const uint GL_DEPTH_TEST = 0x0B71;
    public const uint GL_DEPTH_WRITEMASK = 0x0B72;
    public const uint GL_DEPTH_CLEAR_VALUE = 0x0B73;
    public const uint GL_DEPTH_FUNC = 0x0B74;
    public const uint GL_STENCIL_TEST = 0x0B90;
    public const uint GL_STENCIL_CLEAR_VALUE = 0x0B91;
    public const uint GL_STENCIL_FUNC = 0x0B92;
    public const uint GL_STENCIL_VALUE_MASK = 0x0B93;
    public const uint GL_STENCIL_FAIL = 0x0B94;
    public const uint GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
    public const uint GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
    public const uint GL_STENCIL_REF = 0x0B97;
    public const uint GL_STENCIL_WRITEMASK = 0x0B98;
    public const uint GL_VIEWPORT = 0x0BA2;
    public const uint GL_DITHER = 0x0BD0;
    public const uint GL_BLEND_DST = 0x0BE0;
    public const uint GL_BLEND_SRC = 0x0BE1;
    public const uint GL_BLEND = 0x0BE2;
    public const uint GL_LOGIC_OP_MODE = 0x0BF0;
    public const uint GL_DRAW_BUFFER = 0x0C01;
    public const uint GL_READ_BUFFER = 0x0C02;
    public const uint GL_SCISSOR_BOX = 0x0C10;
    public const uint GL_SCISSOR_TEST = 0x0C11;
    public const uint GL_COLOR_CLEAR_VALUE = 0x0C22;
    public const uint GL_COLOR_WRITEMASK = 0x0C23;
    public const uint GL_DOUBLEBUFFER = 0x0C32;
    public const uint GL_STEREO = 0x0C33;
    public const uint GL_LINE_SMOOTH_HINT = 0x0C52;
    public const uint GL_POLYGON_SMOOTH_HINT = 0x0C53;
    public const uint GL_UNPACK_SWAP_BYTES = 0x0CF0;
    public const uint GL_UNPACK_LSB_FIRST = 0x0CF1;
    public const uint GL_UNPACK_ROW_LENGTH = 0x0CF2;
    public const uint GL_UNPACK_SKIP_ROWS = 0x0CF3;
    public const uint GL_UNPACK_SKIP_PIXELS = 0x0CF4;
    public const uint GL_UNPACK_ALIGNMENT = 0x0CF5;
    public const uint GL_PACK_SWAP_BYTES = 0x0D00;
    public const uint GL_PACK_LSB_FIRST = 0x0D01;
    public const uint GL_PACK_ROW_LENGTH = 0x0D02;
    public const uint GL_PACK_SKIP_ROWS = 0x0D03;
    public const uint GL_PACK_SKIP_PIXELS = 0x0D04;
    public const uint GL_PACK_ALIGNMENT = 0x0D05;
    public const uint GL_MAX_TEXTURE_SIZE = 0x0D33;
    public const uint GL_MAX_VIEWPORT_DIMS = 0x0D3A;
    public const uint GL_SUBPIXEL_BITS = 0x0D50;
    public const uint GL_TEXTURE_1D = 0x0DE0;
    public const uint GL_TEXTURE_2D = 0x0DE1;
    public const uint GL_TEXTURE_WIDTH = 0x1000;
    public const uint GL_TEXTURE_HEIGHT = 0x1001;
    public const uint GL_TEXTURE_BORDER_COLOR = 0x1004;
    public const uint GL_DONT_CARE = 0x1100;
    public const uint GL_FASTEST = 0x1101;
    public const uint GL_NICEST = 0x1102;
    public const uint GL_BYTE = 0x1400;
    public const uint GL_UNSIGNED_BYTE = 0x1401;
    public const uint GL_SHORT = 0x1402;
    public const uint GL_UNSIGNED_SHORT = 0x1403;
    public const uint GL_INT = 0x1404;
    public const uint GL_UNSIGNED_INT = 0x1405;
    public const uint GL_FLOAT = 0x1406;
    public const uint GL_STACK_OVERFLOW = 0x0503;
    public const uint GL_STACK_UNDERFLOW = 0x0504;
    public const uint GL_CLEAR = 0x1500;
    public const uint GL_AND = 0x1501;
    public const uint GL_AND_REVERSE = 0x1502;
    public const uint GL_COPY = 0x1503;
    public const uint GL_AND_INVERTED = 0x1504;
    public const uint GL_NOOP = 0x1505;
    public const uint GL_XOR = 0x1506;
    public const uint GL_OR = 0x1507;
    public const uint GL_NOR = 0x1508;
    public const uint GL_EQUIV = 0x1509;
    public const uint GL_INVERT = 0x150A;
    public const uint GL_OR_REVERSE = 0x150B;
    public const uint GL_COPY_INVERTED = 0x150C;
    public const uint GL_OR_INVERTED = 0x150D;
    public const uint GL_NAND = 0x150E;
    public const uint GL_SET = 0x150F;
    public const uint GL_TEXTURE = 0x1702;
    public const uint GL_COLOR = 0x1800;
    public const uint GL_DEPTH = 0x1801;
    public const uint GL_STENCIL = 0x1802;
    public const uint GL_STENCIL_INDEX = 0x1901;
    public const uint GL_DEPTH_COMPONENT = 0x1902;
    public const uint GL_RED = 0x1903;
    public const uint GL_GREEN = 0x1904;
    public const uint GL_BLUE = 0x1905;
    public const uint GL_ALPHA = 0x1906;
    public const uint GL_RGB = 0x1907;
    public const uint GL_RGBA = 0x1908;
    public const uint GL_POINT = 0x1B00;
    public const uint GL_LINE = 0x1B01;
    public const uint GL_FILL = 0x1B02;
    public const uint GL_KEEP = 0x1E00;
    public const uint GL_REPLACE = 0x1E01;
    public const uint GL_INCR = 0x1E02;
    public const uint GL_DECR = 0x1E03;
    public const uint GL_VENDOR = 0x1F00;
    public const uint GL_RENDERER = 0x1F01;
    public const uint GL_VERSION = 0x1F02;
    public const uint GL_EXTENSIONS = 0x1F03;
    public const uint GL_NEAREST = 0x2600;
    public const uint GL_LINEAR = 0x2601;
    public const uint GL_NEAREST_MIPMAP_NEAREST = 0x2700;
    public const uint GL_LINEAR_MIPMAP_NEAREST = 0x2701;
    public const uint GL_NEAREST_MIPMAP_LINEAR = 0x2702;
    public const uint GL_LINEAR_MIPMAP_LINEAR = 0x2703;
    public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
    public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
    public const uint GL_TEXTURE_WRAP_S = 0x2802;
    public const uint GL_TEXTURE_WRAP_T = 0x2803;
    public const uint GL_REPEAT = 0x2901;

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glCullFace = null;
    /// <summary> <see href="docs.gl/gl4/glCullFace">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCullFace(uint mode) => pfn_glCullFace(mode);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glFrontFace = null;
    /// <summary> <see href="docs.gl/gl4/glFrontFace">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFrontFace(uint mode) => pfn_glFrontFace(mode);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glHint = null;
    /// <summary> <see href="docs.gl/gl4/glHint">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glHint(uint target, uint mode) => pfn_glHint(target, mode);

    private static delegate* unmanaged[Stdcall]<float, void> pfn_glLineWidth = null;
    /// <summary> <see href="docs.gl/gl4/glLineWidth">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glLineWidth(float width) => pfn_glLineWidth(width);

    private static delegate* unmanaged[Stdcall]<float, void> pfn_glPointSize = null;
    /// <summary> <see href="docs.gl/gl4/glPointSize">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPointSize(float size) => pfn_glPointSize(size);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glPolygonMode = null;
    /// <summary> <see href="docs.gl/gl4/glPolygonMode">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPolygonMode(uint face, uint mode) => pfn_glPolygonMode(face, mode);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, void> pfn_glScissor = null;
    /// <summary> <see href="docs.gl/gl4/glScissor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glScissor(int x, int y, int width, int height) => pfn_glScissor(x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, float, void> pfn_glTexParameterf = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameterf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameterf(uint target, uint pname, float param) => pfn_glTexParameterf(target, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glTexParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameterfv(uint target, uint pname, float* @params) => pfn_glTexParameterfv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glTexParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameteri(uint target, uint pname, int param) => pfn_glTexParameteri(target, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glTexParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameteriv(uint target, uint pname, int* @params) => pfn_glTexParameteriv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, uint, uint, void*, void> pfn_glTexImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glTexImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, void* pixels) => pfn_glTexImage1D(target, level, internalformat, width, border, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, uint, void*, void> pfn_glTexImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glTexImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels) => pfn_glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDrawBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glDrawBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawBuffer(uint buf) => pfn_glDrawBuffer(buf);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glClear = null;
    /// <summary> <see href="docs.gl/gl4/glClear">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClear(uint mask) => pfn_glClear(mask);

    private static delegate* unmanaged[Stdcall]<float, float, float, float, void> pfn_glClearColor = null;
    /// <summary> <see href="docs.gl/gl4/glClearColor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearColor(float red, float green, float blue, float alpha) => pfn_glClearColor(red, green, blue, alpha);

    private static delegate* unmanaged[Stdcall]<int, void> pfn_glClearStencil = null;
    /// <summary> <see href="docs.gl/gl4/glClearStencil">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearStencil(int s) => pfn_glClearStencil(s);

    private static delegate* unmanaged[Stdcall]<double, void> pfn_glClearDepth = null;
    /// <summary> <see href="docs.gl/gl4/glClearDepth">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearDepth(double depth) => pfn_glClearDepth(depth);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glStencilMask = null;
    /// <summary> <see href="docs.gl/gl4/glStencilMask">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilMask(uint mask) => pfn_glStencilMask(mask);

    private static delegate* unmanaged[Stdcall]<byte, byte, byte, byte, void> pfn_glColorMask = null;
    /// <summary> <see href="docs.gl/gl4/glColorMask">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glColorMask(byte red, byte green, byte blue, byte alpha) => pfn_glColorMask(red, green, blue, alpha);

    private static delegate* unmanaged[Stdcall]<byte, void> pfn_glDepthMask = null;
    /// <summary> <see href="docs.gl/gl4/glDepthMask">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthMask(byte flag) => pfn_glDepthMask(flag);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDisable = null;
    /// <summary> <see href="docs.gl/gl4/glDisable">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDisable(uint cap) => pfn_glDisable(cap);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glEnable = null;
    /// <summary> <see href="docs.gl/gl4/glEnable">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEnable(uint cap) => pfn_glEnable(cap);

    private static delegate* unmanaged[Stdcall]<void> pfn_glFinish = null;
    /// <summary> <see href="docs.gl/gl4/glFinish">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFinish() => pfn_glFinish();

    private static delegate* unmanaged[Stdcall]<void> pfn_glFlush = null;
    /// <summary> <see href="docs.gl/gl4/glFlush">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFlush() => pfn_glFlush();

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBlendFunc = null;
    /// <summary> <see href="docs.gl/gl4/glBlendFunc">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendFunc(uint sfactor, uint dfactor) => pfn_glBlendFunc(sfactor, dfactor);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glLogicOp = null;
    /// <summary> <see href="docs.gl/gl4/glLogicOp">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glLogicOp(uint opcode) => pfn_glLogicOp(opcode);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void> pfn_glStencilFunc = null;
    /// <summary> <see href="docs.gl/gl4/glStencilFunc">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilFunc(uint func, int @ref, uint mask) => pfn_glStencilFunc(func, @ref, mask);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glStencilOp = null;
    /// <summary> <see href="docs.gl/gl4/glStencilOp">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilOp(uint fail, uint zfail, uint zpass) => pfn_glStencilOp(fail, zfail, zpass);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDepthFunc = null;
    /// <summary> <see href="docs.gl/gl4/glDepthFunc">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthFunc(uint func) => pfn_glDepthFunc(func);

    private static delegate* unmanaged[Stdcall]<uint, float, void> pfn_glPixelStoref = null;
    /// <summary> <see href="docs.gl/gl4/glPixelStoref">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPixelStoref(uint pname, float param) => pfn_glPixelStoref(pname, param);

    private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glPixelStorei = null;
    /// <summary> <see href="docs.gl/gl4/glPixelStorei">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPixelStorei(uint pname, int param) => pfn_glPixelStorei(pname, param);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glReadBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glReadBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glReadBuffer(uint src) => pfn_glReadBuffer(src);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, uint, uint, void*, void> pfn_glReadPixels = null;
    /// <summary> <see href="docs.gl/gl4/glReadPixels">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glReadPixels(int x, int y, int width, int height, uint format, uint type, void* pixels) => pfn_glReadPixels(x, y, width, height, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, byte*, void> pfn_glGetBooleanv = null;
    /// <summary> <see href="docs.gl/gl4/glGetBooleanv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBooleanv(uint pname, byte* data) => pfn_glGetBooleanv(pname, data);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glGetDoublev = null;
    /// <summary> <see href="docs.gl/gl4/glGetDoublev">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetDoublev(uint pname, double* data) => pfn_glGetDoublev(pname, data);

    private static delegate* unmanaged[Stdcall]<uint> pfn_glGetError = null;
    /// <summary> <see href="docs.gl/gl4/glGetError">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetError() => pfn_glGetError();

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glGetFloatv = null;
    /// <summary> <see href="docs.gl/gl4/glGetFloatv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetFloatv(uint pname, float* data) => pfn_glGetFloatv(pname, data);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glGetIntegerv = null;
    /// <summary> <see href="docs.gl/gl4/glGetIntegerv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetIntegerv(uint pname, int* data) => pfn_glGetIntegerv(pname, data);

    private static delegate* unmanaged[Stdcall]<uint, byte*> pfn_glGetString = null;
    /// <summary> <see href="docs.gl/gl4/glGetString">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte* glGetString(uint name) => pfn_glGetString(name);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void*, void> pfn_glGetTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexImage(uint target, int level, uint format, uint type, void* pixels) => pfn_glGetTexImage(target, level, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetTexParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexParameterfv(uint target, uint pname, float* @params) => pfn_glGetTexParameterfv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetTexParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexParameteriv(uint target, uint pname, int* @params) => pfn_glGetTexParameteriv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, float*, void> pfn_glGetTexLevelParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexLevelParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexLevelParameterfv(uint target, int level, uint pname, float* @params) => pfn_glGetTexLevelParameterfv(target, level, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int*, void> pfn_glGetTexLevelParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexLevelParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexLevelParameteriv(uint target, int level, uint pname, int* @params) => pfn_glGetTexLevelParameteriv(target, level, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsEnabled = null;
    /// <summary> <see href="docs.gl/gl4/glIsEnabled">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsEnabled(uint cap) => pfn_glIsEnabled(cap);

    private static delegate* unmanaged[Stdcall]<double, double, void> pfn_glDepthRange = null;
    /// <summary> <see href="docs.gl/gl4/glDepthRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthRange(double n, double f) => pfn_glDepthRange(n, f);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, void> pfn_glViewport = null;
    /// <summary> <see href="docs.gl/gl4/glViewport">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glViewport(int x, int y, int width, int height) => pfn_glViewport(x, y, width, height);
    /*  GL_VERSION_1_1 */
    public const uint GL_VERSION_1_1 = 1;
    public const uint GL_COLOR_LOGIC_OP = 0x0BF2;
    public const uint GL_POLYGON_OFFSET_UNITS = 0x2A00;
    public const uint GL_POLYGON_OFFSET_POINT = 0x2A01;
    public const uint GL_POLYGON_OFFSET_LINE = 0x2A02;
    public const uint GL_POLYGON_OFFSET_FILL = 0x8037;
    public const uint GL_POLYGON_OFFSET_FACTOR = 0x8038;
    public const uint GL_TEXTURE_BINDING_1D = 0x8068;
    public const uint GL_TEXTURE_BINDING_2D = 0x8069;
    public const uint GL_TEXTURE_INTERNAL_FORMAT = 0x1003;
    public const uint GL_TEXTURE_RED_SIZE = 0x805C;
    public const uint GL_TEXTURE_GREEN_SIZE = 0x805D;
    public const uint GL_TEXTURE_BLUE_SIZE = 0x805E;
    public const uint GL_TEXTURE_ALPHA_SIZE = 0x805F;
    public const uint GL_DOUBLE = 0x140A;
    public const uint GL_PROXY_TEXTURE_1D = 0x8063;
    public const uint GL_PROXY_TEXTURE_2D = 0x8064;
    public const uint GL_R3_G3_B2 = 0x2A10;
    public const uint GL_RGB4 = 0x804F;
    public const uint GL_RGB5 = 0x8050;
    public const uint GL_RGB8 = 0x8051;
    public const uint GL_RGB10 = 0x8052;
    public const uint GL_RGB12 = 0x8053;
    public const uint GL_RGB16 = 0x8054;
    public const uint GL_RGBA2 = 0x8055;
    public const uint GL_RGBA4 = 0x8056;
    public const uint GL_RGB5_A1 = 0x8057;
    public const uint GL_RGBA8 = 0x8058;
    public const uint GL_RGB10_A2 = 0x8059;
    public const uint GL_RGBA12 = 0x805A;
    public const uint GL_RGBA16 = 0x805B;
    public const uint GL_VERTEX_ARRAY = 0x8074;

    private static delegate* unmanaged[Stdcall]<uint, int, int, void> pfn_glDrawArrays = null;
    /// <summary> <see href="docs.gl/gl4/glDrawArrays">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawArrays(uint mode, int first, int count) => pfn_glDrawArrays(mode, first, count);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, void> pfn_glDrawElements = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElements">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElements(uint mode, int count, uint type, void* indices) => pfn_glDrawElements(mode, count, type, indices);

    private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glGetPointerv = null;
    /// <summary> <see href="docs.gl/gl4/glGetPointerv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetPointerv(uint pname, void** @params) => pfn_glGetPointerv(pname, @params);

    private static delegate* unmanaged[Stdcall]<float, float, void> pfn_glPolygonOffset = null;
    /// <summary> <see href="docs.gl/gl4/glPolygonOffset">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPolygonOffset(float factor, float units) => pfn_glPolygonOffset(factor, units);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, void> pfn_glCopyTexImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTexImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTexImage1D(uint target, int level, uint internalformat, int x, int y, int width, int border) => pfn_glCopyTexImage1D(target, level, internalformat, x, y, width, border);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, int, void> pfn_glCopyTexImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTexImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => pfn_glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, void> pfn_glCopyTexSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTexSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width) => pfn_glCopyTexSubImage1D(target, level, xoffset, x, y, width);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, void> pfn_glCopyTexSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTexSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => pfn_glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, uint, uint, void*, void> pfn_glTexSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glTexSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, void* pixels) => pfn_glTexSubImage1D(target, level, xoffset, width, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, uint, void*, void> pfn_glTexSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glTexSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) => pfn_glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindTexture = null;
    /// <summary> <see href="docs.gl/gl4/glBindTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindTexture(uint target, uint texture) => pfn_glBindTexture(target, texture);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteTextures = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteTextures">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteTextures(int n, uint* textures) => pfn_glDeleteTextures(n, textures);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenTextures = null;
    /// <summary> <see href="docs.gl/gl4/glGenTextures">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenTextures(int n, uint* textures) => pfn_glGenTextures(n, textures);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsTexture = null;
    /// <summary> <see href="docs.gl/gl4/glIsTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsTexture(uint texture) => pfn_glIsTexture(texture);
    /*  GL_VERSION_1_2 */
    public const uint GL_VERSION_1_2 = 1;
    public const uint GL_UNSIGNED_BYTE_3_3_2 = 0x8032;
    public const uint GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
    public const uint GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
    public const uint GL_UNSIGNED_INT_8_8_8_8 = 0x8035;
    public const uint GL_UNSIGNED_INT_10_10_10_2 = 0x8036;
    public const uint GL_TEXTURE_BINDING_3D = 0x806A;
    public const uint GL_PACK_SKIP_IMAGES = 0x806B;
    public const uint GL_PACK_IMAGE_HEIGHT = 0x806C;
    public const uint GL_UNPACK_SKIP_IMAGES = 0x806D;
    public const uint GL_UNPACK_IMAGE_HEIGHT = 0x806E;
    public const uint GL_TEXTURE_3D = 0x806F;
    public const uint GL_PROXY_TEXTURE_3D = 0x8070;
    public const uint GL_TEXTURE_DEPTH = 0x8071;
    public const uint GL_TEXTURE_WRAP_R = 0x8072;
    public const uint GL_MAX_3D_TEXTURE_SIZE = 0x8073;
    public const uint GL_UNSIGNED_BYTE_2_3_3_REV = 0x8362;
    public const uint GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
    public const uint GL_UNSIGNED_SHORT_5_6_5_REV = 0x8364;
    public const uint GL_UNSIGNED_SHORT_4_4_4_4_REV = 0x8365;
    public const uint GL_UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
    public const uint GL_UNSIGNED_INT_8_8_8_8_REV = 0x8367;
    public const uint GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368;
    public const uint GL_BGR = 0x80E0;
    public const uint GL_BGRA = 0x80E1;
    public const uint GL_MAX_ELEMENTS_VERTICES = 0x80E8;
    public const uint GL_MAX_ELEMENTS_INDICES = 0x80E9;
    public const uint GL_CLAMP_TO_EDGE = 0x812F;
    public const uint GL_TEXTURE_MIN_LOD = 0x813A;
    public const uint GL_TEXTURE_MAX_LOD = 0x813B;
    public const uint GL_TEXTURE_BASE_LEVEL = 0x813C;
    public const uint GL_TEXTURE_MAX_LEVEL = 0x813D;
    public const uint GL_SMOOTH_POINT_SIZE_RANGE = 0x0B12;
    public const uint GL_SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
    public const uint GL_SMOOTH_LINE_WIDTH_RANGE = 0x0B22;
    public const uint GL_SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
    public const uint GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void*, void> pfn_glDrawRangeElements = null;
    /// <summary> <see href="docs.gl/gl4/glDrawRangeElements">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, void* indices) => pfn_glDrawRangeElements(mode, start, end, count, type, indices);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, uint, uint, void*, void> pfn_glTexImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glTexImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels) => pfn_glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, uint, void*, void> pfn_glTexSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glTexSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels) => pfn_glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, void> pfn_glCopyTexSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTexSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => pfn_glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);
    /*  GL_VERSION_1_3 */
    public const uint GL_VERSION_1_3 = 1;
    public const uint GL_TEXTURE0 = 0x84C0;
    public const uint GL_TEXTURE1 = 0x84C1;
    public const uint GL_TEXTURE2 = 0x84C2;
    public const uint GL_TEXTURE3 = 0x84C3;
    public const uint GL_TEXTURE4 = 0x84C4;
    public const uint GL_TEXTURE5 = 0x84C5;
    public const uint GL_TEXTURE6 = 0x84C6;
    public const uint GL_TEXTURE7 = 0x84C7;
    public const uint GL_TEXTURE8 = 0x84C8;
    public const uint GL_TEXTURE9 = 0x84C9;
    public const uint GL_TEXTURE10 = 0x84CA;
    public const uint GL_TEXTURE11 = 0x84CB;
    public const uint GL_TEXTURE12 = 0x84CC;
    public const uint GL_TEXTURE13 = 0x84CD;
    public const uint GL_TEXTURE14 = 0x84CE;
    public const uint GL_TEXTURE15 = 0x84CF;
    public const uint GL_TEXTURE16 = 0x84D0;
    public const uint GL_TEXTURE17 = 0x84D1;
    public const uint GL_TEXTURE18 = 0x84D2;
    public const uint GL_TEXTURE19 = 0x84D3;
    public const uint GL_TEXTURE20 = 0x84D4;
    public const uint GL_TEXTURE21 = 0x84D5;
    public const uint GL_TEXTURE22 = 0x84D6;
    public const uint GL_TEXTURE23 = 0x84D7;
    public const uint GL_TEXTURE24 = 0x84D8;
    public const uint GL_TEXTURE25 = 0x84D9;
    public const uint GL_TEXTURE26 = 0x84DA;
    public const uint GL_TEXTURE27 = 0x84DB;
    public const uint GL_TEXTURE28 = 0x84DC;
    public const uint GL_TEXTURE29 = 0x84DD;
    public const uint GL_TEXTURE30 = 0x84DE;
    public const uint GL_TEXTURE31 = 0x84DF;
    public const uint GL_ACTIVE_TEXTURE = 0x84E0;
    public const uint GL_MULTISAMPLE = 0x809D;
    public const uint GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
    public const uint GL_SAMPLE_ALPHA_TO_ONE = 0x809F;
    public const uint GL_SAMPLE_COVERAGE = 0x80A0;
    public const uint GL_SAMPLE_BUFFERS = 0x80A8;
    public const uint GL_SAMPLES = 0x80A9;
    public const uint GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
    public const uint GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
    public const uint GL_TEXTURE_CUBE_MAP = 0x8513;
    public const uint GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
    public const uint GL_PROXY_TEXTURE_CUBE_MAP = 0x851B;
    public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
    public const uint GL_COMPRESSED_RGB = 0x84ED;
    public const uint GL_COMPRESSED_RGBA = 0x84EE;
    public const uint GL_TEXTURE_COMPRESSION_HINT = 0x84EF;
    public const uint GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
    public const uint GL_TEXTURE_COMPRESSED = 0x86A1;
    public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
    public const uint GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
    public const uint GL_CLAMP_TO_BORDER = 0x812D;

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glActiveTexture = null;
    /// <summary> <see href="docs.gl/gl4/glActiveTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glActiveTexture(uint texture) => pfn_glActiveTexture(texture);

    private static delegate* unmanaged[Stdcall]<float, byte, void> pfn_glSampleCoverage = null;
    /// <summary> <see href="docs.gl/gl4/glSampleCoverage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSampleCoverage(float value, byte invert) => pfn_glSampleCoverage(value, invert);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, int, void*, void> pfn_glCompressedTexImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, void* data) => pfn_glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, void*, void> pfn_glCompressedTexImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, void* data) => pfn_glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, void*, void> pfn_glCompressedTexImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, void* data) => pfn_glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTexSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* data) => pfn_glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTexSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* data) => pfn_glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, uint, int, void*, void> pfn_glCompressedTexSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTexSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, void* data) => pfn_glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, void*, void> pfn_glGetCompressedTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetCompressedTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetCompressedTexImage(uint target, int level, void* img) => pfn_glGetCompressedTexImage(target, level, img);
    /*  GL_VERSION_1_4 */
    public const uint GL_VERSION_1_4 = 1;
    public const uint GL_BLEND_DST_RGB = 0x80C8;
    public const uint GL_BLEND_SRC_RGB = 0x80C9;
    public const uint GL_BLEND_DST_ALPHA = 0x80CA;
    public const uint GL_BLEND_SRC_ALPHA = 0x80CB;
    public const uint GL_POINT_FADE_THRESHOLD_SIZE = 0x8128;
    public const uint GL_DEPTH_COMPONENT16 = 0x81A5;
    public const uint GL_DEPTH_COMPONENT24 = 0x81A6;
    public const uint GL_DEPTH_COMPONENT32 = 0x81A7;
    public const uint GL_MIRRORED_REPEAT = 0x8370;
    public const uint GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
    public const uint GL_TEXTURE_LOD_BIAS = 0x8501;
    public const uint GL_INCR_WRAP = 0x8507;
    public const uint GL_DECR_WRAP = 0x8508;
    public const uint GL_TEXTURE_DEPTH_SIZE = 0x884A;
    public const uint GL_TEXTURE_COMPARE_MODE = 0x884C;
    public const uint GL_TEXTURE_COMPARE_FUNC = 0x884D;
    public const uint GL_BLEND_COLOR = 0x8005;
    public const uint GL_BLEND_EQUATION = 0x8009;
    public const uint GL_CONSTANT_COLOR = 0x8001;
    public const uint GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
    public const uint GL_CONSTANT_ALPHA = 0x8003;
    public const uint GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
    public const uint GL_FUNC_ADD = 0x8006;
    public const uint GL_FUNC_REVERSE_SUBTRACT = 0x800B;
    public const uint GL_FUNC_SUBTRACT = 0x800A;
    public const uint GL_MIN = 0x8007;
    public const uint GL_MAX = 0x8008;

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glBlendFuncSeparate = null;
    /// <summary> <see href="docs.gl/gl4/glBlendFuncSeparate">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha) => pfn_glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);

    private static delegate* unmanaged[Stdcall]<uint, int*, int*, int, void> pfn_glMultiDrawArrays = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawArrays">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawArrays(uint mode, int* first, int* count, int drawcount) => pfn_glMultiDrawArrays(mode, first, count, drawcount);

    private static delegate* unmanaged[Stdcall]<uint, int*, uint, void**, int, void> pfn_glMultiDrawElements = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawElements">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawElements(uint mode, int* count, uint type, void** indices, int drawcount) => pfn_glMultiDrawElements(mode, count, type, indices, drawcount);

    private static delegate* unmanaged[Stdcall]<uint, float, void> pfn_glPointParameterf = null;
    /// <summary> <see href="docs.gl/gl4/glPointParameterf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPointParameterf(uint pname, float param) => pfn_glPointParameterf(pname, param);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glPointParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glPointParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPointParameterfv(uint pname, float* @params) => pfn_glPointParameterfv(pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glPointParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glPointParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPointParameteri(uint pname, int param) => pfn_glPointParameteri(pname, param);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glPointParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glPointParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPointParameteriv(uint pname, int* @params) => pfn_glPointParameteriv(pname, @params);

    private static delegate* unmanaged[Stdcall]<float, float, float, float, void> pfn_glBlendColor = null;
    /// <summary> <see href="docs.gl/gl4/glBlendColor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendColor(float red, float green, float blue, float alpha) => pfn_glBlendColor(red, green, blue, alpha);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBlendEquation = null;
    /// <summary> <see href="docs.gl/gl4/glBlendEquation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendEquation(uint mode) => pfn_glBlendEquation(mode);
    /*  GL_VERSION_1_5 */
    public const uint GL_VERSION_1_5 = 1;
    public const uint GL_BUFFER_SIZE = 0x8764;
    public const uint GL_BUFFER_USAGE = 0x8765;
    public const uint GL_QUERY_COUNTER_BITS = 0x8864;
    public const uint GL_CURRENT_QUERY = 0x8865;
    public const uint GL_QUERY_RESULT = 0x8866;
    public const uint GL_QUERY_RESULT_AVAILABLE = 0x8867;
    public const uint GL_ARRAY_BUFFER = 0x8892;
    public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
    public const uint GL_ARRAY_BUFFER_BINDING = 0x8894;
    public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
    public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
    public const uint GL_READ_ONLY = 0x88B8;
    public const uint GL_WRITE_ONLY = 0x88B9;
    public const uint GL_READ_WRITE = 0x88BA;
    public const uint GL_BUFFER_ACCESS = 0x88BB;
    public const uint GL_BUFFER_MAPPED = 0x88BC;
    public const uint GL_BUFFER_MAP_POINTER = 0x88BD;
    public const uint GL_STREAM_DRAW = 0x88E0;
    public const uint GL_STREAM_READ = 0x88E1;
    public const uint GL_STREAM_COPY = 0x88E2;
    public const uint GL_STATIC_DRAW = 0x88E4;
    public const uint GL_STATIC_READ = 0x88E5;
    public const uint GL_STATIC_COPY = 0x88E6;
    public const uint GL_DYNAMIC_DRAW = 0x88E8;
    public const uint GL_DYNAMIC_READ = 0x88E9;
    public const uint GL_DYNAMIC_COPY = 0x88EA;
    public const uint GL_SAMPLES_PASSED = 0x8914;
    public const uint GL_SRC1_ALPHA = 0x8589;

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenQueries = null;
    /// <summary> <see href="docs.gl/gl4/glGenQueries">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenQueries(int n, uint* ids) => pfn_glGenQueries(n, ids);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteQueries = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteQueries">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteQueries(int n, uint* ids) => pfn_glDeleteQueries(n, ids);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsQuery = null;
    /// <summary> <see href="docs.gl/gl4/glIsQuery">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsQuery(uint id) => pfn_glIsQuery(id);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBeginQuery = null;
    /// <summary> <see href="docs.gl/gl4/glBeginQuery">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBeginQuery(uint target, uint id) => pfn_glBeginQuery(target, id);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glEndQuery = null;
    /// <summary> <see href="docs.gl/gl4/glEndQuery">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEndQuery(uint target) => pfn_glEndQuery(target);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetQueryiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryiv(uint target, uint pname, int* @params) => pfn_glGetQueryiv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetQueryObjectiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryObjectiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryObjectiv(uint id, uint pname, int* @params) => pfn_glGetQueryObjectiv(id, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetQueryObjectuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryObjectuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryObjectuiv(uint id, uint pname, uint* @params) => pfn_glGetQueryObjectuiv(id, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBindBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindBuffer(uint target, uint buffer) => pfn_glBindBuffer(target, buffer);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteBuffers(int n, uint* buffers) => pfn_glDeleteBuffers(n, buffers);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glGenBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenBuffers(int n, uint* buffers) => pfn_glGenBuffers(n, buffers);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glIsBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsBuffer(uint buffer) => pfn_glIsBuffer(buffer);

    private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glBufferData = null;
    /// <summary> <see href="docs.gl/gl4/glBufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBufferData(uint target, long size, void* data, uint usage) => pfn_glBufferData(target, size, data, usage);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBufferSubData(uint target, long offset, long size, void* data) => pfn_glBufferSubData(target, offset, size, data);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glGetBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glGetBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBufferSubData(uint target, long offset, long size, void* data) => pfn_glGetBufferSubData(target, offset, size, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*> pfn_glMapBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glMapBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void* glMapBuffer(uint target, uint access) => pfn_glMapBuffer(target, access);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glUnmapBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glUnmapBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glUnmapBuffer(uint target) => pfn_glUnmapBuffer(target);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetBufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetBufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBufferParameteriv(uint target, uint pname, int* @params) => pfn_glGetBufferParameteriv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetBufferPointerv = null;
    /// <summary> <see href="docs.gl/gl4/glGetBufferPointerv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBufferPointerv(uint target, uint pname, void** @params) => pfn_glGetBufferPointerv(target, pname, @params);
    /*  GL_VERSION_2_0 */
    public const uint GL_VERSION_2_0 = 1;
    public const uint GL_BLEND_EQUATION_RGB = 0x8009;
    public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
    public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
    public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
    public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
    public const uint GL_CURRENT_VERTEX_ATTRIB = 0x8626;
    public const uint GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
    public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
    public const uint GL_STENCIL_BACK_FUNC = 0x8800;
    public const uint GL_STENCIL_BACK_FAIL = 0x8801;
    public const uint GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
    public const uint GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
    public const uint GL_MAX_DRAW_BUFFERS = 0x8824;
    public const uint GL_DRAW_BUFFER0 = 0x8825;
    public const uint GL_DRAW_BUFFER1 = 0x8826;
    public const uint GL_DRAW_BUFFER2 = 0x8827;
    public const uint GL_DRAW_BUFFER3 = 0x8828;
    public const uint GL_DRAW_BUFFER4 = 0x8829;
    public const uint GL_DRAW_BUFFER5 = 0x882A;
    public const uint GL_DRAW_BUFFER6 = 0x882B;
    public const uint GL_DRAW_BUFFER7 = 0x882C;
    public const uint GL_DRAW_BUFFER8 = 0x882D;
    public const uint GL_DRAW_BUFFER9 = 0x882E;
    public const uint GL_DRAW_BUFFER10 = 0x882F;
    public const uint GL_DRAW_BUFFER11 = 0x8830;
    public const uint GL_DRAW_BUFFER12 = 0x8831;
    public const uint GL_DRAW_BUFFER13 = 0x8832;
    public const uint GL_DRAW_BUFFER14 = 0x8833;
    public const uint GL_DRAW_BUFFER15 = 0x8834;
    public const uint GL_BLEND_EQUATION_ALPHA = 0x883D;
    public const uint GL_MAX_VERTEX_ATTRIBS = 0x8869;
    public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
    public const uint GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
    public const uint GL_FRAGMENT_SHADER = 0x8B30;
    public const uint GL_VERTEX_SHADER = 0x8B31;
    public const uint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
    public const uint GL_MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
    public const uint GL_MAX_VARYING_FLOATS = 0x8B4B;
    public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
    public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
    public const uint GL_SHADER_TYPE = 0x8B4F;
    public const uint GL_FLOAT_VEC2 = 0x8B50;
    public const uint GL_FLOAT_VEC3 = 0x8B51;
    public const uint GL_FLOAT_VEC4 = 0x8B52;
    public const uint GL_INT_VEC2 = 0x8B53;
    public const uint GL_INT_VEC3 = 0x8B54;
    public const uint GL_INT_VEC4 = 0x8B55;
    public const uint GL_BOOL = 0x8B56;
    public const uint GL_BOOL_VEC2 = 0x8B57;
    public const uint GL_BOOL_VEC3 = 0x8B58;
    public const uint GL_BOOL_VEC4 = 0x8B59;
    public const uint GL_FLOAT_MAT2 = 0x8B5A;
    public const uint GL_FLOAT_MAT3 = 0x8B5B;
    public const uint GL_FLOAT_MAT4 = 0x8B5C;
    public const uint GL_SAMPLER_1D = 0x8B5D;
    public const uint GL_SAMPLER_2D = 0x8B5E;
    public const uint GL_SAMPLER_3D = 0x8B5F;
    public const uint GL_SAMPLER_CUBE = 0x8B60;
    public const uint GL_SAMPLER_1D_SHADOW = 0x8B61;
    public const uint GL_SAMPLER_2D_SHADOW = 0x8B62;
    public const uint GL_DELETE_STATUS = 0x8B80;
    public const uint GL_COMPILE_STATUS = 0x8B81;
    public const uint GL_LINK_STATUS = 0x8B82;
    public const uint GL_VALIDATE_STATUS = 0x8B83;
    public const uint GL_INFO_LOG_LENGTH = 0x8B84;
    public const uint GL_ATTACHED_SHADERS = 0x8B85;
    public const uint GL_ACTIVE_UNIFORMS = 0x8B86;
    public const uint GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
    public const uint GL_SHADER_SOURCE_LENGTH = 0x8B88;
    public const uint GL_ACTIVE_ATTRIBUTES = 0x8B89;
    public const uint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
    public const uint GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
    public const uint GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
    public const uint GL_CURRENT_PROGRAM = 0x8B8D;
    public const uint GL_POINT_SPRITE_COORD_ORIGIN = 0x8CA0;
    public const uint GL_LOWER_LEFT = 0x8CA1;
    public const uint GL_UPPER_LEFT = 0x8CA2;
    public const uint GL_STENCIL_BACK_REF = 0x8CA3;
    public const uint GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
    public const uint GL_STENCIL_BACK_WRITEMASK = 0x8CA5;

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBlendEquationSeparate = null;
    /// <summary> <see href="docs.gl/gl4/glBlendEquationSeparate">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendEquationSeparate(uint modeRGB, uint modeAlpha) => pfn_glBlendEquationSeparate(modeRGB, modeAlpha);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDrawBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glDrawBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawBuffers(int n, uint* bufs) => pfn_glDrawBuffers(n, bufs);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glStencilOpSeparate = null;
    /// <summary> <see href="docs.gl/gl4/glStencilOpSeparate">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass) => pfn_glStencilOpSeparate(face, sfail, dpfail, dppass);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, void> pfn_glStencilFuncSeparate = null;
    /// <summary> <see href="docs.gl/gl4/glStencilFuncSeparate">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilFuncSeparate(uint face, uint func, int @ref, uint mask) => pfn_glStencilFuncSeparate(face, func, @ref, mask);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glStencilMaskSeparate = null;
    /// <summary> <see href="docs.gl/gl4/glStencilMaskSeparate">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glStencilMaskSeparate(uint face, uint mask) => pfn_glStencilMaskSeparate(face, mask);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glAttachShader = null;
    /// <summary> <see href="docs.gl/gl4/glAttachShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glAttachShader(uint program, uint shader) => pfn_glAttachShader(program, shader);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, void> pfn_glBindAttribLocation = null;
    /// <summary> <see href="docs.gl/gl4/glBindAttribLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindAttribLocation(uint program, uint index, byte* name) => pfn_glBindAttribLocation(program, index, name);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glCompileShader = null;
    /// <summary> <see href="docs.gl/gl4/glCompileShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompileShader(uint shader) => pfn_glCompileShader(shader);

    private static delegate* unmanaged[Stdcall]<uint> pfn_glCreateProgram = null;
    /// <summary> <see href="docs.gl/gl4/glCreateProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glCreateProgram() => pfn_glCreateProgram();

    private static delegate* unmanaged[Stdcall]<uint, uint> pfn_glCreateShader = null;
    /// <summary> <see href="docs.gl/gl4/glCreateShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glCreateShader(uint type) => pfn_glCreateShader(type);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDeleteProgram = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteProgram(uint program) => pfn_glDeleteProgram(program);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDeleteShader = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteShader(uint shader) => pfn_glDeleteShader(shader);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDetachShader = null;
    /// <summary> <see href="docs.gl/gl4/glDetachShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDetachShader(uint program, uint shader) => pfn_glDetachShader(program, shader);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDisableVertexAttribArray = null;
    /// <summary> <see href="docs.gl/gl4/glDisableVertexAttribArray">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDisableVertexAttribArray(uint index) => pfn_glDisableVertexAttribArray(index);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glEnableVertexAttribArray = null;
    /// <summary> <see href="docs.gl/gl4/glEnableVertexAttribArray">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEnableVertexAttribArray(uint index) => pfn_glEnableVertexAttribArray(index);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void> pfn_glGetActiveAttrib = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveAttrib">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveAttrib(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => pfn_glGetActiveAttrib(program, index, bufSize, length, size, type, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void> pfn_glGetActiveUniform = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveUniform">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveUniform(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => pfn_glGetActiveUniform(program, index, bufSize, length, size, type, name);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, uint*, void> pfn_glGetAttachedShaders = null;
    /// <summary> <see href="docs.gl/gl4/glGetAttachedShaders">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetAttachedShaders(uint program, int maxCount, int* count, uint* shaders) => pfn_glGetAttachedShaders(program, maxCount, count, shaders);

    private static delegate* unmanaged[Stdcall]<uint, byte*, int> pfn_glGetAttribLocation = null;
    /// <summary> <see href="docs.gl/gl4/glGetAttribLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetAttribLocation(uint program, byte* name) => pfn_glGetAttribLocation(program, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetProgramiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramiv(uint program, uint pname, int* @params) => pfn_glGetProgramiv(program, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void> pfn_glGetProgramInfoLog = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramInfoLog">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramInfoLog(uint program, int bufSize, int* length, byte* infoLog) => pfn_glGetProgramInfoLog(program, bufSize, length, infoLog);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetShaderiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetShaderiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetShaderiv(uint shader, uint pname, int* @params) => pfn_glGetShaderiv(shader, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void> pfn_glGetShaderInfoLog = null;
    /// <summary> <see href="docs.gl/gl4/glGetShaderInfoLog">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetShaderInfoLog(uint shader, int bufSize, int* length, byte* infoLog) => pfn_glGetShaderInfoLog(shader, bufSize, length, infoLog);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void> pfn_glGetShaderSource = null;
    /// <summary> <see href="docs.gl/gl4/glGetShaderSource">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetShaderSource(uint shader, int bufSize, int* length, byte* source) => pfn_glGetShaderSource(shader, bufSize, length, source);

    private static delegate* unmanaged[Stdcall]<uint, byte*, int> pfn_glGetUniformLocation = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetUniformLocation(uint program, byte* name) => pfn_glGetUniformLocation(program, name);

    private static delegate* unmanaged[Stdcall]<uint, int, float*, void> pfn_glGetUniformfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformfv(uint program, int location, float* @params) => pfn_glGetUniformfv(program, location, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, void> pfn_glGetUniformiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformiv(uint program, int location, int* @params) => pfn_glGetUniformiv(program, location, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, double*, void> pfn_glGetVertexAttribdv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribdv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribdv(uint index, uint pname, double* @params) => pfn_glGetVertexAttribdv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetVertexAttribfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribfv(uint index, uint pname, float* @params) => pfn_glGetVertexAttribfv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetVertexAttribiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribiv(uint index, uint pname, int* @params) => pfn_glGetVertexAttribiv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetVertexAttribPointerv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribPointerv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribPointerv(uint index, uint pname, void** pointer) => pfn_glGetVertexAttribPointerv(index, pname, pointer);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsProgram = null;
    /// <summary> <see href="docs.gl/gl4/glIsProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsProgram(uint program) => pfn_glIsProgram(program);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsShader = null;
    /// <summary> <see href="docs.gl/gl4/glIsShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsShader(uint shader) => pfn_glIsShader(shader);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glLinkProgram = null;
    /// <summary> <see href="docs.gl/gl4/glLinkProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glLinkProgram(uint program) => pfn_glLinkProgram(program);

    private static delegate* unmanaged[Stdcall]<uint, int, byte**, int*, void> pfn_glShaderSource = null;
    /// <summary> <see href="docs.gl/gl4/glShaderSource">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glShaderSource(uint shader, int count, byte** @string, int* length) => pfn_glShaderSource(shader, count, @string, length);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glUseProgram = null;
    /// <summary> <see href="docs.gl/gl4/glUseProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUseProgram(uint program) => pfn_glUseProgram(program);

    private static delegate* unmanaged[Stdcall]<int, float, void> pfn_glUniform1f = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1f(int location, float v0) => pfn_glUniform1f(location, v0);

    private static delegate* unmanaged[Stdcall]<int, float, float, void> pfn_glUniform2f = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2f(int location, float v0, float v1) => pfn_glUniform2f(location, v0, v1);

    private static delegate* unmanaged[Stdcall]<int, float, float, float, void> pfn_glUniform3f = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3f(int location, float v0, float v1, float v2) => pfn_glUniform3f(location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<int, float, float, float, float, void> pfn_glUniform4f = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4f(int location, float v0, float v1, float v2, float v3) => pfn_glUniform4f(location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<int, int, void> pfn_glUniform1i = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1i(int location, int v0) => pfn_glUniform1i(location, v0);

    private static delegate* unmanaged[Stdcall]<int, int, int, void> pfn_glUniform2i = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2i(int location, int v0, int v1) => pfn_glUniform2i(location, v0, v1);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, void> pfn_glUniform3i = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3i(int location, int v0, int v1, int v2) => pfn_glUniform3i(location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, int, void> pfn_glUniform4i = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4i(int location, int v0, int v1, int v2, int v3) => pfn_glUniform4i(location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<int, int, float*, void> pfn_glUniform1fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1fv(int location, int count, float* value) => pfn_glUniform1fv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, float*, void> pfn_glUniform2fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2fv(int location, int count, float* value) => pfn_glUniform2fv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, float*, void> pfn_glUniform3fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3fv(int location, int count, float* value) => pfn_glUniform3fv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, float*, void> pfn_glUniform4fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4fv(int location, int count, float* value) => pfn_glUniform4fv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, int*, void> pfn_glUniform1iv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1iv(int location, int count, int* value) => pfn_glUniform1iv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, int*, void> pfn_glUniform2iv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2iv(int location, int count, int* value) => pfn_glUniform2iv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, int*, void> pfn_glUniform3iv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3iv(int location, int count, int* value) => pfn_glUniform3iv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, int*, void> pfn_glUniform4iv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4iv(int location, int count, int* value) => pfn_glUniform4iv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix2fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix2fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix3fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix3fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix4fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix4fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glValidateProgram = null;
    /// <summary> <see href="docs.gl/gl4/glValidateProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glValidateProgram(uint program) => pfn_glValidateProgram(program);

    private static delegate* unmanaged[Stdcall]<uint, double, void> pfn_glVertexAttrib1d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1d(uint index, double x) => pfn_glVertexAttrib1d(index, x);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttrib1dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1dv(uint index, double* v) => pfn_glVertexAttrib1dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, float, void> pfn_glVertexAttrib1f = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1f(uint index, float x) => pfn_glVertexAttrib1f(index, x);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glVertexAttrib1fv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1fv(uint index, float* v) => pfn_glVertexAttrib1fv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short, void> pfn_glVertexAttrib1s = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1s">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1s(uint index, short x) => pfn_glVertexAttrib1s(index, x);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttrib1sv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib1sv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib1sv(uint index, short* v) => pfn_glVertexAttrib1sv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double, double, void> pfn_glVertexAttrib2d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2d(uint index, double x, double y) => pfn_glVertexAttrib2d(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttrib2dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2dv(uint index, double* v) => pfn_glVertexAttrib2dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, float, float, void> pfn_glVertexAttrib2f = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2f(uint index, float x, float y) => pfn_glVertexAttrib2f(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glVertexAttrib2fv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2fv(uint index, float* v) => pfn_glVertexAttrib2fv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short, short, void> pfn_glVertexAttrib2s = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2s">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2s(uint index, short x, short y) => pfn_glVertexAttrib2s(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttrib2sv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib2sv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib2sv(uint index, short* v) => pfn_glVertexAttrib2sv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double, double, double, void> pfn_glVertexAttrib3d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3d(uint index, double x, double y, double z) => pfn_glVertexAttrib3d(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttrib3dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3dv(uint index, double* v) => pfn_glVertexAttrib3dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, float, float, float, void> pfn_glVertexAttrib3f = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3f(uint index, float x, float y, float z) => pfn_glVertexAttrib3f(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glVertexAttrib3fv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3fv(uint index, float* v) => pfn_glVertexAttrib3fv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short, short, short, void> pfn_glVertexAttrib3s = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3s">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3s(uint index, short x, short y, short z) => pfn_glVertexAttrib3s(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttrib3sv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib3sv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib3sv(uint index, short* v) => pfn_glVertexAttrib3sv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, sbyte*, void> pfn_glVertexAttrib4Nbv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nbv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nbv(uint index, sbyte* v) => pfn_glVertexAttrib4Nbv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttrib4Niv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Niv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Niv(uint index, int* v) => pfn_glVertexAttrib4Niv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttrib4Nsv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nsv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nsv(uint index, short* v) => pfn_glVertexAttrib4Nsv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, byte, byte, byte, byte, void> pfn_glVertexAttrib4Nub = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nub">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w) => pfn_glVertexAttrib4Nub(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, byte*, void> pfn_glVertexAttrib4Nubv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nubv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nubv(uint index, byte* v) => pfn_glVertexAttrib4Nubv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttrib4Nuiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nuiv(uint index, uint* v) => pfn_glVertexAttrib4Nuiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, ushort*, void> pfn_glVertexAttrib4Nusv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4Nusv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4Nusv(uint index, ushort* v) => pfn_glVertexAttrib4Nusv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, sbyte*, void> pfn_glVertexAttrib4bv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4bv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4bv(uint index, sbyte* v) => pfn_glVertexAttrib4bv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double, double, double, double, void> pfn_glVertexAttrib4d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4d(uint index, double x, double y, double z, double w) => pfn_glVertexAttrib4d(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttrib4dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4dv(uint index, double* v) => pfn_glVertexAttrib4dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, float, float, float, float, void> pfn_glVertexAttrib4f = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4f(uint index, float x, float y, float z, float w) => pfn_glVertexAttrib4f(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glVertexAttrib4fv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4fv(uint index, float* v) => pfn_glVertexAttrib4fv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttrib4iv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4iv(uint index, int* v) => pfn_glVertexAttrib4iv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short, short, short, short, void> pfn_glVertexAttrib4s = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4s">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4s(uint index, short x, short y, short z, short w) => pfn_glVertexAttrib4s(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttrib4sv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4sv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4sv(uint index, short* v) => pfn_glVertexAttrib4sv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, byte*, void> pfn_glVertexAttrib4ubv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4ubv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4ubv(uint index, byte* v) => pfn_glVertexAttrib4ubv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttrib4uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4uiv(uint index, uint* v) => pfn_glVertexAttrib4uiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, ushort*, void> pfn_glVertexAttrib4usv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttrib4usv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttrib4usv(uint index, ushort* v) => pfn_glVertexAttrib4usv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, byte, int, void*, void> pfn_glVertexAttribPointer = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribPointer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribPointer(uint index, int size, uint type, byte normalized, int stride, void* pointer) => pfn_glVertexAttribPointer(index, size, type, normalized, stride, pointer);
    /*  GL_VERSION_2_1 */
    public const uint GL_VERSION_2_1 = 1;
    public const uint GL_PIXEL_PACK_BUFFER = 0x88EB;
    public const uint GL_PIXEL_UNPACK_BUFFER = 0x88EC;
    public const uint GL_PIXEL_PACK_BUFFER_BINDING = 0x88ED;
    public const uint GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
    public const uint GL_FLOAT_MAT2x3 = 0x8B65;
    public const uint GL_FLOAT_MAT2x4 = 0x8B66;
    public const uint GL_FLOAT_MAT3x2 = 0x8B67;
    public const uint GL_FLOAT_MAT3x4 = 0x8B68;
    public const uint GL_FLOAT_MAT4x2 = 0x8B69;
    public const uint GL_FLOAT_MAT4x3 = 0x8B6A;
    public const uint GL_SRGB = 0x8C40;
    public const uint GL_SRGB8 = 0x8C41;
    public const uint GL_SRGB_ALPHA = 0x8C42;
    public const uint GL_SRGB8_ALPHA8 = 0x8C43;
    public const uint GL_COMPRESSED_SRGB = 0x8C48;
    public const uint GL_COMPRESSED_SRGB_ALPHA = 0x8C49;

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix2x3fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2x3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2x3fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix2x3fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix3x2fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3x2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3x2fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix3x2fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix2x4fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2x4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2x4fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix2x4fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix4x2fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4x2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4x2fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix4x2fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix3x4fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3x4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3x4fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix3x4fv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, float*, void> pfn_glUniformMatrix4x3fv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4x3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4x3fv(int location, int count, byte transpose, float* value) => pfn_glUniformMatrix4x3fv(location, count, transpose, value);
    /*  GL_VERSION_3_0 */
    public const uint GL_VERSION_3_0 = 1;
    public const uint GL_COMPARE_REF_TO_TEXTURE = 0x884E;
    public const uint GL_CLIP_DISTANCE0 = 0x3000;
    public const uint GL_CLIP_DISTANCE1 = 0x3001;
    public const uint GL_CLIP_DISTANCE2 = 0x3002;
    public const uint GL_CLIP_DISTANCE3 = 0x3003;
    public const uint GL_CLIP_DISTANCE4 = 0x3004;
    public const uint GL_CLIP_DISTANCE5 = 0x3005;
    public const uint GL_CLIP_DISTANCE6 = 0x3006;
    public const uint GL_CLIP_DISTANCE7 = 0x3007;
    public const uint GL_MAX_CLIP_DISTANCES = 0x0D32;
    public const uint GL_MAJOR_VERSION = 0x821B;
    public const uint GL_MINOR_VERSION = 0x821C;
    public const uint GL_NUM_EXTENSIONS = 0x821D;
    public const uint GL_CONTEXT_FLAGS = 0x821E;
    public const uint GL_COMPRESSED_RED = 0x8225;
    public const uint GL_COMPRESSED_RG = 0x8226;
    public const uint GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x00000001;
    public const uint GL_RGBA32F = 0x8814;
    public const uint GL_RGB32F = 0x8815;
    public const uint GL_RGBA16F = 0x881A;
    public const uint GL_RGB16F = 0x881B;
    public const uint GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
    public const uint GL_MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
    public const uint GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
    public const uint GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
    public const uint GL_CLAMP_READ_COLOR = 0x891C;
    public const uint GL_FIXED_ONLY = 0x891D;
    public const uint GL_MAX_VARYING_COMPONENTS = 0x8B4B;
    public const uint GL_TEXTURE_1D_ARRAY = 0x8C18;
    public const uint GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19;
    public const uint GL_TEXTURE_2D_ARRAY = 0x8C1A;
    public const uint GL_PROXY_TEXTURE_2D_ARRAY = 0x8C1B;
    public const uint GL_TEXTURE_BINDING_1D_ARRAY = 0x8C1C;
    public const uint GL_TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
    public const uint GL_R11F_G11F_B10F = 0x8C3A;
    public const uint GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
    public const uint GL_RGB9_E5 = 0x8C3D;
    public const uint GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
    public const uint GL_TEXTURE_SHARED_SIZE = 0x8C3F;
    public const uint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
    public const uint GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
    public const uint GL_PRIMITIVES_GENERATED = 0x8C87;
    public const uint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
    public const uint GL_RASTERIZER_DISCARD = 0x8C89;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
    public const uint GL_INTERLEAVED_ATTRIBS = 0x8C8C;
    public const uint GL_SEPARATE_ATTRIBS = 0x8C8D;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
    public const uint GL_RGBA32UI = 0x8D70;
    public const uint GL_RGB32UI = 0x8D71;
    public const uint GL_RGBA16UI = 0x8D76;
    public const uint GL_RGB16UI = 0x8D77;
    public const uint GL_RGBA8UI = 0x8D7C;
    public const uint GL_RGB8UI = 0x8D7D;
    public const uint GL_RGBA32I = 0x8D82;
    public const uint GL_RGB32I = 0x8D83;
    public const uint GL_RGBA16I = 0x8D88;
    public const uint GL_RGB16I = 0x8D89;
    public const uint GL_RGBA8I = 0x8D8E;
    public const uint GL_RGB8I = 0x8D8F;
    public const uint GL_RED_INTEGER = 0x8D94;
    public const uint GL_GREEN_INTEGER = 0x8D95;
    public const uint GL_BLUE_INTEGER = 0x8D96;
    public const uint GL_RGB_INTEGER = 0x8D98;
    public const uint GL_RGBA_INTEGER = 0x8D99;
    public const uint GL_BGR_INTEGER = 0x8D9A;
    public const uint GL_BGRA_INTEGER = 0x8D9B;
    public const uint GL_SAMPLER_1D_ARRAY = 0x8DC0;
    public const uint GL_SAMPLER_2D_ARRAY = 0x8DC1;
    public const uint GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3;
    public const uint GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
    public const uint GL_SAMPLER_CUBE_SHADOW = 0x8DC5;
    public const uint GL_UNSIGNED_INT_VEC2 = 0x8DC6;
    public const uint GL_UNSIGNED_INT_VEC3 = 0x8DC7;
    public const uint GL_UNSIGNED_INT_VEC4 = 0x8DC8;
    public const uint GL_INT_SAMPLER_1D = 0x8DC9;
    public const uint GL_INT_SAMPLER_2D = 0x8DCA;
    public const uint GL_INT_SAMPLER_3D = 0x8DCB;
    public const uint GL_INT_SAMPLER_CUBE = 0x8DCC;
    public const uint GL_INT_SAMPLER_1D_ARRAY = 0x8DCE;
    public const uint GL_INT_SAMPLER_2D_ARRAY = 0x8DCF;
    public const uint GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
    public const uint GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
    public const uint GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
    public const uint GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
    public const uint GL_QUERY_WAIT = 0x8E13;
    public const uint GL_QUERY_NO_WAIT = 0x8E14;
    public const uint GL_QUERY_BY_REGION_WAIT = 0x8E15;
    public const uint GL_QUERY_BY_REGION_NO_WAIT = 0x8E16;
    public const uint GL_BUFFER_ACCESS_FLAGS = 0x911F;
    public const uint GL_BUFFER_MAP_LENGTH = 0x9120;
    public const uint GL_BUFFER_MAP_OFFSET = 0x9121;
    public const uint GL_DEPTH_COMPONENT32F = 0x8CAC;
    public const uint GL_DEPTH32F_STENCIL8 = 0x8CAD;
    public const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;
    public const uint GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
    public const uint GL_FRAMEBUFFER_DEFAULT = 0x8218;
    public const uint GL_FRAMEBUFFER_UNDEFINED = 0x8219;
    public const uint GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
    public const uint GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
    public const uint GL_DEPTH_STENCIL = 0x84F9;
    public const uint GL_UNSIGNED_INT_24_8 = 0x84FA;
    public const uint GL_DEPTH24_STENCIL8 = 0x88F0;
    public const uint GL_TEXTURE_STENCIL_SIZE = 0x88F1;
    public const uint GL_TEXTURE_RED_TYPE = 0x8C10;
    public const uint GL_TEXTURE_GREEN_TYPE = 0x8C11;
    public const uint GL_TEXTURE_BLUE_TYPE = 0x8C12;
    public const uint GL_TEXTURE_ALPHA_TYPE = 0x8C13;
    public const uint GL_TEXTURE_DEPTH_TYPE = 0x8C16;
    public const uint GL_UNSIGNED_NORMALIZED = 0x8C17;
    public const uint GL_FRAMEBUFFER_BINDING = 0x8CA6;
    public const uint GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
    public const uint GL_RENDERBUFFER_BINDING = 0x8CA7;
    public const uint GL_READ_FRAMEBUFFER = 0x8CA8;
    public const uint GL_DRAW_FRAMEBUFFER = 0x8CA9;
    public const uint GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
    public const uint GL_RENDERBUFFER_SAMPLES = 0x8CAB;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
    public const uint GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
    public const uint GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
    public const uint GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
    public const uint GL_COLOR_ATTACHMENT0 = 0x8CE0;
    public const uint GL_COLOR_ATTACHMENT1 = 0x8CE1;
    public const uint GL_COLOR_ATTACHMENT2 = 0x8CE2;
    public const uint GL_COLOR_ATTACHMENT3 = 0x8CE3;
    public const uint GL_COLOR_ATTACHMENT4 = 0x8CE4;
    public const uint GL_COLOR_ATTACHMENT5 = 0x8CE5;
    public const uint GL_COLOR_ATTACHMENT6 = 0x8CE6;
    public const uint GL_COLOR_ATTACHMENT7 = 0x8CE7;
    public const uint GL_COLOR_ATTACHMENT8 = 0x8CE8;
    public const uint GL_COLOR_ATTACHMENT9 = 0x8CE9;
    public const uint GL_COLOR_ATTACHMENT10 = 0x8CEA;
    public const uint GL_COLOR_ATTACHMENT11 = 0x8CEB;
    public const uint GL_COLOR_ATTACHMENT12 = 0x8CEC;
    public const uint GL_COLOR_ATTACHMENT13 = 0x8CED;
    public const uint GL_COLOR_ATTACHMENT14 = 0x8CEE;
    public const uint GL_COLOR_ATTACHMENT15 = 0x8CEF;
    public const uint GL_COLOR_ATTACHMENT16 = 0x8CF0;
    public const uint GL_COLOR_ATTACHMENT17 = 0x8CF1;
    public const uint GL_COLOR_ATTACHMENT18 = 0x8CF2;
    public const uint GL_COLOR_ATTACHMENT19 = 0x8CF3;
    public const uint GL_COLOR_ATTACHMENT20 = 0x8CF4;
    public const uint GL_COLOR_ATTACHMENT21 = 0x8CF5;
    public const uint GL_COLOR_ATTACHMENT22 = 0x8CF6;
    public const uint GL_COLOR_ATTACHMENT23 = 0x8CF7;
    public const uint GL_COLOR_ATTACHMENT24 = 0x8CF8;
    public const uint GL_COLOR_ATTACHMENT25 = 0x8CF9;
    public const uint GL_COLOR_ATTACHMENT26 = 0x8CFA;
    public const uint GL_COLOR_ATTACHMENT27 = 0x8CFB;
    public const uint GL_COLOR_ATTACHMENT28 = 0x8CFC;
    public const uint GL_COLOR_ATTACHMENT29 = 0x8CFD;
    public const uint GL_COLOR_ATTACHMENT30 = 0x8CFE;
    public const uint GL_COLOR_ATTACHMENT31 = 0x8CFF;
    public const uint GL_DEPTH_ATTACHMENT = 0x8D00;
    public const uint GL_STENCIL_ATTACHMENT = 0x8D20;
    public const uint GL_FRAMEBUFFER = 0x8D40;
    public const uint GL_RENDERBUFFER = 0x8D41;
    public const uint GL_RENDERBUFFER_WIDTH = 0x8D42;
    public const uint GL_RENDERBUFFER_HEIGHT = 0x8D43;
    public const uint GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
    public const uint GL_STENCIL_INDEX1 = 0x8D46;
    public const uint GL_STENCIL_INDEX4 = 0x8D47;
    public const uint GL_STENCIL_INDEX8 = 0x8D48;
    public const uint GL_STENCIL_INDEX16 = 0x8D49;
    public const uint GL_RENDERBUFFER_RED_SIZE = 0x8D50;
    public const uint GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
    public const uint GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
    public const uint GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
    public const uint GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
    public const uint GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
    public const uint GL_MAX_SAMPLES = 0x8D57;
    public const uint GL_FRAMEBUFFER_SRGB = 0x8DB9;
    public const uint GL_HALF_FLOAT = 0x140B;
    public const uint GL_MAP_READ_BIT = 0x0001;
    public const uint GL_MAP_WRITE_BIT = 0x0002;
    public const uint GL_MAP_INVALIDATE_RANGE_BIT = 0x0004;
    public const uint GL_MAP_INVALIDATE_BUFFER_BIT = 0x0008;
    public const uint GL_MAP_FLUSH_EXPLICIT_BIT = 0x0010;
    public const uint GL_MAP_UNSYNCHRONIZED_BIT = 0x0020;
    public const uint GL_COMPRESSED_RED_RGTC1 = 0x8DBB;
    public const uint GL_COMPRESSED_SIGNED_RED_RGTC1 = 0x8DBC;
    public const uint GL_COMPRESSED_RG_RGTC2 = 0x8DBD;
    public const uint GL_COMPRESSED_SIGNED_RG_RGTC2 = 0x8DBE;
    public const uint GL_RG = 0x8227;
    public const uint GL_RG_INTEGER = 0x8228;
    public const uint GL_R8 = 0x8229;
    public const uint GL_R16 = 0x822A;
    public const uint GL_RG8 = 0x822B;
    public const uint GL_RG16 = 0x822C;
    public const uint GL_R16F = 0x822D;
    public const uint GL_R32F = 0x822E;
    public const uint GL_RG16F = 0x822F;
    public const uint GL_RG32F = 0x8230;
    public const uint GL_R8I = 0x8231;
    public const uint GL_R8UI = 0x8232;
    public const uint GL_R16I = 0x8233;
    public const uint GL_R16UI = 0x8234;
    public const uint GL_R32I = 0x8235;
    public const uint GL_R32UI = 0x8236;
    public const uint GL_RG8I = 0x8237;
    public const uint GL_RG8UI = 0x8238;
    public const uint GL_RG16I = 0x8239;
    public const uint GL_RG16UI = 0x823A;
    public const uint GL_RG32I = 0x823B;
    public const uint GL_RG32UI = 0x823C;
    public const uint GL_VERTEX_ARRAY_BINDING = 0x85B5;

    private static delegate* unmanaged[Stdcall]<uint, byte, byte, byte, byte, void> pfn_glColorMaski = null;
    /// <summary> <see href="docs.gl/gl4/glColorMaski">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glColorMaski(uint index, byte r, byte g, byte b, byte a) => pfn_glColorMaski(index, r, g, b, a);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, void> pfn_glGetBooleani_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetBooleani_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBooleani_v(uint target, uint index, byte* data) => pfn_glGetBooleani_v(target, index, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetIntegeri_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetIntegeri_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetIntegeri_v(uint target, uint index, int* data) => pfn_glGetIntegeri_v(target, index, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnablei = null;
    /// <summary> <see href="docs.gl/gl4/glEnablei">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEnablei(uint target, uint index) => pfn_glEnablei(target, index);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisablei = null;
    /// <summary> <see href="docs.gl/gl4/glDisablei">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDisablei(uint target, uint index) => pfn_glDisablei(target, index);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte> pfn_glIsEnabledi = null;
    /// <summary> <see href="docs.gl/gl4/glIsEnabledi">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsEnabledi(uint target, uint index) => pfn_glIsEnabledi(target, index);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBeginTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glBeginTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBeginTransformFeedback(uint primitiveMode) => pfn_glBeginTransformFeedback(primitiveMode);

    private static delegate* unmanaged[Stdcall]<void> pfn_glEndTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glEndTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEndTransformFeedback() => pfn_glEndTransformFeedback();

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, long, void> pfn_glBindBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glBindBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindBufferRange(uint target, uint index, uint buffer, long offset, long size) => pfn_glBindBufferRange(target, index, buffer, offset, size);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBindBufferBase = null;
    /// <summary> <see href="docs.gl/gl4/glBindBufferBase">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindBufferBase(uint target, uint index, uint buffer) => pfn_glBindBufferBase(target, index, buffer);

    private static delegate* unmanaged[Stdcall]<uint, int, byte**, uint, void> pfn_glTransformFeedbackVaryings = null;
    /// <summary> <see href="docs.gl/gl4/glTransformFeedbackVaryings">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTransformFeedbackVaryings(uint program, int count, byte** varyings, uint bufferMode) => pfn_glTransformFeedbackVaryings(program, count, varyings, bufferMode);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void> pfn_glGetTransformFeedbackVarying = null;
    /// <summary> <see href="docs.gl/gl4/glGetTransformFeedbackVarying">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => pfn_glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glClampColor = null;
    /// <summary> <see href="docs.gl/gl4/glClampColor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClampColor(uint target, uint clamp) => pfn_glClampColor(target, clamp);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBeginConditionalRender = null;
    /// <summary> <see href="docs.gl/gl4/glBeginConditionalRender">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBeginConditionalRender(uint id, uint mode) => pfn_glBeginConditionalRender(id, mode);

    private static delegate* unmanaged[Stdcall]<void> pfn_glEndConditionalRender = null;
    /// <summary> <see href="docs.gl/gl4/glEndConditionalRender">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEndConditionalRender() => pfn_glEndConditionalRender();

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void*, void> pfn_glVertexAttribIPointer = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribIPointer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribIPointer(uint index, int size, uint type, int stride, void* pointer) => pfn_glVertexAttribIPointer(index, size, type, stride, pointer);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetVertexAttribIiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribIiv(uint index, uint pname, int* @params) => pfn_glGetVertexAttribIiv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetVertexAttribIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribIuiv(uint index, uint pname, uint* @params) => pfn_glGetVertexAttribIuiv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glVertexAttribI1i = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI1i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI1i(uint index, int x) => pfn_glVertexAttribI1i(index, x);

    private static delegate* unmanaged[Stdcall]<uint, int, int, void> pfn_glVertexAttribI2i = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI2i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI2i(uint index, int x, int y) => pfn_glVertexAttribI2i(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glVertexAttribI3i = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI3i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI3i(uint index, int x, int y, int z) => pfn_glVertexAttribI3i(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, void> pfn_glVertexAttribI4i = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4i(uint index, int x, int y, int z, int w) => pfn_glVertexAttribI4i(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexAttribI1ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI1ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI1ui(uint index, uint x) => pfn_glVertexAttribI1ui(index, x);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexAttribI2ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI2ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI2ui(uint index, uint x, uint y) => pfn_glVertexAttribI2ui(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glVertexAttribI3ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI3ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI3ui(uint index, uint x, uint y, uint z) => pfn_glVertexAttribI3ui(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void> pfn_glVertexAttribI4ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => pfn_glVertexAttribI4ui(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttribI1iv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI1iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI1iv(uint index, int* v) => pfn_glVertexAttribI1iv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttribI2iv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI2iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI2iv(uint index, int* v) => pfn_glVertexAttribI2iv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttribI3iv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI3iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI3iv(uint index, int* v) => pfn_glVertexAttribI3iv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glVertexAttribI4iv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4iv(uint index, int* v) => pfn_glVertexAttribI4iv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttribI1uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI1uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI1uiv(uint index, uint* v) => pfn_glVertexAttribI1uiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttribI2uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI2uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI2uiv(uint index, uint* v) => pfn_glVertexAttribI2uiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttribI3uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI3uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI3uiv(uint index, uint* v) => pfn_glVertexAttribI3uiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glVertexAttribI4uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4uiv(uint index, uint* v) => pfn_glVertexAttribI4uiv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, sbyte*, void> pfn_glVertexAttribI4bv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4bv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4bv(uint index, sbyte* v) => pfn_glVertexAttribI4bv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, short*, void> pfn_glVertexAttribI4sv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4sv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4sv(uint index, short* v) => pfn_glVertexAttribI4sv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, byte*, void> pfn_glVertexAttribI4ubv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4ubv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4ubv(uint index, byte* v) => pfn_glVertexAttribI4ubv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, ushort*, void> pfn_glVertexAttribI4usv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribI4usv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribI4usv(uint index, ushort* v) => pfn_glVertexAttribI4usv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glGetUniformuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformuiv(uint program, int location, uint* @params) => pfn_glGetUniformuiv(program, location, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, void> pfn_glBindFragDataLocation = null;
    /// <summary> <see href="docs.gl/gl4/glBindFragDataLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindFragDataLocation(uint program, uint color, byte* name) => pfn_glBindFragDataLocation(program, color, name);

    private static delegate* unmanaged[Stdcall]<uint, byte*, int> pfn_glGetFragDataLocation = null;
    /// <summary> <see href="docs.gl/gl4/glGetFragDataLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetFragDataLocation(uint program, byte* name) => pfn_glGetFragDataLocation(program, name);

    private static delegate* unmanaged[Stdcall]<int, uint, void> pfn_glUniform1ui = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1ui(int location, uint v0) => pfn_glUniform1ui(location, v0);

    private static delegate* unmanaged[Stdcall]<int, uint, uint, void> pfn_glUniform2ui = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2ui(int location, uint v0, uint v1) => pfn_glUniform2ui(location, v0, v1);

    private static delegate* unmanaged[Stdcall]<int, uint, uint, uint, void> pfn_glUniform3ui = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3ui(int location, uint v0, uint v1, uint v2) => pfn_glUniform3ui(location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<int, uint, uint, uint, uint, void> pfn_glUniform4ui = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => pfn_glUniform4ui(location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<int, int, uint*, void> pfn_glUniform1uiv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1uiv(int location, int count, uint* value) => pfn_glUniform1uiv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, uint*, void> pfn_glUniform2uiv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2uiv(int location, int count, uint* value) => pfn_glUniform2uiv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, uint*, void> pfn_glUniform3uiv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3uiv(int location, int count, uint* value) => pfn_glUniform3uiv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, uint*, void> pfn_glUniform4uiv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4uiv(int location, int count, uint* value) => pfn_glUniform4uiv(location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glTexParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameterIiv(uint target, uint pname, int* @params) => pfn_glTexParameterIiv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glTexParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glTexParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexParameterIuiv(uint target, uint pname, uint* @params) => pfn_glTexParameterIuiv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetTexParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexParameterIiv(uint target, uint pname, int* @params) => pfn_glGetTexParameterIiv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetTexParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTexParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTexParameterIuiv(uint target, uint pname, uint* @params) => pfn_glGetTexParameterIuiv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, void> pfn_glClearBufferiv = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferiv(uint buffer, int drawbuffer, int* value) => pfn_glClearBufferiv(buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glClearBufferuiv = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferuiv(uint buffer, int drawbuffer, uint* value) => pfn_glClearBufferuiv(buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float*, void> pfn_glClearBufferfv = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferfv(uint buffer, int drawbuffer, float* value) => pfn_glClearBufferfv(buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float, int, void> pfn_glClearBufferfi = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferfi">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil) => pfn_glClearBufferfi(buffer, drawbuffer, depth, stencil);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*> pfn_glGetStringi = null;
    /// <summary> <see href="docs.gl/gl4/glGetStringi">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte* glGetStringi(uint name, uint index) => pfn_glGetStringi(name, index);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsRenderbuffer = null;
    /// <summary> <see href="docs.gl/gl4/glIsRenderbuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsRenderbuffer(uint renderbuffer) => pfn_glIsRenderbuffer(renderbuffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindRenderbuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBindRenderbuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindRenderbuffer(uint target, uint renderbuffer) => pfn_glBindRenderbuffer(target, renderbuffer);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteRenderbuffers = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteRenderbuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteRenderbuffers(int n, uint* renderbuffers) => pfn_glDeleteRenderbuffers(n, renderbuffers);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenRenderbuffers = null;
    /// <summary> <see href="docs.gl/gl4/glGenRenderbuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenRenderbuffers(int n, uint* renderbuffers) => pfn_glGenRenderbuffers(n, renderbuffers);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int, void> pfn_glRenderbufferStorage = null;
    /// <summary> <see href="docs.gl/gl4/glRenderbufferStorage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glRenderbufferStorage(uint target, uint internalformat, int width, int height) => pfn_glRenderbufferStorage(target, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetRenderbufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetRenderbufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetRenderbufferParameteriv(uint target, uint pname, int* @params) => pfn_glGetRenderbufferParameteriv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glIsFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsFramebuffer(uint framebuffer) => pfn_glIsFramebuffer(framebuffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBindFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindFramebuffer(uint target, uint framebuffer) => pfn_glBindFramebuffer(target, framebuffer);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteFramebuffers = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteFramebuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteFramebuffers(int n, uint* framebuffers) => pfn_glDeleteFramebuffers(n, framebuffers);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenFramebuffers = null;
    /// <summary> <see href="docs.gl/gl4/glGenFramebuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenFramebuffers(int n, uint* framebuffers) => pfn_glGenFramebuffers(n, framebuffers);

    private static delegate* unmanaged[Stdcall]<uint, uint> pfn_glCheckFramebufferStatus = null;
    /// <summary> <see href="docs.gl/gl4/glCheckFramebufferStatus">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glCheckFramebufferStatus(uint target) => pfn_glCheckFramebufferStatus(target);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, void> pfn_glFramebufferTexture1D = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferTexture1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level) => pfn_glFramebufferTexture1D(target, attachment, textarget, texture, level);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, void> pfn_glFramebufferTexture2D = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferTexture2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) => pfn_glFramebufferTexture2D(target, attachment, textarget, texture, level);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, int, void> pfn_glFramebufferTexture3D = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferTexture3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset) => pfn_glFramebufferTexture3D(target, attachment, textarget, texture, level, zoffset);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glFramebufferRenderbuffer = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferRenderbuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) => pfn_glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetFramebufferAttachmentParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetFramebufferAttachmentParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* @params) => pfn_glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glGenerateMipmap = null;
    /// <summary> <see href="docs.gl/gl4/glGenerateMipmap">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenerateMipmap(uint target) => pfn_glGenerateMipmap(target);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, int, int, int, int, uint, uint, void> pfn_glBlitFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBlitFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => pfn_glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void> pfn_glRenderbufferStorageMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glRenderbufferStorageMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) => pfn_glRenderbufferStorageMultisample(target, samples, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void> pfn_glFramebufferTextureLayer = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferTextureLayer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) => pfn_glFramebufferTextureLayer(target, attachment, texture, level, layer);

    private static delegate* unmanaged[Stdcall]<uint, long, long, uint, void*> pfn_glMapBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glMapBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void* glMapBufferRange(uint target, long offset, long length, uint access) => pfn_glMapBufferRange(target, offset, length, access);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void> pfn_glFlushMappedBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glFlushMappedBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFlushMappedBufferRange(uint target, long offset, long length) => pfn_glFlushMappedBufferRange(target, offset, length);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBindVertexArray = null;
    /// <summary> <see href="docs.gl/gl4/glBindVertexArray">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindVertexArray(uint array) => pfn_glBindVertexArray(array);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteVertexArrays = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteVertexArrays">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteVertexArrays(int n, uint* arrays) => pfn_glDeleteVertexArrays(n, arrays);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenVertexArrays = null;
    /// <summary> <see href="docs.gl/gl4/glGenVertexArrays">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenVertexArrays(int n, uint* arrays) => pfn_glGenVertexArrays(n, arrays);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsVertexArray = null;
    /// <summary> <see href="docs.gl/gl4/glIsVertexArray">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsVertexArray(uint array) => pfn_glIsVertexArray(array);
    /*  GL_VERSION_3_1 */
    public const uint GL_VERSION_3_1 = 1;
    public const uint GL_SAMPLER_2D_RECT = 0x8B63;
    public const uint GL_SAMPLER_2D_RECT_SHADOW = 0x8B64;
    public const uint GL_SAMPLER_BUFFER = 0x8DC2;
    public const uint GL_INT_SAMPLER_2D_RECT = 0x8DCD;
    public const uint GL_INT_SAMPLER_BUFFER = 0x8DD0;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5;
    public const uint GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8;
    public const uint GL_TEXTURE_BUFFER = 0x8C2A;
    public const uint GL_MAX_TEXTURE_BUFFER_SIZE = 0x8C2B;
    public const uint GL_TEXTURE_BINDING_BUFFER = 0x8C2C;
    public const uint GL_TEXTURE_BUFFER_DATA_STORE_BINDING = 0x8C2D;
    public const uint GL_TEXTURE_RECTANGLE = 0x84F5;
    public const uint GL_TEXTURE_BINDING_RECTANGLE = 0x84F6;
    public const uint GL_PROXY_TEXTURE_RECTANGLE = 0x84F7;
    public const uint GL_MAX_RECTANGLE_TEXTURE_SIZE = 0x84F8;
    public const uint GL_R8_SNORM = 0x8F94;
    public const uint GL_RG8_SNORM = 0x8F95;
    public const uint GL_RGB8_SNORM = 0x8F96;
    public const uint GL_RGBA8_SNORM = 0x8F97;
    public const uint GL_R16_SNORM = 0x8F98;
    public const uint GL_RG16_SNORM = 0x8F99;
    public const uint GL_RGB16_SNORM = 0x8F9A;
    public const uint GL_RGBA16_SNORM = 0x8F9B;
    public const uint GL_SIGNED_NORMALIZED = 0x8F9C;
    public const uint GL_PRIMITIVE_RESTART = 0x8F9D;
    public const uint GL_PRIMITIVE_RESTART_INDEX = 0x8F9E;
    public const uint GL_COPY_READ_BUFFER = 0x8F36;
    public const uint GL_COPY_WRITE_BUFFER = 0x8F37;
    public const uint GL_UNIFORM_BUFFER = 0x8A11;
    public const uint GL_UNIFORM_BUFFER_BINDING = 0x8A28;
    public const uint GL_UNIFORM_BUFFER_START = 0x8A29;
    public const uint GL_UNIFORM_BUFFER_SIZE = 0x8A2A;
    public const uint GL_MAX_VERTEX_UNIFORM_BLOCKS = 0x8A2B;
    public const uint GL_MAX_GEOMETRY_UNIFORM_BLOCKS = 0x8A2C;
    public const uint GL_MAX_FRAGMENT_UNIFORM_BLOCKS = 0x8A2D;
    public const uint GL_MAX_COMBINED_UNIFORM_BLOCKS = 0x8A2E;
    public const uint GL_MAX_UNIFORM_BUFFER_BINDINGS = 0x8A2F;
    public const uint GL_MAX_UNIFORM_BLOCK_SIZE = 0x8A30;
    public const uint GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = 0x8A31;
    public const uint GL_MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS = 0x8A32;
    public const uint GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = 0x8A33;
    public const uint GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT = 0x8A34;
    public const uint GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = 0x8A35;
    public const uint GL_ACTIVE_UNIFORM_BLOCKS = 0x8A36;
    public const uint GL_UNIFORM_TYPE = 0x8A37;
    public const uint GL_UNIFORM_SIZE = 0x8A38;
    public const uint GL_UNIFORM_NAME_LENGTH = 0x8A39;
    public const uint GL_UNIFORM_BLOCK_INDEX = 0x8A3A;
    public const uint GL_UNIFORM_OFFSET = 0x8A3B;
    public const uint GL_UNIFORM_ARRAY_STRIDE = 0x8A3C;
    public const uint GL_UNIFORM_MATRIX_STRIDE = 0x8A3D;
    public const uint GL_UNIFORM_IS_ROW_MAJOR = 0x8A3E;
    public const uint GL_UNIFORM_BLOCK_BINDING = 0x8A3F;
    public const uint GL_UNIFORM_BLOCK_DATA_SIZE = 0x8A40;
    public const uint GL_UNIFORM_BLOCK_NAME_LENGTH = 0x8A41;
    public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS = 0x8A42;
    public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = 0x8A43;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = 0x8A44;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER = 0x8A45;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;
    public const uint GL_INVALID_INDEX = 0xFFFFFFFFu;

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glDrawArraysInstanced = null;
    /// <summary> <see href="docs.gl/gl4/glDrawArraysInstanced">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawArraysInstanced(uint mode, int first, int count, int instancecount) => pfn_glDrawArraysInstanced(mode, first, count, instancecount);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void> pfn_glDrawElementsInstanced = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsInstanced">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsInstanced(uint mode, int count, uint type, void* indices, int instancecount) => pfn_glDrawElementsInstanced(mode, count, type, indices, instancecount);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glTexBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glTexBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexBuffer(uint target, uint internalformat, uint buffer) => pfn_glTexBuffer(target, internalformat, buffer);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glPrimitiveRestartIndex = null;
    /// <summary> <see href="docs.gl/gl4/glPrimitiveRestartIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPrimitiveRestartIndex(uint index) => pfn_glPrimitiveRestartIndex(index);

    private static delegate* unmanaged[Stdcall]<uint, uint, long, long, long, void> pfn_glCopyBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glCopyBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyBufferSubData(uint readTarget, uint writeTarget, long readOffset, long writeOffset, long size) => pfn_glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);

    private static delegate* unmanaged[Stdcall]<uint, int, byte**, uint*, void> pfn_glGetUniformIndices = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformIndices">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformIndices(uint program, int uniformCount, byte** uniformNames, uint* uniformIndices) => pfn_glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, uint, int*, void> pfn_glGetActiveUniformsiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveUniformsiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveUniformsiv(uint program, int uniformCount, uint* uniformIndices, uint pname, int* @params) => pfn_glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void> pfn_glGetActiveUniformName = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveUniformName">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveUniformName(uint program, uint uniformIndex, int bufSize, int* length, byte* uniformName) => pfn_glGetActiveUniformName(program, uniformIndex, bufSize, length, uniformName);

    private static delegate* unmanaged[Stdcall]<uint, byte*, uint> pfn_glGetUniformBlockIndex = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformBlockIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetUniformBlockIndex(uint program, byte* uniformBlockName) => pfn_glGetUniformBlockIndex(program, uniformBlockName);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetActiveUniformBlockiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveUniformBlockiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, int* @params) => pfn_glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void> pfn_glGetActiveUniformBlockName = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveUniformBlockName">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName) => pfn_glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glUniformBlockBinding = null;
    /// <summary> <see href="docs.gl/gl4/glUniformBlockBinding">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => pfn_glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
    /*  GL_VERSION_3_2 */
    public const uint GL_VERSION_3_2 = 1;
    public const uint GL_CONTEXT_CORE_PROFILE_BIT = 0x00000001;
    public const uint GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;
    public const uint GL_LINES_ADJACENCY = 0x000A;
    public const uint GL_LINE_STRIP_ADJACENCY = 0x000B;
    public const uint GL_TRIANGLES_ADJACENCY = 0x000C;
    public const uint GL_TRIANGLE_STRIP_ADJACENCY = 0x000D;
    public const uint GL_PROGRAM_POINT_SIZE = 0x8642;
    public const uint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = 0x8C29;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_LAYERED = 0x8DA7;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
    public const uint GL_GEOMETRY_SHADER = 0x8DD9;
    public const uint GL_GEOMETRY_VERTICES_OUT = 0x8916;
    public const uint GL_GEOMETRY_INPUT_TYPE = 0x8917;
    public const uint GL_GEOMETRY_OUTPUT_TYPE = 0x8918;
    public const uint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = 0x8DDF;
    public const uint GL_MAX_GEOMETRY_OUTPUT_VERTICES = 0x8DE0;
    public const uint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = 0x8DE1;
    public const uint GL_MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
    public const uint GL_MAX_GEOMETRY_INPUT_COMPONENTS = 0x9123;
    public const uint GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = 0x9124;
    public const uint GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
    public const uint GL_CONTEXT_PROFILE_MASK = 0x9126;
    public const uint GL_DEPTH_CLAMP = 0x864F;
    public const uint GL_QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION = 0x8E4C;
    public const uint GL_FIRST_VERTEX_CONVENTION = 0x8E4D;
    public const uint GL_LAST_VERTEX_CONVENTION = 0x8E4E;
    public const uint GL_PROVOKING_VERTEX = 0x8E4F;
    public const uint GL_TEXTURE_CUBE_MAP_SEAMLESS = 0x884F;
    public const uint GL_MAX_SERVER_WAIT_TIMEOUT = 0x9111;
    public const uint GL_OBJECT_TYPE = 0x9112;
    public const uint GL_SYNC_CONDITION = 0x9113;
    public const uint GL_SYNC_STATUS = 0x9114;
    public const uint GL_SYNC_FLAGS = 0x9115;
    public const uint GL_SYNC_FENCE = 0x9116;
    public const uint GL_SYNC_GPU_COMMANDS_COMPLETE = 0x9117;
    public const uint GL_UNSIGNALED = 0x9118;
    public const uint GL_SIGNALED = 0x9119;
    public const uint GL_ALREADY_SIGNALED = 0x911A;
    public const uint GL_TIMEOUT_EXPIRED = 0x911B;
    public const uint GL_CONDITION_SATISFIED = 0x911C;
    public const uint GL_WAIT_FAILED = 0x911D;
    public const ulong GL_TIMEOUT_IGNORED = 0xFFFFFFFFFFFFFFFF;
    public const uint GL_SYNC_FLUSH_COMMANDS_BIT = 0x00000001;
    public const uint GL_SAMPLE_POSITION = 0x8E50;
    public const uint GL_SAMPLE_MASK = 0x8E51;
    public const uint GL_SAMPLE_MASK_VALUE = 0x8E52;
    public const uint GL_MAX_SAMPLE_MASK_WORDS = 0x8E59;
    public const uint GL_TEXTURE_2D_MULTISAMPLE = 0x9100;
    public const uint GL_PROXY_TEXTURE_2D_MULTISAMPLE = 0x9101;
    public const uint GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102;
    public const uint GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9103;
    public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE = 0x9104;
    public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY = 0x9105;
    public const uint GL_TEXTURE_SAMPLES = 0x9106;
    public const uint GL_TEXTURE_FIXED_SAMPLE_LOCATIONS = 0x9107;
    public const uint GL_SAMPLER_2D_MULTISAMPLE = 0x9108;
    public const uint GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A;
    public const uint GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B;
    public const uint GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D;
    public const uint GL_MAX_COLOR_TEXTURE_SAMPLES = 0x910E;
    public const uint GL_MAX_DEPTH_TEXTURE_SAMPLES = 0x910F;
    public const uint GL_MAX_INTEGER_SAMPLES = 0x9110;

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void> pfn_glDrawElementsBaseVertex = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsBaseVertex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsBaseVertex(uint mode, int count, uint type, void* indices, int basevertex) => pfn_glDrawElementsBaseVertex(mode, count, type, indices, basevertex);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void*, int, void> pfn_glDrawRangeElementsBaseVertex = null;
    /// <summary> <see href="docs.gl/gl4/glDrawRangeElementsBaseVertex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, void* indices, int basevertex) => pfn_glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, int, void> pfn_glDrawElementsInstancedBaseVertex = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsInstancedBaseVertex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, void* indices, int instancecount, int basevertex) => pfn_glDrawElementsInstancedBaseVertex(mode, count, type, indices, instancecount, basevertex);

    private static delegate* unmanaged[Stdcall]<uint, int*, uint, void**, int, int*, void> pfn_glMultiDrawElementsBaseVertex = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsBaseVertex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawElementsBaseVertex(uint mode, int* count, uint type, void** indices, int drawcount, int* basevertex) => pfn_glMultiDrawElementsBaseVertex(mode, count, type, indices, drawcount, basevertex);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glProvokingVertex = null;
    /// <summary> <see href="docs.gl/gl4/glProvokingVertex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProvokingVertex(uint mode) => pfn_glProvokingVertex(mode);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*> pfn_glFenceSync = null;
    /// <summary> <see href="docs.gl/gl4/glFenceSync">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void* glFenceSync(uint condition, uint flags) => pfn_glFenceSync(condition, flags);

    private static delegate* unmanaged[Stdcall]<void*, byte> pfn_glIsSync = null;
    /// <summary> <see href="docs.gl/gl4/glIsSync">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsSync(void* sync) => pfn_glIsSync(sync);

    private static delegate* unmanaged[Stdcall]<void*, void> pfn_glDeleteSync = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteSync">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteSync(void* sync) => pfn_glDeleteSync(sync);

    private static delegate* unmanaged[Stdcall]<void*, uint, ulong, uint> pfn_glClientWaitSync = null;
    /// <summary> <see href="docs.gl/gl4/glClientWaitSync">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glClientWaitSync(void* sync, uint flags, ulong timeout) => pfn_glClientWaitSync(sync, flags, timeout);

    private static delegate* unmanaged[Stdcall]<void*, uint, ulong, void> pfn_glWaitSync = null;
    /// <summary> <see href="docs.gl/gl4/glWaitSync">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glWaitSync(void* sync, uint flags, ulong timeout) => pfn_glWaitSync(sync, flags, timeout);

    private static delegate* unmanaged[Stdcall]<uint, long*, void> pfn_glGetInteger64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetInteger64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetInteger64v(uint pname, long* data) => pfn_glGetInteger64v(pname, data);

    private static delegate* unmanaged[Stdcall]<void*, uint, int, int*, int*, void> pfn_glGetSynciv = null;
    /// <summary> <see href="docs.gl/gl4/glGetSynciv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetSynciv(void* sync, uint pname, int count, int* length, int* values) => pfn_glGetSynciv(sync, pname, count, length, values);

    private static delegate* unmanaged[Stdcall]<uint, uint, long*, void> pfn_glGetInteger64i_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetInteger64i_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetInteger64i_v(uint target, uint index, long* data) => pfn_glGetInteger64i_v(target, index, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, long*, void> pfn_glGetBufferParameteri64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetBufferParameteri64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetBufferParameteri64v(uint target, uint pname, long* @params) => pfn_glGetBufferParameteri64v(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glFramebufferTexture = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferTexture(uint target, uint attachment, uint texture, int level) => pfn_glFramebufferTexture(target, attachment, texture, level);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, byte, void> pfn_glTexImage2DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTexImage2DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexImage2DMultisample(uint target, int samples, uint internalformat, int width, int height, byte fixedsamplelocations) => pfn_glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, byte, void> pfn_glTexImage3DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTexImage3DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexImage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations) => pfn_glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetMultisamplefv = null;
    /// <summary> <see href="docs.gl/gl4/glGetMultisamplefv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetMultisamplefv(uint pname, uint index, float* val) => pfn_glGetMultisamplefv(pname, index, val);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glSampleMaski = null;
    /// <summary> <see href="docs.gl/gl4/glSampleMaski">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSampleMaski(uint maskNumber, uint mask) => pfn_glSampleMaski(maskNumber, mask);
    /*  GL_VERSION_3_3 */
    public const uint GL_VERSION_3_3 = 1;
    public const uint GL_VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
    public const uint GL_SRC1_COLOR = 0x88F9;
    public const uint GL_ONE_MINUS_SRC1_COLOR = 0x88FA;
    public const uint GL_ONE_MINUS_SRC1_ALPHA = 0x88FB;
    public const uint GL_MAX_DUAL_SOURCE_DRAW_BUFFERS = 0x88FC;
    public const uint GL_ANY_SAMPLES_PASSED = 0x8C2F;
    public const uint GL_SAMPLER_BINDING = 0x8919;
    public const uint GL_RGB10_A2UI = 0x906F;
    public const uint GL_TEXTURE_SWIZZLE_R = 0x8E42;
    public const uint GL_TEXTURE_SWIZZLE_G = 0x8E43;
    public const uint GL_TEXTURE_SWIZZLE_B = 0x8E44;
    public const uint GL_TEXTURE_SWIZZLE_A = 0x8E45;
    public const uint GL_TEXTURE_SWIZZLE_RGBA = 0x8E46;
    public const uint GL_TIME_ELAPSED = 0x88BF;
    public const uint GL_TIMESTAMP = 0x8E28;
    public const uint GL_INT_2_10_10_10_REV = 0x8D9F;

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, byte*, void> pfn_glBindFragDataLocationIndexed = null;
    /// <summary> <see href="docs.gl/gl4/glBindFragDataLocationIndexed">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindFragDataLocationIndexed(uint program, uint colorNumber, uint index, byte* name) => pfn_glBindFragDataLocationIndexed(program, colorNumber, index, name);

    private static delegate* unmanaged[Stdcall]<uint, byte*, int> pfn_glGetFragDataIndex = null;
    /// <summary> <see href="docs.gl/gl4/glGetFragDataIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetFragDataIndex(uint program, byte* name) => pfn_glGetFragDataIndex(program, name);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenSamplers = null;
    /// <summary> <see href="docs.gl/gl4/glGenSamplers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenSamplers(int count, uint* samplers) => pfn_glGenSamplers(count, samplers);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteSamplers = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteSamplers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteSamplers(int count, uint* samplers) => pfn_glDeleteSamplers(count, samplers);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsSampler = null;
    /// <summary> <see href="docs.gl/gl4/glIsSampler">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsSampler(uint sampler) => pfn_glIsSampler(sampler);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindSampler = null;
    /// <summary> <see href="docs.gl/gl4/glBindSampler">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindSampler(uint unit, uint sampler) => pfn_glBindSampler(unit, sampler);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glSamplerParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameteri(uint sampler, uint pname, int param) => pfn_glSamplerParameteri(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glSamplerParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameteriv(uint sampler, uint pname, int* param) => pfn_glSamplerParameteriv(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, float, void> pfn_glSamplerParameterf = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameterf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameterf(uint sampler, uint pname, float param) => pfn_glSamplerParameterf(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glSamplerParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameterfv(uint sampler, uint pname, float* param) => pfn_glSamplerParameterfv(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glSamplerParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameterIiv(uint sampler, uint pname, int* param) => pfn_glSamplerParameterIiv(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glSamplerParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glSamplerParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSamplerParameterIuiv(uint sampler, uint pname, uint* param) => pfn_glSamplerParameterIuiv(sampler, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetSamplerParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetSamplerParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetSamplerParameteriv(uint sampler, uint pname, int* @params) => pfn_glGetSamplerParameteriv(sampler, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetSamplerParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetSamplerParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetSamplerParameterIiv(uint sampler, uint pname, int* @params) => pfn_glGetSamplerParameterIiv(sampler, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetSamplerParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetSamplerParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetSamplerParameterfv(uint sampler, uint pname, float* @params) => pfn_glGetSamplerParameterfv(sampler, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetSamplerParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetSamplerParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetSamplerParameterIuiv(uint sampler, uint pname, uint* @params) => pfn_glGetSamplerParameterIuiv(sampler, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glQueryCounter = null;
    /// <summary> <see href="docs.gl/gl4/glQueryCounter">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glQueryCounter(uint id, uint target) => pfn_glQueryCounter(id, target);

    private static delegate* unmanaged[Stdcall]<uint, uint, long*, void> pfn_glGetQueryObjecti64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryObjecti64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryObjecti64v(uint id, uint pname, long* @params) => pfn_glGetQueryObjecti64v(id, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, ulong*, void> pfn_glGetQueryObjectui64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryObjectui64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryObjectui64v(uint id, uint pname, ulong* @params) => pfn_glGetQueryObjectui64v(id, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexAttribDivisor = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribDivisor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribDivisor(uint index, uint divisor) => pfn_glVertexAttribDivisor(index, divisor);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint, void> pfn_glVertexAttribP1ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP1ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP1ui(uint index, uint type, byte normalized, uint value) => pfn_glVertexAttribP1ui(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint*, void> pfn_glVertexAttribP1uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP1uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP1uiv(uint index, uint type, byte normalized, uint* value) => pfn_glVertexAttribP1uiv(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint, void> pfn_glVertexAttribP2ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP2ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP2ui(uint index, uint type, byte normalized, uint value) => pfn_glVertexAttribP2ui(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint*, void> pfn_glVertexAttribP2uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP2uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP2uiv(uint index, uint type, byte normalized, uint* value) => pfn_glVertexAttribP2uiv(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint, void> pfn_glVertexAttribP3ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP3ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP3ui(uint index, uint type, byte normalized, uint value) => pfn_glVertexAttribP3ui(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint*, void> pfn_glVertexAttribP3uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP3uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP3uiv(uint index, uint type, byte normalized, uint* value) => pfn_glVertexAttribP3uiv(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint, void> pfn_glVertexAttribP4ui = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP4ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP4ui(uint index, uint type, byte normalized, uint value) => pfn_glVertexAttribP4ui(index, type, normalized, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte, uint*, void> pfn_glVertexAttribP4uiv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribP4uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribP4uiv(uint index, uint type, byte normalized, uint* value) => pfn_glVertexAttribP4uiv(index, type, normalized, value);
    /*  GL_VERSION_4_0 */
    public const uint GL_VERSION_4_0 = 1;
    public const uint GL_SAMPLE_SHADING = 0x8C36;
    public const uint GL_MIN_SAMPLE_SHADING_VALUE = 0x8C37;
    public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5E;
    public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5F;
    public const uint GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009;
    public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY = 0x900A;
    public const uint GL_PROXY_TEXTURE_CUBE_MAP_ARRAY = 0x900B;
    public const uint GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C;
    public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D;
    public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E;
    public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F;
    public const uint GL_DRAW_INDIRECT_BUFFER = 0x8F3F;
    public const uint GL_DRAW_INDIRECT_BUFFER_BINDING = 0x8F43;
    public const uint GL_GEOMETRY_SHADER_INVOCATIONS = 0x887F;
    public const uint GL_MAX_GEOMETRY_SHADER_INVOCATIONS = 0x8E5A;
    public const uint GL_MIN_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5B;
    public const uint GL_MAX_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5C;
    public const uint GL_FRAGMENT_INTERPOLATION_OFFSET_BITS = 0x8E5D;
    public const uint GL_MAX_VERTEX_STREAMS = 0x8E71;
    public const uint GL_DOUBLE_VEC2 = 0x8FFC;
    public const uint GL_DOUBLE_VEC3 = 0x8FFD;
    public const uint GL_DOUBLE_VEC4 = 0x8FFE;
    public const uint GL_DOUBLE_MAT2 = 0x8F46;
    public const uint GL_DOUBLE_MAT3 = 0x8F47;
    public const uint GL_DOUBLE_MAT4 = 0x8F48;
    public const uint GL_DOUBLE_MAT2x3 = 0x8F49;
    public const uint GL_DOUBLE_MAT2x4 = 0x8F4A;
    public const uint GL_DOUBLE_MAT3x2 = 0x8F4B;
    public const uint GL_DOUBLE_MAT3x4 = 0x8F4C;
    public const uint GL_DOUBLE_MAT4x2 = 0x8F4D;
    public const uint GL_DOUBLE_MAT4x3 = 0x8F4E;
    public const uint GL_ACTIVE_SUBROUTINES = 0x8DE5;
    public const uint GL_ACTIVE_SUBROUTINE_UNIFORMS = 0x8DE6;
    public const uint GL_ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS = 0x8E47;
    public const uint GL_ACTIVE_SUBROUTINE_MAX_LENGTH = 0x8E48;
    public const uint GL_ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH = 0x8E49;
    public const uint GL_MAX_SUBROUTINES = 0x8DE7;
    public const uint GL_MAX_SUBROUTINE_UNIFORM_LOCATIONS = 0x8DE8;
    public const uint GL_NUM_COMPATIBLE_SUBROUTINES = 0x8E4A;
    public const uint GL_COMPATIBLE_SUBROUTINES = 0x8E4B;
    public const uint GL_PATCHES = 0x000E;
    public const uint GL_PATCH_VERTICES = 0x8E72;
    public const uint GL_PATCH_DEFAULT_INNER_LEVEL = 0x8E73;
    public const uint GL_PATCH_DEFAULT_OUTER_LEVEL = 0x8E74;
    public const uint GL_TESS_CONTROL_OUTPUT_VERTICES = 0x8E75;
    public const uint GL_TESS_GEN_MODE = 0x8E76;
    public const uint GL_TESS_GEN_SPACING = 0x8E77;
    public const uint GL_TESS_GEN_VERTEX_ORDER = 0x8E78;
    public const uint GL_TESS_GEN_POINT_MODE = 0x8E79;
    public const uint GL_ISOLINES = 0x8E7A;
    public const uint GL_FRACTIONAL_ODD = 0x8E7B;
    public const uint GL_FRACTIONAL_EVEN = 0x8E7C;
    public const uint GL_MAX_PATCH_VERTICES = 0x8E7D;
    public const uint GL_MAX_TESS_GEN_LEVEL = 0x8E7E;
    public const uint GL_MAX_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E7F;
    public const uint GL_MAX_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E80;
    public const uint GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS = 0x8E81;
    public const uint GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS = 0x8E82;
    public const uint GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS = 0x8E83;
    public const uint GL_MAX_TESS_PATCH_COMPONENTS = 0x8E84;
    public const uint GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS = 0x8E85;
    public const uint GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS = 0x8E86;
    public const uint GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS = 0x8E89;
    public const uint GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS = 0x8E8A;
    public const uint GL_MAX_TESS_CONTROL_INPUT_COMPONENTS = 0x886C;
    public const uint GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS = 0x886D;
    public const uint GL_MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E1E;
    public const uint GL_MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E1F;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER = 0x84F0;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x84F1;
    public const uint GL_TESS_EVALUATION_SHADER = 0x8E87;
    public const uint GL_TESS_CONTROL_SHADER = 0x8E88;
    public const uint GL_TRANSFORM_FEEDBACK = 0x8E22;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_PAUSED = 0x8E23;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_ACTIVE = 0x8E24;
    public const uint GL_TRANSFORM_FEEDBACK_BINDING = 0x8E25;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_BUFFERS = 0x8E70;

    private static delegate* unmanaged[Stdcall]<float, void> pfn_glMinSampleShading = null;
    /// <summary> <see href="docs.gl/gl4/glMinSampleShading">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMinSampleShading(float value) => pfn_glMinSampleShading(value);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBlendEquationi = null;
    /// <summary> <see href="docs.gl/gl4/glBlendEquationi">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendEquationi(uint buf, uint mode) => pfn_glBlendEquationi(buf, mode);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBlendEquationSeparatei = null;
    /// <summary> <see href="docs.gl/gl4/glBlendEquationSeparatei">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha) => pfn_glBlendEquationSeparatei(buf, modeRGB, modeAlpha);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBlendFunci = null;
    /// <summary> <see href="docs.gl/gl4/glBlendFunci">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendFunci(uint buf, uint src, uint dst) => pfn_glBlendFunci(buf, src, dst);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void> pfn_glBlendFuncSeparatei = null;
    /// <summary> <see href="docs.gl/gl4/glBlendFuncSeparatei">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) => pfn_glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);

    private static delegate* unmanaged[Stdcall]<uint, void*, void> pfn_glDrawArraysIndirect = null;
    /// <summary> <see href="docs.gl/gl4/glDrawArraysIndirect">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawArraysIndirect(uint mode, void* indirect) => pfn_glDrawArraysIndirect(mode, indirect);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*, void> pfn_glDrawElementsIndirect = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsIndirect">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsIndirect(uint mode, uint type, void* indirect) => pfn_glDrawElementsIndirect(mode, type, indirect);

    private static delegate* unmanaged[Stdcall]<int, double, void> pfn_glUniform1d = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1d(int location, double x) => pfn_glUniform1d(location, x);

    private static delegate* unmanaged[Stdcall]<int, double, double, void> pfn_glUniform2d = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2d(int location, double x, double y) => pfn_glUniform2d(location, x, y);

    private static delegate* unmanaged[Stdcall]<int, double, double, double, void> pfn_glUniform3d = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3d(int location, double x, double y, double z) => pfn_glUniform3d(location, x, y, z);

    private static delegate* unmanaged[Stdcall]<int, double, double, double, double, void> pfn_glUniform4d = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4d(int location, double x, double y, double z, double w) => pfn_glUniform4d(location, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<int, int, double*, void> pfn_glUniform1dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform1dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform1dv(int location, int count, double* value) => pfn_glUniform1dv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, double*, void> pfn_glUniform2dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform2dv(int location, int count, double* value) => pfn_glUniform2dv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, double*, void> pfn_glUniform3dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform3dv(int location, int count, double* value) => pfn_glUniform3dv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, double*, void> pfn_glUniform4dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniform4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniform4dv(int location, int count, double* value) => pfn_glUniform4dv(location, count, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix2dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix2dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix3dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix3dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix4dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix4dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix2x3dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2x3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2x3dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix2x3dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix2x4dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix2x4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix2x4dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix2x4dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix3x2dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3x2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3x2dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix3x2dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix3x4dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix3x4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix3x4dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix3x4dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix4x2dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4x2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4x2dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix4x2dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<int, int, byte, double*, void> pfn_glUniformMatrix4x3dv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformMatrix4x3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformMatrix4x3dv(int location, int count, byte transpose, double* value) => pfn_glUniformMatrix4x3dv(location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, double*, void> pfn_glGetUniformdv = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformdv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformdv(uint program, int location, double* @params) => pfn_glGetUniformdv(program, location, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, int> pfn_glGetSubroutineUniformLocation = null;
    /// <summary> <see href="docs.gl/gl4/glGetSubroutineUniformLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetSubroutineUniformLocation(uint program, uint shadertype, byte* name) => pfn_glGetSubroutineUniformLocation(program, shadertype, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, uint> pfn_glGetSubroutineIndex = null;
    /// <summary> <see href="docs.gl/gl4/glGetSubroutineIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetSubroutineIndex(uint program, uint shadertype, byte* name) => pfn_glGetSubroutineIndex(program, shadertype, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int*, void> pfn_glGetActiveSubroutineUniformiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveSubroutineUniformiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, int* values) => pfn_glGetActiveSubroutineUniformiv(program, shadertype, index, pname, values);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, byte*, void> pfn_glGetActiveSubroutineUniformName = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveSubroutineUniformName">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufSize, int* length, byte* name) => pfn_glGetActiveSubroutineUniformName(program, shadertype, index, bufSize, length, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, byte*, void> pfn_glGetActiveSubroutineName = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveSubroutineName">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveSubroutineName(uint program, uint shadertype, uint index, int bufSize, int* length, byte* name) => pfn_glGetActiveSubroutineName(program, shadertype, index, bufSize, length, name);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glUniformSubroutinesuiv = null;
    /// <summary> <see href="docs.gl/gl4/glUniformSubroutinesuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUniformSubroutinesuiv(uint shadertype, int count, uint* indices) => pfn_glUniformSubroutinesuiv(shadertype, count, indices);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glGetUniformSubroutineuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetUniformSubroutineuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetUniformSubroutineuiv(uint shadertype, int location, uint* @params) => pfn_glGetUniformSubroutineuiv(shadertype, location, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetProgramStageiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramStageiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramStageiv(uint program, uint shadertype, uint pname, int* values) => pfn_glGetProgramStageiv(program, shadertype, pname, values);

    private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glPatchParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glPatchParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPatchParameteri(uint pname, int value) => pfn_glPatchParameteri(pname, value);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glPatchParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glPatchParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPatchParameterfv(uint pname, float* values) => pfn_glPatchParameterfv(pname, values);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glBindTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindTransformFeedback(uint target, uint id) => pfn_glBindTransformFeedback(target, id);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteTransformFeedbacks = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteTransformFeedbacks">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteTransformFeedbacks(int n, uint* ids) => pfn_glDeleteTransformFeedbacks(n, ids);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenTransformFeedbacks = null;
    /// <summary> <see href="docs.gl/gl4/glGenTransformFeedbacks">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenTransformFeedbacks(int n, uint* ids) => pfn_glGenTransformFeedbacks(n, ids);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glIsTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsTransformFeedback(uint id) => pfn_glIsTransformFeedback(id);

    private static delegate* unmanaged[Stdcall]<void> pfn_glPauseTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glPauseTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPauseTransformFeedback() => pfn_glPauseTransformFeedback();

    private static delegate* unmanaged[Stdcall]<void> pfn_glResumeTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glResumeTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glResumeTransformFeedback() => pfn_glResumeTransformFeedback();

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDrawTransformFeedback = null;
    /// <summary> <see href="docs.gl/gl4/glDrawTransformFeedback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawTransformFeedback(uint mode, uint id) => pfn_glDrawTransformFeedback(mode, id);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glDrawTransformFeedbackStream = null;
    /// <summary> <see href="docs.gl/gl4/glDrawTransformFeedbackStream">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawTransformFeedbackStream(uint mode, uint id, uint stream) => pfn_glDrawTransformFeedbackStream(mode, id, stream);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBeginQueryIndexed = null;
    /// <summary> <see href="docs.gl/gl4/glBeginQueryIndexed">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBeginQueryIndexed(uint target, uint index, uint id) => pfn_glBeginQueryIndexed(target, index, id);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEndQueryIndexed = null;
    /// <summary> <see href="docs.gl/gl4/glEndQueryIndexed">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEndQueryIndexed(uint target, uint index) => pfn_glEndQueryIndexed(target, index);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetQueryIndexediv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryIndexediv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryIndexediv(uint target, uint index, uint pname, int* @params) => pfn_glGetQueryIndexediv(target, index, pname, @params);
    /*  GL_VERSION_4_1 */
    public const uint GL_VERSION_4_1 = 1;
    public const uint GL_FIXED = 0x140C;
    public const uint GL_IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
    public const uint GL_IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
    public const uint GL_LOW_FLOAT = 0x8DF0;
    public const uint GL_MEDIUM_FLOAT = 0x8DF1;
    public const uint GL_HIGH_FLOAT = 0x8DF2;
    public const uint GL_LOW_INT = 0x8DF3;
    public const uint GL_MEDIUM_INT = 0x8DF4;
    public const uint GL_HIGH_INT = 0x8DF5;
    public const uint GL_SHADER_COMPILER = 0x8DFA;
    public const uint GL_SHADER_BINARY_FORMATS = 0x8DF8;
    public const uint GL_NUM_SHADER_BINARY_FORMATS = 0x8DF9;
    public const uint GL_MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
    public const uint GL_MAX_VARYING_VECTORS = 0x8DFC;
    public const uint GL_MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
    public const uint GL_RGB565 = 0x8D62;
    public const uint GL_PROGRAM_BINARY_RETRIEVABLE_HINT = 0x8257;
    public const uint GL_PROGRAM_BINARY_LENGTH = 0x8741;
    public const uint GL_NUM_PROGRAM_BINARY_FORMATS = 0x87FE;
    public const uint GL_PROGRAM_BINARY_FORMATS = 0x87FF;
    public const uint GL_VERTEX_SHADER_BIT = 0x00000001;
    public const uint GL_FRAGMENT_SHADER_BIT = 0x00000002;
    public const uint GL_GEOMETRY_SHADER_BIT = 0x00000004;
    public const uint GL_TESS_CONTROL_SHADER_BIT = 0x00000008;
    public const uint GL_TESS_EVALUATION_SHADER_BIT = 0x00000010;
    public const uint GL_ALL_SHADER_BITS = 0xFFFFFFFF;
    public const uint GL_PROGRAM_SEPARABLE = 0x8258;
    public const uint GL_ACTIVE_PROGRAM = 0x8259;
    public const uint GL_PROGRAM_PIPELINE_BINDING = 0x825A;
    public const uint GL_MAX_VIEWPORTS = 0x825B;
    public const uint GL_VIEWPORT_SUBPIXEL_BITS = 0x825C;
    public const uint GL_VIEWPORT_BOUNDS_RANGE = 0x825D;
    public const uint GL_LAYER_PROVOKING_VERTEX = 0x825E;
    public const uint GL_VIEWPORT_INDEX_PROVOKING_VERTEX = 0x825F;
    public const uint GL_UNDEFINED_VERTEX = 0x8260;

    private static delegate* unmanaged[Stdcall]<void> pfn_glReleaseShaderCompiler = null;
    /// <summary> <see href="docs.gl/gl4/glReleaseShaderCompiler">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glReleaseShaderCompiler() => pfn_glReleaseShaderCompiler();

    private static delegate* unmanaged[Stdcall]<int, uint*, uint, void*, int, void> pfn_glShaderBinary = null;
    /// <summary> <see href="docs.gl/gl4/glShaderBinary">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glShaderBinary(int count, uint* shaders, uint binaryFormat, void* binary, int length) => pfn_glShaderBinary(count, shaders, binaryFormat, binary, length);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, int*, void> pfn_glGetShaderPrecisionFormat = null;
    /// <summary> <see href="docs.gl/gl4/glGetShaderPrecisionFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, int* range, int* precision) => pfn_glGetShaderPrecisionFormat(shadertype, precisiontype, range, precision);

    private static delegate* unmanaged[Stdcall]<float, float, void> pfn_glDepthRangef = null;
    /// <summary> <see href="docs.gl/gl4/glDepthRangef">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthRangef(float n, float f) => pfn_glDepthRangef(n, f);

    private static delegate* unmanaged[Stdcall]<float, void> pfn_glClearDepthf = null;
    /// <summary> <see href="docs.gl/gl4/glClearDepthf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearDepthf(float d) => pfn_glClearDepthf(d);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, uint*, void*, void> pfn_glGetProgramBinary = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramBinary">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramBinary(uint program, int bufSize, int* length, uint* binaryFormat, void* binary) => pfn_glGetProgramBinary(program, bufSize, length, binaryFormat, binary);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*, int, void> pfn_glProgramBinary = null;
    /// <summary> <see href="docs.gl/gl4/glProgramBinary">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramBinary(uint program, uint binaryFormat, void* binary, int length) => pfn_glProgramBinary(program, binaryFormat, binary, length);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glProgramParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glProgramParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramParameteri(uint program, uint pname, int value) => pfn_glProgramParameteri(program, pname, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glUseProgramStages = null;
    /// <summary> <see href="docs.gl/gl4/glUseProgramStages">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glUseProgramStages(uint pipeline, uint stages, uint program) => pfn_glUseProgramStages(pipeline, stages, program);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glActiveShaderProgram = null;
    /// <summary> <see href="docs.gl/gl4/glActiveShaderProgram">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glActiveShaderProgram(uint pipeline, uint program) => pfn_glActiveShaderProgram(pipeline, program);

    private static delegate* unmanaged[Stdcall]<uint, int, byte**, uint> pfn_glCreateShaderProgramv = null;
    /// <summary> <see href="docs.gl/gl4/glCreateShaderProgramv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glCreateShaderProgramv(uint type, int count, byte** strings) => pfn_glCreateShaderProgramv(type, count, strings);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBindProgramPipeline = null;
    /// <summary> <see href="docs.gl/gl4/glBindProgramPipeline">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindProgramPipeline(uint pipeline) => pfn_glBindProgramPipeline(pipeline);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteProgramPipelines = null;
    /// <summary> <see href="docs.gl/gl4/glDeleteProgramPipelines">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDeleteProgramPipelines(int n, uint* pipelines) => pfn_glDeleteProgramPipelines(n, pipelines);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenProgramPipelines = null;
    /// <summary> <see href="docs.gl/gl4/glGenProgramPipelines">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenProgramPipelines(int n, uint* pipelines) => pfn_glGenProgramPipelines(n, pipelines);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsProgramPipeline = null;
    /// <summary> <see href="docs.gl/gl4/glIsProgramPipeline">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glIsProgramPipeline(uint pipeline) => pfn_glIsProgramPipeline(pipeline);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetProgramPipelineiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramPipelineiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramPipelineiv(uint pipeline, uint pname, int* @params) => pfn_glGetProgramPipelineiv(pipeline, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, void> pfn_glProgramUniform1i = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1i(uint program, int location, int v0) => pfn_glProgramUniform1i(program, location, v0);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform1iv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1iv(uint program, int location, int count, int* value) => pfn_glProgramUniform1iv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float, void> pfn_glProgramUniform1f = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1f(uint program, int location, float v0) => pfn_glProgramUniform1f(program, location, v0);

    private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform1fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1fv(uint program, int location, int count, float* value) => pfn_glProgramUniform1fv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, double, void> pfn_glProgramUniform1d = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1d(uint program, int location, double v0) => pfn_glProgramUniform1d(program, location, v0);

    private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform1dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1dv(uint program, int location, int count, double* value) => pfn_glProgramUniform1dv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void> pfn_glProgramUniform1ui = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1ui(uint program, int location, uint v0) => pfn_glProgramUniform1ui(program, location, v0);

    private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform1uiv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform1uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform1uiv(uint program, int location, int count, uint* value) => pfn_glProgramUniform1uiv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glProgramUniform2i = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2i(uint program, int location, int v0, int v1) => pfn_glProgramUniform2i(program, location, v0, v1);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform2iv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2iv(uint program, int location, int count, int* value) => pfn_glProgramUniform2iv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float, float, void> pfn_glProgramUniform2f = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2f(uint program, int location, float v0, float v1) => pfn_glProgramUniform2f(program, location, v0, v1);

    private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform2fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2fv(uint program, int location, int count, float* value) => pfn_glProgramUniform2fv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, double, double, void> pfn_glProgramUniform2d = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2d(uint program, int location, double v0, double v1) => pfn_glProgramUniform2d(program, location, v0, v1);

    private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform2dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2dv(uint program, int location, int count, double* value) => pfn_glProgramUniform2dv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void> pfn_glProgramUniform2ui = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2ui(uint program, int location, uint v0, uint v1) => pfn_glProgramUniform2ui(program, location, v0, v1);

    private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform2uiv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform2uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform2uiv(uint program, int location, int count, uint* value) => pfn_glProgramUniform2uiv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, void> pfn_glProgramUniform3i = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) => pfn_glProgramUniform3i(program, location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform3iv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3iv(uint program, int location, int count, int* value) => pfn_glProgramUniform3iv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float, float, float, void> pfn_glProgramUniform3f = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) => pfn_glProgramUniform3f(program, location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform3fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3fv(uint program, int location, int count, float* value) => pfn_glProgramUniform3fv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, double, double, double, void> pfn_glProgramUniform3d = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3d(uint program, int location, double v0, double v1, double v2) => pfn_glProgramUniform3d(program, location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform3dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3dv(uint program, int location, int count, double* value) => pfn_glProgramUniform3dv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, void> pfn_glProgramUniform3ui = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2) => pfn_glProgramUniform3ui(program, location, v0, v1, v2);

    private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform3uiv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform3uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform3uiv(uint program, int location, int count, uint* value) => pfn_glProgramUniform3uiv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, void> pfn_glProgramUniform4i = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4i">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) => pfn_glProgramUniform4i(program, location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform4iv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4iv(uint program, int location, int count, int* value) => pfn_glProgramUniform4iv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, float, float, float, float, void> pfn_glProgramUniform4f = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4f">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) => pfn_glProgramUniform4f(program, location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform4fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4fv(uint program, int location, int count, float* value) => pfn_glProgramUniform4fv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, double, double, double, double, void> pfn_glProgramUniform4d = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3) => pfn_glProgramUniform4d(program, location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform4dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4dv(uint program, int location, int count, double* value) => pfn_glProgramUniform4dv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, uint, void> pfn_glProgramUniform4ui = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4ui">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3) => pfn_glProgramUniform4ui(program, location, v0, v1, v2, v3);

    private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform4uiv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniform4uiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniform4uiv(uint program, int location, int count, uint* value) => pfn_glProgramUniform4uiv(program, location, count, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2x3fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2x3fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2x3fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3x2fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3x2fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3x2fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2x4fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2x4fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2x4fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4x2fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x2fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4x2fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4x2fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3x4fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x4fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3x4fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3x4fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4x3fv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x3fv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4x3fv(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4x3fv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2x3dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2x3dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2x3dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3x2dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3x2dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3x2dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2x4dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix2x4dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2x4dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4x2dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4x2dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4x2dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3x4dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix3x4dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3x4dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4x3dv = null;
    /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glProgramUniformMatrix4x3dv(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4x3dv(program, location, count, transpose, value);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glValidateProgramPipeline = null;
    /// <summary> <see href="docs.gl/gl4/glValidateProgramPipeline">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glValidateProgramPipeline(uint pipeline) => pfn_glValidateProgramPipeline(pipeline);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void> pfn_glGetProgramPipelineInfoLog = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramPipelineInfoLog">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, int* length, byte* infoLog) => pfn_glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog);

    private static delegate* unmanaged[Stdcall]<uint, double, void> pfn_glVertexAttribL1d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL1d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL1d(uint index, double x) => pfn_glVertexAttribL1d(index, x);

    private static delegate* unmanaged[Stdcall]<uint, double, double, void> pfn_glVertexAttribL2d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL2d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL2d(uint index, double x, double y) => pfn_glVertexAttribL2d(index, x, y);

    private static delegate* unmanaged[Stdcall]<uint, double, double, double, void> pfn_glVertexAttribL3d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL3d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL3d(uint index, double x, double y, double z) => pfn_glVertexAttribL3d(index, x, y, z);

    private static delegate* unmanaged[Stdcall]<uint, double, double, double, double, void> pfn_glVertexAttribL4d = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL4d">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL4d(uint index, double x, double y, double z, double w) => pfn_glVertexAttribL4d(index, x, y, z, w);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttribL1dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL1dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL1dv(uint index, double* v) => pfn_glVertexAttribL1dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttribL2dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL2dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL2dv(uint index, double* v) => pfn_glVertexAttribL2dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttribL3dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL3dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL3dv(uint index, double* v) => pfn_glVertexAttribL3dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glVertexAttribL4dv = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribL4dv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribL4dv(uint index, double* v) => pfn_glVertexAttribL4dv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void*, void> pfn_glVertexAttribLPointer = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribLPointer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribLPointer(uint index, int size, uint type, int stride, void* pointer) => pfn_glVertexAttribLPointer(index, size, type, stride, pointer);

    private static delegate* unmanaged[Stdcall]<uint, uint, double*, void> pfn_glGetVertexAttribLdv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexAttribLdv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexAttribLdv(uint index, uint pname, double* @params) => pfn_glGetVertexAttribLdv(index, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, float*, void> pfn_glViewportArrayv = null;
    /// <summary> <see href="docs.gl/gl4/glViewportArrayv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glViewportArrayv(uint first, int count, float* v) => pfn_glViewportArrayv(first, count, v);

    private static delegate* unmanaged[Stdcall]<uint, float, float, float, float, void> pfn_glViewportIndexedf = null;
    /// <summary> <see href="docs.gl/gl4/glViewportIndexedf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glViewportIndexedf(uint index, float x, float y, float w, float h) => pfn_glViewportIndexedf(index, x, y, w, h);

    private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glViewportIndexedfv = null;
    /// <summary> <see href="docs.gl/gl4/glViewportIndexedfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glViewportIndexedfv(uint index, float* v) => pfn_glViewportIndexedfv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int, int*, void> pfn_glScissorArrayv = null;
    /// <summary> <see href="docs.gl/gl4/glScissorArrayv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glScissorArrayv(uint first, int count, int* v) => pfn_glScissorArrayv(first, count, v);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, void> pfn_glScissorIndexed = null;
    /// <summary> <see href="docs.gl/gl4/glScissorIndexed">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glScissorIndexed(uint index, int left, int bottom, int width, int height) => pfn_glScissorIndexed(index, left, bottom, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int*, void> pfn_glScissorIndexedv = null;
    /// <summary> <see href="docs.gl/gl4/glScissorIndexedv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glScissorIndexedv(uint index, int* v) => pfn_glScissorIndexedv(index, v);

    private static delegate* unmanaged[Stdcall]<uint, int, double*, void> pfn_glDepthRangeArrayv = null;
    /// <summary> <see href="docs.gl/gl4/glDepthRangeArrayv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthRangeArrayv(uint first, int count, double* v) => pfn_glDepthRangeArrayv(first, count, v);

    private static delegate* unmanaged[Stdcall]<uint, double, double, void> pfn_glDepthRangeIndexed = null;
    /// <summary> <see href="docs.gl/gl4/glDepthRangeIndexed">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDepthRangeIndexed(uint index, double n, double f) => pfn_glDepthRangeIndexed(index, n, f);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetFloati_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetFloati_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetFloati_v(uint target, uint index, float* data) => pfn_glGetFloati_v(target, index, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, double*, void> pfn_glGetDoublei_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetDoublei_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetDoublei_v(uint target, uint index, double* data) => pfn_glGetDoublei_v(target, index, data);
    /*  GL_VERSION_4_2 */
    public const uint GL_VERSION_4_2 = 1;
    public const uint GL_COPY_READ_BUFFER_BINDING = 0x8F36;
    public const uint GL_COPY_WRITE_BUFFER_BINDING = 0x8F37;
    public const uint GL_TRANSFORM_FEEDBACK_ACTIVE = 0x8E24;
    public const uint GL_TRANSFORM_FEEDBACK_PAUSED = 0x8E23;
    public const uint GL_UNPACK_COMPRESSED_BLOCK_WIDTH = 0x9127;
    public const uint GL_UNPACK_COMPRESSED_BLOCK_HEIGHT = 0x9128;
    public const uint GL_UNPACK_COMPRESSED_BLOCK_DEPTH = 0x9129;
    public const uint GL_UNPACK_COMPRESSED_BLOCK_SIZE = 0x912A;
    public const uint GL_PACK_COMPRESSED_BLOCK_WIDTH = 0x912B;
    public const uint GL_PACK_COMPRESSED_BLOCK_HEIGHT = 0x912C;
    public const uint GL_PACK_COMPRESSED_BLOCK_DEPTH = 0x912D;
    public const uint GL_PACK_COMPRESSED_BLOCK_SIZE = 0x912E;
    public const uint GL_NUM_SAMPLE_COUNTS = 0x9380;
    public const uint GL_MIN_MAP_BUFFER_ALIGNMENT = 0x90BC;
    public const uint GL_ATOMIC_COUNTER_BUFFER = 0x92C0;
    public const uint GL_ATOMIC_COUNTER_BUFFER_BINDING = 0x92C1;
    public const uint GL_ATOMIC_COUNTER_BUFFER_START = 0x92C2;
    public const uint GL_ATOMIC_COUNTER_BUFFER_SIZE = 0x92C3;
    public const uint GL_ATOMIC_COUNTER_BUFFER_DATA_SIZE = 0x92C4;
    public const uint GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS = 0x92C5;
    public const uint GL_ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES = 0x92C6;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER = 0x92C7;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER = 0x92C8;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x92C9;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER = 0x92CA;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER = 0x92CB;
    public const uint GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS = 0x92CC;
    public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS = 0x92CD;
    public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS = 0x92CE;
    public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS = 0x92CF;
    public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS = 0x92D0;
    public const uint GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS = 0x92D1;
    public const uint GL_MAX_VERTEX_ATOMIC_COUNTERS = 0x92D2;
    public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTERS = 0x92D3;
    public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTERS = 0x92D4;
    public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTERS = 0x92D5;
    public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTERS = 0x92D6;
    public const uint GL_MAX_COMBINED_ATOMIC_COUNTERS = 0x92D7;
    public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE = 0x92D8;
    public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS = 0x92DC;
    public const uint GL_ACTIVE_ATOMIC_COUNTER_BUFFERS = 0x92D9;
    public const uint GL_UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX = 0x92DA;
    public const uint GL_UNSIGNED_INT_ATOMIC_COUNTER = 0x92DB;
    public const uint GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT = 0x00000001;
    public const uint GL_ELEMENT_ARRAY_BARRIER_BIT = 0x00000002;
    public const uint GL_UNIFORM_BARRIER_BIT = 0x00000004;
    public const uint GL_TEXTURE_FETCH_BARRIER_BIT = 0x00000008;
    public const uint GL_SHADER_IMAGE_ACCESS_BARRIER_BIT = 0x00000020;
    public const uint GL_COMMAND_BARRIER_BIT = 0x00000040;
    public const uint GL_PIXEL_BUFFER_BARRIER_BIT = 0x00000080;
    public const uint GL_TEXTURE_UPDATE_BARRIER_BIT = 0x00000100;
    public const uint GL_BUFFER_UPDATE_BARRIER_BIT = 0x00000200;
    public const uint GL_FRAMEBUFFER_BARRIER_BIT = 0x00000400;
    public const uint GL_TRANSFORM_FEEDBACK_BARRIER_BIT = 0x00000800;
    public const uint GL_ATOMIC_COUNTER_BARRIER_BIT = 0x00001000;
    public const uint GL_ALL_BARRIER_BITS = 0xFFFFFFFF;
    public const uint GL_MAX_IMAGE_UNITS = 0x8F38;
    public const uint GL_MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS = 0x8F39;
    public const uint GL_IMAGE_BINDING_NAME = 0x8F3A;
    public const uint GL_IMAGE_BINDING_LEVEL = 0x8F3B;
    public const uint GL_IMAGE_BINDING_LAYERED = 0x8F3C;
    public const uint GL_IMAGE_BINDING_LAYER = 0x8F3D;
    public const uint GL_IMAGE_BINDING_ACCESS = 0x8F3E;
    public const uint GL_IMAGE_1D = 0x904C;
    public const uint GL_IMAGE_2D = 0x904D;
    public const uint GL_IMAGE_3D = 0x904E;
    public const uint GL_IMAGE_2D_RECT = 0x904F;
    public const uint GL_IMAGE_CUBE = 0x9050;
    public const uint GL_IMAGE_BUFFER = 0x9051;
    public const uint GL_IMAGE_1D_ARRAY = 0x9052;
    public const uint GL_IMAGE_2D_ARRAY = 0x9053;
    public const uint GL_IMAGE_CUBE_MAP_ARRAY = 0x9054;
    public const uint GL_IMAGE_2D_MULTISAMPLE = 0x9055;
    public const uint GL_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9056;
    public const uint GL_INT_IMAGE_1D = 0x9057;
    public const uint GL_INT_IMAGE_2D = 0x9058;
    public const uint GL_INT_IMAGE_3D = 0x9059;
    public const uint GL_INT_IMAGE_2D_RECT = 0x905A;
    public const uint GL_INT_IMAGE_CUBE = 0x905B;
    public const uint GL_INT_IMAGE_BUFFER = 0x905C;
    public const uint GL_INT_IMAGE_1D_ARRAY = 0x905D;
    public const uint GL_INT_IMAGE_2D_ARRAY = 0x905E;
    public const uint GL_INT_IMAGE_CUBE_MAP_ARRAY = 0x905F;
    public const uint GL_INT_IMAGE_2D_MULTISAMPLE = 0x9060;
    public const uint GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9061;
    public const uint GL_UNSIGNED_INT_IMAGE_1D = 0x9062;
    public const uint GL_UNSIGNED_INT_IMAGE_2D = 0x9063;
    public const uint GL_UNSIGNED_INT_IMAGE_3D = 0x9064;
    public const uint GL_UNSIGNED_INT_IMAGE_2D_RECT = 0x9065;
    public const uint GL_UNSIGNED_INT_IMAGE_CUBE = 0x9066;
    public const uint GL_UNSIGNED_INT_IMAGE_BUFFER = 0x9067;
    public const uint GL_UNSIGNED_INT_IMAGE_1D_ARRAY = 0x9068;
    public const uint GL_UNSIGNED_INT_IMAGE_2D_ARRAY = 0x9069;
    public const uint GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = 0x906A;
    public const uint GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE = 0x906B;
    public const uint GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x906C;
    public const uint GL_MAX_IMAGE_SAMPLES = 0x906D;
    public const uint GL_IMAGE_BINDING_FORMAT = 0x906E;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_TYPE = 0x90C7;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE = 0x90C8;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS = 0x90C9;
    public const uint GL_MAX_VERTEX_IMAGE_UNIFORMS = 0x90CA;
    public const uint GL_MAX_TESS_CONTROL_IMAGE_UNIFORMS = 0x90CB;
    public const uint GL_MAX_TESS_EVALUATION_IMAGE_UNIFORMS = 0x90CC;
    public const uint GL_MAX_GEOMETRY_IMAGE_UNIFORMS = 0x90CD;
    public const uint GL_MAX_FRAGMENT_IMAGE_UNIFORMS = 0x90CE;
    public const uint GL_MAX_COMBINED_IMAGE_UNIFORMS = 0x90CF;
    public const uint GL_COMPRESSED_RGBA_BPTC_UNORM = 0x8E8C;
    public const uint GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM = 0x8E8D;
    public const uint GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT = 0x8E8E;
    public const uint GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F;
    public const uint GL_TEXTURE_IMMUTABLE_FORMAT = 0x912F;

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, uint, void> pfn_glDrawArraysInstancedBaseInstance = null;
    /// <summary> <see href="docs.gl/gl4/glDrawArraysInstancedBaseInstance">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawArraysInstancedBaseInstance(uint mode, int first, int count, int instancecount, uint baseinstance) => pfn_glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, baseinstance);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, uint, void> pfn_glDrawElementsInstancedBaseInstance = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsInstancedBaseInstance">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsInstancedBaseInstance(uint mode, int count, uint type, void* indices, int instancecount, uint baseinstance) => pfn_glDrawElementsInstancedBaseInstance(mode, count, type, indices, instancecount, baseinstance);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, int, uint, void> pfn_glDrawElementsInstancedBaseVertexBaseInstance = null;
    /// <summary> <see href="docs.gl/gl4/glDrawElementsInstancedBaseVertexBaseInstance">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawElementsInstancedBaseVertexBaseInstance(uint mode, int count, uint type, void* indices, int instancecount, int basevertex, uint baseinstance) => pfn_glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, instancecount, basevertex, baseinstance);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, void> pfn_glGetInternalformativ = null;
    /// <summary> <see href="docs.gl/gl4/glGetInternalformativ">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetInternalformativ(uint target, uint internalformat, uint pname, int count, int* @params) => pfn_glGetInternalformativ(target, internalformat, pname, count, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetActiveAtomicCounterBufferiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetActiveAtomicCounterBufferiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, int* @params) => pfn_glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, byte, int, uint, uint, void> pfn_glBindImageTexture = null;
    /// <summary> <see href="docs.gl/gl4/glBindImageTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindImageTexture(uint unit, uint texture, int level, byte layered, int layer, uint access, uint format) => pfn_glBindImageTexture(unit, texture, level, layered, layer, access, format);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMemoryBarrier = null;
    /// <summary> <see href="docs.gl/gl4/glMemoryBarrier">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMemoryBarrier(uint barriers) => pfn_glMemoryBarrier(barriers);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void> pfn_glTexStorage1D = null;
    /// <summary> <see href="docs.gl/gl4/glTexStorage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexStorage1D(uint target, int levels, uint internalformat, int width) => pfn_glTexStorage1D(target, levels, internalformat, width);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void> pfn_glTexStorage2D = null;
    /// <summary> <see href="docs.gl/gl4/glTexStorage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexStorage2D(uint target, int levels, uint internalformat, int width, int height) => pfn_glTexStorage2D(target, levels, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, void> pfn_glTexStorage3D = null;
    /// <summary> <see href="docs.gl/gl4/glTexStorage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth) => pfn_glTexStorage3D(target, levels, internalformat, width, height, depth);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glDrawTransformFeedbackInstanced = null;
    /// <summary> <see href="docs.gl/gl4/glDrawTransformFeedbackInstanced">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawTransformFeedbackInstanced(uint mode, uint id, int instancecount) => pfn_glDrawTransformFeedbackInstanced(mode, id, instancecount);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glDrawTransformFeedbackStreamInstanced = null;
    /// <summary> <see href="docs.gl/gl4/glDrawTransformFeedbackStreamInstanced">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDrawTransformFeedbackStreamInstanced(uint mode, uint id, uint stream, int instancecount) => pfn_glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);
    /*  GL_VERSION_4_3 */
    public const uint GL_VERSION_4_3 = 1;
    public const uint GL_NUM_SHADING_LANGUAGE_VERSIONS = 0x82E9;
    public const uint GL_VERTEX_ATTRIB_ARRAY_LONG = 0x874E;
    public const uint GL_COMPRESSED_RGB8_ETC2 = 0x9274;
    public const uint GL_COMPRESSED_SRGB8_ETC2 = 0x9275;
    public const uint GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276;
    public const uint GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
    public const uint GL_COMPRESSED_RGBA8_ETC2_EAC = 0x9278;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9279;
    public const uint GL_COMPRESSED_R11_EAC = 0x9270;
    public const uint GL_COMPRESSED_SIGNED_R11_EAC = 0x9271;
    public const uint GL_COMPRESSED_RG11_EAC = 0x9272;
    public const uint GL_COMPRESSED_SIGNED_RG11_EAC = 0x9273;
    public const uint GL_PRIMITIVE_RESTART_FIXED_INDEX = 0x8D69;
    public const uint GL_ANY_SAMPLES_PASSED_CONSERVATIVE = 0x8D6A;
    public const uint GL_MAX_ELEMENT_INDEX = 0x8D6B;
    public const uint GL_COMPUTE_SHADER = 0x91B9;
    public const uint GL_MAX_COMPUTE_UNIFORM_BLOCKS = 0x91BB;
    public const uint GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS = 0x91BC;
    public const uint GL_MAX_COMPUTE_IMAGE_UNIFORMS = 0x91BD;
    public const uint GL_MAX_COMPUTE_SHARED_MEMORY_SIZE = 0x8262;
    public const uint GL_MAX_COMPUTE_UNIFORM_COMPONENTS = 0x8263;
    public const uint GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS = 0x8264;
    public const uint GL_MAX_COMPUTE_ATOMIC_COUNTERS = 0x8265;
    public const uint GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS = 0x8266;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS = 0x90EB;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_COUNT = 0x91BE;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_SIZE = 0x91BF;
    public const uint GL_COMPUTE_WORK_GROUP_SIZE = 0x8267;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER = 0x90EC;
    public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER = 0x90ED;
    public const uint GL_DISPATCH_INDIRECT_BUFFER = 0x90EE;
    public const uint GL_DISPATCH_INDIRECT_BUFFER_BINDING = 0x90EF;
    public const uint GL_COMPUTE_SHADER_BIT = 0x00000020;
    public const uint GL_DEBUG_OUTPUT_SYNCHRONOUS = 0x8242;
    public const uint GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH = 0x8243;
    public const uint GL_DEBUG_CALLBACK_FUNCTION = 0x8244;
    public const uint GL_DEBUG_CALLBACK_USER_PARAM = 0x8245;
    public const uint GL_DEBUG_SOURCE_API = 0x8246;
    public const uint GL_DEBUG_SOURCE_WINDOW_SYSTEM = 0x8247;
    public const uint GL_DEBUG_SOURCE_SHADER_COMPILER = 0x8248;
    public const uint GL_DEBUG_SOURCE_THIRD_PARTY = 0x8249;
    public const uint GL_DEBUG_SOURCE_APPLICATION = 0x824A;
    public const uint GL_DEBUG_SOURCE_OTHER = 0x824B;
    public const uint GL_DEBUG_TYPE_ERROR = 0x824C;
    public const uint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR = 0x824D;
    public const uint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR = 0x824E;
    public const uint GL_DEBUG_TYPE_PORTABILITY = 0x824F;
    public const uint GL_DEBUG_TYPE_PERFORMANCE = 0x8250;
    public const uint GL_DEBUG_TYPE_OTHER = 0x8251;
    public const uint GL_MAX_DEBUG_MESSAGE_LENGTH = 0x9143;
    public const uint GL_MAX_DEBUG_LOGGED_MESSAGES = 0x9144;
    public const uint GL_DEBUG_LOGGED_MESSAGES = 0x9145;
    public const uint GL_DEBUG_SEVERITY_HIGH = 0x9146;
    public const uint GL_DEBUG_SEVERITY_MEDIUM = 0x9147;
    public const uint GL_DEBUG_SEVERITY_LOW = 0x9148;
    public const uint GL_DEBUG_TYPE_MARKER = 0x8268;
    public const uint GL_DEBUG_TYPE_PUSH_GROUP = 0x8269;
    public const uint GL_DEBUG_TYPE_POP_GROUP = 0x826A;
    public const uint GL_DEBUG_SEVERITY_NOTIFICATION = 0x826B;
    public const uint GL_MAX_DEBUG_GROUP_STACK_DEPTH = 0x826C;
    public const uint GL_DEBUG_GROUP_STACK_DEPTH = 0x826D;
    public const uint GL_BUFFER = 0x82E0;
    public const uint GL_SHADER = 0x82E1;
    public const uint GL_PROGRAM = 0x82E2;
    public const uint GL_QUERY = 0x82E3;
    public const uint GL_PROGRAM_PIPELINE = 0x82E4;
    public const uint GL_SAMPLER = 0x82E6;
    public const uint GL_MAX_LABEL_LENGTH = 0x82E8;
    public const uint GL_DEBUG_OUTPUT = 0x92E0;
    public const uint GL_CONTEXT_FLAG_DEBUG_BIT = 0x00000002;
    public const uint GL_MAX_UNIFORM_LOCATIONS = 0x826E;
    public const uint GL_FRAMEBUFFER_DEFAULT_WIDTH = 0x9310;
    public const uint GL_FRAMEBUFFER_DEFAULT_HEIGHT = 0x9311;
    public const uint GL_FRAMEBUFFER_DEFAULT_LAYERS = 0x9312;
    public const uint GL_FRAMEBUFFER_DEFAULT_SAMPLES = 0x9313;
    public const uint GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS = 0x9314;
    public const uint GL_MAX_FRAMEBUFFER_WIDTH = 0x9315;
    public const uint GL_MAX_FRAMEBUFFER_HEIGHT = 0x9316;
    public const uint GL_MAX_FRAMEBUFFER_LAYERS = 0x9317;
    public const uint GL_MAX_FRAMEBUFFER_SAMPLES = 0x9318;
    public const uint GL_INTERNALFORMAT_SUPPORTED = 0x826F;
    public const uint GL_INTERNALFORMAT_PREFERRED = 0x8270;
    public const uint GL_INTERNALFORMAT_RED_SIZE = 0x8271;
    public const uint GL_INTERNALFORMAT_GREEN_SIZE = 0x8272;
    public const uint GL_INTERNALFORMAT_BLUE_SIZE = 0x8273;
    public const uint GL_INTERNALFORMAT_ALPHA_SIZE = 0x8274;
    public const uint GL_INTERNALFORMAT_DEPTH_SIZE = 0x8275;
    public const uint GL_INTERNALFORMAT_STENCIL_SIZE = 0x8276;
    public const uint GL_INTERNALFORMAT_SHARED_SIZE = 0x8277;
    public const uint GL_INTERNALFORMAT_RED_TYPE = 0x8278;
    public const uint GL_INTERNALFORMAT_GREEN_TYPE = 0x8279;
    public const uint GL_INTERNALFORMAT_BLUE_TYPE = 0x827A;
    public const uint GL_INTERNALFORMAT_ALPHA_TYPE = 0x827B;
    public const uint GL_INTERNALFORMAT_DEPTH_TYPE = 0x827C;
    public const uint GL_INTERNALFORMAT_STENCIL_TYPE = 0x827D;
    public const uint GL_MAX_WIDTH = 0x827E;
    public const uint GL_MAX_HEIGHT = 0x827F;
    public const uint GL_MAX_DEPTH = 0x8280;
    public const uint GL_MAX_LAYERS = 0x8281;
    public const uint GL_MAX_COMBINED_DIMENSIONS = 0x8282;
    public const uint GL_COLOR_COMPONENTS = 0x8283;
    public const uint GL_DEPTH_COMPONENTS = 0x8284;
    public const uint GL_STENCIL_COMPONENTS = 0x8285;
    public const uint GL_COLOR_RENDERABLE = 0x8286;
    public const uint GL_DEPTH_RENDERABLE = 0x8287;
    public const uint GL_STENCIL_RENDERABLE = 0x8288;
    public const uint GL_FRAMEBUFFER_RENDERABLE = 0x8289;
    public const uint GL_FRAMEBUFFER_RENDERABLE_LAYERED = 0x828A;
    public const uint GL_FRAMEBUFFER_BLEND = 0x828B;
    public const uint GL_READ_PIXELS = 0x828C;
    public const uint GL_READ_PIXELS_FORMAT = 0x828D;
    public const uint GL_READ_PIXELS_TYPE = 0x828E;
    public const uint GL_TEXTURE_IMAGE_FORMAT = 0x828F;
    public const uint GL_TEXTURE_IMAGE_TYPE = 0x8290;
    public const uint GL_GET_TEXTURE_IMAGE_FORMAT = 0x8291;
    public const uint GL_GET_TEXTURE_IMAGE_TYPE = 0x8292;
    public const uint GL_MIPMAP = 0x8293;
    public const uint GL_MANUAL_GENERATE_MIPMAP = 0x8294;
    public const uint GL_AUTO_GENERATE_MIPMAP = 0x8295;
    public const uint GL_COLOR_ENCODING = 0x8296;
    public const uint GL_SRGB_READ = 0x8297;
    public const uint GL_SRGB_WRITE = 0x8298;
    public const uint GL_FILTER = 0x829A;
    public const uint GL_VERTEX_TEXTURE = 0x829B;
    public const uint GL_TESS_CONTROL_TEXTURE = 0x829C;
    public const uint GL_TESS_EVALUATION_TEXTURE = 0x829D;
    public const uint GL_GEOMETRY_TEXTURE = 0x829E;
    public const uint GL_FRAGMENT_TEXTURE = 0x829F;
    public const uint GL_COMPUTE_TEXTURE = 0x82A0;
    public const uint GL_TEXTURE_SHADOW = 0x82A1;
    public const uint GL_TEXTURE_GATHER = 0x82A2;
    public const uint GL_TEXTURE_GATHER_SHADOW = 0x82A3;
    public const uint GL_SHADER_IMAGE_LOAD = 0x82A4;
    public const uint GL_SHADER_IMAGE_STORE = 0x82A5;
    public const uint GL_SHADER_IMAGE_ATOMIC = 0x82A6;
    public const uint GL_IMAGE_TEXEL_SIZE = 0x82A7;
    public const uint GL_IMAGE_COMPATIBILITY_CLASS = 0x82A8;
    public const uint GL_IMAGE_PIXEL_FORMAT = 0x82A9;
    public const uint GL_IMAGE_PIXEL_TYPE = 0x82AA;
    public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST = 0x82AC;
    public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST = 0x82AD;
    public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE = 0x82AE;
    public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE = 0x82AF;
    public const uint GL_TEXTURE_COMPRESSED_BLOCK_WIDTH = 0x82B1;
    public const uint GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT = 0x82B2;
    public const uint GL_TEXTURE_COMPRESSED_BLOCK_SIZE = 0x82B3;
    public const uint GL_CLEAR_BUFFER = 0x82B4;
    public const uint GL_TEXTURE_VIEW = 0x82B5;
    public const uint GL_VIEW_COMPATIBILITY_CLASS = 0x82B6;
    public const uint GL_FULL_SUPPORT = 0x82B7;
    public const uint GL_CAVEAT_SUPPORT = 0x82B8;
    public const uint GL_IMAGE_CLASS_4_X_32 = 0x82B9;
    public const uint GL_IMAGE_CLASS_2_X_32 = 0x82BA;
    public const uint GL_IMAGE_CLASS_1_X_32 = 0x82BB;
    public const uint GL_IMAGE_CLASS_4_X_16 = 0x82BC;
    public const uint GL_IMAGE_CLASS_2_X_16 = 0x82BD;
    public const uint GL_IMAGE_CLASS_1_X_16 = 0x82BE;
    public const uint GL_IMAGE_CLASS_4_X_8 = 0x82BF;
    public const uint GL_IMAGE_CLASS_2_X_8 = 0x82C0;
    public const uint GL_IMAGE_CLASS_1_X_8 = 0x82C1;
    public const uint GL_IMAGE_CLASS_11_11_10 = 0x82C2;
    public const uint GL_IMAGE_CLASS_10_10_10_2 = 0x82C3;
    public const uint GL_VIEW_CLASS_128_BITS = 0x82C4;
    public const uint GL_VIEW_CLASS_96_BITS = 0x82C5;
    public const uint GL_VIEW_CLASS_64_BITS = 0x82C6;
    public const uint GL_VIEW_CLASS_48_BITS = 0x82C7;
    public const uint GL_VIEW_CLASS_32_BITS = 0x82C8;
    public const uint GL_VIEW_CLASS_24_BITS = 0x82C9;
    public const uint GL_VIEW_CLASS_16_BITS = 0x82CA;
    public const uint GL_VIEW_CLASS_8_BITS = 0x82CB;
    public const uint GL_VIEW_CLASS_S3TC_DXT1_RGB = 0x82CC;
    public const uint GL_VIEW_CLASS_S3TC_DXT1_RGBA = 0x82CD;
    public const uint GL_VIEW_CLASS_S3TC_DXT3_RGBA = 0x82CE;
    public const uint GL_VIEW_CLASS_S3TC_DXT5_RGBA = 0x82CF;
    public const uint GL_VIEW_CLASS_RGTC1_RED = 0x82D0;
    public const uint GL_VIEW_CLASS_RGTC2_RG = 0x82D1;
    public const uint GL_VIEW_CLASS_BPTC_UNORM = 0x82D2;
    public const uint GL_VIEW_CLASS_BPTC_FLOAT = 0x82D3;
    public const uint GL_UNIFORM = 0x92E1;
    public const uint GL_UNIFORM_BLOCK = 0x92E2;
    public const uint GL_PROGRAM_INPUT = 0x92E3;
    public const uint GL_PROGRAM_OUTPUT = 0x92E4;
    public const uint GL_BUFFER_VARIABLE = 0x92E5;
    public const uint GL_SHADER_STORAGE_BLOCK = 0x92E6;
    public const uint GL_VERTEX_SUBROUTINE = 0x92E8;
    public const uint GL_TESS_CONTROL_SUBROUTINE = 0x92E9;
    public const uint GL_TESS_EVALUATION_SUBROUTINE = 0x92EA;
    public const uint GL_GEOMETRY_SUBROUTINE = 0x92EB;
    public const uint GL_FRAGMENT_SUBROUTINE = 0x92EC;
    public const uint GL_COMPUTE_SUBROUTINE = 0x92ED;
    public const uint GL_VERTEX_SUBROUTINE_UNIFORM = 0x92EE;
    public const uint GL_TESS_CONTROL_SUBROUTINE_UNIFORM = 0x92EF;
    public const uint GL_TESS_EVALUATION_SUBROUTINE_UNIFORM = 0x92F0;
    public const uint GL_GEOMETRY_SUBROUTINE_UNIFORM = 0x92F1;
    public const uint GL_FRAGMENT_SUBROUTINE_UNIFORM = 0x92F2;
    public const uint GL_COMPUTE_SUBROUTINE_UNIFORM = 0x92F3;
    public const uint GL_TRANSFORM_FEEDBACK_VARYING = 0x92F4;
    public const uint GL_ACTIVE_RESOURCES = 0x92F5;
    public const uint GL_MAX_NAME_LENGTH = 0x92F6;
    public const uint GL_MAX_NUM_ACTIVE_VARIABLES = 0x92F7;
    public const uint GL_MAX_NUM_COMPATIBLE_SUBROUTINES = 0x92F8;
    public const uint GL_NAME_LENGTH = 0x92F9;
    public const uint GL_TYPE = 0x92FA;
    public const uint GL_ARRAY_SIZE = 0x92FB;
    public const uint GL_OFFSET = 0x92FC;
    public const uint GL_BLOCK_INDEX = 0x92FD;
    public const uint GL_ARRAY_STRIDE = 0x92FE;
    public const uint GL_MATRIX_STRIDE = 0x92FF;
    public const uint GL_IS_ROW_MAJOR = 0x9300;
    public const uint GL_ATOMIC_COUNTER_BUFFER_INDEX = 0x9301;
    public const uint GL_BUFFER_BINDING = 0x9302;
    public const uint GL_BUFFER_DATA_SIZE = 0x9303;
    public const uint GL_NUM_ACTIVE_VARIABLES = 0x9304;
    public const uint GL_ACTIVE_VARIABLES = 0x9305;
    public const uint GL_REFERENCED_BY_VERTEX_SHADER = 0x9306;
    public const uint GL_REFERENCED_BY_TESS_CONTROL_SHADER = 0x9307;
    public const uint GL_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x9308;
    public const uint GL_REFERENCED_BY_GEOMETRY_SHADER = 0x9309;
    public const uint GL_REFERENCED_BY_FRAGMENT_SHADER = 0x930A;
    public const uint GL_REFERENCED_BY_COMPUTE_SHADER = 0x930B;
    public const uint GL_TOP_LEVEL_ARRAY_SIZE = 0x930C;
    public const uint GL_TOP_LEVEL_ARRAY_STRIDE = 0x930D;
    public const uint GL_LOCATION = 0x930E;
    public const uint GL_LOCATION_INDEX = 0x930F;
    public const uint GL_IS_PER_PATCH = 0x92E7;
    public const uint GL_SHADER_STORAGE_BUFFER = 0x90D2;
    public const uint GL_SHADER_STORAGE_BUFFER_BINDING = 0x90D3;
    public const uint GL_SHADER_STORAGE_BUFFER_START = 0x90D4;
    public const uint GL_SHADER_STORAGE_BUFFER_SIZE = 0x90D5;
    public const uint GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS = 0x90D6;
    public const uint GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS = 0x90D7;
    public const uint GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS = 0x90D8;
    public const uint GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS = 0x90D9;
    public const uint GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS = 0x90DA;
    public const uint GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS = 0x90DB;
    public const uint GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS = 0x90DC;
    public const uint GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS = 0x90DD;
    public const uint GL_MAX_SHADER_STORAGE_BLOCK_SIZE = 0x90DE;
    public const uint GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT = 0x90DF;
    public const uint GL_SHADER_STORAGE_BARRIER_BIT = 0x00002000;
    public const uint GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES = 0x8F39;
    public const uint GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA;
    public const uint GL_TEXTURE_BUFFER_OFFSET = 0x919D;
    public const uint GL_TEXTURE_BUFFER_SIZE = 0x919E;
    public const uint GL_TEXTURE_BUFFER_OFFSET_ALIGNMENT = 0x919F;
    public const uint GL_TEXTURE_VIEW_MIN_LEVEL = 0x82DB;
    public const uint GL_TEXTURE_VIEW_NUM_LEVELS = 0x82DC;
    public const uint GL_TEXTURE_VIEW_MIN_LAYER = 0x82DD;
    public const uint GL_TEXTURE_VIEW_NUM_LAYERS = 0x82DE;
    public const uint GL_TEXTURE_IMMUTABLE_LEVELS = 0x82DF;
    public const uint GL_VERTEX_ATTRIB_BINDING = 0x82D4;
    public const uint GL_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D5;
    public const uint GL_VERTEX_BINDING_DIVISOR = 0x82D6;
    public const uint GL_VERTEX_BINDING_OFFSET = 0x82D7;
    public const uint GL_VERTEX_BINDING_STRIDE = 0x82D8;
    public const uint GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D9;
    public const uint GL_MAX_VERTEX_ATTRIB_BINDINGS = 0x82DA;
    public const uint GL_VERTEX_BINDING_BUFFER = 0x8F4F;

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void*, void> pfn_glClearBufferData = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferData(uint target, uint internalformat, uint format, uint type, void* data) => pfn_glClearBufferData(target, internalformat, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, long, long, uint, uint, void*, void> pfn_glClearBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glClearBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearBufferSubData(uint target, uint internalformat, long offset, long size, uint format, uint type, void* data) => pfn_glClearBufferSubData(target, internalformat, offset, size, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glDispatchCompute = null;
    /// <summary> <see href="docs.gl/gl4/glDispatchCompute">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) => pfn_glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);

    private static delegate* unmanaged[Stdcall]<long, void> pfn_glDispatchComputeIndirect = null;
    /// <summary> <see href="docs.gl/gl4/glDispatchComputeIndirect">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDispatchComputeIndirect(long indirect) => pfn_glDispatchComputeIndirect(indirect);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, uint, uint, int, int, int, int, int, int, int, void> pfn_glCopyImageSubData = null;
    /// <summary> <see href="docs.gl/gl4/glCopyImageSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => pfn_glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glFramebufferParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glFramebufferParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFramebufferParameteri(uint target, uint pname, int param) => pfn_glFramebufferParameteri(target, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetFramebufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetFramebufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetFramebufferParameteriv(uint target, uint pname, int* @params) => pfn_glGetFramebufferParameteriv(target, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, long*, void> pfn_glGetInternalformati64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetInternalformati64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetInternalformati64v(uint target, uint internalformat, uint pname, int count, long* @params) => pfn_glGetInternalformati64v(target, internalformat, pname, count, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, void> pfn_glInvalidateTexSubImage = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateTexSubImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) => pfn_glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth);

    private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glInvalidateTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateTexImage(uint texture, int level) => pfn_glInvalidateTexImage(texture, level);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void> pfn_glInvalidateBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateBufferSubData(uint buffer, long offset, long length) => pfn_glInvalidateBufferSubData(buffer, offset, length);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glInvalidateBufferData = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateBufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateBufferData(uint buffer) => pfn_glInvalidateBufferData(buffer);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glInvalidateFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateFramebuffer(uint target, int numAttachments, uint* attachments) => pfn_glInvalidateFramebuffer(target, numAttachments, attachments);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, int, int, int, int, void> pfn_glInvalidateSubFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateSubFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateSubFramebuffer(uint target, int numAttachments, uint* attachments, int x, int y, int width, int height) => pfn_glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, void*, int, int, void> pfn_glMultiDrawArraysIndirect = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawArraysIndirect">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawArraysIndirect(uint mode, void* indirect, int drawcount, int stride) => pfn_glMultiDrawArraysIndirect(mode, indirect, drawcount, stride);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*, int, int, void> pfn_glMultiDrawElementsIndirect = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsIndirect">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawElementsIndirect(uint mode, uint type, void* indirect, int drawcount, int stride) => pfn_glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetProgramInterfaceiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramInterfaceiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, int* @params) => pfn_glGetProgramInterfaceiv(program, programInterface, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, uint> pfn_glGetProgramResourceIndex = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramResourceIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetProgramResourceIndex(uint program, uint programInterface, byte* name) => pfn_glGetProgramResourceIndex(program, programInterface, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, byte*, void> pfn_glGetProgramResourceName = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramResourceName">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, int* length, byte* name) => pfn_glGetProgramResourceName(program, programInterface, index, bufSize, length, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, int, int*, int*, void> pfn_glGetProgramResourceiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramResourceiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, uint* props, int count, int* length, int* @params) => pfn_glGetProgramResourceiv(program, programInterface, index, propCount, props, count, length, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, int> pfn_glGetProgramResourceLocation = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramResourceLocation">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetProgramResourceLocation(uint program, uint programInterface, byte* name) => pfn_glGetProgramResourceLocation(program, programInterface, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, byte*, int> pfn_glGetProgramResourceLocationIndex = null;
    /// <summary> <see href="docs.gl/gl4/glGetProgramResourceLocationIndex">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int glGetProgramResourceLocationIndex(uint program, uint programInterface, byte* name) => pfn_glGetProgramResourceLocationIndex(program, programInterface, name);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glShaderStorageBlockBinding = null;
    /// <summary> <see href="docs.gl/gl4/glShaderStorageBlockBinding">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => pfn_glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, long, void> pfn_glTexBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glTexBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexBufferRange(uint target, uint internalformat, uint buffer, long offset, long size) => pfn_glTexBufferRange(target, internalformat, buffer, offset, size);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, byte, void> pfn_glTexStorage2DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTexStorage2DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, byte fixedsamplelocations) => pfn_glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, byte, void> pfn_glTexStorage3DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTexStorage3DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations) => pfn_glTexStorage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, uint, uint, uint, void> pfn_glTextureView = null;
    /// <summary> <see href="docs.gl/gl4/glTextureView">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) => pfn_glTextureView(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);

    private static delegate* unmanaged[Stdcall]<uint, uint, long, int, void> pfn_glBindVertexBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBindVertexBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindVertexBuffer(uint bindingindex, uint buffer, long offset, int stride) => pfn_glBindVertexBuffer(bindingindex, buffer, offset, stride);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, byte, uint, void> pfn_glVertexAttribFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribFormat(uint attribindex, int size, uint type, byte normalized, uint relativeoffset) => pfn_glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void> pfn_glVertexAttribIFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribIFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexAttribIFormat(attribindex, size, type, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void> pfn_glVertexAttribLFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribLFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexAttribLFormat(attribindex, size, type, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexAttribBinding = null;
    /// <summary> <see href="docs.gl/gl4/glVertexAttribBinding">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexAttribBinding(uint attribindex, uint bindingindex) => pfn_glVertexAttribBinding(attribindex, bindingindex);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexBindingDivisor = null;
    /// <summary> <see href="docs.gl/gl4/glVertexBindingDivisor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexBindingDivisor(uint bindingindex, uint divisor) => pfn_glVertexBindingDivisor(bindingindex, divisor);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, byte, void> pfn_glDebugMessageControl = null;
    /// <summary> <see href="docs.gl/gl4/glDebugMessageControl">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDebugMessageControl(uint source, uint type, uint severity, int count, uint* ids, byte enabled) => pfn_glDebugMessageControl(source, type, severity, count, ids, enabled);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, byte*, void> pfn_glDebugMessageInsert = null;
    /// <summary> <see href="docs.gl/gl4/glDebugMessageInsert">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, byte* buf) => pfn_glDebugMessageInsert(source, type, id, severity, length, buf);

    private static delegate* unmanaged[Stdcall]<delegate* unmanaged[Stdcall]<uint, uint, uint, uint, long, byte*, void*, void>, void*, void> pfn_glDebugMessageCallback = null;
    /// <summary> <see href="docs.gl/gl4/glDebugMessageCallback">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDebugMessageCallback(delegate* unmanaged[Stdcall]<uint, uint, uint, uint, long, byte*, void*, void> callback, void* userParam) => pfn_glDebugMessageCallback(callback, userParam);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, uint*, uint*, uint*, int*, byte*, uint> pfn_glGetDebugMessageLog = null;
    /// <summary> <see href="docs.gl/gl4/glGetDebugMessageLog">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetDebugMessageLog(uint count, int bufSize, uint* sources, uint* types, uint* ids, uint* severities, int* lengths, byte* messageLog) => pfn_glGetDebugMessageLog(count, bufSize, sources, types, ids, severities, lengths, messageLog);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, byte*, void> pfn_glPushDebugGroup = null;
    /// <summary> <see href="docs.gl/gl4/glPushDebugGroup">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPushDebugGroup(uint source, uint id, int length, byte* message) => pfn_glPushDebugGroup(source, id, length, message);

    private static delegate* unmanaged[Stdcall]<void> pfn_glPopDebugGroup = null;
    /// <summary> <see href="docs.gl/gl4/glPopDebugGroup">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPopDebugGroup() => pfn_glPopDebugGroup();

    private static delegate* unmanaged[Stdcall]<uint, uint, int, byte*, void> pfn_glObjectLabel = null;
    /// <summary> <see href="docs.gl/gl4/glObjectLabel">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glObjectLabel(uint identifier, uint name, int length, byte* label) => pfn_glObjectLabel(identifier, name, length, label);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void> pfn_glGetObjectLabel = null;
    /// <summary> <see href="docs.gl/gl4/glGetObjectLabel">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetObjectLabel(uint identifier, uint name, int bufSize, int* length, byte* label) => pfn_glGetObjectLabel(identifier, name, bufSize, length, label);

    private static delegate* unmanaged[Stdcall]<void*, int, byte*, void> pfn_glObjectPtrLabel = null;
    /// <summary> <see href="docs.gl/gl4/glObjectPtrLabel">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glObjectPtrLabel(void* ptr, int length, byte* label) => pfn_glObjectPtrLabel(ptr, length, label);

    private static delegate* unmanaged[Stdcall]<void*, int, int*, byte*, void> pfn_glGetObjectPtrLabel = null;
    /// <summary> <see href="docs.gl/gl4/glGetObjectPtrLabel">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetObjectPtrLabel(void* ptr, int bufSize, int* length, byte* label) => pfn_glGetObjectPtrLabel(ptr, bufSize, length, label);
    /*  GL_VERSION_4_4 */
    public const uint GL_VERSION_4_4 = 1;
    public const uint GL_MAX_VERTEX_ATTRIB_STRIDE = 0x82E5;
    public const uint GL_PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED = 0x8221;
    public const uint GL_TEXTURE_BUFFER_BINDING = 0x8C2A;
    public const uint GL_MAP_PERSISTENT_BIT = 0x0040;
    public const uint GL_MAP_COHERENT_BIT = 0x0080;
    public const uint GL_DYNAMIC_STORAGE_BIT = 0x0100;
    public const uint GL_CLIENT_STORAGE_BIT = 0x0200;
    public const uint GL_CLIENT_MAPPED_BUFFER_BARRIER_BIT = 0x00004000;
    public const uint GL_BUFFER_IMMUTABLE_STORAGE = 0x821F;
    public const uint GL_BUFFER_STORAGE_FLAGS = 0x8220;
    public const uint GL_CLEAR_TEXTURE = 0x9365;
    public const uint GL_LOCATION_COMPONENT = 0x934A;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_INDEX = 0x934B;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_STRIDE = 0x934C;
    public const uint GL_QUERY_BUFFER = 0x9192;
    public const uint GL_QUERY_BUFFER_BARRIER_BIT = 0x00008000;
    public const uint GL_QUERY_BUFFER_BINDING = 0x9193;
    public const uint GL_QUERY_RESULT_NO_WAIT = 0x9194;
    public const uint GL_MIRROR_CLAMP_TO_EDGE = 0x8743;

    private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glBufferStorage = null;
    /// <summary> <see href="docs.gl/gl4/glBufferStorage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBufferStorage(uint target, long size, void* data, uint flags) => pfn_glBufferStorage(target, size, data, flags);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void*, void> pfn_glClearTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glClearTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearTexImage(uint texture, int level, uint format, uint type, void* data) => pfn_glClearTexImage(texture, level, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, uint, void*, void> pfn_glClearTexSubImage = null;
    /// <summary> <see href="docs.gl/gl4/glClearTexSubImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* data) => pfn_glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, void> pfn_glBindBuffersBase = null;
    /// <summary> <see href="docs.gl/gl4/glBindBuffersBase">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindBuffersBase(uint target, uint first, int count, uint* buffers) => pfn_glBindBuffersBase(target, first, count, buffers);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, long*, long*, void> pfn_glBindBuffersRange = null;
    /// <summary> <see href="docs.gl/gl4/glBindBuffersRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindBuffersRange(uint target, uint first, int count, uint* buffers, long* offsets, long* sizes) => pfn_glBindBuffersRange(target, first, count, buffers, offsets, sizes);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glBindTextures = null;
    /// <summary> <see href="docs.gl/gl4/glBindTextures">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindTextures(uint first, int count, uint* textures) => pfn_glBindTextures(first, count, textures);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glBindSamplers = null;
    /// <summary> <see href="docs.gl/gl4/glBindSamplers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindSamplers(uint first, int count, uint* samplers) => pfn_glBindSamplers(first, count, samplers);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glBindImageTextures = null;
    /// <summary> <see href="docs.gl/gl4/glBindImageTextures">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindImageTextures(uint first, int count, uint* textures) => pfn_glBindImageTextures(first, count, textures);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, long*, int*, void> pfn_glBindVertexBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glBindVertexBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindVertexBuffers(uint first, int count, uint* buffers, long* offsets, int* strides) => pfn_glBindVertexBuffers(first, count, buffers, offsets, strides);
    /*  GL_VERSION_4_5 */
    public const uint GL_VERSION_4_5 = 1;
    public const uint GL_CONTEXT_LOST = 0x0507;
    public const uint GL_NEGATIVE_ONE_TO_ONE = 0x935E;
    public const uint GL_ZERO_TO_ONE = 0x935F;
    public const uint GL_CLIP_ORIGIN = 0x935C;
    public const uint GL_CLIP_DEPTH_MODE = 0x935D;
    public const uint GL_QUERY_WAIT_INVERTED = 0x8E17;
    public const uint GL_QUERY_NO_WAIT_INVERTED = 0x8E18;
    public const uint GL_QUERY_BY_REGION_WAIT_INVERTED = 0x8E19;
    public const uint GL_QUERY_BY_REGION_NO_WAIT_INVERTED = 0x8E1A;
    public const uint GL_MAX_CULL_DISTANCES = 0x82F9;
    public const uint GL_MAX_COMBINED_CLIP_AND_CULL_DISTANCES = 0x82FA;
    public const uint GL_TEXTURE_TARGET = 0x1006;
    public const uint GL_QUERY_TARGET = 0x82EA;
    public const uint GL_GUILTY_CONTEXT_RESET = 0x8253;
    public const uint GL_INNOCENT_CONTEXT_RESET = 0x8254;
    public const uint GL_UNKNOWN_CONTEXT_RESET = 0x8255;
    public const uint GL_RESET_NOTIFICATION_STRATEGY = 0x8256;
    public const uint GL_LOSE_CONTEXT_ON_RESET = 0x8252;
    public const uint GL_NO_RESET_NOTIFICATION = 0x8261;
    public const uint GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT = 0x00000004;
    public const uint GL_CONTEXT_RELEASE_BEHAVIOR = 0x82FB;
    public const uint GL_CONTEXT_RELEASE_BEHAVIOR_FLUSH = 0x82FC;

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glClipControl = null;
    /// <summary> <see href="docs.gl/gl4/glClipControl">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClipControl(uint origin, uint depth) => pfn_glClipControl(origin, depth);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateTransformFeedbacks = null;
    /// <summary> <see href="docs.gl/gl4/glCreateTransformFeedbacks">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateTransformFeedbacks(int n, uint* ids) => pfn_glCreateTransformFeedbacks(n, ids);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glTransformFeedbackBufferBase = null;
    /// <summary> <see href="docs.gl/gl4/glTransformFeedbackBufferBase">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => pfn_glTransformFeedbackBufferBase(xfb, index, buffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, long, void> pfn_glTransformFeedbackBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glTransformFeedbackBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, long offset, long size) => pfn_glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetTransformFeedbackiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTransformFeedbackiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTransformFeedbackiv(uint xfb, uint pname, int* param) => pfn_glGetTransformFeedbackiv(xfb, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetTransformFeedbacki_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetTransformFeedbacki_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTransformFeedbacki_v(uint xfb, uint pname, uint index, int* param) => pfn_glGetTransformFeedbacki_v(xfb, pname, index, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long*, void> pfn_glGetTransformFeedbacki64_v = null;
    /// <summary> <see href="docs.gl/gl4/glGetTransformFeedbacki64_v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTransformFeedbacki64_v(uint xfb, uint pname, uint index, long* param) => pfn_glGetTransformFeedbacki64_v(xfb, pname, index, param);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glCreateBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateBuffers(int n, uint* buffers) => pfn_glCreateBuffers(n, buffers);

    private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glNamedBufferStorage = null;
    /// <summary> <see href="docs.gl/gl4/glNamedBufferStorage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedBufferStorage(uint buffer, long size, void* data, uint flags) => pfn_glNamedBufferStorage(buffer, size, data, flags);

    private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glNamedBufferData = null;
    /// <summary> <see href="docs.gl/gl4/glNamedBufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedBufferData(uint buffer, long size, void* data, uint usage) => pfn_glNamedBufferData(buffer, size, data, usage);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glNamedBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glNamedBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedBufferSubData(uint buffer, long offset, long size, void* data) => pfn_glNamedBufferSubData(buffer, offset, size, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, long, long, long, void> pfn_glCopyNamedBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glCopyNamedBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, long readOffset, long writeOffset, long size) => pfn_glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void*, void> pfn_glClearNamedBufferData = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedBufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, void* data) => pfn_glClearNamedBufferData(buffer, internalformat, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, long, long, uint, uint, void*, void> pfn_glClearNamedBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedBufferSubData(uint buffer, uint internalformat, long offset, long size, uint format, uint type, void* data) => pfn_glClearNamedBufferSubData(buffer, internalformat, offset, size, format, type, data);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*> pfn_glMapNamedBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glMapNamedBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void* glMapNamedBuffer(uint buffer, uint access) => pfn_glMapNamedBuffer(buffer, access);

    private static delegate* unmanaged[Stdcall]<uint, long, long, uint, void*> pfn_glMapNamedBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glMapNamedBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void* glMapNamedBufferRange(uint buffer, long offset, long length, uint access) => pfn_glMapNamedBufferRange(buffer, offset, length, access);

    private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glUnmapNamedBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glUnmapNamedBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte glUnmapNamedBuffer(uint buffer) => pfn_glUnmapNamedBuffer(buffer);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void> pfn_glFlushMappedNamedBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glFlushMappedNamedBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glFlushMappedNamedBufferRange(uint buffer, long offset, long length) => pfn_glFlushMappedNamedBufferRange(buffer, offset, length);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedBufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedBufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedBufferParameteriv(uint buffer, uint pname, int* @params) => pfn_glGetNamedBufferParameteriv(buffer, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, long*, void> pfn_glGetNamedBufferParameteri64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedBufferParameteri64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedBufferParameteri64v(uint buffer, uint pname, long* @params) => pfn_glGetNamedBufferParameteri64v(buffer, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetNamedBufferPointerv = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedBufferPointerv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedBufferPointerv(uint buffer, uint pname, void** @params) => pfn_glGetNamedBufferPointerv(buffer, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glGetNamedBufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedBufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedBufferSubData(uint buffer, long offset, long size, void* data) => pfn_glGetNamedBufferSubData(buffer, offset, size, data);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateFramebuffers = null;
    /// <summary> <see href="docs.gl/gl4/glCreateFramebuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateFramebuffers(int n, uint* framebuffers) => pfn_glCreateFramebuffers(n, framebuffers);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glNamedFramebufferRenderbuffer = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferRenderbuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer) => pfn_glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glNamedFramebufferParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferParameteri(uint framebuffer, uint pname, int param) => pfn_glNamedFramebufferParameteri(framebuffer, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glNamedFramebufferTexture = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTexture">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level) => pfn_glNamedFramebufferTexture(framebuffer, attachment, texture, level);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void> pfn_glNamedFramebufferTextureLayer = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTextureLayer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer) => pfn_glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glNamedFramebufferDrawBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferDrawBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferDrawBuffer(uint framebuffer, uint buf) => pfn_glNamedFramebufferDrawBuffer(framebuffer, buf);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glNamedFramebufferDrawBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferDrawBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferDrawBuffers(uint framebuffer, int n, uint* bufs) => pfn_glNamedFramebufferDrawBuffers(framebuffer, n, bufs);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glNamedFramebufferReadBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glNamedFramebufferReadBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedFramebufferReadBuffer(uint framebuffer, uint src) => pfn_glNamedFramebufferReadBuffer(framebuffer, src);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glInvalidateNamedFramebufferData = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateNamedFramebufferData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, uint* attachments) => pfn_glInvalidateNamedFramebufferData(framebuffer, numAttachments, attachments);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, int, int, int, int, void> pfn_glInvalidateNamedFramebufferSubData = null;
    /// <summary> <see href="docs.gl/gl4/glInvalidateNamedFramebufferSubData">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, uint* attachments, int x, int y, int width, int height) => pfn_glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, attachments, x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, void> pfn_glClearNamedFramebufferiv = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedFramebufferiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, int* value) => pfn_glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, void> pfn_glClearNamedFramebufferuiv = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedFramebufferuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, uint* value) => pfn_glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, float*, void> pfn_glClearNamedFramebufferfv = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedFramebufferfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, float* value) => pfn_glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, value);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, float, int, void> pfn_glClearNamedFramebufferfi = null;
    /// <summary> <see href="docs.gl/gl4/glClearNamedFramebufferfi">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil) => pfn_glClearNamedFramebufferfi(framebuffer, buffer, drawbuffer, depth, stencil);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, int, uint, uint, void> pfn_glBlitNamedFramebuffer = null;
    /// <summary> <see href="docs.gl/gl4/glBlitNamedFramebuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => pfn_glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint> pfn_glCheckNamedFramebufferStatus = null;
    /// <summary> <see href="docs.gl/gl4/glCheckNamedFramebufferStatus">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glCheckNamedFramebufferStatus(uint framebuffer, uint target) => pfn_glCheckNamedFramebufferStatus(framebuffer, target);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedFramebufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedFramebufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedFramebufferParameteriv(uint framebuffer, uint pname, int* param) => pfn_glGetNamedFramebufferParameteriv(framebuffer, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetNamedFramebufferAttachmentParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedFramebufferAttachmentParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, int* @params) => pfn_glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, @params);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateRenderbuffers = null;
    /// <summary> <see href="docs.gl/gl4/glCreateRenderbuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateRenderbuffers(int n, uint* renderbuffers) => pfn_glCreateRenderbuffers(n, renderbuffers);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, int, void> pfn_glNamedRenderbufferStorage = null;
    /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorage(renderbuffer, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void> pfn_glNamedRenderbufferStorageMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorageMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedRenderbufferParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetNamedRenderbufferParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, int* @params) => pfn_glGetNamedRenderbufferParameteriv(renderbuffer, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glCreateTextures = null;
    /// <summary> <see href="docs.gl/gl4/glCreateTextures">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateTextures(uint target, int n, uint* textures) => pfn_glCreateTextures(target, n, textures);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glTextureBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glTextureBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureBuffer(uint texture, uint internalformat, uint buffer) => pfn_glTextureBuffer(texture, internalformat, buffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, long, void> pfn_glTextureBufferRange = null;
    /// <summary> <see href="docs.gl/gl4/glTextureBufferRange">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureBufferRange(uint texture, uint internalformat, uint buffer, long offset, long size) => pfn_glTextureBufferRange(texture, internalformat, buffer, offset, size);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void> pfn_glTextureStorage1D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureStorage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureStorage1D(uint texture, int levels, uint internalformat, int width) => pfn_glTextureStorage1D(texture, levels, internalformat, width);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void> pfn_glTextureStorage2D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureStorage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureStorage2D(uint texture, int levels, uint internalformat, int width, int height) => pfn_glTextureStorage2D(texture, levels, internalformat, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, void> pfn_glTextureStorage3D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureStorage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth) => pfn_glTextureStorage3D(texture, levels, internalformat, width, height, depth);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, byte, void> pfn_glTextureStorage2DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTextureStorage2DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, byte fixedsamplelocations) => pfn_glTextureStorage2DMultisample(texture, samples, internalformat, width, height, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, byte, void> pfn_glTextureStorage3DMultisample = null;
    /// <summary> <see href="docs.gl/gl4/glTextureStorage3DMultisample">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations) => pfn_glTextureStorage3DMultisample(texture, samples, internalformat, width, height, depth, fixedsamplelocations);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, void* pixels) => pfn_glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) => pfn_glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glTextureSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels) => pfn_glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, void* data) => pfn_glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* data) => pfn_glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* data) => pfn_glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, void> pfn_glCopyTextureSubImage1D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage1D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => pfn_glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, void> pfn_glCopyTextureSubImage2D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage2D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => pfn_glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, void> pfn_glCopyTextureSubImage3D = null;
    /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage3D">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => pfn_glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);

    private static delegate* unmanaged[Stdcall]<uint, uint, float, void> pfn_glTextureParameterf = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameterf">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameterf(uint texture, uint pname, float param) => pfn_glTextureParameterf(texture, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glTextureParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameterfv(uint texture, uint pname, float* param) => pfn_glTextureParameterfv(texture, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glTextureParameteri = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameteri">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameteri(uint texture, uint pname, int param) => pfn_glTextureParameteri(texture, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glTextureParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameterIiv(uint texture, uint pname, int* @params) => pfn_glTextureParameterIiv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glTextureParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameterIuiv(uint texture, uint pname, uint* @params) => pfn_glTextureParameterIuiv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glTextureParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glTextureParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureParameteriv(uint texture, uint pname, int* param) => pfn_glTextureParameteriv(texture, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glGenerateTextureMipmap = null;
    /// <summary> <see href="docs.gl/gl4/glGenerateTextureMipmap">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGenerateTextureMipmap(uint texture) => pfn_glGenerateTextureMipmap(texture);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBindTextureUnit = null;
    /// <summary> <see href="docs.gl/gl4/glBindTextureUnit">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glBindTextureUnit(uint unit, uint texture) => pfn_glBindTextureUnit(unit, texture);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, int, void*, void> pfn_glGetTextureImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureImage(uint texture, int level, uint format, uint type, int bufSize, void* pixels) => pfn_glGetTextureImage(texture, level, format, type, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, void*, void> pfn_glGetCompressedTextureImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetCompressedTextureImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetCompressedTextureImage(uint texture, int level, int bufSize, void* pixels) => pfn_glGetCompressedTextureImage(texture, level, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, float*, void> pfn_glGetTextureLevelParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureLevelParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureLevelParameterfv(uint texture, int level, uint pname, float* @params) => pfn_glGetTextureLevelParameterfv(texture, level, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, int*, void> pfn_glGetTextureLevelParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureLevelParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureLevelParameteriv(uint texture, int level, uint pname, int* @params) => pfn_glGetTextureLevelParameteriv(texture, level, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetTextureParameterfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureParameterfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureParameterfv(uint texture, uint pname, float* @params) => pfn_glGetTextureParameterfv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetTextureParameterIiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureParameterIiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureParameterIiv(uint texture, uint pname, int* @params) => pfn_glGetTextureParameterIiv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetTextureParameterIuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureParameterIuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureParameterIuiv(uint texture, uint pname, uint* @params) => pfn_glGetTextureParameterIuiv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetTextureParameteriv = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureParameteriv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureParameteriv(uint texture, uint pname, int* @params) => pfn_glGetTextureParameteriv(texture, pname, @params);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateVertexArrays = null;
    /// <summary> <see href="docs.gl/gl4/glCreateVertexArrays">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateVertexArrays(int n, uint* arrays) => pfn_glCreateVertexArrays(n, arrays);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableVertexArrayAttrib = null;
    /// <summary> <see href="docs.gl/gl4/glDisableVertexArrayAttrib">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glDisableVertexArrayAttrib(uint vaobj, uint index) => pfn_glDisableVertexArrayAttrib(vaobj, index);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableVertexArrayAttrib = null;
    /// <summary> <see href="docs.gl/gl4/glEnableVertexArrayAttrib">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glEnableVertexArrayAttrib(uint vaobj, uint index) => pfn_glEnableVertexArrayAttrib(vaobj, index);

    private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexArrayElementBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayElementBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayElementBuffer(uint vaobj, uint buffer) => pfn_glVertexArrayElementBuffer(vaobj, buffer);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, int, void> pfn_glVertexArrayVertexBuffer = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexBuffer">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, long offset, int stride) => pfn_glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, long*, int*, void> pfn_glVertexArrayVertexBuffers = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexBuffers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayVertexBuffers(uint vaobj, uint first, int count, uint* buffers, long* offsets, int* strides) => pfn_glVertexArrayVertexBuffers(vaobj, first, count, buffers, offsets, strides);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexArrayAttribBinding = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayAttribBinding">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) => pfn_glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, byte, uint, void> pfn_glVertexArrayAttribFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayAttribFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, byte normalized, uint relativeoffset) => pfn_glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void> pfn_glVertexArrayAttribIFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayAttribIFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void> pfn_glVertexArrayAttribLFormat = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayAttribLFormat">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexArrayBindingDivisor = null;
    /// <summary> <see href="docs.gl/gl4/glVertexArrayBindingDivisor">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) => pfn_glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);

    private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetVertexArrayiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexArrayiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexArrayiv(uint vaobj, uint pname, int* param) => pfn_glGetVertexArrayiv(vaobj, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetVertexArrayIndexediv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexArrayIndexediv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexArrayIndexediv(uint vaobj, uint index, uint pname, int* param) => pfn_glGetVertexArrayIndexediv(vaobj, index, pname, param);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long*, void> pfn_glGetVertexArrayIndexed64iv = null;
    /// <summary> <see href="docs.gl/gl4/glGetVertexArrayIndexed64iv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, long* param) => pfn_glGetVertexArrayIndexed64iv(vaobj, index, pname, param);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateSamplers = null;
    /// <summary> <see href="docs.gl/gl4/glCreateSamplers">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateSamplers(int n, uint* samplers) => pfn_glCreateSamplers(n, samplers);

    private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateProgramPipelines = null;
    /// <summary> <see href="docs.gl/gl4/glCreateProgramPipelines">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateProgramPipelines(int n, uint* pipelines) => pfn_glCreateProgramPipelines(n, pipelines);

    private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glCreateQueries = null;
    /// <summary> <see href="docs.gl/gl4/glCreateQueries">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glCreateQueries(uint target, int n, uint* ids) => pfn_glCreateQueries(target, n, ids);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, void> pfn_glGetQueryBufferObjecti64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryBufferObjecti64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryBufferObjecti64v(uint id, uint buffer, uint pname, long offset) => pfn_glGetQueryBufferObjecti64v(id, buffer, pname, offset);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, void> pfn_glGetQueryBufferObjectiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryBufferObjectiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryBufferObjectiv(uint id, uint buffer, uint pname, long offset) => pfn_glGetQueryBufferObjectiv(id, buffer, pname, offset);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, void> pfn_glGetQueryBufferObjectui64v = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryBufferObjectui64v">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryBufferObjectui64v(uint id, uint buffer, uint pname, long offset) => pfn_glGetQueryBufferObjectui64v(id, buffer, pname, offset);

    private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, void> pfn_glGetQueryBufferObjectuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetQueryBufferObjectuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetQueryBufferObjectuiv(uint id, uint buffer, uint pname, long offset) => pfn_glGetQueryBufferObjectuiv(id, buffer, pname, offset);

    private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMemoryBarrierByRegion = null;
    /// <summary> <see href="docs.gl/gl4/glMemoryBarrierByRegion">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMemoryBarrierByRegion(uint barriers) => pfn_glMemoryBarrierByRegion(barriers);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, uint, int, void*, void> pfn_glGetTextureSubImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetTextureSubImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, int bufSize, void* pixels) => pfn_glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, void*, void> pfn_glGetCompressedTextureSubImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetCompressedTextureSubImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels) => pfn_glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint> pfn_glGetGraphicsResetStatus = null;
    /// <summary> <see href="docs.gl/gl4/glGetGraphicsResetStatus">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static uint glGetGraphicsResetStatus() => pfn_glGetGraphicsResetStatus();

    private static delegate* unmanaged[Stdcall]<uint, int, int, void*, void> pfn_glGetnCompressedTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetnCompressedTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnCompressedTexImage(uint target, int lod, int bufSize, void* pixels) => pfn_glGetnCompressedTexImage(target, lod, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, int, void*, void> pfn_glGetnTexImage = null;
    /// <summary> <see href="docs.gl/gl4/glGetnTexImage">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnTexImage(uint target, int level, uint format, uint type, int bufSize, void* pixels) => pfn_glGetnTexImage(target, level, format, type, bufSize, pixels);

    private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glGetnUniformdv = null;
    /// <summary> <see href="docs.gl/gl4/glGetnUniformdv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnUniformdv(uint program, int location, int bufSize, double* @params) => pfn_glGetnUniformdv(program, location, bufSize, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glGetnUniformfv = null;
    /// <summary> <see href="docs.gl/gl4/glGetnUniformfv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnUniformfv(uint program, int location, int bufSize, float* @params) => pfn_glGetnUniformfv(program, location, bufSize, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glGetnUniformiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetnUniformiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnUniformiv(uint program, int location, int bufSize, int* @params) => pfn_glGetnUniformiv(program, location, bufSize, @params);

    private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glGetnUniformuiv = null;
    /// <summary> <see href="docs.gl/gl4/glGetnUniformuiv">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glGetnUniformuiv(uint program, int location, int bufSize, uint* @params) => pfn_glGetnUniformuiv(program, location, bufSize, @params);

    private static delegate* unmanaged[Stdcall]<int, int, int, int, uint, uint, int, void*, void> pfn_glReadnPixels = null;
    /// <summary> <see href="docs.gl/gl4/glReadnPixels">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, void* data) => pfn_glReadnPixels(x, y, width, height, format, type, bufSize, data);

    private static delegate* unmanaged[Stdcall]<void> pfn_glTextureBarrier = null;
    /// <summary> <see href="docs.gl/gl4/glTextureBarrier">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glTextureBarrier() => pfn_glTextureBarrier();
    /*  GL_VERSION_4_6 */
    public const uint GL_VERSION_4_6 = 1;
    public const uint GL_SHADER_BINARY_FORMAT_SPIR_V = 0x9551;
    public const uint GL_SPIR_V_BINARY = 0x9552;
    public const uint GL_PARAMETER_BUFFER = 0x80EE;
    public const uint GL_PARAMETER_BUFFER_BINDING = 0x80EF;
    public const uint GL_CONTEXT_FLAG_NO_ERROR_BIT = 0x00000008;
    public const uint GL_VERTICES_SUBMITTED = 0x82EE;
    public const uint GL_PRIMITIVES_SUBMITTED = 0x82EF;
    public const uint GL_VERTEX_SHADER_INVOCATIONS = 0x82F0;
    public const uint GL_TESS_CONTROL_SHADER_PATCHES = 0x82F1;
    public const uint GL_TESS_EVALUATION_SHADER_INVOCATIONS = 0x82F2;
    public const uint GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED = 0x82F3;
    public const uint GL_FRAGMENT_SHADER_INVOCATIONS = 0x82F4;
    public const uint GL_COMPUTE_SHADER_INVOCATIONS = 0x82F5;
    public const uint GL_CLIPPING_INPUT_PRIMITIVES = 0x82F6;
    public const uint GL_CLIPPING_OUTPUT_PRIMITIVES = 0x82F7;
    public const uint GL_POLYGON_OFFSET_CLAMP = 0x8E1B;
    public const uint GL_SPIR_V_EXTENSIONS = 0x9553;
    public const uint GL_NUM_SPIR_V_EXTENSIONS = 0x9554;
    public const uint GL_TEXTURE_MAX_ANISOTROPY = 0x84FE;
    public const uint GL_MAX_TEXTURE_MAX_ANISOTROPY = 0x84FF;
    public const uint GL_TRANSFORM_FEEDBACK_OVERFLOW = 0x82EC;
    public const uint GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW = 0x82ED;

    private static delegate* unmanaged[Stdcall]<uint, byte*, uint, uint*, uint*, void> pfn_glSpecializeShader = null;
    /// <summary> <see href="docs.gl/gl4/glSpecializeShader">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glSpecializeShader(uint shader, byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue) => pfn_glSpecializeShader(shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue);

    private static delegate* unmanaged[Stdcall]<uint, void*, long, int, int, void> pfn_glMultiDrawArraysIndirectCount = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawArraysIndirectCount">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawArraysIndirectCount(uint mode, void* indirect, long drawcount, int maxdrawcount, int stride) => pfn_glMultiDrawArraysIndirectCount(mode, indirect, drawcount, maxdrawcount, stride);

    private static delegate* unmanaged[Stdcall]<uint, uint, void*, long, int, int, void> pfn_glMultiDrawElementsIndirectCount = null;
    /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsIndirectCount">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glMultiDrawElementsIndirectCount(uint mode, uint type, void* indirect, long drawcount, int maxdrawcount, int stride) => pfn_glMultiDrawElementsIndirectCount(mode, type, indirect, drawcount, maxdrawcount, stride);

    private static delegate* unmanaged[Stdcall]<float, float, float, void> pfn_glPolygonOffsetClamp = null;
    /// <summary> <see href="docs.gl/gl4/glPolygonOffsetClamp">See on docs.gl</see> </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void glPolygonOffsetClamp(float factor, float units, float clamp) => pfn_glPolygonOffsetClamp(factor, units, clamp);
    public static unsafe class GLARBES2Compatibility
    {
        static GLARBES2Compatibility() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_ES2_compatibility") ?? false) _GL_ARB_ES2_compatibility = 1;
        }
        private static uint _GL_ARB_ES2_compatibility = 0;
        public static uint GL_ARB_ES2_compatibility => _GL_ARB_ES2_compatibility;
    }

    public static unsafe class GLARBES31Compatibility
    {
        static GLARBES31Compatibility() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_ES3_1_compatibility") ?? false) _GL_ARB_ES3_1_compatibility = 1;
        }
        private static uint _GL_ARB_ES3_1_compatibility = 0;
        public static uint GL_ARB_ES3_1_compatibility => _GL_ARB_ES3_1_compatibility;
    }

    public static unsafe class GLARBES32Compatibility
    {
        static GLARBES32Compatibility() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_ES3_2_compatibility") ?? false) _GL_ARB_ES3_2_compatibility = 1;
        }
        private static uint _GL_ARB_ES3_2_compatibility = 0;
        public static uint GL_ARB_ES3_2_compatibility => _GL_ARB_ES3_2_compatibility;
        public const uint GL_PRIMITIVE_BOUNDING_BOX_ARB = 0x92BE;
        public const uint GL_MULTISAMPLE_LINE_WIDTH_RANGE_ARB = 0x9381;
        public const uint GL_MULTISAMPLE_LINE_WIDTH_GRANULARITY_ARB = 0x9382;

        private static delegate* unmanaged[Stdcall]<float, float, float, float, float, float, float, float, void> pfn_glPrimitiveBoundingBoxARB = null;
        /// <summary> <see href="docs.gl/gl4/glPrimitiveBoundingBoxARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPrimitiveBoundingBoxARB(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW) => pfn_glPrimitiveBoundingBoxARB(minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
    }

    public static unsafe class GLARBES3Compatibility
    {
        static GLARBES3Compatibility() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_ES3_compatibility") ?? false) _GL_ARB_ES3_compatibility = 1;
        }
        private static uint _GL_ARB_ES3_compatibility = 0;
        public static uint GL_ARB_ES3_compatibility => _GL_ARB_ES3_compatibility;
    }

    public static unsafe class GLARBArraysOfArrays
    {
        static GLARBArraysOfArrays() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_arrays_of_arrays") ?? false) _GL_ARB_arrays_of_arrays = 1;
        }
        private static uint _GL_ARB_arrays_of_arrays = 0;
        public static uint GL_ARB_arrays_of_arrays => _GL_ARB_arrays_of_arrays;
    }

    public static unsafe class GLARBBaseInstance
    {
        static GLARBBaseInstance() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_base_instance") ?? false) _GL_ARB_base_instance = 1;
        }
        private static uint _GL_ARB_base_instance = 0;
        public static uint GL_ARB_base_instance => _GL_ARB_base_instance;
    }

    public static unsafe class GLARBBindlessTexture
    {
        static GLARBBindlessTexture() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_bindless_texture") ?? false) _GL_ARB_bindless_texture = 1;
        }
        private static uint _GL_ARB_bindless_texture = 0;
        public static uint GL_ARB_bindless_texture => _GL_ARB_bindless_texture;
        public const uint GL_UNSIGNED_INT64_ARB = 0x140F;

        private static delegate* unmanaged[Stdcall]<uint, ulong> pfn_glGetTextureHandleARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureHandleARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetTextureHandleARB(uint texture) => pfn_glGetTextureHandleARB(texture);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong> pfn_glGetTextureSamplerHandleARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureSamplerHandleARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetTextureSamplerHandleARB(uint texture, uint sampler) => pfn_glGetTextureSamplerHandleARB(texture, sampler);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeTextureHandleResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glMakeTextureHandleResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeTextureHandleResidentARB(ulong handle) => pfn_glMakeTextureHandleResidentARB(handle);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeTextureHandleNonResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glMakeTextureHandleNonResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeTextureHandleNonResidentARB(ulong handle) => pfn_glMakeTextureHandleNonResidentARB(handle);

        private static delegate* unmanaged[Stdcall]<uint, int, byte, int, uint, ulong> pfn_glGetImageHandleARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetImageHandleARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetImageHandleARB(uint texture, int level, byte layered, int layer, uint format) => pfn_glGetImageHandleARB(texture, level, layered, layer, format);

        private static delegate* unmanaged[Stdcall]<ulong, uint, void> pfn_glMakeImageHandleResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glMakeImageHandleResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeImageHandleResidentARB(ulong handle, uint access) => pfn_glMakeImageHandleResidentARB(handle, access);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeImageHandleNonResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glMakeImageHandleNonResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeImageHandleNonResidentARB(ulong handle) => pfn_glMakeImageHandleNonResidentARB(handle);

        private static delegate* unmanaged[Stdcall]<int, ulong, void> pfn_glUniformHandleui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniformHandleui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformHandleui64ARB(int location, ulong value) => pfn_glUniformHandleui64ARB(location, value);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniformHandleui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniformHandleui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformHandleui64vARB(int location, int count, ulong* value) => pfn_glUniformHandleui64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, void> pfn_glProgramUniformHandleui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformHandleui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformHandleui64ARB(uint program, int location, ulong value) => pfn_glProgramUniformHandleui64ARB(program, location, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniformHandleui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformHandleui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformHandleui64vARB(uint program, int location, int count, ulong* values) => pfn_glProgramUniformHandleui64vARB(program, location, count, values);

        private static delegate* unmanaged[Stdcall]<ulong, byte> pfn_glIsTextureHandleResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glIsTextureHandleResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsTextureHandleResidentARB(ulong handle) => pfn_glIsTextureHandleResidentARB(handle);

        private static delegate* unmanaged[Stdcall]<ulong, byte> pfn_glIsImageHandleResidentARB = null;
        /// <summary> <see href="docs.gl/gl4/glIsImageHandleResidentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsImageHandleResidentARB(ulong handle) => pfn_glIsImageHandleResidentARB(handle);

        private static delegate* unmanaged[Stdcall]<uint, void*, void> pfn_glVertexAttribL1ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1ui64ARB(uint index, void* x) => pfn_glVertexAttribL1ui64ARB(index, x);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glVertexAttribL1ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1ui64vARB(uint index, void** v) => pfn_glVertexAttribL1ui64vARB(index, v);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetVertexAttribLui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexAttribLui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexAttribLui64vARB(uint index, uint pname, void** @params) => pfn_glGetVertexAttribLui64vARB(index, pname, @params);
    }

    public static unsafe class GLARBBlendFuncExtended
    {
        static GLARBBlendFuncExtended() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_blend_func_extended") ?? false) _GL_ARB_blend_func_extended = 1;
        }
        private static uint _GL_ARB_blend_func_extended = 0;
        public static uint GL_ARB_blend_func_extended => _GL_ARB_blend_func_extended;
    }

    public static unsafe class GLARBBufferStorage
    {
        static GLARBBufferStorage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_buffer_storage") ?? false) _GL_ARB_buffer_storage = 1;
        }
        private static uint _GL_ARB_buffer_storage = 0;
        public static uint GL_ARB_buffer_storage => _GL_ARB_buffer_storage;
    }

    public static unsafe class GLARBClEvent
    {
        static GLARBClEvent() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_cl_event") ?? false) _GL_ARB_cl_event = 1;
        }
        private static uint _GL_ARB_cl_event = 0;
        public static uint GL_ARB_cl_event => _GL_ARB_cl_event;
        public const uint GL_SYNC_CL_EVENT_ARB = 0x8240;
        public const uint GL_SYNC_CL_EVENT_COMPLETE_ARB = 0x8241;

        private static delegate* unmanaged[Stdcall]<void**, void**, uint, void*> pfn_glCreateSyncFromCLeventARB = null;
        /// <summary> <see href="docs.gl/gl4/glCreateSyncFromCLeventARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void* glCreateSyncFromCLeventARB(void** context, void** @event, uint flags) => pfn_glCreateSyncFromCLeventARB(context, @event, flags);
    }

    public static unsafe class GLARBClearBufferObject
    {
        static GLARBClearBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_clear_buffer_object") ?? false) _GL_ARB_clear_buffer_object = 1;
        }
        private static uint _GL_ARB_clear_buffer_object = 0;
        public static uint GL_ARB_clear_buffer_object => _GL_ARB_clear_buffer_object;
    }

    public static unsafe class GLARBClearTexture
    {
        static GLARBClearTexture() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_clear_texture") ?? false) _GL_ARB_clear_texture = 1;
        }
        private static uint _GL_ARB_clear_texture = 0;
        public static uint GL_ARB_clear_texture => _GL_ARB_clear_texture;
    }

    public static unsafe class GLARBClipControl
    {
        static GLARBClipControl() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_clip_control") ?? false) _GL_ARB_clip_control = 1;
        }
        private static uint _GL_ARB_clip_control = 0;
        public static uint GL_ARB_clip_control => _GL_ARB_clip_control;
    }

    public static unsafe class GLARBCompressedTexturePixelStorage
    {
        static GLARBCompressedTexturePixelStorage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_compressed_texture_pixel_storage") ?? false) _GL_ARB_compressed_texture_pixel_storage = 1;
        }
        private static uint _GL_ARB_compressed_texture_pixel_storage = 0;
        public static uint GL_ARB_compressed_texture_pixel_storage => _GL_ARB_compressed_texture_pixel_storage;
    }

    public static unsafe class GLARBComputeShader
    {
        static GLARBComputeShader() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_compute_shader") ?? false) _GL_ARB_compute_shader = 1;
        }
        private static uint _GL_ARB_compute_shader = 0;
        public static uint GL_ARB_compute_shader => _GL_ARB_compute_shader;
    }

    public static unsafe class GLARBComputeVariableGroupSize
    {
        static GLARBComputeVariableGroupSize() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_compute_variable_group_size") ?? false) _GL_ARB_compute_variable_group_size = 1;
        }
        private static uint _GL_ARB_compute_variable_group_size = 0;
        public static uint GL_ARB_compute_variable_group_size => _GL_ARB_compute_variable_group_size;
        public const uint GL_MAX_COMPUTE_VARIABLE_GROUP_INVOCATIONS_ARB = 0x9344;
        public const uint GL_MAX_COMPUTE_FIXED_GROUP_INVOCATIONS_ARB = 0x90EB;
        public const uint GL_MAX_COMPUTE_VARIABLE_GROUP_SIZE_ARB = 0x9345;
        public const uint GL_MAX_COMPUTE_FIXED_GROUP_SIZE_ARB = 0x91BF;

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, uint, void> pfn_glDispatchComputeGroupSizeARB = null;
        /// <summary> <see href="docs.gl/gl4/glDispatchComputeGroupSizeARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDispatchComputeGroupSizeARB(uint num_groups_x, uint num_groups_y, uint num_groups_z, uint group_size_x, uint group_size_y, uint group_size_z) => pfn_glDispatchComputeGroupSizeARB(num_groups_x, num_groups_y, num_groups_z, group_size_x, group_size_y, group_size_z);
    }

    public static unsafe class GLARBConditionalRenderInverted
    {
        static GLARBConditionalRenderInverted() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_conditional_render_inverted") ?? false) _GL_ARB_conditional_render_inverted = 1;
        }
        private static uint _GL_ARB_conditional_render_inverted = 0;
        public static uint GL_ARB_conditional_render_inverted => _GL_ARB_conditional_render_inverted;
    }

    public static unsafe class GLARBConservativeDepth
    {
        static GLARBConservativeDepth() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_conservative_depth") ?? false) _GL_ARB_conservative_depth = 1;
        }
        private static uint _GL_ARB_conservative_depth = 0;
        public static uint GL_ARB_conservative_depth => _GL_ARB_conservative_depth;
    }

    public static unsafe class GLARBCopyBuffer
    {
        static GLARBCopyBuffer() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_copy_buffer") ?? false) _GL_ARB_copy_buffer = 1;
        }
        private static uint _GL_ARB_copy_buffer = 0;
        public static uint GL_ARB_copy_buffer => _GL_ARB_copy_buffer;
    }

    public static unsafe class GLARBCopyImage
    {
        static GLARBCopyImage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_copy_image") ?? false) _GL_ARB_copy_image = 1;
        }
        private static uint _GL_ARB_copy_image = 0;
        public static uint GL_ARB_copy_image => _GL_ARB_copy_image;
    }

    public static unsafe class GLARBCullDistance
    {
        static GLARBCullDistance() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_cull_distance") ?? false) _GL_ARB_cull_distance = 1;
        }
        private static uint _GL_ARB_cull_distance = 0;
        public static uint GL_ARB_cull_distance => _GL_ARB_cull_distance;
    }

    public static unsafe class GLARBDebugOutput
    {
        static GLARBDebugOutput() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_debug_output") ?? false) _GL_ARB_debug_output = 1;
        }
        private static uint _GL_ARB_debug_output = 0;
        public static uint GL_ARB_debug_output => _GL_ARB_debug_output;
        public const uint GL_DEBUG_OUTPUT_SYNCHRONOUS_ARB = 0x8242;
        public const uint GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH_ARB = 0x8243;
        public const uint GL_DEBUG_CALLBACK_FUNCTION_ARB = 0x8244;
        public const uint GL_DEBUG_CALLBACK_USER_PARAM_ARB = 0x8245;
        public const uint GL_DEBUG_SOURCE_API_ARB = 0x8246;
        public const uint GL_DEBUG_SOURCE_WINDOW_SYSTEM_ARB = 0x8247;
        public const uint GL_DEBUG_SOURCE_SHADER_COMPILER_ARB = 0x8248;
        public const uint GL_DEBUG_SOURCE_THIRD_PARTY_ARB = 0x8249;
        public const uint GL_DEBUG_SOURCE_APPLICATION_ARB = 0x824A;
        public const uint GL_DEBUG_SOURCE_OTHER_ARB = 0x824B;
        public const uint GL_DEBUG_TYPE_ERROR_ARB = 0x824C;
        public const uint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_ARB = 0x824D;
        public const uint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR_ARB = 0x824E;
        public const uint GL_DEBUG_TYPE_PORTABILITY_ARB = 0x824F;
        public const uint GL_DEBUG_TYPE_PERFORMANCE_ARB = 0x8250;
        public const uint GL_DEBUG_TYPE_OTHER_ARB = 0x8251;
        public const uint GL_MAX_DEBUG_MESSAGE_LENGTH_ARB = 0x9143;
        public const uint GL_MAX_DEBUG_LOGGED_MESSAGES_ARB = 0x9144;
        public const uint GL_DEBUG_LOGGED_MESSAGES_ARB = 0x9145;
        public const uint GL_DEBUG_SEVERITY_HIGH_ARB = 0x9146;
        public const uint GL_DEBUG_SEVERITY_MEDIUM_ARB = 0x9147;
        public const uint GL_DEBUG_SEVERITY_LOW_ARB = 0x9148;

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, byte, void> pfn_glDebugMessageControlARB = null;
        /// <summary> <see href="docs.gl/gl4/glDebugMessageControlARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDebugMessageControlARB(uint source, uint type, uint severity, int count, uint* ids, byte enabled) => pfn_glDebugMessageControlARB(source, type, severity, count, ids, enabled);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, byte*, void> pfn_glDebugMessageInsertARB = null;
        /// <summary> <see href="docs.gl/gl4/glDebugMessageInsertARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDebugMessageInsertARB(uint source, uint type, uint id, uint severity, int length, byte* buf) => pfn_glDebugMessageInsertARB(source, type, id, severity, length, buf);

        private static delegate* unmanaged[Stdcall]<void*, void*, void> pfn_glDebugMessageCallbackARB = null;
        /// <summary> <see href="docs.gl/gl4/glDebugMessageCallbackARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDebugMessageCallbackARB(void* callback, void* userParam) => pfn_glDebugMessageCallbackARB(callback, userParam);

        private static delegate* unmanaged[Stdcall]<uint, int, uint*, uint*, uint*, uint*, int*, byte*, uint> pfn_glGetDebugMessageLogARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetDebugMessageLogARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glGetDebugMessageLogARB(uint count, int bufSize, uint* sources, uint* types, uint* ids, uint* severities, int* lengths, byte* messageLog) => pfn_glGetDebugMessageLogARB(count, bufSize, sources, types, ids, severities, lengths, messageLog);
    }

    public static unsafe class GLARBDepthBufferFloat
    {
        static GLARBDepthBufferFloat() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_depth_buffer_float") ?? false) _GL_ARB_depth_buffer_float = 1;
        }
        private static uint _GL_ARB_depth_buffer_float = 0;
        public static uint GL_ARB_depth_buffer_float => _GL_ARB_depth_buffer_float;
    }

    public static unsafe class GLARBDepthClamp
    {
        static GLARBDepthClamp() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_depth_clamp") ?? false) _GL_ARB_depth_clamp = 1;
        }
        private static uint _GL_ARB_depth_clamp = 0;
        public static uint GL_ARB_depth_clamp => _GL_ARB_depth_clamp;
    }

    public static unsafe class GLARBDerivativeControl
    {
        static GLARBDerivativeControl() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_derivative_control") ?? false) _GL_ARB_derivative_control = 1;
        }
        private static uint _GL_ARB_derivative_control = 0;
        public static uint GL_ARB_derivative_control => _GL_ARB_derivative_control;
    }

    public static unsafe class GLARBDirectStateAccess
    {
        static GLARBDirectStateAccess() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_direct_state_access") ?? false) _GL_ARB_direct_state_access = 1;
        }
        private static uint _GL_ARB_direct_state_access = 0;
        public static uint GL_ARB_direct_state_access => _GL_ARB_direct_state_access;
    }

    public static unsafe class GLARBDrawBuffersBlend
    {
        static GLARBDrawBuffersBlend() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_draw_buffers_blend") ?? false) _GL_ARB_draw_buffers_blend = 1;
        }
        private static uint _GL_ARB_draw_buffers_blend = 0;
        public static uint GL_ARB_draw_buffers_blend => _GL_ARB_draw_buffers_blend;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBlendEquationiARB = null;
        /// <summary> <see href="docs.gl/gl4/glBlendEquationiARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendEquationiARB(uint buf, uint mode) => pfn_glBlendEquationiARB(buf, mode);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBlendEquationSeparateiARB = null;
        /// <summary> <see href="docs.gl/gl4/glBlendEquationSeparateiARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendEquationSeparateiARB(uint buf, uint modeRGB, uint modeAlpha) => pfn_glBlendEquationSeparateiARB(buf, modeRGB, modeAlpha);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBlendFunciARB = null;
        /// <summary> <see href="docs.gl/gl4/glBlendFunciARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendFunciARB(uint buf, uint src, uint dst) => pfn_glBlendFunciARB(buf, src, dst);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void> pfn_glBlendFuncSeparateiARB = null;
        /// <summary> <see href="docs.gl/gl4/glBlendFuncSeparateiARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendFuncSeparateiARB(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) => pfn_glBlendFuncSeparateiARB(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
    }

    public static unsafe class GLARBDrawElementsBaseVertex
    {
        static GLARBDrawElementsBaseVertex() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_draw_elements_base_vertex") ?? false) _GL_ARB_draw_elements_base_vertex = 1;
        }
        private static uint _GL_ARB_draw_elements_base_vertex = 0;
        public static uint GL_ARB_draw_elements_base_vertex => _GL_ARB_draw_elements_base_vertex;
    }

    public static unsafe class GLARBDrawIndirect
    {
        static GLARBDrawIndirect() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_draw_indirect") ?? false) _GL_ARB_draw_indirect = 1;
        }
        private static uint _GL_ARB_draw_indirect = 0;
        public static uint GL_ARB_draw_indirect => _GL_ARB_draw_indirect;
    }

    public static unsafe class GLARBDrawInstanced
    {
        static GLARBDrawInstanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_draw_instanced") ?? false) _GL_ARB_draw_instanced = 1;
        }
        private static uint _GL_ARB_draw_instanced = 0;
        public static uint GL_ARB_draw_instanced => _GL_ARB_draw_instanced;

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glDrawArraysInstancedARB = null;
        /// <summary> <see href="docs.gl/gl4/glDrawArraysInstancedARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawArraysInstancedARB(uint mode, int first, int count, int primcount) => pfn_glDrawArraysInstancedARB(mode, first, count, primcount);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void> pfn_glDrawElementsInstancedARB = null;
        /// <summary> <see href="docs.gl/gl4/glDrawElementsInstancedARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawElementsInstancedARB(uint mode, int count, uint type, void* indices, int primcount) => pfn_glDrawElementsInstancedARB(mode, count, type, indices, primcount);
    }

    public static unsafe class GLARBEnhancedLayouts
    {
        static GLARBEnhancedLayouts() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_enhanced_layouts") ?? false) _GL_ARB_enhanced_layouts = 1;
        }
        private static uint _GL_ARB_enhanced_layouts = 0;
        public static uint GL_ARB_enhanced_layouts => _GL_ARB_enhanced_layouts;
    }

    public static unsafe class GLARBExplicitAttribLocation
    {
        static GLARBExplicitAttribLocation() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_explicit_attrib_location") ?? false) _GL_ARB_explicit_attrib_location = 1;
        }
        private static uint _GL_ARB_explicit_attrib_location = 0;
        public static uint GL_ARB_explicit_attrib_location => _GL_ARB_explicit_attrib_location;
    }

    public static unsafe class GLARBExplicitUniformLocation
    {
        static GLARBExplicitUniformLocation() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_explicit_uniform_location") ?? false) _GL_ARB_explicit_uniform_location = 1;
        }
        private static uint _GL_ARB_explicit_uniform_location = 0;
        public static uint GL_ARB_explicit_uniform_location => _GL_ARB_explicit_uniform_location;
    }

    public static unsafe class GLARBFragmentCoordConventions
    {
        static GLARBFragmentCoordConventions() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_fragment_coord_conventions") ?? false) _GL_ARB_fragment_coord_conventions = 1;
        }
        private static uint _GL_ARB_fragment_coord_conventions = 0;
        public static uint GL_ARB_fragment_coord_conventions => _GL_ARB_fragment_coord_conventions;
    }

    public static unsafe class GLARBFragmentLayerViewport
    {
        static GLARBFragmentLayerViewport() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_fragment_layer_viewport") ?? false) _GL_ARB_fragment_layer_viewport = 1;
        }
        private static uint _GL_ARB_fragment_layer_viewport = 0;
        public static uint GL_ARB_fragment_layer_viewport => _GL_ARB_fragment_layer_viewport;
    }

    public static unsafe class GLARBFragmentShaderInterlock
    {
        static GLARBFragmentShaderInterlock() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_fragment_shader_interlock") ?? false) _GL_ARB_fragment_shader_interlock = 1;
        }
        private static uint _GL_ARB_fragment_shader_interlock = 0;
        public static uint GL_ARB_fragment_shader_interlock => _GL_ARB_fragment_shader_interlock;
    }

    public static unsafe class GLARBFramebufferNoAttachments
    {
        static GLARBFramebufferNoAttachments() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_framebuffer_no_attachments") ?? false) _GL_ARB_framebuffer_no_attachments = 1;
        }
        private static uint _GL_ARB_framebuffer_no_attachments = 0;
        public static uint GL_ARB_framebuffer_no_attachments => _GL_ARB_framebuffer_no_attachments;
    }

    public static unsafe class GLARBFramebufferObject
    {
        static GLARBFramebufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_framebuffer_object") ?? false) _GL_ARB_framebuffer_object = 1;
        }
        private static uint _GL_ARB_framebuffer_object = 0;
        public static uint GL_ARB_framebuffer_object => _GL_ARB_framebuffer_object;
    }

    public static unsafe class GLARBFramebufferSRGB
    {
        static GLARBFramebufferSRGB() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_framebuffer_sRGB") ?? false) _GL_ARB_framebuffer_sRGB = 1;
        }
        private static uint _GL_ARB_framebuffer_sRGB = 0;
        public static uint GL_ARB_framebuffer_sRGB => _GL_ARB_framebuffer_sRGB;
    }

    public static unsafe class GLARBGeometryShader4
    {
        static GLARBGeometryShader4() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_geometry_shader4") ?? false) _GL_ARB_geometry_shader4 = 1;
        }
        private static uint _GL_ARB_geometry_shader4 = 0;
        public static uint GL_ARB_geometry_shader4 => _GL_ARB_geometry_shader4;
        public const uint GL_LINES_ADJACENCY_ARB = 0x000A;
        public const uint GL_LINE_STRIP_ADJACENCY_ARB = 0x000B;
        public const uint GL_TRIANGLES_ADJACENCY_ARB = 0x000C;
        public const uint GL_TRIANGLE_STRIP_ADJACENCY_ARB = 0x000D;
        public const uint GL_PROGRAM_POINT_SIZE_ARB = 0x8642;
        public const uint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS_ARB = 0x8C29;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_LAYERED_ARB = 0x8DA7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS_ARB = 0x8DA8;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_COUNT_ARB = 0x8DA9;
        public const uint GL_GEOMETRY_SHADER_ARB = 0x8DD9;
        public const uint GL_GEOMETRY_VERTICES_OUT_ARB = 0x8DDA;
        public const uint GL_GEOMETRY_INPUT_TYPE_ARB = 0x8DDB;
        public const uint GL_GEOMETRY_OUTPUT_TYPE_ARB = 0x8DDC;
        public const uint GL_MAX_GEOMETRY_VARYING_COMPONENTS_ARB = 0x8DDD;
        public const uint GL_MAX_VERTEX_VARYING_COMPONENTS_ARB = 0x8DDE;
        public const uint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS_ARB = 0x8DDF;
        public const uint GL_MAX_GEOMETRY_OUTPUT_VERTICES_ARB = 0x8DE0;
        public const uint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS_ARB = 0x8DE1;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glProgramParameteriARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramParameteriARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramParameteriARB(uint program, uint pname, int value) => pfn_glProgramParameteriARB(program, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glFramebufferTextureARB = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferTextureARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferTextureARB(uint target, uint attachment, uint texture, int level) => pfn_glFramebufferTextureARB(target, attachment, texture, level);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void> pfn_glFramebufferTextureLayerARB = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferTextureLayerARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferTextureLayerARB(uint target, uint attachment, uint texture, int level, int layer) => pfn_glFramebufferTextureLayerARB(target, attachment, texture, level, layer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void> pfn_glFramebufferTextureFaceARB = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferTextureFaceARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferTextureFaceARB(uint target, uint attachment, uint texture, int level, uint face) => pfn_glFramebufferTextureFaceARB(target, attachment, texture, level, face);
    }

    public static unsafe class GLARBGetProgramBinary
    {
        static GLARBGetProgramBinary() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_get_program_binary") ?? false) _GL_ARB_get_program_binary = 1;
        }
        private static uint _GL_ARB_get_program_binary = 0;
        public static uint GL_ARB_get_program_binary => _GL_ARB_get_program_binary;
    }

    public static unsafe class GLARBGetTextureSubImage
    {
        static GLARBGetTextureSubImage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_get_texture_sub_image") ?? false) _GL_ARB_get_texture_sub_image = 1;
        }
        private static uint _GL_ARB_get_texture_sub_image = 0;
        public static uint GL_ARB_get_texture_sub_image => _GL_ARB_get_texture_sub_image;
    }

    public static unsafe class GLARBGlSpirv
    {
        static GLARBGlSpirv() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_gl_spirv") ?? false) _GL_ARB_gl_spirv = 1;
        }
        private static uint _GL_ARB_gl_spirv = 0;
        public static uint GL_ARB_gl_spirv => _GL_ARB_gl_spirv;
        public const uint GL_SHADER_BINARY_FORMAT_SPIR_V_ARB = 0x9551;
        public const uint GL_SPIR_V_BINARY_ARB = 0x9552;

        private static delegate* unmanaged[Stdcall]<uint, byte*, uint, uint*, uint*, void> pfn_glSpecializeShaderARB = null;
        /// <summary> <see href="docs.gl/gl4/glSpecializeShaderARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSpecializeShaderARB(uint shader, byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue) => pfn_glSpecializeShaderARB(shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue);
    }

    public static unsafe class GLARBGpuShader5
    {
        static GLARBGpuShader5() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_gpu_shader5") ?? false) _GL_ARB_gpu_shader5 = 1;
        }
        private static uint _GL_ARB_gpu_shader5 = 0;
        public static uint GL_ARB_gpu_shader5 => _GL_ARB_gpu_shader5;
    }

    public static unsafe class GLARBGpuShaderFp64
    {
        static GLARBGpuShaderFp64() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_gpu_shader_fp64") ?? false) _GL_ARB_gpu_shader_fp64 = 1;
        }
        private static uint _GL_ARB_gpu_shader_fp64 = 0;
        public static uint GL_ARB_gpu_shader_fp64 => _GL_ARB_gpu_shader_fp64;
    }

    public static unsafe class GLARBGpuShaderInt64
    {
        static GLARBGpuShaderInt64() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_gpu_shader_int64") ?? false) _GL_ARB_gpu_shader_int64 = 1;
        }
        private static uint _GL_ARB_gpu_shader_int64 = 0;
        public static uint GL_ARB_gpu_shader_int64 => _GL_ARB_gpu_shader_int64;
        public const uint GL_INT64_ARB = 0x140E;
        public const uint GL_INT64_VEC2_ARB = 0x8FE9;
        public const uint GL_INT64_VEC3_ARB = 0x8FEA;
        public const uint GL_INT64_VEC4_ARB = 0x8FEB;
        public const uint GL_UNSIGNED_INT64_VEC2_ARB = 0x8FF5;
        public const uint GL_UNSIGNED_INT64_VEC3_ARB = 0x8FF6;
        public const uint GL_UNSIGNED_INT64_VEC4_ARB = 0x8FF7;

        private static delegate* unmanaged[Stdcall]<int, long, void> pfn_glUniform1i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1i64ARB(int location, long x) => pfn_glUniform1i64ARB(location, x);

        private static delegate* unmanaged[Stdcall]<int, long, long, void> pfn_glUniform2i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2i64ARB(int location, long x, long y) => pfn_glUniform2i64ARB(location, x, y);

        private static delegate* unmanaged[Stdcall]<int, long, long, long, void> pfn_glUniform3i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3i64ARB(int location, long x, long y, long z) => pfn_glUniform3i64ARB(location, x, y, z);

        private static delegate* unmanaged[Stdcall]<int, long, long, long, long, void> pfn_glUniform4i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4i64ARB(int location, long x, long y, long z, long w) => pfn_glUniform4i64ARB(location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform1i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1i64vARB(int location, int count, long* value) => pfn_glUniform1i64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform2i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2i64vARB(int location, int count, long* value) => pfn_glUniform2i64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform3i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3i64vARB(int location, int count, long* value) => pfn_glUniform3i64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform4i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4i64vARB(int location, int count, long* value) => pfn_glUniform4i64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, ulong, void> pfn_glUniform1ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1ui64ARB(int location, ulong x) => pfn_glUniform1ui64ARB(location, x);

        private static delegate* unmanaged[Stdcall]<int, ulong, ulong, void> pfn_glUniform2ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2ui64ARB(int location, ulong x, ulong y) => pfn_glUniform2ui64ARB(location, x, y);

        private static delegate* unmanaged[Stdcall]<int, ulong, ulong, ulong, void> pfn_glUniform3ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3ui64ARB(int location, ulong x, ulong y, ulong z) => pfn_glUniform3ui64ARB(location, x, y, z);

        private static delegate* unmanaged[Stdcall]<int, ulong, ulong, ulong, ulong, void> pfn_glUniform4ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4ui64ARB(int location, ulong x, ulong y, ulong z, ulong w) => pfn_glUniform4ui64ARB(location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniform1ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1ui64vARB(int location, int count, ulong* value) => pfn_glUniform1ui64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniform2ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2ui64vARB(int location, int count, ulong* value) => pfn_glUniform2ui64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniform3ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3ui64vARB(int location, int count, ulong* value) => pfn_glUniform3ui64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniform4ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4ui64vARB(int location, int count, ulong* value) => pfn_glUniform4ui64vARB(location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, long*, void> pfn_glGetUniformi64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetUniformi64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetUniformi64vARB(uint program, int location, long* @params) => pfn_glGetUniformi64vARB(program, location, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong*, void> pfn_glGetUniformui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetUniformui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetUniformui64vARB(uint program, int location, ulong* @params) => pfn_glGetUniformui64vARB(program, location, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glGetnUniformi64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformi64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformi64vARB(uint program, int location, int bufSize, long* @params) => pfn_glGetnUniformi64vARB(program, location, bufSize, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glGetnUniformui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformui64vARB(uint program, int location, int bufSize, ulong* @params) => pfn_glGetnUniformui64vARB(program, location, bufSize, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, long, void> pfn_glProgramUniform1i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1i64ARB(uint program, int location, long x) => pfn_glProgramUniform1i64ARB(program, location, x);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, void> pfn_glProgramUniform2i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2i64ARB(uint program, int location, long x, long y) => pfn_glProgramUniform2i64ARB(program, location, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, long, void> pfn_glProgramUniform3i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3i64ARB(uint program, int location, long x, long y, long z) => pfn_glProgramUniform3i64ARB(program, location, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, long, long, void> pfn_glProgramUniform4i64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4i64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4i64ARB(uint program, int location, long x, long y, long z, long w) => pfn_glProgramUniform4i64ARB(program, location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform1i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1i64vARB(uint program, int location, int count, long* value) => pfn_glProgramUniform1i64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform2i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2i64vARB(uint program, int location, int count, long* value) => pfn_glProgramUniform2i64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform3i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3i64vARB(uint program, int location, int count, long* value) => pfn_glProgramUniform3i64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform4i64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4i64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4i64vARB(uint program, int location, int count, long* value) => pfn_glProgramUniform4i64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, void> pfn_glProgramUniform1ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1ui64ARB(uint program, int location, ulong x) => pfn_glProgramUniform1ui64ARB(program, location, x);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, ulong, void> pfn_glProgramUniform2ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2ui64ARB(uint program, int location, ulong x, ulong y) => pfn_glProgramUniform2ui64ARB(program, location, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, ulong, ulong, void> pfn_glProgramUniform3ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3ui64ARB(uint program, int location, ulong x, ulong y, ulong z) => pfn_glProgramUniform3ui64ARB(program, location, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, ulong, ulong, ulong, void> pfn_glProgramUniform4ui64ARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4ui64ARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4ui64ARB(uint program, int location, ulong x, ulong y, ulong z, ulong w) => pfn_glProgramUniform4ui64ARB(program, location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniform1ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1ui64vARB(uint program, int location, int count, ulong* value) => pfn_glProgramUniform1ui64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniform2ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2ui64vARB(uint program, int location, int count, ulong* value) => pfn_glProgramUniform2ui64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniform3ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3ui64vARB(uint program, int location, int count, ulong* value) => pfn_glProgramUniform3ui64vARB(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniform4ui64vARB = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4ui64vARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4ui64vARB(uint program, int location, int count, ulong* value) => pfn_glProgramUniform4ui64vARB(program, location, count, value);
    }

    public static unsafe class GLARBHalfFloatVertex
    {
        static GLARBHalfFloatVertex() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_half_float_vertex") ?? false) _GL_ARB_half_float_vertex = 1;
        }
        private static uint _GL_ARB_half_float_vertex = 0;
        public static uint GL_ARB_half_float_vertex => _GL_ARB_half_float_vertex;
    }

    public static unsafe class GLARBImaging
    {
        static GLARBImaging() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_imaging") ?? false) _GL_ARB_imaging = 1;
        }
        private static uint _GL_ARB_imaging = 0;
        public static uint GL_ARB_imaging => _GL_ARB_imaging;
    }

    public static unsafe class GLARBIndirectParameters
    {
        static GLARBIndirectParameters() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_indirect_parameters") ?? false) _GL_ARB_indirect_parameters = 1;
        }
        private static uint _GL_ARB_indirect_parameters = 0;
        public static uint GL_ARB_indirect_parameters => _GL_ARB_indirect_parameters;
        public const uint GL_PARAMETER_BUFFER_ARB = 0x80EE;
        public const uint GL_PARAMETER_BUFFER_BINDING_ARB = 0x80EF;

        private static delegate* unmanaged[Stdcall]<uint, void*, long, int, int, void> pfn_glMultiDrawArraysIndirectCountARB = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawArraysIndirectCountARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawArraysIndirectCountARB(uint mode, void* indirect, long drawcount, int maxdrawcount, int stride) => pfn_glMultiDrawArraysIndirectCountARB(mode, indirect, drawcount, maxdrawcount, stride);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, long, int, int, void> pfn_glMultiDrawElementsIndirectCountARB = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsIndirectCountARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawElementsIndirectCountARB(uint mode, uint type, void* indirect, long drawcount, int maxdrawcount, int stride) => pfn_glMultiDrawElementsIndirectCountARB(mode, type, indirect, drawcount, maxdrawcount, stride);
    }

    public static unsafe class GLARBInstancedArrays
    {
        static GLARBInstancedArrays() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_instanced_arrays") ?? false) _GL_ARB_instanced_arrays = 1;
        }
        private static uint _GL_ARB_instanced_arrays = 0;
        public static uint GL_ARB_instanced_arrays => _GL_ARB_instanced_arrays;
        public const uint GL_VERTEX_ATTRIB_ARRAY_DIVISOR_ARB = 0x88FE;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glVertexAttribDivisorARB = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribDivisorARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribDivisorARB(uint index, uint divisor) => pfn_glVertexAttribDivisorARB(index, divisor);
    }

    public static unsafe class GLARBInternalformatQuery
    {
        static GLARBInternalformatQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_internalformat_query") ?? false) _GL_ARB_internalformat_query = 1;
        }
        private static uint _GL_ARB_internalformat_query = 0;
        public static uint GL_ARB_internalformat_query => _GL_ARB_internalformat_query;
    }

    public static unsafe class GLARBInternalformatQuery2
    {
        static GLARBInternalformatQuery2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_internalformat_query2") ?? false) _GL_ARB_internalformat_query2 = 1;
        }
        private static uint _GL_ARB_internalformat_query2 = 0;
        public static uint GL_ARB_internalformat_query2 => _GL_ARB_internalformat_query2;
        public const uint GL_SRGB_DECODE_ARB = 0x8299;
        public const uint GL_VIEW_CLASS_EAC_R11 = 0x9383;
        public const uint GL_VIEW_CLASS_EAC_RG11 = 0x9384;
        public const uint GL_VIEW_CLASS_ETC2_RGB = 0x9385;
        public const uint GL_VIEW_CLASS_ETC2_RGBA = 0x9386;
        public const uint GL_VIEW_CLASS_ETC2_EAC_RGBA = 0x9387;
        public const uint GL_VIEW_CLASS_ASTC_4x4_RGBA = 0x9388;
        public const uint GL_VIEW_CLASS_ASTC_5x4_RGBA = 0x9389;
        public const uint GL_VIEW_CLASS_ASTC_5x5_RGBA = 0x938A;
        public const uint GL_VIEW_CLASS_ASTC_6x5_RGBA = 0x938B;
        public const uint GL_VIEW_CLASS_ASTC_6x6_RGBA = 0x938C;
        public const uint GL_VIEW_CLASS_ASTC_8x5_RGBA = 0x938D;
        public const uint GL_VIEW_CLASS_ASTC_8x6_RGBA = 0x938E;
        public const uint GL_VIEW_CLASS_ASTC_8x8_RGBA = 0x938F;
        public const uint GL_VIEW_CLASS_ASTC_10x5_RGBA = 0x9390;
        public const uint GL_VIEW_CLASS_ASTC_10x6_RGBA = 0x9391;
        public const uint GL_VIEW_CLASS_ASTC_10x8_RGBA = 0x9392;
        public const uint GL_VIEW_CLASS_ASTC_10x10_RGBA = 0x9393;
        public const uint GL_VIEW_CLASS_ASTC_12x10_RGBA = 0x9394;
        public const uint GL_VIEW_CLASS_ASTC_12x12_RGBA = 0x9395;
    }

    public static unsafe class GLARBInvalidateSubdata
    {
        static GLARBInvalidateSubdata() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_invalidate_subdata") ?? false) _GL_ARB_invalidate_subdata = 1;
        }
        private static uint _GL_ARB_invalidate_subdata = 0;
        public static uint GL_ARB_invalidate_subdata => _GL_ARB_invalidate_subdata;
    }

    public static unsafe class GLARBMapBufferAlignment
    {
        static GLARBMapBufferAlignment() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_map_buffer_alignment") ?? false) _GL_ARB_map_buffer_alignment = 1;
        }
        private static uint _GL_ARB_map_buffer_alignment = 0;
        public static uint GL_ARB_map_buffer_alignment => _GL_ARB_map_buffer_alignment;
    }

    public static unsafe class GLARBMapBufferRange
    {
        static GLARBMapBufferRange() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_map_buffer_range") ?? false) _GL_ARB_map_buffer_range = 1;
        }
        private static uint _GL_ARB_map_buffer_range = 0;
        public static uint GL_ARB_map_buffer_range => _GL_ARB_map_buffer_range;
    }

    public static unsafe class GLARBMultiBind
    {
        static GLARBMultiBind() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_multi_bind") ?? false) _GL_ARB_multi_bind = 1;
        }
        private static uint _GL_ARB_multi_bind = 0;
        public static uint GL_ARB_multi_bind => _GL_ARB_multi_bind;
    }

    public static unsafe class GLARBMultiDrawIndirect
    {
        static GLARBMultiDrawIndirect() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_multi_draw_indirect") ?? false) _GL_ARB_multi_draw_indirect = 1;
        }
        private static uint _GL_ARB_multi_draw_indirect = 0;
        public static uint GL_ARB_multi_draw_indirect => _GL_ARB_multi_draw_indirect;
    }

    public static unsafe class GLARBOcclusionQuery2
    {
        static GLARBOcclusionQuery2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_occlusion_query2") ?? false) _GL_ARB_occlusion_query2 = 1;
        }
        private static uint _GL_ARB_occlusion_query2 = 0;
        public static uint GL_ARB_occlusion_query2 => _GL_ARB_occlusion_query2;
    }

    public static unsafe class GLARBParallelShaderCompile
    {
        static GLARBParallelShaderCompile() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_parallel_shader_compile") ?? false) _GL_ARB_parallel_shader_compile = 1;
        }
        private static uint _GL_ARB_parallel_shader_compile = 0;
        public static uint GL_ARB_parallel_shader_compile => _GL_ARB_parallel_shader_compile;
        public const uint GL_MAX_SHADER_COMPILER_THREADS_ARB = 0x91B0;
        public const uint GL_COMPLETION_STATUS_ARB = 0x91B1;

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMaxShaderCompilerThreadsARB = null;
        /// <summary> <see href="docs.gl/gl4/glMaxShaderCompilerThreadsARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMaxShaderCompilerThreadsARB(uint count) => pfn_glMaxShaderCompilerThreadsARB(count);
    }

    public static unsafe class GLARBPipelineStatisticsQuery
    {
        static GLARBPipelineStatisticsQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_pipeline_statistics_query") ?? false) _GL_ARB_pipeline_statistics_query = 1;
        }
        private static uint _GL_ARB_pipeline_statistics_query = 0;
        public static uint GL_ARB_pipeline_statistics_query => _GL_ARB_pipeline_statistics_query;
        public const uint GL_VERTICES_SUBMITTED_ARB = 0x82EE;
        public const uint GL_PRIMITIVES_SUBMITTED_ARB = 0x82EF;
        public const uint GL_VERTEX_SHADER_INVOCATIONS_ARB = 0x82F0;
        public const uint GL_TESS_CONTROL_SHADER_PATCHES_ARB = 0x82F1;
        public const uint GL_TESS_EVALUATION_SHADER_INVOCATIONS_ARB = 0x82F2;
        public const uint GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED_ARB = 0x82F3;
        public const uint GL_FRAGMENT_SHADER_INVOCATIONS_ARB = 0x82F4;
        public const uint GL_COMPUTE_SHADER_INVOCATIONS_ARB = 0x82F5;
        public const uint GL_CLIPPING_INPUT_PRIMITIVES_ARB = 0x82F6;
        public const uint GL_CLIPPING_OUTPUT_PRIMITIVES_ARB = 0x82F7;
    }

    public static unsafe class GLARBPixelBufferObject
    {
        static GLARBPixelBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_pixel_buffer_object") ?? false) _GL_ARB_pixel_buffer_object = 1;
        }
        private static uint _GL_ARB_pixel_buffer_object = 0;
        public static uint GL_ARB_pixel_buffer_object => _GL_ARB_pixel_buffer_object;
        public const uint GL_PIXEL_PACK_BUFFER_ARB = 0x88EB;
        public const uint GL_PIXEL_UNPACK_BUFFER_ARB = 0x88EC;
        public const uint GL_PIXEL_PACK_BUFFER_BINDING_ARB = 0x88ED;
        public const uint GL_PIXEL_UNPACK_BUFFER_BINDING_ARB = 0x88EF;
    }

    public static unsafe class GLARBPolygonOffsetClamp
    {
        static GLARBPolygonOffsetClamp() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_polygon_offset_clamp") ?? false) _GL_ARB_polygon_offset_clamp = 1;
        }
        private static uint _GL_ARB_polygon_offset_clamp = 0;
        public static uint GL_ARB_polygon_offset_clamp => _GL_ARB_polygon_offset_clamp;
    }

    public static unsafe class GLARBPostDepthCoverage
    {
        static GLARBPostDepthCoverage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_post_depth_coverage") ?? false) _GL_ARB_post_depth_coverage = 1;
        }
        private static uint _GL_ARB_post_depth_coverage = 0;
        public static uint GL_ARB_post_depth_coverage => _GL_ARB_post_depth_coverage;
    }

    public static unsafe class GLARBProgramInterfaceQuery
    {
        static GLARBProgramInterfaceQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_program_interface_query") ?? false) _GL_ARB_program_interface_query = 1;
        }
        private static uint _GL_ARB_program_interface_query = 0;
        public static uint GL_ARB_program_interface_query => _GL_ARB_program_interface_query;
    }

    public static unsafe class GLARBProvokingVertex
    {
        static GLARBProvokingVertex() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_provoking_vertex") ?? false) _GL_ARB_provoking_vertex = 1;
        }
        private static uint _GL_ARB_provoking_vertex = 0;
        public static uint GL_ARB_provoking_vertex => _GL_ARB_provoking_vertex;
    }

    public static unsafe class GLARBQueryBufferObject
    {
        static GLARBQueryBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_query_buffer_object") ?? false) _GL_ARB_query_buffer_object = 1;
        }
        private static uint _GL_ARB_query_buffer_object = 0;
        public static uint GL_ARB_query_buffer_object => _GL_ARB_query_buffer_object;
    }

    public static unsafe class GLARBRobustBufferAccessBehavior
    {
        static GLARBRobustBufferAccessBehavior() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_robust_buffer_access_behavior") ?? false) _GL_ARB_robust_buffer_access_behavior = 1;
        }
        private static uint _GL_ARB_robust_buffer_access_behavior = 0;
        public static uint GL_ARB_robust_buffer_access_behavior => _GL_ARB_robust_buffer_access_behavior;
    }

    public static unsafe class GLARBRobustness
    {
        static GLARBRobustness() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_robustness") ?? false) _GL_ARB_robustness = 1;
        }
        private static uint _GL_ARB_robustness = 0;
        public static uint GL_ARB_robustness => _GL_ARB_robustness;
        public const uint GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT_ARB = 0x00000004;
        public const uint GL_LOSE_CONTEXT_ON_RESET_ARB = 0x8252;
        public const uint GL_GUILTY_CONTEXT_RESET_ARB = 0x8253;
        public const uint GL_INNOCENT_CONTEXT_RESET_ARB = 0x8254;
        public const uint GL_UNKNOWN_CONTEXT_RESET_ARB = 0x8255;
        public const uint GL_RESET_NOTIFICATION_STRATEGY_ARB = 0x8256;
        public const uint GL_NO_RESET_NOTIFICATION_ARB = 0x8261;

        private static delegate* unmanaged[Stdcall]<uint> pfn_glGetGraphicsResetStatusARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetGraphicsResetStatusARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glGetGraphicsResetStatusARB() => pfn_glGetGraphicsResetStatusARB();

        private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, int, void*, void> pfn_glGetnTexImageARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnTexImageARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnTexImageARB(uint target, int level, uint format, uint type, int bufSize, void* img) => pfn_glGetnTexImageARB(target, level, format, type, bufSize, img);

        private static delegate* unmanaged[Stdcall]<int, int, int, int, uint, uint, int, void*, void> pfn_glReadnPixelsARB = null;
        /// <summary> <see href="docs.gl/gl4/glReadnPixelsARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glReadnPixelsARB(int x, int y, int width, int height, uint format, uint type, int bufSize, void* data) => pfn_glReadnPixelsARB(x, y, width, height, format, type, bufSize, data);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void*, void> pfn_glGetnCompressedTexImageARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnCompressedTexImageARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnCompressedTexImageARB(uint target, int lod, int bufSize, void* img) => pfn_glGetnCompressedTexImageARB(target, lod, bufSize, img);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glGetnUniformfvARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformfvARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformfvARB(uint program, int location, int bufSize, float* @params) => pfn_glGetnUniformfvARB(program, location, bufSize, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glGetnUniformivARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformivARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformivARB(uint program, int location, int bufSize, int* @params) => pfn_glGetnUniformivARB(program, location, bufSize, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glGetnUniformuivARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformuivARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformuivARB(uint program, int location, int bufSize, uint* @params) => pfn_glGetnUniformuivARB(program, location, bufSize, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glGetnUniformdvARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetnUniformdvARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetnUniformdvARB(uint program, int location, int bufSize, double* @params) => pfn_glGetnUniformdvARB(program, location, bufSize, @params);
    }

    public static unsafe class GLARBRobustnessIsolation
    {
        static GLARBRobustnessIsolation() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_robustness_isolation") ?? false) _GL_ARB_robustness_isolation = 1;
        }
        private static uint _GL_ARB_robustness_isolation = 0;
        public static uint GL_ARB_robustness_isolation => _GL_ARB_robustness_isolation;
    }

    public static unsafe class GLARBSampleLocations
    {
        static GLARBSampleLocations() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sample_locations") ?? false) _GL_ARB_sample_locations = 1;
        }
        private static uint _GL_ARB_sample_locations = 0;
        public static uint GL_ARB_sample_locations => _GL_ARB_sample_locations;
        public const uint GL_SAMPLE_LOCATION_SUBPIXEL_BITS_ARB = 0x933D;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_WIDTH_ARB = 0x933E;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_HEIGHT_ARB = 0x933F;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_TABLE_SIZE_ARB = 0x9340;
        public const uint GL_SAMPLE_LOCATION_ARB = 0x8E50;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_ARB = 0x9341;
        public const uint GL_FRAMEBUFFER_PROGRAMMABLE_SAMPLE_LOCATIONS_ARB = 0x9342;
        public const uint GL_FRAMEBUFFER_SAMPLE_LOCATION_PIXEL_GRID_ARB = 0x9343;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, float*, void> pfn_glFramebufferSampleLocationsfvARB = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferSampleLocationsfvARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferSampleLocationsfvARB(uint target, uint start, int count, float* v) => pfn_glFramebufferSampleLocationsfvARB(target, start, count, v);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, float*, void> pfn_glNamedFramebufferSampleLocationsfvARB = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferSampleLocationsfvARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferSampleLocationsfvARB(uint framebuffer, uint start, int count, float* v) => pfn_glNamedFramebufferSampleLocationsfvARB(framebuffer, start, count, v);

        private static delegate* unmanaged[Stdcall]<void> pfn_glEvaluateDepthValuesARB = null;
        /// <summary> <see href="docs.gl/gl4/glEvaluateDepthValuesARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEvaluateDepthValuesARB() => pfn_glEvaluateDepthValuesARB();
    }

    public static unsafe class GLARBSampleShading
    {
        static GLARBSampleShading() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sample_shading") ?? false) _GL_ARB_sample_shading = 1;
        }
        private static uint _GL_ARB_sample_shading = 0;
        public static uint GL_ARB_sample_shading => _GL_ARB_sample_shading;
        public const uint GL_SAMPLE_SHADING_ARB = 0x8C36;
        public const uint GL_MIN_SAMPLE_SHADING_VALUE_ARB = 0x8C37;

        private static delegate* unmanaged[Stdcall]<float, void> pfn_glMinSampleShadingARB = null;
        /// <summary> <see href="docs.gl/gl4/glMinSampleShadingARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMinSampleShadingARB(float value) => pfn_glMinSampleShadingARB(value);
    }

    public static unsafe class GLARBSamplerObjects
    {
        static GLARBSamplerObjects() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sampler_objects") ?? false) _GL_ARB_sampler_objects = 1;
        }
        private static uint _GL_ARB_sampler_objects = 0;
        public static uint GL_ARB_sampler_objects => _GL_ARB_sampler_objects;
    }

    public static unsafe class GLARBSeamlessCubeMap
    {
        static GLARBSeamlessCubeMap() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_seamless_cube_map") ?? false) _GL_ARB_seamless_cube_map = 1;
        }
        private static uint _GL_ARB_seamless_cube_map = 0;
        public static uint GL_ARB_seamless_cube_map => _GL_ARB_seamless_cube_map;
    }

    public static unsafe class GLARBSeamlessCubemapPerTexture
    {
        static GLARBSeamlessCubemapPerTexture() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_seamless_cubemap_per_texture") ?? false) _GL_ARB_seamless_cubemap_per_texture = 1;
        }
        private static uint _GL_ARB_seamless_cubemap_per_texture = 0;
        public static uint GL_ARB_seamless_cubemap_per_texture => _GL_ARB_seamless_cubemap_per_texture;
    }

    public static unsafe class GLARBSeparateShaderObjects
    {
        static GLARBSeparateShaderObjects() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_separate_shader_objects") ?? false) _GL_ARB_separate_shader_objects = 1;
        }
        private static uint _GL_ARB_separate_shader_objects = 0;
        public static uint GL_ARB_separate_shader_objects => _GL_ARB_separate_shader_objects;
    }

    public static unsafe class GLARBShaderAtomicCounterOps
    {
        static GLARBShaderAtomicCounterOps() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_atomic_counter_ops") ?? false) _GL_ARB_shader_atomic_counter_ops = 1;
        }
        private static uint _GL_ARB_shader_atomic_counter_ops = 0;
        public static uint GL_ARB_shader_atomic_counter_ops => _GL_ARB_shader_atomic_counter_ops;
    }

    public static unsafe class GLARBShaderAtomicCounters
    {
        static GLARBShaderAtomicCounters() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_atomic_counters") ?? false) _GL_ARB_shader_atomic_counters = 1;
        }
        private static uint _GL_ARB_shader_atomic_counters = 0;
        public static uint GL_ARB_shader_atomic_counters => _GL_ARB_shader_atomic_counters;
    }

    public static unsafe class GLARBShaderBallot
    {
        static GLARBShaderBallot() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_ballot") ?? false) _GL_ARB_shader_ballot = 1;
        }
        private static uint _GL_ARB_shader_ballot = 0;
        public static uint GL_ARB_shader_ballot => _GL_ARB_shader_ballot;
    }

    public static unsafe class GLARBShaderBitEncoding
    {
        static GLARBShaderBitEncoding() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_bit_encoding") ?? false) _GL_ARB_shader_bit_encoding = 1;
        }
        private static uint _GL_ARB_shader_bit_encoding = 0;
        public static uint GL_ARB_shader_bit_encoding => _GL_ARB_shader_bit_encoding;
    }

    public static unsafe class GLARBShaderClock
    {
        static GLARBShaderClock() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_clock") ?? false) _GL_ARB_shader_clock = 1;
        }
        private static uint _GL_ARB_shader_clock = 0;
        public static uint GL_ARB_shader_clock => _GL_ARB_shader_clock;
    }

    public static unsafe class GLARBShaderDrawParameters
    {
        static GLARBShaderDrawParameters() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_draw_parameters") ?? false) _GL_ARB_shader_draw_parameters = 1;
        }
        private static uint _GL_ARB_shader_draw_parameters = 0;
        public static uint GL_ARB_shader_draw_parameters => _GL_ARB_shader_draw_parameters;
    }

    public static unsafe class GLARBShaderGroupVote
    {
        static GLARBShaderGroupVote() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_group_vote") ?? false) _GL_ARB_shader_group_vote = 1;
        }
        private static uint _GL_ARB_shader_group_vote = 0;
        public static uint GL_ARB_shader_group_vote => _GL_ARB_shader_group_vote;
    }

    public static unsafe class GLARBShaderImageLoadStore
    {
        static GLARBShaderImageLoadStore() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_image_load_store") ?? false) _GL_ARB_shader_image_load_store = 1;
        }
        private static uint _GL_ARB_shader_image_load_store = 0;
        public static uint GL_ARB_shader_image_load_store => _GL_ARB_shader_image_load_store;
    }

    public static unsafe class GLARBShaderImageSize
    {
        static GLARBShaderImageSize() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_image_size") ?? false) _GL_ARB_shader_image_size = 1;
        }
        private static uint _GL_ARB_shader_image_size = 0;
        public static uint GL_ARB_shader_image_size => _GL_ARB_shader_image_size;
    }

    public static unsafe class GLARBShaderPrecision
    {
        static GLARBShaderPrecision() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_precision") ?? false) _GL_ARB_shader_precision = 1;
        }
        private static uint _GL_ARB_shader_precision = 0;
        public static uint GL_ARB_shader_precision => _GL_ARB_shader_precision;
    }

    public static unsafe class GLARBShaderStencilExport
    {
        static GLARBShaderStencilExport() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_stencil_export") ?? false) _GL_ARB_shader_stencil_export = 1;
        }
        private static uint _GL_ARB_shader_stencil_export = 0;
        public static uint GL_ARB_shader_stencil_export => _GL_ARB_shader_stencil_export;
    }

    public static unsafe class GLARBShaderStorageBufferObject
    {
        static GLARBShaderStorageBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_storage_buffer_object") ?? false) _GL_ARB_shader_storage_buffer_object = 1;
        }
        private static uint _GL_ARB_shader_storage_buffer_object = 0;
        public static uint GL_ARB_shader_storage_buffer_object => _GL_ARB_shader_storage_buffer_object;
    }

    public static unsafe class GLARBShaderSubroutine
    {
        static GLARBShaderSubroutine() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_subroutine") ?? false) _GL_ARB_shader_subroutine = 1;
        }
        private static uint _GL_ARB_shader_subroutine = 0;
        public static uint GL_ARB_shader_subroutine => _GL_ARB_shader_subroutine;
    }

    public static unsafe class GLARBShaderTextureImageSamples
    {
        static GLARBShaderTextureImageSamples() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_texture_image_samples") ?? false) _GL_ARB_shader_texture_image_samples = 1;
        }
        private static uint _GL_ARB_shader_texture_image_samples = 0;
        public static uint GL_ARB_shader_texture_image_samples => _GL_ARB_shader_texture_image_samples;
    }

    public static unsafe class GLARBShaderViewportLayerArray
    {
        static GLARBShaderViewportLayerArray() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shader_viewport_layer_array") ?? false) _GL_ARB_shader_viewport_layer_array = 1;
        }
        private static uint _GL_ARB_shader_viewport_layer_array = 0;
        public static uint GL_ARB_shader_viewport_layer_array => _GL_ARB_shader_viewport_layer_array;
    }

    public static unsafe class GLARBShadingLanguage420pack
    {
        static GLARBShadingLanguage420pack() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shading_language_420pack") ?? false) _GL_ARB_shading_language_420pack = 1;
        }
        private static uint _GL_ARB_shading_language_420pack = 0;
        public static uint GL_ARB_shading_language_420pack => _GL_ARB_shading_language_420pack;
    }

    public static unsafe class GLARBShadingLanguageInclude
    {
        static GLARBShadingLanguageInclude() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shading_language_include") ?? false) _GL_ARB_shading_language_include = 1;
        }
        private static uint _GL_ARB_shading_language_include = 0;
        public static uint GL_ARB_shading_language_include => _GL_ARB_shading_language_include;
        public const uint GL_SHADER_INCLUDE_ARB = 0x8DAE;
        public const uint GL_NAMED_STRING_LENGTH_ARB = 0x8DE9;
        public const uint GL_NAMED_STRING_TYPE_ARB = 0x8DEA;

        private static delegate* unmanaged[Stdcall]<uint, int, byte*, int, byte*, void> pfn_glNamedStringARB = null;
        /// <summary> <see href="docs.gl/gl4/glNamedStringARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedStringARB(uint type, int namelen, byte* name, int stringlen, byte* @string) => pfn_glNamedStringARB(type, namelen, name, stringlen, @string);

        private static delegate* unmanaged[Stdcall]<int, byte*, void> pfn_glDeleteNamedStringARB = null;
        /// <summary> <see href="docs.gl/gl4/glDeleteNamedStringARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeleteNamedStringARB(int namelen, byte* name) => pfn_glDeleteNamedStringARB(namelen, name);

        private static delegate* unmanaged[Stdcall]<uint, int, byte**, int*, void> pfn_glCompileShaderIncludeARB = null;
        /// <summary> <see href="docs.gl/gl4/glCompileShaderIncludeARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompileShaderIncludeARB(uint shader, int count, byte** path, int* length) => pfn_glCompileShaderIncludeARB(shader, count, path, length);

        private static delegate* unmanaged[Stdcall]<int, byte*, byte> pfn_glIsNamedStringARB = null;
        /// <summary> <see href="docs.gl/gl4/glIsNamedStringARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsNamedStringARB(int namelen, byte* name) => pfn_glIsNamedStringARB(namelen, name);

        private static delegate* unmanaged[Stdcall]<int, byte*, int, int*, byte*, void> pfn_glGetNamedStringARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedStringARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedStringARB(int namelen, byte* name, int bufSize, int* stringlen, byte* @string) => pfn_glGetNamedStringARB(namelen, name, bufSize, stringlen, @string);

        private static delegate* unmanaged[Stdcall]<int, byte*, uint, int*, void> pfn_glGetNamedStringivARB = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedStringivARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedStringivARB(int namelen, byte* name, uint pname, int* @params) => pfn_glGetNamedStringivARB(namelen, name, pname, @params);
    }

    public static unsafe class GLARBShadingLanguagePacking
    {
        static GLARBShadingLanguagePacking() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_shading_language_packing") ?? false) _GL_ARB_shading_language_packing = 1;
        }
        private static uint _GL_ARB_shading_language_packing = 0;
        public static uint GL_ARB_shading_language_packing => _GL_ARB_shading_language_packing;
    }

    public static unsafe class GLARBSparseBuffer
    {
        static GLARBSparseBuffer() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sparse_buffer") ?? false) _GL_ARB_sparse_buffer = 1;
        }
        private static uint _GL_ARB_sparse_buffer = 0;
        public static uint GL_ARB_sparse_buffer => _GL_ARB_sparse_buffer;
        public const uint GL_SPARSE_STORAGE_BIT_ARB = 0x0400;
        public const uint GL_SPARSE_BUFFER_PAGE_SIZE_ARB = 0x82F8;

        private static delegate* unmanaged[Stdcall]<uint, long, long, byte, void> pfn_glBufferPageCommitmentARB = null;
        /// <summary> <see href="docs.gl/gl4/glBufferPageCommitmentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBufferPageCommitmentARB(uint target, long offset, long size, byte commit) => pfn_glBufferPageCommitmentARB(target, offset, size, commit);

        private static delegate* unmanaged[Stdcall]<uint, long, long, byte, void> pfn_glNamedBufferPageCommitmentEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferPageCommitmentEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferPageCommitmentEXT(uint buffer, long offset, long size, byte commit) => pfn_glNamedBufferPageCommitmentEXT(buffer, offset, size, commit);

        private static delegate* unmanaged[Stdcall]<uint, long, long, byte, void> pfn_glNamedBufferPageCommitmentARB = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferPageCommitmentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferPageCommitmentARB(uint buffer, long offset, long size, byte commit) => pfn_glNamedBufferPageCommitmentARB(buffer, offset, size, commit);
    }

    public static unsafe class GLARBSparseTexture
    {
        static GLARBSparseTexture() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sparse_texture") ?? false) _GL_ARB_sparse_texture = 1;
        }
        private static uint _GL_ARB_sparse_texture = 0;
        public static uint GL_ARB_sparse_texture => _GL_ARB_sparse_texture;
        public const uint GL_TEXTURE_SPARSE_ARB = 0x91A6;
        public const uint GL_VIRTUAL_PAGE_SIZE_INDEX_ARB = 0x91A7;
        public const uint GL_NUM_SPARSE_LEVELS_ARB = 0x91AA;
        public const uint GL_NUM_VIRTUAL_PAGE_SIZES_ARB = 0x91A8;
        public const uint GL_VIRTUAL_PAGE_SIZE_X_ARB = 0x9195;
        public const uint GL_VIRTUAL_PAGE_SIZE_Y_ARB = 0x9196;
        public const uint GL_VIRTUAL_PAGE_SIZE_Z_ARB = 0x9197;
        public const uint GL_MAX_SPARSE_TEXTURE_SIZE_ARB = 0x9198;
        public const uint GL_MAX_SPARSE_3D_TEXTURE_SIZE_ARB = 0x9199;
        public const uint GL_MAX_SPARSE_ARRAY_TEXTURE_LAYERS_ARB = 0x919A;
        public const uint GL_SPARSE_TEXTURE_FULL_ARRAY_CUBE_MIPMAPS_ARB = 0x91A9;

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, byte, void> pfn_glTexPageCommitmentARB = null;
        /// <summary> <see href="docs.gl/gl4/glTexPageCommitmentARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexPageCommitmentARB(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, byte commit) => pfn_glTexPageCommitmentARB(target, level, xoffset, yoffset, zoffset, width, height, depth, commit);
    }

    public static unsafe class GLARBSparseTexture2
    {
        static GLARBSparseTexture2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sparse_texture2") ?? false) _GL_ARB_sparse_texture2 = 1;
        }
        private static uint _GL_ARB_sparse_texture2 = 0;
        public static uint GL_ARB_sparse_texture2 => _GL_ARB_sparse_texture2;
    }

    public static unsafe class GLARBSparseTextureClamp
    {
        static GLARBSparseTextureClamp() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sparse_texture_clamp") ?? false) _GL_ARB_sparse_texture_clamp = 1;
        }
        private static uint _GL_ARB_sparse_texture_clamp = 0;
        public static uint GL_ARB_sparse_texture_clamp => _GL_ARB_sparse_texture_clamp;
    }

    public static unsafe class GLARBSpirvExtensions
    {
        static GLARBSpirvExtensions() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_spirv_extensions") ?? false) _GL_ARB_spirv_extensions = 1;
        }
        private static uint _GL_ARB_spirv_extensions = 0;
        public static uint GL_ARB_spirv_extensions => _GL_ARB_spirv_extensions;
    }

    public static unsafe class GLARBStencilTexturing
    {
        static GLARBStencilTexturing() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_stencil_texturing") ?? false) _GL_ARB_stencil_texturing = 1;
        }
        private static uint _GL_ARB_stencil_texturing = 0;
        public static uint GL_ARB_stencil_texturing => _GL_ARB_stencil_texturing;
    }

    public static unsafe class GLARBSync
    {
        static GLARBSync() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_sync") ?? false) _GL_ARB_sync = 1;
        }
        private static uint _GL_ARB_sync = 0;
        public static uint GL_ARB_sync => _GL_ARB_sync;
    }

    public static unsafe class GLARBTessellationShader
    {
        static GLARBTessellationShader() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_tessellation_shader") ?? false) _GL_ARB_tessellation_shader = 1;
        }
        private static uint _GL_ARB_tessellation_shader = 0;
        public static uint GL_ARB_tessellation_shader => _GL_ARB_tessellation_shader;
    }

    public static unsafe class GLARBTextureBarrier
    {
        static GLARBTextureBarrier() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_barrier") ?? false) _GL_ARB_texture_barrier = 1;
        }
        private static uint _GL_ARB_texture_barrier = 0;
        public static uint GL_ARB_texture_barrier => _GL_ARB_texture_barrier;
    }

    public static unsafe class GLARBTextureBorderClamp
    {
        static GLARBTextureBorderClamp() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_border_clamp") ?? false) _GL_ARB_texture_border_clamp = 1;
        }
        private static uint _GL_ARB_texture_border_clamp = 0;
        public static uint GL_ARB_texture_border_clamp => _GL_ARB_texture_border_clamp;
        public const uint GL_CLAMP_TO_BORDER_ARB = 0x812D;
    }

    public static unsafe class GLARBTextureBufferObject
    {
        static GLARBTextureBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_buffer_object") ?? false) _GL_ARB_texture_buffer_object = 1;
        }
        private static uint _GL_ARB_texture_buffer_object = 0;
        public static uint GL_ARB_texture_buffer_object => _GL_ARB_texture_buffer_object;
        public const uint GL_TEXTURE_BUFFER_ARB = 0x8C2A;
        public const uint GL_MAX_TEXTURE_BUFFER_SIZE_ARB = 0x8C2B;
        public const uint GL_TEXTURE_BINDING_BUFFER_ARB = 0x8C2C;
        public const uint GL_TEXTURE_BUFFER_DATA_STORE_BINDING_ARB = 0x8C2D;
        public const uint GL_TEXTURE_BUFFER_FORMAT_ARB = 0x8C2E;

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glTexBufferARB = null;
        /// <summary> <see href="docs.gl/gl4/glTexBufferARB">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexBufferARB(uint target, uint internalformat, uint buffer) => pfn_glTexBufferARB(target, internalformat, buffer);
    }

    public static unsafe class GLARBTextureBufferObjectRgb32
    {
        static GLARBTextureBufferObjectRgb32() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_buffer_object_rgb32") ?? false) _GL_ARB_texture_buffer_object_rgb32 = 1;
        }
        private static uint _GL_ARB_texture_buffer_object_rgb32 = 0;
        public static uint GL_ARB_texture_buffer_object_rgb32 => _GL_ARB_texture_buffer_object_rgb32;
    }

    public static unsafe class GLARBTextureBufferRange
    {
        static GLARBTextureBufferRange() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_buffer_range") ?? false) _GL_ARB_texture_buffer_range = 1;
        }
        private static uint _GL_ARB_texture_buffer_range = 0;
        public static uint GL_ARB_texture_buffer_range => _GL_ARB_texture_buffer_range;
    }

    public static unsafe class GLARBTextureCompressionBptc
    {
        static GLARBTextureCompressionBptc() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_compression_bptc") ?? false) _GL_ARB_texture_compression_bptc = 1;
        }
        private static uint _GL_ARB_texture_compression_bptc = 0;
        public static uint GL_ARB_texture_compression_bptc => _GL_ARB_texture_compression_bptc;
        public const uint GL_COMPRESSED_RGBA_BPTC_UNORM_ARB = 0x8E8C;
        public const uint GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB = 0x8E8D;
        public const uint GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT_ARB = 0x8E8E;
        public const uint GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT_ARB = 0x8E8F;
    }

    public static unsafe class GLARBTextureCompressionRgtc
    {
        static GLARBTextureCompressionRgtc() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_compression_rgtc") ?? false) _GL_ARB_texture_compression_rgtc = 1;
        }
        private static uint _GL_ARB_texture_compression_rgtc = 0;
        public static uint GL_ARB_texture_compression_rgtc => _GL_ARB_texture_compression_rgtc;
    }

    public static unsafe class GLARBTextureCubeMapArray
    {
        static GLARBTextureCubeMapArray() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_cube_map_array") ?? false) _GL_ARB_texture_cube_map_array = 1;
        }
        private static uint _GL_ARB_texture_cube_map_array = 0;
        public static uint GL_ARB_texture_cube_map_array => _GL_ARB_texture_cube_map_array;
        public const uint GL_TEXTURE_CUBE_MAP_ARRAY_ARB = 0x9009;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY_ARB = 0x900A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP_ARRAY_ARB = 0x900B;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_ARB = 0x900C;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW_ARB = 0x900D;
        public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY_ARB = 0x900E;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY_ARB = 0x900F;
    }

    public static unsafe class GLARBTextureFilterAnisotropic
    {
        static GLARBTextureFilterAnisotropic() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_filter_anisotropic") ?? false) _GL_ARB_texture_filter_anisotropic = 1;
        }
        private static uint _GL_ARB_texture_filter_anisotropic = 0;
        public static uint GL_ARB_texture_filter_anisotropic => _GL_ARB_texture_filter_anisotropic;
    }

    public static unsafe class GLARBTextureFilterMinmax
    {
        static GLARBTextureFilterMinmax() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_filter_minmax") ?? false) _GL_ARB_texture_filter_minmax = 1;
        }
        private static uint _GL_ARB_texture_filter_minmax = 0;
        public static uint GL_ARB_texture_filter_minmax => _GL_ARB_texture_filter_minmax;
        public const uint GL_TEXTURE_REDUCTION_MODE_ARB = 0x9366;
        public const uint GL_WEIGHTED_AVERAGE_ARB = 0x9367;
    }

    public static unsafe class GLARBTextureGather
    {
        static GLARBTextureGather() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_gather") ?? false) _GL_ARB_texture_gather = 1;
        }
        private static uint _GL_ARB_texture_gather = 0;
        public static uint GL_ARB_texture_gather => _GL_ARB_texture_gather;
        public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET_ARB = 0x8E5E;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET_ARB = 0x8E5F;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_COMPONENTS_ARB = 0x8F9F;
    }

    public static unsafe class GLARBTextureMirrorClampToEdge
    {
        static GLARBTextureMirrorClampToEdge() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_mirror_clamp_to_edge") ?? false) _GL_ARB_texture_mirror_clamp_to_edge = 1;
        }
        private static uint _GL_ARB_texture_mirror_clamp_to_edge = 0;
        public static uint GL_ARB_texture_mirror_clamp_to_edge => _GL_ARB_texture_mirror_clamp_to_edge;
    }

    public static unsafe class GLARBTextureMirroredRepeat
    {
        static GLARBTextureMirroredRepeat() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_mirrored_repeat") ?? false) _GL_ARB_texture_mirrored_repeat = 1;
        }
        private static uint _GL_ARB_texture_mirrored_repeat = 0;
        public static uint GL_ARB_texture_mirrored_repeat => _GL_ARB_texture_mirrored_repeat;
        public const uint GL_MIRRORED_REPEAT_ARB = 0x8370;
    }

    public static unsafe class GLARBTextureMultisample
    {
        static GLARBTextureMultisample() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_multisample") ?? false) _GL_ARB_texture_multisample = 1;
        }
        private static uint _GL_ARB_texture_multisample = 0;
        public static uint GL_ARB_texture_multisample => _GL_ARB_texture_multisample;
    }

    public static unsafe class GLARBTextureNonPowerOfTwo
    {
        static GLARBTextureNonPowerOfTwo() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_non_power_of_two") ?? false) _GL_ARB_texture_non_power_of_two = 1;
        }
        private static uint _GL_ARB_texture_non_power_of_two = 0;
        public static uint GL_ARB_texture_non_power_of_two => _GL_ARB_texture_non_power_of_two;
    }

    public static unsafe class GLARBTextureQueryLevels
    {
        static GLARBTextureQueryLevels() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_query_levels") ?? false) _GL_ARB_texture_query_levels = 1;
        }
        private static uint _GL_ARB_texture_query_levels = 0;
        public static uint GL_ARB_texture_query_levels => _GL_ARB_texture_query_levels;
    }

    public static unsafe class GLARBTextureQueryLod
    {
        static GLARBTextureQueryLod() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_query_lod") ?? false) _GL_ARB_texture_query_lod = 1;
        }
        private static uint _GL_ARB_texture_query_lod = 0;
        public static uint GL_ARB_texture_query_lod => _GL_ARB_texture_query_lod;
    }

    public static unsafe class GLARBTextureRg
    {
        static GLARBTextureRg() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_rg") ?? false) _GL_ARB_texture_rg = 1;
        }
        private static uint _GL_ARB_texture_rg = 0;
        public static uint GL_ARB_texture_rg => _GL_ARB_texture_rg;
    }

    public static unsafe class GLARBTextureRgb10A2ui
    {
        static GLARBTextureRgb10A2ui() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_rgb10_a2ui") ?? false) _GL_ARB_texture_rgb10_a2ui = 1;
        }
        private static uint _GL_ARB_texture_rgb10_a2ui = 0;
        public static uint GL_ARB_texture_rgb10_a2ui => _GL_ARB_texture_rgb10_a2ui;
    }

    public static unsafe class GLARBTextureStencil8
    {
        static GLARBTextureStencil8() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_stencil8") ?? false) _GL_ARB_texture_stencil8 = 1;
        }
        private static uint _GL_ARB_texture_stencil8 = 0;
        public static uint GL_ARB_texture_stencil8 => _GL_ARB_texture_stencil8;
    }

    public static unsafe class GLARBTextureStorage
    {
        static GLARBTextureStorage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_storage") ?? false) _GL_ARB_texture_storage = 1;
        }
        private static uint _GL_ARB_texture_storage = 0;
        public static uint GL_ARB_texture_storage => _GL_ARB_texture_storage;
    }

    public static unsafe class GLARBTextureStorageMultisample
    {
        static GLARBTextureStorageMultisample() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_storage_multisample") ?? false) _GL_ARB_texture_storage_multisample = 1;
        }
        private static uint _GL_ARB_texture_storage_multisample = 0;
        public static uint GL_ARB_texture_storage_multisample => _GL_ARB_texture_storage_multisample;
    }

    public static unsafe class GLARBTextureSwizzle
    {
        static GLARBTextureSwizzle() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_swizzle") ?? false) _GL_ARB_texture_swizzle = 1;
        }
        private static uint _GL_ARB_texture_swizzle = 0;
        public static uint GL_ARB_texture_swizzle => _GL_ARB_texture_swizzle;
    }

    public static unsafe class GLARBTextureView
    {
        static GLARBTextureView() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_texture_view") ?? false) _GL_ARB_texture_view = 1;
        }
        private static uint _GL_ARB_texture_view = 0;
        public static uint GL_ARB_texture_view => _GL_ARB_texture_view;
    }

    public static unsafe class GLARBTimerQuery
    {
        static GLARBTimerQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_timer_query") ?? false) _GL_ARB_timer_query = 1;
        }
        private static uint _GL_ARB_timer_query = 0;
        public static uint GL_ARB_timer_query => _GL_ARB_timer_query;
    }

    public static unsafe class GLARBTransformFeedback2
    {
        static GLARBTransformFeedback2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_transform_feedback2") ?? false) _GL_ARB_transform_feedback2 = 1;
        }
        private static uint _GL_ARB_transform_feedback2 = 0;
        public static uint GL_ARB_transform_feedback2 => _GL_ARB_transform_feedback2;
    }

    public static unsafe class GLARBTransformFeedback3
    {
        static GLARBTransformFeedback3() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_transform_feedback3") ?? false) _GL_ARB_transform_feedback3 = 1;
        }
        private static uint _GL_ARB_transform_feedback3 = 0;
        public static uint GL_ARB_transform_feedback3 => _GL_ARB_transform_feedback3;
    }

    public static unsafe class GLARBTransformFeedbackInstanced
    {
        static GLARBTransformFeedbackInstanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_transform_feedback_instanced") ?? false) _GL_ARB_transform_feedback_instanced = 1;
        }
        private static uint _GL_ARB_transform_feedback_instanced = 0;
        public static uint GL_ARB_transform_feedback_instanced => _GL_ARB_transform_feedback_instanced;
    }

    public static unsafe class GLARBTransformFeedbackOverflowQuery
    {
        static GLARBTransformFeedbackOverflowQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_transform_feedback_overflow_query") ?? false) _GL_ARB_transform_feedback_overflow_query = 1;
        }
        private static uint _GL_ARB_transform_feedback_overflow_query = 0;
        public static uint GL_ARB_transform_feedback_overflow_query => _GL_ARB_transform_feedback_overflow_query;
        public const uint GL_TRANSFORM_FEEDBACK_OVERFLOW_ARB = 0x82EC;
        public const uint GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW_ARB = 0x82ED;
    }

    public static unsafe class GLARBUniformBufferObject
    {
        static GLARBUniformBufferObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_uniform_buffer_object") ?? false) _GL_ARB_uniform_buffer_object = 1;
        }
        private static uint _GL_ARB_uniform_buffer_object = 0;
        public static uint GL_ARB_uniform_buffer_object => _GL_ARB_uniform_buffer_object;
    }

    public static unsafe class GLARBVertexArrayBgra
    {
        static GLARBVertexArrayBgra() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_array_bgra") ?? false) _GL_ARB_vertex_array_bgra = 1;
        }
        private static uint _GL_ARB_vertex_array_bgra = 0;
        public static uint GL_ARB_vertex_array_bgra => _GL_ARB_vertex_array_bgra;
    }

    public static unsafe class GLARBVertexArrayObject
    {
        static GLARBVertexArrayObject() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_array_object") ?? false) _GL_ARB_vertex_array_object = 1;
        }
        private static uint _GL_ARB_vertex_array_object = 0;
        public static uint GL_ARB_vertex_array_object => _GL_ARB_vertex_array_object;
    }

    public static unsafe class GLARBVertexAttrib64bit
    {
        static GLARBVertexAttrib64bit() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_attrib_64bit") ?? false) _GL_ARB_vertex_attrib_64bit = 1;
        }
        private static uint _GL_ARB_vertex_attrib_64bit = 0;
        public static uint GL_ARB_vertex_attrib_64bit => _GL_ARB_vertex_attrib_64bit;
    }

    public static unsafe class GLARBVertexAttribBinding
    {
        static GLARBVertexAttribBinding() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_attrib_binding") ?? false) _GL_ARB_vertex_attrib_binding = 1;
        }
        private static uint _GL_ARB_vertex_attrib_binding = 0;
        public static uint GL_ARB_vertex_attrib_binding => _GL_ARB_vertex_attrib_binding;
    }

    public static unsafe class GLARBVertexType10f11f11fRev
    {
        static GLARBVertexType10f11f11fRev() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_type_10f_11f_11f_rev") ?? false) _GL_ARB_vertex_type_10f_11f_11f_rev = 1;
        }
        private static uint _GL_ARB_vertex_type_10f_11f_11f_rev = 0;
        public static uint GL_ARB_vertex_type_10f_11f_11f_rev => _GL_ARB_vertex_type_10f_11f_11f_rev;
    }

    public static unsafe class GLARBVertexType2101010Rev
    {
        static GLARBVertexType2101010Rev() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_vertex_type_2_10_10_10_rev") ?? false) _GL_ARB_vertex_type_2_10_10_10_rev = 1;
        }
        private static uint _GL_ARB_vertex_type_2_10_10_10_rev = 0;
        public static uint GL_ARB_vertex_type_2_10_10_10_rev => _GL_ARB_vertex_type_2_10_10_10_rev;
    }

    public static unsafe class GLARBViewportArray
    {
        static GLARBViewportArray() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_ARB_viewport_array") ?? false) _GL_ARB_viewport_array = 1;
        }
        private static uint _GL_ARB_viewport_array = 0;
        public static uint GL_ARB_viewport_array => _GL_ARB_viewport_array;

        private static delegate* unmanaged[Stdcall]<uint, int, double*, void> pfn_glDepthRangeArraydvNV = null;
        /// <summary> <see href="docs.gl/gl4/glDepthRangeArraydvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDepthRangeArraydvNV(uint first, int count, double* v) => pfn_glDepthRangeArraydvNV(first, count, v);

        private static delegate* unmanaged[Stdcall]<uint, double, double, void> pfn_glDepthRangeIndexeddNV = null;
        /// <summary> <see href="docs.gl/gl4/glDepthRangeIndexeddNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDepthRangeIndexeddNV(uint index, double n, double f) => pfn_glDepthRangeIndexeddNV(index, n, f);
    }

    public static unsafe class GLKHRBlendEquationAdvanced
    {
        static GLKHRBlendEquationAdvanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_blend_equation_advanced") ?? false) _GL_KHR_blend_equation_advanced = 1;
        }
        private static uint _GL_KHR_blend_equation_advanced = 0;
        public static uint GL_KHR_blend_equation_advanced => _GL_KHR_blend_equation_advanced;
        public const uint GL_MULTIPLY_KHR = 0x9294;
        public const uint GL_SCREEN_KHR = 0x9295;
        public const uint GL_OVERLAY_KHR = 0x9296;
        public const uint GL_DARKEN_KHR = 0x9297;
        public const uint GL_LIGHTEN_KHR = 0x9298;
        public const uint GL_COLORDODGE_KHR = 0x9299;
        public const uint GL_COLORBURN_KHR = 0x929A;
        public const uint GL_HARDLIGHT_KHR = 0x929B;
        public const uint GL_SOFTLIGHT_KHR = 0x929C;
        public const uint GL_DIFFERENCE_KHR = 0x929E;
        public const uint GL_EXCLUSION_KHR = 0x92A0;
        public const uint GL_HSL_HUE_KHR = 0x92AD;
        public const uint GL_HSL_SATURATION_KHR = 0x92AE;
        public const uint GL_HSL_COLOR_KHR = 0x92AF;
        public const uint GL_HSL_LUMINOSITY_KHR = 0x92B0;

        private static delegate* unmanaged[Stdcall]<void> pfn_glBlendBarrierKHR = null;
        /// <summary> <see href="docs.gl/gl4/glBlendBarrierKHR">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendBarrierKHR() => pfn_glBlendBarrierKHR();
    }

    public static unsafe class GLKHRBlendEquationAdvancedCoherent
    {
        static GLKHRBlendEquationAdvancedCoherent() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_blend_equation_advanced_coherent") ?? false) _GL_KHR_blend_equation_advanced_coherent = 1;
        }
        private static uint _GL_KHR_blend_equation_advanced_coherent = 0;
        public static uint GL_KHR_blend_equation_advanced_coherent => _GL_KHR_blend_equation_advanced_coherent;
        public const uint GL_BLEND_ADVANCED_COHERENT_KHR = 0x9285;
    }

    public static unsafe class GLKHRContextFlushControl
    {
        static GLKHRContextFlushControl() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_context_flush_control") ?? false) _GL_KHR_context_flush_control = 1;
        }
        private static uint _GL_KHR_context_flush_control = 0;
        public static uint GL_KHR_context_flush_control => _GL_KHR_context_flush_control;
    }

    public static unsafe class GLKHRDebug
    {
        static GLKHRDebug() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_debug") ?? false) _GL_KHR_debug = 1;
        }
        private static uint _GL_KHR_debug = 0;
        public static uint GL_KHR_debug => _GL_KHR_debug;
    }

    public static unsafe class GLKHRNoError
    {
        static GLKHRNoError() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_no_error") ?? false) _GL_KHR_no_error = 1;
        }
        private static uint _GL_KHR_no_error = 0;
        public static uint GL_KHR_no_error => _GL_KHR_no_error;
        public const uint GL_CONTEXT_FLAG_NO_ERROR_BIT_KHR = 0x00000008;
    }

    public static unsafe class GLKHRParallelShaderCompile
    {
        static GLKHRParallelShaderCompile() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_parallel_shader_compile") ?? false) _GL_KHR_parallel_shader_compile = 1;
        }
        private static uint _GL_KHR_parallel_shader_compile = 0;
        public static uint GL_KHR_parallel_shader_compile => _GL_KHR_parallel_shader_compile;
        public const uint GL_MAX_SHADER_COMPILER_THREADS_KHR = 0x91B0;
        public const uint GL_COMPLETION_STATUS_KHR = 0x91B1;

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMaxShaderCompilerThreadsKHR = null;
        /// <summary> <see href="docs.gl/gl4/glMaxShaderCompilerThreadsKHR">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMaxShaderCompilerThreadsKHR(uint count) => pfn_glMaxShaderCompilerThreadsKHR(count);
    }

    public static unsafe class GLKHRRobustBufferAccessBehavior
    {
        static GLKHRRobustBufferAccessBehavior() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_robust_buffer_access_behavior") ?? false) _GL_KHR_robust_buffer_access_behavior = 1;
        }
        private static uint _GL_KHR_robust_buffer_access_behavior = 0;
        public static uint GL_KHR_robust_buffer_access_behavior => _GL_KHR_robust_buffer_access_behavior;
    }

    public static unsafe class GLKHRRobustness
    {
        static GLKHRRobustness() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_robustness") ?? false) _GL_KHR_robustness = 1;
        }
        private static uint _GL_KHR_robustness = 0;
        public static uint GL_KHR_robustness => _GL_KHR_robustness;
        public const uint GL_CONTEXT_ROBUST_ACCESS = 0x90F3;
    }

    public static unsafe class GLKHRShaderSubgroup
    {
        static GLKHRShaderSubgroup() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_shader_subgroup") ?? false) _GL_KHR_shader_subgroup = 1;
        }
        private static uint _GL_KHR_shader_subgroup = 0;
        public static uint GL_KHR_shader_subgroup => _GL_KHR_shader_subgroup;
        public const uint GL_SUBGROUP_SIZE_KHR = 0x9532;
        public const uint GL_SUBGROUP_SUPPORTED_STAGES_KHR = 0x9533;
        public const uint GL_SUBGROUP_SUPPORTED_FEATURES_KHR = 0x9534;
        public const uint GL_SUBGROUP_QUAD_ALL_STAGES_KHR = 0x9535;
        public const uint GL_SUBGROUP_FEATURE_BASIC_BIT_KHR = 0x00000001;
        public const uint GL_SUBGROUP_FEATURE_VOTE_BIT_KHR = 0x00000002;
        public const uint GL_SUBGROUP_FEATURE_ARITHMETIC_BIT_KHR = 0x00000004;
        public const uint GL_SUBGROUP_FEATURE_BALLOT_BIT_KHR = 0x00000008;
        public const uint GL_SUBGROUP_FEATURE_SHUFFLE_BIT_KHR = 0x00000010;
        public const uint GL_SUBGROUP_FEATURE_SHUFFLE_RELATIVE_BIT_KHR = 0x00000020;
        public const uint GL_SUBGROUP_FEATURE_CLUSTERED_BIT_KHR = 0x00000040;
        public const uint GL_SUBGROUP_FEATURE_QUAD_BIT_KHR = 0x00000080;
    }

    public static unsafe class GLKHRTextureCompressionAstcHdr
    {
        static GLKHRTextureCompressionAstcHdr() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_texture_compression_astc_hdr") ?? false) _GL_KHR_texture_compression_astc_hdr = 1;
        }
        private static uint _GL_KHR_texture_compression_astc_hdr = 0;
        public static uint GL_KHR_texture_compression_astc_hdr => _GL_KHR_texture_compression_astc_hdr;
        public const uint GL_COMPRESSED_RGBA_ASTC_4x4_KHR = 0x93B0;
        public const uint GL_COMPRESSED_RGBA_ASTC_5x4_KHR = 0x93B1;
        public const uint GL_COMPRESSED_RGBA_ASTC_5x5_KHR = 0x93B2;
        public const uint GL_COMPRESSED_RGBA_ASTC_6x5_KHR = 0x93B3;
        public const uint GL_COMPRESSED_RGBA_ASTC_6x6_KHR = 0x93B4;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x5_KHR = 0x93B5;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x6_KHR = 0x93B6;
        public const uint GL_COMPRESSED_RGBA_ASTC_8x8_KHR = 0x93B7;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x5_KHR = 0x93B8;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x6_KHR = 0x93B9;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x8_KHR = 0x93BA;
        public const uint GL_COMPRESSED_RGBA_ASTC_10x10_KHR = 0x93BB;
        public const uint GL_COMPRESSED_RGBA_ASTC_12x10_KHR = 0x93BC;
        public const uint GL_COMPRESSED_RGBA_ASTC_12x12_KHR = 0x93BD;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4_KHR = 0x93D0;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4_KHR = 0x93D1;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5_KHR = 0x93D2;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5_KHR = 0x93D3;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6_KHR = 0x93D4;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5_KHR = 0x93D5;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6_KHR = 0x93D6;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8_KHR = 0x93D7;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5_KHR = 0x93D8;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6_KHR = 0x93D9;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8_KHR = 0x93DA;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10_KHR = 0x93DB;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10_KHR = 0x93DC;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12_KHR = 0x93DD;
    }

    public static unsafe class GLKHRTextureCompressionAstcLdr
    {
        static GLKHRTextureCompressionAstcLdr() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_texture_compression_astc_ldr") ?? false) _GL_KHR_texture_compression_astc_ldr = 1;
        }
        private static uint _GL_KHR_texture_compression_astc_ldr = 0;
        public static uint GL_KHR_texture_compression_astc_ldr => _GL_KHR_texture_compression_astc_ldr;
    }

    public static unsafe class GLKHRTextureCompressionAstcSliced3d
    {
        static GLKHRTextureCompressionAstcSliced3d() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_KHR_texture_compression_astc_sliced_3d") ?? false) _GL_KHR_texture_compression_astc_sliced_3d = 1;
        }
        private static uint _GL_KHR_texture_compression_astc_sliced_3d = 0;
        public static uint GL_KHR_texture_compression_astc_sliced_3d => _GL_KHR_texture_compression_astc_sliced_3d;
    }

    public static unsafe class GLAMDFramebufferMultisampleAdvanced
    {
        static GLAMDFramebufferMultisampleAdvanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_AMD_framebuffer_multisample_advanced") ?? false) _GL_AMD_framebuffer_multisample_advanced = 1;
        }
        private static uint _GL_AMD_framebuffer_multisample_advanced = 0;
        public static uint GL_AMD_framebuffer_multisample_advanced => _GL_AMD_framebuffer_multisample_advanced;
        public const uint GL_RENDERBUFFER_STORAGE_SAMPLES_AMD = 0x91B2;
        public const uint GL_MAX_COLOR_FRAMEBUFFER_SAMPLES_AMD = 0x91B3;
        public const uint GL_MAX_COLOR_FRAMEBUFFER_STORAGE_SAMPLES_AMD = 0x91B4;
        public const uint GL_MAX_DEPTH_STENCIL_FRAMEBUFFER_SAMPLES_AMD = 0x91B5;
        public const uint GL_NUM_SUPPORTED_MULTISAMPLE_MODES_AMD = 0x91B6;
        public const uint GL_SUPPORTED_MULTISAMPLE_MODES_AMD = 0x91B7;

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint, int, int, void> pfn_glRenderbufferStorageMultisampleAdvancedAMD = null;
        /// <summary> <see href="docs.gl/gl4/glRenderbufferStorageMultisampleAdvancedAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glRenderbufferStorageMultisampleAdvancedAMD(uint target, int samples, int storageSamples, uint internalformat, int width, int height) => pfn_glRenderbufferStorageMultisampleAdvancedAMD(target, samples, storageSamples, internalformat, width, height);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint, int, int, void> pfn_glNamedRenderbufferStorageMultisampleAdvancedAMD = null;
        /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorageMultisampleAdvancedAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedRenderbufferStorageMultisampleAdvancedAMD(uint renderbuffer, int samples, int storageSamples, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorageMultisampleAdvancedAMD(renderbuffer, samples, storageSamples, internalformat, width, height);
    }

    public static unsafe class GLAMDPerformanceMonitor
    {
        static GLAMDPerformanceMonitor() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_AMD_performance_monitor") ?? false) _GL_AMD_performance_monitor = 1;
        }
        private static uint _GL_AMD_performance_monitor = 0;
        public static uint GL_AMD_performance_monitor => _GL_AMD_performance_monitor;
        public const uint GL_COUNTER_TYPE_AMD = 0x8BC0;
        public const uint GL_COUNTER_RANGE_AMD = 0x8BC1;
        public const uint GL_UNSIGNED_INT64_AMD = 0x8BC2;
        public const uint GL_PERCENTAGE_AMD = 0x8BC3;
        public const uint GL_PERFMON_RESULT_AVAILABLE_AMD = 0x8BC4;
        public const uint GL_PERFMON_RESULT_SIZE_AMD = 0x8BC5;
        public const uint GL_PERFMON_RESULT_AMD = 0x8BC6;

        private static delegate* unmanaged[Stdcall]<int*, int, uint*, void> pfn_glGetPerfMonitorGroupsAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorGroupsAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorGroupsAMD(int* numGroups, int groupsSize, uint* groups) => pfn_glGetPerfMonitorGroupsAMD(numGroups, groupsSize, groups);

        private static delegate* unmanaged[Stdcall]<uint, int*, int*, int, uint*, void> pfn_glGetPerfMonitorCountersAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorCountersAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorCountersAMD(uint group, int* numCounters, int* maxActiveCounters, int counterSize, uint* counters) => pfn_glGetPerfMonitorCountersAMD(group, numCounters, maxActiveCounters, counterSize, counters);

        private static delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void> pfn_glGetPerfMonitorGroupStringAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorGroupStringAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorGroupStringAMD(uint group, int bufSize, int* length, byte* groupString) => pfn_glGetPerfMonitorGroupStringAMD(group, bufSize, length, groupString);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void> pfn_glGetPerfMonitorCounterStringAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorCounterStringAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorCounterStringAMD(uint group, uint counter, int bufSize, int* length, byte* counterString) => pfn_glGetPerfMonitorCounterStringAMD(group, counter, bufSize, length, counterString);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void*, void> pfn_glGetPerfMonitorCounterInfoAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorCounterInfoAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorCounterInfoAMD(uint group, uint counter, uint pname, void* data) => pfn_glGetPerfMonitorCounterInfoAMD(group, counter, pname, data);

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glGenPerfMonitorsAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGenPerfMonitorsAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGenPerfMonitorsAMD(int n, uint* monitors) => pfn_glGenPerfMonitorsAMD(n, monitors);

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeletePerfMonitorsAMD = null;
        /// <summary> <see href="docs.gl/gl4/glDeletePerfMonitorsAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeletePerfMonitorsAMD(int n, uint* monitors) => pfn_glDeletePerfMonitorsAMD(n, monitors);

        private static delegate* unmanaged[Stdcall]<uint, byte, uint, int, uint*, void> pfn_glSelectPerfMonitorCountersAMD = null;
        /// <summary> <see href="docs.gl/gl4/glSelectPerfMonitorCountersAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSelectPerfMonitorCountersAMD(uint monitor, byte enable, uint group, int numCounters, uint* counterList) => pfn_glSelectPerfMonitorCountersAMD(monitor, enable, group, numCounters, counterList);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBeginPerfMonitorAMD = null;
        /// <summary> <see href="docs.gl/gl4/glBeginPerfMonitorAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBeginPerfMonitorAMD(uint monitor) => pfn_glBeginPerfMonitorAMD(monitor);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glEndPerfMonitorAMD = null;
        /// <summary> <see href="docs.gl/gl4/glEndPerfMonitorAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEndPerfMonitorAMD(uint monitor) => pfn_glEndPerfMonitorAMD(monitor);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, int*, void> pfn_glGetPerfMonitorCounterDataAMD = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfMonitorCounterDataAMD">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfMonitorCounterDataAMD(uint monitor, uint pname, int dataSize, uint* data, int* bytesWritten) => pfn_glGetPerfMonitorCounterDataAMD(monitor, pname, dataSize, data, bytesWritten);
    }

    public static unsafe class GLAPPLERgb422
    {
        static GLAPPLERgb422() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_APPLE_rgb_422") ?? false) _GL_APPLE_rgb_422 = 1;
        }
        private static uint _GL_APPLE_rgb_422 = 0;
        public static uint GL_APPLE_rgb_422 => _GL_APPLE_rgb_422;
        public const uint GL_RGB_422_APPLE = 0x8A1F;
        public const uint GL_UNSIGNED_SHORT_8_8_APPLE = 0x85BA;
        public const uint GL_UNSIGNED_SHORT_8_8_REV_APPLE = 0x85BB;
        public const uint GL_RGB_RAW_422_APPLE = 0x8A51;
    }

    public static unsafe class GLEXTEGLImageStorage
    {
        static GLEXTEGLImageStorage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_EGL_image_storage") ?? false) _GL_EXT_EGL_image_storage = 1;
        }
        private static uint _GL_EXT_EGL_image_storage = 0;
        public static uint GL_EXT_EGL_image_storage => _GL_EXT_EGL_image_storage;

        private static delegate* unmanaged[Stdcall]<uint, void*, int*, void> pfn_glEGLImageTargetTexStorageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEGLImageTargetTexStorageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEGLImageTargetTexStorageEXT(uint target, void* image, int* attrib_list) => pfn_glEGLImageTargetTexStorageEXT(target, image, attrib_list);

        private static delegate* unmanaged[Stdcall]<uint, void*, int*, void> pfn_glEGLImageTargetTextureStorageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEGLImageTargetTextureStorageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEGLImageTargetTextureStorageEXT(uint texture, void* image, int* attrib_list) => pfn_glEGLImageTargetTextureStorageEXT(texture, image, attrib_list);
    }

    public static unsafe class GLEXTEGLSync
    {
        static GLEXTEGLSync() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_EGL_sync") ?? false) _GL_EXT_EGL_sync = 1;
        }
        private static uint _GL_EXT_EGL_sync = 0;
        public static uint GL_EXT_EGL_sync => _GL_EXT_EGL_sync;
    }

    public static unsafe class GLEXTDebugLabel
    {
        static GLEXTDebugLabel() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_debug_label") ?? false) _GL_EXT_debug_label = 1;
        }
        private static uint _GL_EXT_debug_label = 0;
        public static uint GL_EXT_debug_label => _GL_EXT_debug_label;
        public const uint GL_PROGRAM_PIPELINE_OBJECT_EXT = 0x8A4F;
        public const uint GL_PROGRAM_OBJECT_EXT = 0x8B40;
        public const uint GL_SHADER_OBJECT_EXT = 0x8B48;
        public const uint GL_BUFFER_OBJECT_EXT = 0x9151;
        public const uint GL_QUERY_OBJECT_EXT = 0x9153;
        public const uint GL_VERTEX_ARRAY_OBJECT_EXT = 0x9154;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, byte*, void> pfn_glLabelObjectEXT = null;
        /// <summary> <see href="docs.gl/gl4/glLabelObjectEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glLabelObjectEXT(uint type, uint @object, int length, byte* label) => pfn_glLabelObjectEXT(type, @object, length, label);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void> pfn_glGetObjectLabelEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetObjectLabelEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetObjectLabelEXT(uint type, uint @object, int bufSize, int* length, byte* label) => pfn_glGetObjectLabelEXT(type, @object, bufSize, length, label);
    }

    public static unsafe class GLEXTDebugMarker
    {
        static GLEXTDebugMarker() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_debug_marker") ?? false) _GL_EXT_debug_marker = 1;
        }
        private static uint _GL_EXT_debug_marker = 0;
        public static uint GL_EXT_debug_marker => _GL_EXT_debug_marker;

        private static delegate* unmanaged[Stdcall]<int, byte*, void> pfn_glInsertEventMarkerEXT = null;
        /// <summary> <see href="docs.gl/gl4/glInsertEventMarkerEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glInsertEventMarkerEXT(int length, byte* marker) => pfn_glInsertEventMarkerEXT(length, marker);

        private static delegate* unmanaged[Stdcall]<int, byte*, void> pfn_glPushGroupMarkerEXT = null;
        /// <summary> <see href="docs.gl/gl4/glPushGroupMarkerEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPushGroupMarkerEXT(int length, byte* marker) => pfn_glPushGroupMarkerEXT(length, marker);

        private static delegate* unmanaged[Stdcall]<void> pfn_glPopGroupMarkerEXT = null;
        /// <summary> <see href="docs.gl/gl4/glPopGroupMarkerEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPopGroupMarkerEXT() => pfn_glPopGroupMarkerEXT();
    }

    public static unsafe class GLEXTDirectStateAccess
    {
        static GLEXTDirectStateAccess() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_direct_state_access") ?? false) _GL_EXT_direct_state_access = 1;
        }
        private static uint _GL_EXT_direct_state_access = 0;
        public static uint GL_EXT_direct_state_access => _GL_EXT_direct_state_access;
        public const uint GL_PROGRAM_MATRIX_EXT = 0x8E2D;
        public const uint GL_TRANSPOSE_PROGRAM_MATRIX_EXT = 0x8E2E;
        public const uint GL_PROGRAM_MATRIX_STACK_DEPTH_EXT = 0x8E2F;

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixLoadfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoadfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoadfEXT(uint mode, float* m) => pfn_glMatrixLoadfEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glMatrixLoaddEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoaddEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoaddEXT(uint mode, double* m) => pfn_glMatrixLoaddEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixMultfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMultfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMultfEXT(uint mode, float* m) => pfn_glMatrixMultfEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glMatrixMultdEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMultdEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMultdEXT(uint mode, double* m) => pfn_glMatrixMultdEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMatrixLoadIdentityEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoadIdentityEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoadIdentityEXT(uint mode) => pfn_glMatrixLoadIdentityEXT(mode);

        private static delegate* unmanaged[Stdcall]<uint, float, float, float, float, void> pfn_glMatrixRotatefEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixRotatefEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixRotatefEXT(uint mode, float angle, float x, float y, float z) => pfn_glMatrixRotatefEXT(mode, angle, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, double, double, double, double, void> pfn_glMatrixRotatedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixRotatedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixRotatedEXT(uint mode, double angle, double x, double y, double z) => pfn_glMatrixRotatedEXT(mode, angle, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, float, float, float, void> pfn_glMatrixScalefEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixScalefEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixScalefEXT(uint mode, float x, float y, float z) => pfn_glMatrixScalefEXT(mode, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, double, double, double, void> pfn_glMatrixScaledEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixScaledEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixScaledEXT(uint mode, double x, double y, double z) => pfn_glMatrixScaledEXT(mode, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, float, float, float, void> pfn_glMatrixTranslatefEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixTranslatefEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixTranslatefEXT(uint mode, float x, float y, float z) => pfn_glMatrixTranslatefEXT(mode, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, double, double, double, void> pfn_glMatrixTranslatedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixTranslatedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixTranslatedEXT(uint mode, double x, double y, double z) => pfn_glMatrixTranslatedEXT(mode, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, double, double, double, double, double, double, void> pfn_glMatrixFrustumEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixFrustumEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixFrustumEXT(uint mode, double left, double right, double bottom, double top, double zNear, double zFar) => pfn_glMatrixFrustumEXT(mode, left, right, bottom, top, zNear, zFar);

        private static delegate* unmanaged[Stdcall]<uint, double, double, double, double, double, double, void> pfn_glMatrixOrthoEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixOrthoEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixOrthoEXT(uint mode, double left, double right, double bottom, double top, double zNear, double zFar) => pfn_glMatrixOrthoEXT(mode, left, right, bottom, top, zNear, zFar);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMatrixPopEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixPopEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixPopEXT(uint mode) => pfn_glMatrixPopEXT(mode);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMatrixPushEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixPushEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixPushEXT(uint mode) => pfn_glMatrixPushEXT(mode);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glClientAttribDefaultEXT = null;
        /// <summary> <see href="docs.gl/gl4/glClientAttribDefaultEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glClientAttribDefaultEXT(uint mask) => pfn_glClientAttribDefaultEXT(mask);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glPushClientAttribDefaultEXT = null;
        /// <summary> <see href="docs.gl/gl4/glPushClientAttribDefaultEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPushClientAttribDefaultEXT(uint mask) => pfn_glPushClientAttribDefaultEXT(mask);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, void> pfn_glTextureParameterfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameterfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameterfEXT(uint texture, uint target, uint pname, float param) => pfn_glTextureParameterfEXT(texture, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glTextureParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameterfvEXT(uint texture, uint target, uint pname, float* @params) => pfn_glTextureParameterfvEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glTextureParameteriEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameteriEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameteriEXT(uint texture, uint target, uint pname, int param) => pfn_glTextureParameteriEXT(texture, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glTextureParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameterivEXT(uint texture, uint target, uint pname, int* @params) => pfn_glTextureParameterivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, uint, uint, void*, void> pfn_glTextureImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureImage1DEXT(uint texture, uint target, int level, int internalformat, int width, int border, uint format, uint type, void* pixels) => pfn_glTextureImage1DEXT(texture, target, level, internalformat, width, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureImage2DEXT(uint texture, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels) => pfn_glTextureImage2DEXT(texture, target, level, internalformat, width, height, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureSubImage1DEXT(uint texture, uint target, int level, int xoffset, int width, uint format, uint type, void* pixels) => pfn_glTextureSubImage1DEXT(texture, target, level, xoffset, width, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureSubImage2DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) => pfn_glTextureSubImage2DEXT(texture, target, level, xoffset, yoffset, width, height, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, void> pfn_glCopyTextureImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyTextureImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyTextureImage1DEXT(uint texture, uint target, int level, uint internalformat, int x, int y, int width, int border) => pfn_glCopyTextureImage1DEXT(texture, target, level, internalformat, x, y, width, border);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, int, void> pfn_glCopyTextureImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyTextureImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyTextureImage2DEXT(uint texture, uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => pfn_glCopyTextureImage2DEXT(texture, target, level, internalformat, x, y, width, height, border);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, void> pfn_glCopyTextureSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyTextureSubImage1DEXT(uint texture, uint target, int level, int xoffset, int x, int y, int width) => pfn_glCopyTextureSubImage1DEXT(texture, target, level, xoffset, x, y, width);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, void> pfn_glCopyTextureSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyTextureSubImage2DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => pfn_glCopyTextureSubImage2DEXT(texture, target, level, xoffset, yoffset, x, y, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void*, void> pfn_glGetTextureImageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureImageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureImageEXT(uint texture, uint target, int level, uint format, uint type, void* pixels) => pfn_glGetTextureImageEXT(texture, target, level, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glGetTextureParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureParameterfvEXT(uint texture, uint target, uint pname, float* @params) => pfn_glGetTextureParameterfvEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetTextureParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureParameterivEXT(uint texture, uint target, uint pname, int* @params) => pfn_glGetTextureParameterivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, float*, void> pfn_glGetTextureLevelParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureLevelParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureLevelParameterfvEXT(uint texture, uint target, int level, uint pname, float* @params) => pfn_glGetTextureLevelParameterfvEXT(texture, target, level, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int*, void> pfn_glGetTextureLevelParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureLevelParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureLevelParameterivEXT(uint texture, uint target, int level, uint pname, int* @params) => pfn_glGetTextureLevelParameterivEXT(texture, target, level, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureImage3DEXT(uint texture, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels) => pfn_glTextureImage3DEXT(texture, target, level, internalformat, width, height, depth, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, uint, uint, void*, void> pfn_glTextureSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureSubImage3DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels) => pfn_glTextureSubImage3DEXT(texture, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, int, void> pfn_glCopyTextureSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyTextureSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyTextureSubImage3DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => pfn_glCopyTextureSubImage3DEXT(texture, target, level, xoffset, yoffset, zoffset, x, y, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glBindMultiTextureEXT = null;
        /// <summary> <see href="docs.gl/gl4/glBindMultiTextureEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBindMultiTextureEXT(uint texunit, uint target, uint texture) => pfn_glBindMultiTextureEXT(texunit, target, texture);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void*, void> pfn_glMultiTexCoordPointerEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexCoordPointerEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexCoordPointerEXT(uint texunit, int size, uint type, int stride, void* pointer) => pfn_glMultiTexCoordPointerEXT(texunit, size, type, stride, pointer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, void> pfn_glMultiTexEnvfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexEnvfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexEnvfEXT(uint texunit, uint target, uint pname, float param) => pfn_glMultiTexEnvfEXT(texunit, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glMultiTexEnvfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexEnvfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexEnvfvEXT(uint texunit, uint target, uint pname, float* @params) => pfn_glMultiTexEnvfvEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glMultiTexEnviEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexEnviEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexEnviEXT(uint texunit, uint target, uint pname, int param) => pfn_glMultiTexEnviEXT(texunit, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glMultiTexEnvivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexEnvivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexEnvivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glMultiTexEnvivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double, void> pfn_glMultiTexGendEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGendEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGendEXT(uint texunit, uint coord, uint pname, double param) => pfn_glMultiTexGendEXT(texunit, coord, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double*, void> pfn_glMultiTexGendvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGendvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGendvEXT(uint texunit, uint coord, uint pname, double* @params) => pfn_glMultiTexGendvEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, void> pfn_glMultiTexGenfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGenfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGenfEXT(uint texunit, uint coord, uint pname, float param) => pfn_glMultiTexGenfEXT(texunit, coord, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glMultiTexGenfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGenfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGenfvEXT(uint texunit, uint coord, uint pname, float* @params) => pfn_glMultiTexGenfvEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glMultiTexGeniEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGeniEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGeniEXT(uint texunit, uint coord, uint pname, int param) => pfn_glMultiTexGeniEXT(texunit, coord, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glMultiTexGenivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexGenivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexGenivEXT(uint texunit, uint coord, uint pname, int* @params) => pfn_glMultiTexGenivEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glGetMultiTexEnvfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexEnvfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexEnvfvEXT(uint texunit, uint target, uint pname, float* @params) => pfn_glGetMultiTexEnvfvEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetMultiTexEnvivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexEnvivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexEnvivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glGetMultiTexEnvivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double*, void> pfn_glGetMultiTexGendvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexGendvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexGendvEXT(uint texunit, uint coord, uint pname, double* @params) => pfn_glGetMultiTexGendvEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glGetMultiTexGenfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexGenfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexGenfvEXT(uint texunit, uint coord, uint pname, float* @params) => pfn_glGetMultiTexGenfvEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetMultiTexGenivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexGenivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexGenivEXT(uint texunit, uint coord, uint pname, int* @params) => pfn_glGetMultiTexGenivEXT(texunit, coord, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glMultiTexParameteriEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameteriEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameteriEXT(uint texunit, uint target, uint pname, int param) => pfn_glMultiTexParameteriEXT(texunit, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glMultiTexParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameterivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glMultiTexParameterivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, void> pfn_glMultiTexParameterfEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameterfEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameterfEXT(uint texunit, uint target, uint pname, float param) => pfn_glMultiTexParameterfEXT(texunit, target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glMultiTexParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameterfvEXT(uint texunit, uint target, uint pname, float* @params) => pfn_glMultiTexParameterfvEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, uint, uint, void*, void> pfn_glMultiTexImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexImage1DEXT(uint texunit, uint target, int level, int internalformat, int width, int border, uint format, uint type, void* pixels) => pfn_glMultiTexImage1DEXT(texunit, target, level, internalformat, width, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, uint, void*, void> pfn_glMultiTexImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexImage2DEXT(uint texunit, uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels) => pfn_glMultiTexImage2DEXT(texunit, target, level, internalformat, width, height, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, uint, uint, void*, void> pfn_glMultiTexSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexSubImage1DEXT(uint texunit, uint target, int level, int xoffset, int width, uint format, uint type, void* pixels) => pfn_glMultiTexSubImage1DEXT(texunit, target, level, xoffset, width, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, uint, void*, void> pfn_glMultiTexSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexSubImage2DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) => pfn_glMultiTexSubImage2DEXT(texunit, target, level, xoffset, yoffset, width, height, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, void> pfn_glCopyMultiTexImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyMultiTexImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyMultiTexImage1DEXT(uint texunit, uint target, int level, uint internalformat, int x, int y, int width, int border) => pfn_glCopyMultiTexImage1DEXT(texunit, target, level, internalformat, x, y, width, border);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, int, void> pfn_glCopyMultiTexImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyMultiTexImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyMultiTexImage2DEXT(uint texunit, uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => pfn_glCopyMultiTexImage2DEXT(texunit, target, level, internalformat, x, y, width, height, border);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, void> pfn_glCopyMultiTexSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyMultiTexSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyMultiTexSubImage1DEXT(uint texunit, uint target, int level, int xoffset, int x, int y, int width) => pfn_glCopyMultiTexSubImage1DEXT(texunit, target, level, xoffset, x, y, width);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, void> pfn_glCopyMultiTexSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyMultiTexSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyMultiTexSubImage2DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => pfn_glCopyMultiTexSubImage2DEXT(texunit, target, level, xoffset, yoffset, x, y, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void*, void> pfn_glGetMultiTexImageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexImageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexImageEXT(uint texunit, uint target, int level, uint format, uint type, void* pixels) => pfn_glGetMultiTexImageEXT(texunit, target, level, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glGetMultiTexParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexParameterfvEXT(uint texunit, uint target, uint pname, float* @params) => pfn_glGetMultiTexParameterfvEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetMultiTexParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexParameterivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glGetMultiTexParameterivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, float*, void> pfn_glGetMultiTexLevelParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexLevelParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexLevelParameterfvEXT(uint texunit, uint target, int level, uint pname, float* @params) => pfn_glGetMultiTexLevelParameterfvEXT(texunit, target, level, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int*, void> pfn_glGetMultiTexLevelParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexLevelParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexLevelParameterivEXT(uint texunit, uint target, int level, uint pname, int* @params) => pfn_glGetMultiTexLevelParameterivEXT(texunit, target, level, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, uint, uint, void*, void> pfn_glMultiTexImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexImage3DEXT(uint texunit, uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels) => pfn_glMultiTexImage3DEXT(texunit, target, level, internalformat, width, height, depth, border, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, uint, uint, void*, void> pfn_glMultiTexSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexSubImage3DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels) => pfn_glMultiTexSubImage3DEXT(texunit, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, int, void> pfn_glCopyMultiTexSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCopyMultiTexSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyMultiTexSubImage3DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => pfn_glCopyMultiTexSubImage3DEXT(texunit, target, level, xoffset, yoffset, zoffset, x, y, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableClientStateIndexedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEnableClientStateIndexedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEnableClientStateIndexedEXT(uint array, uint index) => pfn_glEnableClientStateIndexedEXT(array, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableClientStateIndexedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDisableClientStateIndexedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDisableClientStateIndexedEXT(uint array, uint index) => pfn_glDisableClientStateIndexedEXT(array, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetFloatIndexedvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetFloatIndexedvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetFloatIndexedvEXT(uint target, uint index, float* data) => pfn_glGetFloatIndexedvEXT(target, index, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, double*, void> pfn_glGetDoubleIndexedvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetDoubleIndexedvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetDoubleIndexedvEXT(uint target, uint index, double* data) => pfn_glGetDoubleIndexedvEXT(target, index, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetPointerIndexedvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetPointerIndexedvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPointerIndexedvEXT(uint target, uint index, void** data) => pfn_glGetPointerIndexedvEXT(target, index, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableIndexedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEnableIndexedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEnableIndexedEXT(uint target, uint index) => pfn_glEnableIndexedEXT(target, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableIndexedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDisableIndexedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDisableIndexedEXT(uint target, uint index) => pfn_glDisableIndexedEXT(target, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, byte> pfn_glIsEnabledIndexedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glIsEnabledIndexedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsEnabledIndexedEXT(uint target, uint index) => pfn_glIsEnabledIndexedEXT(target, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetIntegerIndexedvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetIntegerIndexedvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetIntegerIndexedvEXT(uint target, uint index, int* data) => pfn_glGetIntegerIndexedvEXT(target, index, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, byte*, void> pfn_glGetBooleanIndexedvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetBooleanIndexedvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetBooleanIndexedvEXT(uint target, uint index, byte* data) => pfn_glGetBooleanIndexedvEXT(target, index, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, int, void*, void> pfn_glCompressedTextureImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureImage3DEXT(uint texture, uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, void* bits) => pfn_glCompressedTextureImage3DEXT(texture, target, level, internalformat, width, height, depth, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, void*, void> pfn_glCompressedTextureImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureImage2DEXT(uint texture, uint target, int level, uint internalformat, int width, int height, int border, int imageSize, void* bits) => pfn_glCompressedTextureImage2DEXT(texture, target, level, internalformat, width, height, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, void*, void> pfn_glCompressedTextureImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureImage1DEXT(uint texture, uint target, int level, uint internalformat, int width, int border, int imageSize, void* bits) => pfn_glCompressedTextureImage1DEXT(texture, target, level, internalformat, width, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureSubImage3DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* bits) => pfn_glCompressedTextureSubImage3DEXT(texture, target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureSubImage2DEXT(uint texture, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* bits) => pfn_glCompressedTextureSubImage2DEXT(texture, target, level, xoffset, yoffset, width, height, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, uint, int, void*, void> pfn_glCompressedTextureSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedTextureSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedTextureSubImage1DEXT(uint texture, uint target, int level, int xoffset, int width, uint format, int imageSize, void* bits) => pfn_glCompressedTextureSubImage1DEXT(texture, target, level, xoffset, width, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void*, void> pfn_glGetCompressedTextureImageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetCompressedTextureImageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetCompressedTextureImageEXT(uint texture, uint target, int lod, void* img) => pfn_glGetCompressedTextureImageEXT(texture, target, lod, img);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, int, void*, void> pfn_glCompressedMultiTexImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexImage3DEXT(uint texunit, uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, void* bits) => pfn_glCompressedMultiTexImage3DEXT(texunit, target, level, internalformat, width, height, depth, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, int, void*, void> pfn_glCompressedMultiTexImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexImage2DEXT(uint texunit, uint target, int level, uint internalformat, int width, int height, int border, int imageSize, void* bits) => pfn_glCompressedMultiTexImage2DEXT(texunit, target, level, internalformat, width, height, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, void*, void> pfn_glCompressedMultiTexImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexImage1DEXT(uint texunit, uint target, int level, uint internalformat, int width, int border, int imageSize, void* bits) => pfn_glCompressedMultiTexImage1DEXT(texunit, target, level, internalformat, width, border, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedMultiTexSubImage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexSubImage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexSubImage3DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* bits) => pfn_glCompressedMultiTexSubImage3DEXT(texunit, target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, int, uint, int, void*, void> pfn_glCompressedMultiTexSubImage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexSubImage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexSubImage2DEXT(uint texunit, uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* bits) => pfn_glCompressedMultiTexSubImage2DEXT(texunit, target, level, xoffset, yoffset, width, height, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, int, uint, int, void*, void> pfn_glCompressedMultiTexSubImage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCompressedMultiTexSubImage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompressedMultiTexSubImage1DEXT(uint texunit, uint target, int level, int xoffset, int width, uint format, int imageSize, void* bits) => pfn_glCompressedMultiTexSubImage1DEXT(texunit, target, level, xoffset, width, format, imageSize, bits);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void*, void> pfn_glGetCompressedMultiTexImageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetCompressedMultiTexImageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetCompressedMultiTexImageEXT(uint texunit, uint target, int lod, void* img) => pfn_glGetCompressedMultiTexImageEXT(texunit, target, lod, img);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixLoadTransposefEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoadTransposefEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoadTransposefEXT(uint mode, float* m) => pfn_glMatrixLoadTransposefEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glMatrixLoadTransposedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoadTransposedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoadTransposedEXT(uint mode, double* m) => pfn_glMatrixLoadTransposedEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixMultTransposefEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMultTransposefEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMultTransposefEXT(uint mode, float* m) => pfn_glMatrixMultTransposefEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, double*, void> pfn_glMatrixMultTransposedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMultTransposedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMultTransposedEXT(uint mode, double* m) => pfn_glMatrixMultTransposedEXT(mode, m);

        private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glNamedBufferDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferDataEXT(uint buffer, long size, void* data, uint usage) => pfn_glNamedBufferDataEXT(buffer, size, data, usage);

        private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glNamedBufferSubDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferSubDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferSubDataEXT(uint buffer, long offset, long size, void* data) => pfn_glNamedBufferSubDataEXT(buffer, offset, size, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*> pfn_glMapNamedBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMapNamedBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void* glMapNamedBufferEXT(uint buffer, uint access) => pfn_glMapNamedBufferEXT(buffer, access);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glUnmapNamedBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glUnmapNamedBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glUnmapNamedBufferEXT(uint buffer) => pfn_glUnmapNamedBufferEXT(buffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedBufferParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedBufferParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedBufferParameterivEXT(uint buffer, uint pname, int* @params) => pfn_glGetNamedBufferParameterivEXT(buffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetNamedBufferPointervEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedBufferPointervEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedBufferPointervEXT(uint buffer, uint pname, void** @params) => pfn_glGetNamedBufferPointervEXT(buffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, long, long, void*, void> pfn_glGetNamedBufferSubDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedBufferSubDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedBufferSubDataEXT(uint buffer, long offset, long size, void* data) => pfn_glGetNamedBufferSubDataEXT(buffer, offset, size, data);

        private static delegate* unmanaged[Stdcall]<uint, int, float, void> pfn_glProgramUniform1fEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1fEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1fEXT(uint program, int location, float v0) => pfn_glProgramUniform1fEXT(program, location, v0);

        private static delegate* unmanaged[Stdcall]<uint, int, float, float, void> pfn_glProgramUniform2fEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2fEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2fEXT(uint program, int location, float v0, float v1) => pfn_glProgramUniform2fEXT(program, location, v0, v1);

        private static delegate* unmanaged[Stdcall]<uint, int, float, float, float, void> pfn_glProgramUniform3fEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3fEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3fEXT(uint program, int location, float v0, float v1, float v2) => pfn_glProgramUniform3fEXT(program, location, v0, v1, v2);

        private static delegate* unmanaged[Stdcall]<uint, int, float, float, float, float, void> pfn_glProgramUniform4fEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4fEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4fEXT(uint program, int location, float v0, float v1, float v2, float v3) => pfn_glProgramUniform4fEXT(program, location, v0, v1, v2, v3);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void> pfn_glProgramUniform1iEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1iEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1iEXT(uint program, int location, int v0) => pfn_glProgramUniform1iEXT(program, location, v0);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glProgramUniform2iEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2iEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2iEXT(uint program, int location, int v0, int v1) => pfn_glProgramUniform2iEXT(program, location, v0, v1);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, void> pfn_glProgramUniform3iEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3iEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3iEXT(uint program, int location, int v0, int v1, int v2) => pfn_glProgramUniform3iEXT(program, location, v0, v1, v2);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, void> pfn_glProgramUniform4iEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4iEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4iEXT(uint program, int location, int v0, int v1, int v2, int v3) => pfn_glProgramUniform4iEXT(program, location, v0, v1, v2, v3);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform1fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1fvEXT(uint program, int location, int count, float* value) => pfn_glProgramUniform1fvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform2fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2fvEXT(uint program, int location, int count, float* value) => pfn_glProgramUniform2fvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform3fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3fvEXT(uint program, int location, int count, float* value) => pfn_glProgramUniform3fvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float*, void> pfn_glProgramUniform4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4fvEXT(uint program, int location, int count, float* value) => pfn_glProgramUniform4fvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform1ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1ivEXT(uint program, int location, int count, int* value) => pfn_glProgramUniform1ivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform2ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2ivEXT(uint program, int location, int count, int* value) => pfn_glProgramUniform2ivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform3ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3ivEXT(uint program, int location, int count, int* value) => pfn_glProgramUniform3ivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int*, void> pfn_glProgramUniform4ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4ivEXT(uint program, int location, int count, int* value) => pfn_glProgramUniform4ivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2x3fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x3fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2x3fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2x3fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3x2fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x2fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3x2fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3x2fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix2x4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2x4fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix2x4fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4x2fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x2fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4x2fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4x2fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix3x4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3x4fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix3x4fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void> pfn_glProgramUniformMatrix4x3fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x3fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4x3fvEXT(uint program, int location, int count, byte transpose, float* value) => pfn_glProgramUniformMatrix4x3fvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glTextureBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureBufferEXT(uint texture, uint target, uint internalformat, uint buffer) => pfn_glTextureBufferEXT(texture, target, internalformat, buffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glMultiTexBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexBufferEXT(uint texunit, uint target, uint internalformat, uint buffer) => pfn_glMultiTexBufferEXT(texunit, target, internalformat, buffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glTextureParameterIivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameterIivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameterIivEXT(uint texture, uint target, uint pname, int* @params) => pfn_glTextureParameterIivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glTextureParameterIuivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureParameterIuivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureParameterIuivEXT(uint texture, uint target, uint pname, uint* @params) => pfn_glTextureParameterIuivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetTextureParameterIivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureParameterIivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureParameterIivEXT(uint texture, uint target, uint pname, int* @params) => pfn_glGetTextureParameterIivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glGetTextureParameterIuivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureParameterIuivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetTextureParameterIuivEXT(uint texture, uint target, uint pname, uint* @params) => pfn_glGetTextureParameterIuivEXT(texture, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glMultiTexParameterIivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameterIivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameterIivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glMultiTexParameterIivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glMultiTexParameterIuivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexParameterIuivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexParameterIuivEXT(uint texunit, uint target, uint pname, uint* @params) => pfn_glMultiTexParameterIuivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetMultiTexParameterIivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexParameterIivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexParameterIivEXT(uint texunit, uint target, uint pname, int* @params) => pfn_glGetMultiTexParameterIivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glGetMultiTexParameterIuivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetMultiTexParameterIuivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMultiTexParameterIuivEXT(uint texunit, uint target, uint pname, uint* @params) => pfn_glGetMultiTexParameterIuivEXT(texunit, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void> pfn_glProgramUniform1uiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1uiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1uiEXT(uint program, int location, uint v0) => pfn_glProgramUniform1uiEXT(program, location, v0);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void> pfn_glProgramUniform2uiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2uiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2uiEXT(uint program, int location, uint v0, uint v1) => pfn_glProgramUniform2uiEXT(program, location, v0, v1);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, void> pfn_glProgramUniform3uiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3uiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3uiEXT(uint program, int location, uint v0, uint v1, uint v2) => pfn_glProgramUniform3uiEXT(program, location, v0, v1, v2);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, uint, void> pfn_glProgramUniform4uiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4uiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4uiEXT(uint program, int location, uint v0, uint v1, uint v2, uint v3) => pfn_glProgramUniform4uiEXT(program, location, v0, v1, v2, v3);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform1uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1uivEXT(uint program, int location, int count, uint* value) => pfn_glProgramUniform1uivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform2uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2uivEXT(uint program, int location, int count, uint* value) => pfn_glProgramUniform2uivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform3uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3uivEXT(uint program, int location, int count, uint* value) => pfn_glProgramUniform3uivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint*, void> pfn_glProgramUniform4uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4uivEXT(uint program, int location, int count, uint* value) => pfn_glProgramUniform4uivEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, float*, void> pfn_glNamedProgramLocalParameters4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameters4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameters4fvEXT(uint program, uint target, uint index, int count, float* @params) => pfn_glNamedProgramLocalParameters4fvEXT(program, target, index, count, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, int, int, void> pfn_glNamedProgramLocalParameterI4iEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameterI4iEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameterI4iEXT(uint program, uint target, uint index, int x, int y, int z, int w) => pfn_glNamedProgramLocalParameterI4iEXT(program, target, index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glNamedProgramLocalParameterI4ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameterI4ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameterI4ivEXT(uint program, uint target, uint index, int* @params) => pfn_glNamedProgramLocalParameterI4ivEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, void> pfn_glNamedProgramLocalParametersI4ivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParametersI4ivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParametersI4ivEXT(uint program, uint target, uint index, int count, int* @params) => pfn_glNamedProgramLocalParametersI4ivEXT(program, target, index, count, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, uint, uint, void> pfn_glNamedProgramLocalParameterI4uiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameterI4uiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameterI4uiEXT(uint program, uint target, uint index, uint x, uint y, uint z, uint w) => pfn_glNamedProgramLocalParameterI4uiEXT(program, target, index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glNamedProgramLocalParameterI4uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameterI4uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameterI4uivEXT(uint program, uint target, uint index, uint* @params) => pfn_glNamedProgramLocalParameterI4uivEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, void> pfn_glNamedProgramLocalParametersI4uivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParametersI4uivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParametersI4uivEXT(uint program, uint target, uint index, int count, uint* @params) => pfn_glNamedProgramLocalParametersI4uivEXT(program, target, index, count, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetNamedProgramLocalParameterIivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramLocalParameterIivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramLocalParameterIivEXT(uint program, uint target, uint index, int* @params) => pfn_glGetNamedProgramLocalParameterIivEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint*, void> pfn_glGetNamedProgramLocalParameterIuivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramLocalParameterIuivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramLocalParameterIuivEXT(uint program, uint target, uint index, uint* @params) => pfn_glGetNamedProgramLocalParameterIuivEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableClientStateiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEnableClientStateiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEnableClientStateiEXT(uint array, uint index) => pfn_glEnableClientStateiEXT(array, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableClientStateiEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDisableClientStateiEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDisableClientStateiEXT(uint array, uint index) => pfn_glDisableClientStateiEXT(array, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetFloati_vEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetFloati_vEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetFloati_vEXT(uint pname, uint index, float* @params) => pfn_glGetFloati_vEXT(pname, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, double*, void> pfn_glGetDoublei_vEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetDoublei_vEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetDoublei_vEXT(uint pname, uint index, double* @params) => pfn_glGetDoublei_vEXT(pname, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetPointeri_vEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetPointeri_vEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPointeri_vEXT(uint pname, uint index, void** @params) => pfn_glGetPointeri_vEXT(pname, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void*, void> pfn_glNamedProgramStringEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramStringEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramStringEXT(uint program, uint target, uint format, int len, void* @string) => pfn_glNamedProgramStringEXT(program, target, format, len, @string);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double, double, double, double, void> pfn_glNamedProgramLocalParameter4dEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameter4dEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameter4dEXT(uint program, uint target, uint index, double x, double y, double z, double w) => pfn_glNamedProgramLocalParameter4dEXT(program, target, index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double*, void> pfn_glNamedProgramLocalParameter4dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameter4dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameter4dvEXT(uint program, uint target, uint index, double* @params) => pfn_glNamedProgramLocalParameter4dvEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, float, float, float, void> pfn_glNamedProgramLocalParameter4fEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameter4fEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameter4fEXT(uint program, uint target, uint index, float x, float y, float z, float w) => pfn_glNamedProgramLocalParameter4fEXT(program, target, index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glNamedProgramLocalParameter4fvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedProgramLocalParameter4fvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedProgramLocalParameter4fvEXT(uint program, uint target, uint index, float* @params) => pfn_glNamedProgramLocalParameter4fvEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, double*, void> pfn_glGetNamedProgramLocalParameterdvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramLocalParameterdvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramLocalParameterdvEXT(uint program, uint target, uint index, double* @params) => pfn_glGetNamedProgramLocalParameterdvEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glGetNamedProgramLocalParameterfvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramLocalParameterfvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramLocalParameterfvEXT(uint program, uint target, uint index, float* @params) => pfn_glGetNamedProgramLocalParameterfvEXT(program, target, index, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetNamedProgramivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramivEXT(uint program, uint target, uint pname, int* @params) => pfn_glGetNamedProgramivEXT(program, target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void*, void> pfn_glGetNamedProgramStringEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedProgramStringEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedProgramStringEXT(uint program, uint target, uint pname, void* @string) => pfn_glGetNamedProgramStringEXT(program, target, pname, @string);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, void> pfn_glNamedRenderbufferStorageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedRenderbufferStorageEXT(uint renderbuffer, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorageEXT(renderbuffer, internalformat, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedRenderbufferParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedRenderbufferParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedRenderbufferParameterivEXT(uint renderbuffer, uint pname, int* @params) => pfn_glGetNamedRenderbufferParameterivEXT(renderbuffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void> pfn_glNamedRenderbufferStorageMultisampleEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorageMultisampleEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedRenderbufferStorageMultisampleEXT(uint renderbuffer, int samples, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorageMultisampleEXT(renderbuffer, samples, internalformat, width, height);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint, int, int, void> pfn_glNamedRenderbufferStorageMultisampleCoverageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedRenderbufferStorageMultisampleCoverageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedRenderbufferStorageMultisampleCoverageEXT(uint renderbuffer, int coverageSamples, int colorSamples, uint internalformat, int width, int height) => pfn_glNamedRenderbufferStorageMultisampleCoverageEXT(renderbuffer, coverageSamples, colorSamples, internalformat, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint> pfn_glCheckNamedFramebufferStatusEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCheckNamedFramebufferStatusEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glCheckNamedFramebufferStatusEXT(uint framebuffer, uint target) => pfn_glCheckNamedFramebufferStatusEXT(framebuffer, target);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, void> pfn_glNamedFramebufferTexture1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTexture1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTexture1DEXT(uint framebuffer, uint attachment, uint textarget, uint texture, int level) => pfn_glNamedFramebufferTexture1DEXT(framebuffer, attachment, textarget, texture, level);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, void> pfn_glNamedFramebufferTexture2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTexture2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTexture2DEXT(uint framebuffer, uint attachment, uint textarget, uint texture, int level) => pfn_glNamedFramebufferTexture2DEXT(framebuffer, attachment, textarget, texture, level);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, int, void> pfn_glNamedFramebufferTexture3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTexture3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTexture3DEXT(uint framebuffer, uint attachment, uint textarget, uint texture, int level, int zoffset) => pfn_glNamedFramebufferTexture3DEXT(framebuffer, attachment, textarget, texture, level, zoffset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glNamedFramebufferRenderbufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferRenderbufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferRenderbufferEXT(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer) => pfn_glNamedFramebufferRenderbufferEXT(framebuffer, attachment, renderbuffertarget, renderbuffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetNamedFramebufferAttachmentParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedFramebufferAttachmentParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedFramebufferAttachmentParameterivEXT(uint framebuffer, uint attachment, uint pname, int* @params) => pfn_glGetNamedFramebufferAttachmentParameterivEXT(framebuffer, attachment, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glGenerateTextureMipmapEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGenerateTextureMipmapEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGenerateTextureMipmapEXT(uint texture, uint target) => pfn_glGenerateTextureMipmapEXT(texture, target);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glGenerateMultiTexMipmapEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGenerateMultiTexMipmapEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGenerateMultiTexMipmapEXT(uint texunit, uint target) => pfn_glGenerateMultiTexMipmapEXT(texunit, target);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glFramebufferDrawBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferDrawBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferDrawBufferEXT(uint framebuffer, uint mode) => pfn_glFramebufferDrawBufferEXT(framebuffer, mode);

        private static delegate* unmanaged[Stdcall]<uint, int, uint*, void> pfn_glFramebufferDrawBuffersEXT = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferDrawBuffersEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferDrawBuffersEXT(uint framebuffer, int n, uint* bufs) => pfn_glFramebufferDrawBuffersEXT(framebuffer, n, bufs);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glFramebufferReadBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferReadBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferReadBufferEXT(uint framebuffer, uint mode) => pfn_glFramebufferReadBufferEXT(framebuffer, mode);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetFramebufferParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetFramebufferParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetFramebufferParameterivEXT(uint framebuffer, uint pname, int* @params) => pfn_glGetFramebufferParameterivEXT(framebuffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, long, long, long, void> pfn_glNamedCopyBufferSubDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedCopyBufferSubDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedCopyBufferSubDataEXT(uint readBuffer, uint writeBuffer, long readOffset, long writeOffset, long size) => pfn_glNamedCopyBufferSubDataEXT(readBuffer, writeBuffer, readOffset, writeOffset, size);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, void> pfn_glNamedFramebufferTextureEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTextureEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTextureEXT(uint framebuffer, uint attachment, uint texture, int level) => pfn_glNamedFramebufferTextureEXT(framebuffer, attachment, texture, level);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void> pfn_glNamedFramebufferTextureLayerEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTextureLayerEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTextureLayerEXT(uint framebuffer, uint attachment, uint texture, int level, int layer) => pfn_glNamedFramebufferTextureLayerEXT(framebuffer, attachment, texture, level, layer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void> pfn_glNamedFramebufferTextureFaceEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferTextureFaceEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferTextureFaceEXT(uint framebuffer, uint attachment, uint texture, int level, uint face) => pfn_glNamedFramebufferTextureFaceEXT(framebuffer, attachment, texture, level, face);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glTextureRenderbufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureRenderbufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureRenderbufferEXT(uint texture, uint target, uint renderbuffer) => pfn_glTextureRenderbufferEXT(texture, target, renderbuffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glMultiTexRenderbufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMultiTexRenderbufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiTexRenderbufferEXT(uint texunit, uint target, uint renderbuffer) => pfn_glMultiTexRenderbufferEXT(texunit, target, renderbuffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, long, void> pfn_glVertexArrayVertexOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexOffsetEXT(uint vaobj, uint buffer, int size, uint type, int stride, long offset) => pfn_glVertexArrayVertexOffsetEXT(vaobj, buffer, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, long, void> pfn_glVertexArrayColorOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayColorOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayColorOffsetEXT(uint vaobj, uint buffer, int size, uint type, int stride, long offset) => pfn_glVertexArrayColorOffsetEXT(vaobj, buffer, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, long, void> pfn_glVertexArrayEdgeFlagOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayEdgeFlagOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayEdgeFlagOffsetEXT(uint vaobj, uint buffer, int stride, long offset) => pfn_glVertexArrayEdgeFlagOffsetEXT(vaobj, buffer, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, long, void> pfn_glVertexArrayIndexOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayIndexOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayIndexOffsetEXT(uint vaobj, uint buffer, uint type, int stride, long offset) => pfn_glVertexArrayIndexOffsetEXT(vaobj, buffer, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, long, void> pfn_glVertexArrayNormalOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayNormalOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayNormalOffsetEXT(uint vaobj, uint buffer, uint type, int stride, long offset) => pfn_glVertexArrayNormalOffsetEXT(vaobj, buffer, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, long, void> pfn_glVertexArrayTexCoordOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayTexCoordOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayTexCoordOffsetEXT(uint vaobj, uint buffer, int size, uint type, int stride, long offset) => pfn_glVertexArrayTexCoordOffsetEXT(vaobj, buffer, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, int, long, void> pfn_glVertexArrayMultiTexCoordOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayMultiTexCoordOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayMultiTexCoordOffsetEXT(uint vaobj, uint buffer, uint texunit, int size, uint type, int stride, long offset) => pfn_glVertexArrayMultiTexCoordOffsetEXT(vaobj, buffer, texunit, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, long, void> pfn_glVertexArrayFogCoordOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayFogCoordOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayFogCoordOffsetEXT(uint vaobj, uint buffer, uint type, int stride, long offset) => pfn_glVertexArrayFogCoordOffsetEXT(vaobj, buffer, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, long, void> pfn_glVertexArraySecondaryColorOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArraySecondaryColorOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArraySecondaryColorOffsetEXT(uint vaobj, uint buffer, int size, uint type, int stride, long offset) => pfn_glVertexArraySecondaryColorOffsetEXT(vaobj, buffer, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, byte, int, long, void> pfn_glVertexArrayVertexAttribOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribOffsetEXT(uint vaobj, uint buffer, uint index, int size, uint type, byte normalized, int stride, long offset) => pfn_glVertexArrayVertexAttribOffsetEXT(vaobj, buffer, index, size, type, normalized, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, int, long, void> pfn_glVertexArrayVertexAttribIOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribIOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribIOffsetEXT(uint vaobj, uint buffer, uint index, int size, uint type, int stride, long offset) => pfn_glVertexArrayVertexAttribIOffsetEXT(vaobj, buffer, index, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableVertexArrayEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEnableVertexArrayEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEnableVertexArrayEXT(uint vaobj, uint array) => pfn_glEnableVertexArrayEXT(vaobj, array);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableVertexArrayEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDisableVertexArrayEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDisableVertexArrayEXT(uint vaobj, uint array) => pfn_glDisableVertexArrayEXT(vaobj, array);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glEnableVertexArrayAttribEXT = null;
        /// <summary> <see href="docs.gl/gl4/glEnableVertexArrayAttribEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEnableVertexArrayAttribEXT(uint vaobj, uint index) => pfn_glEnableVertexArrayAttribEXT(vaobj, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDisableVertexArrayAttribEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDisableVertexArrayAttribEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDisableVertexArrayAttribEXT(uint vaobj, uint index) => pfn_glDisableVertexArrayAttribEXT(vaobj, index);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetVertexArrayIntegervEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexArrayIntegervEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexArrayIntegervEXT(uint vaobj, uint pname, int* param) => pfn_glGetVertexArrayIntegervEXT(vaobj, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetVertexArrayPointervEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexArrayPointervEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexArrayPointervEXT(uint vaobj, uint pname, void** param) => pfn_glGetVertexArrayPointervEXT(vaobj, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetVertexArrayIntegeri_vEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexArrayIntegeri_vEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexArrayIntegeri_vEXT(uint vaobj, uint index, uint pname, int* param) => pfn_glGetVertexArrayIntegeri_vEXT(vaobj, index, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void**, void> pfn_glGetVertexArrayPointeri_vEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexArrayPointeri_vEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexArrayPointeri_vEXT(uint vaobj, uint index, uint pname, void** param) => pfn_glGetVertexArrayPointeri_vEXT(vaobj, index, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, long, long, uint, void*> pfn_glMapNamedBufferRangeEXT = null;
        /// <summary> <see href="docs.gl/gl4/glMapNamedBufferRangeEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void* glMapNamedBufferRangeEXT(uint buffer, long offset, long length, uint access) => pfn_glMapNamedBufferRangeEXT(buffer, offset, length, access);

        private static delegate* unmanaged[Stdcall]<uint, long, long, void> pfn_glFlushMappedNamedBufferRangeEXT = null;
        /// <summary> <see href="docs.gl/gl4/glFlushMappedNamedBufferRangeEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFlushMappedNamedBufferRangeEXT(uint buffer, long offset, long length) => pfn_glFlushMappedNamedBufferRangeEXT(buffer, offset, length);

        private static delegate* unmanaged[Stdcall]<uint, long, void*, uint, void> pfn_glNamedBufferStorageEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferStorageEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferStorageEXT(uint buffer, long size, void* data, uint flags) => pfn_glNamedBufferStorageEXT(buffer, size, data, flags);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void*, void> pfn_glClearNamedBufferDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glClearNamedBufferDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glClearNamedBufferDataEXT(uint buffer, uint internalformat, uint format, uint type, void* data) => pfn_glClearNamedBufferDataEXT(buffer, internalformat, format, type, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, long, long, uint, uint, void*, void> pfn_glClearNamedBufferSubDataEXT = null;
        /// <summary> <see href="docs.gl/gl4/glClearNamedBufferSubDataEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glClearNamedBufferSubDataEXT(uint buffer, uint internalformat, long offset, long size, uint format, uint type, void* data) => pfn_glClearNamedBufferSubDataEXT(buffer, internalformat, offset, size, format, type, data);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glNamedFramebufferParameteriEXT = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferParameteriEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferParameteriEXT(uint framebuffer, uint pname, int param) => pfn_glNamedFramebufferParameteriEXT(framebuffer, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetNamedFramebufferParameterivEXT = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedFramebufferParameterivEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedFramebufferParameterivEXT(uint framebuffer, uint pname, int* @params) => pfn_glGetNamedFramebufferParameterivEXT(framebuffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, double, void> pfn_glProgramUniform1dEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1dEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1dEXT(uint program, int location, double x) => pfn_glProgramUniform1dEXT(program, location, x);

        private static delegate* unmanaged[Stdcall]<uint, int, double, double, void> pfn_glProgramUniform2dEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2dEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2dEXT(uint program, int location, double x, double y) => pfn_glProgramUniform2dEXT(program, location, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, double, double, double, void> pfn_glProgramUniform3dEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3dEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3dEXT(uint program, int location, double x, double y, double z) => pfn_glProgramUniform3dEXT(program, location, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, int, double, double, double, double, void> pfn_glProgramUniform4dEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4dEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4dEXT(uint program, int location, double x, double y, double z, double w) => pfn_glProgramUniform4dEXT(program, location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform1dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1dvEXT(uint program, int location, int count, double* value) => pfn_glProgramUniform1dvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform2dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2dvEXT(uint program, int location, int count, double* value) => pfn_glProgramUniform2dvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform3dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3dvEXT(uint program, int location, int count, double* value) => pfn_glProgramUniform3dvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, double*, void> pfn_glProgramUniform4dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4dvEXT(uint program, int location, int count, double* value) => pfn_glProgramUniform4dvEXT(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2x3dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x3dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2x3dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2x3dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix2x4dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix2x4dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix2x4dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix2x4dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3x2dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x2dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3x2dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3x2dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix3x4dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix3x4dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix3x4dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix3x4dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4x2dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x2dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4x2dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4x2dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, byte, double*, void> pfn_glProgramUniformMatrix4x3dvEXT = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformMatrix4x3dvEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformMatrix4x3dvEXT(uint program, int location, int count, byte transpose, double* value) => pfn_glProgramUniformMatrix4x3dvEXT(program, location, count, transpose, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, long, long, void> pfn_glTextureBufferRangeEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureBufferRangeEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureBufferRangeEXT(uint texture, uint target, uint internalformat, uint buffer, long offset, long size) => pfn_glTextureBufferRangeEXT(texture, target, internalformat, buffer, offset, size);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, void> pfn_glTextureStorage1DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureStorage1DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureStorage1DEXT(uint texture, uint target, int levels, uint internalformat, int width) => pfn_glTextureStorage1DEXT(texture, target, levels, internalformat, width);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, void> pfn_glTextureStorage2DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureStorage2DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureStorage2DEXT(uint texture, uint target, int levels, uint internalformat, int width, int height) => pfn_glTextureStorage2DEXT(texture, target, levels, internalformat, width, height);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, void> pfn_glTextureStorage3DEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureStorage3DEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureStorage3DEXT(uint texture, uint target, int levels, uint internalformat, int width, int height, int depth) => pfn_glTextureStorage3DEXT(texture, target, levels, internalformat, width, height, depth);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, byte, void> pfn_glTextureStorage2DMultisampleEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureStorage2DMultisampleEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureStorage2DMultisampleEXT(uint texture, uint target, int samples, uint internalformat, int width, int height, byte fixedsamplelocations) => pfn_glTextureStorage2DMultisampleEXT(texture, target, samples, internalformat, width, height, fixedsamplelocations);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int, int, byte, void> pfn_glTextureStorage3DMultisampleEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTextureStorage3DMultisampleEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureStorage3DMultisampleEXT(uint texture, uint target, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations) => pfn_glTextureStorage3DMultisampleEXT(texture, target, samples, internalformat, width, height, depth, fixedsamplelocations);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, long, int, void> pfn_glVertexArrayBindVertexBufferEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayBindVertexBufferEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayBindVertexBufferEXT(uint vaobj, uint bindingindex, uint buffer, long offset, int stride) => pfn_glVertexArrayBindVertexBufferEXT(vaobj, bindingindex, buffer, offset, stride);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, byte, uint, void> pfn_glVertexArrayVertexAttribFormatEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribFormatEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribFormatEXT(uint vaobj, uint attribindex, int size, uint type, byte normalized, uint relativeoffset) => pfn_glVertexArrayVertexAttribFormatEXT(vaobj, attribindex, size, type, normalized, relativeoffset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void> pfn_glVertexArrayVertexAttribIFormatEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribIFormatEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribIFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexArrayVertexAttribIFormatEXT(vaobj, attribindex, size, type, relativeoffset);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, uint, void> pfn_glVertexArrayVertexAttribLFormatEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribLFormatEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribLFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset) => pfn_glVertexArrayVertexAttribLFormatEXT(vaobj, attribindex, size, type, relativeoffset);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexArrayVertexAttribBindingEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribBindingEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribBindingEXT(uint vaobj, uint attribindex, uint bindingindex) => pfn_glVertexArrayVertexAttribBindingEXT(vaobj, attribindex, bindingindex);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexArrayVertexBindingDivisorEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexBindingDivisorEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexBindingDivisorEXT(uint vaobj, uint bindingindex, uint divisor) => pfn_glVertexArrayVertexBindingDivisorEXT(vaobj, bindingindex, divisor);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, int, long, void> pfn_glVertexArrayVertexAttribLOffsetEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribLOffsetEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribLOffsetEXT(uint vaobj, uint buffer, uint index, int size, uint type, int stride, long offset) => pfn_glVertexArrayVertexAttribLOffsetEXT(vaobj, buffer, index, size, type, stride, offset);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, byte, void> pfn_glTexturePageCommitmentEXT = null;
        /// <summary> <see href="docs.gl/gl4/glTexturePageCommitmentEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexturePageCommitmentEXT(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, byte commit) => pfn_glTexturePageCommitmentEXT(texture, level, xoffset, yoffset, zoffset, width, height, depth, commit);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glVertexArrayVertexAttribDivisorEXT = null;
        /// <summary> <see href="docs.gl/gl4/glVertexArrayVertexAttribDivisorEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexArrayVertexAttribDivisorEXT(uint vaobj, uint index, uint divisor) => pfn_glVertexArrayVertexAttribDivisorEXT(vaobj, index, divisor);
    }

    public static unsafe class GLEXTDrawInstanced
    {
        static GLEXTDrawInstanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_draw_instanced") ?? false) _GL_EXT_draw_instanced = 1;
        }
        private static uint _GL_EXT_draw_instanced = 0;
        public static uint GL_EXT_draw_instanced => _GL_EXT_draw_instanced;

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, void> pfn_glDrawArraysInstancedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDrawArraysInstancedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawArraysInstancedEXT(uint mode, int start, int count, int primcount) => pfn_glDrawArraysInstancedEXT(mode, start, count, primcount);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void> pfn_glDrawElementsInstancedEXT = null;
        /// <summary> <see href="docs.gl/gl4/glDrawElementsInstancedEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawElementsInstancedEXT(uint mode, int count, uint type, void* indices, int primcount) => pfn_glDrawElementsInstancedEXT(mode, count, type, indices, primcount);
    }

    public static unsafe class GLEXTMultiviewTessellationGeometryShader
    {
        static GLEXTMultiviewTessellationGeometryShader() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_multiview_tessellation_geometry_shader") ?? false) _GL_EXT_multiview_tessellation_geometry_shader = 1;
        }
        private static uint _GL_EXT_multiview_tessellation_geometry_shader = 0;
        public static uint GL_EXT_multiview_tessellation_geometry_shader => _GL_EXT_multiview_tessellation_geometry_shader;
    }

    public static unsafe class GLEXTMultiviewTextureMultisample
    {
        static GLEXTMultiviewTextureMultisample() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_multiview_texture_multisample") ?? false) _GL_EXT_multiview_texture_multisample = 1;
        }
        private static uint _GL_EXT_multiview_texture_multisample = 0;
        public static uint GL_EXT_multiview_texture_multisample => _GL_EXT_multiview_texture_multisample;
    }

    public static unsafe class GLEXTMultiviewTimerQuery
    {
        static GLEXTMultiviewTimerQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_multiview_timer_query") ?? false) _GL_EXT_multiview_timer_query = 1;
        }
        private static uint _GL_EXT_multiview_timer_query = 0;
        public static uint GL_EXT_multiview_timer_query => _GL_EXT_multiview_timer_query;
    }

    public static unsafe class GLEXTPolygonOffsetClamp
    {
        static GLEXTPolygonOffsetClamp() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_polygon_offset_clamp") ?? false) _GL_EXT_polygon_offset_clamp = 1;
        }
        private static uint _GL_EXT_polygon_offset_clamp = 0;
        public static uint GL_EXT_polygon_offset_clamp => _GL_EXT_polygon_offset_clamp;
        public const uint GL_POLYGON_OFFSET_CLAMP_EXT = 0x8E1B;

        private static delegate* unmanaged[Stdcall]<float, float, float, void> pfn_glPolygonOffsetClampEXT = null;
        /// <summary> <see href="docs.gl/gl4/glPolygonOffsetClampEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPolygonOffsetClampEXT(float factor, float units, float clamp) => pfn_glPolygonOffsetClampEXT(factor, units, clamp);
    }

    public static unsafe class GLEXTPostDepthCoverage
    {
        static GLEXTPostDepthCoverage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_post_depth_coverage") ?? false) _GL_EXT_post_depth_coverage = 1;
        }
        private static uint _GL_EXT_post_depth_coverage = 0;
        public static uint GL_EXT_post_depth_coverage => _GL_EXT_post_depth_coverage;
    }

    public static unsafe class GLEXTRasterMultisample
    {
        static GLEXTRasterMultisample() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_raster_multisample") ?? false) _GL_EXT_raster_multisample = 1;
        }
        private static uint _GL_EXT_raster_multisample = 0;
        public static uint GL_EXT_raster_multisample => _GL_EXT_raster_multisample;
        public const uint GL_RASTER_MULTISAMPLE_EXT = 0x9327;
        public const uint GL_RASTER_SAMPLES_EXT = 0x9328;
        public const uint GL_MAX_RASTER_SAMPLES_EXT = 0x9329;
        public const uint GL_RASTER_FIXED_SAMPLE_LOCATIONS_EXT = 0x932A;
        public const uint GL_MULTISAMPLE_RASTERIZATION_ALLOWED_EXT = 0x932B;
        public const uint GL_EFFECTIVE_RASTER_SAMPLES_EXT = 0x932C;

        private static delegate* unmanaged[Stdcall]<uint, byte, void> pfn_glRasterSamplesEXT = null;
        /// <summary> <see href="docs.gl/gl4/glRasterSamplesEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glRasterSamplesEXT(uint samples, byte fixedsamplelocations) => pfn_glRasterSamplesEXT(samples, fixedsamplelocations);
    }

    public static unsafe class GLEXTSeparateShaderObjects
    {
        static GLEXTSeparateShaderObjects() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_separate_shader_objects") ?? false) _GL_EXT_separate_shader_objects = 1;
        }
        private static uint _GL_EXT_separate_shader_objects = 0;
        public static uint GL_EXT_separate_shader_objects => _GL_EXT_separate_shader_objects;
        public const uint GL_ACTIVE_PROGRAM_EXT = 0x8B8D;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glUseShaderProgramEXT = null;
        /// <summary> <see href="docs.gl/gl4/glUseShaderProgramEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUseShaderProgramEXT(uint type, uint program) => pfn_glUseShaderProgramEXT(type, program);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glActiveProgramEXT = null;
        /// <summary> <see href="docs.gl/gl4/glActiveProgramEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glActiveProgramEXT(uint program) => pfn_glActiveProgramEXT(program);

        private static delegate* unmanaged[Stdcall]<uint, byte*, uint> pfn_glCreateShaderProgramEXT = null;
        /// <summary> <see href="docs.gl/gl4/glCreateShaderProgramEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glCreateShaderProgramEXT(uint type, byte* @string) => pfn_glCreateShaderProgramEXT(type, @string);
    }

    public static unsafe class GLEXTShaderFramebufferFetch
    {
        static GLEXTShaderFramebufferFetch() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_shader_framebuffer_fetch") ?? false) _GL_EXT_shader_framebuffer_fetch = 1;
        }
        private static uint _GL_EXT_shader_framebuffer_fetch = 0;
        public static uint GL_EXT_shader_framebuffer_fetch => _GL_EXT_shader_framebuffer_fetch;
        public const uint GL_FRAGMENT_SHADER_DISCARDS_SAMPLES_EXT = 0x8A52;
    }

    public static unsafe class GLEXTShaderFramebufferFetchNonCoherent
    {
        static GLEXTShaderFramebufferFetchNonCoherent() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_shader_framebuffer_fetch_non_coherent") ?? false) _GL_EXT_shader_framebuffer_fetch_non_coherent = 1;
        }
        private static uint _GL_EXT_shader_framebuffer_fetch_non_coherent = 0;
        public static uint GL_EXT_shader_framebuffer_fetch_non_coherent => _GL_EXT_shader_framebuffer_fetch_non_coherent;

        private static delegate* unmanaged[Stdcall]<void> pfn_glFramebufferFetchBarrierEXT = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferFetchBarrierEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferFetchBarrierEXT() => pfn_glFramebufferFetchBarrierEXT();
    }

    public static unsafe class GLEXTShaderIntegerMix
    {
        static GLEXTShaderIntegerMix() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_shader_integer_mix") ?? false) _GL_EXT_shader_integer_mix = 1;
        }
        private static uint _GL_EXT_shader_integer_mix = 0;
        public static uint GL_EXT_shader_integer_mix => _GL_EXT_shader_integer_mix;
    }

    public static unsafe class GLEXTTextureCompressionS3tc
    {
        static GLEXTTextureCompressionS3tc() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_compression_s3tc") ?? false) _GL_EXT_texture_compression_s3tc = 1;
        }
        private static uint _GL_EXT_texture_compression_s3tc = 0;
        public static uint GL_EXT_texture_compression_s3tc => _GL_EXT_texture_compression_s3tc;
        public const uint GL_COMPRESSED_RGB_S3TC_DXT1_EXT = 0x83F0;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT1_EXT = 0x83F1;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT3_EXT = 0x83F2;
        public const uint GL_COMPRESSED_RGBA_S3TC_DXT5_EXT = 0x83F3;
    }

    public static unsafe class GLEXTTextureFilterMinmax
    {
        static GLEXTTextureFilterMinmax() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_filter_minmax") ?? false) _GL_EXT_texture_filter_minmax = 1;
        }
        private static uint _GL_EXT_texture_filter_minmax = 0;
        public static uint GL_EXT_texture_filter_minmax => _GL_EXT_texture_filter_minmax;
        public const uint GL_TEXTURE_REDUCTION_MODE_EXT = 0x9366;
        public const uint GL_WEIGHTED_AVERAGE_EXT = 0x9367;
    }

    public static unsafe class GLEXTTextureSRGBR8
    {
        static GLEXTTextureSRGBR8() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_sRGB_R8") ?? false) _GL_EXT_texture_sRGB_R8 = 1;
        }
        private static uint _GL_EXT_texture_sRGB_R8 = 0;
        public static uint GL_EXT_texture_sRGB_R8 => _GL_EXT_texture_sRGB_R8;
        public const uint GL_SR8_EXT = 0x8FBD;
    }

    public static unsafe class GLEXTTextureSRGBRG8
    {
        static GLEXTTextureSRGBRG8() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_sRGB_RG8") ?? false) _GL_EXT_texture_sRGB_RG8 = 1;
        }
        private static uint _GL_EXT_texture_sRGB_RG8 = 0;
        public static uint GL_EXT_texture_sRGB_RG8 => _GL_EXT_texture_sRGB_RG8;
        public const uint GL_SRG8_EXT = 0x8FBE;
    }

    public static unsafe class GLEXTTextureSRGBDecode
    {
        static GLEXTTextureSRGBDecode() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_sRGB_decode") ?? false) _GL_EXT_texture_sRGB_decode = 1;
        }
        private static uint _GL_EXT_texture_sRGB_decode = 0;
        public static uint GL_EXT_texture_sRGB_decode => _GL_EXT_texture_sRGB_decode;
        public const uint GL_TEXTURE_SRGB_DECODE_EXT = 0x8A48;
        public const uint GL_DECODE_EXT = 0x8A49;
        public const uint GL_SKIP_DECODE_EXT = 0x8A4A;
    }

    public static unsafe class GLEXTTextureShadowLod
    {
        static GLEXTTextureShadowLod() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_texture_shadow_lod") ?? false) _GL_EXT_texture_shadow_lod = 1;
        }
        private static uint _GL_EXT_texture_shadow_lod = 0;
        public static uint GL_EXT_texture_shadow_lod => _GL_EXT_texture_shadow_lod;
    }

    public static unsafe class GLEXTWindowRectangles
    {
        static GLEXTWindowRectangles() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_EXT_window_rectangles") ?? false) _GL_EXT_window_rectangles = 1;
        }
        private static uint _GL_EXT_window_rectangles = 0;
        public static uint GL_EXT_window_rectangles => _GL_EXT_window_rectangles;
        public const uint GL_INCLUSIVE_EXT = 0x8F10;
        public const uint GL_EXCLUSIVE_EXT = 0x8F11;
        public const uint GL_WINDOW_RECTANGLE_EXT = 0x8F12;
        public const uint GL_WINDOW_RECTANGLE_MODE_EXT = 0x8F13;
        public const uint GL_MAX_WINDOW_RECTANGLES_EXT = 0x8F14;
        public const uint GL_NUM_WINDOW_RECTANGLES_EXT = 0x8F15;

        private static delegate* unmanaged[Stdcall]<uint, int, int*, void> pfn_glWindowRectanglesEXT = null;
        /// <summary> <see href="docs.gl/gl4/glWindowRectanglesEXT">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glWindowRectanglesEXT(uint mode, int count, int* box) => pfn_glWindowRectanglesEXT(mode, count, box);
    }

    public static unsafe class GLINTELBlackholeRender
    {
        static GLINTELBlackholeRender() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_INTEL_blackhole_render") ?? false) _GL_INTEL_blackhole_render = 1;
        }
        private static uint _GL_INTEL_blackhole_render = 0;
        public static uint GL_INTEL_blackhole_render => _GL_INTEL_blackhole_render;
        public const uint GL_BLACKHOLE_RENDER_INTEL = 0x83FC;
    }

    public static unsafe class GLINTELConservativeRasterization
    {
        static GLINTELConservativeRasterization() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_INTEL_conservative_rasterization") ?? false) _GL_INTEL_conservative_rasterization = 1;
        }
        private static uint _GL_INTEL_conservative_rasterization = 0;
        public static uint GL_INTEL_conservative_rasterization => _GL_INTEL_conservative_rasterization;
        public const uint GL_CONSERVATIVE_RASTERIZATION_INTEL = 0x83FE;
    }

    public static unsafe class GLINTELFramebufferCMAA
    {
        static GLINTELFramebufferCMAA() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_INTEL_framebuffer_CMAA") ?? false) _GL_INTEL_framebuffer_CMAA = 1;
        }
        private static uint _GL_INTEL_framebuffer_CMAA = 0;
        public static uint GL_INTEL_framebuffer_CMAA => _GL_INTEL_framebuffer_CMAA;

        private static delegate* unmanaged[Stdcall]<void> pfn_glApplyFramebufferAttachmentCMAAINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glApplyFramebufferAttachmentCMAAINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glApplyFramebufferAttachmentCMAAINTEL() => pfn_glApplyFramebufferAttachmentCMAAINTEL();
    }

    public static unsafe class GLINTELPerformanceQuery
    {
        static GLINTELPerformanceQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_INTEL_performance_query") ?? false) _GL_INTEL_performance_query = 1;
        }
        private static uint _GL_INTEL_performance_query = 0;
        public static uint GL_INTEL_performance_query => _GL_INTEL_performance_query;
        public const uint GL_PERFQUERY_SINGLE_CONTEXT_INTEL = 0x00000000;
        public const uint GL_PERFQUERY_GLOBAL_CONTEXT_INTEL = 0x00000001;
        public const uint GL_PERFQUERY_WAIT_INTEL = 0x83FB;
        public const uint GL_PERFQUERY_FLUSH_INTEL = 0x83FA;
        public const uint GL_PERFQUERY_DONOT_FLUSH_INTEL = 0x83F9;
        public const uint GL_PERFQUERY_COUNTER_EVENT_INTEL = 0x94F0;
        public const uint GL_PERFQUERY_COUNTER_DURATION_NORM_INTEL = 0x94F1;
        public const uint GL_PERFQUERY_COUNTER_DURATION_RAW_INTEL = 0x94F2;
        public const uint GL_PERFQUERY_COUNTER_THROUGHPUT_INTEL = 0x94F3;
        public const uint GL_PERFQUERY_COUNTER_RAW_INTEL = 0x94F4;
        public const uint GL_PERFQUERY_COUNTER_TIMESTAMP_INTEL = 0x94F5;
        public const uint GL_PERFQUERY_COUNTER_DATA_UINT32_INTEL = 0x94F8;
        public const uint GL_PERFQUERY_COUNTER_DATA_UINT64_INTEL = 0x94F9;
        public const uint GL_PERFQUERY_COUNTER_DATA_FLOAT_INTEL = 0x94FA;
        public const uint GL_PERFQUERY_COUNTER_DATA_DOUBLE_INTEL = 0x94FB;
        public const uint GL_PERFQUERY_COUNTER_DATA_BOOL32_INTEL = 0x94FC;
        public const uint GL_PERFQUERY_QUERY_NAME_LENGTH_MAX_INTEL = 0x94FD;
        public const uint GL_PERFQUERY_COUNTER_NAME_LENGTH_MAX_INTEL = 0x94FE;
        public const uint GL_PERFQUERY_COUNTER_DESC_LENGTH_MAX_INTEL = 0x94FF;
        public const uint GL_PERFQUERY_GPA_EXTENDED_COUNTERS_INTEL = 0x9500;

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBeginPerfQueryINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glBeginPerfQueryINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBeginPerfQueryINTEL(uint queryHandle) => pfn_glBeginPerfQueryINTEL(queryHandle);

        private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glCreatePerfQueryINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glCreatePerfQueryINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCreatePerfQueryINTEL(uint queryId, uint* queryHandle) => pfn_glCreatePerfQueryINTEL(queryId, queryHandle);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glDeletePerfQueryINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glDeletePerfQueryINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeletePerfQueryINTEL(uint queryHandle) => pfn_glDeletePerfQueryINTEL(queryHandle);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glEndPerfQueryINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glEndPerfQueryINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEndPerfQueryINTEL(uint queryHandle) => pfn_glEndPerfQueryINTEL(queryHandle);

        private static delegate* unmanaged[Stdcall]<uint*, void> pfn_glGetFirstPerfQueryIdINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetFirstPerfQueryIdINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetFirstPerfQueryIdINTEL(uint* queryId) => pfn_glGetFirstPerfQueryIdINTEL(queryId);

        private static delegate* unmanaged[Stdcall]<uint, uint*, void> pfn_glGetNextPerfQueryIdINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetNextPerfQueryIdINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNextPerfQueryIdINTEL(uint queryId, uint* nextQueryId) => pfn_glGetNextPerfQueryIdINTEL(queryId, nextQueryId);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, byte*, uint, byte*, uint*, uint*, uint*, uint*, ulong*, void> pfn_glGetPerfCounterInfoINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfCounterInfoINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfCounterInfoINTEL(uint queryId, uint counterId, uint counterNameLength, byte* counterName, uint counterDescLength, byte* counterDesc, uint* counterOffset, uint* counterDataSize, uint* counterTypeEnum, uint* counterDataTypeEnum, ulong* rawCounterMaxValue) => pfn_glGetPerfCounterInfoINTEL(queryId, counterId, counterNameLength, counterName, counterDescLength, counterDesc, counterOffset, counterDataSize, counterTypeEnum, counterDataTypeEnum, rawCounterMaxValue);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void*, uint*, void> pfn_glGetPerfQueryDataINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfQueryDataINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfQueryDataINTEL(uint queryHandle, uint flags, int dataSize, void* data, uint* bytesWritten) => pfn_glGetPerfQueryDataINTEL(queryHandle, flags, dataSize, data, bytesWritten);

        private static delegate* unmanaged[Stdcall]<byte*, uint*, void> pfn_glGetPerfQueryIdByNameINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfQueryIdByNameINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfQueryIdByNameINTEL(byte* queryName, uint* queryId) => pfn_glGetPerfQueryIdByNameINTEL(queryName, queryId);

        private static delegate* unmanaged[Stdcall]<uint, uint, byte*, uint*, uint*, uint*, uint*, void> pfn_glGetPerfQueryInfoINTEL = null;
        /// <summary> <see href="docs.gl/gl4/glGetPerfQueryInfoINTEL">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPerfQueryInfoINTEL(uint queryId, uint queryNameLength, byte* queryName, uint* dataSize, uint* noCounters, uint* noInstances, uint* capsMask) => pfn_glGetPerfQueryInfoINTEL(queryId, queryNameLength, queryName, dataSize, noCounters, noInstances, capsMask);
    }

    public static unsafe class GLMESAFramebufferFlipX
    {
        static GLMESAFramebufferFlipX() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_MESA_framebuffer_flip_x") ?? false) _GL_MESA_framebuffer_flip_x = 1;
        }
        private static uint _GL_MESA_framebuffer_flip_x = 0;
        public static uint GL_MESA_framebuffer_flip_x => _GL_MESA_framebuffer_flip_x;
        public const uint GL_FRAMEBUFFER_FLIP_X_MESA = 0x8BBC;
    }

    public static unsafe class GLMESAFramebufferFlipY
    {
        static GLMESAFramebufferFlipY() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_MESA_framebuffer_flip_y") ?? false) _GL_MESA_framebuffer_flip_y = 1;
        }
        private static uint _GL_MESA_framebuffer_flip_y = 0;
        public static uint GL_MESA_framebuffer_flip_y => _GL_MESA_framebuffer_flip_y;
        public const uint GL_FRAMEBUFFER_FLIP_Y_MESA = 0x8BBB;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glFramebufferParameteriMESA = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferParameteriMESA">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferParameteriMESA(uint target, uint pname, int param) => pfn_glFramebufferParameteriMESA(target, pname, param);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetFramebufferParameterivMESA = null;
        /// <summary> <see href="docs.gl/gl4/glGetFramebufferParameterivMESA">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetFramebufferParameterivMESA(uint target, uint pname, int* @params) => pfn_glGetFramebufferParameterivMESA(target, pname, @params);
    }

    public static unsafe class GLMESAFramebufferSwapXy
    {
        static GLMESAFramebufferSwapXy() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_MESA_framebuffer_swap_xy") ?? false) _GL_MESA_framebuffer_swap_xy = 1;
        }
        private static uint _GL_MESA_framebuffer_swap_xy = 0;
        public static uint GL_MESA_framebuffer_swap_xy => _GL_MESA_framebuffer_swap_xy;
        public const uint GL_FRAMEBUFFER_SWAP_XY_MESA = 0x8BBD;
    }

    public static unsafe class GLNVBindlessMultiDrawIndirect
    {
        static GLNVBindlessMultiDrawIndirect() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_bindless_multi_draw_indirect") ?? false) _GL_NV_bindless_multi_draw_indirect = 1;
        }
        private static uint _GL_NV_bindless_multi_draw_indirect = 0;
        public static uint GL_NV_bindless_multi_draw_indirect => _GL_NV_bindless_multi_draw_indirect;

        private static delegate* unmanaged[Stdcall]<uint, void*, int, int, int, void> pfn_glMultiDrawArraysIndirectBindlessNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawArraysIndirectBindlessNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawArraysIndirectBindlessNV(uint mode, void* indirect, int drawCount, int stride, int vertexBufferCount) => pfn_glMultiDrawArraysIndirectBindlessNV(mode, indirect, drawCount, stride, vertexBufferCount);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, int, int, int, void> pfn_glMultiDrawElementsIndirectBindlessNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsIndirectBindlessNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawElementsIndirectBindlessNV(uint mode, uint type, void* indirect, int drawCount, int stride, int vertexBufferCount) => pfn_glMultiDrawElementsIndirectBindlessNV(mode, type, indirect, drawCount, stride, vertexBufferCount);
    }

    public static unsafe class GLNVBindlessMultiDrawIndirectCount
    {
        static GLNVBindlessMultiDrawIndirectCount() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_bindless_multi_draw_indirect_count") ?? false) _GL_NV_bindless_multi_draw_indirect_count = 1;
        }
        private static uint _GL_NV_bindless_multi_draw_indirect_count = 0;
        public static uint GL_NV_bindless_multi_draw_indirect_count => _GL_NV_bindless_multi_draw_indirect_count;

        private static delegate* unmanaged[Stdcall]<uint, void*, int, int, int, int, void> pfn_glMultiDrawArraysIndirectBindlessCountNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawArraysIndirectBindlessCountNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawArraysIndirectBindlessCountNV(uint mode, void* indirect, int drawCount, int maxDrawCount, int stride, int vertexBufferCount) => pfn_glMultiDrawArraysIndirectBindlessCountNV(mode, indirect, drawCount, maxDrawCount, stride, vertexBufferCount);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, int, int, int, int, void> pfn_glMultiDrawElementsIndirectBindlessCountNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawElementsIndirectBindlessCountNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawElementsIndirectBindlessCountNV(uint mode, uint type, void* indirect, int drawCount, int maxDrawCount, int stride, int vertexBufferCount) => pfn_glMultiDrawElementsIndirectBindlessCountNV(mode, type, indirect, drawCount, maxDrawCount, stride, vertexBufferCount);
    }

    public static unsafe class GLNVBindlessTexture
    {
        static GLNVBindlessTexture() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_bindless_texture") ?? false) _GL_NV_bindless_texture = 1;
        }
        private static uint _GL_NV_bindless_texture = 0;
        public static uint GL_NV_bindless_texture => _GL_NV_bindless_texture;

        private static delegate* unmanaged[Stdcall]<uint, ulong> pfn_glGetTextureHandleNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureHandleNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetTextureHandleNV(uint texture) => pfn_glGetTextureHandleNV(texture);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong> pfn_glGetTextureSamplerHandleNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetTextureSamplerHandleNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetTextureSamplerHandleNV(uint texture, uint sampler) => pfn_glGetTextureSamplerHandleNV(texture, sampler);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeTextureHandleResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeTextureHandleResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeTextureHandleResidentNV(ulong handle) => pfn_glMakeTextureHandleResidentNV(handle);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeTextureHandleNonResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeTextureHandleNonResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeTextureHandleNonResidentNV(ulong handle) => pfn_glMakeTextureHandleNonResidentNV(handle);

        private static delegate* unmanaged[Stdcall]<uint, int, byte, int, uint, ulong> pfn_glGetImageHandleNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetImageHandleNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ulong glGetImageHandleNV(uint texture, int level, byte layered, int layer, uint format) => pfn_glGetImageHandleNV(texture, level, layered, layer, format);

        private static delegate* unmanaged[Stdcall]<ulong, uint, void> pfn_glMakeImageHandleResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeImageHandleResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeImageHandleResidentNV(ulong handle, uint access) => pfn_glMakeImageHandleResidentNV(handle, access);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glMakeImageHandleNonResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeImageHandleNonResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeImageHandleNonResidentNV(ulong handle) => pfn_glMakeImageHandleNonResidentNV(handle);

        private static delegate* unmanaged[Stdcall]<int, ulong, void> pfn_glUniformHandleui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniformHandleui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformHandleui64NV(int location, ulong value) => pfn_glUniformHandleui64NV(location, value);

        private static delegate* unmanaged[Stdcall]<int, int, ulong*, void> pfn_glUniformHandleui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniformHandleui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformHandleui64vNV(int location, int count, ulong* value) => pfn_glUniformHandleui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, ulong, void> pfn_glProgramUniformHandleui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformHandleui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformHandleui64NV(uint program, int location, ulong value) => pfn_glProgramUniformHandleui64NV(program, location, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, ulong*, void> pfn_glProgramUniformHandleui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformHandleui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformHandleui64vNV(uint program, int location, int count, ulong* values) => pfn_glProgramUniformHandleui64vNV(program, location, count, values);

        private static delegate* unmanaged[Stdcall]<ulong, byte> pfn_glIsTextureHandleResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsTextureHandleResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsTextureHandleResidentNV(ulong handle) => pfn_glIsTextureHandleResidentNV(handle);

        private static delegate* unmanaged[Stdcall]<ulong, byte> pfn_glIsImageHandleResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsImageHandleResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsImageHandleResidentNV(ulong handle) => pfn_glIsImageHandleResidentNV(handle);
    }

    public static unsafe class GLNVBlendEquationAdvanced
    {
        static GLNVBlendEquationAdvanced() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_blend_equation_advanced") ?? false) _GL_NV_blend_equation_advanced = 1;
        }
        private static uint _GL_NV_blend_equation_advanced = 0;
        public static uint GL_NV_blend_equation_advanced => _GL_NV_blend_equation_advanced;
        public const uint GL_BLEND_OVERLAP_NV = 0x9281;
        public const uint GL_BLEND_PREMULTIPLIED_SRC_NV = 0x9280;
        public const uint GL_BLUE_NV = 0x1905;
        public const uint GL_COLORBURN_NV = 0x929A;
        public const uint GL_COLORDODGE_NV = 0x9299;
        public const uint GL_CONJOINT_NV = 0x9284;
        public const uint GL_CONTRAST_NV = 0x92A1;
        public const uint GL_DARKEN_NV = 0x9297;
        public const uint GL_DIFFERENCE_NV = 0x929E;
        public const uint GL_DISJOINT_NV = 0x9283;
        public const uint GL_DST_ATOP_NV = 0x928F;
        public const uint GL_DST_IN_NV = 0x928B;
        public const uint GL_DST_NV = 0x9287;
        public const uint GL_DST_OUT_NV = 0x928D;
        public const uint GL_DST_OVER_NV = 0x9289;
        public const uint GL_EXCLUSION_NV = 0x92A0;
        public const uint GL_GREEN_NV = 0x1904;
        public const uint GL_HARDLIGHT_NV = 0x929B;
        public const uint GL_HARDMIX_NV = 0x92A9;
        public const uint GL_HSL_COLOR_NV = 0x92AF;
        public const uint GL_HSL_HUE_NV = 0x92AD;
        public const uint GL_HSL_LUMINOSITY_NV = 0x92B0;
        public const uint GL_HSL_SATURATION_NV = 0x92AE;
        public const uint GL_INVERT_OVG_NV = 0x92B4;
        public const uint GL_INVERT_RGB_NV = 0x92A3;
        public const uint GL_LIGHTEN_NV = 0x9298;
        public const uint GL_LINEARBURN_NV = 0x92A5;
        public const uint GL_LINEARDODGE_NV = 0x92A4;
        public const uint GL_LINEARLIGHT_NV = 0x92A7;
        public const uint GL_MINUS_CLAMPED_NV = 0x92B3;
        public const uint GL_MINUS_NV = 0x929F;
        public const uint GL_MULTIPLY_NV = 0x9294;
        public const uint GL_OVERLAY_NV = 0x9296;
        public const uint GL_PINLIGHT_NV = 0x92A8;
        public const uint GL_PLUS_CLAMPED_ALPHA_NV = 0x92B2;
        public const uint GL_PLUS_CLAMPED_NV = 0x92B1;
        public const uint GL_PLUS_DARKER_NV = 0x9292;
        public const uint GL_PLUS_NV = 0x9291;
        public const uint GL_RED_NV = 0x1903;
        public const uint GL_SCREEN_NV = 0x9295;
        public const uint GL_SOFTLIGHT_NV = 0x929C;
        public const uint GL_SRC_ATOP_NV = 0x928E;
        public const uint GL_SRC_IN_NV = 0x928A;
        public const uint GL_SRC_NV = 0x9286;
        public const uint GL_SRC_OUT_NV = 0x928C;
        public const uint GL_SRC_OVER_NV = 0x9288;
        public const uint GL_UNCORRELATED_NV = 0x9282;
        public const uint GL_VIVIDLIGHT_NV = 0x92A6;
        public const uint GL_XOR_NV = 0x1506;

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glBlendParameteriNV = null;
        /// <summary> <see href="docs.gl/gl4/glBlendParameteriNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendParameteriNV(uint pname, int value) => pfn_glBlendParameteriNV(pname, value);

        private static delegate* unmanaged[Stdcall]<void> pfn_glBlendBarrierNV = null;
        /// <summary> <see href="docs.gl/gl4/glBlendBarrierNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBlendBarrierNV() => pfn_glBlendBarrierNV();
    }

    public static unsafe class GLNVBlendEquationAdvancedCoherent
    {
        static GLNVBlendEquationAdvancedCoherent() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_blend_equation_advanced_coherent") ?? false) _GL_NV_blend_equation_advanced_coherent = 1;
        }
        private static uint _GL_NV_blend_equation_advanced_coherent = 0;
        public static uint GL_NV_blend_equation_advanced_coherent => _GL_NV_blend_equation_advanced_coherent;
        public const uint GL_BLEND_ADVANCED_COHERENT_NV = 0x9285;
    }

    public static unsafe class GLNVBlendMinmaxFactor
    {
        static GLNVBlendMinmaxFactor() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_blend_minmax_factor") ?? false) _GL_NV_blend_minmax_factor = 1;
        }
        private static uint _GL_NV_blend_minmax_factor = 0;
        public static uint GL_NV_blend_minmax_factor => _GL_NV_blend_minmax_factor;
        public const uint GL_FACTOR_MIN_AMD = 0x901C;
        public const uint GL_FACTOR_MAX_AMD = 0x901D;
    }

    public static unsafe class GLNVClipSpaceWScaling
    {
        static GLNVClipSpaceWScaling() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_clip_space_w_scaling") ?? false) _GL_NV_clip_space_w_scaling = 1;
        }
        private static uint _GL_NV_clip_space_w_scaling = 0;
        public static uint GL_NV_clip_space_w_scaling => _GL_NV_clip_space_w_scaling;
        public const uint GL_VIEWPORT_POSITION_W_SCALE_NV = 0x937C;
        public const uint GL_VIEWPORT_POSITION_W_SCALE_X_COEFF_NV = 0x937D;
        public const uint GL_VIEWPORT_POSITION_W_SCALE_Y_COEFF_NV = 0x937E;

        private static delegate* unmanaged[Stdcall]<uint, float, float, void> pfn_glViewportPositionWScaleNV = null;
        /// <summary> <see href="docs.gl/gl4/glViewportPositionWScaleNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glViewportPositionWScaleNV(uint index, float xcoeff, float ycoeff) => pfn_glViewportPositionWScaleNV(index, xcoeff, ycoeff);
    }

    public static unsafe class GLNVCommandList
    {
        static GLNVCommandList() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_command_list") ?? false) _GL_NV_command_list = 1;
        }
        private static uint _GL_NV_command_list = 0;
        public static uint GL_NV_command_list => _GL_NV_command_list;
        public const uint GL_TERMINATE_SEQUENCE_COMMAND_NV = 0x0000;
        public const uint GL_NOP_COMMAND_NV = 0x0001;
        public const uint GL_DRAW_ELEMENTS_COMMAND_NV = 0x0002;
        public const uint GL_DRAW_ARRAYS_COMMAND_NV = 0x0003;
        public const uint GL_DRAW_ELEMENTS_STRIP_COMMAND_NV = 0x0004;
        public const uint GL_DRAW_ARRAYS_STRIP_COMMAND_NV = 0x0005;
        public const uint GL_DRAW_ELEMENTS_INSTANCED_COMMAND_NV = 0x0006;
        public const uint GL_DRAW_ARRAYS_INSTANCED_COMMAND_NV = 0x0007;
        public const uint GL_ELEMENT_ADDRESS_COMMAND_NV = 0x0008;
        public const uint GL_ATTRIBUTE_ADDRESS_COMMAND_NV = 0x0009;
        public const uint GL_UNIFORM_ADDRESS_COMMAND_NV = 0x000A;
        public const uint GL_BLEND_COLOR_COMMAND_NV = 0x000B;
        public const uint GL_STENCIL_REF_COMMAND_NV = 0x000C;
        public const uint GL_LINE_WIDTH_COMMAND_NV = 0x000D;
        public const uint GL_POLYGON_OFFSET_COMMAND_NV = 0x000E;
        public const uint GL_ALPHA_REF_COMMAND_NV = 0x000F;
        public const uint GL_VIEWPORT_COMMAND_NV = 0x0010;
        public const uint GL_SCISSOR_COMMAND_NV = 0x0011;
        public const uint GL_FRONT_FACE_COMMAND_NV = 0x0012;

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateStatesNV = null;
        /// <summary> <see href="docs.gl/gl4/glCreateStatesNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCreateStatesNV(int n, uint* states) => pfn_glCreateStatesNV(n, states);

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteStatesNV = null;
        /// <summary> <see href="docs.gl/gl4/glDeleteStatesNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeleteStatesNV(int n, uint* states) => pfn_glDeleteStatesNV(n, states);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsStateNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsStateNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsStateNV(uint state) => pfn_glIsStateNV(state);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glStateCaptureNV = null;
        /// <summary> <see href="docs.gl/gl4/glStateCaptureNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStateCaptureNV(uint state, uint mode) => pfn_glStateCaptureNV(state, mode);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint> pfn_glGetCommandHeaderNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetCommandHeaderNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glGetCommandHeaderNV(uint tokenID, uint size) => pfn_glGetCommandHeaderNV(tokenID, size);

        private static delegate* unmanaged[Stdcall]<uint, ushort> pfn_glGetStageIndexNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetStageIndexNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ushort glGetStageIndexNV(uint shadertype) => pfn_glGetStageIndexNV(shadertype);

        private static delegate* unmanaged[Stdcall]<uint, uint, long*, int*, uint, void> pfn_glDrawCommandsNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawCommandsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawCommandsNV(uint primitiveMode, uint buffer, long* indirects, int* sizes, uint count) => pfn_glDrawCommandsNV(primitiveMode, buffer, indirects, sizes, count);

        private static delegate* unmanaged[Stdcall]<uint, ulong*, int*, uint, void> pfn_glDrawCommandsAddressNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawCommandsAddressNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawCommandsAddressNV(uint primitiveMode, ulong* indirects, int* sizes, uint count) => pfn_glDrawCommandsAddressNV(primitiveMode, indirects, sizes, count);

        private static delegate* unmanaged[Stdcall]<uint, long*, int*, uint*, uint*, uint, void> pfn_glDrawCommandsStatesNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawCommandsStatesNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawCommandsStatesNV(uint buffer, long* indirects, int* sizes, uint* states, uint* fbos, uint count) => pfn_glDrawCommandsStatesNV(buffer, indirects, sizes, states, fbos, count);

        private static delegate* unmanaged[Stdcall]<ulong*, int*, uint*, uint*, uint, void> pfn_glDrawCommandsStatesAddressNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawCommandsStatesAddressNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawCommandsStatesAddressNV(ulong* indirects, int* sizes, uint* states, uint* fbos, uint count) => pfn_glDrawCommandsStatesAddressNV(indirects, sizes, states, fbos, count);

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glCreateCommandListsNV = null;
        /// <summary> <see href="docs.gl/gl4/glCreateCommandListsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCreateCommandListsNV(int n, uint* lists) => pfn_glCreateCommandListsNV(n, lists);

        private static delegate* unmanaged[Stdcall]<int, uint*, void> pfn_glDeleteCommandListsNV = null;
        /// <summary> <see href="docs.gl/gl4/glDeleteCommandListsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeleteCommandListsNV(int n, uint* lists) => pfn_glDeleteCommandListsNV(n, lists);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsCommandListNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsCommandListNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsCommandListNV(uint list) => pfn_glIsCommandListNV(list);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, int*, uint*, uint*, uint, void> pfn_glListDrawCommandsStatesClientNV = null;
        /// <summary> <see href="docs.gl/gl4/glListDrawCommandsStatesClientNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glListDrawCommandsStatesClientNV(uint list, uint segment, void** indirects, int* sizes, uint* states, uint* fbos, uint count) => pfn_glListDrawCommandsStatesClientNV(list, segment, indirects, sizes, states, fbos, count);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glCommandListSegmentsNV = null;
        /// <summary> <see href="docs.gl/gl4/glCommandListSegmentsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCommandListSegmentsNV(uint list, uint segments) => pfn_glCommandListSegmentsNV(list, segments);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glCompileCommandListNV = null;
        /// <summary> <see href="docs.gl/gl4/glCompileCommandListNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCompileCommandListNV(uint list) => pfn_glCompileCommandListNV(list);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glCallCommandListNV = null;
        /// <summary> <see href="docs.gl/gl4/glCallCommandListNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCallCommandListNV(uint list) => pfn_glCallCommandListNV(list);
    }

    public static unsafe class GLNVComputeShaderDerivatives
    {
        static GLNVComputeShaderDerivatives() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_compute_shader_derivatives") ?? false) _GL_NV_compute_shader_derivatives = 1;
        }
        private static uint _GL_NV_compute_shader_derivatives = 0;
        public static uint GL_NV_compute_shader_derivatives => _GL_NV_compute_shader_derivatives;
    }

    public static unsafe class GLNVConditionalRender
    {
        static GLNVConditionalRender() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conditional_render") ?? false) _GL_NV_conditional_render = 1;
        }
        private static uint _GL_NV_conditional_render = 0;
        public static uint GL_NV_conditional_render => _GL_NV_conditional_render;
        public const uint GL_QUERY_WAIT_NV = 0x8E13;
        public const uint GL_QUERY_NO_WAIT_NV = 0x8E14;
        public const uint GL_QUERY_BY_REGION_WAIT_NV = 0x8E15;
        public const uint GL_QUERY_BY_REGION_NO_WAIT_NV = 0x8E16;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glBeginConditionalRenderNV = null;
        /// <summary> <see href="docs.gl/gl4/glBeginConditionalRenderNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBeginConditionalRenderNV(uint id, uint mode) => pfn_glBeginConditionalRenderNV(id, mode);

        private static delegate* unmanaged[Stdcall]<void> pfn_glEndConditionalRenderNV = null;
        /// <summary> <see href="docs.gl/gl4/glEndConditionalRenderNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEndConditionalRenderNV() => pfn_glEndConditionalRenderNV();
    }

    public static unsafe class GLNVConservativeRaster
    {
        static GLNVConservativeRaster() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conservative_raster") ?? false) _GL_NV_conservative_raster = 1;
        }
        private static uint _GL_NV_conservative_raster = 0;
        public static uint GL_NV_conservative_raster => _GL_NV_conservative_raster;
        public const uint GL_CONSERVATIVE_RASTERIZATION_NV = 0x9346;
        public const uint GL_SUBPIXEL_PRECISION_BIAS_X_BITS_NV = 0x9347;
        public const uint GL_SUBPIXEL_PRECISION_BIAS_Y_BITS_NV = 0x9348;
        public const uint GL_MAX_SUBPIXEL_PRECISION_BIAS_BITS_NV = 0x9349;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glSubpixelPrecisionBiasNV = null;
        /// <summary> <see href="docs.gl/gl4/glSubpixelPrecisionBiasNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSubpixelPrecisionBiasNV(uint xbits, uint ybits) => pfn_glSubpixelPrecisionBiasNV(xbits, ybits);
    }

    public static unsafe class GLNVConservativeRasterDilate
    {
        static GLNVConservativeRasterDilate() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conservative_raster_dilate") ?? false) _GL_NV_conservative_raster_dilate = 1;
        }
        private static uint _GL_NV_conservative_raster_dilate = 0;
        public static uint GL_NV_conservative_raster_dilate => _GL_NV_conservative_raster_dilate;
        public const uint GL_CONSERVATIVE_RASTER_DILATE_NV = 0x9379;
        public const uint GL_CONSERVATIVE_RASTER_DILATE_RANGE_NV = 0x937A;
        public const uint GL_CONSERVATIVE_RASTER_DILATE_GRANULARITY_NV = 0x937B;

        private static delegate* unmanaged[Stdcall]<uint, float, void> pfn_glConservativeRasterParameterfNV = null;
        /// <summary> <see href="docs.gl/gl4/glConservativeRasterParameterfNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glConservativeRasterParameterfNV(uint pname, float value) => pfn_glConservativeRasterParameterfNV(pname, value);
    }

    public static unsafe class GLNVConservativeRasterPreSnap
    {
        static GLNVConservativeRasterPreSnap() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conservative_raster_pre_snap") ?? false) _GL_NV_conservative_raster_pre_snap = 1;
        }
        private static uint _GL_NV_conservative_raster_pre_snap = 0;
        public static uint GL_NV_conservative_raster_pre_snap => _GL_NV_conservative_raster_pre_snap;
        public const uint GL_CONSERVATIVE_RASTER_MODE_PRE_SNAP_NV = 0x9550;
    }

    public static unsafe class GLNVConservativeRasterPreSnapTriangles
    {
        static GLNVConservativeRasterPreSnapTriangles() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conservative_raster_pre_snap_triangles") ?? false) _GL_NV_conservative_raster_pre_snap_triangles = 1;
        }
        private static uint _GL_NV_conservative_raster_pre_snap_triangles = 0;
        public static uint GL_NV_conservative_raster_pre_snap_triangles => _GL_NV_conservative_raster_pre_snap_triangles;
        public const uint GL_CONSERVATIVE_RASTER_MODE_NV = 0x954D;
        public const uint GL_CONSERVATIVE_RASTER_MODE_POST_SNAP_NV = 0x954E;
        public const uint GL_CONSERVATIVE_RASTER_MODE_PRE_SNAP_TRIANGLES_NV = 0x954F;

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glConservativeRasterParameteriNV = null;
        /// <summary> <see href="docs.gl/gl4/glConservativeRasterParameteriNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glConservativeRasterParameteriNV(uint pname, int param) => pfn_glConservativeRasterParameteriNV(pname, param);
    }

    public static unsafe class GLNVConservativeRasterUnderestimation
    {
        static GLNVConservativeRasterUnderestimation() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_conservative_raster_underestimation") ?? false) _GL_NV_conservative_raster_underestimation = 1;
        }
        private static uint _GL_NV_conservative_raster_underestimation = 0;
        public static uint GL_NV_conservative_raster_underestimation => _GL_NV_conservative_raster_underestimation;
    }

    public static unsafe class GLNVDepthBufferFloat
    {
        static GLNVDepthBufferFloat() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_depth_buffer_float") ?? false) _GL_NV_depth_buffer_float = 1;
        }
        private static uint _GL_NV_depth_buffer_float = 0;
        public static uint GL_NV_depth_buffer_float => _GL_NV_depth_buffer_float;
        public const uint GL_DEPTH_COMPONENT32F_NV = 0x8DAB;
        public const uint GL_DEPTH32F_STENCIL8_NV = 0x8DAC;
        public const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV_NV = 0x8DAD;
        public const uint GL_DEPTH_BUFFER_FLOAT_MODE_NV = 0x8DAF;

        private static delegate* unmanaged[Stdcall]<double, double, void> pfn_glDepthRangedNV = null;
        /// <summary> <see href="docs.gl/gl4/glDepthRangedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDepthRangedNV(double zNear, double zFar) => pfn_glDepthRangedNV(zNear, zFar);

        private static delegate* unmanaged[Stdcall]<double, void> pfn_glClearDepthdNV = null;
        /// <summary> <see href="docs.gl/gl4/glClearDepthdNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glClearDepthdNV(double depth) => pfn_glClearDepthdNV(depth);

        private static delegate* unmanaged[Stdcall]<double, double, void> pfn_glDepthBoundsdNV = null;
        /// <summary> <see href="docs.gl/gl4/glDepthBoundsdNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDepthBoundsdNV(double zmin, double zmax) => pfn_glDepthBoundsdNV(zmin, zmax);
    }

    public static unsafe class GLNVDrawVulkanImage
    {
        static GLNVDrawVulkanImage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_draw_vulkan_image") ?? false) _GL_NV_draw_vulkan_image = 1;
        }
        private static uint _GL_NV_draw_vulkan_image = 0;
        public static uint GL_NV_draw_vulkan_image => _GL_NV_draw_vulkan_image;

        private static delegate* unmanaged[Stdcall]<ulong, uint, float, float, float, float, float, float, float, float, float, void> pfn_glDrawVkImageNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawVkImageNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawVkImageNV(ulong vkImage, uint sampler, float x0, float y0, float x1, float y1, float z, float s0, float t0, float s1, float t1) => pfn_glDrawVkImageNV(vkImage, sampler, x0, y0, x1, y1, z, s0, t0, s1, t1);

        private static delegate* unmanaged[Stdcall]<byte*, void*> pfn_glGetVkProcAddrNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetVkProcAddrNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void* glGetVkProcAddrNV(byte* name) => pfn_glGetVkProcAddrNV(name);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glWaitVkSemaphoreNV = null;
        /// <summary> <see href="docs.gl/gl4/glWaitVkSemaphoreNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glWaitVkSemaphoreNV(ulong vkSemaphore) => pfn_glWaitVkSemaphoreNV(vkSemaphore);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glSignalVkSemaphoreNV = null;
        /// <summary> <see href="docs.gl/gl4/glSignalVkSemaphoreNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSignalVkSemaphoreNV(ulong vkSemaphore) => pfn_glSignalVkSemaphoreNV(vkSemaphore);

        private static delegate* unmanaged[Stdcall]<ulong, void> pfn_glSignalVkFenceNV = null;
        /// <summary> <see href="docs.gl/gl4/glSignalVkFenceNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSignalVkFenceNV(ulong vkFence) => pfn_glSignalVkFenceNV(vkFence);
    }

    public static unsafe class GLNVFillRectangle
    {
        static GLNVFillRectangle() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_fill_rectangle") ?? false) _GL_NV_fill_rectangle = 1;
        }
        private static uint _GL_NV_fill_rectangle = 0;
        public static uint GL_NV_fill_rectangle => _GL_NV_fill_rectangle;
        public const uint GL_FILL_RECTANGLE_NV = 0x933C;
    }

    public static unsafe class GLNVFragmentCoverageToColor
    {
        static GLNVFragmentCoverageToColor() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_fragment_coverage_to_color") ?? false) _GL_NV_fragment_coverage_to_color = 1;
        }
        private static uint _GL_NV_fragment_coverage_to_color = 0;
        public static uint GL_NV_fragment_coverage_to_color => _GL_NV_fragment_coverage_to_color;
        public const uint GL_FRAGMENT_COVERAGE_TO_COLOR_NV = 0x92DD;
        public const uint GL_FRAGMENT_COVERAGE_COLOR_NV = 0x92DE;

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glFragmentCoverageColorNV = null;
        /// <summary> <see href="docs.gl/gl4/glFragmentCoverageColorNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFragmentCoverageColorNV(uint color) => pfn_glFragmentCoverageColorNV(color);
    }

    public static unsafe class GLNVFragmentShaderBarycentric
    {
        static GLNVFragmentShaderBarycentric() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_fragment_shader_barycentric") ?? false) _GL_NV_fragment_shader_barycentric = 1;
        }
        private static uint _GL_NV_fragment_shader_barycentric = 0;
        public static uint GL_NV_fragment_shader_barycentric => _GL_NV_fragment_shader_barycentric;
    }

    public static unsafe class GLNVFragmentShaderInterlock
    {
        static GLNVFragmentShaderInterlock() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_fragment_shader_interlock") ?? false) _GL_NV_fragment_shader_interlock = 1;
        }
        private static uint _GL_NV_fragment_shader_interlock = 0;
        public static uint GL_NV_fragment_shader_interlock => _GL_NV_fragment_shader_interlock;
    }

    public static unsafe class GLNVFramebufferMixedSamples
    {
        static GLNVFramebufferMixedSamples() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_framebuffer_mixed_samples") ?? false) _GL_NV_framebuffer_mixed_samples = 1;
        }
        private static uint _GL_NV_framebuffer_mixed_samples = 0;
        public static uint GL_NV_framebuffer_mixed_samples => _GL_NV_framebuffer_mixed_samples;
        public const uint GL_COVERAGE_MODULATION_TABLE_NV = 0x9331;
        public const uint GL_COLOR_SAMPLES_NV = 0x8E20;
        public const uint GL_DEPTH_SAMPLES_NV = 0x932D;
        public const uint GL_STENCIL_SAMPLES_NV = 0x932E;
        public const uint GL_MIXED_DEPTH_SAMPLES_SUPPORTED_NV = 0x932F;
        public const uint GL_MIXED_STENCIL_SAMPLES_SUPPORTED_NV = 0x9330;
        public const uint GL_COVERAGE_MODULATION_NV = 0x9332;
        public const uint GL_COVERAGE_MODULATION_TABLE_SIZE_NV = 0x9333;

        private static delegate* unmanaged[Stdcall]<int, float*, void> pfn_glCoverageModulationTableNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverageModulationTableNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverageModulationTableNV(int n, float* v) => pfn_glCoverageModulationTableNV(n, v);

        private static delegate* unmanaged[Stdcall]<int, float*, void> pfn_glGetCoverageModulationTableNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetCoverageModulationTableNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetCoverageModulationTableNV(int bufSize, float* v) => pfn_glGetCoverageModulationTableNV(bufSize, v);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glCoverageModulationNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverageModulationNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverageModulationNV(uint components) => pfn_glCoverageModulationNV(components);
    }

    public static unsafe class GLNVFramebufferMultisampleCoverage
    {
        static GLNVFramebufferMultisampleCoverage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_framebuffer_multisample_coverage") ?? false) _GL_NV_framebuffer_multisample_coverage = 1;
        }
        private static uint _GL_NV_framebuffer_multisample_coverage = 0;
        public static uint GL_NV_framebuffer_multisample_coverage => _GL_NV_framebuffer_multisample_coverage;
        public const uint GL_RENDERBUFFER_COVERAGE_SAMPLES_NV = 0x8CAB;
        public const uint GL_RENDERBUFFER_COLOR_SAMPLES_NV = 0x8E10;
        public const uint GL_MAX_MULTISAMPLE_COVERAGE_MODES_NV = 0x8E11;
        public const uint GL_MULTISAMPLE_COVERAGE_MODES_NV = 0x8E12;

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint, int, int, void> pfn_glRenderbufferStorageMultisampleCoverageNV = null;
        /// <summary> <see href="docs.gl/gl4/glRenderbufferStorageMultisampleCoverageNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glRenderbufferStorageMultisampleCoverageNV(uint target, int coverageSamples, int colorSamples, uint internalformat, int width, int height) => pfn_glRenderbufferStorageMultisampleCoverageNV(target, coverageSamples, colorSamples, internalformat, width, height);
    }

    public static unsafe class GLNVGeometryShaderPassthrough
    {
        static GLNVGeometryShaderPassthrough() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_geometry_shader_passthrough") ?? false) _GL_NV_geometry_shader_passthrough = 1;
        }
        private static uint _GL_NV_geometry_shader_passthrough = 0;
        public static uint GL_NV_geometry_shader_passthrough => _GL_NV_geometry_shader_passthrough;
    }

    public static unsafe class GLNVGpuShader5
    {
        static GLNVGpuShader5() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_gpu_shader5") ?? false) _GL_NV_gpu_shader5 = 1;
        }
        private static uint _GL_NV_gpu_shader5 = 0;
        public static uint GL_NV_gpu_shader5 => _GL_NV_gpu_shader5;
        public const uint GL_INT64_NV = 0x140E;
        public const uint GL_UNSIGNED_INT64_NV = 0x140F;
        public const uint GL_INT8_NV = 0x8FE0;
        public const uint GL_INT8_VEC2_NV = 0x8FE1;
        public const uint GL_INT8_VEC3_NV = 0x8FE2;
        public const uint GL_INT8_VEC4_NV = 0x8FE3;
        public const uint GL_INT16_NV = 0x8FE4;
        public const uint GL_INT16_VEC2_NV = 0x8FE5;
        public const uint GL_INT16_VEC3_NV = 0x8FE6;
        public const uint GL_INT16_VEC4_NV = 0x8FE7;
        public const uint GL_INT64_VEC2_NV = 0x8FE9;
        public const uint GL_INT64_VEC3_NV = 0x8FEA;
        public const uint GL_INT64_VEC4_NV = 0x8FEB;
        public const uint GL_UNSIGNED_INT8_NV = 0x8FEC;
        public const uint GL_UNSIGNED_INT8_VEC2_NV = 0x8FED;
        public const uint GL_UNSIGNED_INT8_VEC3_NV = 0x8FEE;
        public const uint GL_UNSIGNED_INT8_VEC4_NV = 0x8FEF;
        public const uint GL_UNSIGNED_INT16_NV = 0x8FF0;
        public const uint GL_UNSIGNED_INT16_VEC2_NV = 0x8FF1;
        public const uint GL_UNSIGNED_INT16_VEC3_NV = 0x8FF2;
        public const uint GL_UNSIGNED_INT16_VEC4_NV = 0x8FF3;
        public const uint GL_UNSIGNED_INT64_VEC2_NV = 0x8FF5;
        public const uint GL_UNSIGNED_INT64_VEC3_NV = 0x8FF6;
        public const uint GL_UNSIGNED_INT64_VEC4_NV = 0x8FF7;
        public const uint GL_FLOAT16_NV = 0x8FF8;
        public const uint GL_FLOAT16_VEC2_NV = 0x8FF9;
        public const uint GL_FLOAT16_VEC3_NV = 0x8FFA;
        public const uint GL_FLOAT16_VEC4_NV = 0x8FFB;

        private static delegate* unmanaged[Stdcall]<int, long, void> pfn_glUniform1i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1i64NV(int location, long x) => pfn_glUniform1i64NV(location, x);

        private static delegate* unmanaged[Stdcall]<int, long, long, void> pfn_glUniform2i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2i64NV(int location, long x, long y) => pfn_glUniform2i64NV(location, x, y);

        private static delegate* unmanaged[Stdcall]<int, long, long, long, void> pfn_glUniform3i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3i64NV(int location, long x, long y, long z) => pfn_glUniform3i64NV(location, x, y, z);

        private static delegate* unmanaged[Stdcall]<int, long, long, long, long, void> pfn_glUniform4i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4i64NV(int location, long x, long y, long z, long w) => pfn_glUniform4i64NV(location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform1i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1i64vNV(int location, int count, long* value) => pfn_glUniform1i64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform2i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2i64vNV(int location, int count, long* value) => pfn_glUniform2i64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform3i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3i64vNV(int location, int count, long* value) => pfn_glUniform3i64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, long*, void> pfn_glUniform4i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4i64vNV(int location, int count, long* value) => pfn_glUniform4i64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, void*, void> pfn_glUniform1ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1ui64NV(int location, void* x) => pfn_glUniform1ui64NV(location, x);

        private static delegate* unmanaged[Stdcall]<int, void*, void*, void> pfn_glUniform2ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2ui64NV(int location, void* x, void* y) => pfn_glUniform2ui64NV(location, x, y);

        private static delegate* unmanaged[Stdcall]<int, void*, void*, void*, void> pfn_glUniform3ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3ui64NV(int location, void* x, void* y, void* z) => pfn_glUniform3ui64NV(location, x, y, z);

        private static delegate* unmanaged[Stdcall]<int, void*, void*, void*, void*, void> pfn_glUniform4ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4ui64NV(int location, void* x, void* y, void* z, void* w) => pfn_glUniform4ui64NV(location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<int, int, void**, void> pfn_glUniform1ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform1ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform1ui64vNV(int location, int count, void** value) => pfn_glUniform1ui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, void**, void> pfn_glUniform2ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform2ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform2ui64vNV(int location, int count, void** value) => pfn_glUniform2ui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, void**, void> pfn_glUniform3ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform3ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform3ui64vNV(int location, int count, void** value) => pfn_glUniform3ui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<int, int, void**, void> pfn_glUniform4ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniform4ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniform4ui64vNV(int location, int count, void** value) => pfn_glUniform4ui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, long*, void> pfn_glGetUniformi64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetUniformi64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetUniformi64vNV(uint program, int location, long* @params) => pfn_glGetUniformi64vNV(program, location, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, long, void> pfn_glProgramUniform1i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1i64NV(uint program, int location, long x) => pfn_glProgramUniform1i64NV(program, location, x);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, void> pfn_glProgramUniform2i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2i64NV(uint program, int location, long x, long y) => pfn_glProgramUniform2i64NV(program, location, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, long, void> pfn_glProgramUniform3i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3i64NV(uint program, int location, long x, long y, long z) => pfn_glProgramUniform3i64NV(program, location, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, int, long, long, long, long, void> pfn_glProgramUniform4i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4i64NV(uint program, int location, long x, long y, long z, long w) => pfn_glProgramUniform4i64NV(program, location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform1i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1i64vNV(uint program, int location, int count, long* value) => pfn_glProgramUniform1i64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform2i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2i64vNV(uint program, int location, int count, long* value) => pfn_glProgramUniform2i64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform3i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3i64vNV(uint program, int location, int count, long* value) => pfn_glProgramUniform3i64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, long*, void> pfn_glProgramUniform4i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4i64vNV(uint program, int location, int count, long* value) => pfn_glProgramUniform4i64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, void*, void> pfn_glProgramUniform1ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1ui64NV(uint program, int location, void* x) => pfn_glProgramUniform1ui64NV(program, location, x);

        private static delegate* unmanaged[Stdcall]<uint, int, void*, void*, void> pfn_glProgramUniform2ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2ui64NV(uint program, int location, void* x, void* y) => pfn_glProgramUniform2ui64NV(program, location, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, void*, void*, void*, void> pfn_glProgramUniform3ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3ui64NV(uint program, int location, void* x, void* y, void* z) => pfn_glProgramUniform3ui64NV(program, location, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, int, void*, void*, void*, void*, void> pfn_glProgramUniform4ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4ui64NV(uint program, int location, void* x, void* y, void* z, void* w) => pfn_glProgramUniform4ui64NV(program, location, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void**, void> pfn_glProgramUniform1ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform1ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform1ui64vNV(uint program, int location, int count, void** value) => pfn_glProgramUniform1ui64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void**, void> pfn_glProgramUniform2ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform2ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform2ui64vNV(uint program, int location, int count, void** value) => pfn_glProgramUniform2ui64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void**, void> pfn_glProgramUniform3ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform3ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform3ui64vNV(uint program, int location, int count, void** value) => pfn_glProgramUniform3ui64vNV(program, location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void**, void> pfn_glProgramUniform4ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniform4ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniform4ui64vNV(uint program, int location, int count, void** value) => pfn_glProgramUniform4ui64vNV(program, location, count, value);
    }

    public static unsafe class GLNVInternalformatSampleQuery
    {
        static GLNVInternalformatSampleQuery() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_internalformat_sample_query") ?? false) _GL_NV_internalformat_sample_query = 1;
        }
        private static uint _GL_NV_internalformat_sample_query = 0;
        public static uint GL_NV_internalformat_sample_query => _GL_NV_internalformat_sample_query;
        public const uint GL_MULTISAMPLES_NV = 0x9371;
        public const uint GL_SUPERSAMPLE_SCALE_X_NV = 0x9372;
        public const uint GL_SUPERSAMPLE_SCALE_Y_NV = 0x9373;
        public const uint GL_CONFORMANT_NV = 0x9374;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint, int, int*, void> pfn_glGetInternalformatSampleivNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetInternalformatSampleivNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetInternalformatSampleivNV(uint target, uint internalformat, int samples, uint pname, int count, int* @params) => pfn_glGetInternalformatSampleivNV(target, internalformat, samples, pname, count, @params);
    }

    public static unsafe class GLNVMemoryAttachment
    {
        static GLNVMemoryAttachment() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_memory_attachment") ?? false) _GL_NV_memory_attachment = 1;
        }
        private static uint _GL_NV_memory_attachment = 0;
        public static uint GL_NV_memory_attachment => _GL_NV_memory_attachment;
        public const uint GL_ATTACHED_MEMORY_OBJECT_NV = 0x95A4;
        public const uint GL_ATTACHED_MEMORY_OFFSET_NV = 0x95A5;
        public const uint GL_MEMORY_ATTACHABLE_ALIGNMENT_NV = 0x95A6;
        public const uint GL_MEMORY_ATTACHABLE_SIZE_NV = 0x95A7;
        public const uint GL_MEMORY_ATTACHABLE_NV = 0x95A8;
        public const uint GL_DETACHED_MEMORY_INCARNATION_NV = 0x95A9;
        public const uint GL_DETACHED_TEXTURES_NV = 0x95AA;
        public const uint GL_DETACHED_BUFFERS_NV = 0x95AB;
        public const uint GL_MAX_DETACHED_TEXTURES_NV = 0x95AC;
        public const uint GL_MAX_DETACHED_BUFFERS_NV = 0x95AD;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, uint*, void> pfn_glGetMemoryObjectDetachedResourcesuivNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetMemoryObjectDetachedResourcesuivNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetMemoryObjectDetachedResourcesuivNV(uint memory, uint pname, int first, int count, uint* @params) => pfn_glGetMemoryObjectDetachedResourcesuivNV(memory, pname, first, count, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glResetMemoryObjectParameterNV = null;
        /// <summary> <see href="docs.gl/gl4/glResetMemoryObjectParameterNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glResetMemoryObjectParameterNV(uint memory, uint pname) => pfn_glResetMemoryObjectParameterNV(memory, pname);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong, void> pfn_glTexAttachMemoryNV = null;
        /// <summary> <see href="docs.gl/gl4/glTexAttachMemoryNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexAttachMemoryNV(uint target, uint memory, ulong offset) => pfn_glTexAttachMemoryNV(target, memory, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong, void> pfn_glBufferAttachMemoryNV = null;
        /// <summary> <see href="docs.gl/gl4/glBufferAttachMemoryNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBufferAttachMemoryNV(uint target, uint memory, ulong offset) => pfn_glBufferAttachMemoryNV(target, memory, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong, void> pfn_glTextureAttachMemoryNV = null;
        /// <summary> <see href="docs.gl/gl4/glTextureAttachMemoryNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureAttachMemoryNV(uint texture, uint memory, ulong offset) => pfn_glTextureAttachMemoryNV(texture, memory, offset);

        private static delegate* unmanaged[Stdcall]<uint, uint, ulong, void> pfn_glNamedBufferAttachMemoryNV = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferAttachMemoryNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferAttachMemoryNV(uint buffer, uint memory, ulong offset) => pfn_glNamedBufferAttachMemoryNV(buffer, memory, offset);
    }

    public static unsafe class GLNVMemoryObjectSparse
    {
        static GLNVMemoryObjectSparse() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_memory_object_sparse") ?? false) _GL_NV_memory_object_sparse = 1;
        }
        private static uint _GL_NV_memory_object_sparse = 0;
        public static uint GL_NV_memory_object_sparse => _GL_NV_memory_object_sparse;

        private static delegate* unmanaged[Stdcall]<uint, long, long, uint, ulong, byte, void> pfn_glBufferPageCommitmentMemNV = null;
        /// <summary> <see href="docs.gl/gl4/glBufferPageCommitmentMemNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBufferPageCommitmentMemNV(uint target, long offset, long size, uint memory, ulong memOffset, byte commit) => pfn_glBufferPageCommitmentMemNV(target, offset, size, memory, memOffset, commit);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, uint, ulong, byte, void> pfn_glTexPageCommitmentMemNV = null;
        /// <summary> <see href="docs.gl/gl4/glTexPageCommitmentMemNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexPageCommitmentMemNV(uint target, int layer, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint memory, ulong offset, byte commit) => pfn_glTexPageCommitmentMemNV(target, layer, level, xoffset, yoffset, zoffset, width, height, depth, memory, offset, commit);

        private static delegate* unmanaged[Stdcall]<uint, long, long, uint, ulong, byte, void> pfn_glNamedBufferPageCommitmentMemNV = null;
        /// <summary> <see href="docs.gl/gl4/glNamedBufferPageCommitmentMemNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedBufferPageCommitmentMemNV(uint buffer, long offset, long size, uint memory, ulong memOffset, byte commit) => pfn_glNamedBufferPageCommitmentMemNV(buffer, offset, size, memory, memOffset, commit);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, uint, ulong, byte, void> pfn_glTexturePageCommitmentMemNV = null;
        /// <summary> <see href="docs.gl/gl4/glTexturePageCommitmentMemNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexturePageCommitmentMemNV(uint texture, int layer, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint memory, ulong offset, byte commit) => pfn_glTexturePageCommitmentMemNV(texture, layer, level, xoffset, yoffset, zoffset, width, height, depth, memory, offset, commit);
    }

    public static unsafe class GLNVMeshShader
    {
        static GLNVMeshShader() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_mesh_shader") ?? false) _GL_NV_mesh_shader = 1;
        }
        private static uint _GL_NV_mesh_shader = 0;
        public static uint GL_NV_mesh_shader => _GL_NV_mesh_shader;
        public const uint GL_MESH_SHADER_NV = 0x9559;
        public const uint GL_TASK_SHADER_NV = 0x955A;
        public const uint GL_MAX_MESH_UNIFORM_BLOCKS_NV = 0x8E60;
        public const uint GL_MAX_MESH_TEXTURE_IMAGE_UNITS_NV = 0x8E61;
        public const uint GL_MAX_MESH_IMAGE_UNIFORMS_NV = 0x8E62;
        public const uint GL_MAX_MESH_UNIFORM_COMPONENTS_NV = 0x8E63;
        public const uint GL_MAX_MESH_ATOMIC_COUNTER_BUFFERS_NV = 0x8E64;
        public const uint GL_MAX_MESH_ATOMIC_COUNTERS_NV = 0x8E65;
        public const uint GL_MAX_MESH_SHADER_STORAGE_BLOCKS_NV = 0x8E66;
        public const uint GL_MAX_COMBINED_MESH_UNIFORM_COMPONENTS_NV = 0x8E67;
        public const uint GL_MAX_TASK_UNIFORM_BLOCKS_NV = 0x8E68;
        public const uint GL_MAX_TASK_TEXTURE_IMAGE_UNITS_NV = 0x8E69;
        public const uint GL_MAX_TASK_IMAGE_UNIFORMS_NV = 0x8E6A;
        public const uint GL_MAX_TASK_UNIFORM_COMPONENTS_NV = 0x8E6B;
        public const uint GL_MAX_TASK_ATOMIC_COUNTER_BUFFERS_NV = 0x8E6C;
        public const uint GL_MAX_TASK_ATOMIC_COUNTERS_NV = 0x8E6D;
        public const uint GL_MAX_TASK_SHADER_STORAGE_BLOCKS_NV = 0x8E6E;
        public const uint GL_MAX_COMBINED_TASK_UNIFORM_COMPONENTS_NV = 0x8E6F;
        public const uint GL_MAX_MESH_WORK_GROUP_INVOCATIONS_NV = 0x95A2;
        public const uint GL_MAX_TASK_WORK_GROUP_INVOCATIONS_NV = 0x95A3;
        public const uint GL_MAX_MESH_TOTAL_MEMORY_SIZE_NV = 0x9536;
        public const uint GL_MAX_TASK_TOTAL_MEMORY_SIZE_NV = 0x9537;
        public const uint GL_MAX_MESH_OUTPUT_VERTICES_NV = 0x9538;
        public const uint GL_MAX_MESH_OUTPUT_PRIMITIVES_NV = 0x9539;
        public const uint GL_MAX_TASK_OUTPUT_COUNT_NV = 0x953A;
        public const uint GL_MAX_DRAW_MESH_TASKS_COUNT_NV = 0x953D;
        public const uint GL_MAX_MESH_VIEWS_NV = 0x9557;
        public const uint GL_MESH_OUTPUT_PER_VERTEX_GRANULARITY_NV = 0x92DF;
        public const uint GL_MESH_OUTPUT_PER_PRIMITIVE_GRANULARITY_NV = 0x9543;
        public const uint GL_MAX_MESH_WORK_GROUP_SIZE_NV = 0x953B;
        public const uint GL_MAX_TASK_WORK_GROUP_SIZE_NV = 0x953C;
        public const uint GL_MESH_WORK_GROUP_SIZE_NV = 0x953E;
        public const uint GL_TASK_WORK_GROUP_SIZE_NV = 0x953F;
        public const uint GL_MESH_VERTICES_OUT_NV = 0x9579;
        public const uint GL_MESH_PRIMITIVES_OUT_NV = 0x957A;
        public const uint GL_MESH_OUTPUT_TYPE_NV = 0x957B;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_MESH_SHADER_NV = 0x959C;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_TASK_SHADER_NV = 0x959D;
        public const uint GL_REFERENCED_BY_MESH_SHADER_NV = 0x95A0;
        public const uint GL_REFERENCED_BY_TASK_SHADER_NV = 0x95A1;
        public const uint GL_MESH_SHADER_BIT_NV = 0x00000040;
        public const uint GL_TASK_SHADER_BIT_NV = 0x00000080;
        public const uint GL_MESH_SUBROUTINE_NV = 0x957C;
        public const uint GL_TASK_SUBROUTINE_NV = 0x957D;
        public const uint GL_MESH_SUBROUTINE_UNIFORM_NV = 0x957E;
        public const uint GL_TASK_SUBROUTINE_UNIFORM_NV = 0x957F;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_MESH_SHADER_NV = 0x959E;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TASK_SHADER_NV = 0x959F;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glDrawMeshTasksNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawMeshTasksNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawMeshTasksNV(uint first, uint count) => pfn_glDrawMeshTasksNV(first, count);

        private static delegate* unmanaged[Stdcall]<long, void> pfn_glDrawMeshTasksIndirectNV = null;
        /// <summary> <see href="docs.gl/gl4/glDrawMeshTasksIndirectNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDrawMeshTasksIndirectNV(long indirect) => pfn_glDrawMeshTasksIndirectNV(indirect);

        private static delegate* unmanaged[Stdcall]<long, int, int, void> pfn_glMultiDrawMeshTasksIndirectNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawMeshTasksIndirectNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawMeshTasksIndirectNV(long indirect, int drawcount, int stride) => pfn_glMultiDrawMeshTasksIndirectNV(indirect, drawcount, stride);

        private static delegate* unmanaged[Stdcall]<long, long, int, int, void> pfn_glMultiDrawMeshTasksIndirectCountNV = null;
        /// <summary> <see href="docs.gl/gl4/glMultiDrawMeshTasksIndirectCountNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMultiDrawMeshTasksIndirectCountNV(long indirect, long drawcount, int maxdrawcount, int stride) => pfn_glMultiDrawMeshTasksIndirectCountNV(indirect, drawcount, maxdrawcount, stride);
    }

    public static unsafe class GLNVPathRendering
    {
        static GLNVPathRendering() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_path_rendering") ?? false) _GL_NV_path_rendering = 1;
        }
        private static uint _GL_NV_path_rendering = 0;
        public static uint GL_NV_path_rendering => _GL_NV_path_rendering;
        public const uint GL_PATH_FORMAT_SVG_NV = 0x9070;
        public const uint GL_PATH_FORMAT_PS_NV = 0x9071;
        public const uint GL_STANDARD_FONT_NAME_NV = 0x9072;
        public const uint GL_SYSTEM_FONT_NAME_NV = 0x9073;
        public const uint GL_FILE_NAME_NV = 0x9074;
        public const uint GL_PATH_STROKE_WIDTH_NV = 0x9075;
        public const uint GL_PATH_END_CAPS_NV = 0x9076;
        public const uint GL_PATH_INITIAL_END_CAP_NV = 0x9077;
        public const uint GL_PATH_TERMINAL_END_CAP_NV = 0x9078;
        public const uint GL_PATH_JOIN_STYLE_NV = 0x9079;
        public const uint GL_PATH_MITER_LIMIT_NV = 0x907A;
        public const uint GL_PATH_DASH_CAPS_NV = 0x907B;
        public const uint GL_PATH_INITIAL_DASH_CAP_NV = 0x907C;
        public const uint GL_PATH_TERMINAL_DASH_CAP_NV = 0x907D;
        public const uint GL_PATH_DASH_OFFSET_NV = 0x907E;
        public const uint GL_PATH_CLIENT_LENGTH_NV = 0x907F;
        public const uint GL_PATH_FILL_MODE_NV = 0x9080;
        public const uint GL_PATH_FILL_MASK_NV = 0x9081;
        public const uint GL_PATH_FILL_COVER_MODE_NV = 0x9082;
        public const uint GL_PATH_STROKE_COVER_MODE_NV = 0x9083;
        public const uint GL_PATH_STROKE_MASK_NV = 0x9084;
        public const uint GL_COUNT_UP_NV = 0x9088;
        public const uint GL_COUNT_DOWN_NV = 0x9089;
        public const uint GL_PATH_OBJECT_BOUNDING_BOX_NV = 0x908A;
        public const uint GL_CONVEX_HULL_NV = 0x908B;
        public const uint GL_BOUNDING_BOX_NV = 0x908D;
        public const uint GL_TRANSLATE_X_NV = 0x908E;
        public const uint GL_TRANSLATE_Y_NV = 0x908F;
        public const uint GL_TRANSLATE_2D_NV = 0x9090;
        public const uint GL_TRANSLATE_3D_NV = 0x9091;
        public const uint GL_AFFINE_2D_NV = 0x9092;
        public const uint GL_AFFINE_3D_NV = 0x9094;
        public const uint GL_TRANSPOSE_AFFINE_2D_NV = 0x9096;
        public const uint GL_TRANSPOSE_AFFINE_3D_NV = 0x9098;
        public const uint GL_UTF8_NV = 0x909A;
        public const uint GL_UTF16_NV = 0x909B;
        public const uint GL_BOUNDING_BOX_OF_BOUNDING_BOXES_NV = 0x909C;
        public const uint GL_PATH_COMMAND_COUNT_NV = 0x909D;
        public const uint GL_PATH_COORD_COUNT_NV = 0x909E;
        public const uint GL_PATH_DASH_ARRAY_COUNT_NV = 0x909F;
        public const uint GL_PATH_COMPUTED_LENGTH_NV = 0x90A0;
        public const uint GL_PATH_FILL_BOUNDING_BOX_NV = 0x90A1;
        public const uint GL_PATH_STROKE_BOUNDING_BOX_NV = 0x90A2;
        public const uint GL_SQUARE_NV = 0x90A3;
        public const uint GL_ROUND_NV = 0x90A4;
        public const uint GL_TRIANGULAR_NV = 0x90A5;
        public const uint GL_BEVEL_NV = 0x90A6;
        public const uint GL_MITER_REVERT_NV = 0x90A7;
        public const uint GL_MITER_TRUNCATE_NV = 0x90A8;
        public const uint GL_SKIP_MISSING_GLYPH_NV = 0x90A9;
        public const uint GL_USE_MISSING_GLYPH_NV = 0x90AA;
        public const uint GL_PATH_ERROR_POSITION_NV = 0x90AB;
        public const uint GL_ACCUM_ADJACENT_PAIRS_NV = 0x90AD;
        public const uint GL_ADJACENT_PAIRS_NV = 0x90AE;
        public const uint GL_FIRST_TO_REST_NV = 0x90AF;
        public const uint GL_PATH_GEN_MODE_NV = 0x90B0;
        public const uint GL_PATH_GEN_COEFF_NV = 0x90B1;
        public const uint GL_PATH_GEN_COMPONENTS_NV = 0x90B3;
        public const uint GL_PATH_STENCIL_FUNC_NV = 0x90B7;
        public const uint GL_PATH_STENCIL_REF_NV = 0x90B8;
        public const uint GL_PATH_STENCIL_VALUE_MASK_NV = 0x90B9;
        public const uint GL_PATH_STENCIL_DEPTH_OFFSET_FACTOR_NV = 0x90BD;
        public const uint GL_PATH_STENCIL_DEPTH_OFFSET_UNITS_NV = 0x90BE;
        public const uint GL_PATH_COVER_DEPTH_FUNC_NV = 0x90BF;
        public const uint GL_PATH_DASH_OFFSET_RESET_NV = 0x90B4;
        public const uint GL_MOVE_TO_RESETS_NV = 0x90B5;
        public const uint GL_MOVE_TO_CONTINUES_NV = 0x90B6;
        public const uint GL_CLOSE_PATH_NV = 0x00;
        public const uint GL_MOVE_TO_NV = 0x02;
        public const uint GL_RELATIVE_MOVE_TO_NV = 0x03;
        public const uint GL_LINE_TO_NV = 0x04;
        public const uint GL_RELATIVE_LINE_TO_NV = 0x05;
        public const uint GL_HORIZONTAL_LINE_TO_NV = 0x06;
        public const uint GL_RELATIVE_HORIZONTAL_LINE_TO_NV = 0x07;
        public const uint GL_VERTICAL_LINE_TO_NV = 0x08;
        public const uint GL_RELATIVE_VERTICAL_LINE_TO_NV = 0x09;
        public const uint GL_QUADRATIC_CURVE_TO_NV = 0x0A;
        public const uint GL_RELATIVE_QUADRATIC_CURVE_TO_NV = 0x0B;
        public const uint GL_CUBIC_CURVE_TO_NV = 0x0C;
        public const uint GL_RELATIVE_CUBIC_CURVE_TO_NV = 0x0D;
        public const uint GL_SMOOTH_QUADRATIC_CURVE_TO_NV = 0x0E;
        public const uint GL_RELATIVE_SMOOTH_QUADRATIC_CURVE_TO_NV = 0x0F;
        public const uint GL_SMOOTH_CUBIC_CURVE_TO_NV = 0x10;
        public const uint GL_RELATIVE_SMOOTH_CUBIC_CURVE_TO_NV = 0x11;
        public const uint GL_SMALL_CCW_ARC_TO_NV = 0x12;
        public const uint GL_RELATIVE_SMALL_CCW_ARC_TO_NV = 0x13;
        public const uint GL_SMALL_CW_ARC_TO_NV = 0x14;
        public const uint GL_RELATIVE_SMALL_CW_ARC_TO_NV = 0x15;
        public const uint GL_LARGE_CCW_ARC_TO_NV = 0x16;
        public const uint GL_RELATIVE_LARGE_CCW_ARC_TO_NV = 0x17;
        public const uint GL_LARGE_CW_ARC_TO_NV = 0x18;
        public const uint GL_RELATIVE_LARGE_CW_ARC_TO_NV = 0x19;
        public const uint GL_RESTART_PATH_NV = 0xF0;
        public const uint GL_DUP_FIRST_CUBIC_CURVE_TO_NV = 0xF2;
        public const uint GL_DUP_LAST_CUBIC_CURVE_TO_NV = 0xF4;
        public const uint GL_RECT_NV = 0xF6;
        public const uint GL_CIRCULAR_CCW_ARC_TO_NV = 0xF8;
        public const uint GL_CIRCULAR_CW_ARC_TO_NV = 0xFA;
        public const uint GL_CIRCULAR_TANGENT_ARC_TO_NV = 0xFC;
        public const uint GL_ARC_TO_NV = 0xFE;
        public const uint GL_RELATIVE_ARC_TO_NV = 0xFF;
        public const uint GL_BOLD_BIT_NV = 0x01;
        public const uint GL_ITALIC_BIT_NV = 0x02;
        public const uint GL_GLYPH_WIDTH_BIT_NV = 0x01;
        public const uint GL_GLYPH_HEIGHT_BIT_NV = 0x02;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_X_BIT_NV = 0x04;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_Y_BIT_NV = 0x08;
        public const uint GL_GLYPH_HORIZONTAL_BEARING_ADVANCE_BIT_NV = 0x10;
        public const uint GL_GLYPH_VERTICAL_BEARING_X_BIT_NV = 0x20;
        public const uint GL_GLYPH_VERTICAL_BEARING_Y_BIT_NV = 0x40;
        public const uint GL_GLYPH_VERTICAL_BEARING_ADVANCE_BIT_NV = 0x80;
        public const uint GL_GLYPH_HAS_KERNING_BIT_NV = 0x100;
        public const uint GL_FONT_X_MIN_BOUNDS_BIT_NV = 0x00010000;
        public const uint GL_FONT_Y_MIN_BOUNDS_BIT_NV = 0x00020000;
        public const uint GL_FONT_X_MAX_BOUNDS_BIT_NV = 0x00040000;
        public const uint GL_FONT_Y_MAX_BOUNDS_BIT_NV = 0x00080000;
        public const uint GL_FONT_UNITS_PER_EM_BIT_NV = 0x00100000;
        public const uint GL_FONT_ASCENDER_BIT_NV = 0x00200000;
        public const uint GL_FONT_DESCENDER_BIT_NV = 0x00400000;
        public const uint GL_FONT_HEIGHT_BIT_NV = 0x00800000;
        public const uint GL_FONT_MAX_ADVANCE_WIDTH_BIT_NV = 0x01000000;
        public const uint GL_FONT_MAX_ADVANCE_HEIGHT_BIT_NV = 0x02000000;
        public const uint GL_FONT_UNDERLINE_POSITION_BIT_NV = 0x04000000;
        public const uint GL_FONT_UNDERLINE_THICKNESS_BIT_NV = 0x08000000;
        public const uint GL_FONT_HAS_KERNING_BIT_NV = 0x10000000;
        public const uint GL_ROUNDED_RECT_NV = 0xE8;
        public const uint GL_RELATIVE_ROUNDED_RECT_NV = 0xE9;
        public const uint GL_ROUNDED_RECT2_NV = 0xEA;
        public const uint GL_RELATIVE_ROUNDED_RECT2_NV = 0xEB;
        public const uint GL_ROUNDED_RECT4_NV = 0xEC;
        public const uint GL_RELATIVE_ROUNDED_RECT4_NV = 0xED;
        public const uint GL_ROUNDED_RECT8_NV = 0xEE;
        public const uint GL_RELATIVE_ROUNDED_RECT8_NV = 0xEF;
        public const uint GL_RELATIVE_RECT_NV = 0xF7;
        public const uint GL_FONT_GLYPHS_AVAILABLE_NV = 0x9368;
        public const uint GL_FONT_TARGET_UNAVAILABLE_NV = 0x9369;
        public const uint GL_FONT_UNAVAILABLE_NV = 0x936A;
        public const uint GL_FONT_UNINTELLIGIBLE_NV = 0x936B;
        public const uint GL_CONIC_CURVE_TO_NV = 0x1A;
        public const uint GL_RELATIVE_CONIC_CURVE_TO_NV = 0x1B;
        public const uint GL_FONT_NUM_GLYPH_INDICES_BIT_NV = 0x20000000;
        public const uint GL_STANDARD_FONT_FORMAT_NV = 0x936C;
        public const uint GL_PATH_PROJECTION_NV = 0x1701;
        public const uint GL_PATH_MODELVIEW_NV = 0x1700;
        public const uint GL_PATH_MODELVIEW_STACK_DEPTH_NV = 0x0BA3;
        public const uint GL_PATH_MODELVIEW_MATRIX_NV = 0x0BA6;
        public const uint GL_PATH_MAX_MODELVIEW_STACK_DEPTH_NV = 0x0D36;
        public const uint GL_PATH_TRANSPOSE_MODELVIEW_MATRIX_NV = 0x84E3;
        public const uint GL_PATH_PROJECTION_STACK_DEPTH_NV = 0x0BA4;
        public const uint GL_PATH_PROJECTION_MATRIX_NV = 0x0BA7;
        public const uint GL_PATH_MAX_PROJECTION_STACK_DEPTH_NV = 0x0D38;
        public const uint GL_PATH_TRANSPOSE_PROJECTION_MATRIX_NV = 0x84E4;
        public const uint GL_FRAGMENT_INPUT_NV = 0x936D;

        private static delegate* unmanaged[Stdcall]<int, uint> pfn_glGenPathsNV = null;
        /// <summary> <see href="docs.gl/gl4/glGenPathsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glGenPathsNV(int range) => pfn_glGenPathsNV(range);

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glDeletePathsNV = null;
        /// <summary> <see href="docs.gl/gl4/glDeletePathsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glDeletePathsNV(uint path, int range) => pfn_glDeletePathsNV(path, range);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsPathNV(uint path) => pfn_glIsPathNV(path);

        private static delegate* unmanaged[Stdcall]<uint, int, byte*, int, uint, void*, void> pfn_glPathCommandsNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathCommandsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathCommandsNV(uint path, int numCommands, byte* commands, int numCoords, uint coordType, void* coords) => pfn_glPathCommandsNV(path, numCommands, commands, numCoords, coordType, coords);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, void> pfn_glPathCoordsNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathCoordsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathCoordsNV(uint path, int numCoords, uint coordType, void* coords) => pfn_glPathCoordsNV(path, numCoords, coordType, coords);

        private static delegate* unmanaged[Stdcall]<uint, int, int, int, byte*, int, uint, void*, void> pfn_glPathSubCommandsNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathSubCommandsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathSubCommandsNV(uint path, int commandStart, int commandsToDelete, int numCommands, byte* commands, int numCoords, uint coordType, void* coords) => pfn_glPathSubCommandsNV(path, commandStart, commandsToDelete, numCommands, commands, numCoords, coordType, coords);

        private static delegate* unmanaged[Stdcall]<uint, int, int, uint, void*, void> pfn_glPathSubCoordsNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathSubCoordsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathSubCoordsNV(uint path, int coordStart, int numCoords, uint coordType, void* coords) => pfn_glPathSubCoordsNV(path, coordStart, numCoords, coordType, coords);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void*, void> pfn_glPathStringNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathStringNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathStringNV(uint path, uint format, int length, void* pathString) => pfn_glPathStringNV(path, format, length, pathString);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, uint, int, uint, void*, uint, uint, float, void> pfn_glPathGlyphsNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathGlyphsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathGlyphsNV(uint firstPathName, uint fontTarget, void* fontName, uint fontStyle, int numGlyphs, uint type, void* charcodes, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale) => pfn_glPathGlyphsNV(firstPathName, fontTarget, fontName, fontStyle, numGlyphs, type, charcodes, handleMissingGlyphs, pathParameterTemplate, emScale);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, uint, uint, int, uint, uint, float, void> pfn_glPathGlyphRangeNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathGlyphRangeNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathGlyphRangeNV(uint firstPathName, uint fontTarget, void* fontName, uint fontStyle, uint firstGlyph, int numGlyphs, uint handleMissingGlyphs, uint pathParameterTemplate, float emScale) => pfn_glPathGlyphRangeNV(firstPathName, fontTarget, fontName, fontStyle, firstGlyph, numGlyphs, handleMissingGlyphs, pathParameterTemplate, emScale);

        private static delegate* unmanaged[Stdcall]<uint, int, uint*, float*, void> pfn_glWeightPathsNV = null;
        /// <summary> <see href="docs.gl/gl4/glWeightPathsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glWeightPathsNV(uint resultPath, int numPaths, uint* paths, float* weights) => pfn_glWeightPathsNV(resultPath, numPaths, paths, weights);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glCopyPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glCopyPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCopyPathNV(uint resultPath, uint srcPath) => pfn_glCopyPathNV(resultPath, srcPath);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float, void> pfn_glInterpolatePathsNV = null;
        /// <summary> <see href="docs.gl/gl4/glInterpolatePathsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glInterpolatePathsNV(uint resultPath, uint pathA, uint pathB, float weight) => pfn_glInterpolatePathsNV(resultPath, pathA, pathB, weight);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, float*, void> pfn_glTransformPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glTransformPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTransformPathNV(uint resultPath, uint srcPath, uint transformType, float* transformValues) => pfn_glTransformPathNV(resultPath, srcPath, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glPathParameterivNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathParameterivNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathParameterivNV(uint path, uint pname, int* value) => pfn_glPathParameterivNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, void> pfn_glPathParameteriNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathParameteriNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathParameteriNV(uint path, uint pname, int value) => pfn_glPathParameteriNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glPathParameterfvNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathParameterfvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathParameterfvNV(uint path, uint pname, float* value) => pfn_glPathParameterfvNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, float, void> pfn_glPathParameterfNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathParameterfNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathParameterfNV(uint path, uint pname, float value) => pfn_glPathParameterfNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, int, float*, void> pfn_glPathDashArrayNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathDashArrayNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathDashArrayNV(uint path, int dashCount, float* dashArray) => pfn_glPathDashArrayNV(path, dashCount, dashArray);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void> pfn_glPathStencilFuncNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathStencilFuncNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathStencilFuncNV(uint func, int @ref, uint mask) => pfn_glPathStencilFuncNV(func, @ref, mask);

        private static delegate* unmanaged[Stdcall]<float, float, void> pfn_glPathStencilDepthOffsetNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathStencilDepthOffsetNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathStencilDepthOffsetNV(float factor, float units) => pfn_glPathStencilDepthOffsetNV(factor, units);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, void> pfn_glStencilFillPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilFillPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilFillPathNV(uint path, uint fillMode, uint mask) => pfn_glStencilFillPathNV(path, fillMode, mask);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void> pfn_glStencilStrokePathNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilStrokePathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilStrokePathNV(uint path, int reference, uint mask) => pfn_glStencilStrokePathNV(path, reference, mask);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, uint, uint, uint, float*, void> pfn_glStencilFillPathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilFillPathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilFillPathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, uint fillMode, uint mask, uint transformType, float* transformValues) => pfn_glStencilFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, fillMode, mask, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, int, uint, uint, float*, void> pfn_glStencilStrokePathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilStrokePathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilStrokePathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, int reference, uint mask, uint transformType, float* transformValues) => pfn_glStencilStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, reference, mask, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glPathCoverDepthFuncNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathCoverDepthFuncNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glPathCoverDepthFuncNV(uint func) => pfn_glPathCoverDepthFuncNV(func);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glCoverFillPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverFillPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverFillPathNV(uint path, uint coverMode) => pfn_glCoverFillPathNV(path, coverMode);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glCoverStrokePathNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverStrokePathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverStrokePathNV(uint path, uint coverMode) => pfn_glCoverStrokePathNV(path, coverMode);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, uint, uint, float*, void> pfn_glCoverFillPathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverFillPathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverFillPathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, uint coverMode, uint transformType, float* transformValues) => pfn_glCoverFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, coverMode, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, uint, uint, float*, void> pfn_glCoverStrokePathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glCoverStrokePathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glCoverStrokePathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, uint coverMode, uint transformType, float* transformValues) => pfn_glCoverStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, coverMode, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glGetPathParameterivNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathParameterivNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathParameterivNV(uint path, uint pname, int* value) => pfn_glGetPathParameterivNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, uint, float*, void> pfn_glGetPathParameterfvNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathParameterfvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathParameterfvNV(uint path, uint pname, float* value) => pfn_glGetPathParameterfvNV(path, pname, value);

        private static delegate* unmanaged[Stdcall]<uint, byte*, void> pfn_glGetPathCommandsNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathCommandsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathCommandsNV(uint path, byte* commands) => pfn_glGetPathCommandsNV(path, commands);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glGetPathCoordsNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathCoordsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathCoordsNV(uint path, float* coords) => pfn_glGetPathCoordsNV(path, coords);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glGetPathDashArrayNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathDashArrayNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathDashArrayNV(uint path, float* dashArray) => pfn_glGetPathDashArrayNV(path, dashArray);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, uint, int, float*, void> pfn_glGetPathMetricsNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathMetricsNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathMetricsNV(uint metricQueryMask, int numPaths, uint pathNameType, void* paths, uint pathBase, int stride, float* metrics) => pfn_glGetPathMetricsNV(metricQueryMask, numPaths, pathNameType, paths, pathBase, stride, metrics);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, int, float*, void> pfn_glGetPathMetricRangeNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathMetricRangeNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathMetricRangeNV(uint metricQueryMask, uint firstPathName, int numPaths, int stride, float* metrics) => pfn_glGetPathMetricRangeNV(metricQueryMask, firstPathName, numPaths, stride, metrics);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, void*, uint, float, float, uint, float*, void> pfn_glGetPathSpacingNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathSpacingNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetPathSpacingNV(uint pathListMode, int numPaths, uint pathNameType, void* paths, uint pathBase, float advanceScale, float kerningScale, uint transformType, float* returnedSpacing) => pfn_glGetPathSpacingNV(pathListMode, numPaths, pathNameType, paths, pathBase, advanceScale, kerningScale, transformType, returnedSpacing);

        private static delegate* unmanaged[Stdcall]<uint, uint, float, float, byte> pfn_glIsPointInFillPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsPointInFillPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsPointInFillPathNV(uint path, uint mask, float x, float y) => pfn_glIsPointInFillPathNV(path, mask, x, y);

        private static delegate* unmanaged[Stdcall]<uint, float, float, byte> pfn_glIsPointInStrokePathNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsPointInStrokePathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsPointInStrokePathNV(uint path, float x, float y) => pfn_glIsPointInStrokePathNV(path, x, y);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float> pfn_glGetPathLengthNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetPathLengthNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static float glGetPathLengthNV(uint path, int startSegment, int numSegments) => pfn_glGetPathLengthNV(path, startSegment, numSegments);

        private static delegate* unmanaged[Stdcall]<uint, int, int, float, float*, float*, float*, float*, byte> pfn_glPointAlongPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glPointAlongPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glPointAlongPathNV(uint path, int startSegment, int numSegments, float distance, float* x, float* y, float* tangentX, float* tangentY) => pfn_glPointAlongPathNV(path, startSegment, numSegments, distance, x, y, tangentX, tangentY);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixLoad3x2fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoad3x2fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoad3x2fNV(uint matrixMode, float* m) => pfn_glMatrixLoad3x2fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixLoad3x3fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoad3x3fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoad3x3fNV(uint matrixMode, float* m) => pfn_glMatrixLoad3x3fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixLoadTranspose3x3fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixLoadTranspose3x3fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixLoadTranspose3x3fNV(uint matrixMode, float* m) => pfn_glMatrixLoadTranspose3x3fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixMult3x2fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMult3x2fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMult3x2fNV(uint matrixMode, float* m) => pfn_glMatrixMult3x2fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixMult3x3fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMult3x3fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMult3x3fNV(uint matrixMode, float* m) => pfn_glMatrixMult3x3fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, float*, void> pfn_glMatrixMultTranspose3x3fNV = null;
        /// <summary> <see href="docs.gl/gl4/glMatrixMultTranspose3x3fNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMatrixMultTranspose3x3fNV(uint matrixMode, float* m) => pfn_glMatrixMultTranspose3x3fNV(matrixMode, m);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void> pfn_glStencilThenCoverFillPathNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilThenCoverFillPathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilThenCoverFillPathNV(uint path, uint fillMode, uint mask, uint coverMode) => pfn_glStencilThenCoverFillPathNV(path, fillMode, mask, coverMode);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, uint, void> pfn_glStencilThenCoverStrokePathNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilThenCoverStrokePathNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilThenCoverStrokePathNV(uint path, int reference, uint mask, uint coverMode) => pfn_glStencilThenCoverStrokePathNV(path, reference, mask, coverMode);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, uint, uint, uint, uint, float*, void> pfn_glStencilThenCoverFillPathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilThenCoverFillPathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilThenCoverFillPathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, uint fillMode, uint mask, uint coverMode, uint transformType, float* transformValues) => pfn_glStencilThenCoverFillPathInstancedNV(numPaths, pathNameType, paths, pathBase, fillMode, mask, coverMode, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<int, uint, void*, uint, int, uint, uint, uint, float*, void> pfn_glStencilThenCoverStrokePathInstancedNV = null;
        /// <summary> <see href="docs.gl/gl4/glStencilThenCoverStrokePathInstancedNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glStencilThenCoverStrokePathInstancedNV(int numPaths, uint pathNameType, void* paths, uint pathBase, int reference, uint mask, uint coverMode, uint transformType, float* transformValues) => pfn_glStencilThenCoverStrokePathInstancedNV(numPaths, pathNameType, paths, pathBase, reference, mask, coverMode, transformType, transformValues);

        private static delegate* unmanaged[Stdcall]<uint, void*, uint, uint, float, uint*, uint> pfn_glPathGlyphIndexRangeNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathGlyphIndexRangeNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glPathGlyphIndexRangeNV(uint fontTarget, void* fontName, uint fontStyle, uint pathParameterTemplate, float emScale, uint* baseAndCount) => pfn_glPathGlyphIndexRangeNV(fontTarget, fontName, fontStyle, pathParameterTemplate, emScale, baseAndCount);

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, uint, uint, int, uint, float, uint> pfn_glPathGlyphIndexArrayNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathGlyphIndexArrayNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glPathGlyphIndexArrayNV(uint firstPathName, uint fontTarget, void* fontName, uint fontStyle, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale) => pfn_glPathGlyphIndexArrayNV(firstPathName, fontTarget, fontName, fontStyle, firstGlyphIndex, numGlyphs, pathParameterTemplate, emScale);

        private static delegate* unmanaged[Stdcall]<uint, uint, long, void*, int, uint, int, uint, float, uint> pfn_glPathMemoryGlyphIndexArrayNV = null;
        /// <summary> <see href="docs.gl/gl4/glPathMemoryGlyphIndexArrayNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint glPathMemoryGlyphIndexArrayNV(uint firstPathName, uint fontTarget, long fontSize, void* fontData, int faceIndex, uint firstGlyphIndex, int numGlyphs, uint pathParameterTemplate, float emScale) => pfn_glPathMemoryGlyphIndexArrayNV(firstPathName, fontTarget, fontSize, fontData, faceIndex, firstGlyphIndex, numGlyphs, pathParameterTemplate, emScale);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, int, float*, void> pfn_glProgramPathFragmentInputGenNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramPathFragmentInputGenNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramPathFragmentInputGenNV(uint program, int location, uint genMode, int components, float* coeffs) => pfn_glProgramPathFragmentInputGenNV(program, location, genMode, components, coeffs);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, int, int*, float*, void> pfn_glGetProgramResourcefvNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetProgramResourcefvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetProgramResourcefvNV(uint program, uint programInterface, uint index, int propCount, uint* props, int count, int* length, float* @params) => pfn_glGetProgramResourcefvNV(program, programInterface, index, propCount, props, count, length, @params);
    }

    public static unsafe class GLNVPathRenderingSharedEdge
    {
        static GLNVPathRenderingSharedEdge() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_path_rendering_shared_edge") ?? false) _GL_NV_path_rendering_shared_edge = 1;
        }
        private static uint _GL_NV_path_rendering_shared_edge = 0;
        public static uint GL_NV_path_rendering_shared_edge => _GL_NV_path_rendering_shared_edge;
        public const uint GL_SHARED_EDGE_NV = 0xC0;
    }

    public static unsafe class GLNVPrimitiveShadingRate
    {
        static GLNVPrimitiveShadingRate() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_primitive_shading_rate") ?? false) _GL_NV_primitive_shading_rate = 1;
        }
        private static uint _GL_NV_primitive_shading_rate = 0;
        public static uint GL_NV_primitive_shading_rate => _GL_NV_primitive_shading_rate;
        public const uint GL_SHADING_RATE_IMAGE_PER_PRIMITIVE_NV = 0x95B1;
        public const uint GL_SHADING_RATE_IMAGE_PALETTE_COUNT_NV = 0x95B2;
    }

    public static unsafe class GLNVRepresentativeFragmentTest
    {
        static GLNVRepresentativeFragmentTest() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_representative_fragment_test") ?? false) _GL_NV_representative_fragment_test = 1;
        }
        private static uint _GL_NV_representative_fragment_test = 0;
        public static uint GL_NV_representative_fragment_test => _GL_NV_representative_fragment_test;
        public const uint GL_REPRESENTATIVE_FRAGMENT_TEST_NV = 0x937F;
    }

    public static unsafe class GLNVSampleLocations
    {
        static GLNVSampleLocations() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_sample_locations") ?? false) _GL_NV_sample_locations = 1;
        }
        private static uint _GL_NV_sample_locations = 0;
        public static uint GL_NV_sample_locations => _GL_NV_sample_locations;
        public const uint GL_SAMPLE_LOCATION_SUBPIXEL_BITS_NV = 0x933D;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_WIDTH_NV = 0x933E;
        public const uint GL_SAMPLE_LOCATION_PIXEL_GRID_HEIGHT_NV = 0x933F;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_TABLE_SIZE_NV = 0x9340;
        public const uint GL_SAMPLE_LOCATION_NV = 0x8E50;
        public const uint GL_PROGRAMMABLE_SAMPLE_LOCATION_NV = 0x9341;
        public const uint GL_FRAMEBUFFER_PROGRAMMABLE_SAMPLE_LOCATIONS_NV = 0x9342;
        public const uint GL_FRAMEBUFFER_SAMPLE_LOCATION_PIXEL_GRID_NV = 0x9343;

        private static delegate* unmanaged[Stdcall]<uint, uint, int, float*, void> pfn_glFramebufferSampleLocationsfvNV = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferSampleLocationsfvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferSampleLocationsfvNV(uint target, uint start, int count, float* v) => pfn_glFramebufferSampleLocationsfvNV(target, start, count, v);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, float*, void> pfn_glNamedFramebufferSampleLocationsfvNV = null;
        /// <summary> <see href="docs.gl/gl4/glNamedFramebufferSampleLocationsfvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNamedFramebufferSampleLocationsfvNV(uint framebuffer, uint start, int count, float* v) => pfn_glNamedFramebufferSampleLocationsfvNV(framebuffer, start, count, v);

        private static delegate* unmanaged[Stdcall]<void> pfn_glResolveDepthValuesNV = null;
        /// <summary> <see href="docs.gl/gl4/glResolveDepthValuesNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glResolveDepthValuesNV() => pfn_glResolveDepthValuesNV();
    }

    public static unsafe class GLNVSampleMaskOverrideCoverage
    {
        static GLNVSampleMaskOverrideCoverage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_sample_mask_override_coverage") ?? false) _GL_NV_sample_mask_override_coverage = 1;
        }
        private static uint _GL_NV_sample_mask_override_coverage = 0;
        public static uint GL_NV_sample_mask_override_coverage => _GL_NV_sample_mask_override_coverage;
    }

    public static unsafe class GLNVScissorExclusive
    {
        static GLNVScissorExclusive() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_scissor_exclusive") ?? false) _GL_NV_scissor_exclusive = 1;
        }
        private static uint _GL_NV_scissor_exclusive = 0;
        public static uint GL_NV_scissor_exclusive => _GL_NV_scissor_exclusive;
        public const uint GL_SCISSOR_TEST_EXCLUSIVE_NV = 0x9555;
        public const uint GL_SCISSOR_BOX_EXCLUSIVE_NV = 0x9556;

        private static delegate* unmanaged[Stdcall]<int, int, int, int, void> pfn_glScissorExclusiveNV = null;
        /// <summary> <see href="docs.gl/gl4/glScissorExclusiveNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glScissorExclusiveNV(int x, int y, int width, int height) => pfn_glScissorExclusiveNV(x, y, width, height);

        private static delegate* unmanaged[Stdcall]<uint, int, int*, void> pfn_glScissorExclusiveArrayvNV = null;
        /// <summary> <see href="docs.gl/gl4/glScissorExclusiveArrayvNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glScissorExclusiveArrayvNV(uint first, int count, int* v) => pfn_glScissorExclusiveArrayvNV(first, count, v);
    }

    public static unsafe class GLNVShaderAtomicCounters
    {
        static GLNVShaderAtomicCounters() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_atomic_counters") ?? false) _GL_NV_shader_atomic_counters = 1;
        }
        private static uint _GL_NV_shader_atomic_counters = 0;
        public static uint GL_NV_shader_atomic_counters => _GL_NV_shader_atomic_counters;
    }

    public static unsafe class GLNVShaderAtomicFloat
    {
        static GLNVShaderAtomicFloat() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_atomic_float") ?? false) _GL_NV_shader_atomic_float = 1;
        }
        private static uint _GL_NV_shader_atomic_float = 0;
        public static uint GL_NV_shader_atomic_float => _GL_NV_shader_atomic_float;
    }

    public static unsafe class GLNVShaderAtomicFloat64
    {
        static GLNVShaderAtomicFloat64() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_atomic_float64") ?? false) _GL_NV_shader_atomic_float64 = 1;
        }
        private static uint _GL_NV_shader_atomic_float64 = 0;
        public static uint GL_NV_shader_atomic_float64 => _GL_NV_shader_atomic_float64;
    }

    public static unsafe class GLNVShaderAtomicFp16Vector
    {
        static GLNVShaderAtomicFp16Vector() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_atomic_fp16_vector") ?? false) _GL_NV_shader_atomic_fp16_vector = 1;
        }
        private static uint _GL_NV_shader_atomic_fp16_vector = 0;
        public static uint GL_NV_shader_atomic_fp16_vector => _GL_NV_shader_atomic_fp16_vector;
    }

    public static unsafe class GLNVShaderAtomicInt64
    {
        static GLNVShaderAtomicInt64() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_atomic_int64") ?? false) _GL_NV_shader_atomic_int64 = 1;
        }
        private static uint _GL_NV_shader_atomic_int64 = 0;
        public static uint GL_NV_shader_atomic_int64 => _GL_NV_shader_atomic_int64;
    }

    public static unsafe class GLNVShaderBufferLoad
    {
        static GLNVShaderBufferLoad() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_buffer_load") ?? false) _GL_NV_shader_buffer_load = 1;
        }
        private static uint _GL_NV_shader_buffer_load = 0;
        public static uint GL_NV_shader_buffer_load => _GL_NV_shader_buffer_load;
        public const uint GL_BUFFER_GPU_ADDRESS_NV = 0x8F1D;
        public const uint GL_GPU_ADDRESS_NV = 0x8F34;
        public const uint GL_MAX_SHADER_BUFFER_ADDRESS_NV = 0x8F35;

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glMakeBufferResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeBufferResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeBufferResidentNV(uint target, uint access) => pfn_glMakeBufferResidentNV(target, access);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMakeBufferNonResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeBufferNonResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeBufferNonResidentNV(uint target) => pfn_glMakeBufferNonResidentNV(target);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsBufferResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsBufferResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsBufferResidentNV(uint target) => pfn_glIsBufferResidentNV(target);

        private static delegate* unmanaged[Stdcall]<uint, uint, void> pfn_glMakeNamedBufferResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeNamedBufferResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeNamedBufferResidentNV(uint buffer, uint access) => pfn_glMakeNamedBufferResidentNV(buffer, access);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glMakeNamedBufferNonResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glMakeNamedBufferNonResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glMakeNamedBufferNonResidentNV(uint buffer) => pfn_glMakeNamedBufferNonResidentNV(buffer);

        private static delegate* unmanaged[Stdcall]<uint, byte> pfn_glIsNamedBufferResidentNV = null;
        /// <summary> <see href="docs.gl/gl4/glIsNamedBufferResidentNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte glIsNamedBufferResidentNV(uint buffer) => pfn_glIsNamedBufferResidentNV(buffer);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetBufferParameterui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetBufferParameterui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetBufferParameterui64vNV(uint target, uint pname, void** @params) => pfn_glGetBufferParameterui64vNV(target, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetNamedBufferParameterui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetNamedBufferParameterui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetNamedBufferParameterui64vNV(uint buffer, uint pname, void** @params) => pfn_glGetNamedBufferParameterui64vNV(buffer, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glGetIntegerui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetIntegerui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetIntegerui64vNV(uint value, void** result) => pfn_glGetIntegerui64vNV(value, result);

        private static delegate* unmanaged[Stdcall]<int, void*, void> pfn_glUniformui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glUniformui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformui64NV(int location, void* value) => pfn_glUniformui64NV(location, value);

        private static delegate* unmanaged[Stdcall]<int, int, void**, void> pfn_glUniformui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glUniformui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glUniformui64vNV(int location, int count, void** value) => pfn_glUniformui64vNV(location, count, value);

        private static delegate* unmanaged[Stdcall]<uint, int, void**, void> pfn_glGetUniformui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetUniformui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetUniformui64vNV(uint program, int location, void** @params) => pfn_glGetUniformui64vNV(program, location, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, void*, void> pfn_glProgramUniformui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformui64NV(uint program, int location, void* value) => pfn_glProgramUniformui64NV(program, location, value);

        private static delegate* unmanaged[Stdcall]<uint, int, int, void**, void> pfn_glProgramUniformui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glProgramUniformui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glProgramUniformui64vNV(uint program, int location, int count, void** value) => pfn_glProgramUniformui64vNV(program, location, count, value);
    }

    public static unsafe class GLNVShaderBufferStore
    {
        static GLNVShaderBufferStore() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_buffer_store") ?? false) _GL_NV_shader_buffer_store = 1;
        }
        private static uint _GL_NV_shader_buffer_store = 0;
        public static uint GL_NV_shader_buffer_store => _GL_NV_shader_buffer_store;
        public const uint GL_SHADER_GLOBAL_ACCESS_BARRIER_BIT_NV = 0x00000010;
    }

    public static unsafe class GLNVShaderSubgroupPartitioned
    {
        static GLNVShaderSubgroupPartitioned() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_subgroup_partitioned") ?? false) _GL_NV_shader_subgroup_partitioned = 1;
        }
        private static uint _GL_NV_shader_subgroup_partitioned = 0;
        public static uint GL_NV_shader_subgroup_partitioned => _GL_NV_shader_subgroup_partitioned;
        public const uint GL_SUBGROUP_FEATURE_PARTITIONED_BIT_NV = 0x00000100;
    }

    public static unsafe class GLNVShaderTextureFootprint
    {
        static GLNVShaderTextureFootprint() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_texture_footprint") ?? false) _GL_NV_shader_texture_footprint = 1;
        }
        private static uint _GL_NV_shader_texture_footprint = 0;
        public static uint GL_NV_shader_texture_footprint => _GL_NV_shader_texture_footprint;
    }

    public static unsafe class GLNVShaderThreadGroup
    {
        static GLNVShaderThreadGroup() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_thread_group") ?? false) _GL_NV_shader_thread_group = 1;
        }
        private static uint _GL_NV_shader_thread_group = 0;
        public static uint GL_NV_shader_thread_group => _GL_NV_shader_thread_group;
        public const uint GL_WARP_SIZE_NV = 0x9339;
        public const uint GL_WARPS_PER_SM_NV = 0x933A;
        public const uint GL_SM_COUNT_NV = 0x933B;
    }

    public static unsafe class GLNVShaderThreadShuffle
    {
        static GLNVShaderThreadShuffle() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shader_thread_shuffle") ?? false) _GL_NV_shader_thread_shuffle = 1;
        }
        private static uint _GL_NV_shader_thread_shuffle = 0;
        public static uint GL_NV_shader_thread_shuffle => _GL_NV_shader_thread_shuffle;
    }

    public static unsafe class GLNVShadingRateImage
    {
        static GLNVShadingRateImage() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_shading_rate_image") ?? false) _GL_NV_shading_rate_image = 1;
        }
        private static uint _GL_NV_shading_rate_image = 0;
        public static uint GL_NV_shading_rate_image => _GL_NV_shading_rate_image;
        public const uint GL_SHADING_RATE_IMAGE_NV = 0x9563;
        public const uint GL_SHADING_RATE_NO_INVOCATIONS_NV = 0x9564;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_PIXEL_NV = 0x9565;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_1X2_PIXELS_NV = 0x9566;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_2X1_PIXELS_NV = 0x9567;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_2X2_PIXELS_NV = 0x9568;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_2X4_PIXELS_NV = 0x9569;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_4X2_PIXELS_NV = 0x956A;
        public const uint GL_SHADING_RATE_1_INVOCATION_PER_4X4_PIXELS_NV = 0x956B;
        public const uint GL_SHADING_RATE_2_INVOCATIONS_PER_PIXEL_NV = 0x956C;
        public const uint GL_SHADING_RATE_4_INVOCATIONS_PER_PIXEL_NV = 0x956D;
        public const uint GL_SHADING_RATE_8_INVOCATIONS_PER_PIXEL_NV = 0x956E;
        public const uint GL_SHADING_RATE_16_INVOCATIONS_PER_PIXEL_NV = 0x956F;
        public const uint GL_SHADING_RATE_IMAGE_BINDING_NV = 0x955B;
        public const uint GL_SHADING_RATE_IMAGE_TEXEL_WIDTH_NV = 0x955C;
        public const uint GL_SHADING_RATE_IMAGE_TEXEL_HEIGHT_NV = 0x955D;
        public const uint GL_SHADING_RATE_IMAGE_PALETTE_SIZE_NV = 0x955E;
        public const uint GL_MAX_COARSE_FRAGMENT_SAMPLES_NV = 0x955F;
        public const uint GL_SHADING_RATE_SAMPLE_ORDER_DEFAULT_NV = 0x95AE;
        public const uint GL_SHADING_RATE_SAMPLE_ORDER_PIXEL_MAJOR_NV = 0x95AF;
        public const uint GL_SHADING_RATE_SAMPLE_ORDER_SAMPLE_MAJOR_NV = 0x95B0;

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glBindShadingRateImageNV = null;
        /// <summary> <see href="docs.gl/gl4/glBindShadingRateImageNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBindShadingRateImageNV(uint texture) => pfn_glBindShadingRateImageNV(texture);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint*, void> pfn_glGetShadingRateImagePaletteNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetShadingRateImagePaletteNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetShadingRateImagePaletteNV(uint viewport, uint entry, uint* rate) => pfn_glGetShadingRateImagePaletteNV(viewport, entry, rate);

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void> pfn_glGetShadingRateSampleLocationivNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetShadingRateSampleLocationivNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetShadingRateSampleLocationivNV(uint rate, uint samples, uint index, int* location) => pfn_glGetShadingRateSampleLocationivNV(rate, samples, index, location);

        private static delegate* unmanaged[Stdcall]<byte, void> pfn_glShadingRateImageBarrierNV = null;
        /// <summary> <see href="docs.gl/gl4/glShadingRateImageBarrierNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glShadingRateImageBarrierNV(byte synchronize) => pfn_glShadingRateImageBarrierNV(synchronize);

        private static delegate* unmanaged[Stdcall]<uint, uint, int, uint*, void> pfn_glShadingRateImagePaletteNV = null;
        /// <summary> <see href="docs.gl/gl4/glShadingRateImagePaletteNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glShadingRateImagePaletteNV(uint viewport, uint first, int count, uint* rates) => pfn_glShadingRateImagePaletteNV(viewport, first, count, rates);

        private static delegate* unmanaged[Stdcall]<uint, void> pfn_glShadingRateSampleOrderNV = null;
        /// <summary> <see href="docs.gl/gl4/glShadingRateSampleOrderNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glShadingRateSampleOrderNV(uint order) => pfn_glShadingRateSampleOrderNV(order);

        private static delegate* unmanaged[Stdcall]<uint, uint, int*, void> pfn_glShadingRateSampleOrderCustomNV = null;
        /// <summary> <see href="docs.gl/gl4/glShadingRateSampleOrderCustomNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glShadingRateSampleOrderCustomNV(uint rate, uint samples, int* locations) => pfn_glShadingRateSampleOrderCustomNV(rate, samples, locations);
    }

    public static unsafe class GLNVStereoViewRendering
    {
        static GLNVStereoViewRendering() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_stereo_view_rendering") ?? false) _GL_NV_stereo_view_rendering = 1;
        }
        private static uint _GL_NV_stereo_view_rendering = 0;
        public static uint GL_NV_stereo_view_rendering => _GL_NV_stereo_view_rendering;
    }

    public static unsafe class GLNVTextureBarrier
    {
        static GLNVTextureBarrier() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_texture_barrier") ?? false) _GL_NV_texture_barrier = 1;
        }
        private static uint _GL_NV_texture_barrier = 0;
        public static uint GL_NV_texture_barrier => _GL_NV_texture_barrier;

        private static delegate* unmanaged[Stdcall]<void> pfn_glTextureBarrierNV = null;
        /// <summary> <see href="docs.gl/gl4/glTextureBarrierNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTextureBarrierNV() => pfn_glTextureBarrierNV();
    }

    public static unsafe class GLNVTextureRectangleCompressed
    {
        static GLNVTextureRectangleCompressed() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_texture_rectangle_compressed") ?? false) _GL_NV_texture_rectangle_compressed = 1;
        }
        private static uint _GL_NV_texture_rectangle_compressed = 0;
        public static uint GL_NV_texture_rectangle_compressed => _GL_NV_texture_rectangle_compressed;
    }

    public static unsafe class GLNVUniformBufferUnifiedMemory
    {
        static GLNVUniformBufferUnifiedMemory() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_uniform_buffer_unified_memory") ?? false) _GL_NV_uniform_buffer_unified_memory = 1;
        }
        private static uint _GL_NV_uniform_buffer_unified_memory = 0;
        public static uint GL_NV_uniform_buffer_unified_memory => _GL_NV_uniform_buffer_unified_memory;
        public const uint GL_UNIFORM_BUFFER_UNIFIED_NV = 0x936E;
        public const uint GL_UNIFORM_BUFFER_ADDRESS_NV = 0x936F;
        public const uint GL_UNIFORM_BUFFER_LENGTH_NV = 0x9370;
    }

    public static unsafe class GLNVVertexAttribInteger64bit
    {
        static GLNVVertexAttribInteger64bit() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_vertex_attrib_integer_64bit") ?? false) _GL_NV_vertex_attrib_integer_64bit = 1;
        }
        private static uint _GL_NV_vertex_attrib_integer_64bit = 0;
        public static uint GL_NV_vertex_attrib_integer_64bit => _GL_NV_vertex_attrib_integer_64bit;

        private static delegate* unmanaged[Stdcall]<uint, long, void> pfn_glVertexAttribL1i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1i64NV(uint index, long x) => pfn_glVertexAttribL1i64NV(index, x);

        private static delegate* unmanaged[Stdcall]<uint, long, long, void> pfn_glVertexAttribL2i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL2i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL2i64NV(uint index, long x, long y) => pfn_glVertexAttribL2i64NV(index, x, y);

        private static delegate* unmanaged[Stdcall]<uint, long, long, long, void> pfn_glVertexAttribL3i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL3i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL3i64NV(uint index, long x, long y, long z) => pfn_glVertexAttribL3i64NV(index, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, long, long, long, long, void> pfn_glVertexAttribL4i64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL4i64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL4i64NV(uint index, long x, long y, long z, long w) => pfn_glVertexAttribL4i64NV(index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, long*, void> pfn_glVertexAttribL1i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1i64vNV(uint index, long* v) => pfn_glVertexAttribL1i64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, long*, void> pfn_glVertexAttribL2i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL2i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL2i64vNV(uint index, long* v) => pfn_glVertexAttribL2i64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, long*, void> pfn_glVertexAttribL3i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL3i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL3i64vNV(uint index, long* v) => pfn_glVertexAttribL3i64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, long*, void> pfn_glVertexAttribL4i64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL4i64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL4i64vNV(uint index, long* v) => pfn_glVertexAttribL4i64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, void*, void> pfn_glVertexAttribL1ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1ui64NV(uint index, void* x) => pfn_glVertexAttribL1ui64NV(index, x);

        private static delegate* unmanaged[Stdcall]<uint, void*, void*, void> pfn_glVertexAttribL2ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL2ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL2ui64NV(uint index, void* x, void* y) => pfn_glVertexAttribL2ui64NV(index, x, y);

        private static delegate* unmanaged[Stdcall]<uint, void*, void*, void*, void> pfn_glVertexAttribL3ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL3ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL3ui64NV(uint index, void* x, void* y, void* z) => pfn_glVertexAttribL3ui64NV(index, x, y, z);

        private static delegate* unmanaged[Stdcall]<uint, void*, void*, void*, void*, void> pfn_glVertexAttribL4ui64NV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL4ui64NV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL4ui64NV(uint index, void* x, void* y, void* z, void* w) => pfn_glVertexAttribL4ui64NV(index, x, y, z, w);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glVertexAttribL1ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL1ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL1ui64vNV(uint index, void** v) => pfn_glVertexAttribL1ui64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glVertexAttribL2ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL2ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL2ui64vNV(uint index, void** v) => pfn_glVertexAttribL2ui64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glVertexAttribL3ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL3ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL3ui64vNV(uint index, void** v) => pfn_glVertexAttribL3ui64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, void**, void> pfn_glVertexAttribL4ui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribL4ui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribL4ui64vNV(uint index, void** v) => pfn_glVertexAttribL4ui64vNV(index, v);

        private static delegate* unmanaged[Stdcall]<uint, uint, long*, void> pfn_glGetVertexAttribLi64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexAttribLi64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexAttribLi64vNV(uint index, uint pname, long* @params) => pfn_glGetVertexAttribLi64vNV(index, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetVertexAttribLui64vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetVertexAttribLui64vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetVertexAttribLui64vNV(uint index, uint pname, void** @params) => pfn_glGetVertexAttribLui64vNV(index, pname, @params);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void> pfn_glVertexAttribLFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribLFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribLFormatNV(uint index, int size, uint type, int stride) => pfn_glVertexAttribLFormatNV(index, size, type, stride);
    }

    public static unsafe class GLNVVertexBufferUnifiedMemory
    {
        static GLNVVertexBufferUnifiedMemory() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_vertex_buffer_unified_memory") ?? false) _GL_NV_vertex_buffer_unified_memory = 1;
        }
        private static uint _GL_NV_vertex_buffer_unified_memory = 0;
        public static uint GL_NV_vertex_buffer_unified_memory => _GL_NV_vertex_buffer_unified_memory;
        public const uint GL_VERTEX_ATTRIB_ARRAY_UNIFIED_NV = 0x8F1E;
        public const uint GL_ELEMENT_ARRAY_UNIFIED_NV = 0x8F1F;
        public const uint GL_VERTEX_ATTRIB_ARRAY_ADDRESS_NV = 0x8F20;
        public const uint GL_VERTEX_ARRAY_ADDRESS_NV = 0x8F21;
        public const uint GL_NORMAL_ARRAY_ADDRESS_NV = 0x8F22;
        public const uint GL_COLOR_ARRAY_ADDRESS_NV = 0x8F23;
        public const uint GL_INDEX_ARRAY_ADDRESS_NV = 0x8F24;
        public const uint GL_TEXTURE_COORD_ARRAY_ADDRESS_NV = 0x8F25;
        public const uint GL_EDGE_FLAG_ARRAY_ADDRESS_NV = 0x8F26;
        public const uint GL_SECONDARY_COLOR_ARRAY_ADDRESS_NV = 0x8F27;
        public const uint GL_FOG_COORD_ARRAY_ADDRESS_NV = 0x8F28;
        public const uint GL_ELEMENT_ARRAY_ADDRESS_NV = 0x8F29;
        public const uint GL_VERTEX_ATTRIB_ARRAY_LENGTH_NV = 0x8F2A;
        public const uint GL_VERTEX_ARRAY_LENGTH_NV = 0x8F2B;
        public const uint GL_NORMAL_ARRAY_LENGTH_NV = 0x8F2C;
        public const uint GL_COLOR_ARRAY_LENGTH_NV = 0x8F2D;
        public const uint GL_INDEX_ARRAY_LENGTH_NV = 0x8F2E;
        public const uint GL_TEXTURE_COORD_ARRAY_LENGTH_NV = 0x8F2F;
        public const uint GL_EDGE_FLAG_ARRAY_LENGTH_NV = 0x8F30;
        public const uint GL_SECONDARY_COLOR_ARRAY_LENGTH_NV = 0x8F31;
        public const uint GL_FOG_COORD_ARRAY_LENGTH_NV = 0x8F32;
        public const uint GL_ELEMENT_ARRAY_LENGTH_NV = 0x8F33;
        public const uint GL_DRAW_INDIRECT_UNIFIED_NV = 0x8F40;
        public const uint GL_DRAW_INDIRECT_ADDRESS_NV = 0x8F41;
        public const uint GL_DRAW_INDIRECT_LENGTH_NV = 0x8F42;

        private static delegate* unmanaged[Stdcall]<uint, uint, void*, long, void> pfn_glBufferAddressRangeNV = null;
        /// <summary> <see href="docs.gl/gl4/glBufferAddressRangeNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glBufferAddressRangeNV(uint pname, uint index, void* address, long length) => pfn_glBufferAddressRangeNV(pname, index, address, length);

        private static delegate* unmanaged[Stdcall]<int, uint, int, void> pfn_glVertexFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexFormatNV(int size, uint type, int stride) => pfn_glVertexFormatNV(size, type, stride);

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glNormalFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glNormalFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glNormalFormatNV(uint type, int stride) => pfn_glNormalFormatNV(type, stride);

        private static delegate* unmanaged[Stdcall]<int, uint, int, void> pfn_glColorFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glColorFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glColorFormatNV(int size, uint type, int stride) => pfn_glColorFormatNV(size, type, stride);

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glIndexFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glIndexFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glIndexFormatNV(uint type, int stride) => pfn_glIndexFormatNV(type, stride);

        private static delegate* unmanaged[Stdcall]<int, uint, int, void> pfn_glTexCoordFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glTexCoordFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glTexCoordFormatNV(int size, uint type, int stride) => pfn_glTexCoordFormatNV(size, type, stride);

        private static delegate* unmanaged[Stdcall]<int, void> pfn_glEdgeFlagFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glEdgeFlagFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glEdgeFlagFormatNV(int stride) => pfn_glEdgeFlagFormatNV(stride);

        private static delegate* unmanaged[Stdcall]<int, uint, int, void> pfn_glSecondaryColorFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glSecondaryColorFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glSecondaryColorFormatNV(int size, uint type, int stride) => pfn_glSecondaryColorFormatNV(size, type, stride);

        private static delegate* unmanaged[Stdcall]<uint, int, void> pfn_glFogCoordFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glFogCoordFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFogCoordFormatNV(uint type, int stride) => pfn_glFogCoordFormatNV(type, stride);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, byte, int, void> pfn_glVertexAttribFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribFormatNV(uint index, int size, uint type, byte normalized, int stride) => pfn_glVertexAttribFormatNV(index, size, type, normalized, stride);

        private static delegate* unmanaged[Stdcall]<uint, int, uint, int, void> pfn_glVertexAttribIFormatNV = null;
        /// <summary> <see href="docs.gl/gl4/glVertexAttribIFormatNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glVertexAttribIFormatNV(uint index, int size, uint type, int stride) => pfn_glVertexAttribIFormatNV(index, size, type, stride);

        private static delegate* unmanaged[Stdcall]<uint, uint, void**, void> pfn_glGetIntegerui64i_vNV = null;
        /// <summary> <see href="docs.gl/gl4/glGetIntegerui64i_vNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glGetIntegerui64i_vNV(uint value, uint index, void** result) => pfn_glGetIntegerui64i_vNV(value, index, result);
    }

    public static unsafe class GLNVViewportArray2
    {
        static GLNVViewportArray2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_viewport_array2") ?? false) _GL_NV_viewport_array2 = 1;
        }
        private static uint _GL_NV_viewport_array2 = 0;
        public static uint GL_NV_viewport_array2 => _GL_NV_viewport_array2;
    }

    public static unsafe class GLNVViewportSwizzle
    {
        static GLNVViewportSwizzle() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_NV_viewport_swizzle") ?? false) _GL_NV_viewport_swizzle = 1;
        }
        private static uint _GL_NV_viewport_swizzle = 0;
        public static uint GL_NV_viewport_swizzle => _GL_NV_viewport_swizzle;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_X_NV = 0x9350;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_X_NV = 0x9351;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_Y_NV = 0x9352;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_Y_NV = 0x9353;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_Z_NV = 0x9354;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_Z_NV = 0x9355;
        public const uint GL_VIEWPORT_SWIZZLE_POSITIVE_W_NV = 0x9356;
        public const uint GL_VIEWPORT_SWIZZLE_NEGATIVE_W_NV = 0x9357;
        public const uint GL_VIEWPORT_SWIZZLE_X_NV = 0x9358;
        public const uint GL_VIEWPORT_SWIZZLE_Y_NV = 0x9359;
        public const uint GL_VIEWPORT_SWIZZLE_Z_NV = 0x935A;
        public const uint GL_VIEWPORT_SWIZZLE_W_NV = 0x935B;

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void> pfn_glViewportSwizzleNV = null;
        /// <summary> <see href="docs.gl/gl4/glViewportSwizzleNV">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glViewportSwizzleNV(uint index, uint swizzlex, uint swizzley, uint swizzlez, uint swizzlew) => pfn_glViewportSwizzleNV(index, swizzlex, swizzley, swizzlez, swizzlew);
    }

    public static unsafe class GLOVRMultiview
    {
        static GLOVRMultiview() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_OVR_multiview") ?? false) _GL_OVR_multiview = 1;
        }
        private static uint _GL_OVR_multiview = 0;
        public static uint GL_OVR_multiview => _GL_OVR_multiview;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_NUM_VIEWS_OVR = 0x9630;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_BASE_VIEW_INDEX_OVR = 0x9632;
        public const uint GL_MAX_VIEWS_OVR = 0x9631;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_VIEW_TARGETS_OVR = 0x9633;

        private static delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, int, void> pfn_glFramebufferTextureMultiviewOVR = null;
        /// <summary> <see href="docs.gl/gl4/glFramebufferTextureMultiviewOVR">See on docs.gl</see> </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void glFramebufferTextureMultiviewOVR(uint target, uint attachment, uint texture, int level, int baseViewIndex, int numViews) => pfn_glFramebufferTextureMultiviewOVR(target, attachment, texture, level, baseViewIndex, numViews);
    }

    public static unsafe class GLOVRMultiview2
    {
        static GLOVRMultiview2() => Init();
        public static void Init()
        {
            if (glExtensions?.Contains("GL_OVR_multiview2") ?? false) _GL_OVR_multiview2 = 1;
        }
        private static uint _GL_OVR_multiview2 = 0;
        public static uint GL_OVR_multiview2 => _GL_OVR_multiview2;
    }
}
