using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class IndexOfEqual<T, TSource, TValue> : SpanTest<T, TSource, TValue>
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

            int idx = MemoryExt.IndexOfEqual(span, default(TValue));
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(span, default(TValue), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, default(TValue), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, default(TValue));
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(rspan, default(TValue), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, default(TValue), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(span, NextTValue());
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(span, NextTValue(), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, NextTValue(), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, NextTValue());
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(rspan, NextTValue(), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, NextTValue(), EqualityCompareVS);
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

            int idx = MemoryExt.IndexOfEqual(span, default(TValue));
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqual(span, default(TValue), EqualityCompareSV);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, default(TValue), EqualityCompareVS);
            Assert.Equal(0, idx);

            idx = MemoryExt.IndexOfEqual(rspan, default(TValue));
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqual(rspan, default(TValue), EqualityCompareSV);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, default(TValue), EqualityCompareVS);
            Assert.Equal(0, idx);

            idx = MemoryExt.IndexOfEqual(span, NextTValue());
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(span, NextTValue(), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, NextTValue(), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, NextTValue());
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(rspan, NextTValue(), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, NextTValue(), EqualityCompareVS);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(42 * (length + 1));
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(target);

                int idx = MemoryExt.IndexOfEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqual(span, NewTValue(target), EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualFrom(span, NewTValue(target), EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target), EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualFrom(rspan, NewTValue(target), EqualityCompareVS);
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
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

                s[i] = NewTSource(item);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            int idx = MemoryExt.IndexOfEqual(span, NewTValue(target));
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(span, NewTValue(target), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, NewTValue(target), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target));
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, NewTValue(target), EqualityCompareVS);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(44 * (length + 1));
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
            {
                TSource temp0 = s[targetIndex + 0];
                TSource temp1 = s[targetIndex + 1];
                s[targetIndex + 0] = s[targetIndex + 1] = NewTSource(target);

                int idx = MemoryExt.IndexOfEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqual(span, NewTValue(target), EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualFrom(span, NewTValue(target), EqualityCompareVS);
                Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target), EqualityCompareSV);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.IndexOfEqualFrom(rspan, NewTValue(target), EqualityCompareVS);
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
            handle = OnCompareActions<T>.CreateHandler(null);
            TLog<T> log = new TLog<T>(handle);

            var rnd = new Random(45 * (length + 1));
            T target = NewT(rnd.Next());

            T[] t = new T[length];
            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

                t[i] = item;
                s[i] = NewTSource(item, handle);
            }

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for IndexOfEqual to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(s.Length, log.Count);
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x) || EqualityCompareT(x, item)).Count();
                    int numCompares = log.CountCompares(item, target);
                    Assert.True(numCompares == itemCount, $"Expected {itemCount} == {numCompares} for element {item}.");
                }
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            bool logSupported = IsLogSupported();

            log.Clear();
            int idx = MemoryExt.IndexOfEqual(span, NewTValue(target, handle));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target, handle));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            //if (!logSupported)
            //    OnCompare += log.Add;

            log.Clear();
            idx = MemoryExt.IndexOfEqual(span, NewTValue(target, handle), EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualFrom(span, NewTValue(target, handle), EqualityCompareVS);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target, handle), EqualityCompareSV);
            Assert.Equal(-1, idx);
            CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfEqualFrom(rspan, NewTValue(target, handle), EqualityCompareVS);
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
            T target = NewT(rnd.Next());
            const int guardLength = 50;

            T guard;
            do
            {
                guard = NewT(rnd.Next());
            } while (EqualityCompareT(guard, target) || EqualityCompareT(target, guard));

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard) || EqualityCompareT(guard, x) ||
                    EqualityCompareT(y, guard) || EqualityCompareT(guard, y))
                    throw new Exception("Detected out of range access in IndexOfEqual()");
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
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item) ||
                    EqualityCompareT(item, guard) || EqualityCompareT(guard, item));

                s[i + guardLength] = NewTSource(item, handle);
            }
            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);

            int idx = MemoryExt.IndexOfEqual(span, NewTValue(target, handle));
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target, handle));
            Assert.Equal(-1, idx);

            //OnCompare += checkForOutOfRangeAccess;

            idx = MemoryExt.IndexOfEqual(span, NewTValue(target, handle), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(span, NewTValue(target, handle), EqualityCompareVS);
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfEqual(rspan, NewTValue(target, handle), EqualityCompareSV);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualFrom(rspan, NewTValue(target, handle), EqualityCompareVS);
            Assert.Equal(-1, idx);

            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }
    }

    public sealed class IndexOfEqual_byte : IndexOfEqual<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, int handle = 0) => value;
        protected override byte NewTValue(byte value, int handle = 0) => value;
    }

    public sealed class IndexOfEqual_char : IndexOfEqual<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, int handle = 0) => value;
        protected override char NewTValue(char value, int handle = 0) => value;
    }

    public sealed class IndexOfEqual_int : IndexOfEqual<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, int handle = 0) => value;
        protected override int NewTValue(int value, int handle = 0) => value;
    }

    public sealed class IndexOfEqual_string : IndexOfEqual<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, int handle = 0) => value;
        protected override string NewTValue(string value, int handle = 0) => value;
    }

    public sealed class IndexOfEqual_intEE : IndexOfEqual<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class IndexOfEqual_intEO : IndexOfEqual<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = -1) => new TObject<int>(value, -1);
    }

    public sealed class IndexOfEqual_intOE : IndexOfEqual<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = -1) => new TObject<int>(value, -1);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class IndexOfEqual_intOO : IndexOfEqual<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = 0) => new TObject<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = 0) => new TObject<int>(value, handle);
    }

    public sealed class IndexOfEqual_stringEE : IndexOfEqual<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class IndexOfEqual_stringEO : IndexOfEqual<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = -1) => new TObject<string>(value, -1);
    }

    public sealed class IndexOfEqual_stringOE : IndexOfEqual<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = -1) => new TObject<string>(value, -1);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
    }

    public sealed class IndexOfEqual_stringOO : IndexOfEqual<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = 0) => 
            new TObject<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = 0) =>
            new TObject<string>(value, handle);
    }
}
