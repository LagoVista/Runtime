using LagoVista.Core.Interfaces;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Runtime.Core.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IWatchdog
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
        Task DeviceUpdatedAsync(DeviceManagement.Core.Models.Device device, TimeSpan timeout);
    }
}
