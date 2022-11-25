using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Shaders.Compiler;
public class ShaderSignature
{
    public IEnumerable<(Type Type, string Name)> Fields { get; }
    public IEnumerable<Type> Types => Fields.Select(pair => pair.Type);
    public IEnumerable<string> Names => Fields.Select(pair => pair.Name);

    public int Size => Types.Sum(TypeUtilities.SizeOf);
    
    public ShaderSignature(IEnumerable<(Type Type, string Name)> fields)
    {
        Fields = fields.ToArray();
    }

    public bool IsCompatableWith(ShaderSignature other)
    {
        return Size == other.Size;
    }

    public bool GSIsCompatableWith(ShaderSignature gsInput, PrimitiveKind kind)
    {
        return gsInput.Size == (this.Size * kind switch
        {
            PrimitiveKind.Points => 1,
            PrimitiveKind.Lines or PrimitiveKind.LineStrip => 2,
            PrimitiveKind.Triangles or PrimitiveKind.TriangleStrip => 3
        });
    }
}