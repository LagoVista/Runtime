// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ef56ec55aef97cb1b982f1cce63176c1942aed155c052557b43a0efccf374744
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IMessageParser
    {
        InvokeResult Parse(Runtime.Core.Models.PEM.PipelineExecutionMessage pem, DeviceMessageDefinition definition);
    }
}
