using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
internal class ApplicationBuilder
{
    private Func<IApplicationHost>? hostProvider;
    private readonly List<Func<Application, IApplicationComponent>> componentProviders = new();

    public ApplicationBuilder()
    {

    }

    public ApplicationBuilder AddComponent<T>() where T : IApplicationComponentProvider<IApplicationComponent>
    {
        componentProviders.Add(T.CreateComponent);

        return this;
    }

    public ApplicationBuilder WithGraphics<T>() where T : IApplicationComponentProvider<IGraphicsProvider>
    {
        throw new NotImplementedException();
    }

    public ApplicationBuilder WithHost(IApplicationHost host)
    {
        throw new NotImplementedException();
    }

    public Application CreateApplication()
    {
        // var host = hostProvider();

        // Application application = new(host);

        // foreach (var componentProvider in componentProviders)
        // {
        //     var component = componentProvider(application);
        //     application.AddComponent(component);
        // }

        throw new NotImplementedException();
    }
}


interface IWindow : IGraphicsHost
{

}

interface IGraphicsHost
{

}