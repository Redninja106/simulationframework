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
    public bool IsInput => Attribute is ShaderInAttribute;
    public bool IsOutput => Attribute is ShaderOutAttribute;
    public bool IsUniform => Attribute is null;

    public Attribute? Attribute;

    public CompiledVariable(FieldInfo field)
    {
        this.BackingField = field;
        this.VariableType = field.FieldType;
        this.Name = field.Name;

        this.Attribute = field.GetCustomAttribute<ShaderInAttribute>() as Attribute ?? field.GetCustomAttribute<ShaderOutAttribute>();
    }
}
