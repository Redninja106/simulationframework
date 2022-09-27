using System;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class DelimitedNode<T> : Node
    where T : Node
{
    public T Node { get; set; }
    public TokenNode? Delimiter { get; set; }

    public bool HasDelimiter => Delimiter is not null;

    public DelimitedNode(T node, TokenNode? delimiter)
    {
        Node = node;
        Delimiter = delimiter;
    }

    public override void Accept(SyntaxTreeVisitor visitor)
    {
        VisitChildren(visitor);
    }

    public override void VisitChildren(SyntaxTreeVisitor visitor)
    {
        Node.Accept(visitor);
        Delimiter?.Accept(visitor);
    }
}