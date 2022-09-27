using SimulationFramework.ShaderLanguage.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage;

internal static class Scanner
{
    const string MULTILINE_COMMENT_BEGIN = "/*";
    const string MULTILINE_COMMENT_END = "*/";
    const string SINGLE_LINE_COMMENT_BEGIN = "//";
    const string SINGLE_LINE_COMMENT_END = "\n";

    private static readonly Dictionary<string, TokenKind> keywords = new()
    {
        { "var",     TokenKind.Var     },
        { "texture", TokenKind.Texture },
        { "struct",  TokenKind.Struct  },
        { "buffer",  TokenKind.Buffer  },

        { "void",    TokenKind.Void    },
        { "int",     TokenKind.Int     },
        { "uint",    TokenKind.UInt    },
        { "double",  TokenKind.Double  },
        { "float",   TokenKind.Float   },
        { "Vector2", TokenKind.Vector2 },
        { "Vector3", TokenKind.Vector3 },
        { "Vector4", TokenKind.Vector4 },
        { "long",    TokenKind.Long    },
        { "ulong",   TokenKind.ULong   },
        { "half",    TokenKind.Half    },
    };

    private static readonly Dictionary<string, TokenKind> symbols = new()
    {
        { "*", TokenKind.Star },
        { "(", TokenKind.OpenParenthesis },
        { ")", TokenKind.CloseParenthesis },
        { "{", TokenKind.OpenBrace },
        { "}", TokenKind.CloseBrace },
        { "[", TokenKind.OpenBracket },
        { "]", TokenKind.CloseBracket },
        { ";", TokenKind.Semicolon },
        { ",", TokenKind.Comma },
        { ".", TokenKind.Dot },
        { ":", TokenKind.Colon },

        { "+", TokenKind.Plus },
        { "-", TokenKind.Minus },
        { "*", TokenKind.Star },
        { "/", TokenKind.Slash },
        { "%", TokenKind.Percent },

        // assignment
        { "++", TokenKind.PlusPlus },
        { "--", TokenKind.MinusMinus },
        { "=", TokenKind.Equal },
        { "+=", TokenKind.PlusEqual },
        { "-=", TokenKind.MinusEqual },
        { "*=", TokenKind.StarEqual },
        { "/=", TokenKind.SlashEqual },
        { "%=", TokenKind.PercentEqual },
        { "&=", TokenKind.AndEqual },
        { "|=", TokenKind.OrEqual },
        { "^=", TokenKind.HatEqual },
        { "~=", TokenKind.TildeEqual },
        { "<<=", TokenKind.ShiftLeftEqual },
        { ">>=", TokenKind.ShiftRightEqual },

        // bitwise
        { "&", TokenKind.And },
        { "|", TokenKind.Or },
        { "^", TokenKind.Hat },
        { "~", TokenKind.Tilde },
        { "<<", TokenKind.ShiftLeft },
        { ">>", TokenKind.ShiftRight },

        // logical
        { "&&", TokenKind.AndAnd },
        { "||", TokenKind.OrOr },
        { "!", TokenKind.Not },

        // comparison
        { "==", TokenKind.EqualEqual },
        { "!=", TokenKind.NotEqual },
        { "<", TokenKind.LessThan },
        { "<=", TokenKind.LessThanEqual },
        { ">", TokenKind.GreaterThan },
        { ">=", TokenKind.GreaterThanEqual },
    };

