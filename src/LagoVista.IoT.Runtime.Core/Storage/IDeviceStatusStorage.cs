using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceStatusStorage
    {
        Task AddDeviceStatusHistoryAsync(DeviceStatus status);
        Task AddCurrentStatusAsync(DeviceStatus status);
        Task<DeviceStatus> GetCurrentStatusAsync(string uniqueDeviceId);
        Task UpdateCurrentStatus(DeviceStatus status);
        Task<List<DeviceStatus>> GetDeviceStatusHistoryByUniqueDeviceIdAsync(string id);
        Task<List<DeviceStatus>> GetAllDevicesAsync();
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);
    }
}
