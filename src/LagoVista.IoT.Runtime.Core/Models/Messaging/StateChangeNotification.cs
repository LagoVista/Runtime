using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Messaging
{
    public class StateChangeNotification
    {
        public String OldState { get; set; }
        public String NewState { get; set; }

        public bool? OldDeployed { get; set; }
        public bool? NewDeployed { get; set; }
    }
}
