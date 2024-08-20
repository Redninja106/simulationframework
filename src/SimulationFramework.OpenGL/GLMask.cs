using ImGuiNET;
using SimulationFramework.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationFramework.OpenGL;

internal class GLMask : IMask
{
    public bool this[int x, int y] 
    {
        get
        {
            if ((uint)x >= Width || (uint)y >= Height)
                throw new IndexOutOfRangeException();

            return data[y * Width + x] == 1;
        }
        set
        {
            if ((uint)x >= Width || (uint)y >= Height)
                throw new IndexOutOfRangeException();

            data[y * Width + x] = (byte)(value ? 1 : 0);
        }
    }

    public int Width { get; }
    public int Height { get; }
    public bool? WriteValue { get; set; }

    private uint tex;
    private uint texFbo;
    private bool pixelsInvalid;
    protected uint[] data;

    // A Mask can have a size different from the render target it is being used on,
    // so each render target keeps it's own depth stencil buffer and we copy to and
    // from it (only) when needed.

    private IGLImage? residence;

    public unsafe GLMask(int width, int height)
    {
        Width = width;
        Height = height;

        fixed (uint* texPtr = &tex)
        {
            glGenTextures(1, texPtr);
        }

        glBindTexture(GL_TEXTURE_2D, tex);
        glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_DEPTH24_STENCIL8, width, height, 0, GL_DEPTH_STENCIL, GL_UNSIGNED_INT_24_8, null);
        glBindTexture(GL_TEXTURE_2D, 0);

        fixed (uint* texFboPtr = &texFbo)
        {
            glCreateFramebuffers(1, texFboPtr);
            glNamedFramebufferTexture(texFbo, GL_DEPTH_STENCIL_ATTACHMENT, tex, 0);
            if (glCheckNamedFramebufferStatus(texFbo, GL_READ_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
            {
                throw new Exception("Could not create mask framebuffer!");
            }
        }

        data = new uint[width * height];
    }

    public unsafe void ApplyChanges()
    {
        fixed (uint* dataPtr = data)
        {
            glTextureSubImage2D(tex, 0, 0, 0, Width, Height, GL_DEPTH_STENCIL, GL_UNSIGNED_INT_24_8, dataPtr);
        }

        if (this.residence != null)
        {
            CopyToResidence(this.residence);
        }
    }

    public unsafe void Clear(bool value)
    {
        Array.Fill(data, (byte)(value ? 1 : 0));
        ApplyChanges();
    }

    public unsafe void Dispose()
    {
        uint t = tex;
        glDeleteTextures(1, &t);
        tex = 0;
    }

    private unsafe void ReadLocalPixels()
    {
        fixed (uint* dataPtr = data) 
        {
            glReadPixels(0, 0, Width, Height, GL_DEPTH24_STENCIL8, GL_UNSIGNED_INT_24_8, dataPtr);
            pixelsInvalid = false;
        }
    }

    public void InvalidateLocalPixels()
    {
        pixelsInvalid = true;
    }

    public virtual void BindRead(ITexture target)
    {
        if (target is not IGLImage frame)
            throw new NotImplementedException();

        if (frame.Resident != this)
        {
            MakeResident(frame);
        }

        glEnable(GL_STENCIL_TEST);
        glDisable(GL_DEPTH_TEST);

        glStencilFunc(GL_EQUAL, 0x1, 0x1);
        glStencilMask(0x1);
        glStencilOp(GL_KEEP, GL_KEEP, GL_KEEP);
    }

    public void MakeResident(IGLImage texture)
    {
        if (texture.Resident != null)
        {
            GLMask glMask = (GLMask)texture.Resident;
            glMask.EvictResidence();
        }

        this.residence = texture;
        texture.Resident = this;
        CopyToResidence(texture);
    }

    private void CopyToResidence(IGLImage residence)
    {
        if (residence is GLFrame frame)
        {
            int width = Math.Min(Width, residence.Width);
            int height = Math.Min(Height, residence.Height);
            // TODO: this should copy to top left
            glBlitNamedFramebuffer(this.texFbo, 0, 0, 0, width, height, 0, 0, width, height, GL_STENCIL_BUFFER_BIT, GL_NEAREST);
        }
        else if (residence is GLTexture texture)
        {
            if (Width < texture.Width || Height < texture.Height)
            {
                throw new NotImplementedException();
            }
            else
            {
                GLCanvas canvas = (GLCanvas)texture.GetCanvas();
                glNamedFramebufferTexture(canvas.fbo, GL_DEPTH_STENCIL_ATTACHMENT, this.tex, 0);
            }
        }
        else
        {
            throw new();
        }
    }

    public void EvictResidence()
    {
        if (residence is GLFrame frame)
        {
            int width = Math.Min(Width, residence.Width);
            int height = Math.Min(Height, residence.Height);
            // TODO: this should copy to top left
            glBlitNamedFramebuffer(0, this.texFbo, 0, 0, width, height, 0, 0, width, height, GL_STENCIL_BUFFER_BIT, GL_NEAREST);
        }
        else if (residence is GLTexture texture)
        {
            if (Width < texture.Width || Height < texture.Height)
            {
                throw new NotImplementedException();
            }
            else
            {
                GLCanvas canvas = (GLCanvas)texture.GetCanvas();
                glNamedFramebufferTexture(canvas.fbo, GL_DEPTH_STENCIL_ATTACHMENT, 0, 0);
            }
        }
        else
        {
            throw new();
        }

        residence.Resident = null;
        residence = null;
    }

    public virtual void BindWrite(bool value)
    {
        if (value)
        {
            glStencilOp(GL_REPLACE, GL_KEEP, GL_KEEP);
        }
        else
        {
            glStencilOp(GL_KEEP, GL_KEEP, GL_DECR);
        }
    }
}
