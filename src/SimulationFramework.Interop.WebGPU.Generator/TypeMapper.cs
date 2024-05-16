using CppAst;
using CppAst.CodeGen.CSharp;
using System.Diagnostics.CodeAnalysis;

namespace WebGPU.Generator;
class TypeMapper
{
    private readonly Dictionary<CppType, CSharpType> declaredMappings = new();

    public void DeclareMapping(CppType from, CSharpType to)
    {
        declaredMappings.Add(from, to);
    }

    public bool TryGetDeclaredMapping(CppType from, [NotNullWhen(true)] out CSharpType? to)
    {
        return declaredMappings.TryGetValue(from, out to);
    }

    public CSharpType Map(CppType type, bool useRefs = false, bool managed = false)
    {
        if (declaredMappings.TryGetValue(type, out CSharpType? mappedType))
        {
            return mappedType;
        }

        if (type is CppTypedef typedef)
        {
            if ((typedef.ElementType as CppPointerType)?.ElementType is CppFunctionType fn)
                return new CSharpFreeType(typedef.Name[4..]);

            if (typedef.Name == "size_t")
                return new CSharpFreeType("nuint");

            if (typedef.Name == "WGPUBool")
                return new CSharpFreeType(managed ? "bool" : "WGPUBool");

            return Map(typedef.ElementType);
        }

        if (type is CppPrimitiveType prim)
        {
            return new CSharpPrimitiveType(prim.Kind switch
            {
                CppPrimitiveKind.Void => CSharpPrimitiveKind.Void,
                CppPrimitiveKind.Int => CSharpPrimitiveKind.Int,
                CppPrimitiveKind.UnsignedLongLong => CSharpPrimitiveKind.ULong,
                CppPrimitiveKind.UnsignedInt => CSharpPrimitiveKind.UInt,
                CppPrimitiveKind.Float => CSharpPrimitiveKind.Float,
                CppPrimitiveKind.Double => CSharpPrimitiveKind.Double,
                CppPrimitiveKind.Char => CSharpPrimitiveKind.Char, // this is _not_ a correct mapping, but we can't
                                                                   // use byte since Marshallers need to be able
                                                                   // to differentiate between byte arrays and strings
                CppPrimitiveKind.UnsignedShort => CSharpPrimitiveKind.UShort,
            });
        }

        if (type is CppClass clss)
        {
            string name = clss.Name;

            if (clss.Name.StartsWith("WGPU"))
                name = name[4..];

            return new CSharpClass(name);
        }

        if (type is CppPointerType ptr)
        {
            if (ptr.ElementType.FullName.EndsWith("Impl"))
            {
                return new CSharpClass(ptr.ElementType.GetDisplayName()[4..^4]);
            }
            else
            {
                var elemType = Map(ptr.ElementType);

                if (elemType.GetName() == "void")
                    return new CSharpPointerType(new CSharpFreeType("byte"));

                if (useRefs)
                {
                    return new CSharpRefType(CSharpRefKind.Ref, elemType);
                }
                else
                {
                    return new CSharpPointerType(elemType);
                }
            }
        }

        if (type is CppQualifiedType qual)
        {
            return Map(qual.ElementType);
        }

        if (type is CppEnum enm)
            return new CSharpEnum(enm.Name);

        throw new();
    }


}


