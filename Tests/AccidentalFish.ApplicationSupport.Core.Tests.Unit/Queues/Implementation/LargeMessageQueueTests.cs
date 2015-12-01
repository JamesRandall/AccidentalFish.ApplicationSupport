using System;
using System.IO;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Queues.Implementation;
using AccidentalFish.ApplicationSupport.Core.Queues.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Queues.Implementation
{
    [TestClass]
    public class LargeMessageQueueTests
    {
        private Mock<ICoreAssemblyLogger> _loggerMock;
        private Mock<IQueueSerializer> _queueSerializerMock;
        private Mock<IAsynchronousQueue<LargeMessageReference>> _referenceQueueMock;
        private Mock<IAsynchronousBlockBlobRepository> _blobRepository;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ICoreAssemblyLogger>();
            _queueSerializerMock = new Mock<IQueueSerializer>();
            _referenceQueueMock = new Mock<IAsynchronousQueue<LargeMessageReference>>();
            _blobRepository = new Mock<IAsynchronousBlockBlobRepository>();

            _queueSerializerMock.Setup(x => x.Serialize(It.IsAny<TestQueueItem>())).Returns("{ 'Name': 'Zaphod' }");
        }

        [TestMethod]
        public void Constructs()
        {
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);

            // Assert
            Assert.AreEqual(_referenceQueueMock.Object, queue.ReferenceQueue);
            Assert.AreEqual(_blobRepository.Object, queue.BlobRepository);
        }

        [TestMethod]
        public async Task EnqueuePostsQueueAndBlob()
        {
            // Arrange
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);
            var item = new TestQueueItem {Name = "Zaphod"};
            var mockBlob = new Mock<IBlob>();
            string blobName = null;
            _blobRepository.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .Returns(Task.FromResult(mockBlob.Object))
                .Callback((string n, Stream s) =>
                {
                    // capture the blob name to ensure the queue item gets posted with the correct blob reference
                    blobName = n;
                });

            // Act
            await queue.EnqueueAsync(item);

            // Assert
            _referenceQueueMock.Verify(x => x.EnqueueAsync(It.Is<LargeMessageReference>(v=> v.BlobReference == blobName), null));
        }

        [TestMethod]
        public async Task EnqueuePostsQueueAndBlobWithVisibilityDelay()
        {
            // Arrange
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);
            var item = new TestQueueItem { Name = "Zaphod" };
            var mockBlob = new Mock<IBlob>();
            string blobName = null;
            _blobRepository.Setup(x => x.UploadAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .Returns(Task.FromResult(mockBlob.Object))
                .Callback((string n, Stream s) =>
                {
                    // capture the blob name to ensure the queue item gets posted with the correct blob reference
                    blobName = n;
                });
            var timespan = TimeSpan.FromSeconds(30);

            // Act
            await queue.EnqueueAsync(item, timespan);

            // Assert
            _referenceQueueMock.Verify(x => x.EnqueueAsync(It.Is<LargeMessageReference>(v => v.BlobReference == blobName), timespan, null));
        }

        [TestMethod]
        public async Task ExtendLeaseWithDelayCallsReferenceQueue()
        {
            // Arrange
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);
            var item = new TestQueueItem { Name = "Zaphod" };
            var timespan = TimeSpan.FromSeconds(30);
            var underlyingQueueItem = new Mock<IQueueItem<LargeMessageReference>>();
            var queueItem = new LargeMessageQueueItem<TestQueueItem>(item, 1, underlyingQueueItem.Object);
            
            // Act
            await queue.ExtendLeaseAsync(queueItem, timespan);

            // Assert
            _referenceQueueMock.Verify(x => x.ExtendLeaseAsync(underlyingQueueItem.Object, timespan));
        }

        [TestMethod]
        public async Task ExtendLeaseCallsReferenceQueue()
        {
            // Arrange
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);
            var item = new TestQueueItem { Name = "Zaphod" };
            var underlyingQueueItem = new Mock<IQueueItem<LargeMessageReference>>();
            var queueItem = new LargeMessageQueueItem<TestQueueItem>(item, 1, underlyingQueueItem.Object);

            // Act
            await queue.ExtendLeaseAsync(queueItem);

            // Assert
            _referenceQueueMock.Verify(x => x.ExtendLeaseAsync(underlyingQueueItem.Object));
        }

        [TestMethod]
        public async Task DequeueCallsProcessorWithLargeMessage()
        {
            // Arrange
            var didDeque = false;
            var blob = new Mock<IBlob>();
            _blobRepository.Setup(x => x.Get("hello")).Returns(blob.Object);
            var queue = new LargeMessageQueue<TestQueueItem>(_queueSerializerMock.Object, _referenceQueueMock.Object, _blobRepository.Object, _loggerMock.Object);
            var item = new TestQueueItem { Name = "Zaphod" };
            var reference = new LargeMessageReference {BlobReference = "hello"};
            //var referenceQueueItem = new Mock<IQueueItem>();
            var referenceItem = new Mock<IQueueItem<LargeMessageReference>>();
            referenceItem.SetupGet(x => x.Item).Returns(reference);
            _referenceQueueMock.Setup(
                x => x.DequeueAsync(It.IsAny<Func<IQueueItem<LargeMessageReference>, Task<bool>>>()))
                .Returns(Task.FromResult(0))
                .Callback((Func<IQueueItem<LargeMessageReference>,Task<bool>> func) =>
                {
                    func(referenceItem.Object);
                });

            // Act
            await queue.DequeueAsync(tqi =>
            {
                didDeque = true;
                return Task.FromResult(true);
            });

            // Assert
            Assert.IsTrue(didDeque);
        }
    }
}
