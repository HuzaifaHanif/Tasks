﻿using Azure;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.AzureBusService;
using System.Net;
using Task8.Models;
using Tasks.Models.AzureBusService;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureBusServiceController : Controller
    {
        IAzureBusProducerService _azureBusService;
        IConfiguration _configuration;
        APIResponse response;

        public AzureBusServiceController(IAzureBusProducerService azureBusService , IConfiguration configuration)
        {
            _azureBusService = azureBusService;
            response = new APIResponse();
            _configuration = configuration;
        }

        [HttpGet("PublishMessages")]
        public async Task<IActionResult> ProduceData()
        {
            response.Result = await _azureBusService.ProduceMessages(
                _configuration.GetConnectionString("AzureBusServiceConnection"), 
                _configuration.GetConnectionString("BusServiceTopic"));

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);

        }
    }
}
