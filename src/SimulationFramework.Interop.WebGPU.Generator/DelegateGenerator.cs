using ClangSharp;
using CppAst.CodeGen.CSharp;
using CppAst;
using WebGPU.Generator;

namespace WebGPU.Generator;

internal class DelegateGenerator(GenerationContext context)
{
    public CSharpDelegate GenerateDelegate(CppTypedef typeDef)
    {
        var fn = ((typeDef.ElementType as CppPointerType)?.ElementType as CppFunctionType) ?? throw new();

        var csDelegate = new CSharpDelegate(typeDef.Name[4..]);
        csDelegate.Visibility = CSharpVisibility.Public;
        csDelegate.CppElement = typeDef;

        var retMarshaller = Marshaller.Create(context, context.TypeMapper.Map(fn.ReturnType));
        csDelegate.ReturnType = retMarshaller.ManagedType ?? throw new();

        var paramMarshallers = Marshaller.CreateWithArrays(context, fn.Parameters.Select(p => (context.TypeMapper.Map(p.Type), p.Name)).ToArray());
        csDelegate.Parameters.AddRange(paramMarshallers.Where(m => m.ManagedType is not null).Zip(fn.Parameters).Select(pair => new CSharpParameter(pair.Second.Name) { ParameterType = pair.First.ManagedType }));

        return csDelegate;
    } 
}