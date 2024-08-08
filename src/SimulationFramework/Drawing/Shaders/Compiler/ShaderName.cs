namespace SimulationFramework.Drawing.Shaders.Compiler;

public struct ShaderName
{
    public string value;

    public ShaderName(string value)
    {
        this.value = value
            .Replace('.', '_');

        if (this.value.StartsWith('<'))
        {
            this.value = this.value[1..this.value.IndexOf('>')];
        }
    }

    public override string ToString()
    {
        return value;
    }
}