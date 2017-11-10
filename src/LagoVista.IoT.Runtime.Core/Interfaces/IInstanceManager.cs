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
        /* Used for starring out a host  with many instances */
        Task<InvokeResult> InitAsync();
        /* Used for starting out a host with only once instance */
        Task<InvokeResult> InitAsync(String instanceId);

        DeploymentHost Host { get; }
        ConcurrentDictionary<string, IInstanceRuntime> ActiveInstances { get; }
        InvokeResult<InstanceRuntimeDetails> GetInstanceDetails(string instanceId);
        Task<InvokeResult> DeployAsync(string instanceId, string versionId);
        Task<InvokeResult> UpdateAsync(string instanceId, string versionId);
        Task<InvokeResult> StartAsync(string instanceId);
        Task<InvokeResult> PauseAsync(string instanceId);
        Task<InvokeResult> StopAsync(string instanceId);
        Task<InvokeResult> RemoveAsync(string instanceId);
    }
}
