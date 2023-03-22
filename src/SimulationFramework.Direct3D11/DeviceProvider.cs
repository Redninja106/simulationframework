using Vortice.Direct3D;
using Vortice.Direct3D11.Debug;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11;

internal class DeviceProvider : D3D11Object
{
    public ID3D11Device5 Device { get; private set; }
    public ID3D11Debug Debug { get; private set; }

    public DeviceProvider(DeviceResources deviceResources) : base(deviceResources)
    {
        var hr = D3D11.D3D11CreateDevice(null, DriverType.Hardware, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug, new[] { FeatureLevel.Level_11_0 }, out var device);

        if (hr.Failure)
            throw new Exception();

        this.Device = device!.QueryInterface<ID3D11Device5>();
        this.Debug = device!.QueryInterface<ID3D11Debug>();

        device.Dispose();
    }

    public override void Dispose()
    {
        Debug.Dispose();
        Device.Dispose();
    }
}