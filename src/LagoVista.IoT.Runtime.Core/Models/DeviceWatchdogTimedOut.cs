using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    public class DeviceWatchdogTimedout
    {
        public string Id { get; set; }
        public string LastContact { get; set; }
        public int TimeoutSeconds { get; set; }
        public long ExpiredTicks { get; set; }
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public string DeviceConfiguration { get; set; }
        public string DeviceType { get; set; }
        public string LastNotified { get; set; }
    }
}
