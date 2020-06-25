using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IDeviceExceptionHandler
    {
        Task<InvokeResult> HandleDeviceExceptionAsync(Device device, DeviceException exception);
    }
}
