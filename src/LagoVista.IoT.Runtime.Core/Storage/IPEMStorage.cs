using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{

    public interface IPEMStorage
    {
        Task InitAsync(DeviceRepository deviceRepository);
        Task AddMessageAsync(PipelineExecutionMessage message);        
        Task<PipelineExecutionMessage> GetMessageAsync(String deviceId, String messageId);
        Task UpdateMessageAsync(PipelineExecutionMessage message);
    }
}
