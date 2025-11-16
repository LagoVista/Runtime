// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 11d2fcc37be8c8c09856fa59b453c519d2e1d7a9ab731d0a9fcce88e18f3d1a4
// IndexVersion: 2
// --- END CODE INDEX META ---
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
