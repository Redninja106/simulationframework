using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders;

/// <summary>
/// Provides common interface for all kinds of shaders.
/// </summary>
public interface IShader
{
    /// <summary>
    /// The shader's entry point.
    /// </summary>
    [ShaderMethod]
    void Main();
}