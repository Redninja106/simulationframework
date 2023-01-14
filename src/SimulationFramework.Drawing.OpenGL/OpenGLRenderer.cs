using OpenTK.Graphics.OpenGL;
using SimulationFramework.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.OpenGL;
internal class OpenGLRenderer : IRenderer
{
    public ITexture<Color>? RenderTarget { get; set; }
    public ITexture<float>? DepthTarget { get; set; }
    public ITexture<byte>? StencilTarget { get; set; }
    public CullMode CullMode { get; set; }
    public float DepthBias { get; set; }
    public bool Wireframe { get; set; }
    public bool WriteDepth { get; set; }
    public DepthStencilComparison DepthComparison { get; set; }

    private int arrayBufferOffset, elementArrayBufferOffset; 

    public void ClearDepthTarget(float depth)
    {
        throw new NotImplementedException();
    }

    public void ClearRenderTarget(Color color)
    {
        var colorf = color.ToColorF();
        GL.ClearColor(colorf.R, colorf.G, colorf.B, colorf.A);
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public void ClearStencilTarget(byte stencil)
    {
        throw new NotImplementedException();
    }

    public void Clip(Rectangle? rectangle)
    {
        if (rectangle is not null)
        {
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor((int)rectangle.Value.X, (int)rectangle.Value.Y, (int)rectangle.Value.Width, (int)rectangle.Value.Height);
        }
        else
        {
            GL.Disable(EnableCap.ScissorTest);
        }
    }

    public void DrawPrimitives(PrimitiveKind kind, int count)
    {
        GL.DrawArrays(GLPrimitiveType(kind), arrayBufferOffset, Graphics.GetVertexCount(kind, count));
    }

    private PrimitiveType GLPrimitiveType(PrimitiveKind kind)
    {
        return kind switch
        {
            PrimitiveKind.Points => PrimitiveType.Points,
            PrimitiveKind.Triangles => PrimitiveType.Triangles,
            PrimitiveKind.TriangleStrip => PrimitiveType.TriangleStrip,
            PrimitiveKind.Lines => PrimitiveType.Lines,
            PrimitiveKind.LineStrip => PrimitiveType.LineStrip,
        };
    }

    public void DrawPrimitivesIndexed(PrimitiveKind kind, int count)
    {
        GL.DrawElements(GLPrimitiveType(kind), Graphics.GetVertexCount(kind, count), DrawElementsType.UnsignedInt, (nint)(count * 3));
    }

    public void DrawPrimitivesIndexedInstanced(PrimitiveKind kind, int count, int instanceCount)
    {
        throw new NotImplementedException();
    }

    public void DrawPrimitivesInstanced(PrimitiveKind kind, int count, int instanceCount)
    {
        throw new NotImplementedException();
    }

    public void Flush()
    {
        throw new NotImplementedException();
    }

    public void PopState()
    {
        throw new NotImplementedException();
    }

    public void PushState()
    {
        throw new NotImplementedException();
    }

    public void ResetState()
    {
    }

    public void SetFragmentShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetGeometryShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetIndexBuffer(IBuffer<uint>? indexBuffer, int offset = 0)
    {
        throw new NotImplementedException();
    }

    public void SetInstanceBuffer<T>(IBuffer<T>? instanceBuffer, int offset = 0) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void SetVertexBuffer<T>(IBuffer<T>? vertexBuffer, int offset = 0) where T : unmanaged
    {
        throw new NotImplementedException();
    }

    public void SetVertexShader(IShader? shader)
    {
        throw new NotImplementedException();
    }

    public void SetViewport(Rectangle viewport)
    {
        throw new NotImplementedException();
    }

    public void DrawGeometry(IGeometry geometry)
    {
        throw new NotImplementedException();
    }

    public void DrawGeometryInstanced(IGeometry geometry, int instanceCount)
    {
        throw new NotImplementedException();
    }
}
