using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;
using SimulationFramework.Drawing.Shaders.Compiler;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

Start<Program>();

partial class Program : Simulation
{
    public override unsafe void OnInitialize()
    {
        ShaderCompiler.DumpShaders = true;
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Fill(new Shader());
        canvas.DrawRect(1, 1, 1, 1);
    }
}

class Shader : CanvasShader
{
    int[,,] voxels = new int[100, 100, 100];

    public override ColorF GetPixelColor(Vector2 position)
    {
        ColorF r = ColorF.Red;
        raycast(default, out float t, out var n, out var v);

        return r;
    }
    bool raycast(Ray ray, out float outT, out Vector3 outNormal, out int3 outVoxel)
    {
        outNormal = default;

        int3 voxel = new int3(Floor(ray.Origin));
        int3 step = new int3(Sign(ray.Direction));

        if (step.x == 0 && step.y == 0 && step.z == 0)
        {
            outT = default;
            outNormal = default;
            outVoxel = default;
            return false;
        }

        float tNear = 0, tFar = 0;

        // if (!PartialBoxRaycast(ray, Vector3.Zero, new(100, 100, 100), out tNear, out tFar))
        // {
        //     outT = default;
        //     outNormal = default;
        //     outVoxel = default;
        //     return false;
        // }

        // if (voxels[0, 0, 0] != 0)
        // {
        //     outT = default;
        //     outNormal = default;
        //     outVoxel = default;
        //     return true;
        // }

        Vector3 start = ray.At(tNear);
        Vector3 end = ray.At(tFar);

        Vector3 d = end - start;
        Vector3 tDelta = step.ToVec3() / d;
        Vector3 tMax = tDelta * getMax(start, step.ToVec3());

        voxel = new int3(start);

        int dist = 100;

        float t = ray.MinT;

        //outT = default;
        //outNormal = default;
        //outVoxel = default;
        //return false;
        bool b = --dist > 0 && t < ray.MaxT;
        while (b)
        {
            int3 v = voxel;

            if (v.x < 0 || v.x >= voxels.GetLength(0) || v.y < 0 || v.y >= voxels.GetLength(1) || v.z < 0 || v.z >= voxels.GetLength(2))
            {
                outT = 0;
                outVoxel = new(-69, -69, -69);
                return false;
            }

            if (voxels[v.x, v.y, v.z] != 0)
            {
                outT = Max(Max(tMax.X - tDelta.X, tMax.Y - tDelta.Y), tMax.Z - tDelta.Z) * Length(d);
                outVoxel = voxel;
                return t <= ray.MaxT;
            }

            if (tMax.X < tMax.Y)
            {
                if (tMax.X < tMax.Z)
                {
                    voxel.x += step.x;
                    tMax.X += tDelta.X;
                    outNormal = Vec3(-step.x, 0, 0);
                }
                else
                {
                    voxel.z += step.z;
                    tMax.Z += tDelta.Z;
                    outNormal = Vec3(0, 0, -step.z);
                }
            }
            else
            {
                if (tMax.Y < tMax.Z)
                {
                    voxel.y += step.y;
                    tMax.Y += tDelta.Y;
                    outNormal = Vec3(0, -step.y, 0);
                }
                else
                {
                    voxel.z += step.z;
                    tMax.Z += tDelta.Z;
                    outNormal = Vec3(0, 0, -step.z);
                }
            }

            b = --dist > 0 && t < ray.MaxT;
        }

        outT = default;
        outVoxel = default;
        return false;
    }

    Vector3 getMax(Vector3 start, Vector3 step)
    {
        float x, y, z;
        if (step.X > 0)
        {
            x = 1 - Fract(start.X);
        }
        else
        {
            x = Fract(start.X);
        }

        if (step.Y > 0)
        {
            y = 1 - Fract(start.Y);
        }
        else
        {
            y = Fract(start.Y);
        }

        if (step.Z > 0)
        {
            z = 1 - Fract(start.Z);
        }
        else
        {
            z = Fract(start.Z);
        }

        return new Vector3(x, y, z);
    }
}

struct Ray
{
    public Vector3 Origin;
    public float MinT;
    public Vector3 Direction;
    public float MaxT;

    public Vector3 At(float t)
    {
        return Origin + Direction * t;
    }
}

struct int3
{
    public int x, y, z;

    public int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int3(Vector3 vec)
    {
        x = (int)vec.X;
        y = (int)vec.Y;
        z = (int)vec.Z;
    }

    public Vector3 ToVec3()
    {
        return new(x, y, z);
    }

    public static int3 operator +(int3 a, int3 b)
    {
        return new(
            a.x + b.x,
            a.y + b.y,
            a.z + b.z
            );
    }
}