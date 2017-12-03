using LagoVista.IoT.Core.Runtime.Tests.Utils;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.IoT.Core.Runtime.Tests.PipelineModules
{
    [TestClass]
    public class PipelineModuleExecuteTests : PipelineModuleTestBase
    {
        [TestMethod]
        public async Task Pipeline_Execute_BasicSuccessPath()
        {
            var pem = GetMessage();

            await ProcessMessageAsync(pem);
    
            Assert.IsFalse(LogWriter.ErrorRecords.Any());
        }

        [TestMethod]
        public async Task Pipeline_Execute_InitialQueueShouldBeEmptyAfterProcessing()
        {
            var pem = GetMessage();

            await ProcessMessageAsync(pem);

            Assert.IsFalse(LogWriter.ErrorRecords.Any());
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldSetCompleteStatusWithOneInstruction()
        {
            var pem = GetMessage();

            await ProcessMessageAsync(pem);

            Assert.AreEqual(StatusTypes.Completed, pem.Status);
        }


        [TestMethod]
        public async Task Pipeline_Execute_ShouldAddToPemStorageOnSuccess()
        {
            var pem = GetMessage();

            await ProcessMessageAsync(pem);

            PemStorageMock.Verify(pms => pms.AddMessageAsync(pem), Times.Once);
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

            await ProcessMessageAsync(pem);

            Assert.AreEqual(StatusTypes.PendingExecution, pem.Status);
            Assert.IsTrue(OutputQueue.ContainsMessage(pem.Id));
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

            await ProcessMessageAsync(pem);

            PemStorageMock.Verify(pms => pms.AddMessageAsync(pem), Times.Never);
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldSetFailedStatusIfFailed()
        {
            var pem = GetMessage();

            PipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            PipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await ProcessMessageAsync(pem);

            Assert.AreEqual(StatusTypes.Failed, pem.Status);
        }

        [TestMethod]
        public async Task Pipeline_Execute_ShouldAddToDeadLeaderIfFailed()
        {
            var pem = GetMessage();

            PipelineModule.ResultToReturn = new IoT.Runtime.Core.Processor.ProcessResult();
            var failedError = new Logging.Error() { Message = "IT FAILED!", ErrorCode = "ERR001" };
            PipelineModule.ResultToReturn.ErrorMessages.Add(failedError);

            await ProcessMessageAsync(pem);

            PemStorageMock.Verify(pms => pms.AddToDeadLetterStorageAsync(pem), Times.Once);
        }
    }
}
