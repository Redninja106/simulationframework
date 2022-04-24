using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype.Nodes;

internal class Document
{
    public CompilationUnitNode RootNode { get; private set; }

    public Document(CompilationUnitNode root)
    {
        this.RootNode = root;
    }

    public void Accept(DocumentVisitor visitor)
    {
        RootNode.Accept(visitor);
    }
}
