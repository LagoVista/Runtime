using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        Task<InvokeResult> AddMediaMessageAsync(Stream stream, string contentType, long contentLength, DateTime startTimeStamp, string path, String deviceId = "", String topic = "", Dictionary<string, string> headers = null);
        Task<InvokeResult> AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", string deviceId = "", string topic = "", Dictionary<string, string> headers = null);
	}
}