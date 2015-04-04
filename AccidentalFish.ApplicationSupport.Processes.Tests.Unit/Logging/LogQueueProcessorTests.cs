using System;
using System.Threading;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Core.Alerts;
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
        private Mock<IAzureApplicationResourceFactory> _applicationResourceFactory;
        private Mock<IAsynchronousBackoffPolicy> _asynchronousBackoffPolicy;
        private Mock<IMapperFactory> _mapperFactory;
        private Mock<IAlertSender> _alertSender;

        [TestInitialize]
        public void Setup()
        {
            _applicationResourceFactory = new Mock<IAzureApplicationResourceFactory>();
            _asynchronousBackoffPolicy = new Mock<IAsynchronousBackoffPolicy>();
            _alertSender = new Mock<IAlertSender>();
            _mapperFactory = new Mock<IMapperFactory>();
        }

        [TestMethod]
        public void ConstructorSucceeds()
        {
            // Act
            new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object, _alertSender.Object);
        }

        [TestMethod]
        public void StartInitiaitesBackoffPolicy()
        {
            // Arrange
            CancellationTokenSource source = new CancellationTokenSource();
            LogQueueProcessor processor = new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object, _alertSender.Object);

            // Act
            processor.Start(source.Token);

            // Assert
            _asynchronousBackoffPolicy.Verify(x => x.Execute(It.IsAny<Action<Action<bool>>>(), source.Token));
        }

        public void NullQueueItemReturnsFalseToBackoff()
        {
            // Arrange
            /*_asynchronousBackoffPolicy.Setup(
                x => x.Execute(It.IsAny<Action<Action<bool>>>(), It.IsAny<CancellationToken>()))
                .Callback<Action<Action<bool>>>((action) =>
                {
                    action()
                });
            CancellationTokenSource source = new CancellationTokenSource();
            LogQueueProcessor processor = new LogQueueProcessor(_applicationResourceFactory.Object, _asynchronousBackoffPolicy.Object, _mapperFactory.Object, _alertSender.Object);

            // Act
            processor.Start(source.Token);

            // Assert
            _asynchronousBackoffPolicy.Verify(x => x.Execute(It.IsAny<Action<Action<bool>>>(), source.Token));
             * */
        }
    }
}
