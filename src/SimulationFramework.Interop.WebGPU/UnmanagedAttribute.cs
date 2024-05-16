using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;

/// <summary>
/// Used to check at compile time if a type is unmanaged.
/// </summary>
/// <typeparam name="TTarget"></typeparam>
[AttributeUsage(AttributeTargets.Struct)]
internal class UnmanagedAttribute<TTarget> : Attribute where TTarget : unmanaged
{
}
