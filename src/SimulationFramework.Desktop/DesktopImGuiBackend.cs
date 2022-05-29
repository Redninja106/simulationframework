using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.OpenGL;
using SimulationFramework.IMGUI;

namespace SimulationFramework.Desktop;

internal class WindowImGuiBackend : IImGuiNETBackend
{
    private ImGuiController imguiController;

    public WindowImGuiBackend(IWindow window)
    {
        imguiController = new ImGuiController(window.CreateOpenGL(), window, window.CreateInput());
    }

    public void Dispose()
    {

        imguiController.Dispose();
    }

    public void NewFrame()
    {
        imguiController.Update(Time.DeltaTime);
    }

    public void Render()
    {
        imguiController.Render();
    }

    public IntPtr GetTextureID(ITexture texture)
    {
        return IntPtr.Zero;
    }
}