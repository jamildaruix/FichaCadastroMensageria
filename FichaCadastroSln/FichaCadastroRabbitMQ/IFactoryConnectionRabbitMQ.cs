using RabbitMQ.Client;

namespace FichaCadastroRabbitMQ
{
    public interface IFactoryConnectionRabbitMQ
    {
        IModel CreateConnection(string virtualHost);
    }
}