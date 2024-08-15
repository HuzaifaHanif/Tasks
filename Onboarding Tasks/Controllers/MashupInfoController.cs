using Microsoft.AspNetCore.Mvc;
using System.Net;
using Task8.Models;
using Tasks.Data;
using Tasks.Models.VidizmoContract;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MashupInfoController : Controller
    {
        private readonly IConfiguration _config;

        private APIResponse response;

        public MashupInfoController(IConfiguration config)
        {
            _config = config;
            this.response = new APIResponse();
        }

        [HttpGet("Mashupinfo")]
        public IActionResult GetData([FromQuery] long? userId,
                                    [FromQuery] long? mashupId,
                                    [FromQuery] long? tenantId,
                                    [FromQuery] DateTime? publishedDate,
                                    [FromQuery] string? culture,
                                    [FromQuery] string? title,
                                    [FromQuery] string? description,
                                    [FromQuery] string? tags,
                                    [FromQuery] string? category,
                                    [FromQuery] bool? isTranscoded,
                                    [FromQuery] bool? isAIProcessed)
        {

            var filterObj = new RequestMashupInfo
            {
                UserId = userId,
                MashupId = mashupId,
                TenantId = tenantId,
                PublishedDate = publishedDate,
                Culture = culture,
                Title = title,
                Description = description,
                Tags = tags,
                Category = category,
                IsTranscoded = isTranscoded,
                IsAIProcessed = isAIProcessed
            };
            var mashupsInfo =  DataHandler.GetMashupInfo(_config.GetConnectionString("DefaultConnectionVidizmo") , filterObj);

            if (mashupsInfo == null)
            {
                response.Result = mashupsInfo;
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(response);
            }

            response.Result = mashupsInfo;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }
}
