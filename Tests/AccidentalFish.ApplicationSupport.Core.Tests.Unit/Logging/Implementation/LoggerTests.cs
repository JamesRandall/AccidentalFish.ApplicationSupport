using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Logging.Implementation
{
    [TestClass]
    public class LoggerTests
    {
        private Mock<IAsynchronousQueue<LogQueueItem>> _queue;
        private Mock<IFullyQualifiedName> _source;
        private Mock<IRuntimeEnvironment> _runtimeEnvironment;
        private Mock<ILoggerExtension> _extension;
        private Mock<ICorrelationIdProvider> _correlationIdProvider;

        [TestInitialize]
        public void Setup()
        {
            _queue = new Mock<IAsynchronousQueue<LogQueueItem>>();
            _source = new Mock<IFullyQualifiedName>();
            _runtimeEnvironment = new Mock<IRuntimeEnvironment>();
            _extension = new Mock<ILoggerExtension>();
            _correlationIdProvider = new Mock<ICorrelationIdProvider>();
            _source.SetupGet(x => x.FullyQualifiedName).Returns("a.name");
        }

        [TestMethod]
        public async Task DebugLogsWhenLogLevelAtDebug()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Debug, _correlationIdProvider.Object);

            // Act
            await queueLogger.DebugAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task DebugDoesNotLogWhenLogLevelAtInformation()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Information, _correlationIdProvider.Object);

            // Act
            await queueLogger.DebugAsync("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task InformationLogsWhenLogLevelAtInformation()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Information, _correlationIdProvider.Object);

            // Act
            await queueLogger.Information("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task InformationDoesNotLogWhenLogLevelAtWarning()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Warning, _correlationIdProvider.Object);

            // Act
            await queueLogger.Information("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task WarningLogsWhenLogLevelAtWarning()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Warning, _correlationIdProvider.Object);

            // Act
            await queueLogger.Warning("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task WarningDoesNotLogWhenLogLevelAtError()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Error, _correlationIdProvider.Object);

            // Act
            await queueLogger.Warning("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public async Task ErrorLogsWhenLogLevelAtError()
        {
            // Arrange
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Error, _correlationIdProvider.Object);

            // Act
            await queueLogger.Error("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public async Task LogQueueItemCreated()
        {
            // Arrange
            LogQueueItem result = null;
            _queue.Setup(x => x.EnqueueAsync(It.IsAny<LogQueueItem>())).Callback<LogQueueItem>(p => result = p);
            QueueLogger queueLogger = new QueueLogger(_runtimeEnvironment.Object, _queue.Object, _source.Object, _extension.Object, LogLevelEnum.Debug, _correlationIdProvider.Object);
            DateTimeOffset baseline = DateTimeOffset.UtcNow;

            // Act
            await queueLogger.Log(LogLevelEnum.Warning, "A message", new Exception("Some message"));

            // Assert
            Assert.AreEqual("A message", result.Message);
            Assert.AreEqual(LogLevelEnum.Warning, result.Level);
            Assert.IsTrue(result.LoggedAt >= baseline && result.LoggedAt <= DateTimeOffset.UtcNow);
            Assert.AreEqual("System.Exception", result.ExceptionName);
            Assert.IsNull(result.InnerExceptionName);
            Assert.AreEqual("a.name", result.Source);
        }
    }
}
