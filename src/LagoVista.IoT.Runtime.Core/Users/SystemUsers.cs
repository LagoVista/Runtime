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
