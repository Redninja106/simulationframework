namespace SimulationFramework.IMGUI;

public enum PopupFlags
{
    None = 0,
    MouseButtonLeft = 0,        // For BeginPopupContext*(): open on Left Mouse release. Guaranteed to always be == 0 (same as ImGuiMouseButton_Left)
    MouseButtonRight = 1,        // For BeginPopupContext*(): open on Right Mouse release. Guaranteed to always be == 1 (same as ImGuiMouseButton_Right)
    MouseButtonMiddle = 2,        // For BeginPopupContext*(): open on Middle Mouse release. Guaranteed to always be == 2 (same as ImGuiMouseButton_Middle)
    MouseButtonDefault = 1,
    NoOpenOverExistingPopup = 1 << 5,   // For OpenPopup*(), BeginPopupContext*(): don't open if there's already a popup at the same level of the popup stack
    NoOpenOverItems = 1 << 6,   // For BeginPopupContextWindow(): don't return true when hovering items, only when hovering empty space
    AnyPopupId = 1 << 7,   // For IsPopupOpen(): ignore the ImGuiID parameter and test for any popup.
    AnyPopupLevel = 1 << 8,   // For IsPopupOpen(): search/test at any level of the popup stack (default test in the current level)
    AnyPopup = AnyPopupId | AnyPopupLevel
}