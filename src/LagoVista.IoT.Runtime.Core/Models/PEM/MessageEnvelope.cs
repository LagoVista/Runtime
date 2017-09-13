using LagoVista.Core.Models;
using System;
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

        public string DeviceRepoId { get; set; }
        public string DeviceId { get; set; }
        public string Topic { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Headers { get; private set; }
        public int ReceivedOnPort { get; set; }
        public string FromAddress { get; set; }
        public EntityHeader MessageType { get; set; }

        public Dictionary<string, MessageValue> Values { get; set; }
    }
}