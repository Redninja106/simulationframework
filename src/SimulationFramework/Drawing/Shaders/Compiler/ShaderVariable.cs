using System.Reflection;

namespace SimulationFramework.Drawing.Shaders.Compiler;

public class ShaderVariable
{
    public ShaderVariable(ShaderType type, ShaderName name, MemberInfo? backingMember, ShaderVariableKind kind)
    {
        Type = type;
        Name = name;
        BackingMember = backingMember;
        Kind = kind;
    }

    public ShaderType Type { get; }
    public ShaderName Name { get; }
    public MemberInfo? BackingMember { get; }
    public ShaderVariableKind Kind { get; }

    public override string ToString()
    {
        return $"{Type} {Name}";
    }
}
