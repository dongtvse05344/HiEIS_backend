using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HiEIS.Model
{
    public class Template : BaseEntity
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string Serial { get; set; }
        public Guid CompanyId { get; set; }
        public string FileUrl { get; set; }
        public int Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseAnnouncementUrl { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
