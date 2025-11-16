// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 27397427996b35b096c19b2f42d6a2bbc7f3c93580676d09591dea090b96aeab
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{

    public interface IPEMStorage
    {
        UsageMetrics GetAndResetMetrics(DateTime dateStamp, string hostVersion);
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepository, string hostId, string instanceId);
        Task AddMessageAsync(PipelineExecutionMessage message);        
        Task<PipelineExecutionMessage> GetMessageAsync(String deviceId, String messageId);
        Task UpdateMessageAsync(PipelineExecutionMessage message);

        string Id { get; }
    }
}
