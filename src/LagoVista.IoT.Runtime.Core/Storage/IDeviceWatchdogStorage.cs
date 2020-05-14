using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Deployment.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceWatchdogStorage
    {
        Task UpdateDeviceAsync(Device device, TimeSpan timeout);

        Task ResetAsync();

        Task<IEnumerable<WatchdogConnectedDevice>> GetTimedOutDevicesAsync();

        List<WatchdogConnectedDevice> TrackedDevices { get; }

        Task MarkAsNotifiedAsync(String id);
    }
}
