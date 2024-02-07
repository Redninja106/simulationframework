using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class ShaderUniform
{
    public FieldInfo BackingField;
    public Type UniformType;
    public string Name;

    public ShaderUniform(FieldInfo field)
    {
        this.BackingField = field;
        this.UniformType = field.FieldType;
        this.Name = field.Name;
    }
}
