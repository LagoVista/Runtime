using LagoVista.IoT.Deployment.Admin.Models;
    using System;
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
