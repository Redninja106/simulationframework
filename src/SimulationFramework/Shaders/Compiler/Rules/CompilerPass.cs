using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Rules;
internal abstract class CompilerPass : ExpressionVisitor
{
    public virtual bool AppliesTo(ShaderKind shaderKind) => true;

    public virtual void CheckMethod(CompilationContext context, CompiledMethod compiledMethod) { }
    public virtual void CheckStruct(CompilationContext context, CompiledStruct compiledStruct) { }
    public virtual void CheckVariable(CompilationContext context, CompiledVariable compiledVariable) { }
}