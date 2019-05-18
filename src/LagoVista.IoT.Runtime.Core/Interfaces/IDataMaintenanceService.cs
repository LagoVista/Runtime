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
