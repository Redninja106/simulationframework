using Microsoft.VisualBasic.FileIO;
using SimulationFramework.Shaders;
using SimulationFramework.Shaders.Compiler;
using SimulationFramework.Shaders.Compiler.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11.ShaderGen;

public class HLSLCodeGenerator : CodeGenerator
{
    private static readonly Dictionary<Type, string> typeAliases = new()
    {
        [typeof(void)] = "void",
        [typeof(float)] = "float",
        [typeof(uint)] = "uint",
        [typeof(int)] = "int",
        [typeof(Vector2)] = "float2",
        [typeof(Vector3)] = "float3",
        [typeof(Vector4)] = "float4",
        [typeof(Matrix4x4)] = "float4x4",
        [typeof(Matrix3x2)] = "float3x2",
        [typeof(ColorF)] = "float4",
    };

    private static readonly Dictionary<MethodInfo, string> intrinsicAliases = new()
    {
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Mul))] = "mul",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Vec4), new[] { typeof(Vector3), typeof(float) })] = "float4",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Vec4), new[] { typeof(Vector2), typeof(float), typeof(float) })] = "float4",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Vec4), new[] { typeof(float), typeof(float), typeof(float), typeof(float) })] = "float4",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Vec3), new[] { typeof(float), typeof(float), typeof(float) })] = "float3",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Vec3), new[] { typeof(float) })] = "float3",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Sqrt), new[] { typeof(float) })] = "sqrt",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Normalize), new[] { typeof(Vector3) })] = "normalize",
        [typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.Dot), new[] { typeof(Vector3), typeof(Vector3) })] = "dot",
    };

    private static readonly Dictionary<MemberInfo, string> memberAliases = new()
    {
        [typeof(Vector2).GetField(nameof(Vector2.X))] = "x",
        [typeof(Vector2).GetField(nameof(Vector2.Y))] = "y",
        [typeof(Vector3).GetField(nameof(Vector3.X))] = "x",
        [typeof(Vector3).GetField(nameof(Vector3.Y))] = "y",
        [typeof(Vector3).GetField(nameof(Vector3.Z))] = "z",
        [typeof(Vector4).GetField(nameof(Vector4.X))] = "x",
        [typeof(Vector4).GetField(nameof(Vector4.Y))] = "y",
        [typeof(Vector4).GetField(nameof(Vector4.Z))] = "z",
        [typeof(Vector4).GetField(nameof(Vector4.W))] = "w",

        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M11))] = "_11",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M12))] = "_12",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M13))] = "_13",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M14))] = "_14",

        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M21))] = "_21",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M22))] = "_22",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M23))] = "_23",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M24))] = "_24",

        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M31))] = "_31",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M32))] = "_32",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M33))] = "_33",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M34))] = "_34",

        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M41))] = "_41",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M42))] = "_42",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M43))] = "_43",
        //[typeof(Matrix4x4).GetField(nameof(Matrix4x4.M44))] = "_44",

    };

    private Dictionary<FieldInfo, string> fieldSemantics = new();

    private static readonly Dictionary<InputSemantic, string> inputSemanticAliases = new()
    {
        [InputSemantic.Position] = "SV_Position",
    };

    private static readonly Dictionary<OutputSemantic, string> outputSemanticAliases = new()
    {
        [OutputSemantic.Position] = "SV_Position",
        [OutputSemantic.Color] = "SV_Target",
    };

    public HLSLCodeGenerator(ShaderCompilation compilation) : base(compilation)
    {

    }

    protected override void VisitVariables(IEnumerable<CompiledVariable> variables)
    {
        var uniforms = variables.Where(v => v.IsUniform);
        
        Writer.WriteLine();
        Writer.WriteLine("cbuffer __cbuffer : register(b0)");
        Writer.WriteLine("{");
        Writer.Indent++;

        foreach (var uniform in uniforms)
        {
            VisitType(uniform.VariableType);
            Writer.Write(' ');
            VisitIdentifier(uniform.Name);
            Writer.WriteLine(';');
        }

        Writer.Indent--;
        Writer.WriteLine("}");

        var inputs = variables.Where(v => v.IsInput);

        Writer.WriteLine("struct __inputstruct");
        Writer.WriteLine("{");
        Writer.Indent++;

        int idx = 0;
        foreach (var input in inputs)
        {
            VisitType(input.VariableType);
            Writer.Write(' ');
            VisitIdentifier(input.Name);

            string semantic = null;
            if (input.InputSemantic is not null && inputSemanticAliases.ContainsKey(input.InputSemantic.Value))
            {
                semantic = inputSemanticAliases[input.InputSemantic.Value];
            }
            else
            {
                if (Compilation.ShaderKind is ShaderKind.Fragment) 
                {
                    semantic = $"TEXCOORD{idx++}";
                }
                else
                {
                    if (input.VariableType == typeof(int) ||
                        input.VariableType == typeof(uint) ||
                        input.VariableType == typeof(float) ||
                        input.VariableType == typeof(Vector2) ||
                        input.VariableType == typeof(Vector3) ||
                        input.VariableType == typeof(Vector4))
                    {
                        semantic = input.Name;
                    }
                }
            }
            

            if (semantic is not null)
            {
                Writer.Write(" : ");
                Writer.Write(semantic);
            }

            Writer.WriteLine(';');
        }

        Writer.Indent--;
        Writer.WriteLine("};");

        var outputs = variables.Where(v => v.IsOutput);

        Writer.WriteLine("struct __outputstruct");
        Writer.WriteLine("{");
        Writer.Indent++;

        idx = 0;
        foreach (var output in outputs.Reverse())
        {
            VisitType(output.VariableType);
            Writer.Write(' ');
            VisitIdentifier(output.Name);

            string semantic = null;
            if (output.OutSemantic is not null && outputSemanticAliases.ContainsKey(output.OutSemantic.Value))
            {
                semantic = outputSemanticAliases[output.OutSemantic.Value];
            }
            else
            {
                if (Compilation.ShaderKind == ShaderKind.Vertex)
                {
                    semantic = $"TEXCOORD{idx++}";
                }
                else
                { 
                    semantic = output.Name;
                } 
            }

            if (semantic is not null)
            {
                Writer.Write(" : ");
                Writer.Write(semantic);
            }

            Writer.WriteLine(';');
        }

        Writer.Indent--;
        Writer.WriteLine("};");
        Writer.WriteLine("static __inputstruct __input;");
        Writer.Write("static __outputstruct __output;");
    }

    protected override void VisitStruct(CompiledStruct @struct)
    {
        base.VisitStruct(@struct);
        Writer.Write(";");
    }

    protected override void VisitIdentifier(string identifier)
    {
        if (identifier == "matrix")
        {
            identifier = "_" + identifier;
        }

        base.VisitIdentifier(identifier);
    }

    protected override void VisitField(FieldInfo field)
    {
        VisitType(field.FieldType);
        Writer.Write(' ');
        VisitIdentifier(field.Name);

        string semantic;
        var inputAttribute = field.GetCustomAttribute<ShaderInputAttribute>();
        var outputAttribute = field.GetCustomAttribute<ShaderOutputAttribute>();
        if (inputAttribute is not null && inputSemanticAliases.ContainsKey(inputAttribute.Semantic))
        {
            semantic = inputSemanticAliases[inputAttribute.Semantic];
        }
        else if (outputAttribute is not null && outputSemanticAliases.ContainsKey(outputAttribute.Semantic))
        {
            semantic = outputSemanticAliases[outputAttribute.Semantic];
        }
        else if (inputAttribute?.LinkageName is not null)
        {
            semantic = inputAttribute.LinkageName;
        }
        else if (outputAttribute?.LinkageName is not null)
        {
            semantic = outputAttribute.LinkageName;
        }
        else
        {
            semantic = field.Name;
        }

        Writer.Write(" : ");
        Writer.Write(semantic);

        //Writer.WriteLine(';');
    }

    protected override void VisitType(Type type)
    {
        string name;

        if (typeAliases.ContainsKey(type))
        {
            name = typeAliases[type];
        }
        else
        {
            name = type.Name;
        }

        this.Writer.Write(name);
    }

    protected override void VisitMethod(CompiledMethod method)
    {
        if (method == this.Compilation.EntryPoint)
        {
            Writer.WriteLine();
            Writer.Write("__outputstruct ");
            Writer.Write(method.Name);
            Writer.WriteLine("(__inputstruct _input)");
            Writer.WriteLine("{");
            Writer.Indent++;

            Writer.WriteLine("__input = _input;");

            foreach (var typeGroup in method.Body.Variables.GroupBy(p => p.Type))
            {
                VisitType(typeGroup.Key);
                Writer.Write(" ");

                VisitIdentifier(typeGroup.First().Name);
                foreach (var element in typeGroup.Skip(1))
                {
                    Writer.Write(", ");
                    VisitIdentifier(element.Name);
                }

                Writer.WriteLine(";");
            }

            foreach (var expr in method.Body.Expressions.SkipLast(1))
            {
                Visit(expr);
                Writer.WriteLine(";");
            }

            Writer.WriteLine("return __output;");

            Writer.Indent--;
            Writer.WriteLine("}");
        }
        else
        {
            base.VisitMethod(method);
        }
    }

    protected override Expression VisitCompiledVariableExpression(CompiledVariableExpression node)
    {
        if (node.Variable.IsInput) 
        {
            Writer.Write("__input.");
        }
        else if (node.Variable.IsOutput)
        {
            Writer.Write("__output.");
        }

        return base.VisitCompiledVariableExpression(node);
    }

    protected override Expression VisitIntrinsicCall(IntrinsicCallExpression node)
    {
        string name;
        if (node.Method is MethodInfo i && intrinsicAliases.ContainsKey(i))
        {
            name = intrinsicAliases[i];
        }
        else
        {
            name = node.Method.Name;
        }

        VisitIdentifier(name);

        Writer.Write('(');

        if (node.Arguments.Any())
        {
            Visit(node.Arguments.First());

            foreach (var arg in node.Arguments.Skip(1))
            {
                Writer.Write(", ");
                Visit(arg);
            }
        }

        Writer.Write(')');

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member is FieldInfo field && field.DeclaringType == typeof(Matrix4x4))
        {
            Visit(node.Expression);
            var name = field.Name;
            var row = int.Parse(name[1].ToString()) - 1;
            var col = int.Parse(name[2].ToString()) - 1;
            Writer.Write($"[{row}][{col}]");
            return node;
        }
        else if (memberAliases.ContainsKey(node.Member))
        {
            Visit(node.Expression);
            Writer.Write(".");
            Writer.Write(memberAliases[node.Member]);
            return node;
        }
        else
        {
            return base.VisitMember(node);
        }
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        Writer.Write("dasdasd");
        return base.VisitMethodCall(node);
    }
}
