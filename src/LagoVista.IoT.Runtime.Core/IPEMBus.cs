using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPEMBus
    {
        List<IPEMQueue> Queues { get; }

        ILogger Logger { get; }

        DeploymentInstance Instance { get; set; }


        LagoVista.IoT.DeviceManagement.Core.Managers.IDeviceManager DeviceManager { get; set; }

        void AddError(Error errorCode, params KeyValuePair<string, string>[] extras);
    }
}
