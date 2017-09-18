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
        InvokeResult<String> Parse(object lat, object lon); 
    }
}
