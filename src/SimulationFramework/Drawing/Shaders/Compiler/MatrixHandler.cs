using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using SimulationFramework.Drawing.Shaders.Compiler.Translation;
using System.Numerics;

namespace SimulationFramework.Drawing.Shaders.Compiler;
internal class MatrixHandler(int rows, int columns) : TypeHandler
{
    public override string HandleArrayAccess(Expression expression)
    {
        throw new NotImplementedException();
    }

    public override string? HandleField(string fieldName)
    {
        throw new NotImplementedException();
    }

    public override string HandleName()
    {
        return $"float{rows}x{columns}";
    }
}