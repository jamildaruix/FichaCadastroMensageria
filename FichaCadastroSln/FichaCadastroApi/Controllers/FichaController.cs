using FichaCadastroApi.DTO.Ficha;
using FichaCadastroRabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace FichaCadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichaController : ControllerBase
    {

        private readonly IMessageRabbitMQ messageRabbitMQ;

        public FichaController(IMessageRabbitMQ messageRabbitMQ)
        {
            this.messageRabbitMQ = messageRabbitMQ;
        }

        [HttpPost]
        [Route("cadastro/novo/topic")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CadastroNovoTopicPost([FromBody] FichaCreateDTO fichaCreateDTO)
        {
            try
            {
                string routingKey = "ficha-cadastro.novo-routeKey-topic";
                string queue = "ficha-cadastro-novo-queue-topic";

                fichaCreateDTO.Mensagem = "ENVIAR PARA CONSUMER COM A ROUTE FICHA-CADASTRO-NOVO-ROUTEKEY-TOPIC";

                messageRabbitMQ.ConfigureVirtualHost("ficha");

                // Serializa o objeto em JSON
                string jsonFicha = JsonSerializer.Serialize(fichaCreateDTO);

                messageRabbitMQ.ExchangeDeclare(exchange: "ficha-exchange-topic", "topic");

                messageRabbitMQ.QueueDeclare(queue);

                messageRabbitMQ.QueueBind(queue, "ficha-exchange-topic", routingKey);


                messageRabbitMQ.BasicPublish(
                    exchange: "ficha-exchange-topic",
                    routingKey: routingKey,
                    body: Encoding.UTF8.GetBytes(jsonFicha));

                return StatusCode(HttpStatusCode.Created.GetHashCode());
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        [HttpPost]
        [Route("cadastro/deletar/topic")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CadastroDeletarTopicPost([FromBody] FichaCreateDTO fichaCreateDTO)
        {
            try
            {
                string routingKey = "ficha-cadastro.deletar-routeKey-topic";
                string queue = "ficha-deletar-queue-topic";

                fichaCreateDTO.Mensagem = "ENVIAR PARA CONSUMER COM A ROUTE FICHA-CADASTRO-DELETAR-ROUTEKEY-TOPIC";

                messageRabbitMQ.ConfigureVirtualHost("ficha");

                // Serializa o objeto em JSON
                string jsonFicha = JsonSerializer.Serialize(fichaCreateDTO);

                messageRabbitMQ.ExchangeDeclare(exchange: "ficha-exchange-topic", "topic");
                messageRabbitMQ.QueueDeclare(queue);
                messageRabbitMQ.QueueBind(queue, "ficha-exchange-topic", routingKey);

                messageRabbitMQ.BasicPublish(
                    exchange: "ficha-exchange-topic",
                    routingKey: routingKey,
                    body: Encoding.UTF8.GetBytes(jsonFicha));

                return StatusCode(HttpStatusCode.Created.GetHashCode());
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }

        [HttpPost]
        [Route("cadastro/enviar/email/topic")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CadastroEnviarEmailTopicPost([FromBody] FichaCreateDTO fichaCreateDTO)
        {
            try
            {
                string routingKey = "ficha-cadastro.enviar-email-routeKey-topic";
                string queue = "ficha-cadastro-enviar-email-queue-topic";

                fichaCreateDTO.Mensagem = "ENVIAR PARA CONSUMER COM A ROUTE FICHA-CADASTRO-ENVIAR-EMAIL-ROUTEKEY-TOPIC";

                messageRabbitMQ.ConfigureVirtualHost("ficha");

                // Serializa o objeto em JSON
                string jsonFicha = JsonSerializer.Serialize(fichaCreateDTO);

                messageRabbitMQ.ExchangeDeclare(exchange: "ficha-exchange-topic", "topic");

                messageRabbitMQ.QueueDeclare(queue);

                messageRabbitMQ.QueueBind(queue, "ficha-exchange-topic", routingKey);

                messageRabbitMQ.BasicPublish(
                    exchange: "ficha-exchange-topic",
                    routingKey: routingKey,
                    body: Encoding.UTF8.GetBytes(jsonFicha));

                return StatusCode(HttpStatusCode.Created.GetHashCode());
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }
        }


        [HttpPost]
        [Route("cadastro/direct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult FichaCadastroDirectPost([FromBody] FichaCreateDTO fichaCreateDTO)
        {
            try
            {
                while (true)
                {
                    messageRabbitMQ.ConfigureVirtualHost("ficha");

                    // Serializa o objeto em JSON
                    string jsonFicha = JsonSerializer.Serialize(fichaCreateDTO);

                    messageRabbitMQ.ExchangeDeclare(exchange: "ficha-exchange-direct", "direct");

                    messageRabbitMQ.QueueDeclare("ficha-queue-direct");

                    messageRabbitMQ.BasicPublish(
                        exchange: "ficha-exchange-direct",
                        routingKey: "ficha-cadastro-routeKey-direct",
                        body: Encoding.UTF8.GetBytes(jsonFicha));
                }

                return StatusCode(HttpStatusCode.Created.GetHashCode());
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex);
            }

        }


    }

}
