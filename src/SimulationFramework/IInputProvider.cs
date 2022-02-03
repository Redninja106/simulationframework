using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides input to a simulation.
/// </summary>
public interface IInputProvider : ISimulationComponent
{
    ///// <summary>
    ///// Gets the position of the mouse.
    ///// </summary>
    ///// <returns></returns>
    //Vector2 GetMousePosition();

    ///// <summary>
    ///// Gets the change in mouse position since the previous frame.
    ///// </summary>
    ///// <returns></returns>
    //Vector2 GetMouseDelta();

    ///// <summary>
    ///// Gets the distance the scroll wheel has been scrolled since the previous frame.
    ///// </summary>
    //int GetScrollWheelDelta();

    ///// <summary>
    ///// Gets the state of the provided mouse button.
    ///// </summary>
    ///// <returns><see langword="true"/> if the mouse button is pressed, otherwise <see langword="false"/>.</returns>
    //bool IsMouseButtonDown(MouseButton button);

    ///// <summary>
    ///// Returns true if the provided button was pressed this frame.
    ///// </summary>
    //bool IsMouseButtonPressed(MouseButton button);

    ///// <summary>
    ///// Returns true if the provided button was pressed this frame.
    ///// </summary>
    //bool IsMouseButtonReleased(MouseButton button);

    ///// <summary>
    ///// Gets the state of the provided key.
    ///// </summary>
    ///// <returns><see langword="true"/> if the key is pressed, otherwise <see langword="false"/>.</returns>
    //bool IsKeyDown(Key key);

    ///// <summary>
    ///// Gets an array of all the keys pressed this frame.
    ///// </summary>
    //char[] GetChars();

    ///// <summary>
    ///// Returns true if the provided key was pressed this frame.
    ///// </summary>
    //bool IsKeyPressed(Key key);

    ///// <summary>
    ///// Returns true if the provided key was pressed this frame.
    ///// </summary>
    //bool IsKeyReleased(Key key);

    //bool SetCaptureState(bool captured);

    //event KeyEvent KeyPressed;
    //event KeyEvent KeyReleased;
}