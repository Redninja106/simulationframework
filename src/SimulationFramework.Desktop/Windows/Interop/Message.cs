using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;

internal struct Message
{
    public nint HWND;
    public uint Msg;
    public nint WParam;
    public nint LParam;
    public uint Time;
    public Point Point;
    private uint lPrivate;
}
