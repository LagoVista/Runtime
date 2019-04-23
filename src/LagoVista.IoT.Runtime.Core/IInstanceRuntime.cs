using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Runtime.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IInstanceRuntime : IDisposable
    {

        event EventHandler<DeploymentInstanceStates> StateChanged;
        String Id { get; }
        
        Task<InvokeResult> InitAsync(DeploymentInstance instance);

        Task<InvokeResult> CleanupAsync();

        DeploymentInstance Instance { get;  }

        InstanceRuntimeSummary CreateSummary();

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
