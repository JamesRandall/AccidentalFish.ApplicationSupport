using System;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    public class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException(string partitionKey, string rowKey, Exception innerException) : base("Unique key violation", innerException)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }
    }
}
