using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal class D3D11VertexShader<T> : D3D11Shader<T> where T : struct, IShader
{
    protected override ShaderKind Kind => ShaderKind.Vertex;
    protected override string Profile => "vs_5_0";

    private ID3D11VertexShader vertexShader;
    private ID3D11InputLayout inputLayout;

    public D3D11VertexShader(DeviceResources deviceResources) : base(deviceResources, null)
    {
    }

    protected override void CreateShader(Span<byte> bytecode)
    {
        vertexShader = Resources.Device.CreateVertexShader(bytecode);

        CreateInputLayout(bytecode);
    }

    private void CreateInputLayout(Span<byte> bytecode)
    {
        var inputs = Compilation.Variables.Where(c => c.IsInput && c.InputSemantic is InputSemantic.None);

        var elements = new List<InputElementDescription>();
        Stack<(Type, string)> types = new();

        foreach (var input in inputs.Reverse())
        {
            types.Push((input.VariableType, input.Name));
        }

        while (types.Any())
        {
            var (type, name) = types.Pop();

            if (type == typeof(int))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32_SInt, 0));
            }
            else if (type == typeof(uint))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32_UInt, 0));
            }
            else if (type == typeof(float))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32_Float, 0));
            }
            else if (type == typeof(Vector2))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32G32_Float, 0));
            }
            else if (type == typeof(Vector3))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32G32B32_Float, 0));
            }
            else if (type == typeof(Vector4))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32G32B32A32_Float, 0));
            }
            else if (type == typeof(Matrix4x4))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32G32B32A32_Float, 0));
                elements.Add(new InputElementDescription(name, 1, Vortice.DXGI.Format.R32G32B32A32_Float, 0));
                elements.Add(new InputElementDescription(name, 2, Vortice.DXGI.Format.R32G32B32A32_Float, 0));
                elements.Add(new InputElementDescription(name, 3, Vortice.DXGI.Format.R32G32B32A32_Float, 0));
            }
            else if (type == typeof(Matrix3x2))
            {
                elements.Add(new InputElementDescription(name, 0, Vortice.DXGI.Format.R32G32B32_Float, 0));
                elements.Add(new InputElementDescription(name, 1, Vortice.DXGI.Format.R32G32B32_Float, 0));
            }
            else
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var field in fields.Reverse())
                {
                    types.Push((field.FieldType, field.Name));
                }
            }
        }

        inputLayout = Resources.Device.CreateInputLayout(elements.ToArray(), bytecode);
    }

    public override void ApplyConstantBuffer(ID3D11DeviceContext context, ID3D11Buffer constantBuffer)
    {
        context.VSSetConstantBuffer(0, constantBuffer);
    }

    public override void ApplyShader(ID3D11DeviceContext context)
    {
        context.IASetInputLayout(this.inputLayout);
        context.VSSetShader(vertexShader);
    }

    public override void Dispose()
    {
        vertexShader.Dispose();
        inputLayout.Dispose();
        base.Dispose();
    }
}
