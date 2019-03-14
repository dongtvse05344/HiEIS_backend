using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class TemplateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string Serial { get; set; }
        public long Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class TemplateFilesVM
    {
        public string FileUrl { get; set; }
        public string ReleaseAnnouncementUrl { get; set; }
    }

    public class TemplateCM
    {
        public string Name { get; set; }
        public string Form { get; set; }
        public string Serial { get; set; }
        public long Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Llx { get; set; }
        public int Lly { get; set; }
        public int Urx { get; set; }
        public int Ury { get; set; }
    }

    public class TemplateUM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Form { get; set; }
        public string Serial { get; set; }
        public long Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public bool IsActive { get; set; }
        public int Llx { get; set; }
        public int Lly { get; set; }
        public int Urx { get; set; }
        public int Ury { get; set; }
    }
}
