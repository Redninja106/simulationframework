using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public static class LanguageInfo
{
    public static IReadOnlyList<string> Keywords => keywordsList;
    public static IReadOnlyList<Operator> Operators => operatorsList;

    private static readonly string[] keywordsList = new[]
    {
        "texture",
        "buffer",
        "in",
        "out",
        "struct",
        "void"
    };
    private static readonly Operator[] operatorsList = new Operator[]
    {
        new("+", Precedence.Additive),
        new("-", Precedence.Additive),
        new ("*", Precedence.Multiplicative),
        new ("/", Precedence.Multiplicative),
        new ("=", Precedence.Assignment),
    };
}
