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
