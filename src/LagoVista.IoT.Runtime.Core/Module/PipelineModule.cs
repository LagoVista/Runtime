using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;
using LagoVista.Core.Validation;
using LagoVista.Core;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.Core.Models;
using LagoVista.IoT.Logging;
using Newtonsoft.Json;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.Core.Exceptions;
using Newtonsoft.Json.Serialization;

namespace LagoVista.IoT.Runtime.Core.Module
{

    public abstract class PipelineModule : IPipelineModule
    {
        IPEMQueue _inputMessageQueue;
        readonly IPEMBus _pemBus;
        readonly IPipelineModuleConfiguration _pipelineModuleConfiguration;
        UsageMetrics _pipelineMetrics;
        string _stateChangeTimeStamp;


        JsonSerializerSettings _camelCaseSettings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus)
        {
            _inputMessageQueue = pemBus.Queues.Where(queue => queue.PipelineModuleId == pipelineModuleConfiguration.Id).FirstOrDefault();
            if (_inputMessageQueue == null) throw new Exception($"Incoming queue for module {pipelineModuleConfiguration.Id} - {pipelineModuleConfiguration.Name} could not be found.");

            ModuleType = _inputMessageQueue.ForModuleType;

            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
             
            _pipelineMetrics = new UsageMetrics(pemBus.Instance.PrimaryHost.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, string routeModuleId, IPEMBus pemBus)
        {            
            _inputMessageQueue = pemBus.Queues.Where(queue => queue.PipelineModuleId == routeModuleId).FirstOrDefault();
            if (_inputMessageQueue == null) throw new Exception($"Incoming queue for module {pipelineModuleConfiguration.Id} - {pipelineModuleConfiguration.Name} could not be found.");

            ModuleType = _inputMessageQueue.ForModuleType;

            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.PrimaryHost.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();
        }

        public string Id { get; set; }

        //public IPipelineModuleRuntime ModuleHost { get; private set; }

        public PipelineModuleStatus Status { get; private set; }

        public List<IPEMQueue> OutgoingQueues { get; set; }

        public DateTime CreationDate { get; private set; }

        public abstract Task<ProcessResult> ProcessAsync(PipelineExecutionMessage message);

        public UsageMetrics Metrics
        {
            get
            {
                lock (_pipelineMetrics)
                {
                    return _pipelineMetrics;
                }
            }
        }

        public UsageMetrics GetAndResetMetrics(DateTime actualDataStamp, string hostVersion)
        {
            _pipelineMetrics.Version = hostVersion;
            //TODO: This __could__ be a bottle neck, not sure though.
            /* This needs to be VERY, VERY fast since it will block anyone elses access to writing metrics */
            lock (_pipelineMetrics)
            {
                var clonedMetrics = _pipelineMetrics.Clone() as UsageMetrics;
                clonedMetrics.RowKey = actualDataStamp.ToInverseTicksRowKey();
                clonedMetrics.PartitionKey = Id;

                clonedMetrics.EndTimeStamp = actualDataStamp.ToJSONString();
                clonedMetrics.StartTimeStamp = _pipelineMetrics.StartTimeStamp;
                clonedMetrics.Status = this.Status.ToString();

                clonedMetrics.Calculate();

                _pipelineMetrics.Reset(clonedMetrics.EndTimeStamp);
                return clonedMetrics;
            }
        }

        private Task FinalizeMessage(PipelineExecutionMessage message)
        {
            return Task.FromResult(default(object));
        }

        private async Task UpdateDevice(PipelineExecutionMessage message)
        {
            message.Device.LastContact = message.CreationTimeStamp;
            message.Device.Status = EntityHeader<DeviceStates>.Create(DeviceStates.Ready);
            await PEMBus.DeviceStorage.UpdateDeviceAsync(message.Device);

            var json = JsonConvert.SerializeObject(Models.DeviceForNotification.FromDevice(message.Device), _camelCaseSettings);
            var notification = new Notification()
            {
                Payload = json,
                Channel = EntityHeader<Channels>.Create(Channels.Device),
                ChannelId = message.Device.Id,
                PayloadType = "Device",
                DateStamp = DateTime.UtcNow.ToJSONString(),
                MessageId = Guid.NewGuid().ToId(),
                Text = "Device Updated",
                Title = "Device Updated"
            };

            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);

            notification = new Notification()
            {
                Payload = json,
                Channel = EntityHeader<Channels>.Create(Channels.DeviceRepository),
                ChannelId = message.Device.DeviceRepository.Id,
                PayloadType = "Device",
                DateStamp = DateTime.UtcNow.ToJSONString(),
                MessageId = Guid.NewGuid().ToId(),
                Text = "Device Updated",
                Title = "Device Updated"
            };

            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);

