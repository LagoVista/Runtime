// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 64abfb2179cc23c5b414eb6718703d392420e9fb4f7caf3397a6b5fa14912822
// IndexVersion: 2
// --- END CODE INDEX META ---
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
