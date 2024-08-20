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
