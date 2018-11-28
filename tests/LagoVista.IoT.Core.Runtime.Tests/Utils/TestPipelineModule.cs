using LagoVista.IoT.Runtime.Core.Module;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Runtime.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;

namespace LagoVista.IoT.Core.Runtime.Tests.Utils
{
    public class TestPipelineModule : PipelineModule
    {
        public TestPipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost, 
            IPEMQueue listenerQueue, IPEMQueue outputQueue, List<IPEMQueue> secondaryOutputQueues) : base(pipelineModuleConfiguration, pemBus)
        {
            Id = "mypipelinemoduleid";
            ResultToReturn = new ProcessResult()
            {
                 
            };

            ProcessHandler = null;
            
        }

        public Func<PipelineExecutionMessage, Task<ProcessResult>> ProcessHandler;

        public ProcessResult ResultToReturn { get; set; }

        public override Task<ProcessResult> ProcessAsync(PipelineExecutionMessage message)
        {
            if (ProcessHandler == null)
            {
                return Task.FromResult(ResultToReturn);
            }
            else
            {
                return ProcessHandler(message);
            }
        }

        public Task PorcessMessageForTestAsync(PipelineExecutionMessage pem)
        {
            return base.ExecuteAsync(pem);
        }
    }
}
