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
    private List<CompiledVariable> inputs;
    private List<CompiledVariable> outputs;
    private List<CompiledVariable> uniforms;
    private List<CompiledVariable> statics;

    public CompiledMethod EntryPoint;

    public ShaderSignature InputSignature;
    public ShaderSignature OutputSignature;

    public IEnumerable<CompiledMethod> Methods => methods;
    public IEnumerable<CompiledStruct> Structs => structs;
    public IEnumerable<CompiledVariable> Inputs => inputs;
    public IEnumerable<CompiledVariable> Outputs => outputs;
    public IEnumerable<CompiledVariable> Statics => statics;
    public IEnumerable<CompiledVariable> Uniforms => uniforms;
    public IEnumerable<CompiledVariable> Variables => Inputs.Concat(outputs).Concat(statics).Concat(uniforms);

    public ShaderCompilation(ShaderKind shaderKind, IEnumerable<CompiledMethod> methods, IEnumerable<CompiledStruct> structs, IEnumerable<CompiledVariable> inputs, IEnumerable<CompiledVariable> outputs, IEnumerable<CompiledVariable> uniforms, IEnumerable<CompiledVariable> statics)
    {
        this.ShaderKind = shaderKind;
        this.methods = new(methods);
        this.structs = new(structs);
        this.inputs = new(inputs);
        this.outputs = new(outputs);
        this.uniforms = new(uniforms);
        this.statics = new(statics);
    }
}
