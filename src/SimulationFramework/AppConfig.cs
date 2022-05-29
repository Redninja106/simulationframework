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
    public string Title { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    
    public bool Resizable { get; set; }
    public bool Fullscreen { get; set; }
    public bool TitlebarHidden { get; set; }

    public AppConfig()
    {

    }

    public static AppConfig Open() 
    {
        var controller = Application.Current.GetComponent<IAppController>();
        return controller.CreateConfig();
    }

    public bool Apply()
    {
        var controller = Application.Current.GetComponent<IAppController>();
        return controller.ApplyConfig(this);
    }
    
    public void ResetToDefault()
    {
        Title = "Application";
        Width = 1280;
        Height = 720;
        Resizable = true;
        Fullscreen = false;
        TitlebarHidden = false;
    }
    //public void EnableSimulationPane(int width, int height, bool scaling = true) { }
}