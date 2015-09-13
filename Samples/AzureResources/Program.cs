using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace AzureResources
{
    public class SampleQueueItem
    {
        public string Message { get; set; }
    }

    class Program
    {
        private const string ServiceBusConnectionString = "servicebusconnectionstring";

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            IDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);
            resolver.UseCore().UseAzure();

            IAzureQueueFactory queueFactory = resolver.Resolve<IAzureQueueFactory>();
            IAzureResourceManager resourceManager = resolver.Resolve<IAzureResourceManager>();

            IAsynchronousQueue<SampleQueueItem> queue = queueFactory.CreateAsynchronousBrokeredMessageQueue<SampleQueueItem>(ServiceBusConnectionString, "aqueue");
            Task<bool> resultTask = resourceManager.CreateIfNotExistsAsync(queue);
            resultTask.Wait();
            Console.WriteLine(resultTask.Result);

            Task<bool> deleteResultTask = resourceManager.DeleteIfExistsAsync(queue);
            deleteResultTask.Wait();
            Console.WriteLine(deleteResultTask.Result);
        }
    }
}
