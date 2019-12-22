using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;
using System.Linq;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging;
using LagoVista.IoT.DeviceManagement.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public enum StatusTypes
    {
        Created,
        PendingExecution,
        Completed,
        CompletedWithWarnings,
        Failed,
    }

    public enum ErrorReason
    {
        None,
        UnexepctedError,
        Unspecified,
        SeeErrorLog,
        CouldNotDetermineDeviceId,
        CouldNotDetermineMessageId,
        CouldNotFindDevice,
        CouldNotFindMessage
    }

    public enum MessagePayloadTypes
    {
        Unknown,
        Binary,
        Media,
        Text
    }

    public enum MessageTypes
    {
        Unknown,
        Deadletter,
        Regular,
        InputCommand
    }

    public class PipelineExecutionMessage : IIDEntity
    {
        public PipelineExecutionMessage()
        {
            Id = Guid.NewGuid().ToId();

            Status = StatusTypes.Created;

            PayloadType = MessagePayloadTypes.Unknown;

            Envelope = new MessageEnvelope();

            ErrorMessages = new List<Error>();
            InfoMessages = new List<Info>();
            WarningMessages = new List<Warning>();
            Log = new List<Info>();
            OutputCommands = new List<OutputCommand>();
            OutgoingMessages = new List<OutgoingMessage>();

            Instructions = new List<PipelineExecutionInstruction>();

            MessageType = MessageTypes.Unknown;
        }


        /// <summary>
        /// Unique ID assigned to the Message ID
        /// </summary>
        [JsonProperty("pemId")]
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("messageType")]
        public MessageTypes MessageType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public StatusTypes Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("errorReason")]
        public ErrorReason ErrorReason { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("payloadType")]
        public MessagePayloadTypes PayloadType { get; set; }

        [JsonProperty("device", NullValueHandling = NullValueHandling.Ignore)]
        public LagoVista.IoT.DeviceManagement.Core.Models.Device Device { get; set; }

        [JsonProperty("currentInstruction", NullValueHandling = NullValueHandling.Ignore)]
        public PipelineExecutionInstruction CurrentInstruction { get; set; }

        [JsonProperty("instructions")]
        public List<PipelineExecutionInstruction> Instructions { get; set; }

        [JsonProperty("creationTimeStamp")]
        public string CreationTimeStamp { get; set; }


        [JsonProperty("completionTimeStamp")]
        public string CompletionTimeStamp { get; set; }

        /// <summary>
        /// Time Stamp in MS from when the Message was Created to when it was completed.
        /// </summary>
        [JsonProperty("executionTimeMS")]
        public double ExecutionTimeMS { get; set; }

        /// <summary>
        /// Contains Information about the message
        /// </summary>
        [JsonProperty("envelope", NullValueHandling = NullValueHandling.Ignore)]
        public MessageEnvelope Envelope { get; set; }

        /// <summary>
        /// Length of either the Binary or Text Payload
        /// </summary>
        [JsonProperty("payloadLength")]
        public long PayloadLength { get; set; }

        /// <summary>
        /// Byte Array that makes up the Binary Payload
        /// </summary>
        [JsonProperty("binaryPayload", NullValueHandling = NullValueHandling.Ignore)]
        public byte[] BinaryPayload { get; set; }

        /// <summary>
        /// String that makes up the Text Paylaod
        /// </summary>
        [JsonProperty("textPayload", NullValueHandling = NullValueHandling.Ignore)]
        public string TextPayload { get; set; }

        /// <summary>
        /// A reference to the input command that triggered the message.
        /// </summary>
        [JsonProperty("inputCommand", NullValueHandling = NullValueHandling.Ignore)]
        public EntityHeader InputCommand { get; set; }

        /// <summary>
        /// The Message ID as identified by the parser
        /// </summary>
        [JsonProperty("messageId", NullValueHandling = NullValueHandling.Ignore)]
        public String MessageId { get; set; }

        [JsonProperty("errorMessages", NullValueHandling = NullValueHandling.Ignore)]
        public List<Error> ErrorMessages { get; set; }
        [JsonProperty("infoMessages", NullValueHandling = NullValueHandling.Ignore)]
        public List<Info> InfoMessages { get; set; }
        [JsonProperty("warningMessages", NullValueHandling = NullValueHandling.Ignore)]
        public List<Warning> WarningMessages { get; set; }

        /* Execution Log */
        [JsonProperty("log", NullValueHandling = NullValueHandling.Ignore)]
        public List<Info> Log { get; set; }

        [JsonProperty("outputCommands", NullValueHandling = NullValueHandling.Ignore)]
        public List<OutputCommand> OutputCommands { get; set; }

        [JsonProperty("outgoingMessages", NullValueHandling = NullValueHandling.Ignore)]
        public List<OutgoingMessage> OutgoingMessages { get; set; }

        [JsonProperty("responseMessage", NullValueHandling = NullValueHandling.Ignore)]
        public OutgoingMessage ResponseMessage { get; set; }

        /// <summary>
        /// The Message ID as identified by the parser
        /// </summary>
        [JsonProperty("mediaItemId", NullValueHandling = NullValueHandling.Ignore)]
        public String MediaItemId { get; set; }

        /// <summary>
        /// This should be call before doing final storage, will reduce the size of 
        /// unused fields
        /// </summary>
        public void SetEmptyValueToNull()
        {
            foreach (var err in ErrorMessages)
            {
                err.SetEmptyValueToNull();
            }


            if (String.IsNullOrEmpty(MediaItemId)) MediaItemId = null;

            if (String.IsNullOrEmpty(MessageId)) MessageId = null;
            if (!Log.Any()) Log = null;
            if (!OutputCommands.Any()) OutputCommands = null;
            if (!OutgoingMessages.Any()) OutgoingMessages = null;
            if (!ErrorMessages.Any()) ErrorMessages = null;
            if (!InfoMessages.Any()) InfoMessages = null;
            if (!WarningMessages.Any()) WarningMessages = null;
            if (Envelope.SetEmptyValueToNull())
            {
                Envelope = null;
            }
        }

        public PipelineExecutionMessage Clone()
        {
            var pem = new PipelineExecutionMessage()
            {
                BinaryPayload = BinaryPayload,
                CompletionTimeStamp = CompletionTimeStamp,
                CreationTimeStamp = CreationTimeStamp,
                Device = Device,
                ErrorMessages = ErrorMessages,
                ErrorReason = ErrorReason,
                ExecutionTimeMS = ExecutionTimeMS,
                Id = Id,
                InfoMessages = InfoMessages,
                InputCommand = InputCommand,
                Log = Log,
                MediaItemId = MediaItemId,
                MessageId = MessageId,
                MessageType = MessageType,
                OutgoingMessages = OutgoingMessages,
                OutputCommands = OutputCommands,
                PayloadLength = PayloadLength,
                PayloadType = PayloadType,
                ResponseMessage = ResponseMessage,
                Status = Status,
                TextPayload = TextPayload,
                WarningMessages = WarningMessages,
            };
            
            foreach(var instruction in Instructions)
            {
                pem.Instructions.Add(instruction);
            }

            if(CurrentInstruction != null)
            {
                pem.CurrentInstruction = CurrentInstruction.Clone();
            }

            if(Envelope != null)
            {
                pem.Envelope = Envelope.Clone();
            }

            return pem;
        }
    }
}
