using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Storage
{
    public interface IDataStreamServices
    {
        Task<IDataStreamConnector> GetDataStreamConnectorAsync(DataStream dataStream);
    }
}
