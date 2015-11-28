using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace TopicsAndSubscriptions
{
    [DataContract]
    class MyMessage
    {
        [DataMember]
        public string SaySomething { get; set; }
    }

    class Program
    {
        private static readonly IComponentIdentity SampleComponent = new ComponentIdentity("accidentalfish.samples.topicsandsubscriptions.processor");

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver
                .UseCore()
                .UseAzure();

            IApplicationResourceFactory applicationResourceFactory = container.Resolve<IApplicationResourceFactory>();
            string secondSubscriptionName = applicationResourceFactory.Setting(SampleComponent, "second-subscription");
            IAsynchronousTopic<MyMessage> topic = applicationResourceFactory.GetAsyncTopic<MyMessage>(SampleComponent);
            IAsynchronousSubscription<MyMessage> firstSubscription = applicationResourceFactory.GetAsyncSubscription<MyMessage>(SampleComponent);
            IAsynchronousSubscription<MyMessage> secondSubscription = applicationResourceFactory.GetAsyncSubscription<MyMessage>(secondSubscriptionName, SampleComponent);

            // below is exceedingly naive but is just to illustrate api usage
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1500);
                    await firstSubscription.RecieveAsync(m =>
                    {
                        System.Console.Write("First Subscription: ");
                        System.Console.WriteLine(m.SaySomething);
                        return Task.FromResult(true);
                    });
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3333);
                    await secondSubscription.RecieveQueueItemAsync(m =>
                    {
                        System.Console.Write("Second Subscription: ");
                        System.Console.WriteLine(m.Item.SaySomething);
                        return Task.FromResult(true);
                    });
                }
            });

            topic.SendAsync(new MyMessage
            {
                SaySomething = "Hello World"
            }, new Dictionary<string, object>
            {
                {"P1", 56},
                {"P2", "Zaphod"}
            });

            System.Console.Read();
        }
    }
}
