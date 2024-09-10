using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CXType = ClangSharp.Type;

// welcome to the jungle

// moved to my BindingGen project

// Todo: Clean up code
// Todo: Make into a usable api
// Todo: Use that api in the BindingGen main project
// Todo: Download xhtml files from https://github.com/BSVino/docs.gl and add the to the project
// Todo: Use downloaded xhtml files to generate inline xml docs for opengl functions.


using ClangSharp;
using ClangSharp.Interop;
using System.Xml;
using System.Globalization;

HashSet<string> requiredFunctions = [
    "glCullFace",
    "glClearColor",
    "glClearDepth",
    "glClear",
    "glGetNamedBufferSubData",
    "glGenTextures"
];

CXIndex index = CXIndex.Create();

string glHeader = "./include/GLES3/gl32.h";

TranslationUnit tu = TranslationUnit.GetOrCreate(CXTranslationUnit.CreateFromSourceFile(index, glHeader, [], []));
string source = File.ReadAllText(glHeader);

StringWriter sw = new();
Generator generator = new(source, tu, requiredFunctions, sw);
generator.GenerateBindings();

File.WriteAllText("./../../../../../SimulationFramework.OpenGL/OpenGL.gen.cs", sw.ToString());

class Generator
{
    TranslationUnit translationUnit;
    HashSet<string> requiredFunctions;
    IndentedTextWriter writer;
    string source;
    private Dictionary<string, string> macroValues = [];

    public Generator(string source, TranslationUnit translationUnit, HashSet<string> requiredFunctions, StringWriter writer)
    {
        this.source = source;
        this.writer = new(writer);
        this.translationUnit = translationUnit;
        this.requiredFunctions = requiredFunctions;

        FindMacroValues();
    }

    private void FindMacroValues()
    {
        ReadOnlySpan<char> source = this.source;
        while (true)
        {
            int index = source.IndexOf("#define ");
            if (index == -1)
            {
                break;
            }

            source = source[(index + 8)..];
            ReadOnlySpan<char> line = source[..source.IndexOf('\n')];

            int space = line.IndexOf(' ');
            if (space != -1)
            {
                ReadOnlySpan<char> name = line[..space].Trim();
                ReadOnlySpan<char> value = line[(space + 1)..].Trim();
                macroValues.Add(name.ToString(), value.ToString());
            }
            source = source[line.Length..];
        }
    }

