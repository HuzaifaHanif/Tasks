using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Nest;
using ServiceContracts;
using System.Net;
using Task8.Models;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchController : Controller
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly APIResponse response;
        private readonly IMapper _mapper;

        public ElasticSearchController(IElasticSearchService elasticSearchService , IMapper mapper)
        {
            _elasticSearchService = elasticSearchService;
            _mapper = mapper;
            this.response = new APIResponse();  
        }

        [HttpPost("AddUsers")]
        public IActionResult AddUsers()
        {
            User user = new User();
            int age = 20;

            for (int i = 0; i < 100 ; i++)
            {
                user.Id = Guid.NewGuid();
                age = age > 60 ? 20 : age;
                user.Age = age++;
                user.Name = $"Huzaifa {i}";
                user.Street = $"street {i + 1} city Karachi";
                user.message = $"message from {user.Name} , my age is {user.Age} , i live in {user.Street}";

                string result = _elasticSearchService.IndexUser(user);

                if(!String.IsNullOrEmpty(result))
                {
                    response.Result = result;
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(response);
                }
            }

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);

        }

        [HttpGet("GetUser/{text}")]
        public async Task<IActionResult> GetUsers([FromRoute] string text) 
        {
            var users = await _elasticSearchService.GetUser(text);

            if(users.Count == 0)
            {
                response.Result = users;
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(response);
            }

            response.Result = users;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);

        }

    }
}
