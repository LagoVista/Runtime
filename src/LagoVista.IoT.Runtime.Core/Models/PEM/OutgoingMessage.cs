using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class OutgoingMessage
    {
        public OutgoingMessage()
        {
            Headers = new Dictionary<string, string>();
        }

        public EntityHeader<PayloadTypes> PayloadType { get; set; }

        public Dictionary<string, string> Headers { get; private set; }

        public string ContentType { get; set; }
        public string Topic { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public byte[] BinaryPayload { get; set; }
        public string TextPayload { get; set; }
    }
}
