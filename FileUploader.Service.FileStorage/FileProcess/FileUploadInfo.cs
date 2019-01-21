using System;

namespace FileUploader.Service.FileStorage.FileProcess
{
    public class FileUploadInfo
    {
        public string TransactionId { get; set; }

        public string FilePath { get; set; }

        public DateTime ReceiveDate { get; set; }
    }
}
