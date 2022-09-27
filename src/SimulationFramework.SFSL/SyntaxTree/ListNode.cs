using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage.SyntaxTree;

internal class ListNode<T> : IEnumerable<T> where T : Node
{
    private readonly List<T> backingList = new();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator() => backingList.GetEnumerator();

    public static ListNode<T> Parse(ref TokenStream stream, ParserDelegate<T> elementParser, Func<TokenStream, bool> shouldParseElement)
    {
        var result = new ListNode<T>();

        while (shouldParseElement(stream))
        {
            result.backingList.Add(elementParser(ref stream));
        }

        return result;
    }
}