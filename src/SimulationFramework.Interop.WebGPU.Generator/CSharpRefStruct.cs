using CppAst.CodeGen.CSharp;

namespace WebGPU.Generator;

internal class CSharpRefStruct : CSharpStruct
{
    public bool IsPartial { get; set; }

    public CSharpRefStruct(string name) : base(name)
    {
        IsPartial = this.Modifiers.HasFlag(CSharpModifiers.Partial);
        this.Modifiers &= ~CSharpModifiers.Partial;
    }

    protected override string DeclarationKind => $"ref{(IsPartial ? " partial " : " ")}struct";
}