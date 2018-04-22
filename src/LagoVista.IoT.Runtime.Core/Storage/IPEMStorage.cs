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
