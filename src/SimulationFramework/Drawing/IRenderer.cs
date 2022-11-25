using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IRenderer
{
    ITexture<Color>? RenderTarget { get; set; }
    ITexture<float>? DepthTarget { get; set; }
    ITexture<byte>? StencilTarget { get; set; }

    CullMode CullMode { get; set; }
    bool Wireframe { get; set; }

    float DepthBias { get; set; }
    // DepthStencilComparison DepthComparison { get; set; }
    // bool WriteDepth { get; set; }

    void ClearRenderTarget(Color color);
    void ClearDepthTarget(float depth);
    void ClearStencilTarget(byte stencil);

    void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer) where T : unmanaged;
    void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer) where T : unmanaged;
    void SetIndexBuffer(IBuffer<uint>? indexBuffer);
    
    void SetVertexShader(IShader? shader);
    void SetGeometryShader(IShader? shader);
    void SetFragmentShader(IShader? shader);

    void DrawPrimitives(PrimitiveKind kind, int count, int vertexOffset = 0);
    void DrawPrimitivesIndexed(PrimitiveKind kind, int count, int vertexOffset = 0, int indexOffset = 0);
    void DrawPrimitivesInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int instanceOffset = 0);
    void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int count, int instanceCount, int vertexOffset = 0, int indexOffset = 0, int instanceOffset = 0);

    void SetViewport(Rectangle viewport);
    void Clip(Rectangle? rectangle);

    void PushState();
    void PopState();
    void ResetState();

    void Flush();
}

struct LineStream<T> : IPrimitiveStream<T>
    where T : unmanaged
{
    public void EmitVertex(T vertex)
    {
        throw new NotImplementedException();
    }

    public void EndPrimitive()
    {
        throw new NotImplementedException();
    }
}

public struct TriangleStream<T> : IPrimitiveStream<T>
    where T : unmanaged
{
    public void EmitVertex(T vertex)
    {
        throw new NotImplementedException();
    }

    public void EndPrimitive()
    {
        throw new NotImplementedException();
    }
}

struct PointStream<T> : IPrimitiveStream<T>
    where T : unmanaged
{
    public void EmitVertex(T vertex)
    {
        throw new NotImplementedException();
    }

    public void EndPrimitive()
    {
        throw new NotImplementedException();
    }
}

public interface IPrimitiveStream<T> 
    where T : unmanaged
{
    void EmitVertex(T vertex);
    void EndPrimitive();
}

struct MyGeoShader : IShader
{
    [ShaderInput]
    TrianglePrimitive<Vector3> input;

    [ShaderOutput, MaxVertices(3)]
    TriangleStream<Vector3> triangles;

    public void Main()
    {
        triangles.EmitVertex(input.VertexA);
        triangles.EmitVertex(input.VertexB);
        triangles.EmitVertex(input.VertexC);
        triangles.EmitVertex(input.VertexC + input.VertexB - input.VertexA);
        triangles.EndPrimitive();
    }
}

public struct TrianglePrimitive<T> where T : unmanaged { public T VertexA, VertexB, VertexC; }

public struct LinePrimitive<T> where T : unmanaged { public T VertexA, VertexB; }

class ThreadGroupSize : Attribute { public ThreadGroupSize(int width, int height, int depth) { } }
public class MaxVerticesAttribute : Attribute { public MaxVerticesAttribute(int maxVertices) { } }

public enum DepthStencilComparison
{
    Always,
    Never,
    Equal,
    NotEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
}

public enum StencilOperation
{
    None,
    Zero,
    Replace,
    Invert,
    Increment,
    Decrement,
    IncrementWrap,
    DecrementWrap,
}