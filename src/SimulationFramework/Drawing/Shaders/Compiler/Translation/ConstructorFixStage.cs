using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
//internal class ConstructorFixStage : CompilerStage
//{
//    public ConstructorFixStage(CompilerContext context) : base(context)
//    {
//    }

//    public override void Run(ShaderCompilation compilation)
//    {
//        foreach (var method in compilation.Methods)
//        {
//            if (method.Method is ConstructorInfo)
//            {
//                var selfParameter = method.Parameters[0];
//                method.Parameters.Remove(selfParameter);

//                var selfLocal = new LocalVariable("self", selfParameter.ParameterType);
//                method.Locals.Add(selfLocal);

//                SelfParameterReplacementVisitor visitor = new(selfParameter, selfLocal);
//                method.VisitBody(visitor);
//            }
//        }
//    }

//    private class SelfParameterReplacementVisitor : ExpressionVisitor
//    {
//        public SelfParameterReplacementVisitor(MethodParameter parameterToReplace, LocalVariable local)
//        {
//            ParameterToReplace = parameterToReplace;
//            Local = local;
//        }

//        public MethodParameter ParameterToReplace { get; set; }
//        public LocalVariable Local { get; set; }

//        public override ShaderExpression VisitMethodParameterExpression(MethodParameterExpression expression)
//        {
//            if (expression.Parameter == ParameterToReplace)
//            {
//                return base.VisitLocalVariableExpression(new(Local));
//            }

//            return base.VisitMethodParameterExpression(expression);
//        }

//        public override ShaderExpression VisitReturnExpression(ReturnExpression expression)
//        {
//            if (expression.ReturnValue is null)
//            {
//                return base.VisitReturnExpression(new(new LocalVariableExpression(Local)));
//            }

//            return base.VisitReturnExpression(expression);
//        }
//    }
//}
