// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3528e87a38b8cd45bde00b0d6a0475d4c9549045ed14db0290fca6b7fe75518d
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using System.Threading.Tasks;
using LagoVista.IoT.DeviceManagement.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Core.Repos;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceConnectionEventStorage : IDeviceConnectionEventRepo
    {
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);
        Task<InvokeResult> AddDeviceEventConnectionEvent(DeviceConnectionEvent connectionEvent);
    }
}
