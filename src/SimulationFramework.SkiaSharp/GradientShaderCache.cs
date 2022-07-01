﻿using SimulationFramework.Drawing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.SkiaSharp;

/// <summary>
/// Maintains <see cref="SKShader"/> instances for gradients efficiently.
/// </summary>
internal static class GradientShaderCache
{
    private static readonly List<Entry> cache = new();

    public static SKShader GetShader(Gradient gradient)
    {
        Entry result = null;

        // iterate over entire list to invalid entries
        for (int i = 0; i < cache.Count; i++)
        {
            var entry = cache[i];

            if (!entry.IsValid)
            {
                // the entry's gradient object was gc'ed. delete our shader & cache entry.
                cache.RemoveAt(i);
                entry.Dispose();
                continue;
            }

            if (entry.Gradient == gradient)
            {
                result = entry;
            }
        }

        // if we didn't find a shader, make one and cache it.
        if (result is null)
        {
            result = new Entry(gradient, CreateShader(gradient));
            
            cache.Add(result);
        }

        return result.Shader;
    }

    public static void Clear()
    {
        for (int i = 0; i < cache.Count; i++)
        {
            cache[i].Dispose();
        }

        cache.Clear();
    }

    private static SKShader CreateShader(Gradient gradient)
    {
        if (gradient is null)
            return null;

        var inspector = new GradientShaderBuilder();

        gradient.Accept(inspector);

        return inspector.Result;
    }

    private class Entry : IDisposable
    {
        private readonly SKShader shader;
        private readonly GCHandle gradientHandle;

        public bool IsValid => gradientHandle.IsAllocated;
        public Gradient Gradient => gradientHandle.Target as Gradient;
        public SKShader Shader => shader;

        public Entry(Gradient gradient, SKShader shader)
        {
            gradientHandle = GCHandle.Alloc(gradient, GCHandleType.Weak);
            this.shader = shader;
        }

        public void Dispose()
        {
            if (gradientHandle.IsAllocated)
                gradientHandle.Free();

            shader.Dispose();
        }
    }

    private class GradientShaderBuilder : IGradientVisitor
    {
        public SKShader Result { get; private set; }

        public void VisitLinear(Gradient gradient, Vector2 from, Vector2 to)
        {
            DecomposeStops(gradient.Stops, out var colors, out var positions);
            Result = SKShader.CreateLinearGradient(from.AsSKPoint(), to.AsSKPoint(), colors, positions, gradient.TileMode.AsSKShaderTileMode(), gradient.Transform.AsSKMatrix());
        }

        public void VisitRadial(Gradient gradient, Vector2 position, float radius)
        {
            DecomposeStops(gradient.Stops, out var colors, out var positions);
            Result = SKShader.CreateRadialGradient(position.AsSKPoint(), radius, colors, positions, gradient.TileMode.AsSKShaderTileMode(), gradient.Transform.AsSKMatrix());
        }

        private static void DecomposeStops(ReadOnlySpan<GradientStop> stops, out SKColor[] colors, out float[] positions)
        {
            colors = new SKColor[stops.Length];
            positions = new float[stops.Length];

            for (int i = 0; i < stops.Length; i++)
            {
                colors[i] = stops[i].Color.AsSKColor();
                positions[i] = stops[i].Position;
            }
        }
    }
}
