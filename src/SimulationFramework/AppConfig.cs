using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Provides ways to enable and disable certain functionality of the simulation.
/// </summary>
public class AppConfig
{
    /// <summary>
    /// The title of the application.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The width of the application's window.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the application's window.
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Whether the window should be resizable or not.
    /// </summary>
    public bool Resizable { get; set; }

    /// <summary>
    /// Whether the window should be fullscreen or not.
    /// </summary>
    public bool Fullscreen { get; set; }

    /// <summary>
    /// Whether the window's titlebar should be hidden or not.
    /// </summary>
    public bool TitlebarHidden { get; set; }

    private AppConfig()
    {
        Title = string.Empty;
    }

    /// <summary>
    /// Creates a new AppConfig with the simulation's current options.
    /// </summary>
    /// <returns></returns>
    public static AppConfig Create() 
    {
        var controller = Application.Current.GetComponent<IAppController>() ?? throw Exceptions.CoreComponentNotFound();
        var result = new AppConfig();
        controller.InitializeConfig(result);
        return result;
    }

    /// <summary>
    /// Creates a new AppConfig with default options.
    /// </summary>
    public static AppConfig CreateDefault()
    {
        var result = new AppConfig();
        result.ResetToDefault();
        return result;
    }

    /// <summary>
    /// Tries to apply this AppConfig to the simulation.
    /// </summary>
    /// <returns><see langword="true"/> if all changes were applied, <see langword="false"/> if one or more changes failed to apply.</returns>
    public bool Apply()
    {
        var controller = Application.Current.GetComponent<IAppController>() ?? throw Exceptions.CoreComponentNotFound();
        return controller.ApplyConfig(this);
    }

    // Resets this AppConfig to its default values.
    private void ResetToDefault()
    {
        Title = "Application";
        Width = 1280;
        Height = 720;
        Resizable = true;
        Fullscreen = false;
        TitlebarHidden = false;
    }
}