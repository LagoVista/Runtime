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
