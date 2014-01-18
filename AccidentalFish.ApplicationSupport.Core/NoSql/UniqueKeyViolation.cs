using System;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public class UniqueKeyViolation : Exception
    {
        public UniqueKeyViolation(string partitionKey, string rowKey, Exception innerException) : base("Unique key violation", innerException)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }
    }
}
