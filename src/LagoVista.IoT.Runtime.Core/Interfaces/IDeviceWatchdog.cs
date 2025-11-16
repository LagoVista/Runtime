// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 8da1ff9d9b21b5e2940d53d7fdec08fa3bafa59980aa26dde318efb03eba7703
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Deployment.Admin;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IDeviceWatchdog
    {
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
        /// <param name="device">device that has been updated</param>
        /// <returns></returns>
        Task<Device> DeviceUpdatedAsync(Device device);

        Task UpdateAsync();

        /// <summary>
        /// Will check the current list of devices to see fi the devices watch dog is
        /// enabled and that it has not timed out.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<bool> IsDeviceTimedOutAsync(string uniqueDeviceId);
    }
}
