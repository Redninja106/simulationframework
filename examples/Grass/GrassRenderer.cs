using SimulationFramework;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Grass;
internal class GrassRenderer : IDisposable
{
    IRenderer renderer;
    IBuffer<GrassVertex> vertexBuffer;

    public ref ColorF GrassTopColor => ref fragmentShader.TopColor;
    public ref ColorF GrassBottomColor => ref fragmentShader.BottomColor;
    public ref Matrix4x4 TransformMatrix => ref vertexShader.TransformMatrix;

    private GrassVertexShader vertexShader;
    private GrassFragmentShader fragmentShader;

    public GrassRenderer(GrassOptions options)
    {
        Regenerate(options);
        renderer = Graphics.CreateRenderer();
    }

    public void Render()
    {
        vertexShader.time = Time.TotalTime;

        renderer.SetVertexShader(vertexShader);
        renderer.SetFragmentShader(fragmentShader);

        renderer.SetVertexBuffer(vertexBuffer);

        renderer.DrawPrimitives(PrimitiveKind.Triangles, vertexBuffer.Length);
    }

    public void Regenerate(GrassOptions options)
    {
        GrassBlade[] blades = new GrassBlade[options.bladeCount];

        for (int i = 0; i < options.bladeCount; i++)
        {
            var rng = Random.Shared;
            var position = new Vector3(rng.NextSingle() * 2 - 1, 0, rng.NextSingle() * 2 - 1);
            var rotation = rng.NextSingle() * MathF.Tau;

            var height = rng.NextSingle();
            blades[i] = new(position, rotation, MathHelper.Lerp(options.minHeight, options.maxHeight, height * height));
        }

        GrassVertex[] vertices = new GrassVertex[blades.Length * 3];

        for (int i = 0; i < blades.Length; i++)
        {
            blades[i].WriteVertices(vertices, i * 3);
        }

        vertexBuffer?.Dispose();
        vertexBuffer = Graphics.CreateBuffer(vertices);
    }

    public void Dispose()
    {
        vertexBuffer.Dispose();
        renderer.Dispose();
    }
}