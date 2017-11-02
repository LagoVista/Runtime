using System;
using System.Collections.Generic;
using System.Linq;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using Newtonsoft.Json;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public abstract class ListenerModule : PipelineModule
    {
        IPipelineModuleConfiguration _pipelineModuleConfiguration;
        IPEMQueue _plannerQueue;

        public ListenerModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost, IPEMQueue plannerQueue) : base(pipelineModuleConfiguration, pemBus, moduleHost)
        {
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _plannerQueue = plannerQueue;
        }

        public async Task<InvokeResult> AddBinaryMessageAsync(byte[] buffer, DateTime startTimeStamp, String deviceId = "", String topic = "")
        {
            try
            {
                var message = new PipelineExecutionMessage()
                {
                    PayloadType = EntityHeader<MessagePayloadTypes>.Create(MessagePayloadTypes.Binary),
                    BinaryPayload = buffer,
                    CreationTimeStamp = startTimeStamp.ToJSONString()
                };

                Metrics.MessagesProcessed++;

                if (buffer != null)
                {
                    message.PayloadLength = buffer.Length;
                }

                var bytesProcessed = message.PayloadLength + (String.IsNullOrEmpty(topic) ? 0 : topic.Length);
                LogMessage("addbinarymessage", "New Message Created.", new KeyValuePair<string, string>("messageid", message.Id));

                message.Envelope.DeviceId = deviceId;
                message.Envelope.Topic = topic;

                var listenerInstruction = new PipelineExecutionInstruction()
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
                var plannerInstruction = new PipelineExecutionInstruction()
                {
                    Name = "Planner",
                    Type = "Planner",
                    QueueId = "N/A",
                };

                message.CurrentInstruction = plannerInstruction;
                message.Instructions.Add(plannerInstruction);

                await PEMBus.PEMStorage.AddMessageAsync(message);

                await _plannerQueue.EnqueueAsync(message);

                return InvokeResult.Success;

            }
            catch (Exception ex)
            {
                PEMBus.InstanceLogger.AddException("ListenerModule_AddBinaryMessageAsync", ex);
                return InvokeResult.FromException("ListenerModule_AddBinaryMessageAsync", ex);
            }
        }

        public abstract Task<InvokeResult> SendResponseAsync(PipelineExecutionMessage message);

        public async Task<InvokeResult> AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", string deviceId = "", string topic = "", Dictionary<string, string> headers = null)
        {
            try
            {
                var message = new PipelineExecutionMessage()
                {
                    PayloadType = EntityHeader<MessagePayloadTypes>.Create(MessagePayloadTypes.Text),
                    TextPayload = buffer,
                    CreationTimeStamp = startTimeStamp.ToJSONString()
                };

                var headerLength = 0;

                if (headers != null)
                {
                    if (headers.ContainsKey("method"))
                    {
                        message.Envelope.Method = headers["method"];
                    }

                    if (headers.ContainsKey("topic"))
                    {
                        message.Envelope.Topic = headers["topic"];

                        foreach (var header in headers)
                        {
                            headerLength += header.Key.Length + (String.IsNullOrEmpty(header.Value) ? 0 : header.Value.Length);
                        }
                    }

                    if (headers != null)
                    {
                        foreach (var hdr in headers)
                        {
                            message.Envelope.Headers.Add(hdr.Key, hdr.Value);
                        }
                    }
                }

                message.PayloadLength = String.IsNullOrEmpty(buffer) ? 0 : buffer.Length;

                var bytesProcessed = message.PayloadLength + (String.IsNullOrEmpty(path) ? 0 : path.Length) + headerLength;

                LogMessage("addtextmessage", "New Message Created.", new KeyValuePair<string, string>("messageid", message.Id), new KeyValuePair<string, string>("bytes", bytesProcessed.ToString()));

                Metrics.BytesProcessed += bytesProcessed;
                Metrics.MessagesProcessed++;

                var json = JsonConvert.SerializeObject(Metrics);
                /*
                Console.WriteLine("LISTENER => " + Id);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(json);
                Console.WriteLine("----------------------------");
                */

                message.Envelope.DeviceId = deviceId;
                message.Envelope.Path = path;
                message.Envelope.Topic = topic;
                
                var listenerInstruction = new PipelineExecutionInstruction()
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
                var plannerInstruction = new PipelineExecutionInstruction()
                {
                    Name = planner.Name,
                    Type = "Planner",
                    QueueId = _plannerQueue.InstanceId,
                };

                message.CurrentInstruction = plannerInstruction;
                message.Instructions.Add(plannerInstruction);

                await PEMBus.PEMStorage.AddMessageAsync(message);
                await _plannerQueue.EnqueueAsync(message);

                return InvokeResult.Success;
            }
            catch (Exception ex)
            {
                PEMBus.InstanceLogger.AddException("ListenerModule_AddStringMessageAsync", ex);
                return InvokeResult.FromException("ListenerModule_AddStringMessageAsync", ex);
            }
        }
    }
}
