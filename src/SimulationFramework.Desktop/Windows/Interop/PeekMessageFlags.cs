using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows.Interop;
internal enum PeekMessageFlags
{

	/// <summary>
	/// Messages are not removed from the queue after processing by PeekMessage.
	/// </summary>
	PM_NOREMOVE = 0x0000,

	/// <summary>
	/// Messages are removed from the queue after processing by PeekMessage.
	/// 
	/// </summary>
	PM_REMOVE = 0x0001
}