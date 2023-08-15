using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class ShaderCompilation
{
    private List<CompiledMethod> methods;
    private List<CompiledStruct> structs;
    private List<ShaderVariable> uniforms;

    public CompiledMethod EntryPoint;

    public IEnumerable<CompiledMethod> Methods => methods;
    public IEnumerable<CompiledStruct> Structs => structs;
    public IEnumerable<ShaderVariable> Uniforms => uniforms;

    public ShaderCompilation(IEnumerable<CompiledMethod> methods, IEnumerable<CompiledStruct> structs, IEnumerable<ShaderVariable> uniforms)
    {
        this.methods = new(methods);
        this.structs = new(structs);
        this.uniforms = new(uniforms);
    }
}