    public static TokenStream Scan(string source)
    {
        List<TokenNode> tokens = new();

        int lastTokenEnd = 0;
        int tokenStartPosition = 0;
        int position = 0;

        while (position < source.Length)
        {
            char current = source[position];

            if (tokenStartPosition == position)
            {
                // if current token length is 0, take any char
                position++;

                // move past this char if its whitespace
                if (char.IsWhiteSpace(current))
                {
                    tokenStartPosition = position;
                }

                continue;
            }

            // break token on whitespace if current token doesn't include it
            if (char.IsWhiteSpace(current) && !IncludesWhitespace(source[tokenStartPosition..position]))
            {
                var leadingTrivia = lastTokenEnd..tokenStartPosition;
                var range = tokenStartPosition..position;
                var kind = Categorize(source[range]);

                int trailingEnd = position;
                while (position < source.Length && char.IsWhiteSpace(source[trailingEnd++])) ;

                var trailingTrivia = position..trailingEnd;

                if (kind != TokenKind.Comment)
                    tokens.Add(new(kind, range, leadingTrivia, trailingTrivia));

                tokenStartPosition = position;
                lastTokenEnd = position;

                continue;
            }

            if (ContinuesToken(source.AsSpan().Slice(tokenStartPosition, position - tokenStartPosition), current))
            {
                position++;
            }
            else
            {
                var leadingTrivia = lastTokenEnd..tokenStartPosition;
                var range = tokenStartPosition..position;
                var kind = Categorize(source[range]);

                int trailingEnd = position;
                while (position < source.Length && char.IsWhiteSpace(source[trailingEnd++])) ;

                var trailingTrivia = position..trailingEnd;

                if (kind != TokenKind.Comment)
                    tokens.Add(new(kind, range, leadingTrivia, trailingTrivia));

                tokenStartPosition = position;
                lastTokenEnd = position;

                continue;
            }
        }

        if (tokenStartPosition < position)
        {
            var leadingTrivia = lastTokenEnd..tokenStartPosition;
            var range = new Range(tokenStartPosition, position);
            var kind = Categorize(source[range]);

            int trailingEnd = position;
            while (position < source.Length && char.IsWhiteSpace(source[trailingEnd++])) ;

            var trailingTrivia = position..trailingEnd;

            if (kind != TokenKind.Comment)
                tokens.Add(new(kind, range, leadingTrivia, trailingTrivia));
        }

        CompilationContext context = new(source);
        return new TokenStream(context, tokens.ToArray());
    }

    internal static bool IncludesWhitespace(ReadOnlySpan<char> token)
    {
        if (token.StartsWith(MULTILINE_COMMENT_BEGIN))
            return !token.EndsWith(MULTILINE_COMMENT_END);

        if (token.StartsWith(SINGLE_LINE_COMMENT_BEGIN))
            return !token.EndsWith(SINGLE_LINE_COMMENT_END);

        if (token.StartsWith("\""))
            return !token.EndsWith("\"");

        if (token.StartsWith("'"))
            return !token.EndsWith("'");

        return false;
    }

    internal static bool ContinuesToken(ReadOnlySpan<char> token, char c)
    {
        // identifers & keywords
        if (char.IsLetter(token[0]) || token[0] is '@')
            return char.IsLetterOrDigit(c);

        // numbers
        if (token[0] is '+' or '-' || char.IsDigit(token[0]))
            return char.IsDigit(c) || (c is '.' && !token.Contains('.'));

        // parse 2nd char of comment start syntax ('/*' or '//')
        if (token[0] is '/' && token.Length is 1)
            return c is '*' or '/';

        // multiline comments
        if (token.StartsWith(MULTILINE_COMMENT_BEGIN))
            return !token.EndsWith(MULTILINE_COMMENT_END);

        // single line comments
        if (token.StartsWith(SINGLE_LINE_COMMENT_BEGIN))
            return !token.EndsWith(SINGLE_LINE_COMMENT_END);

        // string literals
        if (token[0] is '"')
            return token.Length <= 1 || token[^1] is not '"';

        // char literals
        if (token[0] is '\'')
            return token.Length <= 1 || token[^1] is not '\'';

        if (symbols.ContainsKey(new string(token) + c))
            return true;

        return false;
    }

    internal static TokenKind Categorize(ReadOnlySpan<char> token)
    {
        foreach (var (key, value) in keywords)
        {
            if (token.SequenceEqual(key))
                return value;
        }

        if (char.IsLetter(token[0]) || token[0] is '@')
        {
            for (int i = 1; i < token.Length; i++)
            {
                if (!char.IsLetterOrDigit(token[i]))
                    break;
            }

            return TokenKind.Identifier;
        }

        if (char.IsDigit(token[0]))
        {
            for (int i = 1; i < token.Length; i++)
            {
                if (!char.IsDigit(token[i]))
                    break;
            }

            return TokenKind.NumericLiteral;
        }

        if (token[0] is '"' && token[^1] is '"')
            return TokenKind.StringLiteral;

        if (token[0] is '\'' && token[^1] is '\'')
            return TokenKind.CharLiteral;

        if (token.StartsWith(MULTILINE_COMMENT_BEGIN) || token.StartsWith(SINGLE_LINE_COMMENT_BEGIN))
            return TokenKind.Comment;

        foreach (var (key, value) in symbols)
        {
            if (token.SequenceEqual(key))
                return value;
        }

        return TokenKind.Unknown;
    }
}