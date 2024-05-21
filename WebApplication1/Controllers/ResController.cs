using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    public class ResController : ControllerBase
    {
        private FileService _fs;

        private ILogger<ResController> _logger;

        public ResController(FileService fs, ILogger<ResController> logger)
        {
            _fs = fs;
            _logger = logger;
        }

        [HttpGet("UserFile/{**filePath}")]
        public IActionResult UserFile(string filePath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return BadRequest();

            var fileRootPath = _fs.GetFileLocalRootPath(filePath);
            if (!System.IO.File.Exists(fileRootPath))
                return NotFound();

            var fileExtension = System.IO.Path.GetExtension(filePath);

            var fileData = System.IO.File.ReadAllBytes(fileRootPath);
            var fileMimeType = Utils.MimeMapping.GetMimeMapping(fileRootPath);
            _logger.LogInformation($"{fileExtension}:{fileMimeType}");
            //var fileContentType = Services.
            return File(fileData, fileMimeType, fileName);
        }
    }
}
