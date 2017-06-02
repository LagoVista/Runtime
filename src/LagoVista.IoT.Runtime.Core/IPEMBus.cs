using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPEMBus
    {
        List<IPEMQueue> Queues { get; }

        IInstanceLogger Logger { get; }

        DeploymentInstance Instance { get; set; }

        IWebSocketChannel WebSocketChannel { get; }

        LagoVista.IoT.DeviceManagement.Core.Managers.IDeviceManager DeviceManager { get; set; }

        void AddError(Error errorCode, params KeyValuePair<string, string>[] extras);
    }
}
