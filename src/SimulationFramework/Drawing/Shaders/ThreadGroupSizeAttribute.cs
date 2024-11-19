using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Drawing.Shaders;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ThreadGroupSizeAttribute : Attribute
{
    public int CountX { get; } 
    public int CountY { get; }
    public int CountZ { get; }

    public ThreadGroupSizeAttribute(int sizeX, int sizeY, int sizeZ)
    {
        this.CountX = sizeX;
        this.CountY = sizeY;
        this.CountZ = sizeZ;
    }
}
