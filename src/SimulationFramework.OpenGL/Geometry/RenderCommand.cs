using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;

namespace SimulationFramework.OpenGL.Geometry;

struct RenderCommand
{
    public GeometryEffect effect;
    public Rectangle? clipRect;
    public GeometryChunk chunk;
    public IMask? readMask;
    public IMask? writeMask;
    public bool? writeMaskValue;

    public bool HasSameState(RenderCommand other)
    {
        return effect.Equals(other.effect) &&
            clipRect == other.clipRect &&
            readMask == other.readMask &&
            writeMask == other.writeMask &&
            writeMaskValue == other.writeMaskValue;
    }

    public void Execute(GLCanvas canvas, Matrix4x4 projection)
    {
        if (readMask is GLMask mask)
        {
            mask.BindRead(canvas.Target);
        }
        else
        {
            glDisable(GL_DEPTH_TEST);
            glDisable(GL_STENCIL_TEST);
        }

        if (writeMask is GLMask glWriteMask)
        {
            glWriteMask.BindWrite(writeMaskValue);
        }

        effect.Apply(canvas, projection);
        chunk.Draw();
    }
}

class TestGeom // : IGeometry
{
    private GeometryChunk[] chunks;

    public void Draw()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Draw();
        }
    }
}
