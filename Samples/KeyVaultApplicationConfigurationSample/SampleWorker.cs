using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Threading;

namespace KeyVaultApplicationConfigurationSample
{
    internal class SampleWorker : ISampleWorker
    {
        private static readonly IComponentIdentity SampleComponent = new ComponentIdentity("accidentalfish.samples.topicsandsubscriptions.processor");
        private readonly AsyncLazy<IAsynchronousTopic<MyMessage>> _topic;
        private readonly AsyncLazy<IAsynchronousSubscription<MyMessage>> _subscription;

        public SampleWorker(IAsyncApplicationResourceFactory applicationResourceFactory)
        {
            _topic = new AsyncLazy<IAsynchronousTopic<MyMessage>>(() => applicationResourceFactory.GetAsyncTopicAsync<MyMessage>(SampleComponent));
            _subscription = new AsyncLazy<IAsynchronousSubscription<MyMessage>>(() => applicationResourceFactory.GetAsyncSubscriptionAsync<MyMessage>(SampleComponent));
        }

        public async Task Post()
        {
            await (await _topic).SendAsync(new MyMessage {SaySomething = "Hello World"});
        }

        public async Task Read()
        {
            await (await _subscription).RecieveAsync(msg =>
            {
                if (msg != null)
                {
                    Console.WriteLine(msg.SaySomething);
                }
                return Task.FromResult(msg != null);
            });
        }
    }
}
