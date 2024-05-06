using Azure.Messaging.ServiceBus;

namespace Estudo.AzureServiceBus.Worker.Bases.Interfaces
{
    public interface IConsumerPoolFactory
    {
        Task<ServiceBusProcessor> CreateConsumer(string queueName);
    }
}
