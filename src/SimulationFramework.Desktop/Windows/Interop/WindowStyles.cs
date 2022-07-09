using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;
internal enum WindowStyles
{
    /// <summary>
    /// The window has a title bar (includes the WS_BORDER style).
    /// </summary>
    Caption = 0x00C0000,
    /// <summary>
    /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
    /// </summary>
    MaximizeBox = 0x00010000,

    /// <summary>
    /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
    /// </summary>
    MinimizeBox = 0x00020000,
    
    /// <summary>
    /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
    /// </summary>
    Overlapped = 0x00000000,

    /// <summary>
    /// The window is an overlapped window. Same as the WS_TILEDWINDOW style. 
    /// </summary>
    OverlappedWindow = (Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox),

    /// <summary>
    /// The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
    /// </summary>
    SysMenu = 0x00080000,

    /// <summary>
    /// The window has a sizing border. Same as the WS_SIZEBOX style.
    /// </summary>
    ThickFrame= 0x00040000,

}
