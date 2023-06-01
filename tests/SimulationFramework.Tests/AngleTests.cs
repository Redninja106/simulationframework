using System;

namespace SimulationFramework.Tests;

[TestClass]
public class AngleTests
{
    const float TEST_DELTA = 0.00001f;

    [TestMethod]
    [DataRow(0f, 0)]
    [DataRow(1f, MathF.Tau / 360f)]
    [DataRow(45f, MathF.PI / 4f)]
    [DataRow(90, MathF.PI / 2f)]
    [DataRow(180f, MathF.PI)]
    [DataRow(360f, MathF.Tau)]
    [DataRow(540f, MathF.PI * 3)]
    [DataRow(-180, -MathF.PI)]
    public void ToDegrees(float expected, float value)
    {
        Assert.AreEqual(expected, Angle.ToDegrees(value), TEST_DELTA);
    }

    [TestMethod]
    [DataRow(0, 0f)]
    [DataRow(MathF.Tau / 360f, 1f)]
    [DataRow(MathF.PI / 4f, 45f)]
    [DataRow(MathF.PI / 2f, 90)]
    [DataRow(MathF.PI, 180f)]
    [DataRow(MathF.Tau, 360f)]
    [DataRow(MathF.PI * 3, 540f)]
    [DataRow(-MathF.PI, -180)]
    public void ToRadians(float expected, float value)
    {
        Assert.AreEqual(expected, Angle.ToRadians(value), TEST_DELTA);
    }

    [TestMethod]
    [DataRow(0, 0, 0)]
    [DataRow(0, MathF.PI, MathF.PI)]
    [DataRow(0, 0, MathF.Tau * 10)]
    [DataRow(MathF.PI * (2f/4f), MathF.PI / 4f, MathF.PI * (3f/4f))]
    public void Distance(float expected, float a, float b)
    {
        Assert.AreEqual(expected, Angle.Distance(a, b), TEST_DELTA);
    }
}
