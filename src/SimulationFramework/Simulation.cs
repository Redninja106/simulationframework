﻿using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// The base class for all simulations. Inherit this class or use <see cref="Create(Action{SimulationFramework.AppConfig}, Action{ICanvas})"/> to create a simulation.
/// </summary>
public abstract class Simulation
{
    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    /// <param name="application"></param>
    public abstract void OnInitialize();

    /// <summary>
    /// Called when the simulation should render.
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void OnRender(ICanvas canvas);

    /// <summary>
    /// Called when the simulation should uninitialize.
    /// </summary>
    public virtual void OnUninitialize() { }

    /// <summary>
    /// Called when the simulation's video output is resized.
    /// </summary>
    /// <param name="width">The new width of the simulation's video output.</param>
    /// <param name="height">The new height of the simulation's video output.</param>
    public virtual void OnResize(int width, int height) { }

    public static void Start<T>() where T : Simulation, new()
    {
        Start<T>(null);
    }

    public static void Start<T>(ISimulationPlatform? platform) where T : Simulation, new()
    {
        SimulationHost host = new();
        host.Initialize(platform);
        host.Start(new T());
    }
}