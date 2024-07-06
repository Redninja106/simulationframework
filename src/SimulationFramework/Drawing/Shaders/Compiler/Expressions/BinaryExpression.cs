﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record BinaryExpression(BinaryOperation Operation, ShaderExpression LeftOperand, ShaderExpression RightOperand) : ShaderExpression
{
    public override ShaderType ExpressionType => IsBooleanOperator(Operation) ? ShaderPrimitiveType.Get(ShaderPrimitive.Bool) : LeftOperand.ExpressionType;


    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitBinaryExpression(this);
    public override ShaderExpression VisitChildren(ExpressionVisitor visitor)
    {
        return new BinaryExpression(this.Operation, LeftOperand.Accept(visitor), RightOperand.Accept(visitor));
    }

    public string GetOperator()
    {
        return Operation switch
        {
            BinaryOperation.Add => "+",
            BinaryOperation.Subtract => "-",
            BinaryOperation.Multiply => "*",
            BinaryOperation.Divide => "/",
            BinaryOperation.Modulus => "%",
            BinaryOperation.NotEqual => "!=",
            BinaryOperation.Equal => "==",
            BinaryOperation.LessThan => "<",
            BinaryOperation.LessThanEqual => "<=",
            BinaryOperation.GreaterThan => ">=",
            BinaryOperation.GreaterThanEqual => ">",
            BinaryOperation.Assignment => "=",
            BinaryOperation.LeftShift => "<<=",
            BinaryOperation.RightShift => ">>=",
            BinaryOperation.Or => "|",
            BinaryOperation.And => "&",
        };
    }

    public override string ToString()
    {
        var op = GetOperator();
        return $"({LeftOperand} {op} {RightOperand})";
    }

    public static bool IsBooleanOperator(BinaryOperation operation)
    {
        return operation is
            BinaryOperation.NotEqual or
            BinaryOperation.Equal or
            BinaryOperation.LessThan or
            BinaryOperation.LessThanEqual or
            BinaryOperation.GreaterThan or
            BinaryOperation.GreaterThanEqual;
    }
}
