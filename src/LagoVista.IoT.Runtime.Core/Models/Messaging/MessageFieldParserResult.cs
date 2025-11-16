// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e645cd903e6af2a5eb9ee0aa028f509078ffbe0f7f4b81eb2de78cdc1c635423
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core.Models.Messaging
{
    public struct MessageFieldParserResult
    {
        public bool Success { get; set; }
        public String FailureReason { get; set; }
        public String InvalidConfiguration { get; set; }

        public string Result { get; set; }

        public static MessageFieldParserResult FromSuccess(String result)
        {
            return new MessageFieldParserResult()
            {
                Success = true,
                Result = result
            };
        }

        public static MessageFieldParserResult FromFailure(String reason)
        {
            return new MessageFieldParserResult()
            {
                Success = false,
                FailureReason = reason
            };
        }

        public static MessageFieldParserResult FromInvalidConfiguration(String reason)
        {
            return new MessageFieldParserResult()
            {
                Success = false,
                InvalidConfiguration = reason
            };
        }
    }
}
