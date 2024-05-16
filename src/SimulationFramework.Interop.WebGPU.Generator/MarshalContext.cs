using CppAst.CodeGen.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class MarshalContext
{
    public MarshalContext(CodeWriter writer, string managedName, string unmanagedName)
    {
        ManagedName = managedName;
        UnmanagedName = unmanagedName;
        ParentContext = null;
        Writer = writer;
    }


    public MarshalContext(MarshalContext parentContext, string managedName, string unmanagedName)
    {
        ManagedName = managedName;  
        UnmanagedName = unmanagedName;
        ParentContext = parentContext;
        Writer = parentContext.Writer;
        CanPinArrays = parentContext.CanPinArrays;
    }

    public bool CanPinArrays { get; init; } = true;
    public string ManagedName { get; }
    public string UnmanagedName { get; }
    public CodeWriter Writer { get; }
    public MarshalContext? ParentContext { get; }

    public string SanitizeName(string name)
    {
        return name
            .Replace("?.", "_")
            .Replace("->", "_")
            .Replace(".", "_");
    }
}