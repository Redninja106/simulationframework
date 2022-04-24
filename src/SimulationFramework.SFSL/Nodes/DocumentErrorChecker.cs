using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype.Nodes;

internal class DocumentErrorChecker : DocumentVisitor
{
    private readonly CompilationContext context;

    public DocumentErrorChecker(CompilationContext context)
    {
        this.context = context;
    }

    public override void Visit(UnexpectedTokenNode node)
    {
        context.Notify(new CompilationNotification(1, $"Unexpected Token '{node.Unexpected.Value}'", NotificationSeverity.Error));

        base.Visit(node);
    }

    public override void Visit(TokenNode node)
    {
        if (node.IsMissing)
            context.Notify(new CompilationNotification(2, "Syntax Error", NotificationSeverity.Error));

        base.Visit(node);
    }
}
