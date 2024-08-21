using ImGuiNET;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.OpenGL.Geometry;
using System;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL.Shaders;
internal class ProgrammableShaderEffect : GeometryEffect
{
    public CanvasShader Shader { get; }
    public VertexShader? VertexShader { get; }

    private ShaderCompilation compilation;
    private ShaderCompilation? vsCompilation;
    private UniformHandler uniformHandler;
    private bool usesClipSpace;

    private Matrix3x2 transform;
    public ProgrammableShaderEffect(Matrix3x2 transform, CanvasShader shader, VertexShader? vertexShader, ProgrammableShaderProgram program) : base(program)
    {
        compilation = program.compilation;
        vsCompilation = program.vsCompilation;
        this.transform = transform;
        Shader = shader;
        VertexShader = vertexShader;
        uniformHandler = program.GetUniformHandler(shader, vertexShader);
        usesClipSpace = vsCompilation?.EntryPoint?.BackingMethod?.GetCustomAttribute<UseClipSpaceAttribute>() != null;
    }

    public unsafe override void Apply(GLCanvas canvas, Matrix4x4 projection)
    {
        Program.Use();

        if (usesClipSpace)
        {
            if (canvas.fbo == 0)
            {
                projection = Matrix4x4.Identity;
            }
            else
            {
                // we render upside down to textures
                projection = Matrix4x4.CreateScale(1, -1, 1);
            }
        }
        glUniformMatrix4fv(Program.GetUniformLocation("_vertex_transform"u8), 1, 0, (float*)&projection);

        Matrix3x2 transform = Shader.TransformMatrix * this.transform;

        if (canvas.fbo == 0)
        {
            Matrix3x2 BLtoTL = Matrix3x2.CreateScale(1, -1) * Matrix3x2.CreateTranslation(0, canvas.Target.Height);
            transform *= BLtoTL;
        }

        Matrix3x2.Invert(transform, out var inv);

        Matrix4x4 invTransform = new(
            inv.M11, inv.M12, 0, 0,
            inv.M21, inv.M22, 0, 0,
            0, 0, 1, 0,
            inv.M31, inv.M32, 0, 1
            );
        glUniformMatrix4fv(Program.GetUniformLocation("_inv_transform"u8), 1, 0, (float*)&invTransform);

        uniformHandler.Reset();

        foreach (var variable in compilation.Variables)
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                uniformHandler.SetUniform(variable, Shader);
            }
        }

        foreach (var variable in vsCompilation?.Variables ?? [])
        {
            if (variable.Kind == ShaderVariableKind.Uniform)
            {
                uniformHandler.SetUniform(variable, VertexShader!);
            }
        }
    }

    public override bool CheckStateCompatibility(ref readonly CanvasState state)
    {
        return false; // return false here cause there's no way to verify certain values have changed, such as shader uniforms
    }
}
