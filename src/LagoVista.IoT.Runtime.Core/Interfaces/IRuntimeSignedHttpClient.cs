using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface IRuntimeSignedHttpClient
    {
        Task<InvokeResult<TResult>> GetAsync<TResult>(SignedServiceHttpTarget target, string pathAndQuery);
        Task<InvokeResult<TResult>> PostAsync<TRequest, TResult>(SignedServiceHttpTarget target, string pathAndQuery, TRequest request);
        Task<InvokeResult<TResult>> PutAsync<TRequest, TResult>(SignedServiceHttpTarget target, string pathAndQuery, TRequest request);
        Task<InvokeResult<TResult>> DeleteAsync<TResult>(SignedServiceHttpTarget target, string pathAndQuery);
        Task<InvokeResult<byte[]>> GetBytesAsync(SignedServiceHttpTarget target, string pathAndQuery);
        Task<InvokeResult<string>> GetStringAsync(SignedServiceHttpTarget target, string pathAndQuery);
        Task<InvokeResult<string>> DeleteStringAsync(SignedServiceHttpTarget target, string pathAndQuery);
        Task<InvokeResult<string>> PostJsonStringAsync(SignedServiceHttpTarget target, string pathAndQuery, string json);
        Task<InvokeResult<string>> PutJsonStringAsync(SignedServiceHttpTarget target, string pathAndQuery, string json);
        void SetOrgAndInstance(EntityHeader org, EntityHeader instance);
    }
}
