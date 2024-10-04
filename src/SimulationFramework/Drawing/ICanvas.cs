using System.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimulationFramework.Drawing.Shaders;
using System.Runtime.InteropServices;

namespace SimulationFramework.Drawing;

// TODO: blending api

/// <summary>
/// Enables the rendering shapes to a texture.
/// </summary>
public interface ICanvas
{
    /// <summary>
    /// Gets the width of the canvas, in pixels.
    /// </summary>
    sealed int Width => Target.Width;

    /// <summary>
    /// Gets the height of the canvas, in pixels.
    /// </summary>
    sealed int Height => Target.Height;

    /// <summary>
    /// The texture to which this canvas is drawing to.
    /// </summary>
    ITexture Target { get; }

    /// <summary>
    /// The canvas's current state.
    /// </summary>
    ref readonly CanvasState State { get; }

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color to clear the canvas with.</param>
    void Clear(ColorF color);

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color to clear the canvas with.</param>
    sealed void Clear(Color color) => Clear(color.ToColorF());

    /// <summary>
    /// Waits for all drawing commands to finish executing.
    /// </summary>
    void Flush();

    /// <summary>
    /// Sets whether the canvas should use antialiasing when rendering or not.
    /// </summary>
    void Antialias(bool antialias);

    /// <summary>
    /// Configures the canvas to fill shapes with the provided color.
    /// </summary>
    sealed void Fill(Color color) => Fill(color.ToColorF());

    /// <summary>
    /// Configures the canvas to fill shapes with the provided color.
    /// </summary>
    void Fill(ColorF color);

    /// <summary>
    /// Configures the canvas to fill shapes using the provided shader.
    /// </summary>
    void Fill(CanvasShader shader);
    void Fill(CanvasShader shader, VertexShader vertexShader);

    /// <summary>
    /// Configures the canvas to outline shapes with the provided color.
    /// </summary>
    void Stroke(Color color) => Stroke(color.ToColorF());
    void Stroke(ColorF color);
    void Stroke(CanvasShader shader);
    void Stroke(CanvasShader shader, VertexShader vertexShader);

    /// <summary>
    /// Sets the stroke width of the canvas.
    /// </summary>
    void StrokeWidth(float width);

    /// <summary>
    /// Sets the clipping rectangle of the canvas.
    /// </summary>
    void Clip(Rectangle? rectangle);

    /// <summary>
    /// Draws a line to the canvas, using the current transform, clipping, and drawing settings. To change the thickness of the line, see <see cref="StrokeWidth(float)"/>.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point of the line.</param>
    /// <param name="y1">The y-coordinate of the first point of the line.</param>
    /// <param name="x2">The x-coordinate of the second point of the line.</param>
    /// <param name="y2">The y-coordinate of the second point of the line.</param>
    sealed void DrawLine(float x1, float y1, float x2, float y2) => DrawLine(new(x1, y1), new(x2, y2));

    /// <summary>
    /// Draws a line to the canvas, using the current transform, clipping, and drawing settings. To change the thickness of the line, see <see cref="StrokeWidth(float)"/>.
    /// </summary>
    /// <param name="p1">The first point of the line.</param>
    /// <param name="p2">The second point of the line.</param>
    void DrawLine(Vector2 p1, Vector2 p2);

