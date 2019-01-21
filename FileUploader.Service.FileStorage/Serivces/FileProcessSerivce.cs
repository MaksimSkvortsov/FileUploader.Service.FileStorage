using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FileUploader.Service.FileStorage.Data;
using FileUploader.Service.FileStorage.FileProcess;
using FileUploader.Service.FileStorage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploader.Service.FileStorage.Serivces
{
    public class FileProcessSerivce : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IStorage _storage;
        private readonly IServiceBus _serviceBus;
        private readonly IServiceScopeFactory _scopeFactory;

        public FileProcessSerivce(ILogger<FileProcessSerivce> logger, IServiceBus sericeBus, IStorage storage, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _storage = storage;
            _serviceBus = sericeBus;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceBus.RegisterHandler(ProcessMessage);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        private async Task ProcessMessage(string message, CancellationToken token)
        {
            _logger.LogInformation("ProcessMessage. File received " + message);

            FileUploadInfo uploadInfo = null;
            try
            {
                uploadInfo = JsonConvert.DeserializeObject<FileUploadInfo>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProcessMessage. Deserialization failed for file " + message);
            }

            File fileDto = await GetFileInfo(message, uploadInfo);

            _logger.LogInformation("ProcessMessage. File parsed " + message);

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<FileStorageContext>();
                context.Files.Add(fileDto);
                await context.SaveChangesAsync();
            }

            _logger.LogInformation("ProcessMessage. File info saved to database " + message);
        }

        private async Task<File> GetFileInfo(string message, FileUploadInfo uploadInfo)
        {
            var fileDto = new File();
            fileDto.Path = uploadInfo.FilePath;

            System.IO.MemoryStream readStream = null;
            try
            {
                readStream = await _storage.GetFile(uploadInfo.FilePath);

                fileDto.FileSize = readStream.Length;
                fileDto.IsValid = FileIsDicom(readStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProcessMessage. Deserialization failed for file " + message);
            }
            finally
            {
                readStream.Close();
            }

            return fileDto;
        }

        private bool FileIsDicom(System.IO.MemoryStream fileStream)
        {
            byte[] byteStream = new byte[4];
            fileStream.Read(byteStream, 0, 4);

            var validator = Convert.ToChar(byteStream[0]).ToString() + Convert.ToChar(byteStream[1]) + Convert.ToChar(byteStream[2]) + Convert.ToChar(byteStream[3]);

            return validator == "DICM";
        }
    }
}
