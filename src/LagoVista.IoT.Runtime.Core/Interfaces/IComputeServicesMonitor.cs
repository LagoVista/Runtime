using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IComputeServicesMonitor
    {
        void PopulateComputeResourceMetrics(ComputeResourceMetrics metrics);
    }
}
