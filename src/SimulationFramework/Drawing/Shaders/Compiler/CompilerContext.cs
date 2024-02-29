using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;


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