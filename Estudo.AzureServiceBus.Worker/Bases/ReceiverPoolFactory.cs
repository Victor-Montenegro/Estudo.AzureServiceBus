using Azure.Messaging.ServiceBus;

namespace Estudo.AzureServiceBus.Worker.Bases
{
    public class ReceiverPoolFactory : IReceiverPoolFactory
    {
        private readonly ServiceBusClient serviceBusClient;

        public ReceiverPoolFactory(ServiceBusClient serviceBusClient)
        {
            this.serviceBusClient = serviceBusClient;
        }

        public Task<ServiceBusProcessor> CreateProcessor(string queueName)
        {
            var options = new ServiceBusProcessorOptions()
            {
                PrefetchCount = 1,
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
                MaxAutoLockRenewalDuration = Timeout.InfiniteTimeSpan
            };

            var processor = serviceBusClient.CreateProcessor(queueName, options);

            return Task.FromResult(processor);
        }
    }

    public interface IReceiverPoolFactory
    {
        Task<ServiceBusProcessor> CreateProcessor(string queueName);
    }
}
