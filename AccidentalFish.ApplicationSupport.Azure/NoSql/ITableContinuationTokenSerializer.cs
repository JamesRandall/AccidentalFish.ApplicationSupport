using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal interface ITableContinuationTokenSerializer
    {
        TableContinuationToken Deserialize(string serializedContinuationToken);
        string Serialize(TableContinuationToken continuationToken);
    }
}
