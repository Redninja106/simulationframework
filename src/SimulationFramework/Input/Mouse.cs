using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SimulationFramework.Input;

/// <summary>
/// Provides mouse input to the application.
/// </summary>
public static class Mouse
{
    private static IMouseProvider Provider => Application.GetComponent<IMouseProvider>();

    /// <summary>
    /// The position of the mouse.
    /// </summary>
    public static Vector2 Position 
    { 
        get => Provider.Position; 
        set => Provider.Position = value;
    }

    /// <summary>
    /// The distance the mouse has moved since the last frame.
    /// <para>
    /// Note: this value may not be accurate if the window was moved or resized.
    /// </para>
    /// </summary>
    public static Vector2 DeltaPosition => Provider.DeltaPosition;

    /// <summary>
    /// The distance the scroll whell has moved since the last frame.
    /// </summary>
    public static float ScrollWheelDelta => Provider.ScrollWheelDelta;

    /// <summary>
    /// Whether the cursor is visible or not.
    /// </summary>
    public static bool Visible
    {
        get => Provider.Visible;
        set => Provider.Visible = value;
    }

    /// <summary>
    /// Fired when the user presses a mouse button.
    /// </summary>
    public static event MouseButtonEvent ButtonDown
    {
        add => Provider.ButtonPressed += value;
        remove => Provider.ButtonPressed -= value;
    }

    /// <summary>
    /// Fired when the user released a mouse button.
    /// </summary>
    public static event MouseButtonEvent ButtonUp
    {
        add => Provider.ButtonReleased += value;
        remove => Provider.ButtonReleased -= value;
    }

    /// <summary>
    /// Gets a collection of all buttons which are held this frame.
    /// </summary>
    public static IEnumerable<MouseButton> HeldButtons => Provider.HeldButtons;

    /// <summary>
    /// Gets a collection of all buttons pressed this frame. A key is only considered pressed on the first frame that it is held.
    /// </summary>
    public static IEnumerable<MouseButton> PressedButtons => Provider.PressedButtons;

    /// <summary>
    /// Gets a collection of all buttons released this frame. A key is only considered released on the first frame that it is not held.
    /// </summary>
    public static IEnumerable<MouseButton> ReleasedButtons => Provider.ReleasedButtons;

    /// <summary>
    /// Returns <see langword="true"/> if the provided button is down; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonDown(MouseButton button) => Provider.HeldButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just pressed this frame; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonPressed(MouseButton button) => Provider.PressedButtons.Contains(button);

    /// <summary>
    /// Returns <see langword="true"/> if the provided button was just released this frame; otherwise <see langword="false"/>.
    /// </summary>
    public static bool IsButtonReleased(MouseButton button) => Provider.ReleasedButtons.Contains(button);

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="colors">The image to set the mouse cursor to.</param>
    /// <param name="anchorX">The X position of the cursor's center, relative to the top-left corner of the image.</param>
    /// <param name="anchorY">The Y position of the cursor's center, relative to the top-left corner of the image.</param>
    public static unsafe void SetCursor(Color[,] colors, int anchorX = 0, int anchorY = 0)
    {
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        
        fixed (Color* colorsPtr = &colors[0, 0])
        {
            SetCursor(new ReadOnlySpan<Color>(colorsPtr, width * height), width, height, anchorX, anchorY);
        }
    }

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="colors">The image to set the mouse cursor to.</param>
    /// <param name="anchor">The location of the cursor's center.</param>
    public static unsafe void SetCursor(Color[,] colors, Alignment anchor)
    {
        var bounds = new Rectangle(0, 0, colors.GetLength(0), colors.GetLength(1));
        var anchorPoint = bounds.GetAlignedPoint(anchor);
        SetCursor(colors, (int)anchorPoint.X, (int)anchorPoint.Y);
    }

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="texture">The image to set the mouse cursor to.</param>
    /// <param name="anchorX">The X position of the cursor's center, relative to the top-left corner of the image.</param>
    /// <param name="anchorY">The Y position of the cursor's center, relative to the top-left corner of the image.</param>
    public static void SetCursor(ITexture texture, int anchorX = 0, int anchorY = 0)
    {
        SetCursor(texture.Pixels, texture.Width, texture.Height, anchorX, anchorY);
    }

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="texture">The image to set the mouse cursor to.</param>
    /// <param name="anchor">The location of the cursor's center.</param>
    public static void SetCursor(ITexture texture, Alignment anchor)
    {
        var bounds = new Rectangle(0, 0, texture.Width, texture.Height);
        var anchorPoint = bounds.GetAlignedPoint(anchor);
        SetCursor(texture, (int)anchorPoint.X, (int)anchorPoint.Y);
    }
    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="colors">A <see cref="ReadOnlySpan{T}"/> of <see cref="Color"/> containing the cursor image data. Must be of size <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="width">The width of the cursor image, in pixels.</param>
    /// <param name="height">The height of the cursor image, in pixels.</param>
    /// <param name="anchor">The location of the cursor's center.</param>
    public static void SetCursor(ReadOnlySpan<Color> colors, int width, int height, Alignment anchor)
    {
        var bounds = new Rectangle(0, 0, width, height);
        var anchorPoint = bounds.GetAlignedPoint(anchor);
        Provider.SetCursor(colors, width, height, (int)anchorPoint.X, (int)anchorPoint.Y);
    }

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    /// <param name="colors">A <see cref="ReadOnlySpan{T}"/> of <see cref="Color"/> containing the cursor image data. Must be of size <paramref name="width"/> * <paramref name="height"/>.</param>
    /// <param name="width">The width of the cursor image, in pixels.</param>
    /// <param name="height">The height of the cursor image, in pixels.</param>
    /// <param name="anchorX">The X position of the cursor's center, relative to the top-left corner of the image.</param>
    /// <param name="anchorY">The Y position of the cursor's center, relative to the top-left corner of the image.</param>
    public static void SetCursor(ReadOnlySpan<Color> colors, int width, int height, int anchorX = 0, int anchorY = 0)
    {
        Provider.SetCursor(colors, width, height, anchorX, anchorY);
    }

    /// <summary>
    /// Sets the mouse cursor to a system cursor.
    /// </summary>
    /// <param name="cursor">The system cursor to set the mouse cursor to.</param>
    public static void SetCursor(SystemCursor cursor)
    {
        Provider.SetCursor(cursor);
    }
}