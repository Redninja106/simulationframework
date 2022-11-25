using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class CompilationContext
{
    public ShaderKind kind;
    public ShaderCompiler Compiler;

    public Queue<MethodBase> methodCompileQueue = new();
    public Queue<Type> structCompileQueue = new();

    public List<CompiledStruct> structs = new();
    public List<CompiledMethod> methods = new();
    public List<CompiledVariable> uniforms = new();
    public List<CompiledVariable> statics = new();
    public List<CompiledVariable> inputs = new();
    public List<CompiledVariable> outputs = new();
    public IEnumerable<CompiledVariable> AllVariables => uniforms.Concat(statics).Concat(inputs).Concat(outputs);

    public Type ShaderType;
    public CompiledMethod EntryPoint;

    public CompilationContext(ShaderCompiler shaderCompiler)
    {
        this.Compiler = shaderCompiler;
    }

    public ShaderCompilation GetResult()
    {
        return new ShaderCompilation(this.kind, methods, structs, inputs, outputs, uniforms, statics) 
        { 
            EntryPoint = this.EntryPoint, 
            InputSignature = new ShaderSignature(inputs.Select(v => (v.VariableType, v.Name))),
            OutputSignature = new ShaderSignature(outputs.Select(v => (v.VariableType, v.Name))) 
        };
    }
}
