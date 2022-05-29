using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

public interface IAppController : IAppComponent
{
    void Start(MessageDispatcher dispatcher);
    bool ApplyConfig(AppConfig config);
    AppConfig CreateConfig();
}