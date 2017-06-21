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

        //TODO: SHould condolidate constructors with call to this(....);

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue, IPEMQueue outputQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _outputQueue = outputQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, pipelineModuleConfiguration.Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue, List<IPEMQueue> secondaryOutputQueues)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            _secondaryOutputQueues = secondaryOutputQueues;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, pipelineModuleConfiguration.Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost, IPEMQueue listenerQueue)
        {
            _listenerQueue = listenerQueue;
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, pipelineModuleConfiguration.Id);
            _pipelineMetrics.Reset();
        }

        public PipelineModule(IPipelineModuleConfiguration pipelineModuleConfiguration, IPEMBus pemBus, IPipelineModuleHost moduleHost)
        {
            _pemBus = pemBus;
            _pipelineModuleConfiguration = pipelineModuleConfiguration;
            ModuleHost = moduleHost;

            _pipelineMetrics = new UsageMetrics(pemBus.Instance.Host.Id, pemBus.Instance.Id, pipelineModuleConfiguration.Id);
            _pipelineMetrics.Reset();
        }

        public IPipelineModuleHost ModuleHost { get; private set; }

        public PipelineModuleStatus Status { get; private set; }

        public List<IPEMQueue> OutgoingQueues { get; set; }

        public DateTime CreationDate { get; private set; }
        
        public abstract Task<ProcessResult> ProcessAsync(PipelineExectionMessage message);


        private UsageMetrics Metrics
        {
            get
            {
                lock (_pipelineMetrics)
                {
                    return _pipelineMetrics;
                }
            }
        }

        public UsageMetrics GetAndResetMetrics()
        {
            //TODO: This __could__ be a bottle neck, not sure though.
            /* This needs to be VERY, VERY fast since it will block anyone elses access to writing metrics */
            lock(_pipelineMetrics)
            {
                var clonedMetrics = new UsageMetrics();
                clonedMetrics.EndTimeStamp = DateTime.UtcNow.ToJSONString();

                clonedMetrics.ActiveCount = _pipelineMetrics.ActiveCount;
                clonedMetrics.ErrorCount = _pipelineMetrics.ErrorCount;
                clonedMetrics.BytesProcessed = _pipelineMetrics.BytesProcessed;
                clonedMetrics.MessagesProcessed = _pipelineMetrics.MessagesProcessed;
                clonedMetrics.ProcessingMS = Math.Round(_pipelineMetrics.ProcessingMS, 4);

                clonedMetrics.ElapsedMS = Math.Round((clonedMetrics.EndTimeStamp.ToDateTime() - clonedMetrics.StartStamp.ToDateTime()).TotalMilliseconds, 3);
                if (clonedMetrics.ElapsedMS > 1)
                {
                    clonedMetrics.MessagesPerSecond = clonedMetrics.MessagesProcessed / (clonedMetrics.ElapsedMS * 1000.0);
                }
                clonedMetrics.WarningCount = _pipelineMetrics.WarningCount;
                if(clonedMetrics.MessagesProcessed > 0)
                {
                    clonedMetrics.AvergeProcessingMs = Math.Round(clonedMetrics.ProcessingMS / clonedMetrics.MessagesProcessed, 3);
                }

                _pipelineMetrics.Reset(clonedMetrics.EndTimeStamp);
                return clonedMetrics;
            }
        }

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
                    if (instructionIndex == message.Instructions.Count)
                    {
                        message.Status = EntityHeader<StatusTypes>.Create(message.WarningMessages.Count > 0 ? StatusTypes.CompletedWithWarnings : StatusTypes.Completed);
                        message.CurrentInstruction = null;
                        message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                        await PEMBus.PEMStorage.UpdateMessageAsync(message);
                    }
                    else
                    {
                        if (instructionIndex > message.Instructions.Count)
                        {
                            var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                            LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                            message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex.ToError());
                            message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                            message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                            Metrics.ErrorCount++;
                            await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                            return;
                        }

                        message.CurrentInstruction = message.Instructions[instructionIndex];

                        var nextQueue = PEMBus.Queues.Where(que => que.PipelineModuleId == message.CurrentInstruction.QueueId).FirstOrDefault();
                        if (nextQueue == null)
                        {
                            var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                            LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                            message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.MissingPipelineQueue.ToError());
                            message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                            message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                            Metrics.ErrorCount++;
                            await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                            return;
                        }

                        await PEMBus.PEMStorage.UpdateMessageAsync(message);
                        await nextQueue.EnqueueAsync(message);
                    }
                }
                else
                {
                    var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                    LogError(Resources.ErrorCodes.PipelineEnqueing.InvalidMessageIndex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                    message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.MissingPipelineQueue.ToError());
                    message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                    Metrics.ErrorCount++;
                    message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                    await PEMBus.PEMStorage.MoveToDeadLetterStorageAsync(message);
                }
            }
            catch (Exception ex)
            {
                var deviceId = message.Device != null ? message.Device.DeviceId : "UNKNOWN";
                LogException($"pipeline.{this.GetType().Name.ToLower()}", ex, new KeyValuePair<string, string>("pemid", message.Id), new KeyValuePair<string, string>("deviceId", deviceId));
                message.ErrorMessages.Add(Resources.ErrorCodes.PipelineEnqueing.MissingPipelineQueue.ToError());
                message.Status = EntityHeader<StatusTypes>.Create(StatusTypes.Failed);
                message.CompletionTimeStamp = DateTime.UtcNow.ToJSONString();
                Metrics.ErrorCount++;
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
            });
        }


        public virtual async Task<InvokeResult> StartAsync()
        {
            CreationDate = DateTime.Now;
            Status = PipelineModuleStatus.Running;
            var result = await _listenerQueue.StartListeningAsync();
            if (result.Successful)
            {
                WorkLoop();
            }

            return result;
        }

        public virtual Task<InvokeResult> PauseAsync()
        {
            Status = PipelineModuleStatus.Paused;

            return Task.FromResult(InvokeResult.Success);
        }

        public virtual Task<InvokeResult> StopAsync()
        {
            Status = PipelineModuleStatus.Idle;
            if (_listenerQueue != null)
            {
                return _listenerQueue.StopListeningAsync();
            }
            else
            {
                return Task.FromResult(InvokeResult.Success);
            }
        }

        protected IPEMBus PEMBus { get { return _pemBus; } }

        protected void LogMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, tag, message, newArgs.ToArray());
        }

        protected void LogException(string tag, Exception ex, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddException(tag, ex, newArgs.ToArray());
        }

        public void LogError(ErrorCode err, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddError(err, newArgs.ToArray());
        }

        protected void LogVerboseMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Verbose, tag, message, newArgs.ToArray());
        }

        protected void LogWarningMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.WarningCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Warning, tag, message, newArgs.ToArray());
        }

        protected void LogErrorMessage(string tag, string message, params KeyValuePair<string, string>[] args)
        {
            Metrics.ErrorCount++;
            var newArgs = args.ToList();
            newArgs.Add(new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Error, tag, message, newArgs.ToArray());
        }

        protected void LogStateChange(string tag, string oldState, string newState)
        {
            PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.StateChange, tag, "statusChange",
                new KeyValuePair<string, string>("oldState", oldState),
                new KeyValuePair<string, string>("newState", newState),
                new KeyValuePair<string, string>("pipelineModuleId", _pipelineModuleConfiguration.Id));
        }

        protected async void NotifyWebSocketSubscribers(String messageType, String message)
        {
            var msgReceiveddMsg = StatusUpdateMessage.Create(messageType, message);
            await PEMBus.WebSocketChannel.SendToChannelAsync(msgReceiveddMsg, "instance", PEMBus.Instance.Id);
        }

        protected void NotifySMSSubscribers(string messagType, string message)
        {

        }
    }
}
