// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 31fe8065059677a67370fe05cb3e1bcbc92f721531d9f8398dfdc22b215c7481
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Core.Runtime.Tests.Utils;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core;
using LagoVista.IoT.Runtime.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Runtime.Core.Processor;
using LagoVista.IoT.Runtime.Core.Services;
using LagoVista.IoT.Runtime.Core.Storage;
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
    public class ListenerTests
    {
        const string DEVICE_ID = "abc123";

        class GenericListener : ListenerModule
        {
            public GenericListener(ListenerConfiguration listenerConfiguration, IPEMBus pemBus) : base(listenerConfiguration, pemBus)
            {

            }

            public async override Task<ProcessResult> ProcessAsync(PipelineExecutionMessage message)
            {
                await AddStringMessageAsync(message.TextPayload, DateTime.Now, deviceId: message.Device.DeviceId, topic: message.Envelope.Topic);
                return ProcessResult.FromSuccess;
            }

            public override Task<InvokeResult> SendResponseAsync(PipelineExecutionMessage message, OutgoingMessage msg)
            {
                throw new NotImplementedException();
            }
        }

        Mock<ISensorEvaluator> _sensorEvaluator = new Mock<ISensorEvaluator>();
        Mock<IPEMBus> _pemBus = new Mock<IPEMBus>();
        Mock<INotificationPublisher> _notificationPublisher = new Mock<INotificationPublisher>();
        Mock<IDeviceStorage> _deviceStorage = new Mock<IDeviceStorage>();
        GenericListener _listener;
        LogWriter _logWriter = new LogWriter();

        Device _device;

        [TestInitialize]
        public void Init()
        {
            _device = new Device()
            {
                Id = "A94883B956564CEBB280B03DDB4EC6D9",
                DeviceId = DEVICE_ID,
                SensorCollection = new List<DeviceManagement.Models.Sensor>()
                {
                     new DeviceManagement.Models.Sensor()
                     {
                         PortIndex = 3,
                         Technology =  LagoVista.Core.Models.EntityHeader<DeviceManagement.Models.SensorTechnology>.Create(DeviceManagement.Models.SensorTechnology.ADC)
                     },
                     new DeviceManagement.Models.Sensor()
                     {
                         PortIndex = 5,
                         Technology =  LagoVista.Core.Models.EntityHeader<DeviceManagement.Models.SensorTechnology>.Create(DeviceManagement.Models.SensorTechnology.ADC)
                     },
                     new DeviceManagement.Models.Sensor()
                     {
                         PortIndex = 1,
                         Technology =  LagoVista.Core.Models.EntityHeader<DeviceManagement.Models.SensorTechnology>.Create(DeviceManagement.Models.SensorTechnology.IO)
                     },
                     new DeviceManagement.Models.Sensor()
                     {
                         PortIndex = 2,
                         Technology =  LagoVista.Core.Models.EntityHeader<DeviceManagement.Models.SensorTechnology>.Create(DeviceManagement.Models.SensorTechnology.IO)
                     }
                },
                DeviceRepository = new LagoVista.Core.Models.EntityHeader()
                {
                    Id = "1F4042102C3041F2989DDEE8006E2B56"
                }
            };

            _pemBus.Setup(pmb => pmb.NotificationPublisher).Returns(_notificationPublisher.Object);
            _pemBus.Setup(pmb => pmb.SensorEvaluator).Returns(_sensorEvaluator.Object);
            _pemBus.Setup(pmb => pmb.DeviceStorage).Returns(_deviceStorage.Object);
            _pemBus.Setup(pmb => pmb.InstanceLogger).Returns(new InstanceLogger(_logWriter, "1234", "1.2.3.4", "456"));
            _pemBus.Setup(pmb => pmb.Instance).Returns(new Deployment.Admin.Models.DeploymentInstance()
            {
                Id = "231228217F754DEFAA2F3F364E101D26",
                PrimaryHost = new LagoVista.Core.Models.EntityHeader<Deployment.Admin.Models.DeploymentHost>()
                {
                    Id = "0952B411513346EDBF553EA8150C87C2"
                }
            });
            _pemBus.Setup(pmb => pmb.Queues).Returns(new List<IPEMQueue>()
            {
                new Utils.TestInputQueue() {PipelineModuleId = "6BBDF856F5B4439ABD156C4DFD8C245E"}
            });

            _deviceStorage.Setup(ds => ds.GetDeviceByDeviceIdAsync(It.Is<string>(str => str == DEVICE_ID))).ReturnsAsync(_device);

            _listener = new GenericListener(new ListenerConfiguration() { Id = "6BBDF856F5B4439ABD156C4DFD8C245E" }, _pemBus.Object);
        }

        [TestMethod]
        public async Task ShouldApplySensorValues()
        {
            var pem = new PipelineExecutionMessage()
            {
                Device = _device,
                Envelope = new MessageEnvelope()
                {
                    Topic = $"nuviot/srvr/dvcsrvc/{DEVICE_ID}/iovalues",

                },
                TextPayload = "1.1,2.2,3.3,4.4,5.5,6.6,7.7,8.8,1.9,2.8,3.7,4.6,5.5,6.4,7.3,8.8"
            };

            await _listener.ProcessAsync(pem);

            Assert.AreEqual("4.6", _device.SensorCollection.Where(sns => sns.PortIndex == 3 && sns.Technology.Value == SensorTechnology.ADC).First().Value);
            Assert.AreEqual("6.4", _device.SensorCollection.Where(sns => sns.PortIndex == 5 && sns.Technology.Value == SensorTechnology.ADC).First().Value);

            Assert.AreEqual("2.2", _device.SensorCollection.Where(sns => sns.PortIndex == 1 && sns.Technology.Value == SensorTechnology.IO).First().Value);
            Assert.AreEqual("3.3", _device.SensorCollection.Where(sns => sns.PortIndex == 2 && sns.Technology.Value == SensorTechnology.IO).First().Value);

            _deviceStorage.Verify(ds => ds.UpdateDeviceAsync(It.Is<Device>(dvc => dvc.DeviceId == DEVICE_ID)), Times.Once);
            _notificationPublisher.Verify(np => np.PublishAsync(
                    It.IsAny<Targets>(),
                    It.Is<Notification>(not => not.Channel.Value == Channels.Device && not.ChannelId == _device.Id),
                    It.IsAny<NotificationVerbosity>()), Times.Once);
        }
    }
}
