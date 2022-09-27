using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal abstract class Node
{
    public abstract void Accept(SyntaxTreeVisitor visitor);
    public abstract void VisitChildren(SyntaxTreeVisitor visitor);
}