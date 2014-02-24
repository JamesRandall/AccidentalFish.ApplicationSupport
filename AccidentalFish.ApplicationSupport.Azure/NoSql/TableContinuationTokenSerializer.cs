using System.IO;
using System.Xml;
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
