using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Represents a 32-bit RGBA color.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Color : IEquatable<Color>
{

    /// <summary>
    /// An RGBA color with the value #66CDAAFF.
    /// </summary>
    public static readonly Color MediumAquamarine = (Color)0x66CDAAFF;

    /// <summary>
    /// An RGBA color with the value #0000CDFF.
    /// </summary>
    public static readonly Color MediumBlue = (Color)0x0000CDFF;

    /// <summary>
    /// An RGBA color with the value #BA55D3FF.
    /// </summary>
    public static readonly Color MediumOrchid = (Color)0xBA55D3FF;

    /// <summary>
    /// An RGBA color with the value #9370DBFF.
    /// </summary>
    public static readonly Color MediumPurple = (Color)0x9370DBFF;

    /// <summary>
    /// An RGBA color with the value #3CB371FF.
    /// </summary>
    public static readonly Color MediumSeaGreen = (Color)0x3CB371FF;

    /// <summary>
    /// An RGBA color with the value #7B68EEFF.
    /// </summary>
    public static readonly Color MediumSlateBlue = (Color)0x7B68EEFF;

    /// <summary>
    /// An RGBA color with the value #00FA9AFF.
    /// </summary>
    public static readonly Color MediumSpringGreen = (Color)0x00FA9AFF;

    /// <summary>
    /// An RGBA color with the value #48D1CCFF.
    /// </summary>
    public static readonly Color MediumTurquoise = (Color)0x48D1CCFF;

    /// <summary>
    /// An RGBA color with the value #C71585FF.
    /// </summary>
    public static readonly Color MediumVioletRed = (Color)0xC71585FF;

    /// <summary>
    /// An RGBA color with the value #191970FF.
    /// </summary>
    public static readonly Color MidnightBlue = (Color)0x191970FF;

    /// <summary>
    /// An RGBA color with the value #F5FFFAFF.
    /// </summary>
    public static readonly Color MintCream = (Color)0xF5FFFAFF;

    /// <summary>
    /// An RGBA color with the value #FFE4E1FF.
    /// </summary>
    public static readonly Color MistyRose = (Color)0xFFE4E1FF;

    /// <summary>
    /// An RGBA color with the value #FFE4B5FF.
    /// </summary>
    public static readonly Color Moccasin = (Color)0xFFE4B5FF;

    /// <summary>
    /// An RGBA color with the value #FFDEADFF.
    /// </summary>
    public static readonly Color NavajoWhite = (Color)0xFFDEADFF;

    /// <summary>
    /// An RGBA color with the value #000080FF.
    /// </summary>
    public static readonly Color Navy = (Color)0x000080FF;

    /// <summary>
    /// An RGBA color with the value #FDF5E6FF.
    /// </summary>
    public static readonly Color OldLace = (Color)0xFDF5E6FF;

    /// <summary>
    /// An RGBA color with the value #808000FF.
    /// </summary>
    public static readonly Color Olive = (Color)0x808000FF;

    /// <summary>
    /// An RGBA color with the value #800000FF.
    /// </summary>
    public static readonly Color Maroon = (Color)0x800000FF;

    /// <summary>
    /// An RGBA color with the value #6B8E23FF.
    /// </summary>
    public static readonly Color OliveDrab = (Color)0x6B8E23FF;

    /// <summary>
    /// An RGBA color with the value #FF00FFFF.
    /// </summary>
    public static readonly Color Magenta = (Color)0xFF00FFFF;

    /// <summary>
    /// An RGBA color with the value #32CD32FF.
    /// </summary>
    public static readonly Color LimeGreen = (Color)0x32CD32FF;

    /// <summary>
    /// An RGBA color with the value #FFF0F5FF.
    /// </summary>
    public static readonly Color LavenderBlush = (Color)0xFFF0F5FF;

    /// <summary>
    /// An RGBA color with the value #7CFC00FF.
    /// </summary>
    public static readonly Color LawnGreen = (Color)0x7CFC00FF;

    /// <summary>
    /// An RGBA color with the value #FFFACDFF.
    /// </summary>
    public static readonly Color LemonChiffon = (Color)0xFFFACDFF;

    /// <summary>
    /// An RGBA color with the value #ADD8E6FF.
    /// </summary>
    public static readonly Color LightBlue = (Color)0xADD8E6FF;

    /// <summary>
    /// An RGBA color with the value #F08080FF.
    /// </summary>
    public static readonly Color LightCoral = (Color)0xF08080FF;

    /// <summary>
    /// An RGBA color with the value #E0FFFFFF.
    /// </summary>
    public static readonly Color LightCyan = (Color)0xE0FFFFFF;

    /// <summary>
    /// An RGBA color with the value #FAFAD2FF.
    /// </summary>
    public static readonly Color LightGoldenrodYellow = (Color)0xFAFAD2FF;

    /// <summary>
    /// An RGBA color with the value #D3D3D3FF.
    /// </summary>
    public static readonly Color LightGray = (Color)0xD3D3D3FF;

    /// <summary>
    /// An RGBA color with the value #90EE90FF.
    /// </summary>
    public static readonly Color LightGreen = (Color)0x90EE90FF;

    /// <summary>
    /// An RGBA color with the value #FFB6C1FF.
    /// </summary>
    public static readonly Color LightPink = (Color)0xFFB6C1FF;

    /// <summary>
    /// An RGBA color with the value #FFA07AFF.
    /// </summary>
    public static readonly Color LightSalmon = (Color)0xFFA07AFF;

    /// <summary>
    /// An RGBA color with the value #20B2AAFF.
    /// </summary>
    public static readonly Color LightSeaGreen = (Color)0x20B2AAFF;

    /// <summary>
    /// An RGBA color with the value #87CEFAFF.
    /// </summary>
    public static readonly Color LightSkyBlue = (Color)0x87CEFAFF;

    /// <summary>
    /// An RGBA color with the value #778899FF.
    /// </summary>
    public static readonly Color LightSlateGray = (Color)0x778899FF;

    /// <summary>
    /// An RGBA color with the value #B0C4DEFF.
    /// </summary>
    public static readonly Color LightSteelBlue = (Color)0xB0C4DEFF;

    /// <summary>
    /// An RGBA color with the value #FFFFE0FF.
    /// </summary>
    public static readonly Color LightYellow = (Color)0xFFFFE0FF;

    /// <summary>
    /// An RGBA color with the value #00FF00FF.
    /// </summary>
    public static readonly Color Lime = (Color)0x00FF00FF;

    /// <summary>
    /// An RGBA color with the value #FAF0E6FF.
    /// </summary>
    public static readonly Color Linen = (Color)0xFAF0E6FF;

    /// <summary>
    /// An RGBA color with the value #FFFF00FF.
    /// </summary>
    public static readonly Color Yellow = (Color)0xFFFF00FF;

    /// <summary>
    /// An RGBA color with the value #FFA500FF.
    /// </summary>
    public static readonly Color Orange = (Color)0xFFA500FF;

    /// <summary>
    /// An RGBA color with the value #DA70D6FF.
    /// </summary>
    public static readonly Color Orchid = (Color)0xDA70D6FF;

    /// <summary>
    /// An RGBA color with the value #C0C0C0FF.
    /// </summary>
    public static readonly Color Silver = (Color)0xC0C0C0FF;

    /// <summary>
    /// An RGBA color with the value #87CEEBFF.
    /// </summary>
    public static readonly Color SkyBlue = (Color)0x87CEEBFF;

    /// <summary>
    /// An RGBA color with the value #6A5ACDFF.
    /// </summary>
    public static readonly Color SlateBlue = (Color)0x6A5ACDFF;

    /// <summary>
    /// An RGBA color with the value #708090FF.
    /// </summary>
    public static readonly Color SlateGray = (Color)0x708090FF;

    /// <summary>
    /// An RGBA color with the value #FFFAFAFF.
    /// </summary>
    public static readonly Color Snow = (Color)0xFFFAFAFF;

    /// <summary>
    /// An RGBA color with the value #00FF7FFF.
    /// </summary>
    public static readonly Color SpringGreen = (Color)0x00FF7FFF;

    /// <summary>
    /// An RGBA color with the value #4682B4FF.
    /// </summary>
    public static readonly Color SteelBlue = (Color)0x4682B4FF;

    /// <summary>
    /// An RGBA color with the value #D2B48CFF.
    /// </summary>
    public static readonly Color Tan = (Color)0xD2B48CFF;

    /// <summary>
    /// An RGBA color with the value #008080FF.
    /// </summary>
    public static readonly Color Teal = (Color)0x008080FF;

    /// <summary>
    /// An RGBA color with the value #D8BFD8FF.
    /// </summary>
    public static readonly Color Thistle = (Color)0xD8BFD8FF;

    /// <summary>
    /// An RGBA color with the value #FF6347FF.
    /// </summary>
    public static readonly Color Tomato = (Color)0xFF6347FF;

    /// <summary>
    /// An RGBA color with the value #40E0D0FF.
    /// </summary>
    public static readonly Color Turquoise = (Color)0x40E0D0FF;

    /// <summary>
    /// An RGBA color with the value #EE82EEFF.
    /// </summary>
    public static readonly Color Violet = (Color)0xEE82EEFF;

    /// <summary>
    /// An RGBA color with the value #F5DEB3FF.
    /// </summary>
    public static readonly Color Wheat = (Color)0xF5DEB3FF;

    /// <summary>
    /// An RGBA color with the value #FFFFFFFF.
    /// </summary>
    public static readonly Color White = (Color)0xFFFFFFFF;

    /// <summary>
    /// An RGBA color with the value #F5F5F5FF.
    /// </summary>
    public static readonly Color WhiteSmoke = (Color)0xF5F5F5FF;

    /// <summary>
    /// An RGBA color with the value #A0522DFF.
    /// </summary>
    public static readonly Color Sienna = (Color)0xA0522DFF;

    /// <summary>
    /// An RGBA color with the value #FF4500FF.
    /// </summary>
    public static readonly Color OrangeRed = (Color)0xFF4500FF;

    /// <summary>
    /// An RGBA color with the value #FFF5EEFF.
    /// </summary>
    public static readonly Color SeaShell = (Color)0xFFF5EEFF;

    /// <summary>
    /// An RGBA color with the value #F4A460FF.
    /// </summary>
    public static readonly Color SandyBrown = (Color)0xF4A460FF;

    /// <summary>
    /// An RGBA color with the value #EEE8AAFF.
    /// </summary>
    public static readonly Color PaleGoldenrod = (Color)0xEEE8AAFF;

    /// <summary>
    /// An RGBA color with the value #98FB98FF.
    /// </summary>
    public static readonly Color PaleGreen = (Color)0x98FB98FF;

    /// <summary>
    /// An RGBA color with the value #AFEEEEFF.
    /// </summary>
    public static readonly Color PaleTurquoise = (Color)0xAFEEEEFF;

    /// <summary>
    /// An RGBA color with the value #DB7093FF.
    /// </summary>
    public static readonly Color PaleVioletRed = (Color)0xDB7093FF;

    /// <summary>
    /// An RGBA color with the value #FFEFD5FF.
    /// </summary>
    public static readonly Color PapayaWhip = (Color)0xFFEFD5FF;

    /// <summary>
    /// An RGBA color with the value #FFDAB9FF.
    /// </summary>
    public static readonly Color PeachPuff = (Color)0xFFDAB9FF;

    /// <summary>
    /// An RGBA color with the value #CD853FFF.
    /// </summary>
    public static readonly Color Peru = (Color)0xCD853FFF;

    /// <summary>
    /// An RGBA color with the value #FFC0CBFF.
    /// </summary>
    public static readonly Color Pink = (Color)0xFFC0CBFF;

    /// <summary>
    /// An RGBA color with the value #DDA0DDFF.
    /// </summary>
    public static readonly Color Plum = (Color)0xDDA0DDFF;

    /// <summary>
    /// An RGBA color with the value #B0E0E6FF.
    /// </summary>
    public static readonly Color PowderBlue = (Color)0xB0E0E6FF;

    /// <summary>
    /// An RGBA color with the value #800080FF.
    /// </summary>
    public static readonly Color Purple = (Color)0x800080FF;

    /// <summary>
    /// An RGBA color with the value #663399FF.
    /// </summary>
    public static readonly Color RebeccaPurple = (Color)0x663399FF;

    /// <summary>
    /// An RGBA color with the value #FF0000FF.
    /// </summary>
    public static readonly Color Red = (Color)0xFF0000FF;

    /// <summary>
    /// An RGBA color with the value #BC8F8FFF.
    /// </summary>
    public static readonly Color RosyBrown = (Color)0xBC8F8FFF;

    /// <summary>
    /// An RGBA color with the value #4169E1FF.
    /// </summary>
    public static readonly Color RoyalBlue = (Color)0x4169E1FF;

    /// <summary>
    /// An RGBA color with the value #8B4513FF.
    /// </summary>
    public static readonly Color SaddleBrown = (Color)0x8B4513FF;

    /// <summary>
    /// An RGBA color with the value #FA8072FF.
    /// </summary>
    public static readonly Color Salmon = (Color)0xFA8072FF;

    /// <summary>
    /// An RGBA color with the value #2E8B57FF.
    /// </summary>
    public static readonly Color SeaGreen = (Color)0x2E8B57FF;

    /// <summary>
    /// An RGBA color with the value #F0E68CFF.
    /// </summary>
    public static readonly Color Khaki = (Color)0xF0E68CFF;

    /// <summary>
    /// An RGBA color with the value #E6E6FAFF.
    /// </summary>
    public static readonly Color Lavender = (Color)0xE6E6FAFF;

    /// <summary>
    /// An RGBA color with the value #00FFFFFF.
    /// </summary>
    public static readonly Color Cyan = (Color)0x00FFFFFF;

    /// <summary>
    /// An RGBA color with the value #8B008BFF.
    /// </summary>
    public static readonly Color DarkMagenta = (Color)0x8B008BFF;

    /// <summary>
    /// An RGBA color with the value #BDB76BFF.
    /// </summary>
    public static readonly Color DarkKhaki = (Color)0xBDB76BFF;

    /// <summary>
    /// An RGBA color with the value #006400FF.
    /// </summary>
    public static readonly Color DarkGreen = (Color)0x006400FF;

    /// <summary>
    /// An RGBA color with the value #A9A9A9FF.
    /// </summary>
    public static readonly Color DarkGray = (Color)0xA9A9A9FF;

    /// <summary>
    /// An RGBA color with the value #B8860BFF.
    /// </summary>
    public static readonly Color DarkGoldenrod = (Color)0xB8860BFF;

    /// <summary>
    /// An RGBA color with the value #008B8BFF.
    /// </summary>
    public static readonly Color DarkCyan = (Color)0x008B8BFF;

    /// <summary>
    /// An RGBA color with the value #00008BFF.
    /// </summary>
    public static readonly Color DarkBlue = (Color)0x00008BFF;

    /// <summary>
    /// An RGBA color with the value #FFFFF0FF.
    /// </summary>
    public static readonly Color Ivory = (Color)0xFFFFF0FF;

    /// <summary>
    /// An RGBA color with the value #DC143CFF.
    /// </summary>
    public static readonly Color Crimson = (Color)0xDC143CFF;

    /// <summary>
    /// An RGBA color with the value #FFF8DCFF.
    /// </summary>
    public static readonly Color Cornsilk = (Color)0xFFF8DCFF;

    /// <summary>
    /// An RGBA color with the value #6495EDFF.
    /// </summary>
    public static readonly Color CornflowerBlue = (Color)0x6495EDFF;

    /// <summary>
    /// An RGBA color with the value #FF7F50FF.
    /// </summary>
    public static readonly Color Coral = (Color)0xFF7F50FF;

    /// <summary>
    /// An RGBA color with the value #D2691EFF.
    /// </summary>
    public static readonly Color Chocolate = (Color)0xD2691EFF;

    /// <summary>
    /// An RGBA color with the value #556B2FFF.
    /// </summary>
    public static readonly Color DarkOliveGreen = (Color)0x556B2FFF;

    /// <summary>
    /// An RGBA color with the value #7FFF00FF.
    /// </summary>
    public static readonly Color Chartreuse = (Color)0x7FFF00FF;

    /// <summary>
    /// An RGBA color with the value #DEB887FF.
    /// </summary>
    public static readonly Color BurlyWood = (Color)0xDEB887FF;

    /// <summary>
    /// An RGBA color with the value #A52A2AFF.
    /// </summary>
    public static readonly Color Brown = (Color)0xA52A2AFF;

    /// <summary>
    /// An RGBA color with the value #8A2BE2FF.
    /// </summary>
    public static readonly Color BlueViolet = (Color)0x8A2BE2FF;

    /// <summary>
    /// An RGBA color with the value #0000FFFF.
    /// </summary>
    public static readonly Color Blue = (Color)0x0000FFFF;

    /// <summary>
    /// An RGBA color with the value #FFEBCDFF.
    /// </summary>
    public static readonly Color BlanchedAlmond = (Color)0xFFEBCDFF;

    /// <summary>
    /// An RGBA color with the value #000000FF.
    /// </summary>
    public static readonly Color Black = (Color)0x000000FF;

    /// <summary>
    /// An RGBA color with the value #FFE4C4FF.
    /// </summary>
    public static readonly Color Bisque = (Color)0xFFE4C4FF;

    /// <summary>
    /// An RGBA color with the value #F5F5DCFF.
    /// </summary>
    public static readonly Color Beige = (Color)0xF5F5DCFF;

    /// <summary>
    /// An RGBA color with the value #F0FFFFFF.
    /// </summary>
    public static readonly Color Azure = (Color)0xF0FFFFFF;

    /// <summary>
    /// An RGBA color with the value #7FFFD4FF.
    /// </summary>
    public static readonly Color Aquamarine = (Color)0x7FFFD4FF;

    /// <summary>
    /// An RGBA color with the value #00FFFFFF.
    /// </summary>
    public static readonly Color Aqua = (Color)0x00FFFFFF;

    /// <summary>
    /// An RGBA color with the value #FAEBD7FF.
    /// </summary>
    public static readonly Color AntiqueWhite = (Color)0xFAEBD7FF;

    /// <summary>
    /// An RGBA color with the value #F0F8FFFF.
    /// </summary>
    public static readonly Color AliceBlue = (Color)0xF0F8FFFF;

    /// <summary>
    /// An RGBA color with the value #5F9EA0FF.
    /// </summary>
    public static readonly Color CadetBlue = (Color)0x5F9EA0FF;

    /// <summary>
    /// An RGBA color with the value #FF8C00FF.
    /// </summary>
    public static readonly Color DarkOrange = (Color)0xFF8C00FF;

    /// <summary>
    /// An RGBA color with the value #9ACD32FF.
    /// </summary>
    public static readonly Color YellowGreen = (Color)0x9ACD32FF;

    /// <summary>
    /// An RGBA color with the value #8B0000FF.
    /// </summary>
    public static readonly Color DarkRed = (Color)0x8B0000FF;

    /// <summary>
    /// An RGBA color with the value #4B0082FF.
    /// </summary>
    public static readonly Color Indigo = (Color)0x4B0082FF;

    /// <summary>
    /// An RGBA color with the value #CD5C5CFF.
    /// </summary>
    public static readonly Color IndianRed = (Color)0xCD5C5CFF;

    /// <summary>
    /// An RGBA color with the value #9932CCFF.
    /// </summary>
    public static readonly Color DarkOrchid = (Color)0x9932CCFF;

    /// <summary>
    /// An RGBA color with the value #F0FFF0FF.
    /// </summary>
    public static readonly Color Honeydew = (Color)0xF0FFF0FF;

    /// <summary>
    /// An RGBA color with the value #ADFF2FFF.
    /// </summary>
    public static readonly Color GreenYellow = (Color)0xADFF2FFF;

    /// <summary>
    /// An RGBA color with the value #008000FF.
    /// </summary>
    public static readonly Color Green = (Color)0x008000FF;

    /// <summary>
    /// An RGBA color with the value #808080FF.
    /// </summary>
    public static readonly Color Gray = (Color)0x808080FF;

    /// <summary>
    /// An RGBA color with the value #DAA520FF.
    /// </summary>
    public static readonly Color Goldenrod = (Color)0xDAA520FF;

    /// <summary>
    /// An RGBA color with the value #FFD700FF.
    /// </summary>
    public static readonly Color Gold = (Color)0xFFD700FF;

    /// <summary>
    /// An RGBA color with the value #F8F8FFFF.
    /// </summary>
    public static readonly Color GhostWhite = (Color)0xF8F8FFFF;

    /// <summary>
    /// An RGBA color with the value #DCDCDCFF.
    /// </summary>
    public static readonly Color Gainsboro = (Color)0xDCDCDCFF;

    /// <summary>
    /// An RGBA color with the value #FF00FFFF.
    /// </summary>
    public static readonly Color Fuchsia = (Color)0xFF00FFFF;

    /// <summary>
    /// An RGBA color with the value #228B22FF.
    /// </summary>
    public static readonly Color ForestGreen = (Color)0x228B22FF;

    /// <summary>
    /// An RGBA color with the value #FF69B4FF.
    /// </summary>
    public static readonly Color HotPink = (Color)0xFF69B4FF;

    /// <summary>
    /// An RGBA color with the value #B22222FF.
    /// </summary>
    public static readonly Color Firebrick = (Color)0xB22222FF;

    /// <summary>
    /// An RGBA color with the value #FFFAF0FF.
    /// </summary>
    public static readonly Color FloralWhite = (Color)0xFFFAF0FF;

    /// <summary>
    /// An RGBA color with the value #1E90FFFF.
    /// </summary>
    public static readonly Color DodgerBlue = (Color)0x1E90FFFF;

    /// <summary>
    /// An RGBA color with the value #696969FF.
    /// </summary>
    public static readonly Color DimGray = (Color)0x696969FF;

    /// <summary>
    /// An RGBA color with the value #00BFFFFF.
    /// </summary>
    public static readonly Color DeepSkyBlue = (Color)0x00BFFFFF;

    /// <summary>
    /// An RGBA color with the value #FF1493FF.
    /// </summary>
    public static readonly Color DeepPink = (Color)0xFF1493FF;

    /// <summary>
    /// An RGBA color with the value #9400D3FF.
    /// </summary>
    public static readonly Color DarkViolet = (Color)0x9400D3FF;

    /// <summary>
    /// An RGBA color with the value #00CED1FF.
    /// </summary>
    public static readonly Color DarkTurquoise = (Color)0x00CED1FF;

    /// <summary>
    /// An RGBA color with the value #2F4F4FFF.
    /// </summary>
    public static readonly Color DarkSlateGray = (Color)0x2F4F4FFF;

    /// <summary>
    /// An RGBA color with the value #483D8BFF.
    /// </summary>
    public static readonly Color DarkSlateBlue = (Color)0x483D8BFF;

    /// <summary>
    /// An RGBA color with the value #8FBC8BFF.
    /// </summary>
    public static readonly Color DarkSeaGreen = (Color)0x8FBC8BFF;

    /// <summary>
    /// An RGBA color with the value #E9967AFF.
    /// </summary>
    public static readonly Color DarkSalmon = (Color)0xE9967AFF;

    public static readonly Color Transparent = (Color)0x00000000;

    private readonly byte r;
    private readonly byte g;
    private readonly byte b;
    private readonly byte a;

    /// <summary>
    /// The 32-bit, RGBA value of this color.
    /// </summary>
    public uint Value 
    { 
        readonly get
        {
            return (uint)((r << 24) | (g << 16) | (b << 8) | a);
        }
        init
        {
            r = (byte)((value & 0xFF000000) >> 24);
            g = (byte)((value & 0x00FF0000) >> 16);
            b = (byte)((value & 0x0000FF00) >> 8);
            a = (byte)((value & 0x000000FF));
        }
    }
    
    /// <summary>
    /// The 8-bit value of the red component of this color.
    /// </summary>
    public byte R { readonly get => this.r; init => this.r = value; }
    /// <summary>
    /// The 8-bit value of the green component of this color.
    /// </summary>
    public byte G { readonly get => this.g; init => this.g = value; }
    /// <summary>
    /// The 8-bit value of the blue component of this color.
    /// </summary>
    public byte B { readonly get => this.b; init => this.b = value; }
    /// <summary>
    /// The 8-bit value of the alpha component of this color.
    /// </summary>
    public byte A { readonly get => this.a; init => this.a = value; }

    public Color()
    {
        r = g = b = a = 0;
    }

    public Color(uint value) : this() => this.Value = value;

    /// <summary>
    /// Creates a new color with the provided RGB values, and an alpha of 255.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public Color(byte r, byte g, byte b) : this(r, g, b, 255) { }

    /// <summary>
    /// Creates a new color with the provided values.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <param name="a">The alpha component of the color.</param>
    public Color(byte r, byte g, byte b, byte a) : this()
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color(Vector4 values) : this((byte)(values.X * 255), (byte)(values.Y * 255), (byte)(values.Z * 255), (byte)(values.W * 255))
    {
    }

    public static implicit operator Color((byte r, byte g, byte b) values) => new(values.r, values.g, values.b);
    public static implicit operator Color((byte r, byte g, byte b, byte a) values) => new(values.r, values.g, values.b, values.a);

    public static implicit operator System.Drawing.Color(Color value) => System.Drawing.Color.FromArgb(value.a, value.r, value.g, value.b);
    public static implicit operator Color(System.Drawing.Color value) => new(value.R, value.G, value.B, value.A);

    public static explicit operator uint(Color color) => color.Value;
    public static explicit operator Color(uint value) => new(value);

    public bool Equals(Color other)
    {
        return other.Value == this.Value;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj is Color color)
            return this.Equals(color);

        return false;
    }
    /// <summary>
    /// Returns the formatted values of this color.
    /// </summary>
    public override string ToString()
    {
        return $"#{Value:x8}";
    }

    public override int GetHashCode()
    {
        return unchecked((int)Value);
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns this color as a Vector3, with its R, G, and B values as X, Y, and Z, respectively.
    /// </summary>
    public Vector3 ToVector3()
    {
        return new Vector3(this.r / 255f, this.g / 255f, this.b / 255f);
    }

    /// <summary>
    /// Returns this color as a Vector3, with its R, G, B, and A values as X, Y, Z, and W, respectively.
    /// </summary>
    public Vector4 ToVector4()
    {
        return new Vector4(this.r / 255f, this.g / 255f, this.B / 255f, this.a / 255f);
    }
}