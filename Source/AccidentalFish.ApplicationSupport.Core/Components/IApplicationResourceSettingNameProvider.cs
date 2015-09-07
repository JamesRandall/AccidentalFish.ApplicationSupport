namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Given a component identity this returns the app.config, web.config, *.cscfg setting names that contain component settings
    /// </summary>
    public interface IApplicationResourceSettingNameProvider
    {
        /// <summary>
        /// Return the setting name for the SQL connection string
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string SqlConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the SQL database context type
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string SqlContextType(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the storage account connection string
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string StorageAccountConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the service bus connection string
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string ServiceBusConnectionString(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the default table name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultTableName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the default queue name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultQueueName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the default blob container name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultBlobContainerName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the default topic name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultTopicName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the default subscription name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultSubscriptionName(IComponentIdentity componentIdentity);

        /// <summary>
        /// Return the setting name for the a component setting
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <param name="setting">The name of the setting</param>
        /// <returns>Setting name</returns>
        string SettingName(IComponentIdentity componentIdentity, string setting);
        /// <summary>
        /// Return the setting name for the default lease block name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultLeaseBlockName(IComponentIdentity componentIdentity);
        /// <summary>
        /// Return the setting name for the default brokered message queue name
        /// </summary>
        /// <param name="componentIdentity">The component to obtain the setting name for</param>
        /// <returns>Setting name</returns>
        string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity);
    }
}
