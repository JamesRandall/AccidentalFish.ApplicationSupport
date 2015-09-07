using System.IO;
using System.Text;
using System.Xml;
using AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class TableContinuationTokenSerializer : ITableContinuationTokenSerializer
    {
        public TableContinuationToken Deserialize(string serializedContinuationToken)
        {
            if (serializedContinuationToken == null) return null;

            TableContinuationToken continuationToken = new TableContinuationToken();
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(serializedContinuationToken)))
            {
                continuationToken.ReadXml(xmlReader);
            }
            return continuationToken;
        }

        public string Serialize(TableContinuationToken continuationToken)
        {
            if (continuationToken == null) return null;
            StringBuilder sb = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(new StringWriter(sb)))
            {
                continuationToken.WriteXml(xmlWriter);
            }
            return sb.ToString();
        }
    }
}
