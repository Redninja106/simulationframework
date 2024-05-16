using CppAst;
using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebGPU.Generator;
internal class GenerationContext(TypeMapper typeMapper, CppCompilation compilation, string[] ignoredSymbols, string[] outStructs)
{
    public TypeMapper TypeMapper { get; } = typeMapper;
    public CppCompilation Compilation { get; } = compilation;
    public List<GeneratedStruct> Structs { get; } = [];
    public HashSet<string> IgnoredSymbols { get; } = new(ignoredSymbols);
    public HashSet<string> OutParameters { get; } = new(outStructs);

    public string ToPascalCase(string camelCase)
    {
        if (char.IsLower(camelCase[0]))
        {
            return char.ToUpper(camelCase[0]) + camelCase[1..];
        }
        return camelCase;
    }

    public string ToCamelCase(string pascalCase)
    {
        if (char.IsUpper(pascalCase[0]))
        {
            return char.ToLower(pascalCase[0]) + pascalCase[1..];
        }
        return pascalCase;
    }

    public bool IsOutParameter(string parentName, string parameter)
    {
        return OutParameters.Contains($"{parentName}.{parameter}");
    }
    public CSharpParameter TranslateParameterManaged(CppParameter parameter)
    {
        var result = new CSharpParameter();
        result.Name = parameter.Name;
        result.ParameterType = TypeMapper.Map(parameter.Type, true);
        return result;
    }

    public CSharpParameter TranslateParameterUnmanaged(CppParameter parameter)
    {
        var result = new CSharpParameter();
        result.Name = parameter.Name;
        result.ParameterType = TypeMapper.Map(parameter.Type);
        return result;
    }

    public GeneratedStruct? GetGeneratedStruct(string name)
    {
        return Structs.SingleOrDefault(s => s.CSharpStruct.GetName() == name);
    }
}
