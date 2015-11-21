using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace BackoffPolicy
{
    class Program
    {
        private static int _counter = 0;

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver.UseCore(defaultTraceLoggerMinimumLogLevel: LogLevelEnum.Verbose);

            IAsynchronousBackoffPolicy policy = resolver.Resolve<IAsynchronousBackoffPolicy>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    await policy.ExecuteAsync(BackoffTask, cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            Console.WriteLine("Press a key to finish...");
            Console.ReadKey();
            cancellationTokenSource.Cancel();
        }

        private static async Task<bool> BackoffTask()
        {
            await IncrementCounter();
            Console.WriteLine(_counter);
            return _counter < 100 || _counter > 110; // force backoff between 100 and 110
        }

        private static Task IncrementCounter()
        {
            if (_counter > 0 && _counter % 10000 == 0) throw new Exception("Ooops");
            return Task.FromResult(_counter++);
        }
    }
}
