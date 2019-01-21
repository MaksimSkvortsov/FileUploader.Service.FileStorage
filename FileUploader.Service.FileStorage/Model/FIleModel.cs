using FileUploader.Service.FileStorage.Data;

namespace FileUploader.Service.FileStorage.Model
{
    public class FileModel
    {
        public string Path { get; private set; }
        public long Size { get; private set; }
        public bool IsValid { get; private set; }


        public FileModel(File s)
        {
            Path = s.Path;
            Size = s.FileSize;
            IsValid = s.IsValid;
        }
    }
}
