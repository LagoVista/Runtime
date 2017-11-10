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

        /// <summary>
        /// The version of the host that is handling the runtime(s)
        /// </summary>
        String HostId { get; set; }

        /// <summary>
        /// Used for starring out a host  with many instances 
        /// </summary>
        /// <returns></returns>
        Task<InvokeResult> InitAsync();

        /// <summary>
        /// Used for starting out a host with only once instance */ 
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<InvokeResult> InitAsync(String instanceId);


        DeploymentHost Host { get; }

        ConcurrentDictionary<string, IInstanceRuntime> ActiveInstances { get; }

        /// <summary>
        /// Return a full spec of the instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        InvokeResult<InstanceRuntimeDetails> GetInstanceDetails(string instanceId);

        /// <summary>
        /// Load the solution into a runtime instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        Task<InvokeResult> InitializeInstance(string instanceId, string versionId = "");


        /// <summary>
        /// Start the instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<InvokeResult> StartAsync(string instanceId);

        /// <summary>
        /// Pause the instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<InvokeResult> PauseAsync(string instanceId);

        /// <summary>
        /// Stop the instance
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        Task<InvokeResult> StopAsync(string instanceId);

        /// <summary>
        /// Reload and restart the solution
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        Task<InvokeResult> ReloadSolutionAsync(string instanceId, string versionId = "");


        Task<InvokeResult> RemoveAsync(string instanceId);
    }
}
