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

    public class PipelineExectionMessage
    {
        public PipelineExectionMessage()
        {
            Id = Guid.NewGuid().ToId();

            CreationTimeStamp = DateTime.UtcNow.ToJSONString();
            Status = StatusTypes.Created;

            PayloadType = MessagePayloadTypes.Unknown;

            Envelope = new MessageEnvelope();

            ErrorMessages = new List<Error>();
            InfoMessages = new List<Info>();
            WarningMessages = new List<Warning>();
            Log = new List<Info>();

            Instructions = new List<PipelineExectionInstruction>();
        }


        /// <summary>
        /// Unique ID assigned to the Message ID
        /// </summary>
        public string Id { get; set; }

        public StatusTypes Status { get; set; }

        public MessagePayloadTypes PayloadType { get; set; }

        public EntityHeader<DeviceConfiguration> Configuration { get; set; }

        public Device Device { get; set; }

        public  PipelineExectionInstruction CurrentInstruction { get; set; }

        public List<PipelineExectionInstruction> Instructions { get; set; }

        public string CreationTimeStamp { get; set; }

        public string CompletionTimeStamp { get; set; }

        /// <summary>
        /// Time Stamp in MS from when the Message was Created to when it was completed.
        /// </summary>
        public double ExecutionTimeMS { get; set; }

        /// <summary>
        /// Contains Information about
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



        public List<Error> ErrorMessages {get; set;}
        public List<Info> InfoMessages { get; set; }
        public List<Warning> WarningMessages { get; set; }

        /* Execution Log */
        public List<Info> Log { get; set; }
    }
}
