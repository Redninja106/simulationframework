using SimulationFramework.Desktop;
using SimulationFramework.Drawing.Shaders;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework;
using System.Numerics;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;
using SimulationFramework.Drawing.Shaders.Compiler;
using SimulationFramework.OpenGL;

GLGraphics.ForceCompatability = true;
Start<Program>();

partial class Program : Simulation
{
    IterationsInversions2 shader = new();
    Vector2 targetPos;

    public override void OnInitialize()
    {
    }

    public override void OnRender(ICanvas canvas)
    {
        shader.width = canvas.Width;
        shader.height = canvas.Height;
        shader.mousePosition = Vector2.Lerp(shader.mousePosition, targetPos, 1f - MathF.Pow(0.01f, Time.DeltaTime));
        // targetPos = Mouse.Position - new Vector2(canvas.Width, canvas.Height);
        if (Keyboard.IsKeyDown(Key.Space))
        {
            shader.time += Time.DeltaTime * 100;
        }
        else
        {
            shader.time += Time.DeltaTime;
        }
        ShaderCompiler.DumpShaders = true;
        canvas.Clear(Color.FromHSV(0, 0, .15f));
        canvas.Fill(shader);
        canvas.Fill(new illumshad());
        canvas.DrawRect(0, 0, canvas.Width, canvas.Height);
    }
}

// ported from https://www.shadertoy.com/view/4t3SzN
class IterationsInversions2 : CanvasShader
{
    public int width, height;
    public float time;
    public Vector2 mousePosition;

    int[] i;
    light[] ls = [];

    public Vector3 Shape(Vector2 uv)
    {
        float time = this.time * 0.05f + 47.0f;

        Vector2 z = -System.Numerics.Vector2.One + 2.0f * uv;
        z *= 1.5f;

        bool b = AsBool(1);

        Vector3 col = new Vector3(1.0f);
        for (int j = 0; j < 48; j++)
        {
            float s = j / 16.0f;
            float f = 0.2f * (0.5f + 1.0f * Fract(MathF.Sin(s * 20.0f)));

            Vector2 c = 0.5f * new Vector2(Cos(f * time + 17.0f * s), Sin(f * time + 19.0f * s));
            z -= c;
            float zr = Length(z);
            float ar = MathF.Atan2(z.Y, z.X) + zr * 0.6f;
            z = new Vector2(Cos(ar), Sin(ar)) / zr;
            z += c;

            // color
            col -= 0.5f * Exp(-10.0f * System.Numerics.Vector2.Dot(z, z)) * (new Vector3(0.25f) + 0.4f * Sin(new Vector3(5.5f + 1.5f * s) + MakeVector3(1.6f, 0.8f, 0.5f)));
        }

        return col;
    }

    public override ColorF GetPixelColor(Vector2 position)
    {
        const int AA = 2;
        float e = 1.0f / width;

        Vector3 tot = new Vector3(0.0f);
        for (int m = 0; m < AA; m++)
            for (int n = 0; n < AA; n++)
            {
                Vector2 uv = (position + new Vector2(m, n) * (1f / AA) + mousePosition) / new Vector2(width, height);
                Vector3 col = Shape(uv);
                float f = Dot(col, MakeVector3(0.333f));
                Vector3 nor = Normalize(MakeVector3(Dot(Shape(uv + MakeVector2(e, 0.0f)), MakeVector3(0.333f)) - f,
                                             Dot(Shape(uv + MakeVector2(0.0f, e)), MakeVector3(0.333f)) - f,
                                             e));
                col += 0.2f * MakeVector3(1.0f, 0.9f, 0.5f) * Dot(nor, MakeVector3(0.8f, 0.4f, 0.2f)); ;
                col += MakeVector3(0.3f * nor.Z);
                tot += col;
            }
        tot /= (float)(AA * AA);

        tot = Pow(Clamp(tot, MakeVector3(0.0f), MakeVector3(1.0f)), MakeVector3(0.8f, 1.1f, 1.3f));

        Vector2 uv2 = position / MakeVector2(width, height);
        tot *= 0.4f + 0.6f * Pow(16.0f * uv2.X * uv2.Y * (1.0f - uv2.X) * (1.0f - uv2.Y), 0.1f);

        return new ColorF(tot.X, tot.Y, tot.Z, 1.0f);
    }
}

// ported from https://www.shadertoy.com/view/XsXXDn
class Creation : CanvasShader
{
    public int width, height;
    public float time;
    public Vector2 mousePosition;

    public override ColorF GetPixelColor(Vector2 position)
    {
        Vector2 size = MakeVector2(width, height);
        Vector3 c = default;
        float l = 0, z = time;
        for (int i = 0; i < 3; i++)
        {
            Vector2 uv, p = position / size;
            uv = p;
            p -= mousePosition / size;
            p.X *= size.X / size.Y;
            z += .07f;
            l = Length(p);
            uv += p / l * (Sin(z) + 1.0f) * Abs(Sin(l * 9.0f - z - z));
            c[i] = .01f / Length(Mod(uv, 1.0f) - MakeVector2(.5f));
        }
        return new ColorF(MakeVector4(c * (1f / l), time));
    }
}

class TextureShader : CanvasShader
{
    public ITexture myTexture;

    public override ColorF GetPixelColor(Vector2 position)
    {
        return myTexture.Sample(position);
    }
}
public struct light
{
    public Vector2 pos;
    public float rad;
    public float lum;
    public ColorF col;
    public float rnd;
    public bool clmp;
}

public struct tri
{
    public Vector2 p1, p2, p3;
}

public class illumshad : CanvasShader
{
    public light[] l = [];
    public tri[] objs = [];

    public override ColorF GetPixelColor(Vector2 pos)
    {
        ColorF col = ColorF.Black;
        Vector3 _col = MakeVector3(0, 0, 0);

        for (int i = 0; i < l.Length; i++)
        {
            float dist = Distance(pos, l[i].pos);

            if(dist <= l[i].rad) {
                float r = Max((l[i].lum * l[i].col.R + (l[i].lum - 1)) * (1 - dist / l[i].rad), 0);
                float g = Max((l[i].lum * l[i].col.G + (l[i].lum - 1)) * (1 - dist / l[i].rad), 0);
                float b = Max((l[i].lum * l[i].col.B + (l[i].lum - 1)) * (1 - dist / l[i].rad), 0);

                if (l[i].clmp) {
                    r = Round(r / l[i].rnd) * l[i].rnd;
                    g = Round(g / (l[i].rnd * .5f)) * (l[i].rnd * .5f);
                    b = Round(b / l[i].rnd) * l[i].rnd;
                }

                _col += MakeVector3(r, g, b);
            }
        }

        col = new ColorF(_col.X, _col.Y, _col.Z);

        return col;
    }

    public bool intri(Vector2 p, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float areaOrig = Abs((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));

        float area1 = Abs((p1.X - p.X) * (p2.Y - p.Y) - (p2.X - p.X) * (p1.Y - p.Y));
        float area2 = Abs((p2.X - p.X) * (p3.Y - p.Y) - (p3.X - p.X) * (p2.Y - p.Y));
        float area3 = Abs((p3.X - p.X) * (p1.Y - p.Y) - (p1.X - p.X) * (p3.Y - p.Y));

        return (area1 + area2 + area3) == areaOrig;
    }
}