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
    public class FileContent
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public int Llx { get; set; }
        public int Lly { get; set; }
        public int Urx { get; set; }
        public int Ury { get; set; }
    }
}
