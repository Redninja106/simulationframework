using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.SkiaSharp.Shaders;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;
internal static class RuntimeEffectCache
{
    private static Dictionary<Type, (ShaderCompilation compilation, SKRuntimeEffect effect, SKRuntimeEffectUniforms uniforms)> runtimeEffectCache = new();
    private static CanvasShaderCompiler compiler = new CanvasShaderCompiler();

    public static (SKRuntimeEffect, SKRuntimeEffectUniforms) GetValue(CanvasShader shader)
    {
        if (!runtimeEffectCache.TryGetValue(shader.GetType(), out var effect))
        {
            effect.compilation = compiler.Compile(shader);
            var codeGenerator = new SKSLCodeGenerator(effect.compilation);

            StringWriter writer = new();
            codeGenerator.Emit(writer);
            var source = writer.ToString();
            Console.WriteLine(source);
            effect.effect = SKRuntimeEffect.CreateShader(source, out string errors);

            if (errors != null)
                throw new Exception(errors);
            effect.uniforms = new SKRuntimeEffectUniforms(effect.effect);

            runtimeEffectCache.Add(shader.GetType(), effect);
        }

        foreach (var u in effect.compilation.Uniforms)
        {
            effect.uniforms[u.Name] = GetUniformValue(shader, u);
        }

        return (effect.effect, effect.uniforms);
    }
    private static SKRuntimeEffectUniform GetUniformValue(CanvasShader shader, ShaderVariable uniform)
    {
        var value = uniform.BackingField.GetValue(shader);

        if (value is float f)
            return f;

        if (value is Vector2 v2)
            return new[] { v2.X, v2.Y };

        if (value is Vector3 v3)
            return new[] { v3.X, v3.Y, v3.Z };

        if (value is Vector4 v4)
            return new[] { v4.X, v4.Y, v4.Z, v4.W };

        if (value is Matrix3x2 m32) 
        {
            float[] array = new float[3 * 2];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    array[i * 3 + j] = m32[i, j];
                }
            }
            return array;
        }

        if (value is Matrix4x4 m44)
        {
            float[] array = new float[4 * 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    array[i * 4 + j] = m44[i, j];
                }
            }
            return array;
        }

        throw new NotImplementedException();
    }
}