    public void DrawLines<TVertex>(ReadOnlySpan<TVertex> vertices) where TVertex : unmanaged;

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRect(float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => DrawRect(new(x, y, width, height, alignment));

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRect(Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => DrawRect(new(position, size, alignment));

    /// <summary>
    /// Draws a rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The rectangle to draw.</param>
    void DrawRect(Rectangle rect);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(float x, float y, float width, float height, float radius, Alignment alignment = Alignment.TopLeft)
    {
        DrawRoundedRect(new Rectangle(x, y, width, height, alignment), new Vector2(radius, radius));
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="xRadius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="yRadius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(float x, float y, float width, float height, float xRadius, float yRadius, Alignment alignment = Alignment.TopLeft)
    {
        DrawRoundedRect(new Rectangle(x, y, width, height, alignment), new Vector2(xRadius, yRadius));
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The X position of the rectangle.</param>
    /// <param name="y">The Y position of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="radii">The x and y radii of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(float x, float y, float width, float height, Vector2 radii, Alignment alignment = Alignment.TopLeft)
    {
        DrawRoundedRect(new Rectangle(x, y, width, height, alignment), radii);
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Alignment alignment = Alignment.TopLeft) 
    {
        DrawRoundedRect(new Rectangle(position, size, alignment), new Vector2(radius));
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="radii">The x and y radii of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(Vector2 position, Vector2 size, Vector2 radii, Alignment alignment = Alignment.TopLeft)
    {
        DrawRoundedRect(new Rectangle(position, size, alignment), radii);
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    sealed void DrawRoundedRect(Rectangle rect, float radius) 
    {
        DrawRoundedRect(rect, new Vector2(radius));
    }

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="radii">The x and y radii of the rounded corners of the rectangle.</param>
    void DrawRoundedRect(Rectangle rect, Vector2 radii);

    /// <summary>
    /// Draws a circle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the circle.</param>
    /// <param name="y">The y-coordinate of the circle.</param>
    /// <param name="radius">The radius of the circle on the x-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the circle to align to the provided position.</param>
    sealed void DrawCircle(float x, float y, float radius, Alignment alignment = Alignment.Center) => DrawCircle(new(x, y), radius, alignment);

    /// <summary>
    /// Draws a circle to the canvas using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="circle">The circle to draw.</param>
    sealed void DrawCircle(Circle circle) => DrawCircle(circle.Position, circle.Radius);

    /// <summary>
    /// Draws a circle to the canvas using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the circle.</param>
    /// <param name="radius">The radius of the circle on the x-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the circle to align to the provided position.</param>
    sealed void DrawCircle(Vector2 position, float radius, Alignment alignment = Alignment.Center) => DrawEllipse(position, new(radius, radius), alignment);

    /// <summary>
    /// Draws an ellipse to the canvas using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position</param>
    sealed void DrawEllipse(float x, float y, float radiusX, float radiusY, Alignment alignment = Alignment.Center) => DrawEllipse(new(x, y), new(radiusX, radiusY), alignment);

    /// <summary>
    /// Draws an ellipse to the canvas using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    sealed void DrawEllipse(Vector2 position, Vector2 radii, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position, radii * 2, alignment));

    /// <summary>
    /// Draws an ellipse to the canvas using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds into which the drawn ellipse should fit.</param>
    void DrawEllipse(Rectangle bounds);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints should connect to one other or to the center of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position</param>
    sealed void DrawArc(float x, float y, float radiusX, float radiusY, float begin, float end, bool includeCenter, Alignment alignment = Alignment.Center) => DrawArc(new(x, y), new(radiusX, radiusY), begin, end, includeCenter, alignment);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints include its center or just connect its endpoints.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    sealed void DrawArc(Vector2 position, Vector2 radii, float begin, float end, bool includeCenter, Alignment alignment = Alignment.Center) => DrawArc(new(position, radii * 2, alignment), begin, end, includeCenter);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints include its center or just connect its endpoints.</param>
    void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter);

    /// <summary>
    /// Draws a list of triangles using the current transform, clipping, and drawing settings
    /// </summary>
    void DrawTriangles(ReadOnlySpan<Vector2> triangles);

    /// <summary>
    /// Draws a list of triangles using a custom vertex type.
    /// <para>
    /// This method requires a custom vertex shader be set that accepts vertices of type <typeparamref name="TVertex"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TVertex">The custom vertex type.</typeparam>
    /// <param name="vertices">The list of triangles to draw. Every three vertices make a triangle.</param>
    void DrawTriangles<TVertex>(ReadOnlySpan<TVertex> vertices)
        where TVertex : unmanaged;
    void DrawTriangles<TVertex>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices)
        where TVertex : unmanaged;
    void DrawInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<TInstance> instances)
        where TVertex : unmanaged
        where TInstance : unmanaged;
    void DrawInstancedTriangles<TVertex, TInstance>(ReadOnlySpan<TVertex> vertices, ReadOnlySpan<uint> indices, ReadOnlySpan<TInstance> instances)
        where TVertex : unmanaged
        where TInstance : unmanaged;

    /// <summary>
    /// Draws a texture to the canvas at (0, 0) using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="alignment">The point on the texture to align to (0, 0).</param>
    sealed void DrawTexture(ITexture texture, Alignment alignment = Alignment.TopLeft) => DrawTexture(texture, 0, 0, alignment);

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="x">The x-position of the texture.</param>
    /// <param name="y">The y-position of the texture.</param>
    /// <param name="alignment">The point on the texture to align to the provided position.</param>
    sealed void DrawTexture(ITexture texture, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawTexture(texture, new Vector2(x, y), alignment);

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="position">The position of the texture's destination rectangle.</param>
    /// <param name="alignment">The point on the texture to align to the provided position.</param>
    sealed void DrawTexture(ITexture texture, Vector2 position, Alignment alignment = Alignment.TopLeft) => DrawTexture(texture, position, new(texture.Width, texture.Height), alignment);

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="x">The x-position of the texture.</param>
    /// <param name="y">The y-position of the texture.</param>
    /// <param name="width">The width of the texture destination.</param>
    /// <param name="height">The height of the texture destination.</param>
    /// <param name="alignment">The point on the texture to align to the provided position.</param>
    sealed void DrawTexture(ITexture texture, float x, float y, float width, float height, Alignment alignment = Alignment.TopLeft) => DrawTexture(texture, new Vector2(x, y), new Vector2(width, height), alignment);

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="position">The position of the texture's destination rectangle.</param>
    /// <param name="size">The size of the texture's destination rectangle.</param>
    /// <param name="alignment">The point on the texture to align to the provided position.</param>
    sealed void DrawTexture(ITexture texture, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft)
    {
        Rectangle source = new(0, 0, texture.Width, texture.Height);
        Rectangle destination = new(position, size, alignment);
        DrawTexture(texture, source, destination, ColorF.White);
    }

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="destination">The location to draw the texture.</param>
    sealed void DrawTexture(ITexture texture, Rectangle destination)
    {
        Rectangle source = new(0, 0, texture.Width, texture.Height);
        DrawTexture(texture, source, destination, ColorF.White);
    }

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="destination">The location to draw the texture.</param>
    /// <param name="tint">The tint to use when drawing the texture. Use <see cref="ColorF.White"/> for no tint.</param>
    sealed void DrawTexture(ITexture texture, Rectangle destination, ColorF tint)
    {
        Rectangle source = new(0, 0, texture.Width, texture.Height);
        DrawTexture(texture, source, destination, tint);
    }

    /// <summary>
    /// Draws part of a texture to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="source">The source bounds of the texture.</param>
    /// <param name="destination">The destination bounds of the texture.</param>
    sealed void DrawTexture(ITexture texture, Rectangle source, Rectangle destination) => DrawTexture(texture, source, destination, ColorF.White);

    /// <summary>
    /// Draws part of a texture to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="source">The source region of the texture.</param>
    /// <param name="destination">The destination bounds of the texture.</param>
    /// <param name="tint">The tint to use when drawing the texture. Use <see cref="ColorF.White"/> for no tint.</param>
    void DrawTexture(ITexture texture, Rectangle source, Rectangle destination, ColorF tint);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="close">Whether the first and last vertices of the polygon should be treated as connected.</param>
    sealed void DrawPolygon(IEnumerable<Vector2> polygon, bool close = true)
    {
        CollectionsHelper.EnumerableAsSpan(polygon, close, (span, close) => DrawPolygon(span, close));
    }

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="close">Whether the first and last vertices of the polygon should be treated as connected.</param>
    sealed void DrawPolygon(Vector2[] polygon, bool close = true) => DrawPolygon(polygon.AsSpan(), close);
    
    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="close">Whether the first and last vertices of the polygon should be treated as connected.</param>
    void DrawPolygon(ReadOnlySpan<Vector2> polygon, bool close = true);

    /// <summary>
    /// Draws an <see cref="IGeometry"/> instance to the canvas.
    /// </summary>
    /// <param name="geometry">The <see cref="IGeometry"/> to draw.</param>
    void DrawGeometry(IGeometry geometry);

    /// <summary>
    /// Draws multiple instances of an <see cref="IGeometry"/> to the canvas.
    /// </summary>
    /// <param name="geometry">The <see cref="IGeometry"/> to draw.</param>
    /// <param name="instances">A span of <see cref="Matrix3x2"/> values specifying the instances to draw.</param>
    void DrawInstancedGeometry(IGeometry geometry, ReadOnlySpan<Matrix3x2> instances);

    /// <summary>
    /// Draws multiple instances of an <see cref="IGeometry"/> to the canvas.
    /// </summary>
    /// <param name="geometry">The <see cref="IGeometry"/> to draw.</param>
    /// <param name="instances">A span of <typeparamref name="TInstance"/> values specifying the instances to draw. 
    /// <para>
    /// There must be a custom vertex shader active that accepts instances data of <typeparamref name="TInstance"/>.
    /// </para>
    /// </param>
    void DrawInstancedGeometry<TInstance>(IGeometry geometry, ReadOnlySpan<TInstance> instances)
        where TInstance : unmanaged;

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="size">The size of the text.</param>
    /// <param name="baselineX">The X position of the text.</param>
    /// <param name="baselineY">The Y position of the text.</param>
    /// <param name="style">The style of the text.</param>
    sealed Vector2 DrawText(string text, float size, float baselineX, float baselineY, TextStyle style = TextStyle.Regular) => DrawText(text, size, new Vector2(baselineX, baselineY), style);

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="size">The size of the text.</param>
    /// <param name="baseline">The baseline for drawing the text. </param>
    /// <param name="style">The style of the text.</param>
    sealed Vector2 DrawText(string text, float size, Vector2 baseline, TextStyle style = TextStyle.Regular)
    {
        ArgumentNullException.ThrowIfNull(text);

        return DrawText(text.AsSpan(), size, baseline, style);
    }

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="size">The size of the text.</param>
    /// <param name="baselineX">The X position of the text.</param>
    /// <param name="baselineY">The Y position of the text.</param>
    /// <param name="style">The style of the text.</param>
    sealed Vector2 DrawText(ReadOnlySpan<char> text, float size, float baselineX, float baselineY, TextStyle style = TextStyle.Regular)
    {
        return DrawText(text, size, new Vector2(baselineX, baselineY));
    }

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="size">The size of the text.</param>
    /// <param name="baseline">The baseline of the text.</param>
    /// <param name="style">The style of the text.</param>
    Vector2 DrawText(ReadOnlySpan<char> text, float size, Vector2 baseline, TextStyle style = TextStyle.Regular);

    sealed void DrawAlignedText(string text, float size, float x, float y, Alignment alignment, TextStyle style = TextStyle.Regular)
    {
        ArgumentNullException.ThrowIfNull(text);
        DrawAlignedText(text.AsSpan(), size, new Vector2(x, y), alignment, style);
    }

    sealed void DrawAlignedText(string text, float size, Vector2 position, Alignment alignment, TextStyle style = TextStyle.Regular)
    {
        ArgumentNullException.ThrowIfNull(text);
        DrawAlignedText(text.AsSpan(), size, position, alignment, style);
    }

    sealed void DrawAlignedText(ReadOnlySpan<char> text, float size, float x, float y, Alignment alignment, TextStyle style = TextStyle.Regular)
    {
        DrawAlignedText(text, size, new Vector2(x, y), alignment, style);
    }

    sealed void DrawAlignedText(ReadOnlySpan<char> text, float size, Vector2 position, Alignment alignment, TextStyle style = TextStyle.Regular)
    {
        var bounds = MeasureText(text, size);
        Rectangle destination = new(position - bounds.Position, bounds.Size, alignment);
        DrawText(text, size, destination.Position, style);
    }

    Vector2 DrawCodepoint(int codepoint, float size, Vector2 baseline, TextStyle style = TextStyle.Regular);

    Rectangle MeasureText(ReadOnlySpan<char> text, float size, TextStyle style = TextStyle.Regular) => State.Font.MeasureText(text, size, style);
    Rectangle MeasureText(string text, float size, TextStyle style = TextStyle.Regular) => State.Font.MeasureText(text, size, style); 

    /// <summary>
    /// Sets a font with the specified attributes as current (and loads it if it is not already loaded).
    /// </summary>
    /// <param name="name">The name of the font to load.</param>
    /// <returns><see langword="true"/> if the font was successfully loaded, otherwise <see langword="false"/>.</returns>
    sealed void Font(string name) => Font(Graphics.LoadSystemFont(name));

    /// <summary>
    /// Sets the font used for text rendering.
    /// </summary>
    /// <param name="font">The font to use when drawing text.</param>
    void Font(IFont font);

    /// <summary>
    /// Pushes the a copy of the current <see cref="CanvasState"/> onto the top of the canvas's internal state stack.
    /// </summary>
    void PushState();

    /// <summary>
    /// Pops the current <see cref="CanvasState"/> off the top of the canvas's internal state stack.
    /// </summary>
    void PopState();

    /// <summary>
    /// Resets the current <see cref="CanvasState"/> to its default values.
    /// </summary>
    void ResetState();

    /// <summary>
    /// Sets the canvas' transformation matrix.
    /// </summary>
    void SetTransform(Matrix3x2 transform);

    /// <summary>
    /// Composes the provided transformation with the canvas' current transform.
    /// </summary>
    sealed void Transform(Matrix3x2 transformation) => SetTransform(transformation * State.Transform);

    /// <summary>
    /// Translates the current transformation matrix by the provided translation.
    /// </summary>
    /// <param name="x">The X value of the translation.</param>
    /// <param name="y">The Y value of the translation.</param>
    sealed void Translate(float x, float y) => Translate(new(x, y));

    /// <summary>
    /// Translates the current transformation matrix by the provided translation.
    /// </summary>
    /// <param name="translation">The value of the translation.</param>
    sealed void Translate(Vector2 translation) => Transform(Matrix3x2.CreateTranslation(translation));

    /// <summary>
    /// Rotates the current transformation matrix center around the current translation by the provided angle.
    /// </summary>
    /// <param name="angle">The angle of the rotation, in radians.</param>
    sealed void Rotate(float angle) => Rotate(angle, Vector2.Zero);

    /// <summary>
    /// Rotates the current transformation matrix around the provided point by the provided angle.
    /// </summary>
    /// <param name="angle">The angle of the rotation, in radians.</param>
    /// <param name="centerX">The X coordinate of the point around which the rotation occurs.</param>
    /// <param name="centerY">The Y coordinate of the point around which the rotation occurs.</param>
    sealed void Rotate(float angle, float centerX, float centerY) => Rotate(angle, new(centerX, centerY));

    /// <summary>
    /// Rotates the current transformation matrix around the provided point by the provided angle.
    /// </summary>
    /// <param name="angle">The angle of the rotation, in radians.</param>
    /// <param name="center">The point around which the rotation occurs.</param>
    sealed void Rotate(float angle, Vector2 center) => Transform(Matrix3x2.CreateRotation(angle, center));

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scale">The scale to transform the transformation matrix by.</param>
    sealed void Scale(float scale) => Scale(scale, scale);

    /// <summary>
    /// Scales the current transformation matrix by the provided values.
    /// </summary>
    /// <param name="scaleX">The scale to transform the transformation matrix by on the x-axis.</param>
    /// <param name="scaleY">The scale to transform the transformation matrix by on the y-axis.</param>
    sealed void Scale(float scaleX, float scaleY) => Scale(new Vector2(scaleX, scaleY));

    /// <summary>
    /// Scales the current transformation matrix by the provided value.
    /// </summary>
    /// <param name="scale">The scales to transform the transformation matrix by on the X and Y axes.</param>
    sealed void Scale(Vector2 scale) => Transform(Matrix3x2.CreateScale(scale));

    void Mask(IMask? mask);
    void WriteMask(IMask? mask, bool value = true);
}