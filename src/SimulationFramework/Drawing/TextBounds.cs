namespace SimulationFramework.Drawing;

/// <summary>
/// Determines how the bounding box of text is calculated when rendering.
/// </summary>
public enum TextBounds
{
    /// <summary>
    /// The text's bounding box should be the smallest rectangle that entirely contains the text.
    /// <para>
    /// Note: different string values can yield different heights when using this value, leading to misaligned text when rendering.
    /// </para>
    /// </summary>
    BestFit,
    /// <summary>
    /// Excludes the lower areas of some letters and the higher areas of others in order to maintain a consistent baseline when rendering text.
    /// </summary>
    Smallest,
    /// <summary>
    /// Increases the size of the bounding box in some cases to maintain a consistent baseline when rendering text. For example, the word 'ear' will be treated as same height as 'Day'.
    /// </summary>
    Largest,
}