using System;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Core.Components.Implementation
{
    internal class ApplicationResourceSettingProvider : IApplicationResourceSettingProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationResourceSettingNameProvider _nameProvider;

        public ApplicationResourceSettingProvider(IConfiguration configuration,
            IApplicationResourceSettingNameProvider nameProvider)
        {
            _configuration = configuration;
            _nameProvider = nameProvider;
        }

        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.SqlConnectionString(componentIdentity)];
        }

        public string SqlContextType(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.SqlContextType(componentIdentity)];
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.StorageAccountConnectionString(componentIdentity)];
        }

        public string DefaultTableName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultTableName(componentIdentity)];
        }

        public string DefaultQueueName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultQueueName(componentIdentity)];
        }

        public string DefaultBlobContainerName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultBlobContainerName(componentIdentity)];
        }

        public string DefaultLeaseBlockName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultLeaseBlockName(componentIdentity)];
        }

        public string DefaultTopicName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultTopicName(componentIdentity)];
        }

        public string ServiceBusConnectionString(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.ServiceBusConnectionString(componentIdentity)];
        }

        public string DefaultSubscriptionName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultSubscriptionName(componentIdentity)];
        }

        public string DefaultBrokeredMessageQueueName(IComponentIdentity componentIdentity)
        {
            return _configuration[_nameProvider.DefaultBrokeredMessageQueueName(componentIdentity)];
        }
    }
}
