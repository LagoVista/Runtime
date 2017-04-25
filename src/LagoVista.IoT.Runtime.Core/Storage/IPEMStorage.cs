using LagoVista.IoT.Runtime.Core.Models.PEM;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IPEMStorage
    {
        Task AddMessage(PipelineExectionMessage message);

        Task InitAsync(String instanceId);
        Task<PipelineExectionMessage> GetMessage(String id);
        Task UpdateMessage(PipelineExectionMessage message);
    }
}
