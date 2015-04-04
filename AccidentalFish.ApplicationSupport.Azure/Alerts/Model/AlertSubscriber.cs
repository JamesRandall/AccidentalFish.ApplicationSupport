using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Alerts.Model
{
    public class AlertSubscriber : TableEntity
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
