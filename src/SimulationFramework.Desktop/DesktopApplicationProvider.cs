using SimulationFramework.Components;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.Desktop;
internal class DesktopApplicationProvider : IApplicationProvider
{
    public IDisplay PrimaryDisplay => GetDisplays().Single(d => d.IsPrimary);

    private bool isExiting;
    private bool exitCancelled;

    public void CancelExit()
    {
        if (!isExiting)
            throw new InvalidOperationException("The simulation is not currently exiting.");

        exitCancelled = true;
    }

    public void Exit(bool cancellable)
    {
        isExiting = true;
        SimulationHost.Current.Dispatcher.ImmediateDispatch<ExitMessage>(new(cancellable));
        isExiting = false;

        if (cancellable && exitCancelled)
            return;

        Application.GetComponent<DesktopSimulationController>().NotifySuccessfulExit();
    }

    public IEnumerable<IDisplay> GetDisplays()
    {
        return DesktopDisplay.GetDisplayList();
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
    }

    public void Dispose()
    {
    }
}
