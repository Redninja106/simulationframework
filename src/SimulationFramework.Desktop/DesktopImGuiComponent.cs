﻿using ImGuiNET;
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
        keyboardProvider.capturedByImgui = io.WantCaptureKeyboard;
        mouseProvider.capturedByImgui = io.WantCaptureMouse;
    }

    void AfterRender(AfterRenderMessage message)
    {
        var canvas = Application.GetComponent<IGraphicsProvider>().GetFrameCanvas();
        gl.Viewport(0, 0, (uint)canvas.Width, (uint)canvas.Height);
        imGuiController.Render();
    }

    public void Dispose()
    {
        gl.Dispose();
        imGuiController.Dispose();
    }
}
