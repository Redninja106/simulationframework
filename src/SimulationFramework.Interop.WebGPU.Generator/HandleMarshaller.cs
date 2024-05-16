using CppAst.CodeGen.CSharp;

namespace WebGPU.Generator;

internal class HandleMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    public override CSharpType UnmanagedType => new CSharpFreeType("nint");

    public override bool RequiresMarshalling => true;

    public override void MarshalToNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.UnmanagedName} = {context.ManagedName}?.NativeHandle ?? 0;");
    }

    public override void MarshalFromNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.ManagedName} = {context.UnmanagedName} is 0 ? null : new({context.UnmanagedName});");
    }

    public override string MakeArgument(string value)
    {
        return base.MakeArgument(value);
    }
}