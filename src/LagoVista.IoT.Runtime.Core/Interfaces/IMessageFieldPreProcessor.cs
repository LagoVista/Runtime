// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b6ac412a1d7ef7f8440880686fe07fe2d7458e80d3a33a7e7d922085774738b1
// IndexVersion: 2
// --- END CODE INDEX META ---
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
