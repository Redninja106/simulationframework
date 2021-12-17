using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface ICanvas : IDisposable
{
    /// <summary>
    /// Waits for all drawing commands to finish executing.
    /// </summary>
    void Flush();

    // drawing

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color used to clear the canvas.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws a line to the canvas.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point on the line.</param>
    /// <param name="y1">The y-coordinate of the first point on the line.</param>
    /// <param name="x2">The x-coordinate of the second point on the line.</param>
    /// <param name="y2">The y-coordinate of the second point on the line.</param>
    /// <param name="color">The color of the line.</param>
    void DrawLine(float x1, float y1, float x2, float y2, Color color);

    /// <summary>
    /// Draws a line to the canvas.
    /// </summary>
    /// <param name="p1">The first point on the line.</param>
    /// <param name="p2">The second point on the line.</param>
    /// <param name="color">The color of the line.</param>
    void DrawLine(Vector2 p1, Vector2 p2, Color color);

    /// <summary>
    /// Draws a rectangle to the canvas.
    /// </summary>
    /// <param name="x">The x-coordinate of the rectangle.</param>
    /// <param name="y">The y-coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="position"/>.</param>
    void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    void DrawRect(Rectangle rect, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws an ellipse to the canvas.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="position"/>.</param>
    void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas.
    /// </summary>
    /// <param name="bounds">The bounds into which the drawn ellipse should fit.</param>
    /// <param name="color">The color of the ellipse.</param>
    void DrawEllipse(Rectangle bounds, Color color);

    /// <summary>
    /// Draws a surface to the canvas at (0, 0).
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="alignment">The origin of the surface relative to (0, 0).</param>
    void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative.<paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="width">The width of the surface's destination rectangle.</param>
    /// <param name="height">The height of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative.<paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft);
    
    /// <summary>
    /// Draws a surface to the canvas.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="position">The position of the surface's destination rectangle.</param>
    /// <param name="size">The size of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative.<paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="source">The source bounds of the surface.</param>
    /// <param name="destination">he destination bounds of the surface.</param>
    void DrawSurface(ISurface surface, Rectangle source, Rectangle destination);

    // configuring
    void Mode(DrawMode mode);

    // state caching
    CanvasSession Push();
    void Pop();

    // transformations
    Matrix3x2 Transform { get; set; }
    void Translate(float x, float y);
    void Rotate(float angle);
    void Scale(float scaleX, float scaleY);
}