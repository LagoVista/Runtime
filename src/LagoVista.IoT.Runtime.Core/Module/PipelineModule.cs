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
using LagoVista.IoT.Runtime.Core.Models;
using Newtonsoft.Json;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Globalization;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.Core.Exceptions;

namespace LagoVista.IoT.Runtime.Core.Module
{

    public abstract class PipelineModule : IPipelineModule
    {
        IPEMQueue _listenerQueue;
        IPEMQueue _outputQueue;
        IPEMBus _pemBus;
        IPipelineModuleConfiguration _pipelineModuleConfiguration;
        List<IPEMQueue> _secondaryOutputQueues;
        UsageMetrics _pipelineMetrics;
        string _stateChangeTimeStamp;

        //TODO: SHould condolidate constructors with call to this(....);

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost, IPEMQueue listenerQueue, IPEMQueue outputQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _outputQueue = outputQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost, IPEMQueue listenerQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();

        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost, IPEMQueue listenerQueue)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleRuntime moduleHost)
        {
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, Id);
            _pipelineMetrics.Reset();
        }

        public string Id { get; set; }

        public IPipelineModuleRuntime ModuleHost { get; private set; }

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
            //TODO: This __could__ be a bottle neck, not sure though.
            /* This needs to be VERY, VERY fast since it will block anyone elses access to writing metrics */
            lock (_pipelineMetrics)
            {
                var clonedMetrics = new UsageMetrics()
                {
                    RowKey = actualDataStamp.ToInverseTicksRowKey(),
                    HostId = PEMBus.Instance.Host.Id,
                    InstanceId = PEMBus.Instance.Id,
                    Version = hostVersion,
                    PipelineModuleId = Id,
                    PartitionKey = Id,

                    EndTimeStamp = actualDataStamp.ToJSONString(),
                    StartTimeStamp = _pipelineMetrics.StartTimeStamp,

                    ActiveCount = _pipelineMetrics.ActiveCount,
                    ErrorCount = _pipelineMetrics.ErrorCount,
                    WarningCount = _pipelineMetrics.WarningCount,
                    DeadLetterCount = _pipelineMetrics.DeadLetterCount,
                    BytesProcessed = _pipelineMetrics.BytesProcessed,
                    MessagesProcessed = _pipelineMetrics.MessagesProcessed,
                    ProcessingMS = Math.Round(_pipelineMetrics.ProcessingMS, 4)
                };

                clonedMetrics.ElapsedMS = Math.Round((clonedMetrics.EndTimeStamp.ToDateTime() - clonedMetrics.StartTimeStamp.ToDateTime()).TotalMilliseconds, 3);

                if (clonedMetrics.ElapsedMS > 1)
                {
                    clonedMetrics.MessagesPerSecond = clonedMetrics.MessagesProcessed / (clonedMetrics.ElapsedMS * 1000.0);
                }

                if (clonedMetrics.MessagesProcessed > 0)
                {
                    clonedMetrics.AvergeProcessingMs = Math.Round(clonedMetrics.ProcessingMS / clonedMetrics.MessagesProcessed, 3);
                }

                _pipelineMetrics.Reset(clonedMetrics.EndTimeStamp);
                return clonedMetrics;
            }
        }

        private Task FinalizeMessage(PipelineExecutionMessage message)
        {
            return Task.FromResult(default(object));
        }

        protected async Task QueueForNextExecutionAsync(PipelineExecutionMessage message)
        {
            await _outputQueue.EnqueueAsync(message);
        }

        private async Task UpdateDevice(PipelineExecutionMessage message)
        {
            message.Device.LastContact = message.CreationTimeStamp;
            message.Device.Status = EntityHeader<DeviceStates>.Create(DeviceStates.Ready);
            await PEMBus.DeviceManager.UpdateDeviceAsync(PEMBus.Instance.DeviceRepository.Value, message.Device, PEMBus.SystemUsers.SystemOrg, PEMBus.SystemUsers.DeviceManagerUser);

            var notification = new Notification()
            {
                Payload = JsonConvert.SerializeObject(message.Device),
                Channel = EntityHeader<Channels>.Create(Channels.Device),
                ChannelId = message.Device.Id,
                PayloadType = typeof(Device).ToString(),
                DateStamp = DateTime.UtcNow.ToJSONString(),
                MessageId = Guid.NewGuid().ToId(),
                Text = "Device Updated",
                Title = "Device Updated"
            };


            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);
            await PEMBus.PEMStorage.UpdateMessageAsync(message);
        }

        private async void ExecuteAsync(PipelineExecutionMessage message)
        {
            Metrics.ActiveCount++;

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                message.CurrentInstruction.StartDateStamp = DateTime.UtcNow.ToJSONString();
                var result = await ProcessAsync(message);
                sw.Stop();
                Metrics.ProcessingMS += sw.Elapsed.TotalMilliseconds;
                Metrics.MessagesProcessed++;
                Metrics.BytesProcessed += message.PayloadLength;

                PEMBus.InstanceLogger.AddMetric($"pipeline.{this.GetType().Name.ToLower()}.execution", sw.Elapsed);
                PEMBus.InstanceLogger.AddMetric($"pipeline.{this.GetType().Name.ToLower()}");

                message.ErrorMessages.AddRange(result.ErrorMessages);
                message.InfoMessages.AddRange(result.InfoMessages);
                message.WarningMessages.AddRange(result.WarningMessages);

                message.ExecutionTimeMS += Math.Round(sw.Elapsed.TotalMilliseconds, 3);
                message.CurrentInstruction.ExecutionTimeMS = Math.Round(sw.Elapsed.TotalMilliseconds, 3);
                message.CurrentInstruction.ProcessByHostId = ModuleHost.Id;

                if (result.Success)
                {
                    

                    var instructionIndex = message.Instructions.IndexOf(message.CurrentInstruction);
                    instructionIndex++;
                    if (instructionIndex == message.Instructions.Count) /* We are done processing the pipe line */
                    {
                        message.Status = EntityHeader<StatusTypes>.Create(message.WarningMessages.Count > 0 ? StatusTypes.CompletedWithWarnings : StatusTypes.Completed);
                        message.CurrentInstruction = null;
                        message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();

                        await UpdateDevice(message);
                    }
                    else
                    {
                        if (instructionIndex > message.Instructions.Count) /* Somehow we overrun the index for the pipeline steps */
                        {
                            var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                            LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                            message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex.ToError());
                            message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                            message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                            Metrics.ErrorCount++;
                            Metrics.DeadLetterCount++;
                            await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                            return;
                        }

                        message.CurrentInstruction = message.Instructions[instructionIndex];

                        var nextQueue = PEMBus.Queues.Where(que => que.PipelineModuleId == message.CurrentInstruction.QueueId).FirstOrDefault();
                        if (nextQueue == null) /* We couldn't find the queue for the next step */
                        {
                            var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                            LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                            message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.MissingPipelineQueue.ToError());
                            message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                            message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                            Metrics.ErrorCount++;
                            Metrics.DeadLetterCount++;
                            await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                            return;
                        }

                        await PEMBus.PEMStorage.UpdateMessageAsync(message);
                        await nextQueue.EnqueueAsync(message);
                    }
                }
                else /* Processing Failed*/
                {
                    var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                    LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                    foreach (var err in result.ErrorMessages)
                    {
                        message.ErrorMessages.Add(err);
                    }

                    message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                    Metrics.ErrorCount++;
                    Metrics.DeadLetterCount++;
                    message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                    await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                }
            }
            catch(ValidationException ex)
            {
                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                LogException($"pipeline.{this.GetType().Name.ToLower()}", ex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                message.ErrorMessages.Add(new Error()
                {
                    Message = ex.Message,
                    Details = ex.StackTrace,
                    DeviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN",
                });

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Uncaught Validation Exception in Excution Step " + ex.Message);
                foreach(var err in ex.Errors)
                {
                    Console.WriteLine($"\t{err.ErrorCode} - {err.Message} - {err.Details}");
                }
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();

                message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                Metrics.ErrorCount++;
                Metrics.DeadLetterCount++;
                await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
            }
            catch (Exception ex)
            {
                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                LogException($"pipeline.{this.GetType().Name.ToLower()}", ex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                message.ErrorMessages.Add(new Error()
                {
                    Message = ex.Message,
                    Details = ex.StackTrace,
                    DeviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN",
                });

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Uncaught Exception in Excution Step " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();

                message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                Metrics.ErrorCount++;
                Metrics.DeadLetterCount++;
                await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
            }
            finally
            {
                Metrics.ActiveCount--;
            }
        }

        private void WorkLoop()
        {
            Task.Run(async () =>
            {
                while (Status == PipelineModuleStatus.Running)
                {
                    var msg = await _listenerQueue.ReceiveAsync();

                    /* queue will return a null message when it's "turned off", should probably change the logic to use cancellation tokens, not today though KDW 5/3/2017 */
                    //TODO Use cancellation token rather than return null when queue is no longer listenting.
                    if (msg != null)
                    {
                        ExecuteAsync(msg);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cancelled out " + this.GetType().Name.ToLower() + " " + Status.ToString());
                Console.ResetColor();
            });
        }


        public virtual async Task<InvokeResult> StartAsync()
        {
            if(_listenerQueue == null)
            {
                throw new NullReferenceException("Listener Queue is Null");
            }

            CreationDate = DateTime.Now;
            await StateChanged(PipelineModuleStatus.Running);
            var result = await _listenerQueue.StartListeningAsync();
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
            if (_listenerQueue != null)
            {
                return await _listenerQueue.StopListeningAsync();
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
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, tag, message, newArgs.ToArray());
        }

        protected void LogException(string tag, Exception ex, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            PEMBus.InstanceLogger.AddException(tag, ex, newArgs.ToArray());
        }

        public void LogError(ErrorCode err, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            PEMBus.InstanceLogger.AddError(err, newArgs.ToArray());
        }

        protected void LogVerboseMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Verbose, tag, message, newArgs.ToArray());
        }

        protected void LogWarningMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.WarningCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, tag, message, newArgs.ToArray());
        }

        protected void LogErrorMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Error, tag, message, newArgs.ToArray());
        }

        protected async Task StateChanged(PipelineModuleStatus newState)
        {
            if(newState == Status)
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
                NewState = EntityHeader<PipelineModuleStatus>.Create(Status),
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
            catch(Exception ex)
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

        public IPipelineModuleConfiguration Configuration
        {
            get { return _pipelineModuleConfiguration; }
        }

        public PipelineModuleType ModuleType { get; set; }
        public string CustomModuleType { get; set; }
    }
}
