using Estudo.AzureServiceBus.Worker.Bases;
using Estudo.AzureServiceBus.Worker.Bases.Interfaces;
using Estudo.AzureServiceBus.Worker.Tasks;
using Microsoft.Extensions.Azure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((buider, services) =>
    {
        Infrastructure(buider.Configuration,services);

        services.AddHostedService<PaymentWebhookNotificationTask>();
        services.AddHostedService<RefundPaymentWebhookNotificationTask>();
    })
    .Build();

void Infrastructure(IConfiguration configuration, IServiceCollection services)
{
    string? azureServiceBusConnection = configuration.GetSection("Azure:ServiceBusConnection").Value;

    services.AddAzureClients(options => 
    {
        options.AddServiceBusClient(azureServiceBusConnection);
    });

    services.AddSingleton<IConsumerPoolFactory, AzureConsumerFactory>();
}

host.Run();
