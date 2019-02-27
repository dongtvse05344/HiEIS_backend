using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class TemplateCM
    {
        public string Name { get; set; }
        public string Form { get; set; }
        public string Serial { get; set; }
        public long Amount { get; set; }
        public int BeginNo { get; set; }
        public int CurrentNo { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
