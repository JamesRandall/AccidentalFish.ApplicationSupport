using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Azure.Configuration;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;

namespace KeyVaultApplicationConfigurationSample
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
            IDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            string clientId = ConfigurationManager.AppSettings["keyVaultClientId"];
            string clientSecret = ConfigurationManager.AppSettings["keyVaultClientSecret"];
            string vaultUri = ConfigurationManager.AppSettings["keyVaultUri"];

            resolver
                .UseCore()
                .UseAzure()
                .UseKeyVaultApplicationConfiguration(clientId, clientSecret, vaultUri);

            IKeyVaultConfiguration configuration = (IKeyVaultConfiguration)resolver.Resolve<IConfiguration>();
            configuration.Preload().RunSynchronously();

            IApplicationResourceFactory applicationResourceFactory = resolver.Resolve<IApplicationResourceFactory>();
            IAsynchronousTopic<MyMessage> topic = applicationResourceFactory.GetAsyncTopic<MyMessage>(SampleComponent);
            topic.Send(new MyMessage {SaySomething = "Hello World"});
            
            IAsynchronousSubscription<MyMessage> subscription = applicationResourceFactory.GetAsyncSubscription<MyMessage>(SampleComponent);
            subscription.RecieveAsync(msg =>
            {
                return Task.FromResult(msg != null);
            });
            
            Console.ReadKey();
        }
    }
}
