using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceExceptionStorage
    {
        Task AddDeviceExceptionAsync(DeviceException deviceExcpetion);
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);
    }
}
