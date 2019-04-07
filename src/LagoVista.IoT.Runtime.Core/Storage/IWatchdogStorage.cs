using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IWatchdogStorage
    {
        Task UpdateDeviceAsync(Device device, TimeSpan timeout);

        Task ResetAsync();

        Task<IEnumerable<DeviceWatchdogTimedout>> GetTimedOutDevicesAsync();

        List<DeviceWatchdogTimedout> TrackedDevices { get; }

        Task MarkAsNotifiedAsync(String id);
    }
}
