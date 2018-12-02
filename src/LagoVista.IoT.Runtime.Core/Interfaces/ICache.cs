using LagoVista.Core.Interfaces;
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
        /// <summary>
        /// Can provide multiple end points if necessary
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task InitAsync(params IConnectionSettings[] settings);

        Task<bool> HasItemAsync(CacheItemType itemType, string id, string key);
        Task SetItemAsync<T>(CacheItemType itemType, string id, string key, T item, TimeSpan? expires) where T : class;
        Task<T> GetItemAsync<T>(CacheItemType itemType, string id, string key) where T : class;

        Task RemoveItemAsync(CacheItemType itemType, string id, string key);

        Task SetItemAsync<T>(string key, T item, TimeSpan? expires = null) where T : class;


        Task<bool> HasItemAsync(string key);

        Task<Object> Exec(string cacheCommand);

        Task<T> GetItemAsync<T>(string key) where T : class;

        Task RemoveItemAsync(string key);

        Task SubscribeAsync<T>(SubscriptionType subscriptionType, string key, Action<T> action) where T : class;

        Task SubscribeAsync<T>(SubscriptionType subscriptionType, CacheItemType itemType, string id, string key, Action<T> action) where T : class;
    }
}
