using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU;
public unsafe interface IChainable
{
    IChainable? Next { get; }
    
    int SizeInBytes { get; }
    internal void InitNative(ChainedStruct.Native* native);
    internal void UninitNative(ChainedStruct.Native* native);
}