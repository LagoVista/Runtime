using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.IoT.DeviceManagement.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models
{
    public class DeviceForNotification
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public GeoLocation GoeLocation { get; set; }
        public double Heading { get; set; }
        public string SerialNumber { get; set; }
        public string LastContact { get; set; }
        public EntityHeader Status { get; set; }
        public EntityHeader CustomStatus { get; set; }
        public string FirmwareVersion { get; set; }
        public Dictionary<string, object> PropertyBag { get; set; }
        public IEnumerable<AttributeValue> Attributes { get; set; }
        public IEnumerable<AttributeValue> States { get; set; }
        public IEnumerable<AttributeValue> Properties { get; set; }

        public static DeviceForNotification FromDevice(LagoVista.IoT.DeviceManagement.Core.Models.Device device)
        {
            return new DeviceForNotification()
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                Id = device.Id,
                GoeLocation = device.GeoLocation,
                Heading = device.Heading,
                Attributes = device.Attributes,
                Properties = device.Properties,
                SerialNumber = device.SerialNumber,
                FirmwareVersion = device.FirmwareVersion,
                CustomStatus = device.CustomStatus,
                PropertyBag = device.PropertyBag,
                Status = device.Status,
                States = device.States,
                LastContact = device.LastContact
            };
        }
    }
}
