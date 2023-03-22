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
    public List<ShaderVariable> uniforms = new();
    public List<ShaderVariable> intrinsicUniforms = new();
    public List<ShaderVariable> globals = new();
    public List<ShaderVariable> inputs = new();
    public List<ShaderVariable> outputs = new();
    public IEnumerable<ShaderVariable> AllVariables => uniforms.Concat(intrinsicUniforms).Concat(globals).Concat(inputs).Concat(outputs);

    public Type ShaderType;
    public CompiledMethod EntryPoint;

    public CompilationContext(ShaderCompiler shaderCompiler)
    {
        this.Compiler = shaderCompiler;
    }

    public ShaderCompilation GetResult()
    {
        return new ShaderCompilation(this.kind, methods, structs, inputs, outputs, uniforms, intrinsicUniforms, globals)
        {
            EntryPoint = this.EntryPoint,
            InputSignature = new ShaderSignature(inputs.Where(v => v.InputSemantic is InputSemantic.None).Select(v => (v.VariableType, v.Name))),
            OutputSignature = new ShaderSignature(outputs.Where(v => v.OutputSemantic is OutputSemantic.None).Select(v => (v.VariableType, v.Name)))
        };
    }

    public bool IsGlobalMethod(CompiledMethod method)
    {
        return method.Method.DeclaringType == EntryPoint.Method.DeclaringType;
    }
}
