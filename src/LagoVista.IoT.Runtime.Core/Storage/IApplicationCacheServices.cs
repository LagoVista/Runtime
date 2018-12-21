using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Pipeline.Models;
using LagoVista.IoT.Runtime.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IApplicationCacheServices
    {
        Task<ICache> GetApplicationCacheConnectorAsync(ApplicationCache cache);
    }
}
