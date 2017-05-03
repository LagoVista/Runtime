using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{    
    public interface IDeploymentHost : IDisposable
    {
        DeploymentIntanceStates Status { get; }

        string Id { get; }

        Task<InvokeResult> StartAsync();

        Task<InvokeResult> PauseAsync();

        Task<InvokeResult> StopAsync();

        Task<InvokeResult> InitAsync(string instanceId);
    }
}
