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

        #region Repeat

        public static IEnumerable<TResult> Repeat<TResult>(TResult value)
        {
            for (;;)
                yield return value;
        }

        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> generator)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            for (;;)
                yield return generator();
        }

        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> generator, int count)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Enumerable.Empty<TResult>();

            return DrNetInternalEnumerable.Repeat(generator, count);
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult seed, Func<TResult, TResult> generator, 
            bool fromSeed = false)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            TResult value = seed;
            if (fromSeed)
                yield return value;
            for (;;)
            {
                value = generator(value);
                yield return value;
            }
        }

        public static IEnumerable<TResult> Repeat<TResult>(TResult seed, Func<TResult, TResult> generator, int count,
            bool withSeed = false)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Enumerable.Empty<TResult>();

            return withSeed ? DrNetInternalEnumerable.RepeatWithSeed(seed, generator, count) :
                DrNetInternalEnumerable.RepeatWithoutSeed(seed, generator, count);
        }

        public static IEnumerable<TResult> Repeat<TAccumulate, TResult>(TAccumulate seed, 
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, bool withSeed = false)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            TAccumulate value = seed;
            if (withSeed)
                yield return resultSelector(value);
            for (;;)
            {
                value = generator(value);
                yield return resultSelector(value);
            }
        }

        public static IEnumerable<TResult> Repeat<TAccumulate, TResult>(TAccumulate seed,
            Func<TAccumulate, TAccumulate> generator, Func<TAccumulate, TResult> resultSelector, int count, 
            bool withSeed = false)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Enumerable.Empty<TResult>();

            return withSeed ? DrNetInternalEnumerable.RepeatWithSeed(seed, generator, resultSelector, count) :
                DrNetInternalEnumerable.RepeatWithoutSeed(seed, generator, resultSelector, count);
        }

        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            for (;;)
            {
                bool empty = true;
                foreach (TSource item in source)
                {
                    yield return item;
                    empty = false;
                }
                if (empty)
                    throw new InvalidOperationException();
            }
        }

        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Enumerable.Empty<TSource>();

            return DrNetInternalEnumerable.Repeat(source, count);
        }

        #endregion

        public static IEnumerable<(TSource Item, int Index)> WithIndex<TSource>(this IEnumerable<TSource> source)
            => source.Select((item, index) => (item, index));
    }
}
