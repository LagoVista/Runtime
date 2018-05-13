using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    /* the class that implements this interface is responsible for initially storing the image file, then once we complete processing on the message
     * move the pointer to point at the device rather than the PEM */
    public interface IDeviceMediaStorage
    {
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);

        /// <summary>
        /// Store a media item associated with a device.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mediaStream"></param>
        /// <param name="pemId"></param>
        /// <param name="contentType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        Task<InvokeResult<string>> StoreMediaItemAsync(DeviceRepository repo, Stream mediaStream, string pemId, string contentType, long length);

        Task<InvokeResult> AttachToDevice(DeviceRepository repo, string pemId, string id, string deviceId);
    }
}
