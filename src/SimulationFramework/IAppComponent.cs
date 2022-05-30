using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IAppComponent : IDisposable
{
    void Initialize(Application application);
}