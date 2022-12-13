using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
	public interface IHostConnector
	{
		Task<InvokeResult<DeploymentHost>> GetHostAsync();
		Task<InvokeResult<List<SharedInstanceSummary>>> GetInstanceAsync();
	}
}