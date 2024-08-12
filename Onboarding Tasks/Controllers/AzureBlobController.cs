using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
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
        APIResponse response;
        private readonly IConfiguration _configuration;

        public AzureBlobController(IConfiguration configuration)
        {
            this.response = new APIResponse();
            _configuration = configuration;
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

            string fileNameWithExtension = System.IO.Path.GetFileName(inputFilePath);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("AzureBlobConnection"));
            BlobContainerClient containerClient =  blobServiceClient.GetBlobContainerClient(_configuration.GetConnectionString("BlobContainerName"));

            string[] chunks = chunkSize.Split('*');
            int size = 1;
            for (int i = 0; i < chunks.Length; i++)
            {
                size *= Convert.ToInt32(chunks[i]);   
            }

            
            BlockBlobClient blobClient = containerClient.GetBlockBlobClient(fileNameWithExtension);
            await using FileStream uploadFileStream = System.IO.File.OpenRead(inputFilePath);

            ArrayList blockIDArrayList = new ArrayList();
            byte[] buffer;
            var bytesLeft = uploadFileStream.Length - uploadFileStream.Position;

            while (bytesLeft > 0)
            {
                if (bytesLeft >= size)
                {
                    buffer = new byte[size];
                    await uploadFileStream.ReadAsync(buffer , 0 , size);
                }

                else
                {
                    buffer = new byte[bytesLeft];
                    await uploadFileStream.ReadAsync(buffer , 0 , Convert.ToInt32(bytesLeft));
                    bytesLeft = uploadFileStream.Length - uploadFileStream.Position;
                }

                using (var stream = new MemoryStream(buffer))
                {
                    string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                    blockIDArrayList.Add(blockID);
                    BlockInfo uploadedBlock = await blobClient.StageBlockAsync(blockID, stream);
                    Console.WriteLine($"Block Uploade: \n\n{uploadedBlock}");
                }

                bytesLeft = uploadFileStream.Length - uploadFileStream.Position;
            }

            string[] blockIDArray = (string[])blockIDArrayList.ToArray(typeof(string));

            await blobClient.CommitBlockListAsync(blockIDArray);

            response.Result = "File uploaded successfully";
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);

        }
    }
}

//C:\Users\Huzaifa.hanif\Desktop\Tasks\Task 6and7\video1_854x480.m3u8