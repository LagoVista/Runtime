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
