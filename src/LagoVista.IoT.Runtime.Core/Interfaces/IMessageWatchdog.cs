using LagoVista.IoT.Deployment.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IMessageWatchdog
    {
        /// <summary>
        /// Initialize the message watch dog with d
        /// </summary>
        /// <param name="deviceConfigs"></param>
        /// <returns></returns>
        Task InitAsync(IEnumerable<DeviceConfiguration> deviceConfigs);

        /// <summary>
        /// Reset the watch dog timer
        /// </summary>
        /// <returns></returns>
        Task ResetAsync();

        /// <summary>
        /// Start the watch dog timer, this will also clear any existing devices.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// Stop the watch dog timer.
        /// </summary>
        /// <returns></returns>

        Task StopAsync();

        /// <summary>
        /// This method should be called whenever a device has been updated
        /// </summary>
        /// <param name="device">device that has been updated.</param>
        /// <param name="messageId">message id as identified in the message.</param>
        /// <returns></returns>
        Task MessageProcessedAsync(DeviceManagement.Core.Models.Device device, string messageId);

        Task UpdateAsync();

    }
}
