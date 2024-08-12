using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
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
        private readonly IRabbitMQService _RabbitMQService;
        private readonly IMapper _mapper;
        private readonly IRabbitMQRepository _context;
        private readonly APIResponse response;
        public RabbitMQController(IRabbitMQService RabbitMQService, IMapper mapper, IRabbitMQRepository context)
        {
            _RabbitMQService = RabbitMQService;
            _mapper = mapper;
            _context = context;
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

        //[HttpGet("publisher/messages")]
        //public async Task<IActionResult> GetMessages()
        //{
        //    response.Result = await _context.GetAllMessagesAsync();
        //    response.IsSuccess = true;
        //    response.StatusCode = HttpStatusCode.OK;

        //    return Ok(response);

        //}
    }
}
