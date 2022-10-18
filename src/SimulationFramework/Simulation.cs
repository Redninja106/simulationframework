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
    private static readonly string[] knownPlatforms = new[]
    {
        "SimulationFramework.Desktop.dll",
    };

    /// <summary>
    /// This simulation's application.
    /// <para>
    /// If this simulation has been initialized (any overload of <see cref="Run()"/> has been called), this property will not be null.
    /// </para>
    /// </summary>
    public Application? Application { get; private set; }

    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    /// <param name="config"></param>
    public abstract void OnInitialize(AppConfig config);

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

    /// <summary>
    /// Starts this simulation using the provided platform.
    /// </summary>
    /// <param name="platform"></param>
    public void Run(IApplicationPlatform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);

        Application = new Application(platform);

        Application.Dispatcher.Subscribe<InitializeMessage>(m => {
            var config = AppConfig.CreateDefault();
            config.Title = "Simulation";
            OnInitialize(config);
            config.Apply();
        });

        Application.Dispatcher.Subscribe<RenderMessage>(m =>
        {
            m.Canvas.ResetState();
            OnRender(m.Canvas);
            m.Canvas.Flush();
        });

        Application.Dispatcher.Subscribe<UninitializeMessage>(m =>
        {
            OnUninitialize();
        });

        Application.Dispatcher.Subscribe<ResizeMessage>(m =>
        {
            OnResize(m.Width, m.Height);
        });
        
        Application.Start();

        Application.Dispose();
    }

    /// <summary>
    /// Starts this simulation.
    /// </summary>
    public void Run()
    {
        IEnumerable<IApplicationPlatform> platforms = GetAvailablePlatforms();

        IApplicationPlatform? selectedPlatform = platforms.FirstOrDefault();

        if (selectedPlatform is null)
        {
            throw Exceptions.NoPlatformAvailable();
        }

        Debug.Message($"Selected platform \"{selectedPlatform.GetType()}\".");

        Run(selectedPlatform);
    }

    /// <summary>
    /// Finds available, sorted platforms provided from loaded assemblies via the <see cref="ApplicationPlatformAttribute"/>.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="IApplicationPlatform"/> containing all currently available platforms.</returns>
    public static IEnumerable<IApplicationPlatform> GetAvailablePlatforms()
    {
        const string IsPlatformSupportedMethodName = "IsSupported";
        const string TypePlatformSearchContext = "It will not be used as an available platform.";
        const string MethodPlatformSearchContext = "Its base type will not be used as an available platform.";
                
        List<IApplicationPlatform> platforms = new();
        foreach (var knownPlatform in knownPlatforms)
        {
            try
            {
                Assembly.LoadFile(Path.GetFullPath("./" + knownPlatform));
            }
            catch { }
        }
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var attributes = assemblies.SelectMany(CustomAttributeExtensions.GetCustomAttributes<ApplicationPlatformAttribute>);

        foreach (var attr in attributes)
        {
            Type type = attr.PlatformType;

            Type? platformInterface = type.FindInterfaces((t, o) => t == typeof(IApplicationPlatform), null).SingleOrDefault();
            if (platformInterface is null)
            {
                Debug.Warn(Warnings.TypeDoesNotImplementInterface(type, typeof(IApplicationPlatform), TypePlatformSearchContext));
                continue;
            }

            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor is null)
            {
                Debug.Warn(Warnings.TypeDoesNotHaveParameterlessConstructor(type, TypePlatformSearchContext));
                continue;
            }

            MethodInfo? isPlatformSupportedMethod = type.GetMethod(IsPlatformSupportedMethodName, BindingFlags.Static | BindingFlags.Public, Type.EmptyTypes);
            if (isPlatformSupportedMethod is null)
            {
                Debug.Warn(Warnings.TypeDoesNotHavePublicStaticMethod(type, IsPlatformSupportedMethodName, TypePlatformSearchContext));
                continue;
            }

            if (isPlatformSupportedMethod.IsGenericMethod)
            {
                Debug.Warn(Warnings.MethodExpectedNonGeneric(isPlatformSupportedMethod, MethodPlatformSearchContext));
                continue;
            }

            if (isPlatformSupportedMethod.ReturnType != typeof(bool))
            {
                Debug.Warn(Warnings.MethodExpectedReturnType(isPlatformSupportedMethod, typeof(bool), MethodPlatformSearchContext));
                continue;
            }

            bool isSupported = false;
            try
            {
                isSupported = (bool)isPlatformSupportedMethod.Invoke(null, null)!;
            }
            catch
            {
                Debug.Warn(Warnings.MethodInvocationFailed(isPlatformSupportedMethod, MethodPlatformSearchContext));
                continue;
            }

            if (!isSupported)
            {
                continue;
            }

            var platformInstance = (IApplicationPlatform)constructor.Invoke(null)!;
            platforms.Add(platformInstance);
        }

        return platforms;
    }

    /// <summary>
    /// Creates a simulation from the provided delegates.
    /// </summary>
    /// <param name="initialize">The delegate to call when simulation initializes.</param>
    /// <param name="render">The delegate to call when simulation renders.</param>
    /// <returns>A simulation which uses the provided delegates.</returns>
    public static Simulation Create(Action<AppConfig> initialize, Action<ICanvas> render)
    {
        return Create(initialize, render, null);
    }

    /// <summary>
    /// Creates a simulation from the provided delegates.
    /// </summary>
    /// <param name="initialize">The delegate to call when simulation initializes.</param>
    /// <param name="render">The delegate to call when simulation renders.</param>
    /// <param name="uninitialize">The delegate to call when simulation uninitializes.</param>
    /// <returns>A simulation which uses the provided delegates.</returns>
    public static Simulation Create(Action<AppConfig> initialize, Action<ICanvas> render, Action? uninitialize)
    {
        return new ActionSimulation(initialize, render, uninitialize);
    }

    private class ActionSimulation : Simulation
    {
        private readonly Action<AppConfig> initialize;
        private readonly Action<ICanvas> render;
        private readonly Action? uninitialize;

        public ActionSimulation(Action<AppConfig> initialize, Action<ICanvas> render, Action? uninitialize)
        {
            this.initialize= initialize;
            this.render = render;
            this.uninitialize = uninitialize;
        }

        public override void OnInitialize(AppConfig config)
        {
            this.initialize(config);
        }

        public override void OnRender(ICanvas canvas)
        {
            this.render(canvas);
        }

        public override void OnUninitialize()
        {
            if (this.uninitialize is not null)
            {
                this.uninitialize();
            }
        }
    }
}