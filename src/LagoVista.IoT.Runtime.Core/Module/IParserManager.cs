using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public interface IMessageParser
    {

    }

    public interface IParserManager
    {
        IMessageFieldParser GetFieldMessageParser(IMessageFieldParserConfiguration parserConfig, ILogger logger);

        IMessageParser GetMessageParser(IMessageFieldParserConfiguration parserConfig, ILogger logger);
    }

    public interface IFieldParserVerifierRuntime
    {
        Task<VerificationResult> VerifyAsync(VerificationRequest<MessageFieldParserConfiguration> request);
    }
}
