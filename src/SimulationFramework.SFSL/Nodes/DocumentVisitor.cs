using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

public abstract class DocumentVisitor
{
    public virtual void Visit(CompilationUnitNode node) { }
    public virtual void Visit(TextureNode node) { }
    public virtual void Visit(BufferNode node) { }
    public virtual void Visit(TokenNode node) { }
    public virtual void Visit(UnexpectedTokenNode node) { }
    public virtual void Visit(StructNode node) { }
    public virtual void Visit(VariableNode node) { }
    public virtual void Visit(BlockStatementNode node) { }
    public virtual void Visit(BinaryExpression expression) { }
    public virtual void Visit(LiteralExpression expression) { }
    public virtual void Visit(UnaryExpression expression) { }
    public virtual void Visit(NestedExpression expression) { }
}
