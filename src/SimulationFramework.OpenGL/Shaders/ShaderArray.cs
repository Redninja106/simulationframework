using SimulationFramework.Drawing.Shaders.Compiler;

namespace SimulationFramework.OpenGL;

unsafe abstract class ShaderArray
{
    public bool Synchronized { get; set; } = true;

    public abstract void WriteData(ShaderTypeLayout layout, void* data, int count);
    public abstract void ReadData(ShaderTypeLayout layout, void* outData, int count);
    public abstract void Bind(ShaderVariable uniform, UniformHandler handler);
}