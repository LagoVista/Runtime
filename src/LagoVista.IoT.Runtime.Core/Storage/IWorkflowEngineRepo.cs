using LagoVista.IoT.Runtime.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IWorkflowEngineRepo
    {
        Task UpdateStateAsync(string deviceRepoId, string deviceId, WorkflowState state);
        Task<WorkflowState> GetStateAsync(string deviceRepoId, string deviceId);
    }
}
