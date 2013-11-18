namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IApplicationResourceSettingNameProvider
    {
        string SqlConnectionString(IComponentIdentity componentIdentity);

        string SqlContextType(IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);

        string DefaultTableName(IComponentIdentity componentIdentity);

        string DefaultQueueName(IComponentIdentity componentIdentity);

        string DefaultBlobContainerName(IComponentIdentity componentIdentity);

        string SettingName(IComponentIdentity componentIdentity, string setting);
    }
}
