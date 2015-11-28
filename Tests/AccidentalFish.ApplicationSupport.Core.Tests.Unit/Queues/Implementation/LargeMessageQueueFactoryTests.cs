using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Queues.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Queues.Implementation
{
    [TestClass]
    public class LargeMessageQueueFactoryTests
    {
        [TestMethod]
        public void FactoryConstructs()
        {
            // Arrange
            var applicationResourceSettingProviderMock = new Mock<IApplicationResourceSettingProvider>();
            var loggerMock = new Mock<ICoreAssemblyLogger>();
            var queueSerializerMock = new Mock<IQueueSerializer>();
            var queueFactoryMock = new Mock<IQueueFactory>();
            var blobRepsoitoryFactoryMock = new Mock<IBlobRepositoryFactory>();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            new LargeMessageQueueFactory(applicationResourceSettingProviderMock.Object, queueFactoryMock.Object, blobRepsoitoryFactoryMock.Object, queueSerializerMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public void CreateReturnsQueue()
        {
            // Arrange
            var applicationResourceSettingProviderMock = new Mock<IApplicationResourceSettingProvider>();
            var loggerMock = new Mock<ICoreAssemblyLogger>();
            var queueSerializerMock = new Mock<IQueueSerializer>();
            var blobRepositoryMock = new Mock<IAsynchronousBlockBlobRepository>();
            var underlyingQueueMock = new Mock<IAsynchronousQueue<LargeMessageReference>>();
            var queueFactoryMock = new Mock<IQueueFactory>();
            var blobRepsoitoryFactoryMock = new Mock<IBlobRepositoryFactory>();
            var factory = new LargeMessageQueueFactory(applicationResourceSettingProviderMock.Object, queueFactoryMock.Object, blobRepsoitoryFactoryMock.Object, queueSerializerMock.Object, loggerMock.Object);

            // Act
            var queue = factory.Create<TestQueueItem>(underlyingQueueMock.Object, blobRepositoryMock.Object);

            // Assert
            Assert.IsInstanceOfType(queue, typeof(LargeMessageQueue<TestQueueItem>));
            Assert.AreEqual(underlyingQueueMock.Object, queue.ReferenceQueue);
            Assert.AreEqual(blobRepositoryMock.Object, queue.BlobRepository);
        }
    }
}
