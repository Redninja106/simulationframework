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
    int Width { get; }
    int Height { get; }

    bool this[int x, int y]
    {
        get;
        set;
    }

    void Clear(bool value);
    void ApplyChanges();
}

public interface IDepthMask : IMask
{
    /// <summary>
    /// Sets the comparison to use when rendering with this mask. 
    /// Pixels are discarded when the comparison evalulates to false. For example, use <see cref="Comparison.LessThan"/> to only render pixels with a smaller depth value than the depth buffer.
    /// </summary>
    Comparison Comparison { get; set; }

    void Clear(float depthValue);
}

public enum Comparison
{
    Always,
    Never,
    Equal,
    NotEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
}
