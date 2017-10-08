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
            foreach (var err in result.Errors)
            {
                Console.WriteLine(err.Message + "  " + err.Details);
            }

            foreach (var err in errorCodes)
            {
                var validationError = result.Errors.Where(er => er.ErrorCode == err).FirstOrDefault();
                Assert.IsNotNull(validationError);
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
        public void MessageValue_HasValue()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = "1234"
            };

            AssertValid(msgValue.Validate());
            Assert.IsTrue(msgValue.HasValue);
        }


        [TestMethod]
        public void MessageValue_DoesNotHavValue_Null()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = null
            };

            AssertValid(msgValue.Validate());
            Assert.IsFalse(msgValue.HasValue);
        }

        [TestMethod]
        public void MessageValue_DoesNotHavValue_EmptyString()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = String.Empty
            };

            AssertValid(msgValue.Validate());
            Assert.IsFalse(msgValue.HasValue);
            Assert.IsNull(msgValue.Value);
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

        [TestMethod]
        public void MessageValue_Int_Parsed()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = "1234"
            };

           Assert.AreEqual(1234, msgValue.GetIntValue());
        }

        [TestMethod]
        public void MessageValue_Int_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = "1234AB"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidInteger.Code);
        }


        [TestMethod]
        public void MessageValue_IntWithDecimalPoints_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Integer)
            {
                Key = "one",
                Value = "1234.45"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidInteger.Code);
        }


        [TestMethod]
        public void MessageValue_Decimal_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Decimal)
            {
                Key = "one",
                Value = "1234.54"
            };

            AssertValid(msgValue.Validate());
        }

        [TestMethod]
        public void MessageValue_Decimal_Parsed()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Decimal)
            {
                Key = "one",
                Value = "1234.54"
            };

            Assert.AreEqual(1234.54, msgValue.GetDouble());
        }

        [TestMethod]
        public void MessageValue_Decimal_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.Decimal)
            {
                Key = "one",
                Value = "1234AB"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvlidDecimal.Code);
        }

        [TestMethod]
        public void MessageValue_DateTime_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.DateTime)
            {
                Key = "one",
                Value = "2017-10-08T03:30:00.000Z"
            };

            AssertValid(msgValue.Validate());
        }


        [TestMethod]
        public void MessageValue_DateTime_Parsed()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.DateTime)
            {
                Key = "one",
                Value = "2017-10-08T03:30:00.000Z"
            };
            var date = msgValue.GetUniversalDateTime();

            Assert.AreEqual(2017, date.Year);
            Assert.AreEqual(10, date.Month);
            Assert.AreEqual(8, date.Day);
            Assert.AreEqual(3, date.Hour);
            Assert.AreEqual(30, date.Minute);
        }

        [TestMethod]
        public void MessageValue_DateTime_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.DateTime)
            {
                Key = "one",
                Value = "2017-10-08T03:30.000Z"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidDateTime.Code);
        }

        [TestMethod]
        public void MessageValue_DateTime_OutOfRange_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.DateTime)
            {
                Key = "one",
                Value = "2017-10-08T03:30:90.000Z"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidDateTime.Code);
        }


        [TestMethod]
        public void MessageValue_GeoLocation_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "22.234512,-122.423413"
            };

            AssertValid(msgValue.Validate());
        }

        [TestMethod]
        public void MessageValue_GeoLocation_Parsed()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "22.234512,-122.423413"
            };

            AssertValid(msgValue.Validate());
            Assert.AreEqual(22.234512, msgValue.GetGeoLocation().Latitude);
            Assert.AreEqual(-122.423413, msgValue.GetGeoLocation().Longitude);
        }

        [TestMethod]
        public void MessageValue_GeoLocation_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "22.234512,-122.4234dd13"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidGeoLocation.Code);
        }

        [TestMethod]
        public void MessageValue_GeoLocation_LatOutOfRanagePositive_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "92.234512,-122.423413"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidLatitude.Code);
        }

        [TestMethod]
        public void MessageValue_GeoLocation_LatOutOfRanageNegative_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "-92.234512,-122.423213"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidLatitude.Code);
        }

        [TestMethod]
        public void MessageValue_GeoLocation_LonOutOfRanagePositive_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "22.234512,-180.423413"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidLongitude.Code);
        }

        [TestMethod]
        public void MessageValue_GeoLocation_LonOutOfRanageNegative_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.GeoLocation)
            {
                Key = "one",
                Value = "-22.234512,-180.423213"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidLongitude.Code);
        }

        [TestMethod]
        public void MessageValue_StateSet_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.State)
            {
                Key = "one",
                Value = "state1",
                StateSet = new LagoVista.Core.Models.EntityHeader<DeviceAdmin.Models.StateSet>()
                {
                    Id="abc123",
                    Value=new DeviceAdmin.Models.StateSet()
                    {
                        States = new System.Collections.Generic.List<DeviceAdmin.Models.State>()
                        {
                            new DeviceAdmin.Models.State() { Key="state1"},
                            new DeviceAdmin.Models.State() { Key="state2"},
                        }
                    }
                }
            };

            AssertValid(msgValue.Validate());
        }


        [TestMethod]
        public void MessageValue_StateSet_MissingStateSet_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.State)
            {
                Key = "one",
                Value = "state1"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.StateSetSpecifiedButNotProvided.Code);
        }

        [TestMethod]
        public void MessageValue_StateSet_InvalidState_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.State)
            {
                Key = "one",
                Value = "doesntexist",
                StateSet = new LagoVista.Core.Models.EntityHeader<DeviceAdmin.Models.StateSet>()
                {
                    Id = "abc123",
                    Value = new DeviceAdmin.Models.StateSet()
                    {
                        States = new System.Collections.Generic.List<DeviceAdmin.Models.State>()
                        {
                            new DeviceAdmin.Models.State() { Key="state1"},
                            new DeviceAdmin.Models.State() { Key="state2"},
                        }
                    }
                }
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.InvalidStateValue.Code);
        }


        [TestMethod]
        public void MessageValue_UnitSet_Valid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.ValueWithUnit)
            {
                Key = "one",
                Value = "32.5",
                UnitSet = new LagoVista.Core.Models.EntityHeader<DeviceAdmin.Models.UnitSet>() { Id="123", Text="abc", Value = new DeviceAdmin.Models.UnitSet() { Id="123", Key="abc"} }
            };

            AssertValid(msgValue.Validate());
        }

        [TestMethod]
        public void MessageValue_UnitSet_MissingUnitSet_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.ValueWithUnit)
            {
                Key = "one",
                Value = "state1"
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.UnitTypeSpecifiedButnotProvided.Code);
        }


        [TestMethod]
        public void MessageValue_UnitSet_InvalidValue_InValid()
        {
            var msgValue = new MessageValue(DeviceAdmin.Models.ParameterTypes.ValueWithUnit)
            {
                Key = "one",
                Value = "32.f",
                UnitSet = new LagoVista.Core.Models.EntityHeader<DeviceAdmin.Models.UnitSet>() { Id = "123", Text = "abc", Value = new DeviceAdmin.Models.UnitSet() { Id = "123", Key = "abc" } }
            };

            AssertInValid(msgValue.Validate(), LagoVista.IoT.Runtime.Core.Resources.ErrorCodes.Messaging.ValueOnUnitSetNotDouble.Code);
        }

    }
}
