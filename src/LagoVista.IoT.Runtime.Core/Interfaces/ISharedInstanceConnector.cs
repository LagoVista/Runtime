using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models.Settings;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
	public interface ISharedInstanceConnector
	{
		Task<InvokeResult<DeploymentInstance>> GetInstanceAsync();
		Task<InvokeResult<string>> GetKeyFroMSecureIdAsync(string keyId);
		Task<InvokeResult<LoggingSettings>> GetLoggingSettingsAsync();
		Task<InvokeResult<ConnectionSettings>> GetNotificationSettingsAsync(NotificationServerType notificationServerType);
		Task<InvokeResult<string>> GetSolutionVersionAsync();
		Task<InvokeResult> UpdateHostStatusAsync(string hostId, HostStatus status, string version);
		Task<InvokeResult<ConnectionSettings>> GetUsageStorageSettingsAsync();
	}
}