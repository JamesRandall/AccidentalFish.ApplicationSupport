using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Logging.Implementation
{
    // No longer mockable. Will replace with integration test.
    /*
    [TestClass]
    public class LoggerTests
    {
        private Mock<IAsynchronousQueue<LogQueueItem>> _queue;
        private Mock<IFullyQualifiedName> _source;
        private Mock<IRuntimeEnvironment> _runtimeEnvironment;
        private Mock<IQueueLoggerExtension> _extension;
        private Mock<ICorrelationIdProvider> _correlationIdProvider;

        [TestInitialize]
        public void Setup()
        {
            _queue = new Mock<IAsynchronousQueue<LogQueueItem>>();
            _source = new Mock<IFullyQualifiedName>();
            _runtimeEnvironment = new Mock<IRuntimeEnvironment>();
            _extension = new Mock<IQueueLoggerExtension>();
            _extension.Setup(x => x.BeforeLog(It.IsAny<LogQueueItem>(), It.IsAny<Exception>(), It.IsAny<bool>())).Returns(true);
            _correlationIdProvider = new Mock<ICorrelationIdProvider>();
            _source.SetupGet(x => x.FullyQualifiedName).Returns("a.name");
        }

        [TestMethod]
        public async Task DebugLogsWhenLogLevelAtDebug()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Debug, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.DebugAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task DebugDoesNotLogWhenLogLevelAtInformation()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Information, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.DebugAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task InformationLogsWhenLogLevelAtInformation()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Information, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.InformationAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task InformationDoesNotLogWhenLogLevelAtWarning()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Warning, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.InformationAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task WarningLogsWhenLogLevelAtWarning()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Warning, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.WarningAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task WarningDoesNotLogWhenLogLevelAtError()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Error, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.WarningAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task ErrorLogsWhenLogLevelAtError()
        {
            // Arrange
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Error, _correlationIdProvider.Object);

            // Act
            await queueAsynchronousLogger.ErrorAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task LogQueueItemCreated()
        {
            // Arrange
            LogQueueItem result = null;
            _queue.Setup(x => x.EnqueueAsync(It.IsAny<LogQueueItem>())).Callback<LogQueueItem>(p => result = p);
            QueueAsynchronousLogger queueAsynchronousLogger = new QueueAsynchronousLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Debug, _correlationIdProvider.Object);
            DateTimeOffset baseline = DateTimeOffset.UtcNow;

            // Act
            await queueAsynchronousLogger.LogAsync(LogLevelEnum.Warning, "A message", new Exception("Some message"));

            // Assert
            Assert.AreEqual("A message", result.Message);
            Assert.AreEqual(LogLevelEnum.Warning, result.Level);
            Assert.IsTrue(result.LoggedAt >= baseline && result.LoggedAt <= DateTimeOffset.UtcNow);
            Assert.AreEqual("System.Exception", result.ExceptionName);
            Assert.IsNull(result.InnerExceptionName);
            Assert.AreEqual("a.name", result.Source);
        }
    }*/

}
