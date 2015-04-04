using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal interface ITableContinuationTokenSerializer
    {
        TableContinuationToken Deserialize(string serializedContinuationToken);
        string Serialize(TableContinuationToken continuationToken);
    }
}
