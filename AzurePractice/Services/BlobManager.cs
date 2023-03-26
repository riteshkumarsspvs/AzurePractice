using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice.Services
{
    public class BlobManager : IBlobManager
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobManager(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public string GetBlob()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("ritscontainer");
            var blobItems = blobContainerClient.GetBlobs().ToList() ;
            
            var blobclient = blobContainerClient.GetBlobClient("Blob Test.txt");
            var response = blobclient.DownloadContent();

            var blobDownloadResult = response.Value;
            return blobDownloadResult.Content.ToString();
        }
    }
}
