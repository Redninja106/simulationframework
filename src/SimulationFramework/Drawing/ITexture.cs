using System.IO;

namespace SimulationFramework.Drawing;

/// <summary>
/// A 2D bitmap used for rendering. Textures can be created using <see cref="Graphics.CreateTexture(int, int, TextureOptions)"/> or loaded using <see cref="Graphics.LoadTexture(string, TextureOptions)"/>.
/// </summary>
public interface ITexture : IDisposable
{
    /// <summary>
    /// The width of the texture, in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the texture, in pixels.
    /// </summary>
    int Height { get; }
     
    /// <summary>
    /// The configuration this texture was created with.
    /// </summary>
    TextureOptions Options { get; }

    /// <summary>
    /// The x-axis wrap mode used when sampling the texture.
    /// </summary>
    WrapMode WrapModeX { get; set; }

    /// <summary>
    /// The y-axis wrap mode used when sampling the texture.
    /// </summary>
    WrapMode WrapModeY { get; set; }

    /// <summary>
    /// The Filter used when sampling the texture.
    /// </summary>
    TextureFilter Filter { get; set; }

    /// <summary>
    /// A span of colors making up texture's data
    /// <para>
    /// If changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    Span<Color> Pixels { get; }

    /// <summary>
    /// Gets a reference to the element of <see cref="Pixels"/> at the provided <paramref name="x"/> and <paramref name="y"/> coordinates (index calculated as <c>y * Width + x</c>).
    /// <para>
    /// If changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    [Obsolete("use the indexer instead (ie texture[x, y])")]
    sealed ref Color GetPixel(int x, int y)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x));
        
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));
        
        return ref Pixels[y * Width + x];
    }

    /// <summary>
    /// Sets the element of <see cref="Pixels"/> at the provided <paramref name="x"/> and <paramref name="y"/> coordinates (index calculated as <c>y * Width + x</c>).
    /// <para>
    /// Any changes are made to the texture's data, they may not be applied until <see cref="ApplyChanges"/> is called.
    /// </para>
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    /// <param name="color">The color the set the pixel to.</param>
    [Obsolete("use the indexer instead (ie texture[x, y] = value)")]
    sealed void SetPixel(int x, int y, Color color)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x));

        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));

        Pixels[y * Width + x] = color;
    }

    ref Color this[int x, int y]
    {
        get
        {
            if ((uint)x >= Width || (uint)y >= Height)
            {
                throw new IndexOutOfRangeException();
            }

            return ref Pixels[y * Width + x];
        }
    }

    /// <summary>
    /// Gets a canvas which draws to this texture.
    /// </summary>
    ICanvas GetCanvas();

    /// <summary>
    /// Applies any cpu-side changes made do the bitmap's data using <see cref="Pixels"/> or <see cref="GetPixel(int, int)"/>.
    /// </summary>
    void ApplyChanges();

    /// <summary>
    /// Encode this texture's data to a stream using the specified encoding.
    /// </summary>
    /// <param name="destination">The stream to encode the texture to.</param>
    /// <param name="encoding">The encoding to use to encode the texture.</param>
    void Encode(Stream destination, TextureEncoding encoding);

    /// <summary>
    /// Encode this texture to a byte array.
    /// </summary>
    /// <returns>A byte array containing the encoded texture data.</returns>
    sealed byte[] Encode(TextureEncoding encoding)
    {
        using MemoryStream ms = new();
        Encode(ms, encoding);
        return ms.GetBuffer();
    }

    /// <summary>
    /// Encodes this texture to a file, determining the encoding to use based on the file extension.
    /// </summary>
    /// <param name="file">The file to encode the texture to. The file extension determines which <see cref="TextureEncoding"/> is used.</param>
    sealed void Encode(string file)
    {
        string extension = Path.GetExtension(file).TrimStart('.');
        
        if (!Enum.TryParse<TextureEncoding>(extension, true, out var encoding))
        {
            throw new Exception("Could not determine encoding from file extension.");
        }

        Encode(file, encoding);
    }

    /// <summary>
    /// Encodes this texture to a file, determining the encoding to use based on the file extension.
    /// </summary>
    /// <param name="file">The file to encode the texture to. The file extension determines which <see cref="TextureEncoding"/> is used.</param>
    /// <param name="encoding"></param>
    sealed void Encode(string file, TextureEncoding encoding)
    {
        using FileStream fs = new(file, FileMode.Create, FileAccess.Write);
        Encode(fs, encoding);
    }
}