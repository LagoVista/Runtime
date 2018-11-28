using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Services
{
    public interface IStatusUpdateService
    {
        void Init(DeploymentInstance instance);
        Task<InvokeResult> SetInstanceStateAsync(DeploymentInstanceStates state, string details = "");
    }
}
