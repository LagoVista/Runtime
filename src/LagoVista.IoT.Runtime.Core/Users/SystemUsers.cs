// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 96f277ec4b3c35a3465cf34f07dee8a5c31bb7b2b9d85d85a307f5d5963e6c97
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Users
{
    public interface ISystemUsers
    {
        EntityHeader SystemOrg { get; }
        EntityHeader HostUser { get; }
        EntityHeader InstanceUser { get; }
        EntityHeader DeviceManagerUser { get; }
        EntityHeader JobServiceUser { get; }
    }
}
