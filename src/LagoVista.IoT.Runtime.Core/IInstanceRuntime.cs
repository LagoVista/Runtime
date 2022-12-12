using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
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