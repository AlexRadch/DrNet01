using System.Collections.Generic;
using System.Linq;

namespace DrNet.Linq
{
    public static class DrNetEnumerable
    {
        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
            => source.Select((item, index) => (item, index));
    }
}
