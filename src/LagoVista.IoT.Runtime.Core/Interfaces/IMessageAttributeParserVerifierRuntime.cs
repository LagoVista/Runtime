// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 21592f5e024589e4a15c7c880d0481f918cdca7c35f9a3094dd693a599e5370b
// IndexVersion: 2
// --- END CODE INDEX META ---
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
