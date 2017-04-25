using LagoVista.IoT.Deployment.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPipelineModuleHost
    {
        string Id { get; }

        HostTypes HostType { get; }
    }
}
