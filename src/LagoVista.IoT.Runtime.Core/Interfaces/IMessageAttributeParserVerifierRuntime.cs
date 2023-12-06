using LagoVista.Core.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IMessageAttributeParserVerifierRuntime
    {
        Task<VerificationResults> VerifyAsync(VerificationRequest<MessageAttributeParser> request, EntityHeader org, EntityHeader user);
    }
}
