using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

/// <summary>
/// Shared state between compiler stages
/// </summary>
public class CompilationContextOLD
{
    public CanvasShaderCompiler Compiler;
    internal ShaderIntrinsicsManager Intrinsics { get; }

    public Queue<MethodBase> methodCompileQueue = new();
    public Queue<Type> structCompileQueue = new();

    public List<ShaderStructure> structs = new();
    public List<ShaderMethod> methods = new();
    public List<ShaderUniform> uniforms = new();

    public Type ShaderType;
    public ShaderMethod EntryPoint;

    public MethodBase CurrentMethod { get; internal set; }

    public CompilationContextOLD(CanvasShaderCompiler shaderCompiler)
    {
        this.Compiler = shaderCompiler;
    }

    public ShaderCompilation GetResult()
    {
        throw new NotImplementedException();
        // return new ShaderCompilation(EntryPoint, methods, structs, uniforms);
    }

    public bool IsGlobalMethod(ShaderMethod method)
    {
        return method.Method.DeclaringType == EntryPoint.Method.DeclaringType;
    }

    public void AddMethod(MethodBase method)
    {
        methodCompileQueue.Enqueue(method);
    }

    public void AddType(Type type)
    {
        structCompileQueue.Enqueue(type);
    }

    public bool ContainsMethod(MethodInfo method)
    {
        return methods.Any(s => s.Method == method);
    }

    public bool ContainsType(Type type)
    {
        return structs.Any(s => s.StructType == type);
    }
}

internal class CompilerContext
{
    public MethodBase? CurrentMethod { get; private set; }
    public CompilationQueue CompilationQueue { get; }
    public Type ShaderType { get; }
    internal ShaderIntrinsicsManager Intrinsics { get; }

    public CompilerContext(Type shaderType)
    {
        CompilationQueue = new(this);
        Intrinsics = new();
        ShaderType = shaderType;
    }

    public void SetCurrentMethod(MethodBase method)
    {
        this.CurrentMethod = method;
    }
}

internal class CompilationQueue
{
    private readonly Queue<MethodBase> queue = new();
    private readonly CompilerContext context;

    public CompilationQueue(CompilerContext context)
    {
        this.context = context;
    }

    public void EnqueueMethod(MethodBase method)
    {
        Debug.Assert(!ContainsMethod(method));
        queue.Enqueue(method);
    }

    public bool ContainsMethod(MethodBase method)
    {
        return queue.Contains(method);
    }

    public bool TryDequeueMethod(out MethodBase method)
    {
        return queue.TryDequeue(out method);
    }
}