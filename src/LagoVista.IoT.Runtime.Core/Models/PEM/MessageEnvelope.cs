using LagoVista.Core.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class MessageEnvelope
    {
        public MessageEnvelope()
        {
            Headers = new Dictionary<string, string>();
            Values = new Dictionary<string, MessageValue>();
        }

        /* Note if additional fields are added, make sure the Clone method is updated */
        [JsonProperty("deviceRepoId", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceRepoId { get; set; }
        [JsonProperty("deviceid", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }
        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public string Topic { get; set; }
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Headers { get; private set; }
        [JsonProperty("receivedOnPort", NullValueHandling = NullValueHandling.Ignore)]
        public int? ReceivedOnPort { get; set; }
        [JsonProperty("fromAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string FromAddress { get; set; }
        [JsonProperty("messageType", NullValueHandling = NullValueHandling.Ignore)]
        public EntityHeader MessageType { get; set; }

        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, MessageValue> Values { get; set; }

        public bool SetEmptyValueToNull()
        {
            if (Headers.Count == 0) Headers = null;
            if (Values.Count == 0) Values = null;

            if (String.IsNullOrEmpty(DeviceRepoId)) DeviceRepoId = null;
            if (String.IsNullOrEmpty(DeviceId)) DeviceId = null;
            if (String.IsNullOrEmpty(Topic)) Topic = null;
            if (String.IsNullOrEmpty(Method)) Method = null;
            if (String.IsNullOrEmpty(Path)) Path = null;
            if (String.IsNullOrEmpty(FromAddress)) FromAddress = null;

            if (DeviceRepoId == null &&
               DeviceId == null &&
               Topic == null &&
               Method == null &&
               Path == null &&
               Headers == null &&
               !ReceivedOnPort.HasValue &&
               FromAddress == null &&
               MessageType == null &&
               Values == null)
                return true;
            else
                return false;
        }

        public MessageEnvelope Clone()
        {
            var env = new MessageEnvelope()
            {
                DeviceId = DeviceId,
                DeviceRepoId = DeviceRepoId,
                FromAddress = FromAddress,
                MessageType = MessageType,
                Method = Method,
                Path = Path,
                ReceivedOnPort = ReceivedOnPort,
                Topic = Topic
            };

            if (Headers != null)
            {
                foreach (var hdr in Headers)
                {
                    env.Headers.Add(hdr.Key, hdr.Value);
                }
            }

            if (Values != null)
            {
                foreach (var val in Values)
                {
                    if (val.Value != null)
                    {
                        if (val.Value != null)
                        {
                            env.Values.Add(val.Key, val.Value.Clone());
                        }
                    }
                }
            }

            return env;
        }
    }
}