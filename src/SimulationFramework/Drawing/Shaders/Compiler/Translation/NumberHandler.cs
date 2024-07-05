using SimulationFramework.Drawing.Shaders.Compiler.Expressions;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;

class NumberHandler(string name) : TypeHandler
{
    public override string HandleName()
    {
        return name;
    }

    public override string HandleArrayAccess(ShaderExpression expression)
    {
        throw new NotImplementedException();
    }

    public override string? HandleField(string fieldName)
    {
        throw new NotImplementedException();
    }
}
