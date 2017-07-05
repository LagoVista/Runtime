﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;
using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Models;
using LagoVista.IoT.DeviceAdmin.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public enum PipelineModuleStatus
    {
        Idle,
        StartingUp,
        Running,
        Paused,
        ShuttingDown,
    }

    public interface IPipelineModule
    {
        UsageMetrics Metrics { get; }

        IPipelineModuleHost ModuleHost { get; }

        PipelineModuleStatus Status { get; }

        List<IPEMQueue> OutgoingQueues { get; set; }

        Task<InvokeResult> StartAsync();

        Task<InvokeResult> PauseAsync();

        Task<InvokeResult> StopAsync();

        System.DateTime CreationDate { get; }

        UsageMetrics GetAndResetMetrics(DateTime dateStamp);

        Task<ProcessResult> ProcessAsync(PipelineExectionMessage message);

        IPipelineModuleConfiguration Configuration { get; }
    }
}
