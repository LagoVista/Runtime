using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LagoVista.IoT.Core.Runtime.Tests.Messaging
{
    [TestClass]
    public class MessageValueTests
    {
        private void AssertInValid(ValidationResult result, params string[] errorCodes)
        {
            Assert.IsFalse(result.Successful);
            foreach (var err in errorCodes)
            {
                var validationError = result.Errors.Where(er => er.ErrorCode == err).FirstOrDefault();
                Assert.IsNotNull(validationError);
                Console.WriteLine(validationError.Message + "  " + validationError.Details);
            }
        }

        private void AssertValid(ValidationResult result)
        {
            foreach (var err in result.Errors)
            {
                Console.WriteLine(err.Message + "  " + err.Details);
            }
            Assert.IsTrue(result.Successful);
        }
        
        [TestMethod]
        public void MessageValue_MissingType_InValid()
        {
            var msgValue = new MessageValue()
            {
                Key = "one",
                Value = "1234"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.TypeNotSpecified.Code);
        }

        [TestMethod]
        public void MessageValue_MissingKey_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Value = "1234"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.KeyNotSpecified.Code);
        }

        [TestMethod]
        public void MessageValue_Int_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = "1234"
            };

            AssertValid(msgValue.Validate());
        }
    }
}