    public void GenerateBindings()
    {
        writer.WriteLine("""
using System;

namespace Khronos;

#pragma warning disable IDE1006 // Naming Styles

internal static unsafe class OpenGL
{
    public delegate nint FunctionLoader(string name);

    public static void glInitialize(FunctionLoader functionLoader)
    {
        static nint LoadFunction(FunctionLoader functionLoader, string name)
        {
            nint value = functionLoader(name);
            if (value == 0)
            {
                throw new Exception($"Could not load required opengl function '{name}'");
            }
            return value;
        }
""");   
        writer.Indent++;
        writer.Indent++;

        // emit initialization code
        foreach (var decl in translationUnit.TranslationUnitDecl.CursorChildren)
        {
            if (ShouldEmitFunction(decl))
            {
                writer.WriteLine("pfn_" + decl.Spelling + $" = LoadFunction(functionLoader, \"{decl.Spelling}\");");
            }
        }
        writer.Indent--;
        writer.WriteLine("}");

        // emit constants
        foreach (var decl in translationUnit.TranslationUnitDecl.CursorChildren)
        {
            if (decl is MacroDefinitionRecord macroDefinition && decl.Spelling.StartsWith("GL_") && !decl.Spelling.StartsWith("GL_GLEXT") && !decl.Spelling.StartsWith("GL_ARB") && !decl.Spelling.StartsWith("GL_EXT"))
            {
                if (macroValues.TryGetValue(macroDefinition.Spelling, out string? value)) 
                {
                    if (int.TryParse(value, out _) || 
                        (value.StartsWith("0x") && int.TryParse(value[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _))
                        )
                    {
                        writer.WriteLine($"public const uint {macroDefinition.Spelling} = {value};");
                    }
                }
            }
        }

        // emit functions
        foreach (var decl in translationUnit.TranslationUnitDecl.CursorChildren)
        {
            if (ShouldEmitFunction(decl))
            {
                EmitFunction((FunctionDecl)decl);
            }
        }

        writer.Indent--;
        writer.WriteLine("}");
    }

    private bool ShouldEmitFunction(Cursor cursor)
    {
        Console.WriteLine("function: " + cursor.Spelling);
        if (cursor.Spelling == "GL_APIENTRY")
            Console.WriteLine("hello");

        if (cursor is not FunctionDecl fn)
            return false;

        var name = fn.Name;

        if (name.Length < 2)
            return false;

        bool isExtension = char.IsUpper(name[^1]) && char.IsUpper(name[^2]);
        return name.StartsWith("gl") && !isExtension;
    }

    private void EmitFunction(FunctionDecl decl)
    {
        writer.Write("private static nint pfn_");
        writer.Write(decl.Name);
        writer.WriteLine(";");

        writer.Write("public static ");
        EmitType(decl.ReturnType);
        writer.Write(' ');
        writer.Write(decl.Name);
        writer.Write('(');

        foreach (var parameter in decl.Parameters)
        {
            if (parameter != decl.Parameters[0])
                writer.Write(", ");

            EmitType(parameter.Type);
            writer.Write(' ');
            WriteName(parameter.Name);
        }

        writer.Write(") => ((delegate* unmanaged[Stdcall]<");
        foreach (var parameter in decl.Parameters)
        {
            EmitType(parameter.Type);
            writer.Write(", ");
        }
        EmitType(decl.ReturnType);
        writer.Write(">)pfn_");
        writer.Write(decl.Name);

        writer.Write(")(");
        foreach (var parameter in decl.Parameters)
        {
            if (parameter != decl.Parameters[0])
                writer.Write(", ");

            WriteName(parameter.Name);
        }

        writer.WriteLine(");");
        writer.WriteLine();
    }

    private void WriteName(string name)
    {
        if (name is "params" or "ref" or "string" or "struct" or "event" or "object")
        {
            writer.Write("@");
        }

        writer.Write(name);
    }

    private void EmitType(CXType type)
    {
        if (type.AsString == "GLfloat")
        {
            writer.Write("float");
            return;
        }

        if (type.IsSugared)
        {
            EmitType(type.Desugar);
            return;
        }

        if (type.Kind == CXTypeKind.CXType_Pointer)
        {
            EmitType(type.PointeeType);
            if (type.PointeeType.Kind != CXTypeKind.CXType_FunctionProto)
            {
                writer.Write("*");
            }
            return;
        }

        if (type.Kind == CXTypeKind.CXType_FunctionProto)
        {
            var fnType = (FunctionProtoType)type;
            writer.Write("delegate* unmanaged[Stdcall]<");
            foreach (var parameter in fnType.ParamTypes)
            {
                EmitType(parameter);
                writer.Write(", ");
            }
            EmitType(fnType.ReturnType);
            writer.Write(">");
            return;
        }

        writer.Write(type.Kind switch
        {
            CXTypeKind.CXType_Void => "void",
            CXTypeKind.CXType_Int => "int",
            CXTypeKind.CXType_UInt => "uint",
            CXTypeKind.CXType_Float => "float",
            CXTypeKind.CXType_Double => "double",
            CXTypeKind.CXType_UChar => "byte",
            CXTypeKind.CXType_Char_S => "byte",
            CXTypeKind.CXType_Record => "void",
            _ => throw new(type.Kind.ToString()),
        });
    }
}

class DocumentationLoader
{
    private Dictionary<string, XmlDocument> documentationPages = [];

    public Documentation? Load(string functionName)
    {
        try
        {
            XmlDocument document = new XmlDocument();
            document.Load($"https://raw.githubusercontent.com/BSVino/docs.gl/mainline/es3/{functionName}.xhtml");
            Console.WriteLine(document);
            var parametersNode = document.GetElementById("parameters");
            throw new();
        }
        catch
        {
            Console.WriteLine($"Could not load docs for {functionName}!");
            return null;
        }
    }
}

class Documentation
{
    public string FunctionDescription;
    public Dictionary<string, string> parameterDescriptions = [];
}

internal static unsafe class OpenGL
{
    public delegate nint FunctionLoader(string name);

    public static void glInitialize(FunctionLoader functionLoader)
    {
        static nint LoadFunction(FunctionLoader functionLoader, string name)
        {
            nint value = functionLoader(name);
            if (value == 0)
            {
                throw new Exception($"Could not load required opengl function '{name}'");
            }
            return value;
        }
        pfn_glCullFace = LoadFunction(functionLoader, "glCullFace");
        pfn_glClearColor = LoadFunction(functionLoader, "glClearColor");
    }
    private static nint pfn_glCullFace;
    public static void glCullFace(uint mode) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glCullFace)(mode);
    private static nint pfn_glClearColor;
    public static void glClearColor(int red, int green, int blue, int alpha) => ((delegate* unmanaged[Stdcall]<int, int, int, int, void>)pfn_glClearColor)(red, green, blue, alpha);
}
