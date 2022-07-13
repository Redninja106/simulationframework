using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SFSL.Nodes;

internal class DocumentBuilder
{
    CompilationContext context;

    public DocumentBuilder(CompilationContext context)
    {
        this.context = context;
    }

    internal Document Build(TokenReader reader)
    {
        CompilationUnitNode root = new();

        while (!reader.IsAtEnd) 
        {
            Node node = ParseDeclaration(reader);

            if (node != null)
                root.AddChild(node);
        }

        return new Document(root);
    }

    private Node ParseDeclaration(TokenReader reader)
    { 
        switch (reader.Current.Kind)
        {
            case TokenKind.StructKeyword:
                return ParseStruct(reader);
            case TokenKind.InKeyword:
            case TokenKind.OutKeyword:
                return ParseVariable(reader);
            case TokenKind.Identifier:
                if (reader.PeekToken(2).Kind == TokenKind.OpenParen)
                    return ParseFunction(reader);
                else
                    return ParseVariable(reader);
            case TokenKind.TextureKeyword:
                return ParseTexture(reader);
            case TokenKind.BufferKeyword:
                return ParseBuffer(reader);
            default:
                return ParseUnexpected(reader);
        }
    }

    private UnexpectedTokenNode ParseUnexpected(TokenReader reader)
    {
        return new UnexpectedTokenNode(reader.EatToken());
    }

    private TextureNode ParseTexture(TokenReader reader)
    {
        if (reader.Current.Kind != TokenKind.TextureKeyword)
            throw new Exception("Big Issue!");

        return new TextureNode(
            reader.EatToken(TokenKind.TextureKeyword),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Semicolon)
            );
    }

    private BufferNode ParseBuffer(TokenReader reader)
    {
        if (reader.Current.Kind != TokenKind.BufferKeyword)
            throw new Exception("Big Issue!");

        return new BufferNode(
            reader.EatToken(TokenKind.BufferKeyword),
            reader.EatToken(TokenKind.OpenCaret),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.CloseCaret),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Semicolon)
            );
    }

    private VariableNode ParseVariable(TokenReader reader)
    {
        return new(
            reader.EatTokenOrNull(TokenKind.InKeyword) ?? reader.EatTokenOrNull(TokenKind.OutKeyword) ?? null,
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Semicolon)
            );
    }

    private StructNode ParseStruct(TokenReader reader)
    {
        if (reader.Current.Kind != TokenKind.StructKeyword)
            throw new Exception("Big Issue!");

        return new StructNode(
            reader.EatToken(TokenKind.StructKeyword),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.OpenBracket),
            ParseStructMembers(reader),
            reader.EatToken(TokenKind.CloseBracket)
            );
    }

    private VariableNode[] ParseStructMembers(TokenReader reader)
    {
        var members = new List<VariableNode>();

        while (reader.Current.Kind != TokenKind.CloseBracket)
        {
            members.Add(ParseVariable(reader));
        }

        return members.ToArray();
    }

    private FunctionNode ParseFunction(TokenReader reader)
    {
        return new FunctionNode(
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.OpenParen),
            ParseFunctionParameters(reader),
            reader.EatToken(TokenKind.CloseParen),
            ParseStatement(reader)
            );
    }    

    private FunctionParameterNode[] ParseFunctionParameters(TokenReader reader)
    {
        if (reader.Current.Kind == TokenKind.CloseParen)
            return Array.Empty<FunctionParameterNode>();

        var parameters = new List<FunctionParameterNode>();

        TokenNode comma;
        do
        {
            parameters.Add(new(
                reader.EatToken(TokenKind.Identifier),
                reader.EatToken(TokenKind.Identifier),
                comma = reader.EatTokenOrNull(TokenKind.Comma)
                ));
        }
        while (comma != null);

        return parameters.ToArray();
    }

    private Statement ParseStatement(TokenReader reader)
    {
        switch (reader.Current.Kind)
        {
            case TokenKind.OpenBracket:
                return ParseBlockStatement(reader);
            case TokenKind.Identifier:
                switch (reader.PeekToken(1).Kind)
                {
                    case TokenKind.Identifier:
                        return ParseVariableDeclaration(reader);

                    case TokenKind.OpenParen:
                        break;
                    default:
                        break;
                }
                goto default;
            case TokenKind.Semicolon:
            default:
                throw new Exception();
        }
    }

    private VariableDeclarationStatement ParseVariableDeclaration(TokenReader reader)
    {
        return new VariableDeclarationStatement(
            reader.EatToken(TokenKind.Identifier),
            reader.EatToken(TokenKind.Identifier),
            reader.EatTokenOrNull(TokenKind.Equals) ?? reader.EatToken(TokenKind.Semicolon),
            reader.PeekToken(-1).Kind == TokenKind.Equals ? ParseExpression(reader) : null,
            reader.EatToken(TokenKind.Semicolon)
            );
    }

    private BlockStatementNode ParseBlockStatement(TokenReader reader)
    {
        return new BlockStatementNode(reader.EatToken(TokenKind.OpenBracket), ParseStatements(reader), reader.EatToken(TokenKind.CloseBracket));
    }

    private Statement[] ParseStatements(TokenReader reader)
    {
        var result = new List<Statement>();
        while (reader.Current.Kind != TokenKind.CloseBracket)
        {
            result.Add(ParseStatement(reader));
        }
        return result.ToArray();
    }

    private Expression ParseExpression(TokenReader reader, Precedence precedence = 0)
    {
        // 10 * 11 + 12 * 13
        /* left = 10
         * op = *
         * right = 11
         * 10
         */

        Expression left = null;

        switch (reader.Current.Kind)
        {
            case TokenKind.Literal:
                left = new LiteralExpression(reader.EatToken(TokenKind.Literal));
                break;
            case TokenKind.Operator:
                left = new UnaryExpression(reader.EatToken(TokenKind.Operator), ParseExpression(reader, Precedence.Unary));
                break;
            case TokenKind.OpenParen:
                left = new NestedExpression(reader.EatToken(TokenKind.OpenParen), ParseExpression(reader), reader.EatToken(TokenKind.CloseParen));
                break;
            default:
                throw new Exception();
        }

        while (true)
        {
            if (reader.Current.Kind != TokenKind.Operator)
                break;

            Precedence newPrecedence = reader.Current.AsOperator().Precedence;

            if (newPrecedence < precedence)
                break;

            left = new BinaryExpression(left, reader.EatToken(TokenKind.Operator), ParseExpression(reader, newPrecedence));
        }

        return left;

        //if (reader.Current.Kind == )
        //{
        //    var op = reader.Current.AsOperator();
        //    if (op.IsUnary)
        //    {

        //    }
        //    else
        //    {
        //        // err
        //    }
        //}
        //else if (reader.Current.Kind == TokenKind.OpenParen)
        //{

        //}
        //else if (reader.Current.Kind == TokenKind.Literal reader.Current.Kind == TokenKind.Identifier)
        //{
        //    if (reader.PeekToken(1).Kind == TokenKind.Operator)
        //    {
        //        return ParseBinaryExpression(reader);
        //    }
        //}
    }
}
