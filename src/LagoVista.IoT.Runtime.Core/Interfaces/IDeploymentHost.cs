using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{    
    public interface IDeploymentHost : IDisposable
    {
        DeploymentInstanceStates Status { get; }

        string Id { get; }

        Task<InvokeResult> StartAsync();

        Task<InvokeResult> PauseAsync();

        Task<InvokeResult> StopAsync();

        /// <summary>
        /// To be called upon deployment.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<InvokeResult> InitAsync(string instanceId);

        /// <summary>
        /// To be called when the instances is being removed from the host.
        /// </summary>
        /// <returns></returns>
        Task<InvokeResult> CleanupAsync();
    }
}
