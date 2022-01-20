using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Enables the rendering shapes to a surface.
/// </summary>
public interface ICanvas : IDisposable
{
    /// <summary>
    /// The width of the canvas, in pixels.
    /// </summary>
    int Width { get; }
    
    /// <summary>
    /// The height of the canvas, in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Waits for all drawing commands to finish executing. This is called automatically at the end of each frame and shouldn't be needed most of the time.
    /// </summary>
    void Flush();

    /// <summary>
    /// Gets the surface which this canvas is drawing to. This may be null if the canvas is drawing to the window or any other graphical output.
    /// </summary>
    ISurface GetSurface();

    // drawing

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color with which to clear the canvas.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws a line to the canvas, using the current transform, clipping, and drawing settings. To change the thickness of the line, see <see cref="SetStrokeWidth(float)"/>.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point of the line.</param>
    /// <param name="y1">The y-coordinate of the first point of the line.</param>
    /// <param name="x2">The x-coordinate of the second point of the line.</param>
    /// <param name="y2">The y-coordinate of the second point of the line.</param>
    /// <param name="color">The color of the line.</param>
    void DrawLine(float x1, float y1, float x2, float y2, Color color);

    /// <summary>
    /// Draws a line to the canvas, using the current transform, clipping, and drawing settings. To change the thickness of the line, see <see cref="SetStrokeWidth(float)"/>.
    /// </summary>
    /// <param name="p1">The first point of the line.</param>
    /// <param name="p2">The second point of the line.</param>
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
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    void DrawRect(float x, float y, float width, float height, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    void DrawRect(Vector2 position, Vector2 size, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    /// <param name="color">The color of the rectangle.</param>
    void DrawRect(Rectangle rect, Color color);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    void DrawRoundedRect(float x, float y, float width, float height, float radius, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="color">The color of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
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
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position</param>
    void DrawEllipse(float x, float y, float radiusX, float radiusY, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    void DrawEllipse(Vector2 position, Vector2 radii, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds into which the drawn ellipse should fit.</param>
    /// <param name="color">The color of the ellipse.</param>
    void DrawEllipse(Rectangle bounds, Color color);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints should connect to one other or to the center of the ellipse.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position</param>
    void DrawEllipse(float x, float y, float radiusX, float radiusY, float begin, float end, bool includeCenter, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints include its center or just connect its endpoints.</param>
    /// <param name="color">The color of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    void DrawEllipse(Vector2 position, Vector2 radii, float begin, float end, bool includeCenter, Color color, Alignment alignment = Alignment.Center);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="color">The color of the ellipse.</param>
    void DrawEllipse(Rectangle bounds, float begin, float end, bool includeCenter, Color color);

    /// <summary>
    /// Draws a surface to the canvas at (0, 0), using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="alignment">The point on the surface to align to (0, 0).</param>
    void DrawSurface(ISurface surface, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="alignment">The point on the surface to align to the provided position.</param>
    void DrawSurface(ISurface surface, float x, float y, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="x">The x-position of the surface's destination rectangle.</param>
    /// <param name="y">The y-position of the surface's destination rectangle.</param>
    /// <param name="width">The width of the surface's destination rectangle.</param>
    /// <param name="height">The height of the surface's destination rectangle.</param>
    /// <param name="alignment">The point on the surface to align to the provided position.</param>
    void DrawSurface(ISurface surface, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a surface to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="surface">The surface to draw.</param>
    /// <param name="position">The position of the surface's destination rectangle.</param>
    /// <param name="size">The size of the surface's destination rectangle.</param>
    /// <param name="alignment">The point on the surface to align to the provided position.</param>
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
    /// <param name="color">The color of the polygon.</param>
    void DrawPolygon(Span<Vector2> polygon, Color color);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="color">The color of the polygon.</param>
    void DrawPolygon(IEnumerable<Vector2> polygon, Color color);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// <para>If the current <see cref="DrawMode"/> is <see cref="DrawMode.Fill"/> or <see cref="DrawMode.Gradient"/>, the first and last vertices are connected to create a closed polygon.</para>
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="color">The color of the polygon.</param>
    void DrawPolygon(Vector2[] polygon, Color color);

    // text rendering

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="x">The X position of the text.</param>
    /// <param name="y">The Y position of the text.</param>
    /// <param name="color">The color of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    void DrawText(string text, float x, float y, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text.</param>
    /// <param name="color">The color of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    void DrawText(string text, Vector2 position, Color color, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Determines the size of the provided text based on the current font selection.
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

    // configuring

    /// <summary>
    /// Sets the drawing mode of the canvas.
    /// </summary>
    /// <param name="mode">A <see cref="DrawMode"/> value.</param>
    void SetDrawMode(DrawMode mode);

    /// <summary>
    /// Sets the stroke width of the canvas. This value only has an effect on drawing when drawing lines or when this canvas's <see cref="DrawMode"/> is <see cref="DrawMode.Border"/>.
    /// </summary>
    /// <param name="strokeWidth">The width, in pixels, of any line drawn by the canvas. This value must be greater than 0.</param>
    void SetStrokeWidth(float strokeWidth);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="fromX">The X-coordinate of the beginning point of the gradient.</param>
    /// <param name="fromY">The Y-coordinate of the beginning point of the gradient.</param>
    /// <param name="toX">The X-coordinate of the ending point of the gradient.</param>
    /// <param name="toY">The Y-coordinate of the ending point of the gradient.</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientLinear(float fromX, float fromY, float toX, float toY, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="fromX">The X-coordinate of the beginning point of the gradient.</param>
    /// <param name="fromY">The Y-coordinate of the beginning point of the gradient.</param>
    /// <param name="toX">The X-coordinate of the ending point of the gradient.</param>
    /// <param name="toY">The Y-coordinate of the ending point of the gradient.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(float fromX, float fromY, float toX, float toY, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="fromX">The X-coordinate of the beginning point of the gradient.</param>
    /// <param name="fromY">The Y-coordinate of the beginning point of the gradient.</param>
    /// <param name="toX">The X-coordinate of the ending point of the gradient.</param>
    /// <param name="toY">The Y-coordinate of the ending point of the gradient.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(float fromX, float fromY, float toX, float toY, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);
    
    /// <summary>
    /// Sets the active gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient.</param>
    /// <param name="to">The ending point of the gradient.</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientLinear(Vector2 from, Vector2 to, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient.</param>
    /// <param name="to">The ending point of the gradient.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(Vector2 from, Vector2 to, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient.</param>
    /// <param name="to">The ending point of the gradient.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(Vector2 from, Vector2 to, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient, defined relative to the shape the gradient is being used to draw.</param>
    /// <param name="to">The ending point of the gradient, defined relative to the shape the gradient is being used to draw</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientLinear(Alignment from, Alignment to, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient, defined relative to the shape the gradient is being used to draw.</param>
    /// <param name="to">The ending point of the gradient, defined relative to the shape the gradient is being used to draw</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(Alignment from, Alignment to, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a linear gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="from">The beginning point of the gradient, defined relative to the shape the gradient is being used to draw.</param>
    /// <param name="to">The ending point of the gradient, defined relative to the shape the gradient is being used to draw</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientLinear(Alignment from, Alignment to, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="x">The X-coordinate of the center of the gradient.</param>
    /// <param name="y">The Y-coordinate of the center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient"></param>
    void SetGradientRadial(float x, float y, float radius, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="x">The X-coordinate of the center of the gradient.</param>
    /// <param name="y">The Y-coordinate of the center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(float x, float y, float radius, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="x">The X-coordinate of the center of the gradient.</param>
    /// <param name="y">The Y-coordinate of the center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(float x, float y, float radius, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientRadial(Vector2 position, float radius, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Vector2 position, float radius, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Vector2 position, float radius, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient, relative to the shape the gradient is being used to draw.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientRadial(Alignment position, float radius, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient, relative to the shape the gradient is being used to draw.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Alignment position, float radius, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The center of the gradient, relative to the shape the gradient is being used to draw.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Alignment position, float radius, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The base position for the center of the gradient relative to the shape the gradient is being used to draw.</param>
    /// <param name="offset">The offset of the gradient's center from <paramref name="position"/>.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An array of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    void SetGradientRadial(Alignment position, Vector2 offset, float radius, params GradientStop[] gradient);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The base position for the center of the gradient relative to the shape the gradient is being used to draw.</param>
    /// <param name="offset">The offset of the gradient's center from <paramref name="position"/>.</param>
     /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">An <see cref="IEnumerable{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Alignment position, Vector2 offset, float radius, IEnumerable<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    /// <summary>
    /// Sets the active gradient to a radial gradient. Gradients are only used when this canvas's current <see cref="DrawMode"/> is <see cref="DrawMode.Gradient"/>.
    /// </summary>
    /// <param name="position">The base position for the center of the gradient relative to the shape the gradient is being used to draw.</param>
    /// <param name="offset">The offset of the gradient's center from <paramref name="position"/>.</param>
    /// <param name="radius">The distance from the center at which the gradient should end.</param>
    /// <param name="gradient">A <see cref="Span{GradientStop}"/> of <see cref="GradientStop"/> values which define a gradient between the provided points.</param>
    /// <param name="tileMode">The behavior of the gradient outside of its bounds.</param>
    void SetGradientRadial(Alignment position, Vector2 offset, float radius, Span<GradientStop> gradient, TileMode tileMode = TileMode.Clamp);

    // clipping

    /// <summary>
    /// Sets the current clipping rectangle.
    /// </summary>
    /// <param name="x">The X position of the clipping rectangle.</param>
    /// <param name="y">The Y position of the clipping rectangle.</param>
    /// <param name="width">The width of the clipping rectangle.</param>
    /// <param name="height">The height of the clipping rectangle.</param>
    /// <param name="alignment">The point on the clipping rect to align to the provided position.</param>
    void SetClipRect(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft);

    /// <summary>
    /// Sets the current clipping rectangle.
    /// </summary>
    /// <param name="position">The position of the clipping rectangle.</param>
    /// <param name="size">The size of the clipping rectangle.</param>
    /// <param name="alignment">The point on the clipping rect to align to the provided position.</param>
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
    /// <param name="centerX">The X coordinate of the point around which the rotation occurs.</param>
    /// <param name="centerY">The Y coordinate of the point around which the rotation occurs.</param>
    void Rotate(float angle, float centerX, float centerY);

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

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scale">The scales to transform the transformation matrix by on the X and Y axes.</param>
    void Scale(Vector2 scale);

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scaleX">The scale to transform the transformation matrix by on the x-axis.</param>
    /// <param name="scaleY">The scale to transform the transformation matrix by on the y-axis.</param>
    /// <param name="centerX">The X-coordinate of the point which to focus the scaling around.</param>
    /// <param name="centerY">The Y-coordinate of the point which to focus the scaling around.</param>
    void Scale(float scaleX, float scaleY, float centerX, float centerY);

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scale">The scale to transform the transformation matrix by on the x and y axes.</param>
    /// <param name="center">The center of the scaling.</param>
    void Scale(Vector2 scale, Vector2 center);
}