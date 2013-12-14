using System;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Processes.Logging;
using AccidentalFish.ApplicationSupport.Processes.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Processes.Tests.Unit.Logging
{
    [TestClass]
    public class LogQueueProcessorTests
    {
        private Mock<IApplicationResourceFactory> _applicationResourceFactory;
        private Mock<IAsynchronousBackoffPolicy> _asynchronousBackoffPolicy;
        private Mock<IMapperFactory> _mapperFactory;

        [TestInitialize]
        public void Setup()
        {
            _applicationResourceFactory = new Mock<IApplicationResourceFactory>();
            _asynchronousBackoffPolicy = new Mock<IAsynchronousBackoffPolicy>();
            _mapperFactory = new Mock<IMapperFactory>();


        }

        [TestMethod]
        public void ConstructorSucceeds()
        {
            // Act
            new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object);
        }

        [TestMethod]
        public void StartInitiaitesBackoffPolicy()
        {
            // Arrange
            CancellationTokenSource source = new CancellationTokenSource();
            LogQueueProcessor processor = new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object);

            // Act
            processor.Start(source.Token);

            // Assert
            _asynchronousBackoffPolicy.Verify(x => x.Execute(It.IsAny<Action<Action<bool>>>(), source.Token));
        }

        public void NullQueueItemReturnsFalseToBackoff()
        {
            // Arrange
            _asynchronousBackoffPolicy.Setup(
                x => x.Execute(It.IsAny<Action<Action<bool>>>(), It.IsAny<CancellationToken>()))
                .Callback<Action<Action<bool>>>((action) =>
                {
                    action()
                });
            CancellationTokenSource source = new CancellationTokenSource();
            LogQueueProcessor processor = new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object);

            // Act
            processor.Start(source.Token);

            // Assert
            _asynchronousBackoffPolicy.Verify(x => x.Execute(It.IsAny<Action<Action<bool>>>(), source.Token));
        }
    }
}
