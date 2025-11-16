// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ff1383951fc2d0fb0fdced80c4b6fc785b6dec5952c5376917bf8d21eea8dd57
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core.Models.PEM
{
    public class OutputCommand
    {
        public EntityHeader Command { get; set; }
        public List<MessageValue> Fields { get; set; } = new List<MessageValue>();
    }
}
