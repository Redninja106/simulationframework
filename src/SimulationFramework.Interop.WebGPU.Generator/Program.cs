using ClangSharp;
using CppAst;
using CppAst.CodeGen.Common;
using CppAst.CodeGen.CSharp;
using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using WebGPU.Generator;

/* TODO:
 *      generate [Flags] attributes
 *      figure out pointers in managed structs
 *      figure out chaining
 *      in/out parameters
 *      callbacks
 *      nullable handles/pointers
 *      handle large arrays
 *      reverse marshalling
 *      correct function/handle matching
 *      don't marshal structs that don't need it 
 */

string[] ignoredSymbols = [
    
    // -- implemented manually --

    "Device.EnumerateFeatures",
    // "Device.GetLimits",

    "Adapter.EnumerateFeatures",
    // "Adapter.GetLimits",
    "Adapter.GetProperties",

    "Surface.GetCapabilities",

    "Native.wgpuGetProcAddress", // (avoids generating WGPUProc)

    "Queue.WriteBuffer", // array matching doesn't work due to naming inconsistencies

    "Buffer.GetMappedRange",

    "Surface.CapabilitiesFreeMembers", // degenerate case of wgpuSurfaceCapabilitesFreeMembers()

];

// structs that should be marshalled as out parameters
string[] outParams = [
    "Surface.GetCurrentTexture.surfaceTexture",
    "Device.GetLimits.limits",
    "Adapter.GetLimits.limits",
];

string header = File.ReadAllText("Headers/webgpu.h");
var options = new CppParserOptions();
options.ParseSystemIncludes = false;
options.SystemIncludeFolders.Add("C:\\Program Files\\LLVM\\lib\\clang\\17\\include\\");

CppCompilation compilation = CppParser.Parse(header, options, cppFilename: "webgpu.h");

List<CSharpType> types = new();

GenerationContext context = new(new(), compilation, ignoredSymbols, outParams);

StructGenerator structGenerator = new(context);
HandleGenerator handleGenerator = new(context);
DelegateGenerator delegateGenerator = new(context);

CSharpClass graphicsObject = new("GraphicsObject");

foreach (var cppEnum in compilation.Enums)
{
    var csEnum = new CSharpEnum(cppEnum.Name[4..]);

    foreach (var typeDef in compilation.Typedefs)
    {
        if (typeDef.Name.EndsWith("Flags") && typeDef.Name[4..^5] == csEnum.Name)
        {
            context.TypeMapper.DeclareMapping(typeDef, new CSharpFreeType(typeDef.Name[4..^5]));
            csEnum.Attributes.Add(new CSharpFreeAttribute("Flags"));
        }
    }

    foreach (var item in cppEnum.Items)
    {
        if (item.Name.EndsWith("_Force32"))
            continue;

        var name = item.Name[(cppEnum.Name.Length + 1)..];

        if (char.IsDigit(name[0]))
            name = "_" + name;

        csEnum.Members.Add(new CSharpEnumItem(name, item.ValueExpression.ToString()));
    }
    context.TypeMapper.DeclareMapping(cppEnum, csEnum);
    types.Add(csEnum);
}

foreach (var cppClass in compilation.Classes.Reverse())
{
    CSharpType type;
    if (cppClass.Name.EndsWith("Impl"))
    {
        type = new CSharpClass(cppClass.Name[4..^4]);
    }
    else
    {
        type = new CSharpStruct(cppClass.Name[4..]);
    }

    context.TypeMapper.DeclareMapping(cppClass, type);
}

foreach (var typeDef in compilation.Typedefs)
{
    if (!typeDef.Name.EndsWith("Callback"))
        continue;

    var @delegate = delegateGenerator.GenerateDelegate(typeDef);
    context.TypeMapper.DeclareMapping(typeDef, @delegate);
    types.Add(@delegate);
}

foreach (var cppClass in compilation.Classes.Reverse())
{
    if (cppClass.Name.EndsWith("Impl"))
    {
        types.Add(handleGenerator.GenerateHandle(cppClass));
    }
    else
    {
        types.Add(structGenerator.GenerateStruct(cppClass).CSharpStruct);
    }
}

CSharpClass native = new CSharpClass("Native");
native.Visibility = CSharpVisibility.Internal;
native.Modifiers |= CSharpModifiers.Unsafe;

foreach (var func in compilation.Functions)
{
    var method = new CSharpMethod();
    method.Name = func.Name;

    if (context.IgnoredSymbols.Contains("Native." + method.Name))
    {
        continue;
    }

    var retMarshaller = Marshaller.Create(context, context.TypeMapper.Map(func.ReturnType));
    method.ReturnType = retMarshaller.UnmanagedType;

    var marshallers = Marshaller.CreateWithArrays(context, func.Parameters.Select(p => (context.TypeMapper.Map(p.Type), p.Name)).ToArray());

    method.Parameters.AddRange(marshallers.Zip(func.Parameters).Select(pair => new CSharpParameter(pair.Second.Name) { ParameterType = pair.First.UnmanagedType }));
    method.Modifiers |= CSharpModifiers.Extern;
    method.Modifiers |= CSharpModifiers.Static;
    
    var attr = new CSharpFreeAttribute($"DllImport(LIBRARY_NAME)");
    method.Attributes.Add(attr);

    native.Members.Add(method);
}
types.Add(native);

string start = 
@"using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace WebGPU;
";
string content = types.Select(t => t.ToFullString()).Aggregate((a, b) => a + "\n" + b);

WriteOutput("WebGPU.gen.cs", start + content);

void WriteOutput(string file, string content)
{
    const string outputDirectory = "./../../../../../WebGPU/Generated/";
    Directory.CreateDirectory(outputDirectory);
    File.WriteAllText(outputDirectory + file, content);
}
