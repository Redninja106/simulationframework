using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
internal readonly struct WGPUBool
{
    private readonly uint Value { get; init; }

    public static implicit operator bool(WGPUBool value) => value.Value > 0;
    public static implicit operator WGPUBool(bool value) => new() { Value = value ? 1u : 0u };
}
