using System;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace LargeMessageQueue
{
    class SampleLargeQueueItem
    {
        public string LotsOfText { get; set; }
    }

    class Program
    {
        private const string StorageAccountConnectionString = "UseDevelopmentStorage=true;";

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(container);
            dependencyResolver
                .UseCore()
                .UseAzure();

            IAzureResourceManager azureResourceManager = dependencyResolver.Resolve<IAzureResourceManager>();
            ILargeMessageQueueFactory largeMessageQueueFactory = dependencyResolver.Resolve<ILargeMessageQueueFactory>();
            ILargeMessageQueue<SampleLargeQueueItem> queue = largeMessageQueueFactory.Create<SampleLargeQueueItem>(StorageAccountConnectionString, "samplereferencequeue", "samplequeueblobs");

            azureResourceManager.CreateIfNotExistsAsync(queue.ReferenceQueue).Wait();
            azureResourceManager.CreateIfNotExistsAsync(queue.BlobRepository).Wait();

            queue.EnqueueAsync(CreateLargeQueueItem()).Wait();
            Console.WriteLine("Queued large item. Press a key to dequeue...");
            Console.ReadKey();
            queue.DequeueAsync(message =>
            {
                SampleLargeQueueItem item = message.Item;
                Console.WriteLine("Dequeued large item of text length {0}. Press a key....", item.LotsOfText.Length);
                return Task.FromResult(true);
            }).Wait();
            Console.ReadKey();
        }

        private static SampleLargeQueueItem CreateLargeQueueItem()
        {
            const string repeatedString = "Hello World ";
            StringBuilder sb = new StringBuilder();
            for (int counter = 0; counter < 10000; counter++)
            {
                sb.Append(repeatedString);
            }

            return new SampleLargeQueueItem
            {
                LotsOfText = sb.ToString()
            };
        }
    }
}
