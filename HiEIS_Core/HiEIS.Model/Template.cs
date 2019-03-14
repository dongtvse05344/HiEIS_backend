using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Template : BaseEntity
    {
        public string Name { get; set; }
        public string Serial { get; set; }
        public Guid CompanyId { get; set; }
        public string FileUrl { get; set; }
        public long Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
        public string ReleaseAnnouncementUrl { get; set; }
        public int Llx { get; set; }
        public int Lly { get; set; }
        public int Urx { get; set; }
        public int Ury { get; set; }

        public string Form { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
