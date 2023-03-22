using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;
using Vortice.Direct3D11;
using Vortice.DXGI;

namespace SimulationFramework.Drawing.Direct3D11;

internal class InputLayoutProvider : D3D11Object
{
    private readonly Dictionary<(VertexShader, bool), ID3D11InputLayout> inputLayouts;

    public InputLayoutProvider(DeviceResources deviceResources) : base(deviceResources)
    {
        inputLayouts = new();
    }

    public ID3D11InputLayout GetInputLayout(VertexShader vertexShader, bool instanced)
    {
        if (!inputLayouts.TryGetValue((vertexShader, instanced), out var result))
        {
            result = CreateInputLayout(vertexShader, instanced);
            inputLayouts.Add((vertexShader, instanced), result);
        }

        return result;
    }

    private ID3D11InputLayout CreateInputLayout(VertexShader vertexShader, bool instanced)
    {
        var inputElementDescs = Enumerable.Empty<InputElementDescription>();
        var vertexVars = vertexShader.Compilation.GetInputs(InputSemantic.VertexElement);

        inputElementDescs = inputElementDescs.Concat(CreateInputElementDescriptions(vertexVars, 0, InputClassification.PerVertexData));

        var instanceVars = vertexShader.Compilation.GetInputs(InputSemantic.InstanceElement);
        if (instanced)
        {
            inputElementDescs = inputElementDescs.Concat(CreateInputElementDescriptions(instanceVars, 1, InputClassification.PerInstanceData));
        }

        return Resources.Device.CreateInputLayout(inputElementDescs.ToArray(), vertexShader.bytecode);
    }

    private static IEnumerable<InputElementDescription> CreateInputElementDescriptions(IEnumerable<ShaderVariable> variables, int slot, InputClassification classification)
    {
        Stack<(Type, string)> typeStack = new();

        foreach (var variable in variables.Reverse())
        {
            typeStack.Push((variable.SourceType ?? variable.VariableType, variable.Name));
        }

        InputElementDescription baseDesc = new(string.Empty, 0, Format.Unknown, InputElementDescription.AppendAligned, slot, classification, 0);

        while (typeStack.Any())
        {
            var (type, name) = typeStack.Pop();
            
            if (char.IsNumber(name.Last()))
            {
                name += '_';
            }
            
            baseDesc.SemanticName = name;

            if (type == typeof(int))
            {
                yield return baseDesc with { Format = Format.R32_SInt };
            }
            else if (type == typeof(uint))
            {
                yield return baseDesc with { Format = Format.R32_UInt };
            }
            else if (type == typeof(float))
            {
                yield return baseDesc with { Format = Format.R32_Float };
            }
            else if (type == typeof(Vector2))
            {
                yield return baseDesc with { Format = Format.R32G32_Float };
            }
            else if (type == typeof(Vector3))
            {
                yield return baseDesc with { Format = Format.R32G32B32_Float };
            }
            else if (type == typeof(Vector4))
            {
                yield return baseDesc with { Format = Format.R32G32B32A32_Float };
            }
            else if (type == typeof(Matrix4x4))
            {
                yield return baseDesc with { Format = Format.R32G32B32A32_Float, SemanticIndex = 0 };
                yield return baseDesc with { Format = Format.R32G32B32A32_Float, SemanticIndex = 1 };
                yield return baseDesc with { Format = Format.R32G32B32A32_Float, SemanticIndex = 2 };
                yield return baseDesc with { Format = Format.R32G32B32A32_Float, SemanticIndex = 3 };
            }
            else if (type == typeof(Matrix3x2))
            {
                yield return baseDesc with { Format = Format.R32G32B32_Float, SemanticIndex = 0 };
                yield return baseDesc with { Format = Format.R32G32B32_Float, SemanticIndex = 1 };
            }
            else if (type == typeof(ColorF))
            {
                yield return baseDesc with { Format = Format.R32G32B32A32_Float };
            }
            else if (type == typeof(Color))
            {
                yield return baseDesc with { Format = Format.R8G8B8A8_UNorm };
            }
            else
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var field in fields.Reverse())
                {
                    typeStack.Push((field.FieldType, field.Name));
                }
            }
        }
    }

    public override void Dispose()
    {
        foreach (var (_, inputLayout) in inputLayouts)
        {
            inputLayout.Dispose();
        }
    }
}