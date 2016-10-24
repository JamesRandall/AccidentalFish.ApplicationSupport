using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Core;
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
        static void Main(string[] args)
        {
            // don't do this in a production app - use something like Stephen Cleary's AsyncContext
            Task.Run(async () =>
            {
                IUnityContainer container = new UnityContainer();
                IDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

                string clientId = ConfigurationManager.AppSettings["keyVaultClientId"];
                string clientSecret = ConfigurationManager.AppSettings["keyVaultClientSecret"];
                string vaultUri = ConfigurationManager.AppSettings["keyVaultUri"];

                resolver
                    .UseCore()
                    .UseAzure()
                    .UseAsyncKeyVaultApplicationConfiguration(clientId, clientSecret, vaultUri);
                resolver.Register<ISampleWorker, SampleWorker>();

                ISampleWorker worker = resolver.Resolve<ISampleWorker>();
                await worker.Post();
                await worker.Read();
            }).Wait();
            
            Console.ReadKey();
        }
    }
}
