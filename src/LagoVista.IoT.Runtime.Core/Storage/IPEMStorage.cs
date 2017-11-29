using LagoVista.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{

    public interface IPEMStorage
    {
        Task AddMessageAsync(PipelineExecutionMessage message);

        Task RemoveMessageAsync(String id);

        Task InitAsync(String instanceId);
        Task<PipelineExecutionMessage> GetMessageAsync(String id);
        Task UpdateMessageAsync(PipelineExecutionMessage message);

        Task AddToDeadLetterStorageAsync(PipelineExecutionMessage message);
        Task MoveToDeadLetterStorageAsync(PipelineExecutionMessage message);
    }
}
