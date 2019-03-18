using System;
using System.Collections.Generic;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static ref readonly TSource MaxRef<TSource>(this Span<TSource> span)
        {
            Comparer<TSource> comparer = Comparer<TSource>.Default;
            ref readonly TSource MaxIns(in TSource v1, in TSource v2)
            {
                if (v2 == null)
                    return ref v1;
                if (v1 == null)
                    return ref v2;
                if (comparer.Compare(v1, v2) >= 0)
                    return ref v1;
                return ref v2;
            }

            return ref span.Aggregate(MaxIns);
        }
    }
}
