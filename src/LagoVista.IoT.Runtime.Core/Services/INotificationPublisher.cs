using LagoVista.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Models.Messaging;
using System;
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
        DeviceRepository,
        DeviceGroup,
        Device,
        DeviceLog,
        PipelineModule,
        Route,
        DeviceConfiguration,
        MessageType,

        Dependency,

        Simulator,    
    
        Jobs,
        
        DFU,

        Customer,
        ToDo,
        WorkTask,
        Inbox,

        Entity,
    }

    public enum NotificationVerbosity
    {
        Diagnostics,
        Normal,
        Errors,
    }

    public enum Targets
    {
        Sms,
        WebSocket,
        Push,
        WebSocketAndPush
    }

    public interface INotificationPublisher
    {
        Task PublishAsync(Targets target, Notification notification, NotificationVerbosity verbosity = NotificationVerbosity.Normal);
        Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal);
        Task PublishAsync<TPayload>(Targets target, Channels channel, string channelId, String text, TPayload message, NotificationVerbosity verbosity = NotificationVerbosity.Normal);
        Task PublishAsync<TEntity>(Targets target, TEntity entity, NotificationVerbosity verbosity = NotificationVerbosity.Normal) where TEntity : IIDEntity;
        Task PublishTextAsync(Targets target, Channels channel, string channelId, String text, NotificationVerbosity verbosity = NotificationVerbosity.Normal);
    }
}
