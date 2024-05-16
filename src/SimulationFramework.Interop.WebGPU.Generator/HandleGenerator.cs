using CppAst;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio.FileSystems;

namespace WebGPU.Generator;
internal class HandleGenerator(GenerationContext context)
{
    CSharpClass graphicsObject = new("GraphicsObject");

    public CSharpClass GenerateHandle(CppClass @class)
    {
        var csClass = new CSharpClass(@class.Name[4..^4]);
        csClass.BaseTypes.Add(graphicsObject);
        csClass.Modifiers |= CSharpModifiers.Unsafe;

        var ctor = new CSharpFreeMember();
        ctor.Text = @$"public {csClass.Name}(nint nativeHandle) : base(nativeHandle)
{{
}}";
        csClass.Members.Add(ctor);

        foreach (var func in GetFunctionsForHandleType(@class))
        {
            var method = new CSharpMethod();
            method.Name = func.Name[(4 + csClass.Name.Length)..];

            var fullName = $"{csClass.Name}.{method.Name}";
            if (context.IgnoredSymbols.Contains(fullName))
                continue;

            var retType = context.TypeMapper.Map(func.ReturnType, false, true);
            var retMarshaller = Marshaller.Create(context, retType);
            method.ReturnType = retMarshaller.ManagedType;

            var parameters = MarshalledParameter.Map(context, $"{csClass.Name}.{method.Name}", func.Parameters.Skip(1)).ToArray();
            method.Parameters.AddRange(parameters.Where(p => p.Marshaller.ManagedType is not null).Select(p => p.Parameter));

            var w = new CodeWriter(new CodeWriterOptions(new MemoryFileSystem(), CodeWriterMode.Full));

            bool hasFixedStatements = false;
            foreach (var fixedStatement in parameters.SelectMany(p => p.Marshaller.GetFixedStatements(new(w, p.Parameter.Name, "native_" + p.Parameter.Name))))
            {
                hasFixedStatements = true;
                w.WriteLine(fixedStatement);
            }

            if (hasFixedStatements)
                w.OpenBraceBlock();

            foreach (var p in parameters)
            {
                if (p.Marshaller.RequiresMarshalling && !context.IsOutParameter($"{csClass.Name}.{method.Name}", p.Parameter.Name))
                {
                    if (p.Marshaller.MarshalsToVariable)
                        w.Write($"{p.Marshaller.UnmanagedType} ");
                    MarshalContext ctx = new(w, p.Parameter.Name, "native_" + p.Parameter.Name);
                    p.Marshaller.MarshalToNative(ctx);
                }
            }

            foreach (var p in parameters)
            {
                if (p.Marshaller.RequiresMarshalling && context.IsOutParameter($"{csClass.Name}.{method.Name}", p.Parameter.Name))
                {
                    MarshalContext ctx = new(w, p.Parameter.Name, "native_" + p.Parameter.Name);
                    p.Marshaller.MarshalFromNativeAlloc(ctx);
                }
            }

            if (retType.GetName() != "void")
                w.Write("var result = ");
            
            w.WriteLine($"Native.{func.Name}(this.NativeHandle{string.Join("", parameters.Select(p => ", " + p.GetMarshalledName("native_")))});");

            foreach (var p in parameters)
            {
                if (p.Marshaller.RequiresMarshalling && !context.IsOutParameter($"{csClass.Name}.{method.Name}", p.Parameter.Name))
                {
                    MarshalContext ctx = new(w, p.Parameter.Name, "native_" + p.Parameter.Name);
                    p.Marshaller.MarshalToNativeFree(ctx);
                }
            }

            foreach (var p in parameters)
            {
                if (p.Marshaller.RequiresMarshalling && context.IsOutParameter($"{csClass.Name}.{method.Name}", p.Parameter.Name))
                {
                    MarshalContext ctx = new(w, p.Parameter.Name, "native_" + p.Parameter.Name);
                    p.Marshaller.MarshalFromNative(ctx);
                }
            }

            bool isReturningClass = method.ReturnType is CSharpClass;
            if (retType.GetName() != "void")
            {
                w.Write("return ");
                w.Write(isReturningClass ? "new(result)" : "result");
                w.WriteLine(";");
            }

            if (hasFixedStatements)
                w.CloseBraceBlock();

            string body = w.CurrentWriter.ToString()!;

            if (method.Parameters.Count is 0 && method.Name.StartsWith("Get"))
            {
                var property = new CSharpProperty(method.Name[3..])
                {
                    ReturnType = method.ReturnType,
                    GetBody = (w, e) => w.Write(body),
                };
                property.Visibility = CSharpVisibility.Public;
                csClass.Members.Add(property);
                continue;
            }

            method.Body = (w, e) => w.Write(body);
            csClass.Members.Add(method);
        }

        csClass.Members.Add(new CSharpFreeMember()
        {
            Text = @"public override void Dispose() 
{
    this.Release();
    base.Dispose();
}
"
        });

        return csClass;
    }


    private IEnumerable<CppFunction> GetFunctionsForHandleType(CppClass @class)
    {
        return context.Compilation.Functions.Where(f => GetBestMatchForFunction(f) == @class);
    }

    private CppClass? GetBestMatchForFunction(CppFunction function)
    {
        string functionName = function.Name[4..]; // trim wgpu prefix

        int bestMatchStrength = 0; // strength of a match is how many chars matched
        CppClass? bestMatch = null;

        foreach (var @class in context.Compilation.Classes)
        {
            if (!@class.Name.EndsWith("Impl"))
                continue;

            string name = @class.Name[4..^4]; // trim wgpu prefix and impl postfix

            int index = 0;
            while (index < functionName.Length && index < name.Length)
            {
                if (functionName[index] != name[index])
                    break;
                index++;
            }

            // has to have matched the whole name plus be the best one
            if (index >= name.Length && index > bestMatchStrength)
            {
                bestMatch = @class;
            }
        }

        return bestMatch;
    }

    
}

record MarshalledParameter(CSharpParameter Parameter, Marshaller Marshaller)
{
    public static IEnumerable<MarshalledParameter> Map(GenerationContext context, string parentName, IEnumerable<CppParameter> parameters)
    {
        (CSharpType, string)[] elements = parameters.Select(p => (context.TypeMapper.Map(p.Type), p.Name)).ToArray();
        var marshallers = Marshaller.CreateWithArrays(context, elements);
        return parameters.Zip(marshallers).Select(pair =>
            new MarshalledParameter(
                new CSharpParameter(pair.First.Name)
                {
                    ParameterType = AddParameterModifiers(context.IsOutParameter(parentName, pair.First.Name), pair.Second.ManagedType),
                },
                pair.Second
                )
            );
    }

    static CSharpType AddParameterModifiers(bool isOut, CSharpType type)
    {
        if (isOut)
        {
            return new CSharpFreeType("out " + type.GetName().TrimEnd('?'));
        }

        if (type is CSharpPointerType ptr)
        {
            return new CSharpFreeType("ref " + ptr.ElementType.GetName());
        }

        return type;
    }

    public string GetMarshalledName(string prefix)
    {
        if (Parameter.Name is "userdata")
        {
            // return Marshaller..GetName() + "_userdata";
        }

        var name = (Marshaller.RequiresMarshalling ? prefix : "") + Parameter.Name;
        return Marshaller.MakeArgument(name);
    }

}   