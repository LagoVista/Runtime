using LagoVista.IoT.Runtime.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core;
using Newtonsoft.Json;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Runtime.Core.Models.Messaging
{
    public class Notification
    {
        public Notification()
        {
            MessageId = Guid.NewGuid().ToId();
            DateStamp = DateTime.Now.ToJSONString();
        }

        [JsonProperty("messageId")]
        public String MessageId { get; set; }
        [JsonProperty("dateStamp")]
        public String DateStamp { get; set; }

        [JsonProperty("channel")]
        public EntityHeader<Channels> Channel { get; set; }

        [JsonProperty("verbosity")]
        public EntityHeader<NotificationVerbosity> Verbosity { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("text")]
        public String Text { get; set; }

        [JsonProperty("payloadType")]
        public String PayloadType { get; set; }

        [JsonProperty("payloadJSON")]
        public String Payload { get; set; }
    }
}
