using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FichaCadastroRabbitMQ
{
    public interface IMessageRabbitMQ
    {
        IModel ConfigureVirtualHost(string virutalHost);
        void BasicPublish(string exchange, string routingKey, byte[] body);
        void QueueDeclare(string queue, bool durable = false, bool exclusive = false);
        void QueueBind(string queue, string exchange, string routingKey);
        void ExchangeDeclare(string exchange, string type);
        void BasicConsume(string queue, IBasicConsumer consumer, bool autoAck = true);
        EventingBasicConsumer InstanciarEventingBasicConsumer();
    }
}