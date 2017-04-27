using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Models.PEM;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public enum ParserTypes
    {
        Text,
        Binary,
        TextOrBinary
    }

    public interface IMessageFieldParser
    {
        MessageFieldParserResult Parse(PipelineExectionMessage msg);

        ParserTypes ParserType { get; }
    }
    public interface IParserManager
    {

        IMessageFieldParser GetFieldMessageParser(IMessageFieldParserConfiguration parserConfig, ILogger logger);
    }
}
