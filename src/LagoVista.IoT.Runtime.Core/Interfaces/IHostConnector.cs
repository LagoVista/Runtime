using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models;
using LagoVista.IoT.Deployment.Models.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
	public interface IHostConnector : IUsageMetricsSettingsProvider, ILogWriterSettingsProvider, INotificationConnectionProvider, IRPCSettingsProvider, IEHCheckPointSettingsProvider
	{
		Task<InvokeResult<DeploymentHost>> GetHostAsync();
		Task<InvokeResult<List<SharedInstanceSummary>>> GetInstanceAsync();
		Task<InvokeResult> UpdateHostStatusAsync(string hostId, HostStatus status, string version);
	}
}