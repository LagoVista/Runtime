using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models.Settings;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IInstanceConnector
    {
        Task<InvokeResult<DeviceDataStorageSettings>> GetDeviceDataStorageSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetDeviceStorageSettingsAsync();
        Task<InvokeResult<DeploymentInstance>> GetInstanceAsync();
        Task<InvokeResult<string>> GetKeyFroMSecureIdAsync(string keyId);
        Task<InvokeResult<LoggingSettings>> GetLoggingSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetNotificationSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetPEMStorageSettingsAsync();
        Task<InvokeResult<RPCSettings>> GetRPCConnectionAsync();
        Task<InvokeResult<string>> GetSolutionVersionAsync();
        Task<InvokeResult> UpdateInstanceStatusAsync(DeploymentInstanceStates status, bool isDeployed, string version);
        Task<InvokeResult<ConnectionSettings>> GetUsageStorageSettingsAsync();
    }
}
