using System.Numerics;

namespace SimulationFramework.Drawing;

/// <summary>
/// Represents a font. Fonts can be loaded using <see cref="Graphics.LoadFont(string)"/>.
/// </summary>
public interface IFont : IDisposable
{
    /// <summary>
    /// The family name of the font.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Whether this font can be rendered using <see cref="FontStyle.Bold"/>. If this value is <see langword="false"/>, the <see cref="FontStyle.Bold"/> value has no effect when rendering text using this font.
    /// </summary>
    public bool SupportsBold { get; }

    /// <summary>
    /// Whether this font can be rendered using <see cref="FontStyle.Italic"/>. If this value is <see langword="false"/>, the <see cref="FontStyle.Italic"/> value has no effect when rendering text using this font.
    /// </summary>
    public bool SupportsItalic { get; }

    sealed Rectangle MeasureText(string text, float size, FontStyle style) => MeasureText(text.AsSpan(), size, style, 0, out _);
     
    sealed Rectangle MeasureText(string text, float size, FontStyle style, float maxLength, out int charsMeasured) => MeasureText(text.AsSpan(), size, style, maxLength, out charsMeasured);

    sealed Rectangle MeasureText(ReadOnlySpan<char> text, float size, FontStyle style) => MeasureText(text, size, style, 0, out _);

    Rectangle MeasureText(ReadOnlySpan<char> text, float size, FontStyle style, float maxLength, out int charsMeasured);

}
