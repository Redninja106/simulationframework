using ClangSharp.Interop;
using CppAst;
using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class DelegateMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    private CSharpFunctionPointer fnPtr = GetFunctionPointer(context, (CSharpDelegate)type);
    private MarshalledParameter[] parameters = CreateMarshallers(context, (CSharpDelegate)type).ToArray();


    public override CSharpType? UnmanagedType => fnPtr;
    public override bool RequiresMarshalling => true;
    public override bool MarshalsToVariable => false;

    private static CSharpFunctionPointer GetFunctionPointer(GenerationContext context, CSharpDelegate @delegate)
    {
        var returnTypeMarshaller = Marshaller.Create(context, @delegate.ReturnType);
        CSharpFunctionPointer result = new(returnTypeMarshaller.UnmanagedType);
        result.Parameters.AddRange(CreateMarshallers(context, @delegate).Select(m => m.Marshaller.UnmanagedType));
        result.IsUnmanaged = true;
        result.UnmanagedCallingConvention.Add("Cdecl");
        return result;
    }

    private static IEnumerable<MarshalledParameter> CreateMarshallers(GenerationContext context, CSharpDelegate @delegate)
    {
        var typeDef = (CppTypedef)@delegate.CppElement;
        var ptrType = (CppPointerType)typeDef.ElementType;
        var fnType = (CppFunctionType)ptrType.ElementType;
        return MarshalledParameter.Map(context, @delegate.Name, fnType.Parameters);
    }

    public override string MakeArgument(string value)
    {
        return $"&Native_{type.GetName()}";
    }

    public override void MarshalToNative(MarshalContext ctx)
    {
        string paramList = string.Join(", ", CreateMarshallers(context, (CSharpDelegate)type).Select(m => $"{m.Marshaller.UnmanagedType?.GetName()} {m.Parameter.Name}"));
        ctx.Writer.WriteLine($@"[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]");
        ctx.Writer.WriteLine($@"static {fnPtr.ReturnType.GetName()} Native_{type.GetName()}({paramList})");
        ctx.Writer.OpenBraceBlock();
        
        foreach (var p in this.parameters)
        {
            if (p.Marshaller.RequiresMarshalling)
            {
                if (p.Marshaller.MarshalsToVariable)
                    ctx.Writer.Write($"{p.Marshaller.ManagedType!.GetName()} ");
                MarshalContext childCtx = new(ctx, "managed_" + p.Parameter.Name, p.Parameter.Name);
                p.Marshaller.MarshalFromNative(childCtx);
            }
        }

        ctx.Writer.WriteLine($"GCHandle handle = GCHandle.FromIntPtr(userdata);");
        ctx.Writer.WriteLine($"{type.GetName()} callback = ({type.GetName()})handle.Target!;");
        string argList = string.Join(", ", parameters.Where(parameters => parameters.Marshaller.ManagedType != null).Select(p => p.GetMarshalledName("managed_")));
        ctx.Writer.WriteLine($"callback({argList});");
        ctx.Writer.WriteLine($"handle.Free();");

        ctx.Writer.CloseBraceBlock();
        ctx.Writer.WriteLine($"GCHandle {type.GetName()}_callbackHandle = GCHandle.Alloc({ctx.ManagedName});");
        ctx.Writer.WriteLine($"nint {type.GetName()}_userdata = GCHandle.ToIntPtr({type.GetName()}_callbackHandle);");
    }
}
