using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal static class D3DExceptions
{
    public static Exception InvalidBuffer(string paramName) => new ArgumentException($"Argument '{paramName}' must a buffer created by the D3D11 graphics provider.", paramName);
}
