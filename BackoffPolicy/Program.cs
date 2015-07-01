using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace BackoffPolicy
{
    class Program
    {
        private static int Counter = 0;

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);
            Bootstrapper.RegisterDependencies(resolver);

            IAsynchronousBackoffPolicy policy = resolver.Resolve<IAsynchronousBackoffPolicy>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    await policy.Execute(BackoffTask, cancellationTokenSource.Token);
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
            Console.WriteLine(Counter);
            return true;
        }

        private static Task IncrementCounter()
        {
            if (Counter > 0 && Counter % 10000 == 0) throw new Exception("Ooops");
            return Task.FromResult(Counter++);
        }
    }
}
