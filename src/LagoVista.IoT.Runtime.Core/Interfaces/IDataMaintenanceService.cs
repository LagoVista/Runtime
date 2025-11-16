// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 6017ecb48cd08c0ea355869570c4f56ad82eaad2fbe1d9114a6246c3d5396644
// IndexVersion: 2
// --- END CODE INDEX META ---
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IDataMaintenance
    {
        void AddMaintainedTable<TTable>(TimeSpan maxAge) where TTable : IDateStampEntity;

        Task MaintainAsync();
    }
}
