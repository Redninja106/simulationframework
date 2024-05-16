using CppAst.CodeGen.CSharp;

namespace WebGPU.Generator;

internal class BoolMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    public override CSharpType ManagedType => new CSharpFreeType("bool");
}