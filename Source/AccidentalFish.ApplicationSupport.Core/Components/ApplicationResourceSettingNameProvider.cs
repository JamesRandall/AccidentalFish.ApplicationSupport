using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Given a component name this will return a app.config / web.config / *.cscfg setting name for a given type
    /// of resource
    /// </summary>
    public class ApplicationResourceSettingNameProvider : IApplicationResourceSettingNameProvider
    {
        /// <summary>
        /// SQL connection string setting.
        /// Format: {componentIdentity}.sql-connection-string
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.sql-connection-string";
        }

        /// <summary>
        /// Database context setting.
        /// Format: {componentIdentity}.db-context-type
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string SqlContextType(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.db-context-type";
        }

        /// <summary>
        /// Storage account connection string setting.
        /// Format: {componentIdentity}.storage-account-connection-string
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.storage-account-connection-string";
        }

        /// <summary>
        /// Service bus connection string setting.
        /// Format: {componentIdentity}.service-bus-connection-string
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string ServiceBusConnectionString(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.service-bus-connection-string";
        }

        /// <summary>
        /// Default table name setting.
        /// Format: {componentIdentity}.default-table-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultTableName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-table-name";
        }

        /// <summary>
        /// Default queue name setting.
        /// Format: {componentIdentity}.default-queue-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultQueueName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-queue-name";
        }

        /// <summary>
        /// Default blob container name setting.
        /// Format: {componentIdentity}.default-blob-container-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultBlobContainerName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-blob-container-name";
        }

        /// <summary>
        /// Default topic name setting.
        /// Format: {componentIdentity}.default-topic-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultTopicName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-topic-name";
        }

        /// <summary>
        /// Default subscription name setting.
        /// Format: {componentIdentity}.default-subscription-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultSubscriptionName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-subscription-name";
        }

        /// <summary>
        /// Application setting name .
        /// Format: {componentIdentity}.setting.{setting}
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <param name="setting">Name of the setting</param>
        /// <returns>Setting name</returns>
        public string SettingName(IComponentIdentity componentIdentity, string setting)
        {
            return $"{componentIdentity.FullyQualifiedName}.setting.{setting}";
        }

        /// <summary>
        /// Default lease block name.
        /// Format: {componentIdentity}.default-lease-block-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultLeaseBlockName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-lease-block-name";
        }

        /// <summary>
        /// Default brokered message queue name setting.
        /// Format: {componentIdentity}.default-brokered-message-queue-name
        /// </summary>
        /// <param name="componentIdentity">The component identity</param>
        /// <returns>Setting name</returns>
        public string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity)
        {
            return $"{componentIdentity.FullyQualifiedName}.default-brokered-message-queue-name";
        }
    }
}
