using FichaCadastroRabbitMQ;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Channels;
using FichaCadastroWorkerService.DTO.Ficha;

namespace FichaCadastroWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageRabbitMQ messageRabbitMQ;

        public Worker(ILogger<Worker> logger, IMessageRabbitMQ messageRabbitMQ)
        {
            _logger = logger;
            this.messageRabbitMQ = messageRabbitMQ;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                CadastrarFichaTopic();

            }
        }

        private void CadastrarFichaTopic()
        {
            messageRabbitMQ.ConfigureVirtualHost("ficha");

            messageRabbitMQ.QueueBind("ficha-queue", "ficha-exchange", "ficha-cadastro-routeKey");

            var fichaCadastroEventingBasicConsumer = messageRabbitMQ.InstanciarEventingBasicConsumer();

            fichaCadastroEventingBasicConsumer.Received += (model, basicDeliverEventArgs) =>
            {
                basicDeliverEventArgs.RoutingKey = "ficha-cadastro-routeKey";
                basicDeliverEventArgs.Exchange = "ficha-exchange";

                var body = basicDeliverEventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Dados recebidos no formato json | {messageJson}");


                var ficha = JsonSerializer.Deserialize<FichaCreateDTO>(messageJson);

                Console.WriteLine($"Nome: {ficha!.NomeCompleto}");
                Console.WriteLine($"Email: {ficha!.EmailInformado}");
                Console.WriteLine($"Data de Nascimento: {ficha!.DataDeNascimento}");
            };

            messageRabbitMQ.BasicConsume(queue: "ficha-queue",
                             consumer: fichaCadastroEventingBasicConsumer);
        }
    }
}