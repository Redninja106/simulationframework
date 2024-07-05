using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

/// <summary>
/// gradually built up by stages during compilation
/// </summary>
public class ShaderCompilation
{
    public List<ShaderMethod> Methods { get; } = [];
    public List<ShaderVariable> Uniforms { get; } = [];
    public List<ShaderStructure> Structures { get; } = [];
}

public abstract class ShaderType
{
   
}

public class ShaderPrimitiveType : ShaderType 
{
    public ShaderPrimitive primitive;

    private static Dictionary<ShaderPrimitive, ShaderPrimitiveType> instances = [];

    static ShaderPrimitiveType()
    {
        foreach (var value in Enum.GetValues<ShaderPrimitive>())
        {
            instances.Add(value, new(value));
        }
    }

    private ShaderPrimitiveType(ShaderPrimitive primitive)
    {
        this.primitive = primitive;
    }

    public static ShaderType Get(ShaderPrimitive primitive)
    {
        return instances[primitive];
    }
}

public enum ShaderPrimitive
{
    Void,
    Bool,
    Int,
    Int2,
    Int3,
    Int4,
    Float,
    Float2,
    Float3,
    Float4,
    Matrix4x4,
    Matrix3x2,
    Texture,
    Buffer,
}

public class ShaderMethod
{
    public ShaderVariable[] Parameters { get; set; }
    public ShaderVariable[] Locals { get; set; }
    public ShaderExpression Body { get; set; }
    public ShaderType ReturnType { get; set; }
    public ShaderName Name { get; set; }
}

public class ShaderStructureType : ShaderType 
{ 
    public ShaderStructure structure;

    public ShaderStructureType(ShaderStructure structure)
    {
        this.structure = structure;
    }
}

public class ShaderStructure
{
    public ShaderVariable[] fields;
    public ShaderName name;
}

public class ShaderVariable
{
    public ShaderVariable(ShaderType type, ShaderName name, MemberInfo? backingMember)
    {
        Type = type;
        Name = name;
        BackingMember = backingMember;
    }

    public ShaderType Type { get; }
    public ShaderName Name { get; }
    public MemberInfo? BackingMember { get; }
}

public struct ShaderName
{
    public string value;

    public ShaderName(string value)
    {
        this.value = value
            .Replace('.', '_');
    }

    public override string ToString()
    {
        return value;
    }
}