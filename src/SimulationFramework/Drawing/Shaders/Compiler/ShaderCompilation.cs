using SimulationFramework.Drawing.Shaders.Compiler.ILDisassembler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders.Compiler;

/// <summary>
/// gradually built up by stages during compilation
/// </summary>
public class ShaderCompilation
{
    private readonly List<ShaderMethod> methods = new();
    private readonly List<ShaderUniform> uniforms = new();

    public IReadOnlyList<ShaderMethod> Methods => methods;
    public IReadOnlyList<ShaderUniform> Uniforms => uniforms;

    public ShaderMethod EntryPoint { get; internal set; }

    //private List<ShaderStructure> structs;

    //public ShaderMethod EntryPoint { get; }

    //public IEnumerable<ShaderMethod> Methods => methods;
    //public IEnumerable<ShaderStructure> Structs => structs;
    //public IEnumerable<ShaderVariable> Uniforms => uniforms;

    //public ShaderCompilation()
    //{

    //}

    //public ShaderCompilation(ShaderMethod entryPoint, IEnumerable<ShaderMethod> methods, IEnumerable<ShaderStructure> structs, IEnumerable<ShaderVariable> uniforms)
    //{
    //    this.EntryPoint = entryPoint;
    //    this.methods = new(methods);
    //    this.structs = new(structs);
    //    this.uniforms = new(uniforms);
    //}

    //public void AddDisassembly(MethodDisassembly disassembly)
    //{

    //}

    //internal ShaderMethod GetOrAddShaderMethod(MethodBase method)
    //{
    //    throw new NotImplementedException();
    //}

    public ShaderMethod GetMethod(MethodBase method)
    {
        return methods.Single(m => m.Method == method);
    }

    internal void AddMethod(MethodBase method)
    {
        methods.Add(new ShaderMethod(method));
    }

    internal void AddUniform(FieldInfo backingField)
    {
        uniforms.Add(new(backingField));
    }

    public ShaderUniform GetUniform(FieldInfo fieldInfo)
    {
        return uniforms.Single(u => u.BackingField == fieldInfo);
    }
}
