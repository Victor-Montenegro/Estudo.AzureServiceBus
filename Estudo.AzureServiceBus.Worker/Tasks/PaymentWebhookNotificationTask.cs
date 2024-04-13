using Azure.Messaging.ServiceBus;
using Estudo.AzureServiceBus.Domain.Messages;
using Estudo.AzureServiceBus.Worker.Bases;

namespace Estudo.AzureServiceBus.Worker.Tasks
{
    internal class PaymentWebhookNotificationTask : ReceiverBaseTask<PaymentWebhookNotificationMessage>
    {
        public PaymentWebhookNotificationTask(
            ILogger<ReceiverBaseTask<PaymentWebhookNotificationMessage>> logger,
            IReceiverPoolFactory receiverPoolFactory,
            IServiceProvider serviceProvider,
            IConfiguration configuration
            ) : base(
                configuration.GetSection("PaymentWebhookNotificationTask:Queue").Value,
                nameof(PaymentWebhookNotificationTask), 
                receiverPoolFactory,
                logger)
        {
        }

        public override async Task Run(ProcessMessageEventArgs args, PaymentWebhookNotificationMessage message)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
