using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;
internal interface IUnorderedAccessViewProvider
{
    ID3D11UnorderedAccessView GetUnorderedAccessView();
}
