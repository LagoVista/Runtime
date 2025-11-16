// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 8bdef51353829d9010c9cd6434087077f6bc3d3c399f36484a3d26eeec88c7d2
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;

namespace LagoVista.IoT.Runtime.Core.Module
{
    /// <summary>
    /// The purpose of this class is to peform validation (i.e. regex, min, max etc...) prior to the final value being installed.
    /// </summary>
    public interface IMessageParserFieldValidator
    {
        InvokeResult Validate(DeviceMessageDefinitionField field, MessageValue msgValue);
    }
}
