using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

internal class Lexer
{
    string source;

    public Lexer(string source)
    {
        this.source = source;
    }

    public TokenNode[] GetTokens()
    {
        return Tokenize(source);
    }

    private static TokenNode[] Tokenize(string src)
    {
        bool IsPunctuation(string s, int i)
        {
            return char.IsPunctuation(s, i) || s[i] == '>' || s[i] == '<';
        }

        bool IsIndependent(char c)
        {
            switch (c)
            {
                case '{':
                case '}':
                case '(':
                case ')':
                    return true;
                default:
                    return false;
            }
        }

        List<TokenNode> result = new();

        string token = "";
        for (int i = 0; i < src.Length; i++)
        {
            if (char.IsWhiteSpace(src[i]))
                continue;

            token += src[i];

            if (token == "//")
            {
                while (src[i] != '\n') i++;
                token = "";
                continue;
            }

            if (IsIndependent(token[0]) || i >= src.Length - 1 || char.IsWhiteSpace(src[i + 1]) || (IsPunctuation(token, 0) != IsPunctuation(src, i + 1)))
            {
                result.Add(new(token, i));
                token = "";
            }
        }

        return result.ToArray();
    }
}
