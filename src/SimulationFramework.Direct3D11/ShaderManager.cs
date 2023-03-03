using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
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

    public D3D11ComputeShader GetComputeShader(Type shaderType)
    {
        var result = Shaders.OfType<D3D11ComputeShader>().SingleOrDefault();

        if (result is null)
        {
            result = new D3D11ComputeShader(this.Resources, shaderType);
            Shaders.Add(result);
        }

        return result;
    }

    public D3D11GeometryShader GetGeometryShader(Type shaderType, ShaderSignature signature)
    {
        var result = Shaders.OfType<D3D11GeometryShader>().SingleOrDefault();

        if (result is null)
        {
            result = new D3D11GeometryShader(this.Resources, shaderType, signature);
            Shaders.Add(result);
        }

        return result;
    }

    public D3D11VertexShader GetVertexShader(Type shaderType)
    {
        var result = Shaders.OfType<D3D11VertexShader>().SingleOrDefault();

        if (result is null)
        {
            result = new D3D11VertexShader(this.Resources, shaderType);
            Shaders.Add(result);
        }

        return result;
    }

    public D3D11FragmentShader GetFragmentShader(Type shaderType, ShaderSignature shaderSignature)
    {
        var result = Shaders.OfType<D3D11FragmentShader>().SingleOrDefault();

        if (result is null)
        {
            result = new D3D11FragmentShader(this.Resources, shaderType, shaderSignature);
            Shaders.Add(result);
        }

        return result;
    }
}