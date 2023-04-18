using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop;
internal class DesktopImGuiComponent : ISimulationComponent
{
    GL gl;
    ImGuiController imGuiController;

    public DesktopImGuiComponent(IWindow window)
    {
        var inputComponent = Application.GetComponent<DesktopInputComponent>();
        gl = window.CreateOpenGL();
        imGuiController = new(gl, window, inputComponent.silkInputContext);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        dispatcher.Subscribe<RenderMessage>(PreRender, ListenerPriority.High);
        dispatcher.Subscribe<RenderMessage>(PostRender, ListenerPriority.Low);
    }

    void PreRender(RenderMessage renderMessage)
    {
        gl.Viewport(0, 0, (uint)renderMessage.Canvas.Width, (uint)renderMessage.Canvas.Height);
        imGuiController.Update(Time.DeltaTime);
    }

    void PostRender(RenderMessage renderMessage)
    {
        imGuiController.Render();
    }

    public void Dispose()
    {
        gl.Dispose();
        imGuiController.Dispose();
    }
}
