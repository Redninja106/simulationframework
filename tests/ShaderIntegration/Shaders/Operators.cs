using SimulationFramework;
using SimulationFramework.Drawing.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShaderIntegration.Shaders;

[ShaderTest]
internal class Operators : CanvasShader
{
    bool d = false;

    public override ColorF GetPixelColor(Vector2 position)
    {
        int x = 0, y = 0, z = 0;
        bool a = false, b = false, c = false;

        // basic arithmetic operators
        x = -y;
        x = y + z;
        x = y - z;
        x = y * z;
        x = y / z;
        x = y % z;

        // comparisons
        a = x == y;
        a = x != y;
        a = x < y;
        a = x <= y;
        a = x > y;
        a = x >= y;

        // boolean operators

        a = !b;
        a = b && c;
        a = b || c;
        a = c && d; // w/ short circuiting
        a = b || d; // w/ short circuiting

        // bitwise operators;
        x = ~y;
        x = y & z;
        x = y | z;
        x = y ^ z;
        x = y >> z;
        x = y << z;

        return ColorF.Black;
    }
}
