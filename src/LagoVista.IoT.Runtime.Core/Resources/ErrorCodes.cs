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

        public static class PipelineEnqueing
        {
            public static ErrorCode InvalidMessageIndex { get { return new ErrorCode() { Code = "ENQ1001", Message = "The requested index number of the instruction is greater than the number of instructions" }; } }
            public static ErrorCode MissingPipelineQueue { get { return new ErrorCode() { Code = "ENQ1002", Message = "Requested Queue was Not Found" }; } }
        }
    }
}
