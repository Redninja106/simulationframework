using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;

internal struct WindowClassEx
{
    public uint size = (uint)Unsafe.SizeOf<WindowClassEx>();
    public WindowClassStyles style;
    public WindowProc WndProc;
    public int clsExtra;
    public int wndExtra;
    public nint instance;
    public nint icon;
    public nint cursor;
    public nint background;
    public string menuName;
    public string className;
    public nint smallIcon;

    public WindowClassEx()
    {
        style = 0;
        WndProc = null;
        clsExtra = wndExtra = 0;
        instance = icon = cursor = background = 0;
        menuName = className = null;
        smallIcon = 0;
    }
}
