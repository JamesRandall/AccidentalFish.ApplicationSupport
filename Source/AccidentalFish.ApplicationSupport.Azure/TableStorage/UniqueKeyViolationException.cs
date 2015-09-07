using System;
using System.Runtime.Serialization;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    [Serializable]
    public class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException(string partitionKey, string rowKey, Exception innerException) : base("Unique key violation", innerException)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; }
        public string RowKey { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PartitionKey", PartitionKey);
            info.AddValue("RowKey", RowKey);
        }
    }
}
