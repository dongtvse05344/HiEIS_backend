using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class DataTableResponseModel<T>
        where T : class
    {
        public DataTableResponseModel()
        {
            data = new List<T>();
        }
        public List<T> data { get; set; }
        public int total { get; set; }
        public int display { get; set; }
    }
}
