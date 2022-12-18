using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models;
using LagoVista.IoT.Deployment.Models.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
	public interface IHostConnector
	{
		Task<InvokeResult<DeploymentHost>> GetHostAsync();
		Task<InvokeResult<List<SharedInstanceSummary>>> GetInstanceAsync();
		Task<InvokeResult<ConnectionSettings>> GetEHCheckPointSettingsAsync();
		Task<InvokeResult<RPCSettings>> GetRPCConnectionAsync();
		Task<InvokeResult<ConnectionSettings>> GetNotificationSettingsAsync(NotificationServerType notificationServerType);
		Task<InvokeResult<LoggingSettings>> GetLoggingSettingsAsync();
		Task<InvokeResult<ConnectionSettings>> GetUsageStorageSettingsAsync();
	}
}