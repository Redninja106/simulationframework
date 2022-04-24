using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public enum TokenKind
{
    Unknown,

    Identifier,
    Literal,
    Operator,

    // keywords
    StructKeyword,
    TextureKeyword,
    BufferKeyword,
    InKeyword,
    OutKeyword,

    // puncuation
    Equals,
    Comma,
    Semicolon,
    OpenCaret,
    CloseCaret,
    OpenBracket,
    CloseBracket,
    OpenParen,
    CloseParen,
}
