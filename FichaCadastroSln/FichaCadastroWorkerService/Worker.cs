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

                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    messageRabbitMQ.ConfigureVirtualHost("ficha");

                    CadastroNovoTopicPost();
                    //CadastroDeletarTopicPost();
                    //CadastroEnviarEmailTopicPost();

                    Console.ReadLine();

                }
                catch (Exception ex)
                {
                    //await Task.Delay(10000, stoppingToken);
                }
            }
        }

        private void CadastroNovoTopicPost()
        {
            string routingKey = "ficha-cadastro.novo-routeKey-topic";
            string queue = "ficha-cadastro-novo-queue-topic";

            var basicConsumer = messageRabbitMQ.InstanciarEventingBasicConsumer();

            basicConsumer.Received += (model, basicDeliverEventArgs) =>
            {
                basicDeliverEventArgs.RoutingKey = routingKey;
                basicDeliverEventArgs.Exchange = "ficha-exchange-topic";

                var body = basicDeliverEventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Dados recebidos no formato json | {messageJson}");


                var ficha = JsonSerializer.Deserialize<FichaCreateDTO>(messageJson);

                Console.WriteLine($"Mensagem : {ficha!.Mensagem}");
                Console.WriteLine($"Nome: {ficha!.NomeCompleto}");
                Console.WriteLine($"Email: {ficha!.EmailInformado}");
                Console.WriteLine($"Data de Nascimento: {ficha!.DataDeNascimento}");
            };

            messageRabbitMQ.BasicConsume(queue: queue, consumer: basicConsumer);
        }

        private void CadastroDeletarTopicPost()
        {
            string routingKey = "ficha-cadastro.deletar-routeKey-topic";
            string queue = "ficha-deletar-queue-topic";

            messageRabbitMQ.QueueBind(queue, "ficha-exchange-topic", routingKey);

            var basicConsumer = messageRabbitMQ.InstanciarEventingBasicConsumer();

            basicConsumer.Received += (model, basicDeliverEventArgs) =>
            {
                basicDeliverEventArgs.RoutingKey = routingKey;
                basicDeliverEventArgs.Exchange = "ficha-exchange-topic";

                var body = basicDeliverEventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Dados recebidos no formato json | {messageJson}");


                var ficha = JsonSerializer.Deserialize<FichaCreateDTO>(messageJson);

                Console.WriteLine($"Mensagem : {ficha!.Mensagem}");
                Console.WriteLine($"Nome: {ficha!.NomeCompleto}");
                Console.WriteLine($"Email: {ficha!.EmailInformado}");
                Console.WriteLine($"Data de Nascimento: {ficha!.DataDeNascimento}");
            };

            messageRabbitMQ.BasicConsume(queue: queue, consumer: basicConsumer);
        }


        private void CadastroEnviarEmailTopicPost()
        {
            string routingKey = "ficha-cadastro.enviar-email-routeKey-topic";
            string queue = "ficha-cadastro-enviar-email-queue-topic";

            messageRabbitMQ.QueueBind(queue, "ficha-exchange-topic", routingKey);

            var basicConsumer = messageRabbitMQ.InstanciarEventingBasicConsumer();

            basicConsumer.Received += (model, basicDeliverEventArgs) =>
            {
                basicDeliverEventArgs.RoutingKey = routingKey;
                basicDeliverEventArgs.Exchange = "ficha-exchange-topic";

                var body = basicDeliverEventArgs.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Dados recebidos no formato json | {messageJson}");


                var ficha = JsonSerializer.Deserialize<FichaCreateDTO>(messageJson);

                Console.WriteLine($"Mensagem : {ficha!.Mensagem}");
                Console.WriteLine($"Nome: {ficha!.NomeCompleto}");
                Console.WriteLine($"Email: {ficha!.EmailInformado}");
                Console.WriteLine($"Data de Nascimento: {ficha!.DataDeNascimento}");
            };

            messageRabbitMQ.BasicConsume(queue: queue, consumer: basicConsumer);
        }
    }
}