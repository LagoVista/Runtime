using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IUsageMetricsRepoRemote
    {
        Task AddMetricAsync(UsageMetrics metrics);
    }

    public interface IUsageMetricsRepo : IUsageMetricsRepoRemote
    {
        //TODO: We will need to add rollup capability this is probably a good place for it...or not.
        Task<IEnumerable<UsageMetrics>> GetMetricsForHostAsync(String pipelineModuleId, DateTime? start, DateTime? end);
        Task<IEnumerable<UsageMetrics>> GetMetricsForInstanceAsync(String pipelineModuleId, DateTime? start, DateTime? end);
        Task<IEnumerable<UsageMetrics>> GetMetricsForPipelineModuleAsync(String pipelineModuleId, DateTime? start, DateTime? end);
    }
}
