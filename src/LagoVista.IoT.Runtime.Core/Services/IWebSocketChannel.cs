using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Services
{
    public interface IWebSocketChannel
    {
        Task SendToChannelAsync<T>(T message, String channelName, string resourceId);
    }
}
