using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

internal class TokenReader
{
    private readonly Stack<int> locations = new();
    private readonly TokenNode[] tokens;

    public int Location
    {
        get => locations.Peek();
        set
        {
            locations.Pop(); 
            locations.Push(value);
        }
    }

    public TokenNode Current => locations.Peek() < 0 ? null : tokens[locations.Peek()];

    public int CountRemaining => tokens.Length - (locations.Peek() + 1);

    public bool IsAtEnd => (locations.Peek() + 1) >= tokens.Length;

    public TokenReader(TokenNode[] tokens)
    {
        this.tokens = tokens;
        locations.Push(0);
    }

    public TokenNode PeekToken(int offset = 0)
    {
        if (Location + offset >= tokens.Length)
            return null;

        return tokens[Location + offset];
    }

    public TokenNode EatToken()
    {
        var t = Current;
        Location++;
        return t;
    }

    public TokenNode EatToken(TokenKind kind)
    {
        return EatTokenOrNull(kind) ?? TokenNode.CreateMissing(kind);
    }

    public TokenNode EatTokenOrNull(TokenKind kind)
    {
        if (Current.Kind != kind)
            return null;
        return EatToken();
    }

    //public Token GetNext(int amount = 1)
    //{
    //    return tokens[locations.Peek() + amount];
    //}

    //public Token MoveNext()
    //{
    //    return TryMoveNext(out Token token) ? token : throw new Exception("There are no more tokens!"); 
    //}

    //public bool TryMoveNext(out Token token)
    //{
    //    token = null;

    //    if (IsAtEnd)
    //        return false;

    //    locations.Push(locations.Pop() + 1);

    //    token = tokens[locations.Peek()];
    //    return true;
    //}

    public void PushLocation()
    {
        locations.Push(locations.Peek());
    }

    public int PopLocation()
    {
        return locations.Pop();
    }

    public TokenReader Subsection(int length)
    {
        return new TokenReader(tokens[Location..(Location + length)]);
    }
}
