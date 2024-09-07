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
    Comparison Comparison { get; set; }

    void Clear(float depthValue);
}

public enum Comparison
{
    Always,
    Never,
    Equals,
    NotEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
}
