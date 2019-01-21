using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.Service.FileStorage.Infrastructure
{
    public interface IStorage
    {
        Task<MemoryStream> GetFile(string name);
    }
}
