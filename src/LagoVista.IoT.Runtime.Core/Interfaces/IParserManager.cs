// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 7b21a79248905f857c001f4f1fe2197cb5681c0263b3290b1e9ca5c651bb2f1d
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public enum ParserTypes
    {
        Text,
        Binary,
        TextOrBinary
    }

    public interface IParserManager
    {
        IMessageFieldParser GetFieldMessageParser(MessageAttributeParser parserConfig, IInstanceLogger logger);

        IMessageParser GetMessageParser(DeviceMessageDefinition parserConfig, IInstanceLogger logger);
    }
}
