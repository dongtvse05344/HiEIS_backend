using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Model
{
    public class SignHistories :  BaseEntity
    {
        public DateTime DateCreated { get; set; }
        public Guid StaffId { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
