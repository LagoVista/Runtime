using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Models.PEM;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IMessageFieldParser
    {
        MessageFieldParserResult Parse(PipelineExecutionMessage msg);

        ParserTypes ParserType { get; }
     }
}
