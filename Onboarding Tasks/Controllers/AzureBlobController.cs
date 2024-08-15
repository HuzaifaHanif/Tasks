using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using Task8.Models;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureBlobController : Controller
    {
        private readonly IAzureFileUploadService _uploadService;
        private readonly IConfiguration _configuration;
        APIResponse response;


        public AzureBlobController(IConfiguration configuration , IAzureFileUploadService uploadService)
        {
            _uploadService = uploadService;
            _configuration = configuration;
            this.response = new APIResponse();
        }

        [HttpGet("UploadFile/{inputFilePath}/{chunkSize}")]
        public async Task<IActionResult> UploadFile([FromRoute] string inputFilePath , [FromRoute]  string chunkSize)
        {
            if (!System.IO.File.Exists(inputFilePath))
            {
                response.Result = "No file found in the given path";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;

                return NotFound(response);
            }

            await _uploadService.FileUploadOnAzure(
                    inputFilePath , 
                    chunkSize , 
                    _configuration.GetConnectionString("AzureBlobConnection") , 
                    _configuration.GetConnectionString("BlobContainerName")
                );

            response.Result = "File uploaded successfully";
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);

        }
    }
}

//C:\Users\Huzaifa.hanif\Desktop\Tasks\Task 6and7\video1_854x480.m3u8