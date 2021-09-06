using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface ISensorEvaluator
    {
        Task EvaluateAsync(Device device);
    }
}
