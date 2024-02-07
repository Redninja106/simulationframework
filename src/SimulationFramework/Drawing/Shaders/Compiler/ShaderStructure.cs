using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class ShaderStructure
{
    public Type StructType { get; private set; }
    public IEnumerable<FieldInfo> Fields { get; private set; }

    public ShaderStructure(Type structType)
    {
        StructType = structType;
        Fields = structType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
