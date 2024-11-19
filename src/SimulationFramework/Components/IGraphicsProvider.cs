using System.Diagnostics.CodeAnalysis;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Components;

/// <summary>
/// Interface which provides graphics functionality as a component of a simulation.
/// </summary>
public interface IGraphicsProvider : ISimulationComponent
{
    /// <summary>
    /// Returns an instance of the default font.
    /// </summary>
    IFont DefaultFont { get; }

    /// <summary>
    /// Gets the canvas for the current frame.
    /// </summary>
    /// <returns>
    /// The canvas which draws to the current frame. This object should never be saved, as it may be different every frame.
    /// </returns>
    ICanvas GetWindowCanvas();

    /// <summary>
    /// Creates a new bitmap with the provided data.
    /// </summary>
    ITexture CreateTexture(int width, int height, ReadOnlySpan<Color> pixels, TextureOptions options);

    /// <summary>
    /// Loads a bitmap from a image file.
    /// </summary>
    ITexture LoadTexture(ReadOnlySpan<byte> encodedData, TextureOptions options);

    /// <summary>
    /// Loads a font from a font file.
    /// </summary>
    IFont LoadFont(ReadOnlySpan<byte> encodedData);

    /// <summary>
    ///Loads a system font by name.
    /// </summary>
    IFont LoadSystemFont(string name);
    
    /// <summary>
    /// Invokes a compute shader with the provided number of threads on each axis. 
    /// </summary>
    void Dispatch(ComputeShader computeShader, int lengthI, int lengthJ, int lengthK);

    /// <summary>
    /// Creates a screen-space mask.
    /// </summary>
    IMask CreateMask(int width, int height);

    /// <summary>
    /// Creates a screen-space mask with an additional depth channel.
    /// </summary>
    IDepthMask CreateDepthMask(int width, int height);

    IGeometry CreateGeometry<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices) 
        where TVertex : unmanaged;

    /// <summary>
    /// Generates mipmaps for a texture. Set <see cref="ITexture.Filter"/> to a mipmap filter to use them.
    /// </summary>
    void GenerateMipmaps(ITexture texture);

    void UploadArray(Array array);
    void SyncArray(Array array);
}