﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IAzureFileUploadService
    {
        Task FileUploadOnAzure(string inputFilePath , string chunkSize , string azureBlobConnection , string blobContainerName );
    }
}
