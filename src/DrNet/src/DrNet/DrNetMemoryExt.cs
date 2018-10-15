using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;

using DrNet.Internal.Unsafe;
using DrNet.Unsafe;

namespace DrNet                                                                                                         
{
    public static partial class DrNetMemoryExt
    {
        #region Parsing

        //#region Skip

        //public static int Skip<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> skipValues)
        //{

        //}

        //#endregion

        //#region Trim

        //public static Span<TSource> Trim<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> trimValues)
        //{

        //}

        //public static Span<TSource> Trim<TSource>(this Span<TSource> span, int start, int end)
        //{
        //    int length;
        //    if (start > 0)
        //    {
        //        if (end >= 0)
        //            length = end - start;
        //        else
        //            length = span.Length - start;
        //    }
        //    else
        //    {
        //        if (end < 0 || end == span.Length)
        //            return span;
        //        length = end;
        //    }
        //    return span.Slice(start, length);
        //}

        //#endregion

        #endregion
    }
}
