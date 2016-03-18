using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Policies.Implementation
{
    [TestClass]
    public class AsynchronousIntervalTimerTests
    {
        private const double PretendDelayInMilliseconds = 500.0;

        [TestMethod]
        public async Task IntervalOccursBetweenTasksWithCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();
            int repeatCount = 0;
           
            // Act
            await timer.ExecuteAsync(ct =>
            { 
                repeatCount++;
                return Task.FromResult(repeatCount < 2);
            }, source.Token);
            
            // Assert
            Assert.AreEqual(2, repeatCount);
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), source.Token));
            source.Dispose();
        }

        [TestMethod]
        public async Task ReturningFalseCancelsTimerWithCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();
            int repeatCount = 0;

            // Act
            await timer.ExecuteAsync(ct =>
            {
                repeatCount++;
                return Task.FromResult(false);
            }, source.Token);

            // Assert
            Assert.AreEqual(1, repeatCount);
            source.Dispose();
        }

        [TestMethod]
        public async Task ShutdownActionInvokedWithCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            bool shutdownActionInvoked = false;
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();
            
            // Act
            await timer.ExecuteAsync(ct => Task.FromResult(false), source.Token, () => shutdownActionInvoked = true);

            // Assert
            Assert.IsTrue(shutdownActionInvoked);
            source.Dispose();
        }

        [TestMethod]
        public async Task DelayOnExecuteWaitsForIntervalBeforeFirstTaskWithCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), true);
            CancellationTokenSource source = new CancellationTokenSource();

            // Act
            await timer.ExecuteAsync(ct => Task.FromResult(false), source.Token);

            // Assert
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), source.Token));
        }

        [TestMethod]
        public async Task SettingCancellationTokenCancelsTimer()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();
            int repeatCount = 0;

            // Act
            await timer.ExecuteAsync(ct =>
            {
                source.Cancel(true);
                repeatCount++;
                return Task.FromResult(true);
            }, source.Token);

            // Assert
            Assert.AreEqual(1, repeatCount);
            source.Dispose();
        }

        [TestMethod]
        public async Task IntervalOccursBetweenTasksWithoutCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            int repeatCount = 0;

            // Act
            await timer.ExecuteAsync(() =>
            {
                repeatCount++;
                return Task.FromResult(repeatCount < 2);
            });

            // Assert
            Assert.AreEqual(2, repeatCount);
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds)));
        }

        [TestMethod]
        public async Task ReturningFalseCancelsTimerWithoutCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            int repeatCount = 0;

            // Act
            await timer.ExecuteAsync(() =>
            {
                repeatCount++;
                return Task.FromResult(false);
            });

            // Assert
            Assert.AreEqual(1, repeatCount);
        }

        [TestMethod]
        public async Task ShutdownActionInvokedWithoutCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            bool shutdownActionInvoked = false;
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);

            // Act
            await timer.ExecuteAsync(() => Task.FromResult(false), () => shutdownActionInvoked = true);

            // Assert
            Assert.IsTrue(shutdownActionInvoked);
        }

        [TestMethod]
        public async Task DelayOnExecuteWaitsForIntervalBeforeFirstTaskWithoutCancellationToken()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            AsynchronousIntervalTimer timer = new AsynchronousIntervalTimer(delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), true);

            // Act
            await timer.ExecuteAsync(() => Task.FromResult(false));

            // Assert
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds)));
        }
    }
}
