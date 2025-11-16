// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 655f03e5861f4f3426b1fced536b75194a43301f71feea3f379df461522a6824
// IndexVersion: 2
// --- END CODE INDEX META ---
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
