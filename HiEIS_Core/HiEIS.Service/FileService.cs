using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HiEIS.Service
{
    public interface IFileService
    {
        void CreateFolder(string companyName);
        string GenerateFileName(string fileName);
        Task<string> SaveFile(string companyId, string type, IFormFile file);
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
