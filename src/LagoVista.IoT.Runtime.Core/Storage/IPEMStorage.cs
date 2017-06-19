using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IPEMStorage
    {
        Task AddMessageAsync(PipelineExectionMessage message);

        Task RemoveMessageAsync(String id);

        Task InitAsync(String instanceId);
        Task<PipelineExectionMessage> GetMessageAsync(String id);
        Task UpdateMessageAsync(PipelineExectionMessage message);

        Task MoveToDeadLetterStorageAsync(string id);
    }
}
