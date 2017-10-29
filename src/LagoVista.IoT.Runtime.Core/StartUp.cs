using LagoVista.Core.Interfaces;
using LagoVista.IoT.Logging;

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
