namespace SimulationFramework.IMGUI;
public enum ComboFlags
{
    None = 0,
    ///<summary>
    /// Align the popup toward the left by default
    ///</summary>
    PopupAlignLeft = 1 << 0,   
    ///<summary>
    /// Max ~4 items visible. Tip: If you want your combo popup to be a specific size you can use SetNextWindowSizeConstraints() prior to calling BeginCombo()
    ///</summary>
    HeightSmall = 1 << 1,   
    ///<summary>
    /// Max ~8 items visible (default)
    ///</summary>
    HeightRegular = 1 << 2,   
    ///<summary>
    /// Max ~20 items visible
    ///</summary>
    HeightLarge = 1 << 3,   
    ///<summary>
    /// As many fitting items as possible
    ///</summary>
    HeightLargest = 1 << 4,   
    ///<summary>
    /// Display on the preview box without the square arrow button
    ///</summary>
    NoArrowButton = 1 << 5,   
    ///<summary>
    /// Display only a square arrow button
    ///</summary>
    NoPreview = 1 << 6,   
}
