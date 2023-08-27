using FichaCadastroRabbitMQ;
using FichaCadastroWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IFactoryConnectionRabbitMQ, FactoryConnectionRabbitMQ>();
        services.AddSingleton<IMessageRabbitMQ, MessageRabbitMQ>();
    })
    .Build();

host.Run();
