
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Estudo.AzureServiceBus.Worker.Bases
{
    public abstract class ReceiverBaseTask<TMessage> : BackgroundService
    {
        private readonly string _taskName;
        private readonly string _queueName;
        private readonly IReceiverPoolFactory _receiverPoolFactory;
        protected readonly ILogger<ReceiverBaseTask<TMessage>> _logger;

        protected ReceiverBaseTask(string queueName, string taskName, IReceiverPoolFactory receiverPoolFactory, ILogger<ReceiverBaseTask<TMessage>> logger)
        {
            _logger = logger;
            _taskName = taskName;
            _queueName = queueName;
            _receiverPoolFactory = receiverPoolFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning($"Starting task {_taskName} in queue {_queueName}");
            try
            {
                var processor = await _receiverPoolFactory.CreateProcessor(_queueName);

                processor.ProcessMessageAsync += HandlerMessage;
                processor.ProcessErrorAsync += HandlerError;

                await processor.StartProcessingAsync();
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

                _logger.LogWarning($"Execution task {_taskName} finished in {time.Elapsed}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error in task {_taskName}");

                await AbandonMessage(args);
            }
        }

        private async Task AbandonMessage(ProcessMessageEventArgs args)
        {
            await args.AbandonMessageAsync(args.Message);
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
