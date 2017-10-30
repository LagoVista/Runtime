using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IInstanceManager
    {
        HostVersion Version { get; }

        String HostId { get; set; }
        Task InitAsync();
        Task InitAsync(String instanceId);

        DeploymentHost Host { get; }
        ConcurrentDictionary<string, IInstanceHost> ActiveInstances { get; }
        InvokeResult<InstanceRuntimeDetails> GetInstanceDetails(string instanceId);
        Task<InvokeResult> DeployAsync(string instanceId);
        Task<InvokeResult> UpdateAsync(string instanceId);
        Task<InvokeResult> StartAsync(string instanceId);
        Task<InvokeResult> PauseAsync(string instanceId);
        Task<InvokeResult> StopAsync(string instanceId);
        Task<InvokeResult> RemoveAsync(string instanceId);
    }
}
