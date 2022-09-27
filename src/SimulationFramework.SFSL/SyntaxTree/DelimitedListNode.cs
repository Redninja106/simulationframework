using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class DelimitedListNode<T> : ListNode<DelimitedNode<T>>
    where T : Node
{
    public static DelimitedListNode<T> Parse(ref TokenStream stream, ParserDelegate<T> parser, TokenKind delimiterKind)
    {
        return null;
    }
}