// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 55d858c912eeece1e3af9c8c95529cb4696dd76f3d0f905e0ef0327deb568769
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IListenerModule : IPipelineModule
    {
        Task<InvokeResult> StartListeningAsync();
        Task<InvokeResult> SendResponseAsync(PipelineExecutionMessage message, OutgoingMessage msg);
    }
}
