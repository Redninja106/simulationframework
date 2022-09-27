using SimulationFramework.ShaderLanguage.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage;

internal struct TokenStream : IEquatable<TokenStream>
{
    private readonly CompilationContext context;
    private readonly TokenNode[] tokens;
    private int index;
    
    public CompilationContext Context => context;
    public bool IsAtEnd => index >= tokens.Length;

    public TokenStream(CompilationContext context, TokenNode[] items)
    {
        this.context = context;
        this.tokens = items;
        this.index = 0;
    }

    public void Seek(int offset)
    {
        index += offset;
    }

    public void Reset()
    {
        index = 0;
    }

    public TokenNode? Peek()
    {
        if (IsAtEnd)
            return null;

        return tokens[index];
    }

    public TokenNode? Read()
    {
        return IsAtEnd ? null : tokens[index++];
    }

    public TokenNode? Read(TokenKind kind)
    {
        return Read(t => t.Kind == kind);
    }
    
    public TokenNode? Read(Predicate<TokenNode> condition)
    {
        var token = Read();

        if (token is null)
        {
            return null;
        }

        if (condition(token))
        {
            return token;
        }

        Seek(-1);
        return null;
    }

    public TokenNode ReadOrMissing(TokenKind kind)
    {
        return Read(kind) ?? TokenNode.Missing(kind, GetCurrentLocationInSource());
    }

    public TokenNode ReadOrMissing(Predicate<TokenNode> condition, TokenKind expectedKind = TokenKind.Unknown)
    {
        return Read(condition) ?? TokenNode.Missing(expectedKind, GetCurrentLocationInSource());
    }

    public int GetCurrentLocationInSource()
    {
        if (this.index == 0)
            return 0;
        
        var current = this.tokens[index - 1];
        return current.Range.End.Value;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is TokenStream other)
            return Equals(other);

        return false;
    }

    public bool Equals(TokenStream other)
    {
        return this.index == other.index && this.tokens == other.tokens;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.context, this.tokens, this.index);
    }
}