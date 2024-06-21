using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal class ColorFHandler : TypeHandler
{
    public override string HandleArrayAccess(Expression expression)
    {
        throw new NotImplementedException();
    }

    public override string? HandleField(string fieldName)
    { 
        return fieldName switch
        {
            "R" => "x",
            "G" => "y",
            "B" => "z",
            "A" => "w",
        };
    }

    public override string HandleName()
    {
        return nameof(ColorF);
    }
}
