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

        Task<List<DeviceSummary>> GetDevicesForConfiugrationAsync(string deviceConfigurationid);

        Task<List<DeviceSummary>> GetDevicesForDevicesByDeviceGroupKeyAsync(string deviceGroupKey);

        Task UpdateDeviceAsync(Device device);

        string Id { get; }       
        string ReadMetricsId { get; }
        string WriteMetricsId { get; }
    }
}
