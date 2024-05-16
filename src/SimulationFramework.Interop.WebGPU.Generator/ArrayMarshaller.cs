using ClangSharp.Interop;
using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class ArrayMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    private readonly Marshaller ElementMarshaller = Create(context, (type as CSharpPointerType)!.ElementType);

    public override bool RequiresMarshalling => true;

    public override CSharpType? ManagedType => new CSharpFreeType($"{ElementMarshaller.ManagedType!.GetName()}[]");
    public override CSharpType? UnmanagedType => new CSharpFreeType($"{ElementMarshaller.UnmanagedType!.GetName()}*");

    public override void MarshalToNative(MarshalContext context)
    {
        if (ElementMarshaller.RequiresMarshalling)
        {
            context.Writer.WriteLine($"{context.UnmanagedName} = MarshalUtils.SpanToPointer(stackalloc {ElementMarshaller.UnmanagedType!.GetName()}[{context.ManagedName}?.Length ?? 0]);");
            string loopVar = context.UnmanagedName.Split([".", "->"], StringSplitOptions.TrimEntries).Last() + "_i";
            context.Writer.WriteLine($"for (int {loopVar} = 0; {loopVar} < ({context.ManagedName}?.Length ?? 0); {loopVar}++)");
            context.Writer.OpenBraceBlock();
            ElementMarshaller.MarshalToNative(new MarshalContext(context, $"{context.ManagedName}[{loopVar}]", $"{context.UnmanagedName}[{loopVar}]"));
            context.Writer.CloseBraceBlock();
        }
        else if (context.CanPinArrays)
        {
            context.Writer.WriteLine($"{context.UnmanagedName} = {context.SanitizeName(context.UnmanagedName)}_ptr;");
        }
        else
        {
            context.Writer.WriteLine($"{context.UnmanagedName} = MarshalUtils.AllocArray({context.ManagedName});");
        }
    }

    public override void MarshalFromNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.ManagedName} = new {this.ManagedType!.GetName()[..^2]}[{GetLengthVar(context.UnmanagedName)}];");
        string loopVar = context.ManagedName.Split([".", "->"], StringSplitOptions.TrimEntries).Last() + "_i";
        context.Writer.WriteLine($"for (int {loopVar} = 0; {loopVar} < {context.ManagedName}.Length; {loopVar}++)");
        context.Writer.OpenBraceBlock();
        ElementMarshaller.MarshalFromNative(new MarshalContext(context, $"{context.ManagedName}[{loopVar}]", $"{context.UnmanagedName}[{loopVar}]"));
        context.Writer.CloseBraceBlock();
    }

    public override IEnumerable<string> GetFixedStatements(MarshalContext context)
    {
        if (ElementMarshaller.RequiresMarshalling)
            yield break;

        yield return $"fixed ({ElementMarshaller.UnmanagedType!.GetName()}* {context.SanitizeName(context.UnmanagedName)}_ptr = {context.ManagedName.Replace(".Value.", "?.")})";
    }

    private string GetLengthVar(string var)
    {
        if (var.EndsWith('s'))
        {
            return var[..^1] + "Count";
        }
        else
        {
            return var + "Size";
        }
    }

    public override void MarshalToNativeFree(MarshalContext context)
    {
        if (!context.CanPinArrays)
        {
            context.Writer.WriteLine($"MarshalUtils.FreeArray({context.UnmanagedName});");
        }
    }
}
