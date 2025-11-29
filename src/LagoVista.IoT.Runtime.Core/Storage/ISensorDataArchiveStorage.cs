// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e3cd1d583602c3dffafc73553a9bf2f7bd5e8537504e07bf91747f89da382795
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface ISensorDataArchiveStorage
    {
        Task<InvokeResult> InitAsync(DeviceRepository deviceRepo, string hostId, string instanceId);

        UsageMetrics GetAndResetMetrics(DateTime dateStamp, string hostVersion);
        Task AddArchiveAsync(SensorDataArchive archive);

        string Id { get; }
    }
}
