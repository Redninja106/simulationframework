using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal interface IApplicationComponentProvider<out T>
{
    static abstract T CreateComponent(Application application);
}
