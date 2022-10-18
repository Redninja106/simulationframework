using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Specifies a platform implementation in an assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class ApplicationPlatformAttribute : Attribute
{
    /// <summary>
    /// The platform implementation. This type must implement <see cref="IApplicationPlatform"/>and a static method <c>IsSupported</c> which returns a <see langword="bool"/> and has no parameters, and have a parameterless constructor.
    /// </summary>
    public Type PlatformType { get; }

    /// <summary>
    /// Creates a new <see cref="ApplicationPlatformAttribute"/> instance.
    /// </summary>
    /// <param name="platformType">The platform implementation. This type must implement <see cref="IApplicationPlatform"/> and a static method <c>IsSupported</c> which returns a <see langword="bool"/> and has no parameters, and have a parameterless constructor.</param>
    public ApplicationPlatformAttribute(Type platformType)
    {
        PlatformType = platformType;
    }
}