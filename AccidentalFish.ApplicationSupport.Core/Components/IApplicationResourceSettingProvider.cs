namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Given a component identity this returns the app.config, web.config, *.cscfg setting values
    /// </summary>
    public interface IApplicationResourceSettingProvider
    {
        /// <summary>
        /// Given a component name this returns the sql connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        string SqlConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the sql database context type for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The context type</returns>
        string SqlContextType(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the storage account connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        string StorageAccountConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default table name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The table name</returns>
        string DefaultTableName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default queue name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The queue name</returns>
        string DefaultQueueName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default blob container name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The blob container name</returns>
        string DefaultBlobContainerName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default lease block name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The lease block name</returns>
        string DefaultLeaseBlockName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default topic name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The topic name</returns>
        string DefaultTopicName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the service bus connection string for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The connection string</returns>
        string ServiceBusConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default topic subscription for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The subscription name</returns>
        string DefaultSubscriptionName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Given a component name this returns the default brokered message queue name for it
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>The queue name</returns>
        string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity);
    }
}
