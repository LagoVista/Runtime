// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: a438a5b587df441d2fb1644b9433c030dcc17094dbacf555ae01ad61d545c368
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging;
using LagoVista.IoT.Logging.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LagoVista.IoT.Runtime.Core
{
    public class StartUp
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            ErrorCodes.Register(typeof(Resources.ErrorCodes.Common));
            ErrorCodes.Register(typeof(Resources.ErrorCodes.PipelineEnqueing));
            ErrorCodes.Register(typeof(Resources.ErrorCodes.Verifiers));
            ErrorCodes.Register(typeof(Resources.ErrorCodes.Messaging));
        }
    }
}

namespace LagoVista.DependencyInjection
{
    public static class RuntimeModule
    {
        public static void AddRuntimeCoreModule(this IServiceCollection services, IConfigurationRoot configRoot, IAdminLogger logger)
        {
            LagoVista.IoT.Runtime.Core.StartUp.ConfigureServices(services);
        }
    }
}
