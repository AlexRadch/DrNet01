using System;
using System.Collections.Generic;
using System.Linq;

namespace DrNet.Linq
{
    public static class DrNetEnumerable
    {
        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
            => source.Select((item, index) => (item, index));


        public static IEnumerable<TResult> Repeat<TResult>(TResult value)
        {
            for (; ; )
                yield return value;
        }

        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> generator)
        {
            for (; ; )
                yield return generator();
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult seed, Func<TResult, TResult> generator,
            bool withSeed = false)
        {
            TResult value = seed;
            if (withSeed)
                yield return value;
            for (; ; )
            {
                value = generator(value);
                yield return value;
            }
        }

        public static IEnumerable<TResult> Repeat<TAccumulate, TResult>(TAccumulate seed, 
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, bool withSeed = false)
        {
            TAccumulate value = seed;
            if (withSeed)
                yield return resultSelector(value);
            for (; ; )
            {
                value = generator(value);
                yield return resultSelector(value);
            }
        }
    }
}
