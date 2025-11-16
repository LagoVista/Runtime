// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d483c84f875c1cc8ab6efab898457fd9a134e89e0c0fb41b18c824bccf2050ae
// IndexVersion: 2
// --- END CODE INDEX META ---
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
