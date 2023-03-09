using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11;
internal interface IBindableResource
{
    void NotifyBound(GraphicsQueueBase queue, BindingUsage usage, bool mayWrite);
    void NotifyUnbound(GraphicsQueueBase queue);
}
