using AutoMapper;
using Azure;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using System.Net;
using Task8.Models;
using Task8.Models.Employees;
using Task8.Models.Kafka;
using Task8.Repository;
using Task8.Repository.IRepository;

namespace Task8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : Controller
    {
        private readonly IKafkaService _kafkaService;
        private readonly IMapper _mapper;
        private readonly IKafkaRepository _context;
        private readonly APIResponse response;
        public KafkaController(IKafkaService kafkaService , IMapper mapper , IKafkaRepository context) 
        {
            _kafkaService = kafkaService;
            _mapper = mapper;
            _context = context;
            this.response = new APIResponse();
        }

        [HttpGet("publisher")]
        public async Task<IActionResult> ProduceData()
        {
            List<ProducerKafka> results = await _kafkaService.ProduceMessages();
            List<Kafka> messages = new List<Kafka>();

            foreach (ProducerKafka result in results)
            {
                Kafka message = _mapper.Map<Kafka>(result);
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
        //    response.Result =  await _context.GetAllMessagesAsync();
        //    response.IsSuccess = true;
        //    response.StatusCode = HttpStatusCode.OK;

        //    return Ok(response);

        //}
    }
}
