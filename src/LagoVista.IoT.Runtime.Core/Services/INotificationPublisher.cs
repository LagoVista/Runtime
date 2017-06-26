using LagoVista.IoT.Runtime.Core.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Services
{
    public enum Channels
    {
        Org,
        Team,
        User,
        Host,
        Instance,
        DeviceGroup,
        Device,
        PipelineModule,
    }

    public enum Targets
    {
        WebSocket,
        Push,
        WebSocketAndPush
    }

    public interface INotificationPublisher
    {
        Task PublishAsync(Targets target, Notification notification);

        Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, TPayload message);

        Task PUblishAsync<TPayload>(Targets target, Channels channel, string channelId, String text, TPayload message);
    }
}
