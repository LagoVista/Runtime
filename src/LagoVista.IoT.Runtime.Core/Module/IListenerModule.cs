using LagoVista.Core.Validation;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IListenerModule : IPipelineModule
    {
        Task<InvokeResult> StartListeningAsync();
    }
}
