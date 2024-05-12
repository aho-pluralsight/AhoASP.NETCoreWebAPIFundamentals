using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Ch03.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _fileNamePattern = "Ch03.Aho.CityInfo.API.";
        private readonly string _fileLocation = "content\\download";
        private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{id}")]
        public ActionResult GetFile(int id)
        {
            var file = GetLocalFile(id);
            if (string.IsNullOrEmpty(file))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(file, out var fileContentType))
            {
                //Default Content Type if no type was found
                fileContentType = "applicaiton/octet-stream";
            }

            var fileBytes = System.IO.File.ReadAllBytes(file);
            return File(fileBytes, fileContentType, file);
        }

        private string? GetLocalFile(int id)
        {
            return Directory.GetFiles(_fileLocation).FirstOrDefault(fi => Path.GetFileName(fi).StartsWith($"{_fileNamePattern}{id.ToString("00")}"));
        }
    }
}
