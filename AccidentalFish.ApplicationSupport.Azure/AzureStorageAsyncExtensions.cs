using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure
{
    public static class AzureStorageAsyncExtensions
    {
        #region Queue

        public static Task AddMessageAsync(this CloudQueue queue, CloudQueueMessage message)
        {
            return Task.Factory.FromAsync((m, cb, obj) =>
                                   queue.BeginAddMessage
                                       (m, null, null,
                                        null, null, cb,
                                        obj), queue.EndAddMessage,
                                   message, null);
        }

        public static Task<CloudQueueMessage> GetMessageAsync(this CloudQueue queue)
        {
            return Task.Factory.FromAsync<CloudQueueMessage>(queue.BeginGetMessage, queue.EndGetMessage, null);
        }

        public static Task DeleteMessageAsync(this CloudQueue queue, CloudQueueMessage message)
        {
            return Task.Factory.FromAsync((m, cb, obj) => queue.BeginDeleteMessage(message, null, null, cb, obj), queue.EndDeleteMessage, message, null);
        }

        #endregion

        #region Blob

        public static Task UploadFromStreamAsync(this CloudBlockBlob blob, Stream stream)
        {
            return Task.Factory.FromAsync(blob.BeginUploadFromStream, blob.EndUploadFromStream, stream, null);
        }

        public static Task DownloadToStreamAsync(this CloudBlockBlob blob, Stream stream)
        {
            return Task.Factory.FromAsync(blob.BeginDownloadToStream, blob.EndDownloadRangeToStream, stream, null);
        }

        public static Task<bool> ExistsAsync(this CloudBlockBlob blob)
        {
            return Task.Factory.FromAsync<bool>(blob.BeginExists, blob.EndExists, null);
        }

        #endregion

        #region Table

        public static Task<TableResult> ExecuteAsync(this CloudTable table, TableOperation operation)
        {
            return Task.Factory.FromAsync(table.BeginExecute, (Func<IAsyncResult, TableResult>) table.EndExecute, operation, null);
        }

        public static Task<IList<TableResult>> ExecuteBatchAsync(this CloudTable table, TableBatchOperation operation)
        {
            return Task.Factory.FromAsync(table.BeginExecuteBatch, (Func<IAsyncResult, IList<TableResult>>)table.EndExecuteBatch, operation, null);
        }

        #endregion
    }
}
