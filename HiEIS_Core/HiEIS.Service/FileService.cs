using IronPdf;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HiEIS.Service
{
    public class FileUtils
    {
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".aac", "audio/aac"},
                {".abw", "application/x-abiword"},
                {".arc", "application/x-freearc"},
                {".avi", "video/x-msvideo"},
                {".azw", "application/vnd.amazon.ebook"},
                {".bin", "application/octet-stream"},
                {".bmp", "image/bmp"},
                {".bz", "application/x-bzip"},
                {".bz2", "application/x-bzip2"},
                {".csh", "application/x-csh"},
                {".css", "text/css"},
                {".csv", "text/csv"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".eot", "application/vnd.ms-fontobject"},
                {".epub", "application/epub+zip"},
                {".gif", "image/gif"},
                {".html", "text/html"},
                {".htm", "text/html"},
                {".ico", "image/vnd.microsoft.icon"},
                {".ics", "text/calendar"},
                {".jar", "application/java-archive"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "text/javascript"},
                {".json", "application/json"},
                {".mid", "audio/x-midi"},
                {".midi", "audio/x-midi"},
                {".mjs", "text/javascript"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpkg", "application/vnd.apple.installer+xml"},
                {".odp", "application/vnd.oasis.opendocument.presentation"},
                {".ods", "application/vnd.oasis.opendocument.spreadsheet"},
                {".odt", "application/vnd.oasis.opendocument.text"},
                {".oga", "audio/ogg"},
                {".ogv", "video/ogg"},
                {".ogx", "application/ogg"},
                {".otf", "font/otf"},
                {".png", "image/png"},
                {".pdf", "application/pdf"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".rar", "application/x-rar-compressed"},
                {".rtf", "application/rtf"},
                {".sh", "application/x-sh"},
                {".svg", "image/svg+xml"},
                {".swf", "application/x-shockwave-flash"},
                {".tar", "application/x-tar"},
                {".tif","image/tiff"},
                {" tiff", "image/tiff"},
                {".ttf", "font/ttf"},
                {".txt", "text/plain"},
                {".vsd", "application/vnd.visio"},
                {".wav", "audio/wav"},
                {".weba", "audio/webm"},
                {".webm", "video/webm"},
                {".webp", "image/webp"},
                {".woff", "font/woff"},
                {".woff2", "font/woff2"},
                {".xhtml", "application/xhtml+xml"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".xml", "application/xml "},
                {".zip", "application/zip"},
                {".3gp", "video/3gpp"},
                {".3g2", "video/3gpp2"},
                {".7z", "application/x-7z-compressed"}
            };
        }
    }

    public interface IFileService
    {
        void CreateFolder(string companyName);
        string GenerateFileName(string fileName);
        Task<string> SaveFile(string companyId, string type, IFormFile file);
        Task<string> SaveFile(string companyId, string type, IFormFile file,string fileName);

        string SaveFile(string companyId, string type, List<PdfDocument> pdfDocuments, int currentNo);
        Task<FileSupport> GetFile(string url);
        void DeleteFile(string url);
    }
    public class FileService : IFileService
    {
        public void CreateFolder(string companyId)
        {
            // Template
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Files", companyId);
            var pathTemplate = Path.Combine(Directory.GetCurrentDirectory(), "Files", companyId, "Template");
            System.IO.Directory.CreateDirectory(pathTemplate);
            // Invoice
            var pathInvoice = Path.Combine(Directory.GetCurrentDirectory(), "Files", companyId, "Invoice");
            System.IO.Directory.CreateDirectory(pathInvoice);

        }

        public void DeleteFile(string url)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), url);
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string GenerateFileName(string fileName)
        {
            string result = fileName.Split('.')[0];
            string extension = Path.GetExtension(fileName);
            string currentDate = DateTime.Now.ToString("dMyyyyhmmss");
            result += "_" + currentDate + extension;
            return result;
        }

        public async Task<FileSupport> GetFile(string url)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), url);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return new FileSupport
            {
                Stream = memory,
                FileName = Path.GetFileName(url),
                ContentType = GetContentType(path)
            };
        }

        public async Task<string> SaveFile(string companyId, string type, IFormFile file)
        {
            string filename = GenerateFileName(file.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                "Files", companyId, type,
                                filename);
            try
            {
                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
                return "Files\\" + companyId + "\\" + type + "\\" + filename;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SaveFile(string companyId, string type, List<PdfDocument> pdfDocuments, int currentNo)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("dMyyyyhmmss");
                string filename = currentNo + "_" + currentDate + ".pdf";
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                                    "Files", companyId, type,
                                    filename);
                PdfDocument.Merge(pdfDocuments).SaveAs(path);

                return "Files\\" + companyId + "\\" + type + "\\" + filename;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> SaveFile(string companyId, string type, IFormFile file, string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                "Files", companyId, type,
                                fileName);
            try
            {
                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
                return "Files\\" + companyId + "\\" + type + "\\" + fileName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
