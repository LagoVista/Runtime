// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: dff1f25c7d65c9b574a87a9eb07cd745cef24f43e66f8975665cfca7bf95abb4
// IndexVersion: 2
// --- END CODE INDEX META ---
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

        Task<InvokeResult> SetHostStateAsync(HostStatus state, string details = "");
    }
}
