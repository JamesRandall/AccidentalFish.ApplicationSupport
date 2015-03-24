using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ApplicationResourceSettingNameProvider : IApplicationResourceSettingNameProvider
    {
        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.sql-connection-string", componentIdentity.FullyQualifiedName);
        }

        public string SqlContextType(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.db-context-type", componentIdentity.FullyQualifiedName);
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.storage-account-connection-string", componentIdentity.FullyQualifiedName);
        }

        public string ServiceBusConnectionString(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.service-bus-connection-string", componentIdentity.FullyQualifiedName);
        }

        public string DefaultTableName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-table-name", componentIdentity.FullyQualifiedName);
        }

        public string DefaultQueueName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-queue-name", componentIdentity.FullyQualifiedName);
        }

        public string DefaultBlobContainerName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-blob-container-name", componentIdentity.FullyQualifiedName);
        }

        public string DefaultTopicName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-topic-name", componentIdentity.FullyQualifiedName);
        }

        public string DefaultSubscriptionName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-subscription-name", componentIdentity.FullyQualifiedName);
        }

        public string SettingName(IComponentIdentity componentIdentity, string setting)
        {
            return String.Format("{0}.setting.{1}", componentIdentity.FullyQualifiedName, setting);
        }

        public string DefaultLeaseBlockName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-lease-block-name", componentIdentity.FullyQualifiedName);
        }

        public string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity)
        {
            return String.Format("{0}.default-brokered-message-queue-name", componentIdentity.FullyQualifiedName);
        }
    }
}
