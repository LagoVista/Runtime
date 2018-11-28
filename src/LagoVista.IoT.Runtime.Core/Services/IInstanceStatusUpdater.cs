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
