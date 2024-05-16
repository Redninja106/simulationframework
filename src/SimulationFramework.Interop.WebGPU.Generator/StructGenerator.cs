using CppAst;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zio.FileSystems;

namespace WebGPU.Generator;
internal partial class StructGenerator(GenerationContext context)
{
    [GeneratedRegex("Can be chained in ([a-zA-Z0-9_]*)")]
    private static partial Regex ChainedRegex();

    public GeneratedStruct GenerateStruct(CppClass @class)
    {
        var name = @class.Name[4..];
        CSharpStruct csStruct = new CSharpStruct(name);
        CSharpStruct nativeStruct = new("Native");
        nativeStruct.Modifiers |= CSharpModifiers.Unsafe;
        nativeStruct.Visibility = CSharpVisibility.Internal;
        nativeStruct.Attributes.Add(new CSharpFreeAttribute(CSharpAttributeScope.None, $"Unmanaged<{nativeStruct.GetName()}>"));

        var chainableType = GetChainableType(@class);
        if (chainableType != null)
        {
            csStruct.BaseTypes.Add(new CSharpFreeType("IChainable"));
        }

        List<MarshalledField> fields = MarshalFields(@class.Fields).ToList();

        bool isOutStruct = context.OutParameters.Contains(csStruct.Name);

        foreach (var field in fields)
        {
            if (field.Marshaller is ChainedStructMarshaller chainedStructMarshaller)
            {
                isOutStruct = chainedStructMarshaller.IsOut;
            }

            var managedField = field.GetManagedField();
            if (managedField != null)
            {
                csStruct.Members.Add(managedField);
            }

            var unmanagedField = field.GetUnmanagedField();
            if (unmanagedField != null)
            {
                nativeStruct.Members.Add(unmanagedField);
            }
        }

        if (chainableType != null)
        {
            csStruct.Members.Add(new CSharpFreeMember() { Text = "readonly IChainable? IChainable.Next => Chain;" });
            csStruct.Members.Add(new CSharpFreeMember() { Text = $"unsafe readonly int IChainable.SizeInBytes => sizeof(Native);" });

            CodeWriter writer = new CodeWriter(new CodeWriterOptions(new MemoryFileSystem(), CodeWriterMode.Full));
            writer.WriteLine();
            writer.OpenBraceBlock();
            writer.WriteLine("Native* native = (Native*)chainedStruct;");
            writer.WriteLine($"native->chain.sType = SType.{csStruct.GetName()};");

            foreach (var field in fields)
            {
                if (field.Marshaller is ChainedStructMarshaller)
                    continue;

                var marshalContext = new MarshalContext(writer, $"this.{field.ManagedName}", $"native->{field.UnmanagedName}")
                {
                    CanPinArrays = false,
                };

                field.Marshaller.MarshalToNative(marshalContext);
            }

            writer.CloseBraceBlock();
            csStruct.Members.Add(new CSharpFreeMember() { Text = $"unsafe readonly void IChainable.InitNative(ChainedStruct.Native* chainedStruct) {writer}" });

            writer = new CodeWriter(new CodeWriterOptions(new MemoryFileSystem(), CodeWriterMode.Full));
            writer.WriteLine();
            writer.OpenBraceBlock();
            writer.WriteLine("Native* native = (Native*)chainedStruct;");

            foreach (var field in fields)
            {
                if (field.Marshaller is ChainedStructMarshaller)
                    continue;

                var marshalContext = new MarshalContext(writer, $"this.{field.ManagedName}", $"native->{field.UnmanagedName}")
                {
                    CanPinArrays = false,
                };

                field.Marshaller.MarshalToNativeFree(marshalContext);
            }

            writer.CloseBraceBlock();
            csStruct.Members.Add(new CSharpFreeMember() { Text = $"unsafe readonly void IChainable.UninitNative(ChainedStruct.Native* chainedStruct) {writer}" });

            // csStruct.Members.Add(new CSharpFreeMember() { Text = $"IBox IChainable.Box {{ get; }} = new Box<Native>();" });
            // csStruct.Members.Add(new CSharpFreeMember() { Text = $"public {csStruct.Name}() {{ }}" });
        }

        var parameters = csStruct.Members.OfType<CSharpField>();
        csStruct.Members.Add(new CSharpFreeMember()
        {
            Text = GetConstructor(csStruct, parameters),
        });
        csStruct.Members.Add(nativeStruct);

        GeneratedStruct result = new(isOutStruct, csStruct, fields.ToArray());
        context.Structs.Add(result);
        return result;
    }

