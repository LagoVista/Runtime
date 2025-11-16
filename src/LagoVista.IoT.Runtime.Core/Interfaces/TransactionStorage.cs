// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 17a457abb0040e3fb46deebb70661551003e86db6a16a641993acd99829b9fb7
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface ITransactionStorage
    {
        void AddSetting(ConnectionSettings settings);
        Task<double> AddTransactionAsync(DeviceTransaction transaction);
    }
}
