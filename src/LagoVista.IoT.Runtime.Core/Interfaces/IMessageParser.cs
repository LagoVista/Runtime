using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IMessageParser
    {
        InvokeResult Parse(Runtime.Core.Models.PEM.PipelineExecutionMessage pem, DeviceMessageDefinition definition);
    }
}
