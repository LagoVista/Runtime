using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public enum DeploymentHostStates
    {
        New,
        Initializing,
        Running,
        Paused,
        Stopping,
        Stopped,
        Degraded,
        FatalError,
    }

    public interface IDeploymentHost : IDisposable
    {
        DeploymentHostStates Status { get; }

        string Id { get; }

        Task StartAsync();

        Task PauseAsync();

        Task StopAsync();

        Task<bool> InitAsync(string instanceId);
    }
}
