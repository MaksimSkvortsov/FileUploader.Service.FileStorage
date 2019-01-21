using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileUploader.Service.FileStorage.Data;
using FileUploader.Service.FileStorage.Model;

namespace FileUploader.Service.FileStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private FileStorageContext _context;

        public DataController(FileStorageContext context)
        {
            _context = context;
        }


        // GET api/data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileModel>>> Get()
        {
            return await _context.Files.Select(s => new FileModel(s)).ToListAsync();
        }
    }
}
