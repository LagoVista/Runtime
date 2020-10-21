using LagoVista.Core.Validation;
using System.Threading.Tasks;
using LagoVista.IoT.DeviceManagement.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Core.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceConnectionEventStorage : IDeviceConnectionEventRepo
    {
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);
        Task<InvokeResult> AddDeviceEventConnectionEvent(DeviceConnectionEvent connectionEvent);
    }
}
