// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b538ee2bcf68ba8f09b8146a2c7143216a9c8662b2fdcd1e8a995e38514d655f
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LagoVista.Core;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LagoVista.IoT.Core.Runtime.Tests.PipelineModules
{
    [TestClass]
    public class MetricsRollupTests : PipelineModuleTestBase
    {
        [TestMethod]
        public void MeticsRollupTest_ShouldResetCounts()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.BytesProcessed = 2000000;
            PipelineModule.Metrics.MessagesProcessed = 120;
            PipelineModule.Metrics.DeadLetterCount= 300;
            PipelineModule.Metrics.WarningCount = 120;
            PipelineModule.Metrics.ErrorCount = 50;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(0, PipelineModule.Metrics.BytesProcessed);
            Assert.AreEqual(0, PipelineModule.Metrics.MessagesProcessed);
            Assert.AreEqual(0, PipelineModule.Metrics.ErrorCount);
            Assert.AreEqual(0, PipelineModule.Metrics.WarningCount);
            Assert.AreEqual(0, PipelineModule.Metrics.DeadLetterCount);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldCopyRolledUpValues()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.BytesProcessed = 2000000;
            PipelineModule.Metrics.MessagesProcessed = 120;
            PipelineModule.Metrics.DeadLetterCount = 300;
            PipelineModule.Metrics.WarningCount = 120;
            PipelineModule.Metrics.ErrorCount = 50;
            PipelineModule.Metrics.ActiveCount = 123;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(123, result.ActiveCount);
            Assert.AreEqual(2000000, result.BytesProcessed);
            Assert.AreEqual(120, result.MessagesProcessed);
            Assert.AreEqual(50, result.ErrorCount);
            Assert.AreEqual(120, result.WarningCount);
            Assert.AreEqual(300, result.DeadLetterCount);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldIdProperties()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();
            PipelineModule.Metrics.HostId = Instance.PrimaryHost.Id;
            PipelineModule.Metrics.InstanceId = Instance.Id;
            PipelineModule.Metrics.PipelineModuleId = PipelineModule.Id;

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(Instance.PrimaryHost.Id, result.HostId);
            Assert.AreEqual(Instance.Id, result.InstanceId);
            Assert.AreEqual(PipelineModule.Id, result.PipelineModuleId);
            Assert.AreEqual(PipelineModule.Status.ToString(), result.Status);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldAddVersionProperty()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual("1.2.3.4", result.Version);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldSetPartitionKey()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");
            
            Assert.AreEqual(PipelineModule.Id, result.PartitionKey);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldSetRowKey()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.IsFalse(String.IsNullOrEmpty(result.RowKey));
            Assert.IsTrue(32 < result.RowKey.Length); /* This will be based on the time stamp and a random guid and timestamp, can't determine exact way (well not worth it, assume if it's longer than 32 characters we probably created the right row key */
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldProcessingMS()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.ProcessingMS = 22425;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(0, PipelineModule.Metrics.ProcessingMS);
        }


        [TestMethod]
        public void MeticsRollupTest_ShouldMaintainActiveAcount()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.ActiveCount = 23;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(23, PipelineModule.Metrics.ActiveCount);
        }


        [TestMethod]
        public void MeticsRollupTest_ShouldSetPropertStartEndTimesOnRolledUpRecord()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;
            
            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(rollupTime.ToJSONString(), result.EndTimeStamp);
            Assert.AreEqual(rollupTime.ToJSONString(), PipelineModule.Metrics.StartTimeStamp);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldCalculateElapsedMS()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual((rollupTime - startTime).TotalMilliseconds, result.ElapsedMS, 0.01);
        }

        [TestMethod]
        public void MeticsRollupTest_ShouldSetStartTimeAndHaveEmptyEndOfOfCurrentToRollup()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(rollupTime.ToJSONString(), PipelineModule.Metrics.StartTimeStamp);
            Assert.IsTrue(String.IsNullOrEmpty(PipelineModule.Metrics.EndTimeStamp));
        }

        [TestMethod]
        public void MeticsRollupTest_RollupAverageProcessingTime()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.MessagesProcessed = 120;
            PipelineModule.Metrics.ProcessingMS = 20000;

            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(result.ProcessingMS / result.MessagesProcessed, result.AverageProcessingMS, 0.01);
        }


        [TestMethod]
        public void MeticsRollupTest_RollupMessagesPerSecond()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var rollupTime = DateTime.UtcNow;

            PipelineModule.Metrics.MessagesProcessed = 120;
            
            PipelineModule.Metrics.StartTimeStamp = startTime.ToJSONString();

            var result = PipelineModule.GetAndResetMetrics(rollupTime, "1.2.3.4");

            Assert.AreEqual(result.MessagesProcessed / (rollupTime - startTime).TotalSeconds, result.MessagesPerSecond, 0.01);
        }


    }
}
