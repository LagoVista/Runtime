// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2531cf207b9b1dc002e1b4569b68ab6daf315856a7691c2b0386c8fdd70c5fd3
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceStorage
    {
        Task InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);

        UsageMetrics GetAndResetReadMetrics(DateTime dateStamp, string hostVersion);
        UsageMetrics GetAndResetWriteMetrics(DateTime dateStamp, string hostVersion);

        Task<Device> GetDeviceByDeviceIdAsync(string deviceId);
        Task<Device> GetDeviceByIdAsync(string id);
        Task<List<DeviceSummary>> GetDevicesForConfiugrationAsync(string deviceConfigurationid);

        Task<List<DeviceSummary>> GetDevicesForDevicesByDeviceGroupKeyAsync(string deviceGroupKey);

        Task UpdateDeviceAsync(Device device);

        string Id { get; }       
        string ReadMetricsId { get; }
        string WriteMetricsId { get; }
    }
}
