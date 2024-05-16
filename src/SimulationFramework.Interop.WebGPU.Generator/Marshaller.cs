using CppAst;
using CppAst.CodeGen.CSharp;

namespace WebGPU.Generator;

class Marshaller(GenerationContext Context, CSharpType MappedType)
{
    public virtual bool RequiresMarshalling => false;

    public virtual CSharpType? ManagedType => MappedType;
    public virtual CSharpType? UnmanagedType => MappedType;

    public virtual bool MarshalsToVariable => true;

    public virtual void MarshalToNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.UnmanagedName} = {context.ManagedName};");
    }

    public virtual void MarshalToNativeFree(MarshalContext context)
    {
    }

    public virtual void MarshalFromNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.ManagedName} = {context.UnmanagedName};");
    }

    public virtual void MarshalFromNativeAlloc(MarshalContext context)
    {
    }

    public virtual string MakeArgument(string value)
    {
        return value;
    }

    public virtual IEnumerable<string> GetFixedStatements(MarshalContext context)
    {
        yield break;
    }

    public static Marshaller Create(GenerationContext context, CSharpType type)
    {
        if (type is null)
        {
            throw new();
        }

        if (type is CSharpClass)
        {
            return new HandleMarshaller(context, type);
        }

        if (type is CSharpDelegate)
        {
            return new DelegateMarshaller(context, type);
        }

        switch (type.GetName())
        {
            case "char*":
                return new StringMarshaller(context, type);
            case "ChainedStruct*":
            case "ChainedStruct":
                return new ChainedStructMarshaller(context, type);
            case "ChainedStructOut*":
            case "ChainedStructOut":
                return new ChainedStructMarshaller(context, type);
            case "WGPUBool":
                return new BoolMarshaller(context, type);

            // this is a void* that got mapped to a byte* (which is done for arrays)
            // non-array void* are things like windows HWNDs and such
            // cleanest way to map this into C# is nint
            case "byte*":
                return new Marshaller(context, new CSharpFreeType("nint"));
            
            default:
                break;
        }

        if (type is CSharpStruct || (type as CSharpPointerType)?.ElementType is CSharpStruct || (type as CSharpRefType)?.ElementType is CSharpStruct)
        {
            return new StructMarshaller(context, type);
        }

        return new(context, type);
    }

    public static IEnumerable<Marshaller> CreateWithArrays(GenerationContext context, IEnumerable<CppParameter> elements)
    {
        return CreateWithArrays(context, elements.Select(e => (context.TypeMapper.Map(e.Type), e.Name)).ToArray());
    }

    public static IEnumerable<Marshaller> CreateWithArrays(GenerationContext context, IEnumerable<CSharpParameter> elements)
    {
        return CreateWithArrays(context, elements.Select(e => (e.ParameterType, e.Name)).ToArray());
    }

    public static IEnumerable<Marshaller> CreateWithArrays(GenerationContext context, (CSharpType Type, string Name)[] elements)
    {
        DelegateMarshaller? lastDelegateMarshaller = null;
        foreach (var element in elements)
        {
            if (element.Name.EndsWith("userdata", StringComparison.OrdinalIgnoreCase))
            {
                yield return new UserDataMarshaller(context, lastDelegateMarshaller!);
                continue;
            }

            lastDelegateMarshaller = null;
            if (IsArrayOrLengthType(element.Type))
            {
                if (element.Name.EndsWith('s'))
                {
                    var namePart = element.Name[..^1];
                    if (namePart == "entrie")
                        namePart = "entry"; // array length will be singular
                    var lengthField = elements.FirstOrDefault(f => f.Name == namePart + "Count");
                    if (lengthField != default)
                    {
                        yield return new ArrayMarshaller(context, element.Type);
                        continue;
                    }
                }
                else if (element.Name.EndsWith("Count"))
                {
                    var namePart = element.Name[..^5];
                    if (namePart == "entry")
                        namePart = "entrie"; // array ptr will be plural
                    var elementsField = elements.FirstOrDefault(f => f.Name == namePart + "s");
                    if (elementsField != default)
                    {
                        yield return new ArrayLengthMarshaller(context, element.Type);
                        continue;
                    }
                }
                else if (element.Name.EndsWith("Size", StringComparison.OrdinalIgnoreCase))
                {
                    var namePart = element.Name[..^4];
                    var elementsField = elements.FirstOrDefault(f => f.Name == namePart);

                    if (elementsField != default)
                    {
                        yield return new ArrayLengthMarshaller(context, element.Type);
                        continue;
                    }
                }
                else
                {
                    var namePart = element.Name;
                    var lengthField = elements.FirstOrDefault(f => f.Name == namePart + "Size");

                    if (lengthField != default)
                    {
                        yield return new ArrayMarshaller(context, element.Type);
                        continue;
                    }
                }
            }

            var result = Create(context, element.Type);

            if (result is DelegateMarshaller delegateMarshaller)
            {
                lastDelegateMarshaller = delegateMarshaller;
            }

            yield return result;
        }
    }

    private static bool IsArrayOrLengthType(CSharpType type)
    {
        if (type is CSharpPointerType)
            return true;

        switch (type.GetName())
        {
            case "nint":
            case "nuint":
            case "long":
            case "ulong":
            case "int":
            case "uint":
                return true;
            default:
                return false;
        }
    }
}