using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain.Internal
{
    public static class QueryableExtensions
    {
        public static TSource ExclusiveOrDefault<TSource>(this IQueryable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var results = source.Take(2).ToArray();
            return results.Length == 1 ? results[0] : default(TSource);
        }

        public static TSource ExclusiveOrDefault<TSource>(this IQueryable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            var results = source.Where(predicate).Take(2).ToArray();
            return results.Length == 1 ? results[0] : default(TSource);
        }
    }
}
