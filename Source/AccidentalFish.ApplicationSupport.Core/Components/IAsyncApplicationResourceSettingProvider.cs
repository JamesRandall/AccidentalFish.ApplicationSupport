using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Given a component identity this returns the app.config, web.config, *.cscfg setting values
    /// </summary>
    public interface IAsyncApplicationResourceSettingProvider
    {
        /// <summary>
        /// Given a component name this returns the sql connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        Task<string> SqlConnectionStringAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the sql database context type for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The context type</returns>
        Task<string> SqlContextTypeAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the storage account connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        Task<string> StorageAccountConnectionStringAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default table name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The table name</returns>
        Task<string> DefaultTableNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default queue name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The queue name</returns>
        Task<string> DefaultQueueNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default blob container name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The blob container name</returns>
        Task<string> DefaultBlobContainerNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default lease block name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The lease block name</returns>
        Task<string> DefaultLeaseBlockNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default topic name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The topic name</returns>
        Task<string> DefaultTopicNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the service bus connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        Task<string> ServiceBusConnectionStringAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default topic subscription for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The subscription name</returns>
        Task<string> DefaultSubscriptionNameAsync(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default brokered message queue name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The queue name</returns>
        Task<string> DefaultBrokeredMessageQueueNameAsync(IComponentIdentity componentIdentity);
    }
}
