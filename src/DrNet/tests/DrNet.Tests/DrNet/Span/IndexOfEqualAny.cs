using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class IndexOfEqualAny<T, TSource, TValue>
    {
        protected abstract T NewT(int value);

        protected abstract TSource NewTSource(T value, Action<T, T> onCompare = default);

        protected abstract TValue NewTValue(T value, Action<T, T> onCompare = default);

        private Action<T, T> onCompare;

        private bool EqualityCompareT(T v1, T v2)
        {
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        private bool EqualityCompare(TSource sValue, TValue vValue)
        {
            if (onCompare != null && sValue is T tSource && vValue is T tValue)
                onCompare(tSource, tValue);

            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            return sValue.Equals(vValue);
        }

        private bool EqualityCompareFrom(TValue vValue, TSource sValue)
        {
            if (onCompare != null && vValue is T tValue && sValue is T tSource)
                onCompare(tValue, tSource);

            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            return vValue.Equals(sValue);
        }

        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);
            TValue NextTValue() => NewTValue(NewT(rnd.Next(1, int.MaxValue)));

            Span<TSource> span = new Span<TSource>(Array.Empty<TSource>());
            ReadOnlySpan<TSource> rspan = new Span<TSource>(Array.Empty<TSource>());

            ReadOnlySpan<TValue> values = default;
            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { default, default, default, default });
            idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), NextTValue() });
            idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void DefaultFilled(int length)
        {
            try
            {
                if (!EqualityCompare(default, default))
                    return;
                if (!EqualityCompareFrom(default, default))
                    return;
            }
            catch
            {
                return;
            }

            var rnd = new Random(41);
            TValue NextTValue() => NewTValue(NewT(rnd.Next(1, int.MaxValue)));

            TSource[] s = new TSource[length];
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            ReadOnlySpan<TValue> values = default;
            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), default });
            idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(0, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(0, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), NextTValue() });
            idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(42 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];

                int index = rnd.Next(0, targets.Length);
                s[targetIndex] = NewTSource(targets[index]);

                int idx = MemoryExt.IndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                s[targetIndex] = temp;
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatchValuesLarger(int length)
        {
            var rnd = new Random(47 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            TValue[] v = Enumerable.Range(1, length * 2).
                Select(i => targets[rnd.Next(1, targets.Length)]).
                Select(temp => NewTValue(temp)).
                ToArray();
            ReadOnlySpan<TValue> values = v;

            int targetIndex = length / 2;
            //for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource tempS = s[targetIndex];
                s[targetIndex] = NewTSource(targets[0]);

                int index = rnd.Next(length, length * 2);
                TValue tempV = v[index];
                v[index] = NewTValue(targets[0]);

                int idx = MemoryExt.IndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                s[targetIndex] = tempS;
                v[index] = tempV;
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatch(int length)
        {
            var rnd = new Random(43 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            values = default;

            idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatchValuesLarger(int length)
        {
            var rnd = new Random(48 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = Enumerable.Range(1, length * 2).
                Select(i => targets[rnd.Next(0, targets.Length)]).
                Select(temp => NewTValue(temp)).
                ToArray().AsReadOnlySpan();

            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(44 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
            {
                TSource temp0 = s[targetIndex + 0];
                TSource temp1 = s[targetIndex + 1];
                s[targetIndex + 0] = NewTSource(targets[rnd.Next(0, targets.Length)]);
                s[targetIndex + 1] = NewTSource(targets[rnd.Next(0, targets.Length)]);

                int idx = MemoryExt.IndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
                Assert.Equal(targetIndex, idx);

                s[targetIndex + 0] = temp0;
                s[targetIndex + 1] = temp1;
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void OnNoMatchMakeSureEveryElementIsCompared(int length)
        {
            var rnd = new Random(45 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };
            TLog<T> log = new TLog<T>();

            T[] t = new T[length];
            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0);

                t[i] = item;
                s[i] = NewTSource(item, log.Add);
            }

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for IndexOfEqualAny to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(s.Length * targets.Length, log.Count);
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x) || EqualityCompareT(x, item)).Count();
                    foreach(T target in targets)
                    {
                        int targetCount = targets.
                            Where(x => EqualityCompareT(target, x) || EqualityCompareT(x, target)).Count();

                        int count = itemCount * targetCount;
                        int numCompares = log.CountCompares(item, target);
                        Assert.True(numCompares == itemCount, 
                            $"Expected {numCompares} == {itemCount} for element {item}.");
                    }
                }
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, log.Add)).ToArray().AsReadOnlySpan();

            {
                EqualityCompare(NewTSource(NewT(1), log.Add), NewTValue(NewT(1), log.Add));
                EqualityCompareFrom(NewTValue(NewT(1), log.Add), NewTSource(NewT(1), log.Add));
            }
            bool logSupported = log.Count == 2;
            if (!logSupported)
            {
                bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) && typeof(TSource) == typeof(TEquatable<T>);
                bool valueWithLog = typeof(TValue) == typeof(TObject<T>) && typeof(TValue) == typeof(TEquatable<T>);
                Assert.False(sourceWithLog && valueWithLog);
            }


            log.Clear();
            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            if (!logSupported)
                onCompare = log.Add;

            log.Clear();
            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
            CheckCompares();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void MakeSureNoChecksGoOutOfRange(int length)
        {
            var rnd = new Random(46 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };
            const int guardLength = 50;

            T guard;
            do
            {
                guard = NewT(rnd.Next());
            } while (targets.AsSpan().IndexOfEqual(guard) >= 0);

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard) || EqualityCompareT(guard, x) ||
                    EqualityCompareT(y, guard) || EqualityCompareT(guard, y))
                    throw new Exception("Detected out of range access in IndexOfEqualAny()");
            }

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard, checkForOutOfRangeAccess));
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0 ||
                    EqualityCompareT(item, guard) || EqualityCompareT(guard, item));

                s[i + guardLength] = NewTSource(item, checkForOutOfRangeAccess);
            }
            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, checkForOutOfRangeAccess)).ToArray().
                AsReadOnlySpan();

            int idx = MemoryExt.IndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);

            onCompare = checkForOutOfRangeAccess;

            idx = MemoryExt.IndexOfEqualAny(span, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(span, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqualAny(rspan, values, EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyFrom(rspan, values, EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }
    }

    public class IndexOfEqualAny_byte : IndexOfEqualAny<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public class IndexOfEqualAny_char : IndexOfEqualAny<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public class IndexOfEqualAny_int : IndexOfEqualAny<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, Action<int, int> onCompare) => value;
        protected override int NewTValue(int value, Action<int, int> onCompare) => value;
    }

    public class IndexOfEqualAny_string : IndexOfEqualAny<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, Action<string, string> onCompare) => value;
        protected override string NewTValue(string value, Action<string, string> onCompare) => value;
    }

    public class IndexOfEqualAny_intEE : IndexOfEqualAny<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class IndexOfEqualAny_intEO : IndexOfEqualAny<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class IndexOfEqualAny_intOE : IndexOfEqualAny<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class IndexOfEqualAny_intOO : IndexOfEqualAny<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public class IndexOfEqualAny_stringEE : IndexOfEqualAny<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public class IndexOfEqualAny_stringEO : IndexOfEqualAny<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class IndexOfEqualAny_stringOE : IndexOfEqualAny<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public class IndexOfEqualAny_stringOO : IndexOfEqualAny<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
    }
}
