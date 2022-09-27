using System;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

class FunctionNode : Node
{
    public override void Accept(SyntaxTreeVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public override void VisitChildren(SyntaxTreeVisitor visitor)
    {
        throw new NotImplementedException();
    }
}