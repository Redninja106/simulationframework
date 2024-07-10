using System.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimulationFramework.Drawing.Shaders;

namespace SimulationFramework.Drawing;

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
    CanvasState State { get; }

    /// <summary>
    /// Clears the canvas.
    /// </summary>
    /// <param name="color">The color with which to clear the canvas.</param>
    void Clear(ColorF color);
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
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Fill"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided color. 
    /// </para>
    /// </summary>
    void Fill(Color color) => Fill(color.ToColorF());

    void Fill(ColorF color);

    /// <summary>
    /// Configures the canvas to fill shapes using the provided gradient.
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Gradient"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided gradient. 
    /// </para>
    /// </summary>
    void Fill(Gradient gradient)
    {
        throw new NotImplementedException("add shader");
    }

    /// <summary>
    /// Configures the canvas to outline shapes with the provided color.
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Stroke"/>, 
    /// meaning that any shapes drawn after this call will be outlined with the provided color. 
    /// </para>
    /// </summary>
    void Stroke(Color color);

    /// <summary>
    /// Configures the canvas to fill shapes using the provided shader.
    /// </summary>
    /// 
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Shader"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided shader. 
    /// </para>
    void Fill(CanvasShader shader);

    /// <summary>
    /// Sets the stroke width of the canvas.
    /// </summary>
    void StrokeWidth(float width);

    /// <summary>
    /// Sets the clipping rectangle of the canvas.
    /// </summary>
    void Clip(Rectangle? rectangle);

    /// <summary>
    /// Fills any drawn shapes with the provided texture.
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Textured"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided texture. 
    /// </para>
    /// </summary>
    sealed void Fill(ITexture texture) => Fill(texture, Matrix3x2.Identity);

    /// <summary>
    /// Fills any drawn shapes with the provided texture.
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Textured"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided texture. 
    /// </para>
    /// </summary>
    void Fill(ITexture texture, Matrix3x2 transform, TileMode tileMode = TileMode.Repeat)
    {
        throw new Exception("use a shader");
    }

    void DrawTriangles(ReadOnlySpan<Vector2> triangles);

    /// <summary>
    /// Fills any drawn shapes with the provided texture.
    /// <para>
    /// Calling this method sets the current state's <see cref="DrawMode"/> to <see cref="DrawMode.Textured"/>, 
    /// meaning that any shapes drawn after this call will be filled with the provided texture. 
    /// </para>
    /// </summary>
    sealed void Fill(ITexture texture, Matrix3x2 transform, TileMode tileModeX, TileMode tileModeY)
    {
        throw new Exception("use a shader");
    }

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
    sealed void DrawRoundedRect(float x, float y, float width, float height, float radius, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect(new(x, y, width, height, alignment), radius);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    /// <param name="alignment">The point on the rectangle to align to the provided position.</param>
    sealed void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Alignment alignment = Alignment.TopLeft) => DrawRoundedRect(new(position, size, alignment), radius);

    /// <summary>
    /// Draws a rounded rectangle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="rect">The position and size of rectangle.</param>
    /// <param name="radius">The radius of the rounded corners of the rectangle.</param>
    void DrawRoundedRect(Rectangle rect, float radius);

    /// <summary>
    /// Draws a circle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the circle.</param>
    /// <param name="y">The y-coordinate of the circle.</param>
    /// <param name="radius">The radius of the circle on the x-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the circle to align to the provided position.</param>
    sealed void DrawCircle(float x, float y, float radius, Alignment alignment = Alignment.Center) => DrawCircle(new(x, y), radius, alignment);

    /// <summary>
    /// Draws a circle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="circle">The circle to draw.</param>
    sealed void DrawCircle(Circle circle) => DrawCircle(circle.Position, circle.Radius);

    /// <summary>
    /// Draws a circle to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the circle.</param>
    /// <param name="radius">The radius of the circle on the x-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the circle to align to the provided position.</param>
    sealed void DrawCircle(Vector2 position, float radius, Alignment alignment = Alignment.Center) => DrawEllipse(position, new(radius, radius), alignment);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="x">The x-coordinate of the ellipse.</param>
    /// <param name="y">The y-coordinate of the ellipse.</param>
    /// <param name="radiusX">The radius of the ellipse on the x-axis.</param>
    /// <param name="radiusY">The radius of the ellipse on the y-axis.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position</param>
    sealed void DrawEllipse(float x, float y, float radiusX, float radiusY, Alignment alignment = Alignment.Center) => DrawEllipse(new(x, y), new(radiusX, radiusY), alignment);

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    sealed void DrawEllipse(Vector2 position, Vector2 radii, Alignment alignment = Alignment.Center) => DrawEllipse(new Rectangle(position, radii * 2, alignment));

    /// <summary>
    /// Draws an ellipse to the canvas, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds into which the drawn ellipse should fit.</param>
    sealed void DrawEllipse(Rectangle bounds) => DrawArc(bounds, 0, MathF.Tau, true);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
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
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="radii">The radii of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints include its center or just connect its endpoints.</param>
    /// <param name="alignment">The point on the bounding-box of the ellipse to align to the provided position.</param>
    sealed void DrawArc(Vector2 position, Vector2 radii, float begin, float end, bool includeCenter, Alignment alignment = Alignment.Center) => DrawArc(new(position, radii * 2, alignment), begin, end, includeCenter);

    /// <summary>
    /// Draws a segment of an ellipse to the canvas to form an arc, using the current transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="bounds">The bounds of the ellipse.</param>
    /// <param name="begin">The angle at which the ellipse segment begins.</param>
    /// <param name="end">The angle at which the ellipse segment begins.</param>
    /// <param name="includeCenter">Whether the arc's endpoints include its center or just connect its endpoints.</param>
    void DrawArc(Rectangle bounds, float begin, float end, bool includeCenter);

    /// <summary>
    /// Draws a texture to the canvas at (0, 0), using the current transform and clipping settings.
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
    sealed void DrawTexture(ITexture texture, Vector2 position, Vector2 size, Alignment alignment = Alignment.TopLeft) => DrawTexture(texture, new Rectangle(position, size, alignment));

    /// <summary>
    /// Draws a texture to the canvas using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="destination">The location to draw the texture.</param>
    sealed void DrawTexture(ITexture texture, Rectangle destination) => DrawTexture(texture, new(0, 0, texture.Width, texture.Height), destination, ColorF.White);

    /// <summary>
    /// Draws a texture to the canvas, using the current transform and clipping settings.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="source">The source bounds of the texture.</param>
    /// <param name="destination">The destination bounds of the texture.</param>
    void DrawTexture(ITexture texture, Rectangle source, Rectangle destination, ColorF tint);

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// <para>
    /// If the current <see cref="DrawMode"/> is <see cref="DrawMode.Fill"/> or <see cref="DrawMode.Gradient"/>,
    /// the first and last vertices are connected to create a closed polygon.
    /// </para>
    /// </summary>
    /// <param name="polygon">The vertices of the polygon.</param>
    /// <param name="close">Whether the first and last vertices of the polygon should be treated as connected.</param>
    sealed void DrawPolygon(IEnumerable<Vector2> polygon, bool close = true)
    {
        CollectionsHelper.EnumerableAsSpan(polygon, 0, (span, _) => DrawPolygon(span, close));
    }

    /// <summary>
    /// Draws a polygon to the canvas, using the current transform, clipping, and drawing settings.
    /// <para>
    /// If the current <see cref="DrawMode"/> is <see cref="DrawMode.Fill"/> or <see cref="DrawMode.Gradient"/>,
    /// the first and last vertices are connected to create a closed polygon.
    /// </para>
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
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="x">The X position of the text.</param>
    /// <param name="y">The Y position of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    /// <param name="origin">Determines the text's bounding box.</param>
    sealed Vector2 DrawText(string text, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawText(text, new Vector2(x, y), alignment);

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    /// <param name="origin">Determines the text's bounding box.</param>
    sealed Vector2 DrawText(string text, Vector2 position, Alignment alignment = Alignment.TopLeft)
    {
        ArgumentNullException.ThrowIfNull(text);

        return DrawText(text.AsSpan(), position, alignment);
    }

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="x">The X position of the text.</param>
    /// <param name="y">The Y position of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    /// <param name="origin">Determines the text's bounding box.</param>
    sealed Vector2 DrawText(ReadOnlySpan<char> text, float x, float y, Alignment alignment = Alignment.TopLeft) => DrawText(text, new(x, y), alignment);

    /// <summary>
    /// Draws a set of text to the screen using the current font, transform, clipping, and drawing settings.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text.</param>
    /// <param name="alignment">The point on the text's bounding box to align to the provided position.</param>
    /// <param name="origin">Determines the text's bounding box.</param>
    Vector2 DrawText(ReadOnlySpan<char> text, Vector2 position, Alignment alignment = Alignment.TopLeft);

    Vector2 DrawCodepoint(int codepoint) => DrawCodepoint(codepoint, Vector2.Zero, Alignment.BottomLeft);
    Vector2 DrawCodepoint(int codepoint, Vector2 position, Alignment alignment);

    Rectangle MeasureText(ReadOnlySpan<char> text) => State.font.MeasureText(text, State.FontSize, State.FontStyle);
    Rectangle MeasureText(string text) => State.font.MeasureText(text, State.FontSize, State.FontStyle);

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
    /// Configures the style of the current font.
    /// </summary>
    /// <param name="style">The style of the font.</param>
    void FontStyle(FontStyle style);

    /// <summary>
    /// Configures the size of the current font.
    /// </summary>
    /// <param name="size">The size of the font.</param>
    void FontSize(float size);

    /// <summary>
    /// Pushes the current transformation matrix, clipping rectangle, and drawing state onto the stack.
    /// </summary>
    void PushState();

    /// <summary>
    /// Pops a transformation matrix, clipping rectangle, and drawing state off the top of the stack.
    /// </summary>
    void PopState();

    /// <summary>
    /// Resets the transformation matrix, clipping rectangle, and drawing state to their defaults.
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
}