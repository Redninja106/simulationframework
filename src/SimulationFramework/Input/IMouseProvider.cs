using SimulationFramework.Components;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SimulationFramework.Input;

/// <summary>
/// Provides mouse input to the simulation.
/// </summary>
public interface IMouseProvider : ISimulationComponent
{
    /// <summary>
    /// Invoked when a button is pressed.
    /// </summary>
    event MouseButtonEvent ButtonPressed;

    /// <summary>
    /// Invoked when a button is released
    /// </summary>
    event MouseButtonEvent ButtonReleased;

    /// <summary>
    /// Gets or sets the mouse position in pixels relative to the origin of the window's contents.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// The distance the mouse position has moved since the last frame, in pixels.
    /// </summary>
    Vector2 DeltaPosition { get; }
    
    /// <summary>
    /// The scroll wheel movement since the last frame.
    /// </summary>
    float ScrollWheelDelta { get; }

    /// <summary>
    /// Whether the mouse cursor is visible.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets a collection of all buttons which are held this frame.
    /// </summary>
    IEnumerable<MouseButton> HeldButtons { get; }

    /// <summary>
    /// Gets a collection of all buttons pressed this frame. A key is only considered pressed on the first frame that it is held.
    /// </summary>
    IEnumerable<MouseButton> PressedButtons { get; }

    /// <summary>
    /// Gets a collection of all buttons released this frame. A key is only considered released on the first frame that it is not held.
    /// </summary>
    IEnumerable<MouseButton> ReleasedButtons { get; }

    /// <summary>
    /// Sets the mouse cursor to a custom image.
    /// </summary>
    void SetCursor(int width, int height, ReadOnlySpan<Color> colors, int anchorX, int anchorY);

    /// <summary>
    /// Sets the mouse cursor to a system cursor.
    /// </summary>
    void SetCursor(SystemCursor cursor);
}
