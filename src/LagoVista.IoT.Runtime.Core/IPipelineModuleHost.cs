using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPipelineModuleHost
    {
        string Id { get; }

        HostTypes HostType { get; }

        Task<UsageMetrics> GetAndResetMetricsAsync(DateTime dateStamp);
    }
}
