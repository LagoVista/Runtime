// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: eca247f4bb48d01619dc9bc5a258cb63cf02ab036de3f9410d572927cb1764aa
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Runtime.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IInstanceRuntime : IDisposable
    {

        event EventHandler<DeploymentInstanceStates> StateChanged;
        string Id { get; }

        Task<InvokeResult> InitAsync(DeploymentInstance instance);

        Task<InvokeResult> CleanupAsync();

        DeploymentInstance Instance { get; }

        IPEMBus PEMBus { get; }

        IFallbackMessageHandler FallbackMessageHandler { get; }

        InstanceRuntimeSummary CreateSummary();

        void PopulateInstanceDetails(InstanceRuntimeDetails instanceDetails);

        Task<InvokeResult> PauseAsync();
        Task<InvokeResult> StartListeningAsync();
        Task<InvokeResult> StopListeningAsync();
        Task<bool> CheckAllQueuesEmpty();
        Task<InvokeResult> StopAsync();
        Task<InvokeResult> StartAsync();
        Task<UsageMetrics> GetAndResetMetricsAsync(DateTime dateStamp, string hostVersion);
        InstanceRuntimeDetails GetInstanceDetails();
	}
}