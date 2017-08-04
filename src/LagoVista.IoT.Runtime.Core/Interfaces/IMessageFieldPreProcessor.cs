using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;

namespace LagoVista.IoT.Runtime.Core.Module
{
    /// <summary>
    /// This class is responsible for doing any assembly or modifying of the content prior to being assigned to the final value
    /// an example of this is if the field definition has a RegExValueLocator to extract a sub string of the found string.  That extraction via
    /// reg ex would happen here.
    /// </summary>
    public interface IMessageFieldPreProcessor
    {
        InvokeResult PreProcess(DeviceMessageDefinitionField field, MessageValue value);
    }
}
