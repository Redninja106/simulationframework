using SimulationFramework.Drawing.Shaders.Compiler.Expressions;

namespace SimulationFramework.Drawing.Shaders.Compiler.Translation;

public abstract class TypeHandler
{
    private static Dictionary<LanguageTarget, Dictionary<Type, TypeHandler>> handlers = new();

    public abstract string HandleName();
    public abstract string? HandleField(string fieldName);
    public abstract string HandleArrayAccess(Expression expression);

    public static void Get(Type type, LanguageTarget language)
    {
    }

    public static void Register(Type type, TypeHandler handler, LanguageTarget language)
    {
        handlers[language][type] = handler;
    }
}
