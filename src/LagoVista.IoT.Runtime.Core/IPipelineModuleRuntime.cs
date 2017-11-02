using LagoVista.IoT.Deployment.Admin.Models;
    using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPipelineModuleRuntime
    {
        string Id { get; }

        HostTypes HostType { get; }

        Task<UsageMetrics> GetAndResetMetricsAsync(DateTime dateStamp, string hostVersion);
    }
}
