﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;
using LagoVista.Core.Validation;

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
        IPipelineModuleHost ModuleHost { get; }

        PipelineModuleStatus Status { get; }

        List<IPEMQueue> OutgoingQueues { get; set; }

        Task<InvokeResult> StartAsync();

        Task<InvokeResult> PauseAsync();

        Task<InvokeResult> StopAsync();

        System.DateTime CreationDate { get; }

        int MessagesProcessed { get; }
        int UnhandledExceptionCount { get; }
        int ErrorCount { get; }
        int WarningCount { get; }
        int InProcessCount { get; }
        TimeSpan AverageExecutionTimeMS { get; }

        Task<ProcessResult> ProcessAsync(PipelineExectionMessage message);
    }
}