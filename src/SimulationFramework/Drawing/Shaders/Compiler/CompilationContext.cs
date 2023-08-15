using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;
public class CompilationContext
{
    public CanvasShaderCompiler Compiler;

    public Queue<MethodBase> methodCompileQueue = new();
    public Queue<Type> structCompileQueue = new();

    public List<CompiledStruct> structs = new();
    public List<CompiledMethod> methods = new();
    public List<ShaderVariable> uniforms = new();

    public Type ShaderType;
    public CompiledMethod EntryPoint;

    public CompilationContext(CanvasShaderCompiler shaderCompiler)
    {
        this.Compiler = shaderCompiler;
    }

    public ShaderCompilation GetResult()
    {
        return new ShaderCompilation(methods, structs, uniforms)
        {
            EntryPoint = this.EntryPoint,
        };
    }

    public bool IsGlobalMethod(CompiledMethod method)
    {
        return method.Method.DeclaringType == EntryPoint.Method.DeclaringType;
    }
}
