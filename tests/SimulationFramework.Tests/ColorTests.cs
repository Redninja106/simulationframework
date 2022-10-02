using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Tests;

[TestClass]
public class ColorTests
{
    [TestMethod]
    [DataRow(0xff, 0x00, 0x00, 0.000f, 1.000f, 1.000f)]
    [DataRow(0x7f, 0x7f, 0x7f, 0.000f, 0.000f, 0.500f)]
    [DataRow(0x7f, 0x7f, 0x7f, 1.000f, 0.000f, 0.500f)]
    [DataRow(0x00, 0x7f, 0x00, 0.3333333f, 1.000f, 0.500f)]
    [DataRow(0xFF, 0xFF, 0xFF, 1.000f, 1.000f, 0.000f)]
    [DataRow(0xFF, 0xFF, 0xFF, 0.000f, 1.000f, 0.000f)]
    [DataRow(0x80, 0x80, 0x80, 1.000f, 0.500f, 0.000f)]
    [DataRow(0x80, 0x80, 0x80, 0.000f, 0.500f, 0.000f)]
    [DataRow(0x00, 0x00, 0x00, 1.000f, 0.000f, 0.000f)]
    [DataRow(0x00, 0x00, 0x00, 0.000f, 0.000f, 0.000f)]
    [DataRow(0xFF, 0x00, 0x00, 0.000f, 1.000f, 1.000f)]
    [DataRow(0xBF, 0xBF, 0x00, 0.167f, 0.750f, 1.000f)]
    [DataRow(0x00, 0x80, 0x00, 0.33333f, 0.500f, 1.000f)]
    [DataRow(0x80, 0xFF, 0xFF, 0.500f, 1.000f, 0.500f)]
    [DataRow(0x80, 0x80, 0xFF, 0.667f, 1.000f, 0.500f)]
    [DataRow(0xBF, 0x40, 0xBF, 0.834f, 0.750f, 0.667f)]
    [DataRow(0xA0, 0xA4, 0x24, 0.172f, 0.643f, 0.779f)]
    [DataRow(0x41, 0x1B, 0xEA, 0.698f, 0.918f, 0.887f)]
    [DataRow(0x1E, 0xAC, 0x41, 0.375f, 0.675f, 0.828f)]
    [DataRow(0xF0, 0xC8, 0x0E, 0.138f, 0.941f, 0.944f)]
    [DataRow(0xB4, 0x30, 0xE5, 0.788f, 0.897f, 0.792f)]
    [DataRow(0xED, 0x76, 0x51, 0.040f, 0.931f, 0.661f)]
    [DataRow(0xFE, 0xF8, 0x88, 0.158f, 0.998f, 0.467f)]
    [DataRow(0x19, 0xCB, 0x97, 0.451f, 0.795f, 0.875f)]
    [DataRow(0x36, 0x26, 0x98, 0.690f, 0.597f, 0.750f)]
    [DataRow(0x7E, 0x7E, 0xB8, 0.668f, 0.721f, 0.316f)]
    public void FromHSV(int expectedR, int expectedG, int expectedB, float hue, float saturation, float value)
    {
        Color hsv = Color.FromHSV(hue, saturation, value, 1.0f);

        Assert.AreEqual(expectedR, hsv.R);
        Assert.AreEqual(expectedG, hsv.G);
        Assert.AreEqual(expectedB, hsv.B);
    }
    
    [TestMethod]
    [DataRow(255, 0, 0, 255, "#FF0000")]
    [DataRow(255, 0, 0, 255, "#FF0000FF")]
    [DataRow(255, 255, 255, 255, "#FFFFFFFF")]
    [DataRow(255, 0, 0, 255, "0xFF0000")]
    [DataRow(255, 0, 0, 255, "0xFF0000FF")]
    [DataRow(255, 255, 255, 255, "0xFFFFFFFF")]
    [DataRow(255, 0, 0, 255, "FF0000")]
    [DataRow(255, 0, 0, 255, "FF0000FF")]
    [DataRow(255, 255, 255, 255, "FFFFFFFF")]
    public void Parse(int expectedR, int expectedG, int expectedB, int expectedA, string input)
    {
        bool success = Color.TryParse(input, out Color color);

        Assert.IsTrue(success);

        Assert.AreEqual(expectedR, color.R);
        Assert.AreEqual(expectedG, color.G);
        Assert.AreEqual(expectedB, color.B);
        Assert.AreEqual(expectedA, color.A);
    }

    [TestMethod]
    [DataRow("Hello, world!")]
    [DataRow("#this is some test input")]
    [DataRow("0xthis is some test input")]
    [DataRow("#FFGGFFFF")]
    [DataRow("#FFFFFF0G")]
    [DataRow("#FFFFFFFFF")]
    [DataRow("#FFFFFFF")]
    [DataRow("#FFFFF")]
    [DataRow("#FF")]
    public void Parse_InvalidInput(string input)
    {
        bool success = Color.TryParse(input, out _);

        Assert.IsFalse(success);
    }
}
