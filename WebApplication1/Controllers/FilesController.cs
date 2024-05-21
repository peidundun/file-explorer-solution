using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class FilesController : ControllerBase
    {
        private FileService _fileService;

        private DB.XDbContext _dbContext;

        private ILogger<FilesController> _logger;

        public FilesController(FileService fileService, DB.XDbContext dbContext, ILogger<FilesController> logger)
        {
            _fileService = fileService;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("GetFiles")]
        public IActionResult GetFiles()
        {
            var files = _dbContext.GeneralFiles
                .ToList();

            var resData = new
            {
                code = 0,
                data = files.Select(file => new
                {
                    file.Id,
                    file.Name,
                    IconUrl = _fileService.GetFileThumbnailFileUrl(file.ThumbnailFilePath),
                    file.UploadDT,
                    file.Size,
                    Url = _fileService.GetFileOriginalFileUrl(file.Path),
                    file.OriginalName
                })
            };

            return Ok(resData);
        }

        [HttpPost("UploadFile")]
        public IActionResult UploadFile()
        {
            _logger.LogInformation($"num files:{Request.Form.Files.Count}");
            foreach (var item in Request.Form.Files)
            {
                if (item.Length == 0)
                    continue;

                var ext = System.IO.Path.GetExtension(item.FileName);
                var fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), ext);
                //var path = string.Format("{0:D06}/{1}{2}", stumis.Id, Guid.NewGuid().ToString(), ext);
                var path = _fileService.GetFileFullPath(fileName);
                var thumbnailFilePath = _fileService.GetFileFullPath_thumb(fileName);
                using (var stream = item.OpenReadStream())
                {
                    //_aliyun_oss.PutObject(path, stream);
                    //_fs.SaveStudentActFile(t.Id, fileName, stream);
                    //_fs.SaveStudentActFile_thumb(t.Id, fileName, stream);
                    _fileService.SaveFile(stream, path, thumbnailFilePath);
                }

                var file = new DB.Entities.GeneralFile();
                file.Name = fileName;
                file.OriginalName = item.FileName;
                file.Path = path;
                file.ThumbnailFilePath = thumbnailFilePath;
                file.Size = (int)item.Length;
                file.Type = Utils.FilePathUtil.ResolveFileType(item.FileName);
                file.MimeType = item.ContentType;
                file.Extension = ext;
                file.UploadDT = DateTime.Now;
                _dbContext.GeneralFiles.Add(file);
            }
            _dbContext.SaveChanges();

            var resData = new
            {
                code = 0
            };
            return Ok(resData);
        }
    }
}
