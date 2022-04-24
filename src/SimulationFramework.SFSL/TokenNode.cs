using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFSLPrototype.Nodes;

namespace SFSLPrototype;

public record TokenNode : Node
{
    public string Value { get; private set; }
    public int EndIndex { get; private set; }
    public TokenKind Kind { get; private set; }

    public bool IsMissing => EndIndex == -1;

    public int StartIndex => EndIndex - Value.Length;
    public int Length => Value.Length;

    public TokenNode(string value, int endIndex)
    {
        this.Value = value;
        this.EndIndex = endIndex;
        Kind = MapStringToKind(value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static TokenNode CreateMissing(TokenKind kind)
    {
        return new("", -1) { Kind = kind };
    }

    private static TokenKind MapStringToKind(string value)
    {
        switch (value)
        {
            case "struct": 
                return TokenKind.StructKeyword;
            case "texture": 
                return TokenKind.TextureKeyword;
            case "buffer": 
                return TokenKind.BufferKeyword;
            case "in": 
                return TokenKind.InKeyword;
            case "out": 
                return TokenKind.OutKeyword;
            case "{": 
                return TokenKind.OpenBracket;
            case "}": 
                return TokenKind.CloseBracket;
            case "(":
                return TokenKind.OpenParen;
            case ")": 
                return TokenKind.CloseParen;
            case ";": 
                return TokenKind.Semicolon;
            case "<": 
                return TokenKind.OpenCaret;
            case ">": 
                return TokenKind.CloseCaret;
            case ",": 
                return TokenKind.Comma;
            case "=": 
                return TokenKind.Equals;
            case "":
                return TokenKind.Unknown;
            default:
                if (LanguageInfo.Operators.Any(op => op.Symbol == value))
                {
                    return TokenKind.Operator;
                }
                else if (char.IsDigit(value, 0))
                {
                    return TokenKind.Literal;
                }
                else if (char.IsLetter(value, 0))
                {
                    return TokenKind.Identifier;
                }
                else
                {
                    goto case "";
                }
        };
    }

    public void ExpectKind(TokenKind kind)
    {
        if (this.Kind != kind)
            throw new Exception();
    }

    public override void Accept(DocumentVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Operator AsOperator()
    {
        if (this.Kind != TokenKind.Operator)
            throw new Exception();

        return LanguageInfo.Operators.First(op => op.Symbol == this.Value);
    }
}
