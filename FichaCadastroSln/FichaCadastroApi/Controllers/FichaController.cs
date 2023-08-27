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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Post([FromBody] FichaCreateDTO fichaCreateDTO)
        {
            try
            {
                while (true)
                {
                    messageRabbitMQ.ConfigureVirtualHost("ficha");

                    // Serializa o objeto em JSON
                    string jsonFicha = JsonSerializer.Serialize(fichaCreateDTO);

                    messageRabbitMQ.ExchangeDeclare(exchange: "ficha-exchange", "topic");

                    messageRabbitMQ.QueueDeclare("ficha-queue");

                    messageRabbitMQ.BasicPublish(
                        exchange: "ficha-exchange",
                        routingKey: "ficha-cadastro-routeKey",
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
