// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 403cff465fa1c3d058129823d7f516d11cce675b0aa8bbd1a1bb57c945fa0aa1
// IndexVersion: 2
// --- END CODE INDEX META ---
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
