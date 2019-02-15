using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.Paging
{
    public static class PagedList
    {
        public static PageModel<U> ToPageList<U,T>(this IQueryable<T> list,int index =1, int pageSize =5)
        {
            int total = list.Count();
            list = list.Skip((index - 1) * pageSize).Take(pageSize);
            List<T> data = list.ToList();
            List<U> result = new List<U>();
            foreach (var item in data)
            {
                result.Add(item.Adapt<U>());
            }
            int left = Math.Max(index - 2, 1);
            int right = Math.Min(left + 4, (int)Math.Ceiling(1.0 * total / pageSize));
            left = Math.Max(1, Math.Min(right - 4, left));

            return new PageModel<U>
            {
                Index = index,
                Left =left,
                Right = right,
                List = result,
                Total = result.Count
            };
        }
    }
}
