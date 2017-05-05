using Newtonsoft.Json;
using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Messaging
{
    public class StatusUpdateMessage
    {
        public StatusUpdateMessage()
        {
            MessageId = Guid.NewGuid().ToId();
            DateStamp = DateTime.Now.ToJSONString();
        }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
        [JsonProperty("dateStamp")]
        public string DateStamp { get; set; }
        [JsonProperty("messageType")]
        public string MessageType { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("details")]
        public string Details { get; set; }
        [JsonProperty("count")]
        public double Count { get; set; }
        [JsonProperty("units")]
        public string Units { get; set; }

        public static StatusUpdateMessage Create(String messageType, string message, string details = "")
        {
            return new StatusUpdateMessage()
            {
                MessageType = messageType,
                Message = message,
                Details = details
            };
        }

        public static StatusUpdateMessage Create(String messageType, double count, string units)
        {
            return new StatusUpdateMessage()
            {
                MessageType = messageType,
                Count = count,
                Units = units
            };
        }
    }
}
