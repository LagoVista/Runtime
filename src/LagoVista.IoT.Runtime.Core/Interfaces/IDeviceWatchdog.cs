using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.Runtime.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IDeviceWatchdog
    {
        /// <summary>
        /// Initialize the Device Watchdog
        /// </summary>
        /// <returns></returns>
        Task InitAsync(IPEMBus pemBus);

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
        /// <param name="timeout">timeout for the watch dog</param>
        /// <returns></returns>
        Task DeviceUpdatedAsync(DeviceManagement.Core.Models.Device device);


        Task<ListResponse<DeviceWatchdogTimedout>> GetLastUpdatedDevicesAsync(ListRequest listRequest);

        Task<ListResponse<DeviceWatchdogTimedout>> GetTimedOutDevicesAsync(ListRequest listRequest);

        Task UpdateAsync();
    }
}
