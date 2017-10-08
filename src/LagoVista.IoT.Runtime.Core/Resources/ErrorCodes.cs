using LagoVista.IoT.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Resources
{
    //TODO: Need to make sure this gets registered with the global Logging Error Code registery.
    public static class ErrorCodes
    {
        public static class Common
        {
            public static ErrorCode InvalidConfiguration { get { return new ErrorCode() { Code = "CMNR1001", Message = "Invalid Configuartion, See Details" }; } }


        }

        public static class Verifiers
        {
            public static ErrorCode MissingBinaryInput { get { return new ErrorCode() { Code = "VERF1001", Message = "Missing Input to Convert to Binary Payload" }; } }
            public static ErrorCode CouldNotConvertInputToBytes { get { return new ErrorCode() { Code = "VERF1002", Message = "Could not convert input into Binary Payload" }; } }
        }

        public static class Messaging
        {
            public static ErrorCode TypeNotSpecified { get { return new ErrorCode() { Code = "MGV1001", Message = "Type of Message Value is Not Specified" }; } }
            public static ErrorCode KeyNotSpecified { get { return new ErrorCode() { Code = "MGV1002", Message = "Key on Message Value is Not Specified" }; } }
            public static ErrorCode UnitTypeSpecifiedButnotProvided { get { return new ErrorCode() { Code = "MGV1003", Message = "Data Type was specified as Value With Unit but Unit Set was empty." }; } }
            public static ErrorCode ValueOnUnitSetNotDouble { get { return new ErrorCode() { Code = "MGV1004", Message = "Value provided on Unit Set is not Numeric." }; } }
            public static ErrorCode ValueOnBoolIsNotBoolean { get { return new ErrorCode() { Code = "MGV1005", Message = "Value provided on Boolean can not be parsed to a boolean value." }; } }
            public static ErrorCode StateSetSpecifiedButNotProvided { get { return new ErrorCode() { Code = "MGV1006", Message = "Data Type as specified as State Set, but not State Set was provide." }; } }
            public static ErrorCode InvalidStateValue { get { return new ErrorCode() { Code = "MGV1007", Message = "Data Type as specified as State Set, but provided value was not found in states, see details." }; } }
            public static ErrorCode InvalidInteger { get { return new ErrorCode() { Code = "MGV1008", Message = "Value provided on Integer can not be parsed to an integer value." }; } }
            public static ErrorCode InvlidDecimal { get { return new ErrorCode() { Code = "MGV1009", Message = "Value provided on Decimal can not be parsed to a Decimal value." }; } }
            public static ErrorCode InvalidGeoLocation { get { return new ErrorCode() { Code = "MGV1010", Message = "Value provided for Geolocation is invalid, should be (-)dd.dddddd,(-)ddd.dddddd." }; } }
            public static ErrorCode InvalidLatitude { get { return new ErrorCode() { Code = "MGV1011", Message = "Value provided for Latitude on Geolocation is out uof range." }; } }
            public static ErrorCode InvalidLongitude { get { return new ErrorCode() { Code = "MGV1011", Message = "Value provided for Longitude on Geolcation is out of range." }; } }
            public static ErrorCode InvalidDateTime { get { return new ErrorCode() { Code = "MGV1011", Message = "Invalid value stored for DateTime, format should be YYYY-MM-DDTHH:MM:SS.sssZ." }; } }
        }

        public static class PipelineEnqueing
        {
            public static ErrorCode InvalidMessageIndex { get { return new ErrorCode() { Code = "ENQ1001", Message = "The requested index number of the instruction is greater than the number of instructions" }; } }
            public static ErrorCode MissingPipelineQueue { get { return new ErrorCode() { Code = "ENQ1002", Message = "Requested Queue was Not Found" }; } }
        }
    }
}
