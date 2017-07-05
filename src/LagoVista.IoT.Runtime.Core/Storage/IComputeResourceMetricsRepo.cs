using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IComputeResourceMetricsRepo
    {
        Task AddMetricAsync(ComputeResourceMetrics metrics);

        //TODO: We will need to add rollup capability this is probably a good place for it...or not.
        Task<IEnumerable<ComputeResourceMetrics>> GetMetricsForHostAsync(String pipelineModuleId, DateTime? start, DateTime? end);
    }
}
