using SimulationFramework.Drawing.Shaders.Compiler;
using System;

namespace SimulationFramework.OpenGL;

unsafe abstract class ShaderArray : IDisposable
{
    public bool Synchronized { get; set; } = true;

    public abstract void WriteData(ShaderTypeLayout layout, void* data, int count);
    public abstract void ReadData(ShaderTypeLayout layout, void* outData, int count);
    public abstract void Bind(ShaderVariable uniform, UniformHandler handler);

    public virtual void Dispose()
    {
    }
}