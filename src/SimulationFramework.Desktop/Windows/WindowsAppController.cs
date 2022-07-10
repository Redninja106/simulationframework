using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows;

using Interop;

internal class WindowsAppController : IAppController
{
    private const string CLASS_NAME = "SimulationFrameworkApp";

    private nint instance;
    private nint windowHandle;
    bool isRunning;

    public bool ApplyConfig(AppConfig config)
    {
        return false;
    }

    public AppConfig CreateConfig()
    {
        return new AppConfig();
    }

    public void Dispose()
    {
    }

    public void Initialize(Application application)
    {
        this.instance = Win32.GetModuleHandle(null);

        WindowClassEx windowClass = new();
        windowClass.WndProc = WndProc;
        windowClass.className = CLASS_NAME;
        windowClass.instance = this.instance;

        var atom = Win32.RegisterClassEx(ref windowClass);
        if (atom is 0)
        {
            throw new Exception("Error registering window class!");
        }

        windowHandle = Win32.CreateWindowEx(0, CLASS_NAME, "Hello world!", WindowStyles.OverlappedWindow, Win32.CW_USEDEFAULT, Win32.CW_USEDEFAULT, 1920, 1920, instance: instance);
    }

    public void Start(MessageDispatcher dispatcher)
    {
        isRunning = true;

        dispatcher.Dispatch(new InitializeMessage());

        while (isRunning)
        {
            dispatcher.Dispatch(new FrameBeginMessage());

            ProcessMessages();

            dispatcher.Dispatch(new RenderMessage(Graphics.GetOutputCanvas()));

            dispatcher.Dispatch(new FrameEndMessage());
        }

        dispatcher.Dispatch(new UninitializeMessage());
    }

    private nint WndProc(nint hwnd, WindowMessage message, nint wParam, nint lParam)
    {
        return 0;
    }

    private void ProcessMessages()
    {
        Message message;

        while (Win32.PeekMessage(out message, this.windowHandle, 0, 0, PeekMessageFlags.PM_REMOVE))
        {
            Win32.TranslateMessage(ref message);
            Win32.DispatchMessage(ref message);
        }
    }
}
