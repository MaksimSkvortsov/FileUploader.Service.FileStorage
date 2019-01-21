namespace FileUploader.Service.FileStorage.Data
{
    public class File
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public long FileSize { get; set; }

        public bool IsValid { get; set; }
    }
}
