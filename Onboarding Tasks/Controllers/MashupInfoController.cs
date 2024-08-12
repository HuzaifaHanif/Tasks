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
        public IActionResult GetData()
        {
            var mashupsInfo =  DataHandler.GetMashupInfo(_config.GetConnectionString("DefaultConnectionVidizmo"));

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
