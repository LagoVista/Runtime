// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: f124713eaa2869f4832e190c4b27b319e098bfc20b8b28f4aa012099c9e70b42
// IndexVersion: 2
// --- END CODE INDEX META ---
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
