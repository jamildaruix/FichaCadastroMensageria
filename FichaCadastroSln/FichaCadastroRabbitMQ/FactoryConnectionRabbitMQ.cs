using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace FichaCadastroRabbitMQ
{
    public class FactoryConnectionRabbitMQ : IFactoryConnectionRabbitMQ
    {
        private IConnection connection;
        private readonly ConnectionFactory _connectionFactory;
        private IModel _channel;

        public FactoryConnectionRabbitMQ()
        {
            _connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };
        }

        public IModel CreateConnection(string virtualHost)
        {
            if (connection == null || connection!.IsOpen == false && _connectionFactory.VirtualHost == virtualHost)
            {
                _connectionFactory.VirtualHost = virtualHost;
                connection = _connectionFactory.CreateConnection();
                _channel = connection.CreateModel();
            }

            return _channel;
        }

    }
}