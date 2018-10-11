using System;
using System.Collections.Generic;
using System.Linq;

namespace DrNet.Linq
{
    public static class DrNetEnumerable
    {
        public static void CheckOperationAll(this (bool All, bool ToEnd) operationResult)
        {
            if (!operationResult.All)
                throw new InvalidOperationException();
        }

        public static void CheckOperationToEnd(this (bool All, bool ToEnd) operationResult)
        {
            if (!operationResult.ToEnd)
                throw new InvalidOperationException();
        }

        public static void CheckOperationAllToEnd(this (bool All, bool ToEnd) operationResult)
        {
            CheckOperationAll(operationResult);
            CheckOperationToEnd(operationResult);
        }

        public static (bool All, bool ToEnd) CopyTo<TSource>(this IEnumerable<TSource> source, Span<TSource> dest)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                int length = dest.Length;
                int index = 0;
                while (index < length)
                {
                    if (!e.MoveNext())
                        return (true, false);
                    dest[index++] = e.Current;
                }
                return (!e.MoveNext(), true);
            }
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult value)
        {
            for (;;)
                yield return value;
        }

        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> generator)
        {
            for (;;)
                yield return generator();
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult seed, Func<TResult, TResult> generator, 
            bool fromSeed = false)
        {
            TResult value = seed;
            if (fromSeed)
                yield return value;
            for (;;)
            {
                value = generator(value);
                yield return value;
            }
        }

        public static IEnumerable<TResult> Repeat<TAccumulate, TResult>(TAccumulate seed, 
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, bool fromSeed = false)
        {
            TAccumulate value = seed;
            if (fromSeed)
                yield return resultSelector(value);
            for (;;)
            {
                value = generator(value);
                yield return resultSelector(value);
            }
        }

        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
            => source.Select((item, index) => (item, index));
    }
}
