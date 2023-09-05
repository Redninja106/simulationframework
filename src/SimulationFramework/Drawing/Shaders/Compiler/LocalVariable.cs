using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class LocalVariable
{
    public LocalVariableInfo? Info { get; }
    public string Name { get; }
    public Type VariableType { get; }

    public LocalVariable(string name, Type type)
    {
        Name = name;
        VariableType = type;
    }

    public LocalVariable(LocalVariableInfo info)
    {
        Info = info;
        Name = "var" + info.LocalIndex;
        VariableType = info.LocalType;
    }

}
