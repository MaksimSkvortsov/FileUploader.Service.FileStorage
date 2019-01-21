using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Service.FileStorage.Infrastructure
{
    public class AzureStorage : IStorage
    {
        private string _connectionString;


        public AzureStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<MemoryStream> GetFile(string name)
        {
            CloudBlobContainer container;
            CloudBlockBlob blob;

            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            var client = storageAccount.CreateCloudBlobClient();

            container = client.GetContainerReference("azure-fileupload-dev");
            var check = await container.ExistsAsync();

            blob = container.GetBlockBlobReference(name);
            check = await blob.ExistsAsync();

            if (blob == null)
                return null;

            var fileStream = new MemoryStream();
            await blob.DownloadToStreamAsync(fileStream);

            return fileStream;
        }
    }
}
