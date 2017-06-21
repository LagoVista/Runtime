using System;
using System.Collections.Generic;
using System.Linq;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.Core.Models;

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
                PayloadType = EntityHeader<MessagePayloadTypes>.Create(MessagePayloadTypes.Binary),
                BinaryPayload = buffer,
                CreationTimeStamp = startTimeStamp.ToJSONString()
            };

            if (buffer != null)
            {
                message.PayloadLength = buffer.Length;
            }

            var listenerInstruction = new PipelineExectionInstruction()
            {
                Name = _pipelineModuleConfiguration.Name,
                Type = GetType().Name,
                QueueId = "N/A",
                StartDateStamp = startTimeStamp.ToJSONString(),
                ProcessByHostId = ModuleHost.Id,
                ExecutionTimeMS = (DateTime.UtcNow - startTimeStamp).TotalMilliseconds,
            };

            message.Instructions.Add(listenerInstruction);

            var planner = PEMBus.Instance.Solution.Value.Planner.Value;
            var plannerInstruction = new PipelineExectionInstruction()
            {
                Name = "Planner",
                Type = "Planner",
                QueueId = "N/A",
            };

            message.CurrentInstruction = plannerInstruction;
            message.Instructions.Add(plannerInstruction);

            await PEMBus.PEMStorage.AddMessageAsync(message);

            await _plannerQueue.EnqueueAsync(message);
        }

        public async Task AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", Dictionary<string, string> headers = null)
        {
            var message = new PipelineExectionMessage()
            {
                PayloadType = EntityHeader<MessagePayloadTypes>.Create(MessagePayloadTypes.Text),
                TextPayload = buffer,
                CreationTimeStamp = startTimeStamp.ToJSONString()
            };

            if (buffer != null)
            {
                message.PayloadLength = buffer.Length;
            }

            message.Envelope.Path = path;

            if (headers != null)
            {
                foreach (var hdr in headers)
                {
                    message.Envelope.Headers.Add(hdr.Key, hdr.Value);
                }
            }

            var listenerInstruction = new PipelineExectionInstruction()
            {
                Name = _pipelineModuleConfiguration.Name,
                Type = GetType().Name,
                QueueId = "N/A",
                StartDateStamp = startTimeStamp.ToJSONString(),
                ProcessByHostId = ModuleHost.Id,
                ExecutionTimeMS = (DateTime.UtcNow - startTimeStamp).TotalMilliseconds,
            };

            message.Instructions.Add(listenerInstruction);

            var planner = PEMBus.Instance.Solution.Value.Planner.Value;
            var plannerInstruction = new PipelineExectionInstruction()
            {
                Name = "Planner",
                Type = "Planner",
                QueueId = "N/A",
            };

            message.CurrentInstruction = plannerInstruction;
            message.Instructions.Add(plannerInstruction);

            await PEMBus.PEMStorage.AddMessageAsync(message);

            await _plannerQueue.EnqueueAsync(message);
        }
    }
}
