// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: de88368e9ca788d1aa36aaaffe3a90f2d6b20bc738bde1fd841cc05ad034b39b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IComputeResourceMetricsRepoRemote
    {
        Task AddMetricAsync(ComputeResourceMetrics metrics);
    }

    public interface IComputeResourceMetricsRepo : IComputeResourceMetricsRepoRemote
    {
        //TODO: We will need to add rollup capability this is probably a good place for it...or not.
        Task<IEnumerable<ComputeResourceMetrics>> GetMetricsForHostAsync(String pipelineModuleId, DateTime? start, DateTime? end);
    }
}
