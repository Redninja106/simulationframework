//using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
//internal class UniformAccessReplacementStage : CompilerStage
//{
//    public UniformAccessReplacementStage(CompilerContext context) : base(context)
//    {
//    }

//    public override void Run(ShaderCompilation compilation)
//    {
//        Visitor visitor = new(context, compilation);
//        foreach (var method in compilation.Methods)
//        {
//            method.VisitBody(visitor);
//        }
//    }

//    private class Visitor : ExpressionVisitor
//    {
//        private readonly CompilerContext context;
//        private readonly ShaderCompilation compilation;

//        public Visitor(CompilerContext context, ShaderCompilation compilation)
//        {
//            this.context = context;
//            this.compilation = compilation;
//        }

//        public override ShaderExpression VisitMemberAccessExpression(MemberAccessExpression expression)
//        {
//            if (expression.Member.DeclaringType == context.ShaderType)
//            {
//                return base.VisitUniformExpression(new(compilation.GetUniform(expression.Member as FieldInfo ?? throw new())));
//            }

//            return base.VisitMemberAccessExpression(expression);
//        }
//    }
//}
