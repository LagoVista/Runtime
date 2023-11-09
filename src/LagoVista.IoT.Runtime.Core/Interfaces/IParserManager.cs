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
        IMessageFieldParser GetFieldMessageParser(DeviceField parserConfig, IInstanceLogger logger);

        IMessageParser GetMessageParser(DeviceMessageDefinition parserConfig, IInstanceLogger logger);
    }
}
