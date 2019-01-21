using Microsoft.EntityFrameworkCore;

namespace FileUploader.Service.FileStorage.Data
{
    public class FileStorageContext : DbContext
    {
        public FileStorageContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }
        
        public DbSet<File> Files { get; set; }
    }
}
