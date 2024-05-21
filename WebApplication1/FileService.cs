using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.IO;
using WebApplication1.Extensions.UrlHelper;

namespace WebApplication1
{
    public class FileService
    {
        private readonly IConfiguration _config;

        private string _RootDir;

        private IUrlHelper _urlHelper;

        public FileService(IConfiguration config, IUrlHelper urlHelper, ILogger<FileService> logger)
        {
            _config = config;
            _urlHelper = urlHelper;

            _RootDir = _config.GetValue<string>("FilesRootDir");
        }

        public string GetFileFullPath(string fileName)
        {
            return string.Format("general-files/{0}", fileName);
        }

        public string GetFileFullPath_thumb(string fileName)
        {
            return string.Format("general-files/thumb/{0}", fileName);
        }

        /// <summary>
        /// 获取文件在本地文件系统中的绝对路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFileLocalRootPath(string filePath)
        {
            return System.IO.Path.Combine(_RootDir, filePath);
        }

        public void ImageFile_GenThumb(System.IO.Stream stream, System.IO.Stream outStream, int w = 200, int h = 200)
        {
            var image = SixLabors.ImageSharp.Image.Load(stream);
            var imageThumb = Utils.ImageUtil.GenThumbImage(image, w, h);
            imageThumb.SaveAsPng(outStream);
        }

        public void SaveFile(System.IO.Stream fileDataStream, string originalFilePath, string thumbnailFilePath)
        {
            if (fileDataStream == null)
                throw new ArgumentException(nameof(fileDataStream));

            if (string.IsNullOrWhiteSpace(originalFilePath))
                throw new ArgumentException(nameof(originalFilePath));

            //_oss.PutObject(originalFilePath, fileDataStream);
            var originalFilePathRooted = GetFileLocalRootPath(originalFilePath);
            var originalFileDir = Path.GetDirectoryName(originalFilePathRooted);
            if (!Directory.Exists(originalFileDir))
            {
                Directory.CreateDirectory(originalFileDir);
            }
            using (var fs = System.IO.File.Create(originalFilePathRooted))
            {
                fileDataStream.Seek(0, System.IO.SeekOrigin.Begin);

                var buf = new byte[1024];
                var n = fileDataStream.Read(buf, 0, buf.Length);
                while (n > 0)
                {
                    fs.Write(buf, 0, n);

                    n = fileDataStream.Read(buf, 0, buf.Length);
                }
                fs.Flush();
            }

            var thumbnailFileType = Utils.FilePathUtil.ResolveFileType(originalFilePath);
            var thumbnailFileRootedPath = GetFileLocalRootPath(thumbnailFilePath);
            var thumbnailFileDir = System.IO.Path.GetDirectoryName(thumbnailFileRootedPath);
            if (!Directory.Exists(thumbnailFileDir))
            {
                Directory.CreateDirectory(thumbnailFileDir);
            }
            switch (thumbnailFileType)
            {
                case "image":
                    fileDataStream.Seek(0, System.IO.SeekOrigin.Begin);
                    using (var ms = new System.IO.MemoryStream())
                    {
                        ImageFile_GenThumb(fileDataStream, ms);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);

                        var buf = new byte[ms.Length];
                        ms.Read(buf, 0, buf.Length);
                        System.IO.File.WriteAllBytes(thumbnailFileRootedPath, buf);
                    }

                    break;
            }
        }

        public string GetFileOriginalFileUrl(string filePath)
        {
            return _urlHelper.Action("UserFile", "res", new { filePath });
        }

        public string GetFileOriginalFileUrl(string filePath, string fileName)
        {
            return _urlHelper.Action("UserFile", "res", new { filePath, fileName });
        }

        public string GetFileThumbnailFileUrl(string filePath)
        {
            var url = "";
            var fileType = Utils.FilePathUtil.ResolveFileType(filePath);
            switch (fileType)
            {
                case "image":
                case "video":
                    url = _urlHelper.Action("UserFile", "res", new { filePath });
                    break;
                default:
                    url = GetFileTypeIconUrl(fileType);
                    break;
            }
            url = _urlHelper.AbsoluteContent(url);
            //if (absolute)
            //{
            //    url = _urlHelper.AbsoluteContent(url);
            //}
            return url;
        }

        public static string GetFileTypeIconUrl(string fileType)
        {
            var url = string.Empty;
            switch (fileType)
            {
                case "image":
                case "png":
                case "jpg":
                case "jpeg":
                    //_logger.LogInformation(fileType);
                    //url = _aliyun_oss.GenerateSignedUrlForImageWithProcess("xijinghospital-waike-materiallib", file.Path, 100, 100);
                    break;
                case "pdf":
                    url = "/images/file-type/pdf.png";
                    break;
                case "excel":
                case "xls":
                case "xlsx":
                    url = "/images/file-type/xls.png";
                    break;
                case "word":
                case "doc":
                case "docx":
                    url = "/images/file-type/doc.png";
                    break;
                default:
                    break;
            }
            return url;
        }

    }
}
