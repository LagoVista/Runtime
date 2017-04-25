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
        }

        public Dictionary<string, string> Headers { get; private set; }
        public int ReceivedOnPort { get; set; }
        public String FromAddress { get; set; }
    }
}
