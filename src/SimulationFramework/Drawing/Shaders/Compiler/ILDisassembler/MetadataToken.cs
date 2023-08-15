using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;

/// <summary>
/// Wraps a int32 metadata token.
/// Allows instructions to differentiate between int32 instruction arguments and metadata token instruction arguments.
/// </summary>
internal struct MetadataToken
{
    private readonly Module module;

    public int Value { get; }

    public MetadataToken(Module module, int value)
    {
        this.module = module;
        Value = value;
    }

    public override string ToString()
    {
        var target = Resolve();

        return target?.ToString() ?? $"0x{Value:x8}";
    }

    /// <summary>
    /// Resolves this token to a runtime object, or returns <see langword="null"/>.
    /// </summary>
    public object Resolve()
    {
        var type = (MetadataTokenKind)(Value >> 24);

        return type switch
        {
            MetadataTokenKind.MemberRef => module.ResolveMember(Value),
            MetadataTokenKind.String => module.ResolveString(Value),
            MetadataTokenKind.TypeRef => module.ResolveType(Value),
            MetadataTokenKind.FieldDef => module.ResolveField(Value),
            MetadataTokenKind.MethodDef => module.ResolveMethod(Value),
            MetadataTokenKind.TypeDef => module.ResolveType(Value),
            _ => throw new Exception(),
        };
    }

}