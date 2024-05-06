using Azure.Messaging.ServiceBus;
using Estudo.AzureServiceBus.Worker.Bases.Interfaces;

namespace Estudo.AzureServiceBus.Worker.Bases
{
    public class AzureConsumerFactory : IConsumerPoolFactory
    {
        private readonly ServiceBusClient serviceBusClient;

        public AzureConsumerFactory(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        public Task<ServiceBusProcessor> CreateConsumer(string queueName)
        {
            var options = new ServiceBusProcessorOptions()
            {
                PrefetchCount = 0,
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                MaxAutoLockRenewalDuration = Timeout.InfiniteTimeSpan
            };

            var processor = serviceBusClient.CreateProcessor(queueName, options);

            return Task.FromResult(processor);
        }
    }
}
