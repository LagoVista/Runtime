using LagoVista.IoT.Core.Runtime.Tests.Utils;
using LagoVista.IoT.DeviceAdmin.Interfaces;
using LagoVista.IoT.DeviceManagement.Core;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Runtime.Core.Storage;
using LagoVista.IoT.Runtime.Core.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Core.Runtime.Tests.PipelineModules
{
    [TestClass]
    public class PipelineModuleExecuteTests
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
        Mock<IDeviceManager> _deviceManager = new Mock<IDeviceManager>();

        List<IPEMQueue> _queues = new List<IPEMQueue>();

        [TestInitialize]
        public void Init()
        {
            _pemBus.Setup(pmb => pmb.Instance).Returns(new Deployment.Admin.Models.DeploymentInstance()
            {
                PrimaryHost = new LagoVista.Core.Models.EntityHeader<Deployment.Admin.Models.DeploymentHost>() { Id = "1234" },
                Id = "456",
                DeviceRepository = new LagoVista.Core.Models.EntityHeader<DeviceManagement.Core.Models.DeviceRepository>()
                {
                    Value = new DeviceManagement.Core.Models.DeviceRepository(),
                },
            });
            _pemBus.Setup(pmb => pmb.DeviceManager).Returns(_deviceManager.Object);
            _pemBus.Setup(pmb => pmb.SystemUsers).Returns(_systemUsers.Object);
            _pemBus.Setup(pmb => pmb.Queues).Returns(_queues);
            _pemBus.Setup(pmb => pmb.PEMStorage).Returns(_pemStorage.Object);
            _pemBus.Setup(pmb => pmb.InstanceLogger).Returns(new InstanceLogger(_logWriter, "1234", "1.2.3.4", "456"));
            _pemBus.Setup(pmb => pmb.NotificationPublisher).Returns(_notificationPublisher.Object);
            _queues.Add(_listenerQueue);
            _queues.Add(_outputQueue);

            _pipelineModule = new Utils.TestPipelineModule(_pipelineModuleConfiguration.Object, _pemBus.Object, _moduleHost.Object,
                _listenerQueue, _outputQueue, _secondaryOutputQueues);

        }

        public PipelineExecutionMessage GetMessage()
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

                },
                Instructions = new List<PipelineExecutionInstruction>(),
                Status = LagoVista.Core.Models.EntityHeader<StatusTypes>.Create(StatusTypes.PendingExecution)
            };

            msg.Instructions.Add(msg.CurrentInstruction);

            return msg;
        }

        [TestMethod]
        public async Task Pipeline_Execute_BasicSuccessPath()
        {
            var pem = GetMessage();
            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            // Give it time to process on a different thread
            await Task.Delay(150);

            await _pipelineModule.StopAsync();

            Assert.IsFalse(_logWriter.ErrorRecords.Any());
        }

        [TestMethod]
        public async Task Pipeline_Execute_InitialQueueShouldBeEmptyAfterProcessing()
        {
            var pem = GetMessage();
            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            // Give it time to process on a different thread
            await Task.Delay(150);
            Assert.IsTrue(await _listenerQueue.CheckIfEmptyAsync());

            await _pipelineModule.StopAsync();

            Assert.IsFalse(_logWriter.ErrorRecords.Any());
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldSetCompleteStatusWithOneInstruction()
        {
            var pem = GetMessage();
            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);

            await _pipelineModule.StopAsync();

            Assert.AreEqual(StatusTypes.Completed, pem.Status.Value);
        }


        [TestMethod]
        public async Task Pipeline_Execute_ShouldAddToPemStorageOnSuccess()
        {
            var pem = GetMessage();
            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);

            await _pipelineModule.StopAsync();

            _pemStorage.Verify(pms => pms.AddMessageAsync(pem), Times.Once);
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldEnqeueForSecondStatepForTwoInstruction()
        {
            var pem = GetMessage();
            pem.Instructions.Add(new PipelineExecutionInstruction()
            {
                Name = "Second Step",
                QueueId = "MODULE2",
                Type = "ANOTHEREQUEUE"
            });

            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);
            await _pipelineModule.StopAsync();

            Assert.AreEqual(StatusTypes.PendingExecution, pem.Status.Value);
            Assert.IsTrue(_outputQueue.ContainsMessage(pem.Id));
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldNotAddToPEMStorageIfAdditionalSteps()
        {
            var pem = GetMessage();
            pem.Instructions.Add(new PipelineExecutionInstruction()
            {
                Name = "Second Step",
                QueueId = "MODULE2",
                Type = "ANOTHEREQUEUE"
            });

            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);
            await _pipelineModule.StopAsync();

            _pemStorage.Verify(pms => pms.AddMessageAsync(pem), Times.Never);
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldSetFailedStatusIfFailed()
        {
            var pem = GetMessage();

            _pipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            _pipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);
            await _pipelineModule.StopAsync();

            Assert.AreEqual(StatusTypes.Failed, pem.Status.Value);
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldAddToDeadLeaderIfFailed()
        {
            var pem = GetMessage();

            _pipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            _pipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await _pipelineModule.StartAsync();
            await _listenerQueue.EnqueueAsync(pem);
            await Task.Delay(150);
            await _pipelineModule.StopAsync();

            _pemStorage.Verify(pms => pms.AddToDeadLetterStorageAsync(pem), Times.Once);
        }

    }
}
