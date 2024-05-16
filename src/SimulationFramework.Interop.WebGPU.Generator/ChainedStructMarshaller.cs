using CppAst.CodeGen.CSharp;
using System.Reflection;

namespace WebGPU.Generator;

internal class ChainedStructMarshaller(GenerationContext context, CSharpType type) : StructMarshaller(context, type)
{
    public bool IsOut => type.GetName().Contains("Out");

    public override CSharpType ManagedType => new CSharpFreeType("IChainable?");

    public override void MarshalToNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.UnmanagedName} = MarshalUtils.AllocChain({context.ManagedName});");
    }

    public override void MarshalToNativeFree(MarshalContext context)
    {
        context.Writer.WriteLine($"MarshalUtils.FreeChain({context.ManagedName}, {context.UnmanagedName});");
    }

    public override void MarshalFromNative(MarshalContext ctx)
    {
    }

    public override void MarshalFromNativeAlloc(MarshalContext ctx)
    {
    }

    public override IEnumerable<string> GetFixedStatements(MarshalContext ctx)
    {
        yield break;
    }
}