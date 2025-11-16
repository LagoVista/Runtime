// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 7b901b3b704df8c83f0d3e7b3aa06946a71234f477da705d22e217a7359ce8f6
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.Deployment.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IMessageWatchdogStorage
    {
        Task UpdateDeviceAsync(Device device, EntityHeader message, MessageWatchDog messageWatchDog);

        Task ResetAsync();

        Task<IEnumerable<WatchdogMessageStatus>> GetTimedOutMessagesAsync();

        List<WatchdogMessageStatus> TrackedMessages { get; }

        Task MarkAsNotifiedAsync(String deviceUniqueId, string messageId);
    }
}
