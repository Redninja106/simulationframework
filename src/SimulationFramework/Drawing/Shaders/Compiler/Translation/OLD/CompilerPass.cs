using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;
internal abstract class CompilerPass
{
    public virtual void CheckMethod(CompilationContextOLD context, ShaderMethod compiledMethod) { }
    public virtual void CheckStruct(CompilationContextOLD context, ShaderStructure compiledStruct) { }
    public virtual void CheckVariable(CompilationContextOLD context, ShaderUniform compiledVariable) { }
}