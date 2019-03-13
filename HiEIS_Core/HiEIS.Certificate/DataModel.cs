using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiEIS.Certificate
{
    class DataModel
    {
        public Guid CompanyId { get; set; }
        public List<FileContent> fileContents { get; set; }
        public string Type { get; set; }
    }
    class FileContent
    {
        public string Id { get; set; }
        public string Path { get; set; }
    }
}
