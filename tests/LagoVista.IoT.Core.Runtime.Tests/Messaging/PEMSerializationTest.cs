// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d961c3d852c1847c2c96ce9db86b3efa4355f1e84a108d132b869543703b8d39
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Runtime.Core.Models.PEM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Core.Runtime.Tests.Messaging
{
    [TestClass]
    public class PEMSerializationTest
    {
        [TestMethod]
        public void PEM_Serialization_BinaryData()
        {
            /* Ensure if we stuff binary data into BinaryPayload field it will be serialzied and deserialized propertly */
            var container = new PipelineExecutionMessage();
            container.BinaryPayload = new byte[] { 0x34, 0x03, 0xAC, 0xDD };
            var json = JsonConvert.SerializeObject(container);
            var data = JsonConvert.DeserializeObject<PipelineExecutionMessage>(json);
            Assert.IsNotNull(data.BinaryPayload);
            Assert.AreEqual(4, data.BinaryPayload.Length);
            Assert.AreEqual(0x34, data.BinaryPayload[0]);
            Assert.AreEqual(0x03, data.BinaryPayload[1]);
            Assert.AreEqual(0xAC, data.BinaryPayload[2]);
            Assert.AreEqual(0xDD, data.BinaryPayload[3]);
        }
    }
}
