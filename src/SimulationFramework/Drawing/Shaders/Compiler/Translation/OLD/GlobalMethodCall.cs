﻿using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation.OLD;
internal class GlobalMethodCall : CompilerPass
{
    //private CompilationContext context;
    //public override void CheckMethod(CompilationContext context, ShaderMethod compiledMethod)
    //{
    //    this.context = context;
    //    compiledMethod.TransformBody(this);

    //    base.CheckMethod(context, compiledMethod);
    //}

    //protected override Expression VisitCompiledMethodCallExpression(CompiledMethodCallExpression methodCall)
    //{
    //    if (context.IsGlobalMethod(methodCall.Method))
    //    {
    //        // remove this argument
    //        return new CompiledMethodCallExpression(methodCall.Method, methodCall.Arguments.Skip(1));
    //    }

    //    return base.VisitCompiledMethodCallExpression(methodCall);
    //}
}