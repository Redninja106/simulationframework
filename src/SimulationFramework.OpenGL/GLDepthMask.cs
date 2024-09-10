using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;
internal class GLDepthMask : GLMask, IDepthMask
{
    public Comparison Comparison { get; set; } = Comparison.LessThanEqual;
    public float Bias { get; set; }

    public GLDepthMask(int width, int height) : base(width, height)
    {
    }


    public void Clear(float value)
    {
        BindFramebuffer();
        glClearDepthf(value);
        glClear(GL_DEPTH_BUFFER_BIT);
    }

    public override void BindRead(ITexture target)
    {
        base.BindRead(target);
        glEnable(GL_DEPTH_TEST);
        if (Bias != 0)
        {
            glEnable(GL_POLYGON_OFFSET_FILL);
            glPolygonOffset(Bias, 0);
        }
        else
        {
            glDisable(GL_POLYGON_OFFSET_FILL);
        }

        glDepthFunc(Comparison switch
        {
            Comparison.Always => GL_ALWAYS,
            Comparison.Never => GL_NEVER,
            Comparison.Equals => GL_EQUAL,
            Comparison.NotEqual => GL_NOTEQUAL,
            Comparison.LessThan => GL_LESS,
            Comparison.LessThanEqual => GL_LEQUAL,
            Comparison.GreaterThan => GL_GREATER,
            Comparison.GreaterThanEqual => GL_GEQUAL,
            _ => throw new InvalidOperationException(),
        });
        glDepthMask((byte)GL_FALSE);
    }

    public override void BindWrite(bool value)
    {
        base.BindWrite(value);
        glDepthMask((byte)GL_TRUE);
    }
}
