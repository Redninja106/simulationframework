using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

public sealed class SkiaGraphicsFactory : IGraphicsFactory
{
    private readonly ISkiaFrameProvider frameProvider;
    private readonly GRGlGetProcedureAddressDelegate glGetProcAddress;
    private bool used = false;

    public SkiaGraphicsFactory(ISkiaFrameProvider frameProvider, GRGlGetProcedureAddressDelegate glGetProcAddress)
    {
        this.frameProvider = frameProvider;
        this.glGetProcAddress = glGetProcAddress;
    }

    public IGraphicsProvider CreateGraphics()
    {
        if (used)
        {
            Debug.Warn("An instance of SkiaGraphics factory has been used twice!");
            used = true;
        }

        var result = new SkiaGraphics(glGetProcAddress, frameProvider);

        return result;
    }
}