using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
//internal class DoubleEqualityFixStage : CompilerStage
//{
//    public DoubleEqualityFixStage(CompilerContext context) : base(context)
//    {
//    }

//    public override void Run(ShaderCompilation compilation)
//    {
//        Visitor visitor = new();

//        foreach (var method in compilation.Methods)
//        {
//            method.VisitBody(visitor);
//        }
//    }

//    private class Visitor : ExpressionVisitor
//    {
//        public override ShaderExpression VisitBinaryExpression(BinaryExpression expression)
//        {
//            if (expression.Operation == BinaryOperation.Equal && expression.RightOperand is ConstantExpression constExpr && constExpr.Value is 0)
//            {
//                return expression.LeftOperand;
//            }

//            return base.VisitBinaryExpression(expression);
//        }
//    }
//}
