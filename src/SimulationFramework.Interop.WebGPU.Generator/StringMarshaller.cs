using CppAst.CodeGen.CSharp;

namespace WebGPU.Generator;

class StringMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    public override bool RequiresMarshalling => true;

    public override CSharpType ManagedType => new CSharpFreeType("string");
    public override CSharpType UnmanagedType => new CSharpFreeType("byte*");

    public override void MarshalToNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.UnmanagedName} = MarshalUtils.AllocString({context.ManagedName});");
    }

    public override void MarshalFromNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.ManagedName} = MarshalUtils.StringFromPointer({context.UnmanagedName});");
    }

    public override void MarshalToNativeFree(MarshalContext context)
    {
        context.Writer.WriteLine($"MarshalUtils.FreeString({context.UnmanagedName});");
    }

}
