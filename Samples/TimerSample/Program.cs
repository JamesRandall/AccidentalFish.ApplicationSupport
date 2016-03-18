using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace TimerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Regular timer (1) or interval timer (2 or any other key)?");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
            {
                RegularTimerDemo();
            }
            else
            {
                IntervalTimerDemo();
            }

            Console.WriteLine("Cancelled. Press a key");
            Console.ReadKey();
        }

        private static void IntervalTimerDemo()
        {
            Console.WriteLine("Interval timer demo");
            var container = new UnityContainer();
            var dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver.UseCore();

            var timerFactory = dependencyResolver.Resolve<ITimerFactory>();
            var timer = timerFactory.CreateAsynchronousIntervalTimer(TimeSpan.FromSeconds(1));
            var source = new CancellationTokenSource();
            var counter = 0;

            timer.ExecuteAsync((ct) =>
            {
                Console.WriteLine($"Step {counter++}");
                return Task.FromResult(true);
            }, source.Token);

            Console.ReadKey();
            source.Cancel();
        }

        private static void RegularTimerDemo()
        {
            Console.WriteLine("Regular timer demo");
            var container = new UnityContainer();
            var dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);

            dependencyResolver.UseCore();

            var timerFactory = dependencyResolver.Resolve<ITimerFactory>();
            var timer = timerFactory.CreateAsynchronousRegularTimer(TimeSpan.FromSeconds(1));
            var source = new CancellationTokenSource();
            var counter = 0;

            timer.ExecuteAsync((ct) =>
            {
                Console.WriteLine($"Step {counter++}");
            }, source.Token);

            Console.ReadKey();
            source.Cancel();
        }
    }
}
