using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public enum CacheItemType
    {
        Host,
        Instance,
        PipelineModule,
        Device
    }

    public enum SubscriptionType
    {
        Changed,
        Removed,
        Expired
    }

    public interface ICache
    {

        Task<bool> HasItemAsync(CacheItemType itemType, string id, string key);
        Task SetItemAsync<T>(CacheItemType itemType, string id, string key, T item, TimeSpan? expires);
        Task<T> GetItemAsync<T>(CacheItemType itemType, string id, string key);

        Task RemoveItemAsync(CacheItemType itemType, string id, string key);

        Task SetItemAsync<T>(string key, TimeSpan? expires = null);


        Task HasItemAsync(string key);

        Task<T> GetItemAsync<T>(string key0);

        Task RemvoeItemAsync(string key);

        Task SubscribeAsync<T>(SubscriptionType subscriptionType, string key, Action<T> action);

        Task SubscribeAsync<T>(CacheItemType itemType, string id, string key, SubscriptionType subscriptionType, Action<T> action);
    }
}
