using System.Runtime.Serialization;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
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
            AccidentalFish.ApplicationSupport.Core.Bootstrapper.RegisterDependencies(container);
            AccidentalFish.ApplicationSupport.Azure.Bootstrapper.RegisterDependencies(container);

            IApplicationResourceFactory applicationResourceFactory = container.Resolve<IApplicationResourceFactory>();
            string secondSubscriptionName = applicationResourceFactory.Setting(SampleComponent, "second-subscription");
            IAsynchronousTopic<MyMessage> topic = applicationResourceFactory.GetTopic<MyMessage>(SampleComponent);
            IAsynchronousSubscription<MyMessage> firstSubscription = applicationResourceFactory.GetSubscription<MyMessage>(SampleComponent);
            IAsynchronousSubscription<MyMessage> secondSubscription = applicationResourceFactory.GetSubscription<MyMessage>(secondSubscriptionName, SampleComponent);

            // below is exceedingly naive but is just to illustrate api usage
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1500);
                    await firstSubscription.Recieve(m =>
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
                    await secondSubscription.Recieve(m =>
                    {
                        System.Console.Write("Second Subscription: ");
                        System.Console.WriteLine(m.SaySomething);
                        return Task.FromResult(true);
                    });
                }
            });
            
            topic.SendAsync(new MyMessage
            {
                SaySomething = "Hello World"
            });

            System.Console.Read();
        }
    }
}
