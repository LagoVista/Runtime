// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 58fea774079ce3f0baf655620b9fdd7aa5426f654ced01c23312a37e278acba0
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Services
{
    public interface IInstanceStatusLogger
    {
        Task AddDeploymentInstanceStatusAsync(DeploymentInstanceStatus instanceStatus);
    }
}
