using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class LastIndexOfNotEqualAll<T, TSource, TValue> : SpanTest<T, TSource, TValue>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);

            Span<TSource> span = new TSource[] { NextS(rnd), NextS(rnd), NextS(rnd) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NextS(rnd), NextS(rnd), NextS(rnd) }.AsReadOnlySpan(2, 0);

            ReadOnlySpan<TValue> values = new TValue[] { NextNotDefaultV(rnd), NextNotDefaultV(rnd),
                NextNotDefaultV(rnd) }.AsReadOnlySpan(3, 0);
            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { default, default, default, default });
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextNotDefaultV(rnd), NextNotDefaultV(rnd),
                NextNotDefaultV(rnd), NextNotDefaultV(rnd) });
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
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

            TSource[] s = new TSource[length];
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            ReadOnlySpan<TValue> values = new TValue[] { NextNotDefaultV(rnd), NextNotDefaultV(rnd),
                NextNotDefaultV(rnd) }.AsReadOnlySpan(3, 0);
            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextNotDefaultV(rnd), NextNotDefaultV(rnd),
                NextNotDefaultV(rnd), default });
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { NextNotDefaultV(rnd), NextNotDefaultV(rnd),
                NextNotDefaultV(rnd), NextNotDefaultV(rnd) });
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(length - 1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(length - 1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(42 * (length + 1));
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(targets[rnd.Next(0, targets.Length)]);
            }

            T item;
            do
            {
                item = NextT(rnd);
            } while (targets.AsSpan().IndexOfEqual(item) >= 0);

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(item);

                int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
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
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(targets[rnd.Next(0, targets.Length)]);
            }

            T item;
            do
            {
                item = NextT(rnd);
            } while (targets.AsSpan().IndexOfEqual(item) >= 0);

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            TValue[] v = Enumerable.Range(1, length * 2).
                Select(i => targets[rnd.Next(0, targets.Length)]).
                Select(temp => NewTValue(temp)).
                ToArray();
            ReadOnlySpan<TValue> values = v;

            int targetIndex = length / 2;
            //for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(item);

                int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                s[targetIndex] = temp;
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
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(targets[rnd.Next(0, targets.Length)]);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatchValuesLarger(int length)
        {
            var rnd = new Random(48 * (length + 1));
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(targets[rnd.Next(0, targets.Length)]);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = Enumerable.Range(1, Math.Max(length * 2, targets.Length)).
                Select(i => targets[rnd.Next(0, targets.Length)]).
                Select(temp => NewTValue(temp)).
                ToArray().AsReadOnlySpan();

            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(44 * (length + 1));
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(targets[rnd.Next(0, targets.Length)]);
            }

            T item;
            do
            {
                item = NextT(rnd);
            } while (targets.AsSpan().IndexOfEqual(item) >= 0);

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp)).ToArray().AsReadOnlySpan();

            for (int targetIndex = 1; targetIndex < length; targetIndex++)
            {
                TSource temp0 = s[targetIndex - 0];
                TSource temp1 = s[targetIndex - 1];
                s[targetIndex - 0] = s[targetIndex - 1] = NewTSource(item);

                int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
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
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };

            T[] t = new T[length];
            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                t[i] = targets[rnd.Next(0, targets.Length)];
                s[i] = NewTSource(t[i], handle);
            }

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for LastIndexOfNotEqualAll to compare an element more than once
            // but that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                int countAll = 0;
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x, true) || EqualityCompareT(x, item, true)).
                        Count();
                    int itemIndex = targets.AsReadOnlySpan().IndexOfEqual(item);
                    foreach (var target in targets)
                    {
                        int targetCount = targets.Take(itemIndex < 0 ? targets.Length : itemIndex + 1).
                            Where(x => EqualityCompareT(target, x, true) || EqualityCompareT(x, target, true)).Count();

                        int count = itemCount * targetCount;
                        countAll += targetCount;

                        if (!EqualityCompareT(item, target, true))
                        {
                            int itemCount2 = t.Where(x => EqualityCompareT(target, x, true) || 
                                EqualityCompareT(x, target, true)).Count();
                            int itemIndex2 = targets.AsReadOnlySpan().IndexOfEqual(target);
                            int targetCount2 = targets.Take(itemIndex2 < 0 ? targets.Length : itemIndex2 + 1).
                                Where(x => EqualityCompareT(item, x, true) || EqualityCompareT(x, item, true)).Count();

                            count += itemCount2 * targetCount2;
                        }

                        int numCompares = log.CountCompares(item, target);
                        Assert.True(numCompares == count,
                            $"Expected {count} == {numCompares} for element ({item}, {target}).");
                    }
                }
                Assert.Equal(countAll, log.Count);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, handle)).ToArray().AsReadOnlySpan();

            bool logSupported = IsLogSupported();

            log.Clear();
            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            //if (!logSupported)
            //    OnCompare += log.Add;

            log.Clear();
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
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
            T[] targets = new T[] { NextT(rnd), NextT(rnd), NextT(rnd), NextT(rnd) };
            const int guardLength = 50;

            T guard;
            do
            {
                guard = NextT(rnd);
            } while (targets.AsSpan().IndexOfEqual(guard) >= 0);

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard, true) || EqualityCompareT(guard, x, true) ||
                    EqualityCompareT(y, guard, true) || EqualityCompareT(guard, y, true))
                    throw new Exception("Detected out of range access in LastIndexOfNotEqualAll()");
            }
            OnCompareActions<T>.Add(handle, checkForOutOfRangeAccess);

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard, handle));

            for (int i = 0; i < length; i++)
            {
                s[i + guardLength] = NewTSource(targets[rnd.Next(0, targets.Length)], handle);
            }

            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);
            ReadOnlySpan<TValue> values = targets.Select(temp => NewTValue(temp, handle)).ToArray().
                AsReadOnlySpan();

            int idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values);
            Assert.Equal(-1, idx);

            //OnCompare += checkForOutOfRangeAccess;

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(span, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(span, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.LastIndexOfNotEqualAll(rspan, values, EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = DrNetMemoryExt.LastIndexOfNotEqualAllFrom(rspan, values, EqualityCompareVS);
            Assert.Equal(-1, idx);

            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }
    }

    public sealed class LastIndexOfNotEqualAll_byte : LastIndexOfNotEqualAll<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, int handle = 0) => value;
        protected override byte NewTValue(byte value, int handle = 0) => value;
    }

    public sealed class LastIndexOfNotEqualAll_char : LastIndexOfNotEqualAll<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, int handle = 0) => value;
        protected override char NewTValue(char value, int handle = 0) => value;
    }

    public sealed class LastIndexOfNotEqualAll_int : LastIndexOfNotEqualAll<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, int handle = 0) => value;
        protected override int NewTValue(int value, int handle = 0) => value;
    }

    public sealed class LastIndexOfNotEqualAll_string : LastIndexOfNotEqualAll<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, int handle = 0) => value;
        protected override string NewTValue(string value, int handle = 0) => value;
    }

    public sealed class LastIndexOfNotEqualAll_intEE : LastIndexOfNotEqualAll<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class LastIndexOfNotEqualAll_intEO : LastIndexOfNotEqualAll<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = -1) => new TObject<int>(value, -1);
    }

    public sealed class LastIndexOfNotEqualAll_intOE : LastIndexOfNotEqualAll<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = -1) => new TObject<int>(value, -1);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class LastIndexOfNotEqualAll_intOO : LastIndexOfNotEqualAll<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = 0) => new TObject<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = 0) => new TObject<int>(value, handle);
    }

    public sealed class LastIndexOfNotEqualAll_stringEE : LastIndexOfNotEqualAll<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class LastIndexOfNotEqualAll_stringEO : LastIndexOfNotEqualAll<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = -1) => new TObject<string>(value, -1);
    }

    public sealed class LastIndexOfNotEqualAll_stringOE : LastIndexOfNotEqualAll<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = -1) => new TObject<string>(value, -1);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class LastIndexOfNotEqualAll_stringOO : LastIndexOfNotEqualAll<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = 0) =>
            new TObject<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = 0) =>
            new TObject<string>(value, handle);
    }
}
