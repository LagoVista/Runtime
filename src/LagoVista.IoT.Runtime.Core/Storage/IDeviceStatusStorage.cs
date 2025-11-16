// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: a08a580fa7e4cb9bf66b7b911ae9734f2995b03341fb91133bddf5651dd4f57d
// IndexVersion: 2
// --- END CODE INDEX META ---
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
