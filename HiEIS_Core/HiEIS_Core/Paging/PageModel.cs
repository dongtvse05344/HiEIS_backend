using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Paging
{
    public class PageModel<T> 
    {
        public ICollection<T> List { get; set; }

        public int Index { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Total { get; set; }
    }
}
