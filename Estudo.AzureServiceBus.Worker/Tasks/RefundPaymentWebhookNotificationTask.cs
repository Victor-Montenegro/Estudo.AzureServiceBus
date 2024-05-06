using Azure.Messaging.ServiceBus;
using Estudo.AzureServiceBus.Domain.Messages;
using Estudo.AzureServiceBus.Worker.Bases;
using Estudo.AzureServiceBus.Worker.Bases.Interfaces;

namespace Estudo.AzureServiceBus.Worker.Tasks
{
    internal class RefundPaymentWebhookNotificationTask : ConsumerBaseTask<RefundPaymentWebhookNotificationMessage>
    {
        public RefundPaymentWebhookNotificationTask(
          ILogger<ConsumerBaseTask<RefundPaymentWebhookNotificationMessage>> logger,
          IConsumerPoolFactory consumerPoolFactory,
          IServiceProvider serviceProvider,
          IConfiguration configuration
          ) : base(
              false,
              configuration.GetSection("RefundPaymentWebhookNotificationTask:Queue").Value,
              nameof(RefundPaymentWebhookNotificationTask),
              consumerPoolFactory,
              logger)
        {
        }

        public override  Task Run(ProcessMessageEventArgs args, RefundPaymentWebhookNotificationMessage message)
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