            foreach(var group in message.Device.DeviceGroups)
            {
                notification = new Notification()
                {
                    Payload = json,
                    Channel = EntityHeader<Channels>.Create(Channels.DeviceGroup),
                    ChannelId = group.Id,
                    PayloadType = "Device",
                    DateStamp = DateTime.UtcNow.ToJSONString(),
                    MessageId = Guid.NewGuid().ToId(),
                    Text = "Device Updated",
                    Title = "Device Updated"
                };

                await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);
            }
        }

        protected async Task ExecuteAsync(PipelineExecutionMessage message)
        {
            Metrics.ActiveCount++;
            var sw = new Stopwatch();

            try
            {
                if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Start processing {this.Configuration.Name}");

                var currentInstruction = message.Instructions.Where(pm => pm.QueueId == message.CurrentInstruction.QueueId).FirstOrDefault();

                sw.Start();
                currentInstruction.StartDateStamp = DateTime.UtcNow.ToJSONString();
                var result = await ProcessAsync(message);
                sw.Stop();

                currentInstruction.ExecutionTimeMS = Math.Round(sw.Elapsed.TotalMilliseconds, 3);;
                message.ExecutionTimeMS += currentInstruction.ExecutionTimeMS;
                currentInstruction.ProcessByHostId = PEMBus.Instance.Id;

                Metrics.BytesProcessed += message.PayloadLength;

                message.ErrorMessages.AddRange(result.ErrorMessages);
                message.InfoMessages.AddRange(result.InfoMessages);
                message.WarningMessages.AddRange(result.WarningMessages);
                
                if (result.Success)
                {
                    if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Success processing {this.Configuration.Name}");

                    var instructionIndex = message.Instructions.IndexOf(currentInstruction);
                    instructionIndex++;

                    if (instructionIndex == message.Instructions.Count) /* We are done processing the pipe line */
                    {
                        message.Status = message.WarningMessages.Count > 0 ? StatusTypes.CompletedWithWarnings : StatusTypes.Completed;
                        message.CurrentInstruction = null;
                        message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                        //For now since we are working on the same instance we don't have to write this to external storage while enqueing, will need to when we run on different nodes

                        await UpdateDevice(message);
                        await PEMBus.PEMStorage.AddMessageAsync(message);

                        if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Completed processing {this.Configuration.Name}");
                    }
                    else
                    {
                        if (instructionIndex > message.Instructions.Count) /* Somehow we overrun the index for the pipeline steps */
                        {
                            var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                            LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, message.Id.ToKVP("pemId"), deviceId.ToKVP("deviceId"));
                            message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex.ToError());
                            message.Status = StatusTypes.Failed;
                            message.ErrorReason = ErrorReason.UnexepctedError;
                            message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                            message.CurrentInstruction = null;
                            Metrics.ErrorCount++;
                            Metrics.DeadLetterCount++;
                            await PEMBus.PEMStorage.AddMessageAsync(message);

                            if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Invalid Message Index {this.Configuration.Name}");
                        }
                        else
                        {
                            //Find and set the next instruction.
                            message.CurrentInstruction = message.Instructions[instructionIndex];
                            
                            var nextQueue = PEMBus.Queues.Where(que => que.PipelineModuleId == message.CurrentInstruction.QueueId).FirstOrDefault();
                            if (nextQueue == null) /* We couldn't find the queue for the next step */
                            {
                                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                                LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, message.Id.ToKVP("pemId"), deviceId.ToKVP("deviceId"));
                                message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.MissingPipelineQueue.ToError());
                                message.Status = StatusTypes.Failed;
                                message.ErrorReason = ErrorReason.UnexepctedError;
                                message.CurrentInstruction = null;
                                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                                Metrics.DeadLetterCount++;
                                await PEMBus.PEMStorage.AddMessageAsync(message);

                                if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Could not find next message {this.Configuration.Name}");
                            }
                            else
                            {
                                if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Enqueued for next {this.Configuration.Name}");

                                message.CurrentInstruction.Enqueued = DateTime.UtcNow.ToJSONString();
                                await nextQueue.EnqueueAsync(message);
                            }
                        }
                    }
                }
                else /* Processing Failed*/
                {
                    if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Failed processing {this.Configuration.Name} - {message.ErrorReason}");

                    var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";

                    if (message.ErrorReason == ErrorReason.None)
                    {
                        message.ErrorReason = ErrorReason.SeeErrorLog;
                    }

                    message.ExecutionTimeMS += Math.Round(sw.Elapsed.TotalMilliseconds, 3);
                    message.Status = StatusTypes.Failed;
                    Metrics.ErrorCount++;
                    Metrics.DeadLetterCount++;
                    message.CurrentInstruction = null;
                    message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                    await PEMBus.PEMStorage.AddMessageAsync(message);
                }
            }
            catch (ValidationException ex)
            {
                if (sw.IsRunning) sw.Stop();

                if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Failed processing {this.Configuration.Name} - Validation Exception {ex.Message}");

                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                LogException($"pipeline.{this.GetType().Name.ToLower()}", ex, message.Id.ToKVP("pemId"), deviceId.ToKVP("deviceId"), (string.IsNullOrEmpty(message.MessageId) ? "????".ToKVP("messageId") : message.MessageId.ToKVP("messageId")));
                message.ErrorMessages.Add(new Error()
                {
                    Message = ex.Message,
                    Details = ex.StackTrace,
                    DeviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN",
                });

                message.CurrentInstruction = null;
                message.ErrorReason = ErrorReason.UnexepctedError;
                message.Status = StatusTypes.Failed;
                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                Metrics.DeadLetterCount++;
                await PEMBus.PEMStorage.AddMessageAsync(message);
            }
            catch (Exception ex)
            {
                if (sw.IsRunning) sw.Stop();

                if (message.Device != null && message.Device.DebugMode) SendDeviceNotification(Targets.WebSocket, message.Device.Id, $"Failed processing {this.Configuration.Name} - Exception {ex.Message}");

                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                message.CurrentInstruction = null;                

                message.ErrorMessages.Add(new Error()
                {
                    Message = ex.Message,
                    Details = ex.StackTrace,
                    DeviceId = deviceId,
                });

                message.ErrorReason = ErrorReason.UnexepctedError;
                message.Status = StatusTypes.Failed;
                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                Metrics.DeadLetterCount++;
                await PEMBus.PEMStorage.AddMessageAsync(message);
                LogException($"pipeline.{this.GetType().Name.ToLower()}", ex, message.Id.ToKVP("pemid"), deviceId.ToKVP("deviceId"), (string.IsNullOrEmpty(message.MessageId) ? "????".ToKVP("messageId") : message.MessageId.ToKVP("messageId")));
            }
            finally
            {
                Metrics.ProcessingMS += sw.Elapsed.TotalMilliseconds;                

                Metrics.MessagesProcessed++;
                Metrics.ActiveCount--;
            }
        }

        protected void Execute(PipelineExecutionMessage pem)
        {
            Task.Run(async () =>
            {
                await ExecuteAsync(pem);
            });
        }

        private void WorkLoop()
        {
            Task.Run(async () =>
            {
                while (Status == PipelineModuleStatus.Running)
                {
                    var msg = await _inputMessageQueue.ReceiveAsync();
                    /* queue will return a null message when it's "turned off", should probably change the logic to use cancellation tokens, not today though KDW 5/3/2017 */
                    //TODO Use cancellation token rather than return null when queue is no longer listenting.
                    if (msg != null)
                    {
                        Execute(msg);
                    }
                }
            });
        }


        public virtual async Task<InvokeResult> StartAsync()
        {
            /* ACME Listeners are dedicated port 80 listeners that only listen for very special requests to verify domain ownership
             * if we have a port 80 listener in addition to the AcmeListener, it will not be an AcmeListener and should have a
             * a listener queue */
            if (_inputMessageQueue == null)
            {
                throw new NullReferenceException("Input Message Queue is Null");
            }

            CreationDate = DateTime.Now;
            await StateChanged(PipelineModuleStatus.Running);
            var result = await _inputMessageQueue.StartListeningAsync();

            if (result.Successful)
            {
                WorkLoop();
            }

            return result;
        }

        public async virtual Task<InvokeResult> PauseAsync()
        {
            await StateChanged(PipelineModuleStatus.Paused);
            return InvokeResult.Success;
        }

        public async virtual Task<InvokeResult> StopAsync()
        {
            if (_inputMessageQueue != null)
            {
                var queueStopResult = await _inputMessageQueue.StopListeningAsync();
                await StateChanged(PipelineModuleStatus.Idle);
                return queueStopResult;
            }
            else
            {
                await StateChanged(PipelineModuleStatus.Idle);
                return InvokeResult.Success;
            }
        }

        protected IPEMBus PEMBus { get { return _pemBus; } }

        protected void LogMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, tag, message, newArgs.ToArray());
        }

        protected void LogException(string tag, Exception ex, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddException(tag, ex, newArgs.ToArray());
        }

        public void LogError(ErrorCode err, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddError(err, newArgs.ToArray());
        }

        protected void LogVerboseMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Verbose, tag, message, newArgs.ToArray());
        }

        protected void LogWarningMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.WarningCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, tag, message, newArgs.ToArray());
        }

        protected void LogErrorMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleName", Configuration.Name));
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleType", ModuleType.ToString()));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Error, tag, message, newArgs.ToArray());
        }

        protected async Task StateChanged(PipelineModuleStatus newState)
        {
            if (newState == Status)
            {
                return;
            }

            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.StateChange, $"StatusChange: {GetType().Name}", "statusChange",
                new KeyValuePair<string, string>("oldState", Status.ToString()),
                new KeyValuePair<string, string>("newState", newState.ToString()),
                new KeyValuePair<string, string>("pipelineModuleId", Id));

            var stateChangeNotification = new StateChangeNotification()
            {
                OldState = EntityHeader<PipelineModuleStatus>.Create(Status),
                NewState = EntityHeader<PipelineModuleStatus>.Create(newState),
            };

            var msg = new Notification()
            {
                Payload = JsonConvert.SerializeObject(stateChangeNotification),
                PayloadType = typeof(StateChangeNotification).Name,
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.PipelineModule),
                ChannelId = Id,
                Text = $"Status Change from {stateChangeNotification.OldState.Text} to {stateChangeNotification.NewState.Text}"
            };
            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, msg);

            StateChangeTimeStamp = DateTime.Now.ToJSONString();

            Status = newState;
        }

        protected async Task SendChangeStateMessageAsync(string msg, PipelineModuleStatus newState)
        {
            try
            {
                SendNotification(Runtime.Core.Services.Targets.WebSocket, msg);
                LogMessage(this.GetType().Name, msg);
                await StateChanged(newState);
            }
            catch (Exception ex)
            {
                LogException("PipelineModule_SendChangeStateMessageAsync", ex);
            }
        }

        public String StateChangeTimeStamp
        {
            get { return _stateChangeTimeStamp; }
            set { _stateChangeTimeStamp = value; }
        }

        protected async void SendNotification<TPayload>(Targets target, String text, TPayload payload)
        {
            var msg = new Notification()
            {
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = typeof(TPayload).Name,
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.PipelineModule),
                ChannelId = Id,
                Text = text
            };
            await PEMBus.NotificationPublisher.PublishAsync(target, msg);

            msg = new Notification()
            {
                Payload = JsonConvert.SerializeObject(payload),
                PayloadType = typeof(TPayload).Name,
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.Instance),
                ChannelId = PEMBus.Instance.Id,
                Text = text
            };
            await PEMBus.NotificationPublisher.PublishAsync(target, msg);
        }

        protected async void SendNotification(Targets target, String text)
        {
            var msg = new Notification()
            {
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.PipelineModule),
                ChannelId = Id,
                Text = text
            };
            await PEMBus.NotificationPublisher.PublishAsync(target, msg);

            msg = new Notification()
            {
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.Instance),
                ChannelId = PEMBus.Instance.Id,
                Text = text
            };
            await PEMBus.NotificationPublisher.PublishAsync(target, msg);
        }

        protected async void SendDeviceNotification(Targets target, string deviceId, String text)
        {
            var msg = new Notification()
            {
                Channel = EntityHeader<Services.Channels>.Create(Services.Channels.Device),
                ChannelId = deviceId,
                Text = text
            };
            await PEMBus.NotificationPublisher.PublishAsync(target, msg);
        }


        public IPipelineModuleConfiguration Configuration
        {
            get { return _pipelineModuleConfiguration; }
        }

        public PipelineModuleType ModuleType { get; set; }
        public string CustomModuleType { get; set; }
    }
}
