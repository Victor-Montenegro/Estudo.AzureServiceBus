using Estudo.AzureServiceBus.Worker.Tasks;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((buider, services) =>
    {
        Infrastructure(buider.Configuration,services);
        services.AddHostedService<PaymentWebhookNotificationTask>();
    })
    .Build();

void Infrastructure(IConfiguration configuration, IServiceCollection services)
{

}

host.Run();
