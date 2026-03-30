using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Ch06.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/files")]
    [Route("api/v{version:apiVersion}/files")]
    [Authorize]
    [ApiController]
    [ApiVersion(1.1, Deprecated = true)]
    public class FilesController : ControllerBase
    {
        private readonly string _fileNamePrefix = "Ch03.Aho.CityInfo.API.";
        private readonly string _download = @"content\download";
        private readonly string _upload = @"content\upload";
        private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        #region API Methods
        /// <summary>
        /// Returns a file with the Id passed in the params
        /// </summary>
        /// <param name="id">File Id</param>
        /// <returns>A file</returns>
        /// <response code="200">Returns the requested file</response>
        /// <response code="404">Pas aujourd'hui !</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="file">The file to load</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="400">Bad request yo !</response>
        /// <response code="404">Pas aujourd'hui !</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("Missing or invalid file.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), _upload, $"Ch04.Aho.CityInfo.API.{DateTime.Now.ToString("yyyyMMdd.hhmmss.fff")}.pdf");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok($"File uploaded and saved successfully to: {Path.Combine(Directory.GetCurrentDirectory(), _upload)}");
        }
        #endregion

        #region Private Methods
        private string? GetLocalFile(int id)
        {
            return Directory.GetFiles(_download).FirstOrDefault(fi => Path.GetFileName(fi).StartsWith($"{_fileNamePrefix}{id.ToString("00")}"));
        }
        #endregion
    }
}
