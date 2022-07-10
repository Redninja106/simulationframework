using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;

/// <summary>
/// A callback function that processes messages sent to a window.
/// </summary>
/// <param name="hwnd">A handle to the window.</param>
/// <param name="msg">The message.</param>
/// <param name="wParam">Additional message information.</param>
/// <param name="lParam">Additional message information.</param>
/// <returns>The return value is the result of the message processing, and depends on the message sent.</returns>
delegate nint WindowProc(nint hwnd, WindowMessage msg, nint wParam, nint lParam);

internal static class Win32
{
    public const int CW_USEDEFAULT = unchecked((int)0x80000000);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern short RegisterClassEx(ref WindowClassEx windowClass);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern nint CreateWindowEx(WindowStylesEx stylesEx, string className, string windowName, WindowStyles styles, int x, int y, int width, int height, nint hwndParent = 0, nint menu = 0, nint instance = 0, nint lpvoid = 0);

    [DllImport("kernel32.dll")]
    public static extern nint GetModuleHandle(string module);

    [DllImport("user32.dll")] 
    public static extern bool PeekMessage(out Message lpMsg, nint hWnd, WindowMessage wMsgFilterMin, WindowMessage wMsgFilterMax, PeekMessageFlags flags);
    
    [DllImport("user32.dll")] 
    public static extern bool TranslateMessage(ref Message message);

    [DllImport("user32.dll")] 
    public static extern void DispatchMessage(ref Message message);
}   