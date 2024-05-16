using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class UserDataMarshaller(GenerationContext Context, DelegateMarshaller delegateMarshaller) : Marshaller(Context, new CSharpFreeType("nint"))
{
    public override CSharpType? ManagedType => null;

    public override void MarshalToNative(MarshalContext context)
    {
        base.MarshalToNative(new(context, delegateMarshaller.ManagedType!.GetName() + "_userdata", context.UnmanagedName));
    }

    public override void MarshalFromNative(MarshalContext context)
    {
        base.MarshalToNative(new(context, delegateMarshaller.ManagedType!.GetName() + "_userdata", context.UnmanagedName));
    }

    public override string MakeArgument(string value)
    {
        return base.MakeArgument(delegateMarshaller.ManagedType!.GetName() + "_userdata");
    }
}
