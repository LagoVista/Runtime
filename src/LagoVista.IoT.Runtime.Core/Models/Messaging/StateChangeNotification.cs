// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ee0344d0bf4023a680de084c07cc726f6015b205cf1ba9a73fd45cb0017b0bc8
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Messaging
{
    public class StateChangeNotification
    {
        public EntityHeader OldState { get; set; }
        public EntityHeader NewState { get; set; }

        public bool? OldDeployed { get; set; }
        public bool? NewDeployed { get; set; }
    }
}
