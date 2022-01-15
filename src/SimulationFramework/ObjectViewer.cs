using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.IMGUI;

namespace SimulationFramework;

/// <summary>
/// Simple window to display <see cref="IViewable"/> objects.
/// </summary>
public static class ObjectViewer
{
    public static bool IsOpen { get => isOpen; set => isOpen = value; }

    private static bool isOpen;

    private static List<WeakReference<IViewable>> history = new();
    private static List<(string name, List<IViewable> objects)> lists = new();

    private static IViewable currentlySelected = null;
    private static Key keybind = Key.Unknown;

    static ObjectViewer()
    {
        AddList("Pinned Objects");
    }

    /// <summary>
    /// Toggles the viewer between being open and closed.
    /// </summary>
    public static void ToggleOpen()
    {
        isOpen = !isOpen;
    }

    /// <summary>
    /// Sets the keybind to open the viewer.
    /// </summary>
    /// <param name="key">The key to toggle the window.</param>
    public static void SetKeybind(Key key)
    {
        keybind = key;
    }

    /// <summary>
    /// Selects an object to be viewed in the Object Viewer window.
    /// </summary>
    /// <param name="viewable">The object to select.</param>
    /// <param name="popup">Indicates to force the window open if its closed.</param>
    public static void Select(IViewable viewable, bool popup = false)
    {
        if (popup)
            isOpen = true;

        history.Add(new WeakReference<IViewable>(viewable));

        currentlySelected = viewable;
    }

    /// <summary>
    /// Adds an object to the "Pinned Objects" list.
    /// </summary>
    /// <param name="item">The item to pin.</param>
    public static void PinObject(IViewable item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        AddListObject(item, "Pinned Objects");
    }

    /// <summary>
    /// Creates a new list, optionally with an inital set of objects. A list is a collapsable menu at the top of the viewer that can be used to easily select objects.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="initialObjects"></param>
    public static void AddList(string name, IEnumerable<IViewable> initialObjects = null)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        if (lists.Any(l => l.name == name))
            throw new Exception("A list of this name already exists!");

        lists.Add((name, new List<IViewable>(initialObjects ?? Array.Empty<IViewable>())));
    }

    /// <summary>
    /// Adds an object to a list. If the provided list does not exist, this method creates a new one with the provided name.
    /// </summary>
    /// <param name="item">The item to add to the list.</param>
    /// <param name="listName">The name of the list to add the object to.</param>
    public static void AddListObject(IViewable item, string listName)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        if (listName is null) throw new ArgumentNullException(nameof(listName));

        if (!lists.Any(l => l.name == listName))
            AddList(listName);

        lists.Single(l => l.name == listName).objects.Add(item);
    }

    /// <summary>
    /// Lays out the viewer window.
    /// </summary>
    internal static void Layout()
    {
        if (Keyboard.IsKeyPressed(keybind))
            ToggleOpen();

        if (IsOpen && ImGui.BeginWindow("Object Viewer", ref isOpen, WindowFlags.MenuBar))
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("History", true))
                {
                    foreach (var item in history)
                    {
                        if (item.TryGetTarget(out IViewable viewable))
                        {
                            if (ImGui.MenuItem(viewable.ToString()))
                            {
                                Select(viewable);
                            }
                        }
                        else
                        {
                            history.Remove(item);
                        }
                    }

                    ImGui.EndMenu();
                }

                foreach (var (name, items) in lists)
                {
                    if (ImGui.BeginMenu(name, true))
                    {
                        foreach (var item in items)
                        {
                            if (ImGui.MenuItem(item.ToString()))
                            {
                                Select(item);
                            }
                        }

                        ImGui.EndMenu();
                    }
                }

                ImGui.EndMenuBar();
            }

            if (currentlySelected is not null)
            {
                currentlySelected.Layout();
            }
            else
            {
                ImGui.Text("No object is currently selected! Call Viewer.Select() to selected an object.");
            }
        }
        ImGui.EndWindow();
    }
}