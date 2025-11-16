// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b51422d06fcfb62aca7589b671fc271a1aef6b97046ba3d160d83e774b889401
// IndexVersion: 2
// --- END CODE INDEX META ---
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