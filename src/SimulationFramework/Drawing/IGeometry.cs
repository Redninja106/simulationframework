using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing;

public interface IGeometry : IDisposable
{
    TVertex[] GetVertices<TVertex>() where TVertex : unmanaged;
    void SetVertices<TVertex>(TVertex[] vertices) where TVertex : unmanaged;
    uint[]? GetIndices();

    //    void AddRect(Rectangle rectangle, Matrix3x2 transform);
    //    void AddRectOutline(Rectangle rectangle, Matrix3x2 transform, float outlineWidth = 0);
    //    void AddPolygon(ReadOnlySpan<Vector2> polygon, Matrix3x2 transform);
    //    void AddCircle(Circle circle, int vertices, Matrix3x2 transform);
    //    void AddLine(Vector2 from, Vector2 to, Matrix3x2 transform);
    //    void AddRoundedRect(Rectangle rectangle, Vector2 radii, Matrix3x2 transform);
    //    void AddEllipse(Rectangle bounds, int vertices, Matrix3x2 transform);
    //    void AddTriangles(ReadOnlySpan<Vector2> triangles, Matrix3x2 transform);
    //    void AddLines(ReadOnlySpan<Vector2> lines, Matrix3x2 transform);

    //    void AddGeometry(IGeometry geometry, Matrix3x2 transform);
    //    void AddInstancedGeometry(IGeometry geometry, ReadOnlySpan<Matrix3x2> instances);
    //    void AddInstancedGeometry<TInstance>(IGeometry geometry, ReadOnlySpan<TInstance> instances);

    //    void AddTriangles<TVertex>(ReadOnlySpan<TVertex> vertices);
    //    void AddTriangles<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices);
    //    void AddInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<TInstance> instances);
    //    void AddInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices, ReadOnlySpan<TInstance> instances);

    //    void AddLines<TVertex>(ReadOnlySpan<TVertex> vertices);
    //    void AddLines<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices);
    //    void AddInstancedLines<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices, ReadOnlySpan<TInstance> instances);


    //    public TVertex[] GetCustomTriangles<TVertex>(Span<TVertex> vertices, Span<uint> indices);
    //    public TVertex[] GetCustomLines<TVertex>(Span<TVertex> vertices, Span<uint> indices);
    //    public Vector2[] GetTriangles();
    //    public Vector2[] GetLines();

}