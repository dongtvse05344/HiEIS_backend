using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class InvoiceItemCM
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Total { get; set; }
    }
}
