using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class CurrentSignVM
    {
        public Guid CompanyId { get; set; }
        public List<FileContent> fileContents { get; set; }
        public string Type { get; set; }
    }
    public class FileContent
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public int Llx { get; set; }
        public int Lly { get; set; }
        public int Urx { get; set; }
        public int Ury { get; set; }
    }
}
