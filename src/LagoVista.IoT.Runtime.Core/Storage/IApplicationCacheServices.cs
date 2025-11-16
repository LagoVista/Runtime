// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: a410f1a904d6215f272c91039604791525c009b699f2a7b3e69c3985c9b17807
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        Task<ICache> GetApplicationCacheConnectorAsync(string cacheKey);
    }
}
