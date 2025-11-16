// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ce737fbd878735466e51362c036a2ef5f8bcf0b830688dd082fa9bd5dcc3c9f2
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        Task<InvokeResult<string>> StoreMediaItemAsync(Stream mediaStream, string pemId, string contentType, long length, double? latitude, double? longitude);

        Task<InvokeResult<Stream>> GetMediaItemAsync(string uniqueDeviceId, string pemId);

        /// <summary>
        /// First media item id is temporary since we don't know our device id, after we do, then we can assign it 
        /// after it has been attached we have a new id, this will be returned and assigned to the pem media id item.
        /// </summary>
        /// <param name="pemId"></param>
        /// <param name="title"></param>
        /// <param name="id"></param>
        /// <param name="deviceId"></param>
        /// <returns>Newly assigned media id.</returns>
        Task<InvokeResult<string>> AttachToDevice(string pemId, string title, string id, string deviceId);
    }
}
