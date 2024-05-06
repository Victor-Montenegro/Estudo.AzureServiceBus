using Azure.Messaging.ServiceBus;
using Estudo.AzureServiceBus.Domain.Messages;
using Estudo.AzureServiceBus.Worker.Bases;
using Estudo.AzureServiceBus.Worker.Bases.Interfaces;

namespace Estudo.AzureServiceBus.Worker.Tasks
{
    internal class PaymentWebhookNotificationTask : ConsumerBaseTask<PaymentWebhookNotificationMessage>
    {
        public PaymentWebhookNotificationTask(
            ILogger<ConsumerBaseTask<PaymentWebhookNotificationMessage>> logger,
            IConsumerPoolFactory receiverPoolFactory,
            IServiceProvider serviceProvider,
            IConfiguration configuration
            ) : base(
                false,
                configuration.GetSection("PaymentWebhookNotificationTask:Queue").Value,
                nameof(PaymentWebhookNotificationTask), 
                receiverPoolFactory,
                logger)
        {
        }

        public override Task Run(ProcessMessageEventArgs args, PaymentWebhookNotificationMessage message)
        {
            try
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
