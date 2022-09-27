using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;
internal class SyntaxTreeVisitor
{
    public virtual void VisitFunction(FunctionNode function)
    {
        function.VisitChildren(this);
    }

    public virtual void VisitToken(TokenNode token)
    {
        token.VisitChildren(this);
    }

    public virtual void VisitTypeNode(TypeNode type)
    {
        type.VisitChildren(this);
    }

    public virtual void VisitVariable(GlobalVariableNode variable)
    {
        variable.VisitChildren(this);
    }
}
