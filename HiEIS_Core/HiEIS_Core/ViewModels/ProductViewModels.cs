using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class ProductVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public float UnitPrice { get; set; }
        public float VATRate { get; set; }
    }
    public class ProductUM
    {
        public Guid Id { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public float VATRate { get; set; }
    }
    public class ProductCM
    {
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        [Required]
        public float UnitPrice { get; set; }
        [Required]
        public float VATRate { get; set; }
        public bool HasIndex { get; set; }
    }
}
