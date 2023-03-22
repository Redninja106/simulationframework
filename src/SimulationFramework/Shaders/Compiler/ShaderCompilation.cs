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
    private List<ShaderVariable> inputs;
    private List<ShaderVariable> outputs;
    private List<ShaderVariable> uniforms;
    private List<ShaderVariable> intrinsicUniforms;
    private List<ShaderVariable> globals;

    public CompiledMethod EntryPoint;

    public ShaderSignature InputSignature;
    public ShaderSignature OutputSignature;

    public IEnumerable<CompiledMethod> Methods => methods;
    public IEnumerable<CompiledStruct> Structs => structs;
    public IEnumerable<ShaderVariable> Inputs => inputs;
    public IEnumerable<ShaderVariable> Outputs => outputs;
    public IEnumerable<ShaderVariable> Globals => globals;
    public IEnumerable<ShaderVariable> IntrinsicUniforms => intrinsicUniforms;
    public IEnumerable<ShaderVariable> Uniforms => uniforms;
    public IEnumerable<ShaderVariable> AllUniforms => uniforms.Concat(intrinsicUniforms);
    public IEnumerable<ShaderVariable> Variables => Inputs.Concat(outputs).Concat(globals).Concat(AllUniforms);

    public ShaderCompilation(ShaderKind shaderKind, IEnumerable<CompiledMethod> methods, IEnumerable<CompiledStruct> structs, IEnumerable<ShaderVariable> inputs, IEnumerable<ShaderVariable> outputs, IEnumerable<ShaderVariable> uniforms, IEnumerable<ShaderVariable> intrinsicUniforms, IEnumerable<ShaderVariable> globals)
    {
        this.ShaderKind = shaderKind;
        this.methods = new(methods);
        this.structs = new(structs);
        this.inputs = new(inputs);
        this.outputs = new(outputs);
        this.uniforms = new(uniforms);
        this.intrinsicUniforms = new(intrinsicUniforms);
        this.globals = new(globals);
    }

    public IEnumerable<ShaderVariable> GetInputs(InputSemantic semantic)
    {
        return inputs.Where(i => i.InputSemantic == semantic);
    }
}