    private string GetConstructor(CSharpStruct csStruct, IEnumerable<CSharpField> parameters)
    {
        var next = parameters.First();
        if (next.Name == "Next" || next.Name == "NextInChain" || next.Name == "Chain")
        {
            parameters = parameters.Skip(1).Append(next);
        }
        else
        {
            next = null;
        }

        string result = $"public {csStruct.Name}({string.Join(", ", GetParameterList(parameters.SkipLast(next == null ? 0 : 1), next))})\n";
        result += "{\n";
        foreach (var p in parameters)
        {
            result += $"    this.{p.Name} = {context.ToCamelCase(p.Name)};\n";
        }

        result += "}\n";
        return result;

        IEnumerable<string> GetParameterList(IEnumerable<CSharpField> parameters, CSharpField? next)
        {
            var result = parameters.Select(p => $"{p.FieldType.GetName()} {context.ToCamelCase(p.Name)}");

            if (next != null)
            {
                result = result.Append($"IChainable? {context.ToCamelCase(next.Name)} = null");
            }

            return result;
        }
    }

private bool HasArrayField(CppClass @class)
    {
        IEnumerable<string> candidates = @class.Fields.Where(f => f.Name.EndsWith('s')).Select(f => f.Name[..^1]).ToArray();

        foreach (var candidate in candidates)
        {
            if (@class.Fields.Any(f => f.Name.EndsWith("Size") && f.Name[..^4] == candidate))
                return true;

            if (@class.Fields.Any(f => f.Name.EndsWith("Count") && f.Name[..^5] == candidate))
                return true;
        }

        return false;
    }

    private CSharpFreeType? GetChainableType(CppClass @class)
    {
        if (@class.Comment is null)
            return null;
        var match = ChainedRegex().Match(@class.Comment.ToString());
        if (match.Success)
        {
            var type = match.Captures[0];
            return new CSharpFreeType(type.Value);
        }
        return null;
    }

    private IEnumerable<MarshalledField> MarshalFields(IEnumerable<CppField> fields)
    {
        (CSharpType, string)[] elements = fields.Select(f => (context.TypeMapper.Map(f.Type), f.Name)).ToArray();
        var marshallers = Marshaller.CreateWithArrays(context, elements);
        return fields.Zip(marshallers).Select(pair => new MarshalledField(context, pair.Second, pair.First.Name));
    }
}

record GeneratedStruct(bool IsOutStruct, CSharpStruct CSharpStruct, MarshalledField[] Fields)
{
    public bool IsRefStruct => CSharpStruct is CSharpRefStruct;
}

class MarshalledField(GenerationContext context, Marshaller marshaller, string CppName)
{
    public Marshaller Marshaller => marshaller;
    public virtual string ManagedName => context.ToPascalCase(CppName);
    public virtual string UnmanagedName => CppName;

    // public virtual string? MarshalToNative() => $"result.{UnmanagedName} = value.{ManagedName};";
    // public virtual string? MarshalFree() => null;
    // public virtual string? MarshalFromNative() => $"result.{ManagedName} = value.{UnmanagedName};";
    // 
    public virtual CSharpField? GetManagedField() => marshaller.ManagedType is null ? null : new(ManagedName) { FieldType = Marshaller.ManagedType };
    public virtual CSharpField? GetUnmanagedField() => new(UnmanagedName) { FieldType = Marshaller.UnmanagedType };
}

/*
class StringField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override string? MarshalToNative()
    {
        return $"result.{UnmanagedName} = MarshalUtils.AllocString(value.{ManagedName});";
    }

    public override string? MarshalFromNative()
    {
        return $"result.{ManagedName} = MarshalUtils.StringFromPointer(value.{UnmanagedName});";
    }

    public override string? MarshalFree()
    {
        return $"MarshalUtils.FreeString(value.{UnmanagedName});";
    }

    public override CSharpType ManagedType => new CSharpFreeType("string");
}

internal class WebGPUObjectField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override string? MarshalToNative()
    {
        return $"result.{UnmanagedName} = value.{ManagedName}.NativeHandle;";
    }

    public override string? MarshalFromNative()
    {
        return $"result.{ManagedName} = new(value.{UnmanagedName});";
    }

    public override CSharpType UnmanagedType => new CSharpFreeType("nint");
}

internal class ChainedStructField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override string? MarshalFromNative() => null;
    public override string? MarshalToNative() => null;

    public override CSharpType ManagedType => new CSharpFreeType("IChainable");
}
internal class WGPUBoolField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override CSharpType ManagedType => new CSharpFreeType("bool");
}

internal class ArrayField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override string? MarshalToNative()
    {
        return $"result.{UnmanagedName} = MarshalUtils.SpanToPointer(value.{ManagedName});";
    }

    public override CSharpType ManagedType => new CSharpFreeType($"Span<{(base.ManagedType as CSharpPointerType)!.ElementType.GetName()}>");
}

internal class ArrayLengthField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override CSharpField? GetManagedField()
    {
        return null;
    }

    public override string? MarshalToNative()
    {
        return "// length";
    }
}

internal class WGPUStructField(GenerationContext context, CppField field) : MarshalledField(context, field)
{
    public override CSharpType UnmanagedType => new CSharpFreeType(GetNativeTypeName());

    private string GetNativeTypeName()
    {
        if (base.UnmanagedType is CSharpPointerType ptr)
            return ptr.ElementType.GetName() + ".Native*";
        return base.UnmanagedType.GetName() + ".Native";
    }
}*/