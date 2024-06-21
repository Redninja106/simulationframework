using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;
internal class VectorHandler(int dimensions) : TypeHandler
{
    public override string HandleArrayAccess(Expression expression)
    {
        throw new NotImplementedException();
    }

    public override string? HandleField(string fieldName)
    {
        return fieldName switch
        {
            "X" => "x",
            "Y" => "y",
            "Z" => "z",
            "W" => "w",
            _ => null
        };
    }

    public override string HandleName()
    {
        return "vec" + dimensions;
    }
}
