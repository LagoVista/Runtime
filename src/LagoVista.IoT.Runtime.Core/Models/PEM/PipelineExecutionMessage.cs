using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging;
using LagoVista.IoT.DeviceManagement.Core.Models;

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

    public enum MessagePayloadTypes
    {
        Unknown,
        Binary,
        Text
    }

    public class PipelineExecutionMessage
    {
        public PipelineExecutionMessage()
        {
            Id = Guid.NewGuid().ToId();

            Status = EntityHeader<StatusTypes>.Create(StatusTypes.Created);

            PayloadType = EntityHeader<MessagePayloadTypes>.Create(MessagePayloadTypes.Unknown);

            Envelope = new MessageEnvelope();

            ErrorMessages = new List<Error>();
            InfoMessages = new List<Info>();
            WarningMessages = new List<Warning>();
            Log = new List<Info>();

            Instructions = new List<PipelineExecutionInstruction>();
        }


        /// <summary>
        /// Unique ID assigned to the Message ID
        /// </summary>
        public string Id { get; set; }

        public EntityHeader<StatusTypes> Status { get; set; }

        public EntityHeader<MessagePayloadTypes> PayloadType { get; set; }        

        public Device Device { get; set; }

        public  PipelineExecutionInstruction CurrentInstruction { get; set; }

        public List<PipelineExecutionInstruction> Instructions { get; set; }

        public string CreationTimeStamp { get; set; }

        public string CompletionTimeStamp { get; set; }

        /// <summary>
        /// Total of MS (better by subsecond) required to parse the message.
        /// </summary>
        public double ParsingExecutionTimeMS { get; set; }

        /// <summary>
        /// Time Stamp in MS from when the Message was Created to when it was completed.
        /// </summary>
        public double ExecutionTimeMS { get; set; }

        /// <summary>
        /// Contains Information about the message
        /// </summary>
        public MessageEnvelope Envelope { get; set; }

        /// <summary>
        /// Length of either the Binary or Text Payload
        /// </summary>
       public int PayloadLength { get; set; }

        /// <summary>
        /// Byte Array that makes up the Binary Payload
        /// </summary>
        public byte[] BinaryPayload { get; set; }

        /// <summary>
        /// String that makes up the Text Paylaod
        /// </summary>
        public string TextPayload { get; set; }

        /// <summary>
        /// The Message ID as identified by the parser
        /// </summary>
        public String MessageId { get; set; }    

        public List<Error> ErrorMessages {get; set;}
        public List<Info> InfoMessages { get; set; }
        public List<Warning> WarningMessages { get; set; }

        /* Execution Log */
        public List<Info> Log { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MessageEnvelope OutgoingEnvelope { get; set; }

        /// <summary>
        /// Type of data sent back to the device
        /// </summary>
        public EntityHeader<MessagePayloadTypes> OutputPayloadType { get; set; }
        /// <summary>
        /// Size of the outgoing message
        /// </summary>
        public int OutgoingPayloadLength { get; set; }

        /// <summary>
        /// Text Content to be Sent back to the device
        /// </summary>
        public string OutgoingTextPaylaod { get; set; }

        /// <summary>
        /// Binary Content to be sent back to the device
        /// </summary>
        public byte[] OutgoingBinaryPayload { get; set; }
    }
}
