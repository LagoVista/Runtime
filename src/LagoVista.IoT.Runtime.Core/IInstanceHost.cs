using LagoVista.IoT.Runtime.Core.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IInstanceHost
    {
        String Id { get; set; }

        IPEMBus PemBus { get; }

        IPEMQueue PlannerQueue { get; }

        List<IListenerModule> Listeners { get; }

        IPipelineModule Planner { get; }

        List<IPipelineModule> Modules { get; }

    }
}
