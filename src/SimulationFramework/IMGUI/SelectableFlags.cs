namespace SimulationFramework.IMGUI;

// imgui won't be comment as it will be replace by a custom gui solution
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public enum SelectableFlags
{
    None = 0,
    DontClosePopups = 1 << 0,
    SpanAllColumns = 1 << 1,
    AllowDoubleClick = 1 << 2,
    Disabled = 1 << 3,
    AllowItemOverlap = 1 << 4
}
