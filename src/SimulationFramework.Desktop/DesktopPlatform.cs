using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Windowing;
using SimulationFramework.SkiaSharp;
using ImGuiNET;
using SimulationFramework.IMGUI;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using SimulationFramework.Desktop.Windows;

namespace SimulationFramework.Desktop;

/// <summary>
/// Implements a simulation environment which runs the simulation in a window.
/// </summary>
public abstract class DesktopAppPlatform : IAppPlatform
{

    public abstract IAppController CreateController();
    public abstract void Initialize(Application application);
    
    public static DesktopAppPlatform CreateForCurrentPlatform()
    {
        if (OperatingSystem.IsWindows())
        {
            return new WindowsAppPlatform();
        }
        
        throw new NotSupportedException("The the SimulationFramework.Desktop project does not support this platform!");
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}