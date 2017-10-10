using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class OutgoingMessage
    {
        public List<MessageValue> Fields { get; set; } = new List<MessageValue>();
    }
}
