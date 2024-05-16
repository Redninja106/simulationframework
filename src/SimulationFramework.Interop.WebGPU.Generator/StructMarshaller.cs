using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using System.Diagnostics.CodeAnalysis;

namespace WebGPU.Generator;

internal class StructMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    bool IsPointer => type is CSharpPointerType || type.GetName().EndsWith('*');

    public override bool RequiresMarshalling => true;

    public override CSharpType? ManagedType 
    {
        get 
        {
            if (IsPointer)
                return new CSharpFreeType(GetElemType(type) + "?");

            return base.ManagedType;
        }
    }

    public override CSharpType UnmanagedType => new CSharpFreeType(GetNativeTypeName());

    public override void MarshalToNative(MarshalContext ctx)
    {
        if (IsPointer)
        {
            string elemType = GetElemType(UnmanagedType);
            ctx.Writer.WriteLine($"{ctx.UnmanagedName} = MarshalUtils.SpanToPointer(stackalloc {elemType}[{ctx.ManagedName}.HasValue ? 1 : 0]);");
            ctx.Writer.WriteLine($"if ({ctx.ManagedName} is not null)");
            ctx.Writer.OpenBraceBlock();

            var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
            foreach (var field in generatedStruct.Fields)
            {
                field.Marshaller.MarshalToNative(CreateChildContext(ctx, field));

            }

            ctx.Writer.CloseBraceBlock();
        }
        else
        {
            ctx.Writer.WriteLine($"{ctx.UnmanagedName} = default;");

            var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
            foreach (var field in generatedStruct.Fields)
            {
                field.Marshaller.MarshalToNative(CreateChildContext(ctx, field));
            }
        }
    }

    public override void MarshalFromNativeAlloc(MarshalContext ctx)
    {
        ctx.Writer.WriteLine($"{UnmanagedType} {ctx.UnmanagedName} = MarshalUtils.SpanToPointer(stackalloc {GetElemType(UnmanagedType)}[1]);");
        base.MarshalFromNativeAlloc(ctx);
    }

    public override IEnumerable<string> GetFixedStatements(MarshalContext ctx)
    {
        var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
        foreach (var field in generatedStruct.Fields)
        {
            foreach (var fixedStatement in field.Marshaller.GetFixedStatements(CreateChildContext(ctx, field)))
            {
                yield return fixedStatement;
            }
        }
    }

    public override void MarshalToNativeFree(MarshalContext ctx)
    {
        if (IsPointer)
        {
            ctx.Writer.WriteLine($"if ({ctx.ManagedName} is not null)");
            ctx.Writer.OpenBraceBlock();
        }
        var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
        foreach (var field in generatedStruct.Fields)
        {
            field.Marshaller.MarshalToNativeFree(CreateChildContext(ctx, field));
        }
        if (IsPointer)
        {
            ctx.Writer.CloseBraceBlock();
        }
    }

    public override void MarshalFromNative(MarshalContext ctx)
    {
        if (IsPointer)
        {
            ctx.Writer.WriteLine($"{ctx.ManagedName} = default;");
            ctx.Writer.WriteLine($"if ({ctx.UnmanagedName} is not null)");
            ctx.Writer.OpenBraceBlock();
            if (IsPointer)
            {
                ctx.Writer.WriteLine($"{ManagedType!.GetName()[..^1]} {ctx.ManagedName}_value = default;");
                ctx = new(ctx, ctx.ManagedName + "_value", ctx.UnmanagedName);
            }

            var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
            foreach (var field in generatedStruct.Fields)
            {
                field.Marshaller.MarshalFromNative(CreateChildContext(ctx, field, false));
            }

            if (IsPointer)
            {
                ctx = ctx.ParentContext!;
                ctx.Writer.WriteLine($"{ctx.ManagedName} = {ctx.ManagedName}_value;");
            }

            ctx.Writer.CloseBraceBlock();
        }
        else
        {
            ctx.Writer.WriteLine($"{ctx.ManagedName} = default;");

            var generatedStruct = context.GetGeneratedStruct(GetElemType(type));
            foreach (var field in generatedStruct.Fields)
            {
                field.Marshaller.MarshalFromNative(CreateChildContext(ctx, field));
            }
        }
    }

    private MarshalContext CreateChildContext(MarshalContext parent, MarshalledField field, bool maybeNullable = true)
    {
        string unmanagedName = $"{parent.UnmanagedName}{GetNativeMemberAccessToken()}{field.UnmanagedName}";
        string managedName = $"{parent.ManagedName}{GetMemberAccessToken(maybeNullable)}{field.ManagedName}";
        MarshalContext childContext = new(parent, managedName, unmanagedName);
        return childContext;
    }

    private string GetMemberAccessToken(bool maybeNullable)
    {
        if (maybeNullable && IsPointer)
            return ".Value.";
        return ".";
    }

    private string GetNativeMemberAccessToken()
    {

        if (IsPointer)
            return "->";
        return ".";
    }

    private string GetNativeTypeName()
    {
        if (base.UnmanagedType is CSharpPointerType ptr)
            return ptr.ElementType.GetName() + ".Native*";
        if (base.UnmanagedType is CSharpRefType refType)
            return refType.ElementType.GetName() + ".Native*";
        return base.UnmanagedType.GetName() + ".Native";
    }

    private string GetElemType(CSharpType type)
    {
        return IsPointer ? type.GetName()[..^1] : type.GetName();
    }

    public override string MakeArgument(string value)
    {
        return base.MakeArgument(value);
    }
}