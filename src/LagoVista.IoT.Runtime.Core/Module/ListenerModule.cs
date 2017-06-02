using System;
using System.Collections.Generic;
using System.Linq;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public abstract class ListenerModule : PipelineModule
    {
        IPipelineModuleConfiguration _pipelineModuleConfiguration;
        IPEMQueue _plannerQueue;

        public ListenerModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue plannerQueue) : base(pipelineModuleConfiguration, pemBus, moduleHost)
        {
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _plannerQueue = plannerQueue;
        }

        public async Task AddBinaryMessageAsync(byte[] buffer, DateTime startTimeStamp)
        {
            //PEMBus.Logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, this.GetType().Name, "Received byte " + buffer.Length);

            var message = new PipelineExectionMessage()
            {
                PayloadType = MessagePayloadTypes.Binary
            };

            message.BinaryPayload = buffer;
        
            var listenerInstruction = new PipelineExectionInstruction() { };
            listenerInstruction.StartDateStamp = startTimeStamp.ToJSONString();
            listenerInstruction.ProcessByHostId = ModuleHost.Id;
            listenerInstruction.ExecutionTimeMS = (DateTime.Now.ToUniversalTime() - startTimeStamp).TotalMilliseconds;
            message.Instructions.Add(listenerInstruction);

            var planner = PEMBus.Instance.Solution.Value.Planner.Value;
            var plannerInstruction = new PipelineExectionInstruction() { };
            message.Instructions.Add(new PipelineExectionInstruction() { QueueName = planner.Key });
            message.CurrentInstruction = plannerInstruction;
            message.Instructions.Add(plannerInstruction);

            await _plannerQueue.EnqueueAsync(message);
        }

        public async Task AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", Dictionary<string, string> headers = null)
        {
            var message = new PipelineExectionMessage()
            {
                PayloadType = MessagePayloadTypes.Binary,
                TextPayload = buffer
            };

            message.Envelope.Path = path;
            if (headers != null)
            {
                foreach (var hdr in headers)
                {
                    message.Envelope.Headers.Add(hdr.Key, hdr.Value);
                }
            }


            var listenerInstruction = new PipelineExectionInstruction() { };
            listenerInstruction.StartDateStamp = startTimeStamp.ToJSONString();
            listenerInstruction.ProcessByHostId = ModuleHost.Id;
            listenerInstruction.ExecutionTimeMS = (DateTime.Now.ToUniversalTime() - startTimeStamp).TotalMilliseconds;
            message.Instructions.Add(listenerInstruction);

            var planner = PEMBus.Instance.Solution.Value.Planner.Value;
            var plannerInstruction = new PipelineExectionInstruction() { };
            message.Instructions.Add(new PipelineExectionInstruction() { QueueName = planner.Key });
            message.CurrentInstruction = plannerInstruction;
            message.Instructions.Add(plannerInstruction);
            await _plannerQueue.EnqueueAsync(message);
        }
    }
}
