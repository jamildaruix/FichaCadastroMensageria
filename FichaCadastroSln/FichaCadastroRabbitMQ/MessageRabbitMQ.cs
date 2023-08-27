using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Threading.Channels;

namespace FichaCadastroRabbitMQ
{
    public class MessageRabbitMQ : IMessageRabbitMQ
    {
        private IModel _channel;
        private readonly IFactoryConnectionRabbitMQ _factoryConnectionRabbitMQ;

        public MessageRabbitMQ(IFactoryConnectionRabbitMQ factoryConnectionRabbitMQ)
        {
            _factoryConnectionRabbitMQ = factoryConnectionRabbitMQ;

        }

        public void ExchangeDeclare(string exchange, string type)
        {
            _channel.ExchangeDeclare(exchange: exchange, type: type);
        }

        public void QueueDeclare(string queue, bool durable = false, bool exclusive = false)
        {
            _channel.QueueDeclare(queue: queue,
                                  durable: durable,
                                  exclusive: exclusive);
        }

        /// <summary>
        /// Responsabilidade do produtor informando o local que será enviado a publicação
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        public void BasicPublish(string exchange, string routingKey, byte[] body)
        {
            _channel.BasicPublish(
               exchange: exchange,
               routingKey: routingKey,
               body: body);
        }

        /// <summary>
        /// Binding responsavel para configurar com o exchante sempre será do consumidor
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        public void QueueBind(string queue, string exchange, string routingKey)
        {
            _channel.QueueBind(queue: queue,
                                  exchange: exchange,
                                  routingKey: routingKey);
        }

        public void BasicConsume(string queue, IBasicConsumer consumer, bool autoAck = true)
        {
            _channel.BasicConsume(queue: queue,
                                  autoAck: true,
                                  consumer: consumer);
        }

        public IModel ConfigureVirtualHost(string virutalHost)
        {
            _channel = _factoryConnectionRabbitMQ.CreateConnection(virutalHost);
            return _channel;
        }

        public EventingBasicConsumer InstanciarEventingBasicConsumer()
        {
            return new EventingBasicConsumer(_channel);
        }
    }
}