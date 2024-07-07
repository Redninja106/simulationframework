namespace SimulationFramework.Drawing.Shaders.Compiler;

public struct ShaderName
{
    public string value;

    public ShaderName(string value)
    {
        this.value = value
            .Replace('.', '_');
    }

    public override string ToString()
    {
        return value;
    }
}