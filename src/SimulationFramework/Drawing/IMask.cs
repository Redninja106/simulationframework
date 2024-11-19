using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

/// <summary>
/// A screen-space mask used to selectively discard pixels.
/// </summary>
public interface IMask : IDisposable
{
    /// <summary>
    /// The width of the mask, in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the mask, in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Gets or sets the value of the mask at a point. <see langword="true"/> means a pixel should be rendered, <see langword="false"/> means it should be discarded.
    /// </summary>
    /// <param name="x">The X coordinate of the mask value.</param>
    /// <param name="y">The Y coordinate of the mask value.</param>
    bool this[int x, int y]
    {
        get;
        set;
    }

    /// <summary>
    /// Fills the mask with the specified value.
    /// </summary>
    /// <param name="value">The value to fill the mask with. <see langword="true"/> means a pixel should be rendered, <see langword="false"/> means it should be discarded.</param>
    void Clear(bool value);

    /// <summary>
    /// Applies any changes made using <see cref="this[int, int]"/>.
    /// </summary>
    void ApplyChanges();
}

/// <summary>
/// A screen space depth buffer used to discard pixels based on their Z value.
/// </summary>
public interface IDepthMask : IMask
{
    /// <summary>
    /// Sets the comparison to use when rendering with this mask. 
    /// Pixels are discarded when the comparison evalulates to false. For example, use <see cref="Comparison.LessThan"/> to only render pixels with a smaller depth value than the depth buffer.
    /// </summary>
    Comparison Comparison { get; set; }

    /// <summary>
    /// Clears the depth buffer with the specified value.
    /// </summary>
    /// <param name="depthValue">The depth value to clear with. Must be between 0 and 1.</param>
    void Clear(float depthValue);
}

/// <summary>
/// Specifies different kinds of comparisons to use while rendering.
/// </summary>
public enum Comparison
{
    /// <summary>
    /// Always <see langword="true"/>.
    /// </summary>
    Always,
    /// <summary>
    /// Always <see langword="false"/>.
    /// </summary>
    Never,
    /// <summary>
    /// <see langword="true"/> when two values are equal. 
    /// </summary>
    Equal,
    /// <summary>
    /// <see langword="true"/> when two values are not equal. 
    /// </summary>
    NotEqual,
    /// <summary>
    /// <see langword="true"/> when the first value is less than the second. 
    /// </summary>
    LessThan,
    /// <summary>
    /// <see langword="true"/> when the first value is less than or equal to the second. 
    /// </summary>
    LessThanEqual,
    /// <summary>
    /// <see langword="true"/> when the first value is greater than the second. 
    /// </summary>
    GreaterThan,
    /// <summary>
    /// <see langword="true"/> when the first value is greater than or equal to the second. 
    /// </summary>
    GreaterThanEqual,
}
