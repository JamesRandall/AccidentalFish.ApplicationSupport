using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Logging.Implementation
{
    [TestClass]
    public class LoggerTests
    {
        private Mock<IAsynchronousQueue<LogQueueItem>> _queue;
        private Mock<IFullyQualifiedName> _source;

        [TestInitialize]
        public void Setup()
        {
            _queue = new Mock<IAsynchronousQueue<LogQueueItem>>();
            _source = new Mock<IFullyQualifiedName>();
            _source.SetupGet(x => x.FullyQualifiedName).Returns("a.name");
        }

        [TestMethod]
        public void DebugLogsWhenLogLevelAtDebug()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Debug);

            // Act
            logger.Debug("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public void DebugDoesNotLogWhenLogLevelAtInformation()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Information);

            // Act
            logger.Debug("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public void InformationLogsWhenLogLevelAtInformation()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Information);

            // Act
            logger.Information("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public void InformationDoesNotLogWhenLogLevelAtWarning()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Warning);

            // Act
            logger.Information("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public void WarningLogsWhenLogLevelAtWarning()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Warning);

            // Act
            logger.Warning("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public void WarningDoesNotLogWhenLogLevelAtError()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Error);

            // Act
            logger.Warning("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()), Times.Never);
        }

        [TestMethod]
        public void ErrorLogsWhenLogLevelAtError()
        {
            // Arrange
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Error);

            // Act
            logger.Error("a message");

            // Assert
            _queue.Verify(x => x.EnqueueAsync(It.IsAny<LogQueueItem>()));
        }

        [TestMethod]
        public void LogQueueItemCreated()
        {
            // Arrange
            LogQueueItem result = null;
            _queue.Setup(x => x.EnqueueAsync(It.IsAny<LogQueueItem>())).Callback<LogQueueItem>(p => result = p);
            Logger logger = new Logger(_queue.Object, _source.Object, LogLevelEnum.Debug);
            DateTimeOffset baseline = DateTimeOffset.UtcNow;

            // Act
            logger.Log(LogLevelEnum.Warning, "A message", new Exception("Some message"));

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
