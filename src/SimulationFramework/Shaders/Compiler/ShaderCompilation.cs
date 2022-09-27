using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class ShaderCompilation
{
    public ShaderKind ShaderKind; 
    
    private List<CompiledMethod> methods;
    private List<CompiledStruct> structs;
    private List<CompiledVariable> variables;

    public CompiledMethod EntryPoint;
    

    public IEnumerable<CompiledMethod> Methods => methods;
    public IEnumerable<CompiledStruct> Structs => structs;
    public IEnumerable<CompiledVariable> Variables => variables;

    public ShaderCompilation(ShaderKind shaderKind, IEnumerable<CompiledMethod> methods, IEnumerable<CompiledStruct> structs, IEnumerable<CompiledVariable> variables)
    {
        this.ShaderKind = shaderKind;
        this.methods = new(methods);
        this.structs = new(structs);
        this.variables = new(variables);
    }
}
