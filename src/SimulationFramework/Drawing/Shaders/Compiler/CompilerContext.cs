using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

internal class CompilerContext
{
    private readonly Dictionary<Type, ShaderType> primitiveTypeMap;

    public ShaderCompilation Compilation { get; }
    public Queue<MethodBase> MethodQueue { get; } = [];
    public Dictionary<MethodBase, ShaderMethod> Methods { get; } = [];
    public Dictionary<Type, ShaderStructureType> Structs { get; } = [];

    public HashSet<MethodInfo> IntrinsicMethods { get; } = [];
    public Dictionary<MethodBase, MethodInfo> IntrinsicIntercepts { get; } = [];

    public Dictionary<FieldInfo, ShaderVariable> Uniforms { get; } = [];

    public Type ShaderType { get; }
    public MethodInfo EntryPoint { get; }

    public CompilerContext(Type shaderType, MethodInfo entryPoint, Dictionary<Type, ShaderType> primitiveTypeMap)
    {
        ShaderType = shaderType;
        EntryPoint = entryPoint;
        this.primitiveTypeMap = primitiveTypeMap;
        MethodQueue = [];
        
        Compilation = new();

        foreach (var method in typeof(ShaderIntrinsics).GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (method.GetCustomAttribute<ShaderIntrinsicAttribute>() is null)
                continue;

            IntrinsicMethods.Add(method);

            foreach (var interceptAttr in method.GetCustomAttributes<ShaderInterceptAttribute>())
            {
                MethodBase? interceptedMethod = interceptAttr.GetMethod(method.GetParameters().Select(p => p.ParameterType).ToArray());

                if (interceptedMethod != null)
                {
                    IntrinsicIntercepts.Add(interceptedMethod, method);
                }
                else
                {
                    Console.WriteLine($"Warning: could not resolve shader intercept for " + method.Name);
                }
            }
        }
    }

    public ShaderMethod EnqueueMethod(MethodBase method)
    {
        if (Methods.TryGetValue(method, out ShaderMethod? compiled))
        {
            return compiled;
        }

        MethodQueue.Enqueue(method);

        ShaderMethod shaderMethod = new();
        
        if (method is ConstructorInfo ctor)
        {
            shaderMethod.ReturnType = CompileType(ctor.DeclaringType);
        }
        else if (method is MethodInfo m)
        {
            shaderMethod.ReturnType = CompileType(m.ReturnType);
        }
        else
        {
            throw new UnreachableException();
        }

        Methods.Add(method, shaderMethod);
        return shaderMethod;
    }

    public MethodInfo? ResolveMethodToIntrinsic(MethodBase method)
    {
        if (IntrinsicMethods.Contains(method))
        {
            return (MethodInfo)method;
        }
        else
        {
            if (method is MethodInfo methodInfo && method.IsGenericMethod)
            {
                if (IntrinsicMethods.Contains(methodInfo.GetGenericMethodDefinition()))
                {
                    return methodInfo;
                }
            }
        }

        if (IntrinsicIntercepts.TryGetValue(method, out MethodInfo? intrinsic))
        {
            return intrinsic;
        }

        if ((method.DeclaringType?.IsArray ?? false) && method.Name == "Get")
        {
            int rank = method.DeclaringType.GetArrayRank();
            switch (rank)
            {
                case 1:
                    return typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLoad), [typeof(object), typeof(int)]);
                case 2:
                    return typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLoad), [typeof(object), typeof(int), typeof(int)]);
                case 3:
                    return typeof(ShaderIntrinsics).GetMethod(nameof(ShaderIntrinsics.BufferLoad), [typeof(object), typeof(int), typeof(int), typeof(int)]);
                default:
                    throw new Exception($"Unsupported array type '{method.DeclaringType}'!");
            }
        }

        return null;
    }

    public ShaderType CompileType(Type type)
    {
        if (IsSelfType(type))
        {
            return new ShaderGlobalType(type);
        }

        if (Structs.TryGetValue(type, out var result))
        {
            return result;
        }

        if (type.IsByRef)
        {
            return new ShaderReferenceType(CompileType(type.GetElementType()));
        }

        if (type.IsEnum)
        {
            return CompileType(Enum.GetUnderlyingType(type));
        }

        if (this.primitiveTypeMap.TryGetValue(type, out ShaderType? primitive))
        {
            return primitive;
        }

        if (type.IsValueType)
        {
            var shaderStruct = CompileStruct(type);
            var shaderType = new ShaderStructureType(shaderStruct);
            Structs.Add(type, shaderType);
            Compilation.Structures.Add(shaderType.structure);
            return shaderType;
        }

        if (type.IsArray && type.GetElementType()!.IsValueType)
        {
            return new ShaderArrayType(CompileType(type.GetElementType()!));
        }

        throw new Exception($"type {type} is not supported!");
    }

    private ShaderStructure CompileStruct(Type structType)
    {
        if (structType.IsPrimitive)
        {
            throw new NotSupportedException("Unsupported primitive type " + structType.ToString());
        }

        FieldInfo[] fieldInfos = structType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        ShaderVariable[] fields = new ShaderVariable[fieldInfos.Length];
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            var fieldInfo = fieldInfos[i];
            fields[i] = new ShaderVariable(
                CompileType(fieldInfo.FieldType),
                new(fieldInfo.Name),
                fieldInfo,
                ShaderVariableKind.Field
                );
        }

        return new()
        {
            fields = fields,
            name = new(structType.Name),
        };
    }

    internal bool IsSelfType(Type declaringType)
    {
        return declaringType == ShaderType || ShaderType.IsSubclassOf(declaringType);
    }
}