// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3e2b7e0a63047cd1aa88803a04ba11579a92614370da436535678e81a3768d28
// IndexVersion: 2
// --- END CODE INDEX META ---
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
