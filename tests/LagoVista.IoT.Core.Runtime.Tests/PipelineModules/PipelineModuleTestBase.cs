// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2697639a6edf1d5ce2e6788269ba94c7725cd28f137afc8918c54160ebd09eb4
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Core.Runtime.Tests.Utils;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.DeviceManagement.Core;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Runtime.Core.Storage;
using LagoVista.IoT.Runtime.Core.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Core.Runtime.Tests.PipelineModules
{
    public class PipelineModuleTestBase
    {
        TestPipelineModule _pipelineModule;
        Mock<IPipelineModuleConfiguration> _pipelineModuleConfiguration = new Mock<IPipelineModuleConfiguration>();
        Mock<IPEMBus> _pemBus = new Mock<IPEMBus>();
        Mock<IPipelineModuleRuntime> _moduleHost = new Mock<IPipelineModuleRuntime>();
        TestInputQueue _listenerQueue = new Utils.TestInputQueue() { PipelineModuleId = "MODULE1" };
        TestInputQueue _outputQueue = new Utils.TestInputQueue() { PipelineModuleId = "MODULE2" };
        List<IPEMQueue> _secondaryOutputQueues = new List<IPEMQueue>();
        Mock<IPEMStorage> _pemStorage = new Mock<IPEMStorage>();
        Mock<INotificationPublisher> _notificationPublisher = new Mock<INotificationPublisher>();
        LogWriter _logWriter = new LogWriter();
        Mock<ISystemUsers> _systemUsers = new Mock<ISystemUsers>();
        Mock<IDeviceStorage> _deviceManager = new Mock<IDeviceStorage>();

        List<IPEMQueue> _queues = new List<IPEMQueue>();

        [TestInitialize]
        public void Init()
        {
            Instance = new Deployment.Admin.Models.DeploymentInstance()
            {
                PrimaryHost = new LagoVista.Core.Models.EntityHeader<Deployment.Admin.Models.DeploymentHost>() { Id = "hostid1234" },
                Id = "instanceid1234",
                DeviceRepository = new LagoVista.Core.Models.EntityHeader<DeviceManagement.Core.Models.DeviceRepository>()
                {
                    Value = new DeviceManagement.Core.Models.DeviceRepository(),
                },
            };

            _pipelineModuleConfiguration.Setup(mod => mod.Id).Returns("MODULE1");

            _pemBus.Setup(pmb => pmb.Instance).Returns(Instance);
            _pemBus.Setup(pmb => pmb.DeviceStorage).Returns(_deviceManager.Object);
            _pemBus.Setup(pmb => pmb.SystemUsers).Returns(_systemUsers.Object);
            _pemBus.Setup(pmb => pmb.Queues).Returns(_queues);
            _pemBus.Setup(pmb => pmb.PEMStorage).Returns(_pemStorage.Object);
            _pemBus.Setup(pmb => pmb.InstanceLogger).Returns(new InstanceLogger(_logWriter, "1234", "1.2.3.4", "456"));
            _pemBus.Setup(pmb => pmb.NotificationPublisher).Returns(_notificationPublisher.Object);
            _queues.Add(_listenerQueue);
            _queues.Add(_outputQueue);

            _pipelineModule = new Utils.TestPipelineModule(_pipelineModuleConfiguration.Object, _pemBus.Object);
        }

        protected PipelineExecutionMessage GetMessage()
        {
            var msg = new PipelineExecutionMessage()
            {
                Id = "messageid",
                ErrorMessages = new List<Logging.Error>(),
                CurrentInstruction = new PipelineExecutionInstruction()
                {
                    Name = "First Step",
                    QueueId = "MODULE1",
                    Type = "Process"
                },
                Device = new DeviceManagement.Core.Models.Device()
                {
                    DeviceRepository = EntityHeader.Create("id", "text")
                },
               
                Instructions = new List<PipelineExecutionInstruction>(),
                Status = StatusTypes.PendingExecution
            };

            msg.Instructions.Add(msg.CurrentInstruction);

            return msg;
        }

        protected async Task ProcessMessageAsync(PipelineExecutionMessage pem)
        {
            await _pipelineModule.PorcessMessageForTestAsync(pem);
      //      await _listenerQueue.EnqueueAsync(pem);
            // Give it time to process on a different thread
        //    await Task.Delay(500);

          //  await _pipelineModule.StopAsync();
        }

        protected LogWriter LogWriter { get { return _logWriter; } }
        protected Mock<IPEMStorage> PemStorageMock { get { return _pemStorage; } }

        protected TestPipelineModule PipelineModule { get { return _pipelineModule; } }

        protected TestInputQueue OutputQueue { get { return _outputQueue; } }

        protected DeploymentInstance Instance { get; private set; }
    }
}
