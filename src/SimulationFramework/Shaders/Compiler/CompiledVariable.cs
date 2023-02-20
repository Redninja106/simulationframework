using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class CompiledVariable
{
    public FieldInfo BackingField;
    public Type VariableType;
    public string Name;

    public InputSemantic? InputSemantic;
    public OutputSemantic? OutputSemantic;

    public string InputName;
    public string OutputName;

    public bool IsUniform;
    public bool IsInput;
    public bool IsOutput;

    public CompiledVariable(FieldInfo field)
    {
        this.BackingField = field;
        this.VariableType = field.FieldType;
        this.Name = field.Name;

        var inputAttribute = field.GetCustomAttribute<InputAttribute>();
        this.IsInput = inputAttribute is not null;
        this.InputName = inputAttribute?.LinkageName ?? Name;
        this.InputSemantic = inputAttribute?.Semantic;

        var outputAttribute = field.GetCustomAttribute<OutputAttribute>();
        this.IsOutput = outputAttribute is not null;
        this.OutputName = outputAttribute?.LinkageName ?? Name;
        this.OutputSemantic = outputAttribute?.Semantic;

        this.IsUniform = field.GetCustomAttribute<UniformAttribute>() is not null;

        if (IsUniform && (IsOutput || IsInput))
        {
            throw new Exception("in/outs and uniforms are mutually exclusive!");
        }
    }
}
