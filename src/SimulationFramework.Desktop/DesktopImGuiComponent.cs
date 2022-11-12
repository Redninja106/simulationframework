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
internal class DesktopImGuiComponent : IApplicationComponent
{
    GL gl;
    ImGuiController imGuiController;


    public DesktopImGuiComponent(IWindow window)
    {
        var inputComponent = Application.Current.GetComponent<DesktopInputComponent>();
        gl = window.CreateOpenGL();// GL.GetApi(window.GLContext);
        imGuiController = new(gl, window, inputComponent.silkInputDevice);
    }

    public void Initialize(Application application)
    {
        application.Dispatcher.Subscribe<RenderMessage>(PreRender, ListenerPriority.High);
        application.Dispatcher.Subscribe<RenderMessage>(PostRender, ListenerPriority.Low);
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
