using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class ShaderVariable
{
    public FieldInfo BackingField;
    public Type VariableType;
    public string Name;

    public Type SourceType;

    public ShaderVariable(FieldInfo field)
    {
        this.BackingField = field;
        this.VariableType = field.FieldType;
        this.Name = field.Name;
    }
}
