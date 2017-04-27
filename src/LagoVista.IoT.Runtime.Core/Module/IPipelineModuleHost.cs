using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{    
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
