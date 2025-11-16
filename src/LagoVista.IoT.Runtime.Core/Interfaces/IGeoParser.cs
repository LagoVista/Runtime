// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 143f65e57dbf9c594d6a51064beb7859e6eaf1c5e88f619c6a00da6e58c7c6e8
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Runtime.Core
{
    /// <summary>
    /// To be used to take lat/lon obects and attempt to turn them into a string that is a lat and a lon
    /// </summary>
    public interface IGeoParser
    {
        InvokeResult<String> Parse(object lat, object lon, object altitude); 
    }
}
