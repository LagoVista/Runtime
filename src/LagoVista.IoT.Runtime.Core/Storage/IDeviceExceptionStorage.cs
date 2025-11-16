// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: cf2baec29096c5d613f029b51e3aae7fa3070e5cf716b14100bed00e613f9747
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceExceptionStorage
    {
        Task AddDeviceExceptionAsync(DeviceException deviceExcpetion);
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);
    }
}
