using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace SimulationFramework.Drawing.Direct3D11.Textures;
internal class SamplerProvider : D3D11Object
{
    private Dictionary<TextureSampler, ID3D11SamplerState> samplers;

    public SamplerProvider(DeviceResources deviceResources) : base(deviceResources)
    {
        samplers = new();
    }

    public ID3D11SamplerState GetSampler(TextureSampler sampler)
    {
        if (samplers.TryGetValue(sampler, out var samplerState))
        {
            return samplerState;
        }

        samplerState = CreateSamplerState(sampler);
        samplers.Add(sampler, samplerState);
        return samplerState;
    }

    private ID3D11SamplerState CreateSamplerState(TextureSampler sampler)
    {
        var desc = new SamplerDescription()
        {
            Filter = GetFilter(sampler.FilterMode),
            AddressU = GetAddressMode(sampler.TileModeX),
            AddressV = GetAddressMode(sampler.TileModeY),
            AddressW = GetAddressMode(TileMode.None),
        };

        return Resources.Device.CreateSamplerState(desc);

        static Filter GetFilter(FilterMode filterMode) => filterMode switch
        {
            FilterMode.Linear => Filter.MinMagMipLinear,
            FilterMode.Point => Filter.MinMagMipPoint,
        };

        static TextureAddressMode GetAddressMode(TileMode tileMode) => tileMode switch
        {
            TileMode.None => TextureAddressMode.Border,
            TileMode.Mirror => TextureAddressMode.Mirror,
            TileMode.Repeat => TextureAddressMode.Wrap,
            TileMode.Clamp => TextureAddressMode.Clamp,
        };
    }

    public override void Dispose()
    {
        foreach (var (_, samplerState) in samplers)
        {
            samplerState.Dispose();
        }

        base.Dispose();
    }
}