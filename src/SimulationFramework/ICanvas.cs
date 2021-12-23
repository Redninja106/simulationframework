using System;
using System.Collections.Generic;
using System.IO;
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

    ISurface GetSurface();

    // drawing

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color used to clear the canvas.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws a line to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point on the line.</param>
    /// <param name="y1">The y-coordinate of the first point on the line.</param>
    /// <param name="x2">The x-coordinate of the second point on the line.</param>
    /// <param name="y2">The y-coordinate of the second point on the line.</param>
    /// <param name="color">The color of the line.</param>
    void DrawLine(float x1, float y1, float x2, float y2, Color color);

    /// <summary>
    /// Draws a line to the canvas, using the current drawing settings.
    /// </summary>
    /// <param name="p1">The first point on the line.</param>
    /// <param name="p2">The second point on the line.</param>
    /// <param name="color">The color of the line.</param>
    void DrawLine(Vector2 p1, Vector2 p2, Color color);

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="position"/>.</param>
    void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    void DrawRect(Rectangle rect, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawRoundedRect(float x, float y, float width, float height, float radius, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The origin of the rectangle relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    void DrawRoundedRect(Rectangle rect, float radius, Color color);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="position"/>.</param>
    void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds into which the drawn ellipse should fit.</param>
    /// <param name="color">The color of the ellipse.</param>
    void DrawEllipse(Rectangle bounds, Color color);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawEllipse(float x, float y, float radiusX, float radiusY, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft);
    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The origin of the ellipse relative to the provided <paramref name="position"/>.</param>
    void DrawEllipse(Vector2 position, Vector2 radii, float begin, float end, Color color, Alignment alignment = Alignment.TopLeft);
    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="color">The color of the ellipse.</param>
    void DrawEllipse(Rectangle bounds, float begin, float end, Color color);

    /// <summary>
    /// Draws a surface to the canvas at (0, 0), using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="alignment">The origin of the surface relative to (0, 0).</param>
    void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="width">The width of the surface's destination rectangle.</param>
    /// <param name="height">The height of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="position">The position of the surface's destination rectangle.</param>
    /// <param name="size">The size of the surface's destination rectangle.</param>
    /// <param name="alignment">The origin of the surface relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawSurface(ISurface surface, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="source">The source bounds of the surface.</param>
    /// <param name="destination">The destination bounds of the surface.</param>
    void DrawSurface(ISurface surface, Rectangle source, Rectangle destination);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="closed">Whether the first and last point in <paramref name="polygon"/> should be treated as if they are connected.</param>
    void DrawPolygon(Span<Vector2> polygon, bool closed);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="closed">Whether the first and last point in <paramref name="polygon"/> should be treated as if they are connected.</param>
    void DrawPolygon(IEnumerable<Vector2> polygon, bool closed);

    // text rendering
    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="x">The X position of the text.</param>
    /// <param name="y">The Y position of the text.</param>
    /// <param name="color">The color of the text.</param>
    /// <param name="alignment">The origin of the surface relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void DrawText(string text, float x, float y, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text.</param>
    /// <param name="color">The color of the text.</param>
    /// <param name="alignment">The origin of the text relative to the provided <paramref name="position"/>.</param>
    void DrawText(string text, Vector2 position, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Determines the size of the provided text based on the current font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>The width and height of the provided text.</returns>
    Vector2 MeasureText(string text);

    /// <summary>
    /// Sets a font with the specified attributes as current (and loads it if it is not already loaded).
    /// </summary>
    /// <param name="fontName">The name of the system font to try to load.</param>
    /// <param name="styles">The style of the text.</param>
    /// <param name="size">The size of the font, in pixels.</param>
    /// <returns><see langword="true"/> if the font was successfully loaded, otherwise <see langword="false"/>.</returns>
    bool SetFont(string fontName, TextStyles styles, float size);

    // TODO: support for custom fonts? perhaps font objects?

    /// <summary>
    /// Frees any loaded fonts.
    /// </summary>
    void ClearFontCache();

    // configuring

    /// <summary>
    /// Sets the drawing mode of the canvas.
    /// </summary>
    /// <param name="mode">A <see cref="DrawMode"/> value.</param>
    void SetDrawMode(DrawMode mode);

    /// <summary>
    /// Sets the stroke width of the canvas. On most shapes, this value only has an effect on drawing when this canvas's <see cref="DrawMode"/> is <see cref="DrawMode.Border"/>.
    /// </summary>
    /// <param name="strokeWidth">The width, in pixels, of any line drawn by the canvas. This value must be greater than 0.</param>
    void SetStrokeWidth(float strokeWidth);

    // clipping
    /// <summary>
    /// Sets the current clipping rectangle.
    /// </summary>
    /// <param name="x">The X position of the clipping rectangle.</param>
    /// <param name="y">The Y position of the clipping rectangle.</param>
    /// <param name="width">The width of the clipping rectangle.</param>
    /// <param name="height">The height of the clipping rectangle.</param>
    /// <param name="alignment">The origin of the clipping rectangle relative to the provided <paramref name="x"/> and <paramref name="y"/> coordinates.</param>
    void SetClipRect(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Sets the current clipping rectangle.
    /// </summary>
    /// <param name="position">The position of the clipping rectangle.</param>
    /// <param name="size">The size of the clipping rectangle.</param>
    /// <param name="alignment">The origin of the clipping rectangle relative to the provided <paramref name="position"/>.</param>
    void SetClipRect(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Sets the current clipping rectangle.
    /// </summary>
    /// <param name="rect">The position and size of clipping rectangle.</param>
    void SetClipRect(Rectangle rect);

    // state save/load stack operations

    /// <summary>
    /// Pushes the current transformation matrix, clipping rectangle, and drawing state onto the stack.
    /// </summary>
    /// <returns>A <see cref="CanvasSession"/> which, when disposed, calls <see cref="Pop"/> on this canvas.</returns>
    CanvasSession Push();

    /// <summary>
    /// Pops a transformation matrix, clipping rectangle, and drawing state off the top of the stack.
    /// </summary>
    void Pop();

    /// <summary>
    /// Resets the transformation matrix, clipping rectangle, and drawing state to their defaults.
    /// </summary>
    void ResetState();

    // transformations
    /// <summary>
    /// Gets or sets the current transformation matrix.
    /// </summary>
    Matrix3x2 Transform { get; set; }

    /// <summary>
    /// Translates the current transformation matrix by the provided translation.
    /// </summary>
    /// <param name="x">The X value of the translation.</param>
    /// <param name="y">The Y value of the translation.</param>
    void Translate(float x, float y);

    /// <summary>
    /// Translates the current transformation matrix by the provided translation.
    /// </summary>
    /// <param name="translation">The value of the translation.</param>
    void Translate(Vector2 translation);

    /// <summary>
    /// Rotates the current transformation matrix center around the current translation by the provided angle.
    /// </summary>
    /// <param name="angle">The angle of the rotation, in radians.</param>
    void Rotate(float angle);

    /// <summary>
    /// Rotates the current transformation matrix around the provided point by the provided angle.
    /// </summary>
    /// <param name="angle">The angle of the rotation, in radians.</param>
    /// <param name="center">The point around which the rotation occurs.</param>
    void Rotate(float angle, Vector2 center);

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scale">The scale to transform the transformation matrix by.</param>
    void Scale(float scale);

    /// <summary>
    /// Scales the current transformation matrix by the provided values.
    /// </summary>
    /// <param name="scaleX">The scale to transform the transformation matrix by on the x-axis.</param>
    /// <param name="scaleY">The scale to transform the transformation matrix by on the y-axis.</param>
    void Scale(float scaleX, float scaleY);
}