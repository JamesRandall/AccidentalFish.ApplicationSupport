using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.BackoffProcesses;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace BackoffQueueProcessorSample
{
    public class SampleQueueMessage
    {
        public string Text { get; set; }
    }

    class Program
    {
        public const string StorageAccountConnectionString = "UseDevelopmentStorage=true;";
        public const string QueueName = "backoffqueueprocessorsamplequeue";

        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // Set up dependencies
            IUnityContainer unityContainer = new UnityContainer();
            IDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(unityContainer);
            resolver.UseCore(defaultTraceLoggerMinimumLogLevel: LogLevelEnum.Verbose).UseAzure();

            // Normally these would be injected by the container
            IAzureResourceManager azureResourceManager = resolver.Resolve<IAzureResourceManager>();
            IAsynchronousBackoffPolicy backoffPolicy = resolver.Resolve<IAsynchronousBackoffPolicy>();
            IQueueFactory queueFactory = resolver.Resolve<IQueueFactory>();
            ILoggerFactory loggerFactory = resolver.Resolve<ILoggerFactory>();
            Processor processor = new Processor(backoffPolicy, queueFactory, loggerFactory);

            // Drop some queue items on

            IQueue<SampleQueueMessage> queue = queueFactory.CreateQueue<SampleQueueMessage>(StorageAccountConnectionString, QueueName);
            azureResourceManager.CreateIfNotExists(queue);
            for (int counter = 0; counter < 200; counter++)
            {
                queue.Enqueue(new SampleQueueMessage
                {
                    Text = $"Message {counter}"
                });
            }

            // Start the processor - in a worker role / service this would often be hosted in the ComponentHost (see sample).
            Task.Run(async () => await processor.StartAsync(cancellationTokenSource.Token));

            Console.ReadLine();
            cancellationTokenSource.Cancel();
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
        }
    }

    public class Processor : BackoffQueueProcessor<SampleQueueMessage>
    {
        private readonly Random _random = new Random();

        public Processor(
            IAsynchronousBackoffPolicy backoffPolicy,
            IQueueFactory queueFactory,
            ILoggerFactory loggerFactory) :
                base(backoffPolicy,
                    queueFactory.CreateAsynchronousQueue<SampleQueueMessage>(Program.StorageAccountConnectionString,
                        Program.QueueName),
                    loggerFactory.CreateLogger())
        {
            
        }

        protected override async Task<bool> HandleRecievedItemAsync(IQueueItem<SampleQueueMessage> message)
        {
            SampleQueueMessage item = message.Item;
            if (_random.Next(100) < 10)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                Console.WriteLine("Abandoning queue message - will be placed back on queue");
                return false;
            }

            Console.WriteLine($"Processed queue message {item.Text}");
            return true;
        }

        public override IComponentIdentity ComponentIdentity => new ComponentIdentity("sample.queueprocessor");
    }
}
