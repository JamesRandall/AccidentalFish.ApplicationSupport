using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Policies.Implementation
{
    [TestClass]
    public class AsynchronousRegularTimerTests
    {
        private const double PretendDelayInMilliseconds = 500.0;

        [TestMethod]
        public async Task IntervalAndActionIsTriggered()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            Mock<ITimerThreadPoolExecuter> executer = new Mock<ITimerThreadPoolExecuter>();
            executer.Setup(x => x.Run(It.IsAny<Action>(), It.IsAny<CancellationToken>())).Callback((Action a, CancellationToken c) =>
            {
                a();
            }).Returns(Task.FromResult(0));
            AsynchronousRegularTimer timer = new AsynchronousRegularTimer(executer.Object, delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();
            int repeatCount = 0;
            
            // Act
            await timer.ExecuteAsync(ct =>
            {
                repeatCount++;
                source.Cancel();                
            }, source.Token);

            // Assert
            Assert.AreEqual(1, repeatCount);
            executer.Verify(x => x.Run(It.IsAny<Action>(), It.IsAny<CancellationToken>()));
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), source.Token));
        }

        [TestMethod]
        public async Task DelayBeforeFirstExecute()
        {
            // Arrange
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            Mock<ITimerThreadPoolExecuter> executer = new Mock<ITimerThreadPoolExecuter>();
            AsynchronousRegularTimer timer = new AsynchronousRegularTimer(executer.Object, delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), true);
            CancellationTokenSource source = new CancellationTokenSource();
            delay.Setup(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), source.Token)).Callback(() =>
            {
                source.Cancel();
            }).Returns(Task.FromResult(0));

            // Act
            await timer.ExecuteAsync(ct =>
            {
            
            }, source.Token);

            // Assert
            executer.Verify(x => x.Run(It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Never);
            delay.Verify(x => x.Delay(TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), source.Token));
        }

        [TestMethod]
        public async Task ShutdownActionIsCalled()
        {
            // Arrange
            bool didCallShutdown = false;
            Mock<IAsynchronousDelay> delay = new Mock<IAsynchronousDelay>();
            Mock<ITimerThreadPoolExecuter> executer = new Mock<ITimerThreadPoolExecuter>();
            executer.Setup(x => x.Run(It.IsAny<Action>(), It.IsAny<CancellationToken>())).Callback((Action a, CancellationToken c) =>
            {
                a();
            }).Returns(Task.FromResult(0));
            AsynchronousRegularTimer timer = new AsynchronousRegularTimer(executer.Object, delay.Object, TimeSpan.FromMilliseconds(PretendDelayInMilliseconds), false);
            CancellationTokenSource source = new CancellationTokenSource();

            // Act
            await timer.ExecuteAsync(ct =>
            {
                source.Cancel();
            }, source.Token, () => didCallShutdown=true);

            // Assert
            Assert.IsTrue(didCallShutdown);
        }
    }
}
