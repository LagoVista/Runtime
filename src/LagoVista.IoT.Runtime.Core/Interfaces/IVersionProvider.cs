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
