using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SFSLPrototype.Nodes;

namespace SFSLPrototype.Emit.GLSL;

internal class GlslEmitter : Emitter
{
    public GlslEmitter(TextWriter writer) : base(writer)
    {
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override void Visit(VariableNode node)
    {
        
    }
}
