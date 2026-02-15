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
    public BlendMode blendMode;

    public bool HasSameState(RenderCommand other)
    {
        return effect.Equals(other.effect) &&
            clipRect == other.clipRect &&
            readMask == other.readMask &&
            writeMask == other.writeMask &&
            writeMaskValue == other.writeMaskValue &&
            blendMode == other.blendMode;
    }

    public void Execute(GLCanvas canvas, Matrix4x4 projection)
    {
        ApplyBlendMode(blendMode);

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

    private static void ApplyBlendMode(BlendMode blendMode)
    {
        switch (blendMode)
        {
            
            case BlendMode.Overwrite:
                glDisable(GL_BLEND);
                break;
            case BlendMode.Add:
                glEnable(GL_BLEND);
                glBlendEquation(GL_FUNC_ADD);
                glBlendFunc(GL_ONE, GL_ONE);
                break;
            case BlendMode.Subtract:
                glEnable(GL_BLEND);
                glBlendEquation(GL_FUNC_REVERSE_SUBTRACT);
                glBlendFunc(GL_ONE, GL_ONE);
                break;
            case BlendMode.Min:
                glEnable(GL_BLEND);
                glBlendEquation(GL_MIN);
                break;
            case BlendMode.Max:
                glEnable(GL_BLEND);
                glBlendEquation(GL_MAX);
                break;
            case BlendMode.Multiply:
                glEnable(GL_BLEND);
                glBlendEquation(GL_FUNC_ADD);
                glBlendFunc(GL_DST_COLOR, GL_ZERO);
                break;
            case BlendMode.Alpha:
            default:
                glEnable(GL_BLEND);
                glBlendEquation(GL_FUNC_ADD);
                glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
                break;
        }
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
