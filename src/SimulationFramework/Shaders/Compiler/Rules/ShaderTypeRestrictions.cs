using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler.Rules;

internal class ShaderTypeRestrictions : CompilerRule
{
    public override void CheckStruct(CompilationContext context, CompiledStruct compiledStruct)
    {
        CheckType(context, compiledStruct.StructType);

        foreach (var field in compiledStruct.Fields)
        {
            CheckType(context, field.FieldType);
        }

        base.CheckStruct(context, compiledStruct);
    }

    public override void CheckVariable(CompilationContext context, CompiledVariable compiledVariable)
    {

        base.CheckVariable(context, compiledVariable);
    }

    private void CheckType(CompilationContext context, Type type)
    {
        if (type == context.ShaderType)
            context.AddError("Shader type not allowed!");
    }
}
