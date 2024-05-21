using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Utils
{
    public class FilePathUtil
    {
        public static string ResolveFileType(string fileName)
        {
            var type = "";
            var fileExt = System.IO.Path.GetExtension(fileName);
            if (fileExt != null)
            {
                fileExt = fileExt.ToLower();
                if (fileExt.EndsWith("jpg") || fileExt.EndsWith("jpeg") || fileExt.EndsWith("png") || fileExt.EndsWith("bmp"))
                {
                    type = "image";
                }
                else if (fileExt.EndsWith("heic"))
                {
                    type = "image";
                }
                else if (fileExt.EndsWith("mp4"))
                {
                    type = "video";
                }
                else if (fileExt.EndsWith("pdf"))
                {
                    type = "pdf";
                }
                else if (fileExt.EndsWith("doc") || fileExt.EndsWith("docx"))
                {
                    type = "word";
                }
                else if (fileExt.EndsWith("xls") || fileExt.EndsWith("xlsx"))
                {
                    type = "excel";
                }
                else
                {
                    if (fileExt.StartsWith("."))
                    {
                        type = fileExt.Substring(1);
                    }
                    else
                    {
                        type = fileExt;
                    }
                }
            }
            return type;
        }
    }
}
