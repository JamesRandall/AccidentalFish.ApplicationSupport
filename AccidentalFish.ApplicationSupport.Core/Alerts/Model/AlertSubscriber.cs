using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.ApplicationSupport.Core.Alerts.Model
{
    public class AlertSubscriber : NoSqlEntity
    {
        public void SetPartitionAndRowKey()
        {
            PartitionKey = "v1.0.0.0";
            RowKey = Email;
        }

        public string Email { get; set; }

        public string Mobile { get; set; }
    }
}
