using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;

namespace SimulationFramework.Desktop;
internal class DesktopImGuiComponent : ISimulationComponent
{
    GL gl;
    ImGuiController imGuiController;

    private DesktopMouseProvider mouseProvider;
    private DesktopKeyboardProvider keyboardProvider;

    public DesktopImGuiComponent(IWindow window, IInputContext input)
    {
        gl = window.CreateOpenGL();
        imGuiController = new(gl, window, input);

        // we expect the mouse & keyboard providers to be registered already
        mouseProvider = Application.GetComponent<DesktopMouseProvider>();
        keyboardProvider = Application.GetComponent<DesktopKeyboardProvider>();

        var io = ImGui.GetIO();

        foreach (ImGuiKey key in Enum.GetValues<ImGuiKey>())
        {
            if (Enum.TryParse(key.ToString(), true, out SilkKey silkKey))
            {
                io.KeyMap[(int)silkKey] = (int)key;
            }
        }

        io.KeyMap[(int)SilkKey.ShiftLeft] = (int)ImGuiKey.LeftShift;
        io.KeyMap[(int)SilkKey.ShiftRight] = (int)ImGuiKey.RightShift;
        io.KeyMap[(int)SilkKey.AltLeft] = (int)ImGuiKey.LeftAlt;
        io.KeyMap[(int)SilkKey.AltRight] = (int)ImGuiKey.RightAlt;
        io.KeyMap[(int)SilkKey.ControlLeft] = (int)ImGuiKey.LeftCtrl;
        io.KeyMap[(int)SilkKey.ControlRight] = (int)ImGuiKey.RightCtrl;
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<BeforeRenderMessage>(PreRender);
        dispatcher.Subscribe<AfterRenderMessage>(AfterRender);
    }

    void PreRender(BeforeRenderMessage message)
    {
        imGuiController.Update(Time.DeltaTime);

        var io = ImGui.GetIO();
        keyboardProvider.CapturedByImgui = io.WantCaptureKeyboard;
        mouseProvider.CapturedByImgui = io.WantCaptureMouse;
    }

    void AfterRender(AfterRenderMessage message)
    {
        var canvas = Application.GetComponent<IGraphicsProvider>().GetWindowCanvas();
        gl.Viewport(0, 0, (uint)canvas.Width, (uint)canvas.Height);
        gl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        imGuiController.Render();
    }

    public void Dispose()
    {
        gl.Dispose();
        imGuiController.Dispose();
    }
}
