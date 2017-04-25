using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IRepository
    {
        Task<T> RequestAsync<T>(Uri uri);
    }
}
