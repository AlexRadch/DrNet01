using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DrNet.Linq
{
    public static class DrNetInternalEnumerable
    {
        #region Repeat

        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> generator, int count)
        {
            while (count-- > 0)
                yield return generator();
        }

        public static IEnumerable<TResult> RepeatWithSeed<TResult>(TResult seed, Func<TResult, TResult> generator,
            int count)
        {
            TResult value = seed;
            if (count-- > 0)
                yield return value;
            while (count-- > 0)
            {
                value = generator(value);
                yield return value;
            }
        }

        public static IEnumerable<TResult> RepeatWithoutSeed<TResult>(TResult seed, Func<TResult, TResult> generator, 
            int count)
        {
            TResult value = seed;
            while (count-- > 0)
            {
                value = generator(value);
                yield return value;
            }
        }

        public static IEnumerable<TResult> RepeatWithSeed<TAccumulate, TResult>(TAccumulate seed,
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, int count)
        {
            TAccumulate value = seed;
            if (count-- > 0)
                yield return resultSelector(value);
            while (count-- > 0)
            {
                value = generator(value);
                yield return resultSelector(value);
            }
        }

        public static IEnumerable<TResult> RepeatWithoutSeed<TAccumulate, TResult>(TAccumulate seed,
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, int count)
        {
            TAccumulate value = seed;
            while (count-- > 0)
            {
                value = generator(value);
                yield return resultSelector(value);
            }
        }

        public static IEnumerable<TSource> Repeat<TSource>(IEnumerable<TSource> source, int count)
        {
            Debug.Assert(count > 0);

            for (;;)
            {
                bool empty = true;
                foreach (TSource item in source)
                {
                    if (count-- > 0)
                        yield return item;
                    else
                        yield break;

                    empty = false;
                }
                if (empty)
                    throw new NotSupportedException();
            }
        }

        #endregion

    }
}
