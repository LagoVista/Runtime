using LagoVista.IoT.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Resources
{
    public static class ErrorCodes
    {
        public static class Common
        {
            public static ErrorCode InvalidConfiguration { get { return new ErrorCode() { Code = "CMNR1001", Message = "Invalid Configuartion, See Details" }; } }
        }
    }
}
