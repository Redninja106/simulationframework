using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11GraphicsCapabilities : GraphicsCapabilities
{
    /// <summary>
    /// The maximum width of a thread group.
    /// </summary>
    public override int MaxThreadGroupWidth => 1024;
    public override int MaxThreadGroupHeight => 1024;
    public override int MaxThreadGroupDepth => 64;
    
    /// <summary>
    /// 
    /// </summary>
    public override int MaxThreadGroupSize => 1024;

    /// <summary>
    /// The maximum number of thread groups on the X axis in a <see cref="Graphics.DispatchComputeShader(Shaders.IShader?, int, IGraphicsQueue?)"/> call.
    /// </summary>
    public override int MaxThreadGroupCountX => 65535;
    /// <summary>
    /// The maximum number of thread groups on the Y axis in a <see cref="Graphics.DispatchComputeShader(Shaders.IShader?, int, IGraphicsQueue?)"/> call.
    /// </summary>
    public override int MaxThreadGroupCountY => 65535;
    /// <summary>
    /// The maximum number of thread groups on the Z axis in a <see cref="Graphics.DispatchComputeShader(Shaders.IShader?, int, IGraphicsQueue?)"/> call.
    /// </summary>
    public override int MaxThreadGroupCountZ => 65535;

}
