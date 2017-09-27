using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Pipeline.Admin.Models;

namespace LagoVista.IoT.Runtime.Core.Module
{

    public interface IPipelineModule
    {
        String Id { get; set; }
        UsageMetrics Metrics { get; }

        IPipelineModuleHost ModuleHost { get; }

        PipelineModuleStatus Status { get; }

        String StateChangeTimeStamp { get; }

        List<IPEMQueue> OutgoingQueues { get; set; }

        Task<InvokeResult> StartAsync();

        Task<InvokeResult> PauseAsync();

        Task<InvokeResult> StopAsync();

        System.DateTime CreationDate { get; }

        UsageMetrics GetAndResetMetrics(DateTime dateStamp, string hostVersion);

        Task<ProcessResult> ProcessAsync(PipelineExecutionMessage message);

        IPipelineModuleConfiguration Configuration { get; }

        PipelineModuleType ModuleType { get; set; }
        string CustomModuleType { get; set; }
    }
}
