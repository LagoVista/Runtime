using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public abstract class ListenerModule : PipelineModule
    {
        private readonly ListenerConfiguration _listenerConfiguration;
        private readonly IPEMQueue _outgoingMessageQueue;

        public ListenerModule(ListenerConfiguration listenerConfiguration, IPEMBus pemBus) : base(listenerConfiguration, pemBus)
        {
            _listenerConfiguration = listenerConfiguration;

            _outgoingMessageQueue = pemBus.Queues.Where(queue => queue.PipelineModuleId == listenerConfiguration.Id).FirstOrDefault();
            if (_outgoingMessageQueue == null) throw new Exception($"Incoming queue for listener module {_listenerConfiguration.Id} - {_listenerConfiguration.Name} could not be found.");
        }

        public async Task<InvokeResult<PipelineExecutionMessage>> AddBinaryMessageAsync(byte[] buffer, DateTime startTimeStamp, String deviceId = "", String topic = "")
        {
            if (!String.IsNullOrEmpty(topic) && topic.StartsWith("nuviot/srvr/dvcsrvc"))
            {
                var strPayload = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                return await HandleSystemMessageAsync(topic, strPayload);
            }

            try
            {
                var message = new PipelineExecutionMessage()
                {
                    SolutionVersion = PEMBus.Instance.Solution.Value.Version,
                    RuntimeVersion = PEMBus.RuntimeVersion,
                    PayloadType = MessagePayloadTypes.Binary,
                    BinaryPayload = buffer,
                    CreationTimeStamp = startTimeStamp.ToJSONString()
                };

                Metrics.MessagesProcessed++;

                if (buffer != null)
                {
                    message.PayloadLength = buffer.Length;
                }

                Metrics.BytesProcessed = message.PayloadLength + (String.IsNullOrEmpty(topic) ? 0 : topic.Length);

                if (!String.IsNullOrEmpty(deviceId))
                {
                    message.Envelope.DeviceId = deviceId;
                }

                message.Envelope.Topic = topic;

                var listenerInstruction = new PipelineExecutionInstruction()
                {
                    Name = _listenerConfiguration.Name,
                    Type = GetType().Name,
                    QueueId = "N/A",
                    StartDateStamp = startTimeStamp.ToJSONString(),
                    ProcessByHostId = PEMBus.Instance.PrimaryHost.Id,
                    ExecutionTimeMS = (DateTime.UtcNow - startTimeStamp).TotalMilliseconds,
                };

                message.Instructions.Add(listenerInstruction);
                var plannerQueue = PEMBus.Queues.Where(queue => queue.ForModuleType == PipelineModuleType.Planner).FirstOrDefault();

                var planner = PEMBus.Instance.Solution.Value.Planner.Value;
                var plannerInstruction = new PipelineExecutionInstruction()
                {
                    Name = "Planner",
                    Type = "Planner",
                    QueueId = plannerQueue.PipelineModuleId,
                    Enqueued = DateTime.UtcNow.ToJSONString()
                };

                message.CurrentInstruction = plannerInstruction;
                message.Instructions.Add(plannerInstruction);


                await plannerQueue.EnqueueAsync(message);

                return InvokeResult<PipelineExecutionMessage>.Create(message);
            }
            catch (Exception ex)
            {
                PEMBus.InstanceLogger.AddException("ListenerModule_AddBinaryMessageAsync", ex);
                return InvokeResult<PipelineExecutionMessage>.FromException("ListenerModule_AddBinaryMessageAsync", ex);
            }
        }

        public async Task<InvokeResult<PipelineExecutionMessage>> AddMediaMessageAsync(Stream stream, string contentType, long contentLength, DateTime startTimeStamp, string path, String deviceId = "", String topic = "", Dictionary<string, string> headers = null)
        {
            try
            {
                var message = new PipelineExecutionMessage()
                {
                    PayloadType = MessagePayloadTypes.Media,
                    CreationTimeStamp = startTimeStamp.ToJSONString(),
                    SolutionVersion = PEMBus.Instance.Solution.Value.Version,
                    RuntimeVersion = PEMBus.RuntimeVersion,
                };

                Metrics.MessagesProcessed++;

                message.PayloadLength = contentLength;
                Metrics.BytesProcessed += message.PayloadLength + (String.IsNullOrEmpty(topic) ? 0 : topic.Length);

                message.Envelope.DeviceId = deviceId;
                message.Envelope.Topic = topic;
                message.Envelope.Path = path;

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

                    foreach (var hdr in headers)
                    {
                        message.Envelope.Headers.Add(hdr.Key, hdr.Value);
                    }
                }

                var listenerInstruction = new PipelineExecutionInstruction()
                {
                    Name = _listenerConfiguration.Name,
                    Type = GetType().Name,
                    QueueId = "N/A",
                    StartDateStamp = startTimeStamp.ToJSONString(),
                    ProcessByHostId = PEMBus.Instance.PrimaryHost.Id,
                    ExecutionTimeMS = (DateTime.UtcNow - startTimeStamp).TotalMilliseconds,
                };

                message.Instructions.Add(listenerInstruction);
                var plannerQueue = PEMBus.Queues.Where(queue => queue.ForModuleType == PipelineModuleType.Planner).FirstOrDefault();

                var planner = PEMBus.Instance.Solution.Value.Planner.Value;
                var plannerInstruction = new PipelineExecutionInstruction()
                {
                    Name = "Planner",
                    Type = "Planner",
                    QueueId = plannerQueue.PipelineModuleId,
                    Enqueued = DateTime.UtcNow.ToJSONString()
                };

                message.CurrentInstruction = plannerInstruction;
                message.Instructions.Add(plannerInstruction);

                double? lat = null, lon = null;

                if (headers != null
                    && headers.ContainsKey("x-latitude") && headers.ContainsKey("x-longitude"))
                {
                    if (double.TryParse(headers["x-latitude"], out double tmpLatitude) &&
                       double.TryParse(headers["x-longitude"], out double tmpLongitude))
                    {
                        lat = tmpLatitude;
                        lon = tmpLongitude;
                    }
                }

                var insertResult = await PEMBus.DeviceMediaStorage.StoreMediaItemAsync(stream, message.Id, contentType, contentLength, lat, lon);
                if (!insertResult.Successful)
                {
                    return InvokeResult<PipelineExecutionMessage>.FromInvokeResult(insertResult.ToInvokeResult());
                }

                message.MediaItemId = insertResult.Result;

                await plannerQueue.EnqueueAsync(message);

                return InvokeResult<PipelineExecutionMessage>.Create(message);

            }
            catch (Exception ex)
            {
                PEMBus.InstanceLogger.AddException("ListenerModule_AddBinaryMessageAsync", ex);
                return InvokeResult<PipelineExecutionMessage>.FromException("ListenerModule_AddBinaryMessageAsync", ex);
            }
        }


        protected void WorkLoop()
        {
            Task.Run(async () =>
            {
                await _outgoingMessageQueue.StartListeningAsync();

                while (Status == PipelineModuleStatus.Running || Status == PipelineModuleStatus.Listening)
                {
                    var msg = await _outgoingMessageQueue.ReceiveAsync();
                    /* queue will return a null message when it's "turned off", should probably change the logic to use cancellation tokens, not today though KDW 5/3/2017 */
                    //TODO Use cancellation token rather than return null when queue is no longer listenting.
                    // don't have the ACME listener process the message, even though it will likely do nothing.
                    if (msg != null && _listenerConfiguration.RESTListenerType != RESTListenerTypes.AcmeListener)
                    {
                        await SendResponseAsync(msg, msg.OutgoingMessages.First());
                    }
                }
            });
        }

        public async override Task<InvokeResult> StartAsync()
        {
            /* ACME Listeners are dedicated port 80 listeners that only listen for very special requests to verify domain ownership
            * if we have a port 80 listener in addition to the AcmeListener, it will not be an AcmeListener and should have a
            * a listener queue */
            if (_outgoingMessageQueue == null)
            {
                throw new NullReferenceException("Input Message Queue is Null");
            }

            CreationDate = DateTime.Now;
            await StateChanged(PipelineModuleStatus.Running);

            WorkLoop();

            return InvokeResult.Success;
        }

        public async override Task<InvokeResult> StopAsync()
        {
            return await base.StopAsync();
        }

        public abstract Task<InvokeResult> SendResponseAsync(PipelineExecutionMessage message, OutgoingMessage msg);

        private async Task<InvokeResult<PipelineExecutionMessage>> HandleSystemMessageAsync(string path, string payload)
        {
            Metrics.MessagesProcessed++;

            //PEMBus.InstanceLogger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, "ListenerModule_HandleSystemMessageAsync", "Received System Message", path.ToKVP("topic"), payload.ToKVP("body"));

            var parts = path.Split('/');
            if (parts.Length < 5)
            {
                var errMsg = $"NuvIoT service messages must be at least 5 segments {path} is {parts.Length} segments";
                PEMBus.InstanceLogger.AddError("ListenerModule__HandleSystemMessage", errMsg);
                return InvokeResult<PipelineExecutionMessage>.FromError(errMsg);
            }

            var deviceId = parts[3];

            var device = await PEMBus.DeviceStorage.GetDeviceByDeviceIdAsync(deviceId);
            if (device == null)
            {
                var errMsg = $"Could not find device with device id {deviceId}.";
                PEMBus.InstanceLogger.AddError("ListenerModule__HandleSystemMessage", errMsg);
                return InvokeResult<PipelineExecutionMessage>.FromError(errMsg);
            }

            device.LastContact = DateTime.UtcNow.ToJSONString();

            var sysMessageType = parts[4];
            var details = $"payload: {payload} - ";

            if (sysMessageType == "state")
            {
                device.DeviceTwinDetails.Insert(0, new DeviceManagement.Models.DeviceTwinDetails()
                {
                    Timestamp = DateTime.UtcNow.ToJSONString(),
                    Details = payload
                });

                var payloadSegements = payload.Split(',');
                foreach (var segement in payloadSegements)
                {
                    var fieldParts = segement.Split('=');
                    if (fieldParts.Length == 2)
                    {
                        var nameParts = fieldParts[0].Split('-');
                        if (nameParts.Length == 2)
                        {
                            var typeName = nameParts[0];
                            var key = nameParts[1];
                            var value = fieldParts[1];
                            if (typeName != "readonly")
                            {
                                var prop = device.Properties.FirstOrDefault(prp => prp.Key == key);
                                if (prop != null)
                                {
                                    if (prop.Value != value)
                                    {
                                        prop.Value = value;
                                        prop.LastUpdated = DateTime.UtcNow.ToJSONString();
                                        prop.LastUpdatedBy = "Device Twin";
                                    }
                                }
                            }
                            else
                            {
                                if (key == "firmwareSku")
                                {
                                    device.ActualFirmware = value;
                                    device.ActualFirmwareDate = DateTime.Now.ToJSONString();
                                }
                                if (key == "firmwareVersion")
                                {
                                    device.ActualFirmwareRevision = value;
                                    device.ActualFirmwareDate = DateTime.Now.ToJSONString();
                                }
                            }
                        }
                    }
                }
            }
            else if (sysMessageType == "err")
            {
                var err = parts[5];
                var action = parts[6];
                var exception = new DeviceException()
                {
                    ErrorCode = err,
                    DeviceId = device.DeviceId,
                    DeviceUniqueId = device.Id,
                    DeviceRepositoryId = device.DeviceRepository.Id,
                    Timestamp = DateTime.UtcNow.ToString(),
                };

                exception.AdditionalDetails.Add(payload);

                if (action == "raise")
                {

                    await PEMBus.InstanceConnector.HandleDeviceExceptionAsync(exception);
                }
                else if (action == "clear")
                {
                    await PEMBus.InstanceConnector.ClearDeviceExceptionAsync(exception);
                }
            }
            else if (sysMessageType == "relays")
            {
                var values = payload.Split(',');

                if (values.Length != 8)
                    throw new InvalidDataException($"Relay values from device should consist of 8 comma delimited values, message consists of [{values.Length}] items data: [{payload}].");

                if (device.Relays.Count == 0)
                {
                    for (var idx = 0; idx < 8; idx++)
                    {
                        device.Relays.Add(new Relay()
                        {
                            Index = idx,
                            CurrentState = EntityHeader<RelayStates>.Create(RelayStates.Unknown),
                            DesiredState = EntityHeader<RelayStates>.Create(RelayStates.Unknown),
                            LastUpdated = DateTime.UtcNow.ToJSONString(),
                            Name = $"Relay {idx + 1}"
                        });
                    }
                }

                for (var idx = 0; idx < 8; ++idx)
                {
                    if (device.Relays[idx].DesiredState == null)
                        device.Relays[idx].DesiredState = EntityHeader<RelayStates>.Create(RelayStates.Unknown);

                    var state = RelayStates.Unknown;
                    if (values[idx] == "1")
                        state = RelayStates.On;
                    else if (values[idx] == "0")
                        state = RelayStates.Off;


                    if (device.Relays[idx].CurrentState.Value != state)
                    {
                        device.Relays[idx].CurrentState = EntityHeader<RelayStates>.Create(state);
                        device.Relays[idx].LastUpdated = DateTime.UtcNow.ToJSONString();
                    }
                }

            }
            else if (sysMessageType == "iovalues")
            {
                var values = payload.Split(',');
                if (values.Length != 16)
                    throw new InvalidDataException($"IO Configuration from device should consist of 16 comma delimited values, message consists of {values.Length} items.");

                for (int idx = 0; idx < 8; ++idx)
                {
                    if (!String.IsNullOrEmpty(values[idx + 8]))
                    {
                        details += $" ADC {idx} has value {values[idx + 8]}";
                        var sensor = device.SensorCollection.Where(sns => sns.Technology != null &&
                                                                   sns.Technology.Value == DeviceManagement.Models.SensorTechnology.ADC &&
                                                                   sns.PortIndex == idx).FirstOrDefault();
                        if (sensor != null)
                        {
                            sensor.Value = values[idx + 8];
                            sensor.LastUpdated = DateTime.UtcNow.ToJSONString();
                        }
                        else
                        {
                            sensor = new Sensor()
                            {
                                PortIndex = idx,
                                Technology = EntityHeader<SensorTechnology>.Create(SensorTechnology.ADC),
                                Value = values[idx + 8],
                                LastUpdated = DateTime.UtcNow.ToJSONString()
                            };

                            sensor.Name = $"{sensor.Technology.Text} - {sensor.PortIndexSelection.Text}";
                            sensor.Key = sensor.Name.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();

                            device.SensorCollection.Add(sensor);
                        }
                    }
                }

                for (int idx = 0; idx < 8; ++idx)
                {
                    if (!String.IsNullOrEmpty(values[idx]))
                    {
                        details += $" IO {idx} has value {values[idx]}";
                        var sensor = device.SensorCollection.Where(sns => sns.Technology != null &&
                                                                    sns.Technology.Value == DeviceManagement.Models.SensorTechnology.IO &&
                                                                    sns.PortIndex == idx).FirstOrDefault();
                        if (sensor != null)
                        {
                            sensor.Value = values[idx];
                            sensor.LastUpdated = DateTime.UtcNow.ToJSONString();
                        }
                        else
                        {
                            sensor = new Sensor()
                            {
                                PortIndex = idx,
                                Technology = EntityHeader<SensorTechnology>.Create(SensorTechnology.IO),
                                Value = values[idx],
                                LastUpdated = DateTime.UtcNow.ToJSONString()
                            };

                            sensor.Name = $"{sensor.Technology.Text} - {sensor.PortIndexSelection.Text}";
                            sensor.Key = sensor.Name.Replace(" ", "").Replace("-", "").Replace("/", "").ToLower();

                            device.SensorCollection.Add(sensor);
                        }
                    }
                }

                await PEMBus.SensorEvaluator.EvaluateAsync(device);
            }
            else if (sysMessageType == "geo")
            {
                if (payload == "nofix")
                {
                    device.HasGeoFix = false;
                }
                else
                {
                    device.HasGeoFix = true;
                    var values = payload.Split(',');
                    if (values.Length < 2)
                    {
                        throw new InvalidDataException($"Geo Location Data type must contain a minimum of 2 fields for latitude and longitude, message consists of ${values.Length} items.");
                    }

                    if (!double.TryParse(values[0], out double lat))
                    {
                        throw new InvalidDataException($"Invalid Latitude value [{values[0]}].");
                    }

                    if (lat > 90 || lat < -90)
                    {
                        throw new InvalidDataException($"Invalid Latitude value [{values[0]}], must be between -90 and 90.");
                    }

                    if (!double.TryParse(values[1], out double lon))
                    {
                        throw new InvalidDataException($"Invalid Longitude value [{values[1]}].");
                    }

                    if (lon > 180 || lon < -180)
                    {
                        throw new InvalidDataException($"Invalid Latitude value [{values[1]}], must be between -180 and 180.");
                    }

                    double? alt = null;
                    if (values.Length > 2)
                    {
                        if (!double.TryParse(values[2], out double altValue))
                        {
                            throw new InvalidDataException($"Invalid Longitude value [{values[2]}].");
                        }
                        else
                        {
                            alt = altValue;
                        }
                    }

                    device.GeoLocation = new LagoVista.Core.Models.Geo.GeoLocation()
                    {
                        LastUpdated = DateTime.UtcNow.ToJSONString(),
                        Latitude = lat,
                        Longitude = lon,
                        Altitude = alt
                    };
                }
            }
            else if (sysMessageType == "online")
            {
                device.ConnectionTimeStamp = DateTime.UtcNow.ToJSONString();
                var rssi = -1.0;
                var reconnect = false;

                var payloadSegements = payload.Split(',');
                foreach (var segement in payloadSegements)
                {
                    var fieldParts = segement.Split('=');
                    if (fieldParts.Length == 2)
                    {
                        var nameParts = fieldParts[0].Split('-');
                        if (nameParts.Length == 2)
                        {
                            var typeName = nameParts[0];
                            var key = nameParts[1];
                            var value = fieldParts[1];
                            if (typeName != "readonly")
                            {
                                var prop = device.Properties.FirstOrDefault(prp => prp.Key == key);
                                if (prop != null)
                                {
                                    if (prop.Value != value)
                                    {
                                        if (prop.AttributeType.Value == DeviceAdmin.Models.ParameterTypes.TrueFalse)
                                        {
                                            if (value == "1")
                                            {
                                                value = "true";
                                            }
                                            else if (value == "0")
                                            {
                                                value = "false";
                                            }
                                            else
                                            {
                                                value = value.ToLower();
                                            }
                                        }
                                        prop.Value = value;
                                        prop.LastUpdated = DateTime.UtcNow.ToJSONString();
                                        prop.LastUpdatedBy = "Device Twin";
                                    }
                                }
                            }
                        }
                        else
                        {
                            var key = fieldParts[0];
                            var value = fieldParts[1];

                            if (key == "firmwareSku")
                            {
                                device.ActualFirmware = value;
                                device.ActualFirmwareDate = DateTime.Now.ToJSONString();
                            }
                            else if (key == "firmwareVersion")
                            {
                                device.ActualFirmwareRevision = value;
                                device.ActualFirmwareDate = DateTime.Now.ToJSONString();
                            }
                            else if (key == "rssi")
                            {
                                double.TryParse(value, out rssi);
                            }
                            else if (key == "reconnect")
                            {
                                reconnect = value != "0";
                            }
                            else if (key == "ipAddress")
                            {
                                device.IPAddress = value;
                            }
                        }
                    }
                }

                var connectionEvent = new DeviceConnectionEvent()
                {
                    DeviceId = device.Id,
                    FirmwareRevision = device.ActualFirmwareRevision,
                    FirmwareSKU = device.ActualFirmware,
                    TimeStamp = DateTime.UtcNow.ToJSONString(),
                    RSSI = rssi,
                    Reconnect = reconnect
                };

                await PEMBus.DeviceConnectionEvent.AddDeviceEventConnectionEvent(connectionEvent);


            }

            await PEMBus.DeviceStorage.UpdateDeviceAsync(device);

            var json = JsonConvert.SerializeObject(Models.DeviceForNotification.FromDevice(device), _camelCaseSettings);
            var notification = new Notification()
            {
                Payload = json,
                Channel = EntityHeader<Channels>.Create(Channels.Device),
                ChannelId = device.Id,
                PayloadType = "Device",
                DateStamp = DateTime.UtcNow.ToJSONString(),
                MessageId = Guid.NewGuid().ToId(),
                Text = $"Device Updated - Sys Message {sysMessageType} - {details}",
                Title = "Device Updated"
            };

            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);

            notification = new Notification()
            {
                Payload = json,
                Channel = EntityHeader<Channels>.Create(Channels.DeviceRepository),
                ChannelId = device.DeviceRepository.Id,
                PayloadType = "Device",
                DateStamp = DateTime.UtcNow.ToJSONString(),
                MessageId = Guid.NewGuid().ToId(),
                Text = $"Device Updated - Sys Message {sysMessageType} - {details}",
                Title = "Device Updated"
            };

            await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);

            foreach (var group in device.DeviceGroups)
            {
                notification = new Notification()
                {
                    Payload = json,
                    Channel = EntityHeader<Channels>.Create(Channels.DeviceGroup),
                    ChannelId = group.Id,
                    PayloadType = "Device",
                    DateStamp = DateTime.UtcNow.ToJSONString(),
                    MessageId = Guid.NewGuid().ToId(),
                    Text = $"Device Updated - Sys Message {sysMessageType} - {details}",
                    Title = "Device Updated"
                };

                await PEMBus.NotificationPublisher.PublishAsync(Targets.WebSocket, notification);
            }

            return InvokeResult<PipelineExecutionMessage>.Create(null);

        }

        public async Task<InvokeResult<PipelineExecutionMessage>> AddStringMessageAsync(string buffer, DateTime startTimeStamp, string path = "", string deviceId = "", string topic = "", Dictionary<string, string> headers = null)
        {

            try
            {
                if (!String.IsNullOrEmpty(topic))
                {
                    Console.WriteLine($"Received Message with topic [{topic}]");
                }

                if (!String.IsNullOrEmpty(topic) && topic.StartsWith("nuviot/srvr/dvcsrvc"))
                {
                    return await HandleSystemMessageAsync(topic, buffer);
                }

                if (!String.IsNullOrEmpty(path) && path.StartsWith("/nuviot/srvr/dvcsrvc"))
                {
                    return await HandleSystemMessageAsync(path.TrimStart('/'), buffer);
                }

                var message = new PipelineExecutionMessage()
                {
                    PayloadType = MessagePayloadTypes.Text,
                    TextPayload = buffer,
                    SolutionVersion = PEMBus.Instance.Solution.Value.Version,
                    RuntimeVersion = PEMBus.RuntimeVersion,
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
                    Name = _listenerConfiguration.Name,
                    Type = GetType().Name,
                    QueueId = _listenerConfiguration.Id,
                    StartDateStamp = startTimeStamp.ToJSONString(),
                    ProcessByHostId = PEMBus.Instance.PrimaryHost.Id,
                    Enqueued = startTimeStamp.ToJSONString(),
                    ExecutionTimeMS = (DateTime.UtcNow - startTimeStamp).TotalMilliseconds,
                };

                message.Instructions.Add(listenerInstruction);

                var plannerQueue = PEMBus.Queues.Where(queue => queue.ForModuleType == PipelineModuleType.Planner).FirstOrDefault();
                if (plannerQueue == null)
                {
                    PEMBus.InstanceLogger.AddError("ListenerModule_AddStringMessageAsync", "Could not find planner queue.");
                    return InvokeResult<PipelineExecutionMessage>.FromError("Could not find planner queue.");
                }

                var planner = PEMBus.Instance.Solution.Value.Planner.Value;
                var plannerInstruction = new PipelineExecutionInstruction()
                {
                    Name = planner.Name,
                    Type = "Planner",
                    QueueId = plannerQueue.PipelineModuleId,
                    Enqueued = DateTime.UtcNow.ToJSONString()
                };

                message.CurrentInstruction = plannerInstruction;
                message.Instructions.Add(plannerInstruction);

                await plannerQueue.EnqueueAsync(message);

                return InvokeResult<PipelineExecutionMessage>.Create(message);
            }
            catch (Exception ex)
            {
                Metrics.DeadLetterCount++;
                Metrics.ErrorCount++;
                PEMBus.InstanceLogger.AddException("ListenerModule_AddStringMessageAsync", ex);
                Console.WriteLine(ex.StackTrace);
                return InvokeResult<PipelineExecutionMessage>.FromException("ListenerModule_AddStringMessageAsync", ex);
            }
        }
    }
}
