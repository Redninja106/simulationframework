using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;
internal class FragmentDiscardedException : Exception
{
    public FragmentDiscardedException() : base("The shader invocation was discarded")
    {
    }
}
