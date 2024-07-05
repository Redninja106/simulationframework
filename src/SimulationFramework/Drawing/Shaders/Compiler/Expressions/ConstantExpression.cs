﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Expressions;
public record ConstantExpression(object Value) : ShaderExpression
{
    public override ShaderExpression Accept(ExpressionVisitor visitor) => visitor.VisitConstantExpression(this);
    public override ShaderExpression VisitChildren(ExpressionVisitor visitor) => this;

    public override string ToString()
    {
        return Value.ToString();
    }
}
