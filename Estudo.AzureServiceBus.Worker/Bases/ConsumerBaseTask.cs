using Azure.Messaging.ServiceBus;
using Estudo.AzureServiceBus.Worker.Bases.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
namespace Estudo.AzureServiceBus.Worker.Bases
{
    public abstract class ConsumerBaseTask<TMessage> : BackgroundService
    {
        private readonly string _taskName;
        private readonly string _queueName;
        private readonly bool _reProcessMessages;

        private readonly IConsumerPoolFactory _receiverPoolFactory;
        protected readonly ILogger<ConsumerBaseTask<TMessage>> _logger;

        protected ConsumerBaseTask(bool reProcessMessages, string queueName, string taskName, IConsumerPoolFactory receiverPoolFactory, ILogger<ConsumerBaseTask<TMessage>> logger)
        {
            _logger = logger;
            _taskName = taskName;
            _queueName = queueName;
            _receiverPoolFactory = receiverPoolFactory;
            _reProcessMessages = reProcessMessages;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning($"Starting task {_taskName} in queue {_queueName}");
            try
            {
                var consumer = await _receiverPoolFactory.CreateConsumer(_queueName);

                consumer.ProcessMessageAsync += HandlerMessage;
                consumer.ProcessErrorAsync += HandlerError;

                await consumer.StartProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Critical Error in task {_taskName}");
            }
        }

        private Task HandlerError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "HandlerError");

            return Task.CompletedTask;
        }

        private async Task HandlerMessage(ProcessMessageEventArgs args)
        {
            try
            {
                var time = new Stopwatch();

                time.Start();
                var message = DeserializeMessage(args.Message);

                await Run(args, message);

                await CompleteMessage(args);

                time.Stop();

                _logger.LogWarning($"Execution task {_taskName} finished in {time.Elapsed}. Message : {args.Message.Body}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in task {_taskName}. Message : {args.Message.Body}");

                await AbandonMessage(args);
            }
        }

        private async Task AbandonMessage(ProcessMessageEventArgs args)
        {
            try
            {
                if (_reProcessMessages)
                    await args.DeadLetterMessageAsync(args.Message);
                else
                    await args.DeferMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro in AbondonMessage");
                throw;
            }
        }

        private async Task CompleteMessage(ProcessMessageEventArgs args)
        {
            await args.CompleteMessageAsync(args.Message);
        }

        private TMessage DeserializeMessage(ServiceBusReceivedMessage message)
        {
            var deserializedMessage = JsonConvert.DeserializeObject<TMessage>(message.Body.ToString());

            return deserializedMessage;
        }

        public abstract Task Run(ProcessMessageEventArgs args, TMessage message);
    }
}
