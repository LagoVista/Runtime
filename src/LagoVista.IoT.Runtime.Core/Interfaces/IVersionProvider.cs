// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 39e50ecb600520e90ec5ed06c74ff22f63cf795454d9c738fd50f4519baa6fab
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Runtime.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IVersionProvider
    {
        Task<HostVersion> LoadHostVersionAsync(string fileName = "version.json");

        void WriteVersionInfoToConsole(HostVersion version, string hostId);
    }
}
