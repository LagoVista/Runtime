using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Logging;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Runtime.Core.Storage;
using LagoVista.IoT.Runtime.Core.Users;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPEMBus
    {
        List<IPEMQueue> Queues { get; }

        IInstanceLogger InstanceLogger { get; }

        IDeviceLogger DeviceLogger { get; }

        IPEMStorage PEMStorage { get; set; }

        ISystemUsers SystemUsers { get; set; }

        DeploymentInstance Instance { get; set; }

        INotificationPublisher NotificationPublisher { get; }

        LagoVista.IoT.DeviceManagement.Core.IDeviceManagerRemote DeviceManager { get; set; }

        void AddError(Error errorCode, params KeyValuePair<string, string>[] extras);
    }
}
