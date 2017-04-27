using LagoVista.Core.Validation;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public interface IVerifier : IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IIDEntity
    {
    }
}
