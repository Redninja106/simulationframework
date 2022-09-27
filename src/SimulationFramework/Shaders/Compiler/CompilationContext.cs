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

    public List<CompilerMessage> messages = new();

    public Queue<MethodBase> methodCompileQueue = new();
    public Queue<Type> structCompileQueue = new();

    public List<CompiledStruct> structs = new();
    public List<CompiledMethod> methods = new();
    public List<CompiledVariable> variables = new();

    public Type ShaderType;
    public CompiledMethod EntryPoint;

    public CompilationContext(ShaderCompiler shaderCompiler)
    {
        this.Compiler = shaderCompiler;
    }

    public bool HasErrors()
    {
        return messages.Any(m => m.Severity is CompilationMessageSeverity.Error);
    }

    public ShaderCompilation? GetResult()
    {
        if (HasErrors())
            return null;

        return new ShaderCompilation(this.kind, methods, structs, variables) { EntryPoint = this.EntryPoint };
    }

    public void AddError(string message)
    {
        messages.Add(new() { Message = message, Severity = CompilationMessageSeverity.Error });
    }
}
