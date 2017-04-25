using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public abstract class PipelineModule : IPipelineModule
    {
        IPEMQueue _listenerQueue;
        IPEMQueue _outputQueue;
        IPEMBus _pemBus;
        IPipelineModuleConfiguration _pipelineModuleConfiguration;
        List<IPEMQueue> _secondaryOutputQueues;

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue, IPEMQueue outputQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _outputQueue = outputQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost)
        {
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;
        }

        public IPipelineModuleHost ModuleHost { get; private set; }

        public PipelineModuleStatus Status { get; private set; }

        public List<IPEMQueue> OutgoingQueues { get; set; }

        public DateTime CreationDate { get; private set; }

        public int MessagesProcessed { get; private set; }

        public int UnhandledExceptionCount { get; private set; }

        public int ErrorCount { get; private set; }

        public int WarningCount { get; private set; }

        public int InProcessCount { get; private set; }

        public TimeSpan AverageExecutionTimeMS { get; private set; }

        public abstract Task<ProcessResult> ProcessAsync(PipelineExectionMessage message);


        private Task FinalizeMessage(PipelineExectionMessage message)
        {
            return Task.FromResult(default(object));
        }

        protected async Task QueueForNextExecutionAsync(PipelineExectionMessage message)
        {
            await _outputQueue.EnqueueAsync(message);
        }

        private async void ExecuteAsync(PipelineExectionMessage message)
        {
            InProcessCount++;

            var sw = new Stopwatch();
            sw.Start();
            var result = await ProcessAsync(message);
            sw.Stop();            

            InProcessCount--;

            message.ErrorMessages.AddRange(result.ErrorMessages);
            message.InfoMessages.AddRange(result.InfoMessages);
            message.WarningMessages.AddRange(result.WarningMessages);

            message.ExecutionTimeMS += sw.Elapsed.TotalMilliseconds;
            message.CurrentInstruction.ExecutionTimeMS = sw.Elapsed.TotalMilliseconds;
            message.CurrentInstruction.ProcessByHostId = ModuleHost.Id;

            var instructionIndex = message.Instructions.IndexOf(message.CurrentInstruction);
            instructionIndex++;
            if(instructionIndex == message.Instructions.Count)
            {
                PEMBus.Logger.Log(LagoVista.Core.PlatformSupport.LogLevel.Message, "PipelineModule", "Message completed",new KeyValuePair<string, string>("Execution Time (ms)", message.ExecutionTimeMS.ToString()));
            }
            else
            {
                message.CurrentInstruction = message.Instructions[instructionIndex];
                await QueueForNextExecutionAsync(message); 
            }
        }

        private void WorkLoop()
        {
            Task.Run(async () =>
            {
                while(Status == PipelineModuleStatus.Running)
                {
                    var msg = await _listenerQueue.ReceiveAsync();
                    ExecuteAsync(msg);
                }                
            });
        }
        

        public virtual async Task StartAsync()
        {
            CreationDate = DateTime.Now;
            Status = PipelineModuleStatus.Running;
            await _listenerQueue.StartListeningAsync();

            WorkLoop();
        }

        public virtual Task StopAsync()
        {
            Status = PipelineModuleStatus.Idle;
            return _listenerQueue.StopListeningAsync();
        }

        protected IPEMBus PEMBus { get { return _pemBus; } }
    }
}
