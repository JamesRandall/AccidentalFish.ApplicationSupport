using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Core.Components.Implementation
{
    internal class AsyncApplicationResourceSettingProvider : IAsyncApplicationResourceSettingProvider
    {
        private readonly IAsyncConfiguration _configuration;
        private readonly IApplicationResourceSettingNameProvider _nameProvider;

        public AsyncApplicationResourceSettingProvider(IAsyncConfiguration configuration,
            IApplicationResourceSettingNameProvider nameProvider)
        {
            _configuration = configuration;
            _nameProvider = nameProvider;
        }

        public Task<string> SqlConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.SqlConnectionString(componentIdentity));
        }

        public Task<string> SqlContextTypeAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.SqlContextType(componentIdentity));
        }

        public Task<string> StorageAccountConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.StorageAccountConnectionString(componentIdentity));
        }

        public Task<string> DefaultTableNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultTableName(componentIdentity));
        }

        public Task<string> DefaultQueueNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultQueueName(componentIdentity));
        }

        public Task<string> DefaultBlobContainerNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultBlobContainerName(componentIdentity));
        }

        public Task<string> DefaultLeaseBlockNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultLeaseBlockName(componentIdentity));
        }

        public Task<string> DefaultTopicNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultTopicName(componentIdentity));
        }

        public Task<string> ServiceBusConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.ServiceBusConnectionString(componentIdentity));
        }

        public Task<string> DefaultSubscriptionNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultSubscriptionName(componentIdentity));
        }

        public Task<string> DefaultBrokeredMessageQueueNameAsync(IComponentIdentity componentIdentity)
        {
            return _configuration.GetAsync(_nameProvider.DefaultBrokeredMessageQueueName(componentIdentity));
        }
    }
}
