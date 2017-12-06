using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDeviceArchiveStorage
    {
        Task InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);

        UsageMetrics GetAndResetMetrics(DateTime dateStamp, string hostVersion);
        Task AddArchiveAsync(DeviceArchive logEntry);

        string Id { get; }
    }
}
