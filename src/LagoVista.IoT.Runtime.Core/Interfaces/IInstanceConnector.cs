﻿using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.Deployment.Models;
using LagoVista.IoT.Deployment.Models.Settings;
using LagoVista.IoT.DeviceManagement.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{

    public enum NotificationServerType
    {
        AzureEventHubs,
        RabbitMQ
    }

    public interface IInstanceConnector
    {
        Task<InvokeResult<DeviceDataStorageSettings>> GetDeviceDataStorageSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetDeviceStorageSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetDeviceConnectionEventStorageSettingsAsync();
        Task<InvokeResult<DeploymentInstance>> GetInstanceAsync();
        Task<InvokeResult<string>> GetKeyFroMSecureIdAsync(string keyId);
        Task<InvokeResult<byte[]>> DownloadMLModelAsync(string modelId, string revision = null);
        Task<InvokeResult<LoggingSettings>> GetLoggingSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetNotificationSettingsAsync(NotificationServerType notificationServerType);
        Task<InvokeResult<ConnectionSettings>> GetPEMStorageSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetWatchdogStorageSettingsAsync();
        Task<InvokeResult<ConnectionSettings>> GetEHCheckPointSettingsAsync();
        Task<InvokeResult<RPCSettings>> GetRPCConnectionAsync();
        Task<InvokeResult<UserNotificationInfo>> GetAppUserNotificationInfoAsync(string userId);
        Task SendUserMessageAsync(UserMessage message);
        Task<InvokeResult<string>> GetSolutionVersionAsync();
        Task<InvokeResult> UpdateHostStatusAsync(string hostId, HostStatus status, string version);
        Task<InvokeResult> UpdateInstanceStatusAsync(DeploymentInstanceStates status, bool isDeployed, string version);
        Task<InvokeResult<ConnectionSettings>> GetUsageStorageSettingsAsync();
        Task<InvokeResult<string>> CreateServiceTicketAsync(string ticketTemplateId, string repoId, string deviceId);
        Task<InvokeResult<string>> CreateServiceTicketAsync(CreateServiceTicketRequest ticketRequest);
        Task<InvokeResult> HandleDeviceExceptionAsync(DeviceException exception);
        Task<InvokeResult> ClearDeviceExceptionAsync(DeviceException exception);
    }
}
