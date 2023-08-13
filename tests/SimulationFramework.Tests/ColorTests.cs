namespace SimulationFramework.Tests;

[TestClass]
public class ColorTests
{
    [TestMethod]
    [DataRow(0xFF, 0x00, 0x00, 0f, 1f, 1f)]
    [DataRow(0x00, 0x00, 0x00, 0f, 0f, 0f)]
    [DataRow(0xFF, 0x00, 0x00, 3/3f, 1f, 1f)]
    [DataRow(0x00, 0x00, 0xFF, 2/3f, 1f, 1f)]
    [DataRow(0x00, 0xFF, 0x00, 1/3f, 1f, 1f)]
    [DataRow(0xFF, 0x00, 0x00, 0/3f, 1f, 1f)]
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
