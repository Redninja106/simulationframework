using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class ArrayLengthMarshaller(GenerationContext context, CSharpType type) : Marshaller(context, type)
{
    public override bool RequiresMarshalling => false;

    public override CSharpType? ManagedType => null;

    public override void MarshalToNative(MarshalContext context)
    {
        context.Writer.WriteLine($"{context.UnmanagedName} = ({this.UnmanagedType})({GetArrayName(context.ManagedName)}?.Length ?? 0);");
    }

    public override void MarshalFromNative(MarshalContext context)
    {
    }

    public override string MakeArgument(string value)
    {
        return "(nuint)" + GetArrayName(value) + ".Length";
    }

    private string GetArrayName(string value)
    {
        string arrayName;
        if (value.EndsWith("Count", StringComparison.OrdinalIgnoreCase))
        {
            if (value[..^5].EndsWith("Entry"))
                return value[..^10] + "Entries";

            arrayName = value[..^5] + "s";
        }
        else if (value.EndsWith("Size", StringComparison.OrdinalIgnoreCase))
        {
            arrayName = value[..^4];
        }
        else
        {
            throw new();
        }

        return arrayName;
    }
}