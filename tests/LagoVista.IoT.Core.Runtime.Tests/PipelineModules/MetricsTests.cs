using LagoVista.Core.Exceptions;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Core.Runtime.Tests.PipelineModules
{
    [TestClass]
    public class MetricsTests : PipelineModuleTestBase
    {
        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementMessageProcess()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.MessagesProcessed = 27;

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.MessagesProcessed);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldNotIncrementErrorMessageProcess()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.ErrorCount = 32;

            await ProcessMessageAsync(pem);

            Assert.AreEqual(32, PipelineModule.Metrics.ErrorCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldNotIncrementDeadLetterCount()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.DeadLetterCount = 34;

            await ProcessMessageAsync(pem);

            Assert.AreEqual(34, PipelineModule.Metrics.DeadLetterCount);
        }
       

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementMessageCountWithErrorProcess()
        {
            var pem = GetMessage();
            PipelineModule.Metrics.MessagesProcessed = 12;

            await ProcessMessageAsync(pem);

            Assert.AreEqual(13, PipelineModule.Metrics.MessagesProcessed);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementMessageCountWithException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.MessagesProcessed = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new Exception("Doesn't really matter.");
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.MessagesProcessed);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementMessageCountWithValidationException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.MessagesProcessed = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new ValidationException("Doesn't really matter.", new List<LagoVista.Core.Validation.ErrorMessage>() { new LagoVista.Core.Validation.ErrorMessage() { Message = "sometthing", ErrorCode = "err001" } });
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.MessagesProcessed);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementErrorCountWithException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.ErrorCount = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new Exception("Doesn't really matter.");
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.ErrorCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementErrorCountWithValidationException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.ErrorCount = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new ValidationException("Doesn't really matter.", new List<LagoVista.Core.Validation.ErrorMessage>() { new LagoVista.Core.Validation.ErrorMessage() { Message = "sometthing", ErrorCode = "err001" } });
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.ErrorCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementDeadLetterCountWithException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.DeadLetterCount = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new Exception("Doesn't really matter.");
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.DeadLetterCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementDeadLetterCountWithValidationException()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.DeadLetterCount = 27;

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                throw new ValidationException("Doesn't really matter.", new List<LagoVista.Core.Validation.ErrorMessage>() { new LagoVista.Core.Validation.ErrorMessage() { Message = "sometthing", ErrorCode = "err001" } });
            };

            await ProcessMessageAsync(pem);

            Assert.AreEqual(28, PipelineModule.Metrics.DeadLetterCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementErrorCountWithErrorProcess()
        {
            var pem = GetMessage();

            Assert.AreEqual(0, PipelineModule.Metrics.ErrorCount);

            PipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            PipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await ProcessMessageAsync(pem);

            Assert.AreEqual(1, PipelineModule.Metrics.ErrorCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementDeadLetterCountWithError()
        {
            var pem = GetMessage();

            PipelineModule.Metrics.DeadLetterCount = 15;

            PipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            PipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await ProcessMessageAsync(pem);

            Assert.AreEqual(16, PipelineModule.Metrics.DeadLetterCount);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldIncrementActiveCouldWhileProcessing()
        {
            var pem = GetMessage();

            Assert.AreEqual(0, PipelineModule.Metrics.ActiveCount);

            PipelineModule.ProcessHandler =  (PipelineExecutionMessage msg) =>
            {
                Assert.AreEqual(1, PipelineModule.Metrics.ActiveCount);
                return Task.FromResult(ProcessResult.FromSuccess);
            };

            await ProcessMessageAsync(pem);
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldReturnActiveCountToZerAfterProcess()
        {
            var pem = GetMessage();

            Assert.AreEqual(0, PipelineModule.Metrics.ActiveCount);

            PipelineModule.ProcessHandler = (PipelineExecutionMessage msg) =>
            {
                return Task.FromResult(ProcessResult.FromSuccess);
            };

            Assert.AreEqual(0, PipelineModule.Metrics.ActiveCount);

            await ProcessMessageAsync(pem);
        }


        [TestMethod]
        public async Task PipelineMetrics_ShouldSetExecutionTimeOnPEMInstruction()
        {
            var pem = GetMessage();

            Assert.AreEqual(0, PipelineModule.Metrics.ErrorCount);

            PipelineModule.ProcessHandler = async (PipelineExecutionMessage msg) =>
            {
                await Task.Delay(260);
                return ProcessResult.FromSuccess;
            };

            await ProcessMessageAsync(pem);

            await Task.Delay(200);

            Assert.IsTrue(pem.Instructions.First().ExecutionTimeMS > 249, $"ElapsedMS should be greater than 249, but was {pem.Instructions.First().ExecutionTimeMS}");
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldAddStepExecutionTimeAtPEMLevel()
        {
            var pem = GetMessage();
            pem.ExecutionTimeMS = 1000;

            Assert.AreEqual(0, PipelineModule.Metrics.ErrorCount);

            PipelineModule.ProcessHandler = async (PipelineExecutionMessage msg) =>
            {
                await Task.Delay(250);
                return ProcessResult.FromSuccess;
            };

            await ProcessMessageAsync(pem);

            await Task.Delay(200);

            Assert.IsTrue(pem.ExecutionTimeMS > 1249, $"ElapsedMS should be greater than 1249, but was {pem.ExecutionTimeMS}");
        }

        [TestMethod]
        public async Task PipelineMetrics_ShouldAddExecutionTimeToMetrics()
        {
            var pem = GetMessage();
            PipelineModule.Metrics.ProcessingMS = 1000;

            Assert.AreEqual(0, PipelineModule.Metrics.ErrorCount);

            PipelineModule.ProcessHandler = async (PipelineExecutionMessage msg) =>
            {
                await Task.Delay(250);
                return ProcessResult.FromSuccess;
            };

            await ProcessMessageAsync(pem);

            await Task.Delay(200);

            Assert.IsTrue(PipelineModule.Metrics.ProcessingMS > 1249, $"ElapsedMS should be greater than 1249, but was {PipelineModule.Metrics.ElapsedMS}");
        }
    }
}
