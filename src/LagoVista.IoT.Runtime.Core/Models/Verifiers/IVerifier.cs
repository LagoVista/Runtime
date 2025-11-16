// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 61f0ff3915084beac540485e3a4720f616fe2e965d5deed0e1972c7fc7c09a5b
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Validation;
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Runtime.Core.Models.Verifiers
{
    public interface IVerifier : IOwnedEntity, IKeyedEntity, INoSQLEntity, IValidateable, IIDEntity
    {
    }
}
