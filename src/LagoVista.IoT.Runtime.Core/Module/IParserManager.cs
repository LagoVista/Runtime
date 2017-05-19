using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
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
        IMessageFieldParser GetFieldMessageParser(DeviceMessageDefinitionField parserConfig, ILogger logger);

        IMessageParser GetMessageParser(DeviceMessageDefinition parserConfig, ILogger logger);
    }
}
