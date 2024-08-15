using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using ServiceContracts;
using System.Collections;
using System.Text;

namespace Services
{
    public class AzureFileUploadService : IAzureFileUploadService
    {
        public async Task FileUploadOnAzure(string inputFilePath, string chunkSize, string azureBlobConnection, string blobContainerName)
        {
            string fileNameWithExtension = System.IO.Path.GetFileName(inputFilePath);

            BlobServiceClient blobServiceClient = new BlobServiceClient(azureBlobConnection);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

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
                    await uploadFileStream.ReadAsync(buffer, 0, size);
                }

                else
                {
                    buffer = new byte[bytesLeft];
                    await uploadFileStream.ReadAsync(buffer, 0, Convert.ToInt32(bytesLeft));
                    bytesLeft = uploadFileStream.Length - uploadFileStream.Position;
                }

                using (var stream = new MemoryStream(buffer))
                {
                    string blockID = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                    blockIDArrayList.Add(blockID);
                    BlockInfo uploadedBlock = await blobClient.StageBlockAsync(blockID, stream);
                    Console.WriteLine($"Block Uploaded: \n\n{uploadedBlock.ToString()}");
                }

                bytesLeft = uploadFileStream.Length - uploadFileStream.Position;
            }

            string[] blockIDArray = (string[])blockIDArrayList.ToArray(typeof(string));

            await blobClient.CommitBlockListAsync(blockIDArray);
        }
    }
}
