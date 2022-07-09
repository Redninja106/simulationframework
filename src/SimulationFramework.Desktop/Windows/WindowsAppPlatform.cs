using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop.Windows;
internal class WindowsAppPlatform : DesktopAppPlatform
{
    public override IAppController CreateController()
    {
        return new WindowsAppController();
    }

    public override void Initialize(Application application)
    {
        application.AddComponent(new RealtimeProvider());
        application.AddComponent(new WindowsInputComponent());
    }
}
