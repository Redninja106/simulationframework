using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.ShaderLanguage;

internal enum TokenKind
{
    Unknown,
    Identifier,
    NumericLiteral,
    CharLiteral,
    StringLiteral,
    Comment,

    // type keywords
    Void,

    Bool,

    Int,
    Long,

    UInt,
    ULong,

    Half,
    Float,
    Double,
    
    Vector2,
    Vector3,
    Vector4,

    // symbols
    OpenParenthesis,
    CloseParenthesis,
    OpenBrace,
    CloseBrace,
    OpenBracket,
    CloseBracket,
    Semicolon,
    Comma,
    Dot,
    Colon,

    // keywords
    Var,
    Texture,
    Struct,
    Buffer,

    // operators
    Plus,
    Minus,
    Star,
    Slash,
    Percent,

    // assignment
    PlusPlus,
    MinusMinus,
    Equal,
    PlusEqual,
    MinusEqual,
    StarEqual,
    SlashEqual,
    PercentEqual,
    AndEqual,
    OrEqual,
    HatEqual,
    TildeEqual,
    ShiftLeftEqual,
    ShiftRightEqual,

    // bitwise
    And,
    Or,
    Hat,
    Tilde,
    ShiftLeft,
    ShiftRight,

    // logical
    AndAnd,
    OrOr,
    Not,

    // comparison
    EqualEqual,
    NotEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
}