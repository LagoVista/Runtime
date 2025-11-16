// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: acb1960263e9ae3b762ebee394fbc2953491ca015f40e4a93bb8838edbc601db
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceManagement.Core;
using LagoVista.IoT.Logging;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Interfaces;
using LagoVista.IoT.Runtime.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Runtime.Core.Storage;
using LagoVista.IoT.Runtime.Core.Users;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core
{
    public interface IPEMBus
    {
        string RuntimeVersion { get;  }
        DeploymentInstance Instance { get; set; }

        List<IPEMQueue> Queues { get; }

        IInstanceLogger InstanceLogger { get; }

        IDeviceLogger DeviceLogger { get; }

        IPEMStorage PEMStorage { get; set; }


        IApplicationCacheServices CacheServices { get; set; }
        IDeviceArchiveStorage DeviceArchiveStorage { get; set; }
        IDeviceMediaStorage DeviceMediaStorage { get; set; }
        IDataStreamServices DataStreamServices { get; set; }
        IDeviceStatusStorage DeviceStatusStorage { get; set; }
        IDeviceExceptionStorage DeviceExceptionStorage { get; set; }

        ICache SystemCache { get; set; }

        IDataMaintenance DataMaintence { get; set; }

        ISystemUsers SystemUsers { get; set; }

        ISecureStorage SecureStorage { get; set; }

        IDeviceWatchdog DeviceWatchdog { get; set; }
        IMessageWatchdog MessageWatchdog { get; set; }

        ISensorEvaluator SensorEvaluator { get; }
        ITransactionStorage TransactionStorage { get; set; }

        INotificationPublisher NotificationPublisher { get; }

        IDeviceStorage DeviceStorage { get; set; }

        IDeviceConnectionEventStorage DeviceConnectionEvent {get; set; }

        IInstanceConnector InstanceConnector { get; set; }

        IDeviceExceptionHandler DeviceExceptionHandler { get; set; }

        void AddError(Error errorCode, params KeyValuePair<string, string>[] extras);
    }
}
