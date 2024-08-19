using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.RabbitMQService;
using Services;
using System.Net;
using Task8.Models;
using Task8.Models.RabbitMq;
using Task8.Repository.IRepository;

namespace Task8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : Controller
    {
        private readonly IRabbitMQProducerService _RabbitMQService;
        private readonly IMapper _mapper;
        private readonly APIResponse response;

        public RabbitMQController(IRabbitMQProducerService RabbitMQService, IMapper mapper)
        {
            _RabbitMQService = RabbitMQService;
            _mapper = mapper;
            this.response = new APIResponse();
        }


        [HttpGet("publisher")]
        public IActionResult ProduceData()
        {
            List<ProducerRabbitMQ> messagesProduced = _RabbitMQService.ProduceMessages();
            List<RabbitMq> messages = new List<RabbitMq>();

            foreach(ProducerRabbitMQ messageProduced in messagesProduced)
            {
                RabbitMq message = _mapper.Map<RabbitMq>(messageProduced);
                messages.Add(message);
            }

            //_context.AddMessagesAsync(messages);

            response.Result = messages.ToList();
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);

        }

    }
}
