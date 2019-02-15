using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HiEIS_Core.Utils
{
    public static class LinqUtils
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
            this IQueryable<TSource> source
            , Expression<Func<TSource, TKey>> keySelector
            , bool isAsc)
        {
            if (isAsc)
            {
                return source.OrderBy(keySelector);
            }
            else
            {
                return source.OrderByDescending(keySelector);
            }
        }
    }
}
