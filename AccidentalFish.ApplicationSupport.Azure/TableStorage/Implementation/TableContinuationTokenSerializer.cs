using System.IO;
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
            using (StringReader stringReader = new StringReader(serializedContinuationToken))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    continuationToken.ReadXml(xmlReader);
                }
            }
            return continuationToken;
        }

        public string Serialize(TableContinuationToken continuationToken)
        {
            if (continuationToken == null) return null;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    continuationToken.WriteXml(xmlWriter);
                }
                return stringWriter.ToString();
            }
        }
    }
}
