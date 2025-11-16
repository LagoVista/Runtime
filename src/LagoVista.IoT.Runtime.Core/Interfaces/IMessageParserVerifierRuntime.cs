// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: cf25e4379d6f9d0625ce5e7e3fe73c71d490c3963e7ba0c47638fd02b1cb55f4
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IMessageParserVerifierRuntime
    {
        Task<VerificationResults> VerifyAsync(VerificationRequest<DeviceMessageDefinition> request, EntityHeader org, EntityHeader user);
    }
}
