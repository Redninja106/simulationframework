using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal class ShaderManager : D3D11Object
{
    public List<D3D11Object> Shaders { get; private set; }
    
    public ShaderManager(DeviceResources deviceResources) : base(deviceResources)
    {
        Shaders = new();
    }

    public override void Dispose()
    {
        Shaders.ForEach(s => s.Dispose());

        base.Dispose();
    }

    public D3D11ComputeShader<TShader> GetComputeShader<TShader>() where TShader : struct, IShader
    {
        var result = Shaders.OfType<D3D11ComputeShader<TShader>>().SingleOrDefault();

        if (result is null)
        {
            result = new D3D11ComputeShader<TShader>(this.Resources, new Shaders.Compiler.ShaderSignature(Enumerable.Empty<(Type, string)>()));
            Shaders.Add(result);
        }

        return result;
    }
}