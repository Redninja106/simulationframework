using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;

internal enum WindowMessage : uint
{
    /// <summary>
    /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
    /// </summary>
    Null = 0x00,
}
