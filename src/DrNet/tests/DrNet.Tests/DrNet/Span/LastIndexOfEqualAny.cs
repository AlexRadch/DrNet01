using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class LastIndexOfEqualAny<T, TSource, TValue> : SpanTest<T, TSource, TValue>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);
            TValue NextTValue() => NewTValue(NewT(rnd.Next(1, int.MaxValue)));

            Span<TSource> span = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())) }.AsReadOnlySpan(2, 0);

            ReadOnlySpan<TValue> values = new TValue[] { NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);
            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { default, default, default, default });
            idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), NextTValue() });
            idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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
                if (!EqualityCompareSV(default, default))
                    return;
                if (!EqualityCompareVS(default, default))
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

            ReadOnlySpan<TValue> values = new TValue[] { NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);
            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), default });
            idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextTValue(), NextTValue(), NextTValue(), NextTValue() });
            idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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

                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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

                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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

            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new TValue[] { NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);
            idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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

            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
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

            for (int targetIndex = 1; targetIndex < length; targetIndex++)
            {
                TSource temp0 = s[targetIndex - 0];
                TSource temp1 = s[targetIndex - 1];
                s[targetIndex - 0] = NewTSource(targets[rnd.Next(0, targets.Length)]);
                s[targetIndex - 1] = NewTSource(targets[rnd.Next(0, targets.Length)]);

                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                s[targetIndex - 0] = temp0;
                s[targetIndex - 1] = temp1;
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void OnNoMatchMakeSureEveryElementIsCompared(int length)
        {
            handle = OnCompareActions<T>.CreateHandler(null);
            TLog<T> log = new TLog<T>(handle);

            var rnd = new Random(45 * (length + 1));
            T[] targets = new T[] { NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()), NewT(rnd.Next()) };

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
                s[i] = NewTSource(item, handle);
            }

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for LastIndexOfEqualAny to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(s.Length * targets.Length, log.Count);
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x) || EqualityCompareT(x, item)).Count();
                    foreach (T target in targets)
                    {
                        int targetCount = targets.
                            Where(x => EqualityCompareT(target, x) || EqualityCompareT(x, target)).Count();

                        int count = itemCount * targetCount;
                        int numCompares = log.CountCompares(item, target);
                        Assert.True(numCompares == itemCount,
                            $"Expected {count} == {numCompares} for element ({item}, {target}).");
                    }
                }
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, handle)).ToArray().AsReadOnlySpan();

            bool logSupported = IsLogSupported();

            log.Clear();
            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            //if (!logSupported)
            //    OnCompare += log.Add;

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Dispose();
            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void MakeSureNoChecksGoOutOfRange(int length)
        {
            handle = OnCompareActions<T>.CreateHandler(null);

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
                    throw new Exception("Detected out of range access in LastIndexOfEqualAny()");
            }
            OnCompareActions<T>.Add(handle, checkForOutOfRangeAccess);

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard, handle));
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (targets.AsSpan().IndexOfEqual(item) >= 0 ||
                    EqualityCompareT(item, guard) || EqualityCompareT(guard, item));

                s[i + guardLength] = NewTSource(item, handle);
            }
            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, handle)).ToArray().
                AsReadOnlySpan();

            int idx = MemoryExt.LastIndexOfEqualAny(span, values);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values);
            Assert.Equal(-1, idx);

            //OnCompare += checkForOutOfRangeAccess;

            idx = MemoryExt.LastIndexOfEqualAny(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.LastIndexOfEqualAny(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualAnyFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }
    }

    public sealed class LastIndexOfEqualAny_byte : LastIndexOfEqualAny<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, int handle = 0) => value;
        protected override byte NewTValue(byte value, int handle = 0) => value;
    }

    public sealed class LastIndexOfEqualAny_char : LastIndexOfEqualAny<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, int handle = 0) => value;
        protected override char NewTValue(char value, int handle = 0) => value;
    }

    public sealed class LastIndexOfEqualAny_int : LastIndexOfEqualAny<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, int handle = 0) => value;
        protected override int NewTValue(int value, int handle = 0) => value;
    }

    public sealed class LastIndexOfEqualAny_string : LastIndexOfEqualAny<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, int handle = 0) => value;
        protected override string NewTValue(string value, int handle = 0) => value;
    }

    public sealed class LastIndexOfEqualAny_intEE : LastIndexOfEqualAny<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class LastIndexOfEqualAny_intEO : LastIndexOfEqualAny<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = -1) => new TObject<int>(value, -1);
    }

    public sealed class LastIndexOfEqualAny_intOE : LastIndexOfEqualAny<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = -1) => new TObject<int>(value, -1);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class LastIndexOfEqualAny_intOO : LastIndexOfEqualAny<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = 0) => new TObject<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = 0) => new TObject<int>(value, handle);
    }

    public sealed class LastIndexOfEqualAny_stringEE : LastIndexOfEqualAny<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class LastIndexOfEqualAny_stringEO : LastIndexOfEqualAny<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = -1) => new TObject<string>(value, -1);
    }

    public sealed class LastIndexOfEqualAny_stringOE : LastIndexOfEqualAny<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = -1) => new TObject<string>(value, -1);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class LastIndexOfEqualAny_stringOO : LastIndexOfEqualAny<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = 0) =>
            new TObject<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = 0) =>
            new TObject<string>(value, handle);
    }
}
