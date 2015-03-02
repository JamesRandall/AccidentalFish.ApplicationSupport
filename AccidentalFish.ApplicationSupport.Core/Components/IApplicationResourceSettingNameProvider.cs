namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IApplicationResourceSettingNameProvider
    {
        string SqlConnectionString(IComponentIdentity componentIdentity);

        string SqlContextType(IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);

        string ServiceBusConnectionString(IComponentIdentity componentIdentity);

        string DefaultTableName(IComponentIdentity componentIdentity);

        string DefaultQueueName(IComponentIdentity componentIdentity);

        string DefaultBlobContainerName(IComponentIdentity componentIdentity);

        string DefaultTopicName(IComponentIdentity componentIdentity);

        string DefaultSubscriptionName(IComponentIdentity componentIdentity);

        string SettingName(IComponentIdentity componentIdentity, string setting);

        string DefaultLeaseBlockName(IComponentIdentity componentIdentity);
    }
}
