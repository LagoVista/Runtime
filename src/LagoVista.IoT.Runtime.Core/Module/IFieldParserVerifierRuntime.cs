using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Module
{
    public interface IFieldParserVerifierRuntime
    {
        Task<VerificationResult> VerifyAsync(VerificationRequest<DeviceMessageDefinitionField> request);
    }
}
