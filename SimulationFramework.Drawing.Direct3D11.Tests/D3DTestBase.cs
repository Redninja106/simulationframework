using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Direct3D11.Tests;
public class D3DTestBase
{
    internal DeviceResources DeviceResources;

    [TestInitialize]
    public void Initialize()
    {
        DeviceResources = new(0);
    }

    [TestCleanup]
    public void Cleanup()
    {
        DeviceResources.Dispose();
    }
}
