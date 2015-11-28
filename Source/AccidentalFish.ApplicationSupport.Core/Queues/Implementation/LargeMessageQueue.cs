using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Queues.Implementation
{
    internal class LargeMessageQueue<T> : ILargeMessageQueue<T> where T : class
    {
        private readonly IQueueSerializer _serializer;
        private readonly IAsynchronousQueue<LargeMessageReference> _referenceQueue;
        private readonly IAsynchronousBlockBlobRepository _blobRepository;
        private readonly ICoreAssemblyLogger _logger;

        public LargeMessageQueue(
            IQueueSerializer serializer,
            IAsynchronousQueue<LargeMessageReference> referenceQueue,
            IAsynchronousBlockBlobRepository blobRepository,
            ICoreAssemblyLogger logger)
        {
            _serializer = serializer;
            _referenceQueue = referenceQueue;
            _blobRepository = blobRepository;
            _logger = logger;

            _logger.Verbose("LargeMessageQueue<T>: constructing");
        }

        public async Task EnqueueAsync(T item)
        {
            await DoEnqueue(item, null);
        }

        public async Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay)
        {
            await DoEnqueue(item, initialVisibilityDelay);
        }

        public async Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor)
        {
            _logger.Verbose("LargeMessageQueue<T>: DequeueAsync - attempting dequeue");
            await _referenceQueue.DequeueAsync(async message =>
            {
                if (message != null)
                {
                    _logger.Verbose("LargeMessageQueue<T>: DequeueAsync - dequeued large message with blob reference {0}", message.Item.BlobReference);
                    IBlob blob;
                    T item;
                    try
                    {
                        blob = _blobRepository.Get(message.Item.BlobReference);
                        var serializedObjectText = await blob.DownloadStringAsync(Encoding.UTF8);
                        item = _serializer.Deserialize<T>(serializedObjectText);
                    }
                    catch (Exception ex)
                    {
                        _logger.Verbose("LargeMessageQueue<T>: DequeueAsync - unable to download blob {0}", message.Item.BlobReference);
                        throw new LargeMessageQueueException("Unable to download blob", ex, message.Item.BlobReference);
                    }
                    
                    bool result = await processor(new LargeMessageQueueItem<T>(item, message.DequeueCount, message));

                    if (result)
                    {
                        try
                        {
                            _logger.Verbose("LargeMessageQueue<T>: DequeueAsync - deleting blob reference {0}", message.Item.BlobReference);
                            await blob.DeleteAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.Verbose("LargeMessageQueue<T>: DequeueAsync - unable to delete blob reference {0}", message.Item.BlobReference);
                            throw new LargeMessageQueueException("Unable to delete blob, abandoning queue item", ex, message.Item.BlobReference);
                        }
                    }

                    return result;
                }
                return await processor(null);
            });
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout)
        {
            LargeMessageQueueItem<T> castItem = queueItem as LargeMessageQueueItem<T>;
            if (castItem == null)
            {
                throw new InvalidOperationException("Unsupported queue item type.");
            }

            await _referenceQueue.ExtendLeaseAsync(castItem.ActualQueueItem, visibilityTimeout);
            _logger?.Verbose("LargeMessageQueue<T>: ExtendLeaseAsync - extending by {0}ms", visibilityTimeout.TotalMilliseconds);
        }

        public async Task ExtendLeaseAsync(IQueueItem<T> queueItem)
        {
            LargeMessageQueueItem<T> castItem = queueItem as LargeMessageQueueItem<T>;
            if (castItem == null)
            {
                throw new InvalidOperationException("Unsupported queue item type.");
            }

            await _referenceQueue.ExtendLeaseAsync(castItem.ActualQueueItem);
            _logger?.Verbose("LargeMessageQueue<T>: ExtendLeaseAsync - extended by default time");
        }

        private async Task DoEnqueue(T item, TimeSpan? initialVisibilityDelay)
        {
            string serializedItem = _serializer.Serialize(item);
            string blobname = $"{Guid.NewGuid()}.que";
            
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedItem)))
                {
                    await _blobRepository.UploadAsync(blobname, stream);
                }
            }
            catch (Exception ex)
            {
                throw new LargeMessageQueueException("Unable to upload blob to blob store. No item queued.", ex, blobname);
            }

            try
            {
                if (initialVisibilityDelay.HasValue)
                {
                    await _referenceQueue.EnqueueAsync(new LargeMessageReference
                    {
                        BlobReference = blobname
                    }, initialVisibilityDelay.Value);
                }
                else
                {
                    await _referenceQueue.EnqueueAsync(new LargeMessageReference
                    {
                        BlobReference = blobname
                    });
                }
            }
            catch (Exception ex)
            {
                throw new LargeMessageQueueException("Error enqueuing item, blob may be orphaned", ex, blobname);
            }
        }

        public IAsynchronousQueue<LargeMessageReference> ReferenceQueue => _referenceQueue;
        public IAsynchronousBlockBlobRepository BlobRepository => _blobRepository;
    }
}
