using System;
using System.Linq;

using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class StartsWithSeq<T, TSource, TValue> : SpanTest<T, TSource, TValue>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);

            Span<TSource> span = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsReadOnlySpan(2, 0);
            ReadOnlySpan<TValue> values = new TValue[] { NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);

            bool b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);

            values = default;

            b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);

            span = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsSpan(1, 1);
            rspan = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsReadOnlySpan(2, 1);
            values = new TValue[] { NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);

            b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);

            values = default;

            b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);

            span = default;
            rspan = default;
            values = new TValue[] { NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);

            b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);

            values = default;

            b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void SameSpan(int length)
        {
            var rnd = new Random(41 * (length + 1));

            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                s[i] = NewTSource(NewT(rnd.Next()));
            }
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TSource> values = new ReadOnlySpan<TSource>(s);

            bool b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            //b = MemoryExt.StartsWithSeq(span, values, EqualityCompareS);
            //Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            //b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareS);
            //Assert.True(b);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void LengthMismatch(int length)
        {
            var rnd = new Random(44 * (length + 1));

            TSource[] s = new TSource[length + 1];
            TValue[] v = new TValue[length + 1];
            for (int i = 0; i < length + 1; i++)
            {
                T item = NewT(rnd.Next());
                s[i] = NewTSource(item);
                v[i] = NewTValue(item);
            }

            Span<TSource> span = new Span<TSource>(s, 0, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, 0, length);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v, 0, length + 1);

            bool c = MemoryExt.StartsWithSeq(span, values);
            Assert.False(c);
            c = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.False(c);
            c = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.False(c);

            c = MemoryExt.StartsWithSeq(rspan, values);
            Assert.False(c);
            c = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.False(c);
            c = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.False(c);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void OnEqualSpansMakeSureEveryElementIsCompared(int length)
        {
            var rnd = new Random(45 * (length + 1));
            TLog<T> log = new TLog<T>();

            T[] t = new T[length];
            TSource[] s = new TSource[length + 1];
            TValue[] v = new TValue[length];
            s[length] = NewTSource(NewT(rnd.Next()), log.Add);
            for (int i = 0; i < length; i++)
            {
                t[i] = NewT(rnd.Next());
                s[i] = NewTSource(t[i], log.Add);
                v[i] = NewTValue(t[i], log.Add);
            }

            // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
            // EqualToSeq to compare an element more than once but that would be a non-optimal implementation and 
            // a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(length, log.Count);
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x) || EqualityCompareT(x, item)).Count();
                    int numCompares = log.CountCompares(item, item);
                    Assert.True(itemCount == numCompares, $"Expected {itemCount} == {numCompares} for element {item}.");
                }
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v);

            {
                EqualityCompareSV(NewTSource(NewT(1), log.Add), NewTValue(NewT(1), log.Add));
                EqualityCompareVS(NewTValue(NewT(1), log.Add), NewTSource(NewT(1), log.Add));
            }
            bool logSupported = log.Count == 2;
            if (!logSupported)
            {
                bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) && typeof(TSource) == typeof(TEquatable<T>);
                bool valueWithLog = typeof(TValue) == typeof(TObject<T>) && typeof(TValue) == typeof(TEquatable<T>);
                Assert.False(sourceWithLog && valueWithLog);
            }

            log.Clear();
            bool b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            if (logSupported)
                CheckCompares();

            log.Clear();
            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            if (logSupported)
                CheckCompares();

            if (!logSupported)
                OnCompare += log.Add;

            log.Clear();
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            CheckCompares();

            log.Clear();
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            CheckCompares();

            log.Clear();
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);
            CheckCompares();

            log.Clear();
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);
            CheckCompares();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatch(int length)
        {
            var rnd = new Random(46 * (length + 1));
            T target = NewT(rnd.Next());
            TLog<T> log = new TLog<T>();

            TSource[] s = new TSource[length + 1];
            TValue[] v = new TValue[length];
            s[length] = NewTSource(target, log.Add);
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

                s[i] = NewTSource(item, log.Add);
                v[i] = NewTValue(item, log.Add);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v);

            {
                EqualityCompareSV(NewTSource(NewT(1), log.Add), NewTValue(NewT(1), log.Add));
                EqualityCompareVS(NewTValue(NewT(1), log.Add), NewTSource(NewT(1), log.Add));
            }
            bool logSupported = log.Count == 2;
            if (!logSupported)
            {
                bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) && typeof(TSource) == typeof(TEquatable<T>);
                bool valueWithLog = typeof(TValue) == typeof(TObject<T>) && typeof(TValue) == typeof(TEquatable<T>);
                Assert.False(sourceWithLog && valueWithLog);
            }

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource tempS = s[targetIndex];
                s[targetIndex] = NewTSource(target, log.Add);

                log.Clear();
                bool b = MemoryExt.StartsWithSeq(span, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeq(rspan, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                s[targetIndex] = tempS;
                TValue tempV = v[targetIndex];
                v[targetIndex] = NewTValue(target, log.Add);

                log.Clear();
                b = MemoryExt.StartsWithSeq(span, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeq(rspan, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                v[targetIndex] = tempV;
            }

            if (!logSupported)
                OnCompare += log.Add;

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource tempS = s[targetIndex];
                s[targetIndex] = NewTSource(target, log.Add);

                log.Clear();
                bool b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                s[targetIndex] = tempS;
                TValue tempV = v[targetIndex];
                v[targetIndex] = NewTValue(target, log.Add);

                log.Clear();
                b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                v[targetIndex] = tempV;
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void MakeSureNoChecksGoOutOfRange(int length)
        {
            var rnd = new Random(47 * (length + 1));
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
                    throw new Exception("Detected out of range access in StartsWithSeq()");
            }

            TSource[] s = new TSource[guardLength + length + guardLength];
            TValue[] v = new TValue[guardLength + length + guardLength];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = NewTSource(guard, checkForOutOfRangeAccess);
                v[i] = NewTValue(guard, checkForOutOfRangeAccess);
            }

            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item) ||
                    EqualityCompareT(item, guard) || EqualityCompareT(guard, item));

                s[guardLength + i] = NewTSource(item, checkForOutOfRangeAccess);
                v[guardLength + i] = NewTValue(item, checkForOutOfRangeAccess);
            }

            Span<TSource> span = new Span<TSource>(s, guardLength, guardLength + length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, guardLength + length);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v, guardLength, length);

            OnCompare += checkForOutOfRangeAccess;

            bool b = MemoryExt.StartsWithSeq(span, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(span, values, EqualityCompareVS);
            Assert.True(b);

            b = MemoryExt.StartsWithSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.StartsWithSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            b = MemoryExt.StartsWithSeqFrom(rspan, values, EqualityCompareVS);
            Assert.True(b);
        }
    }

    public sealed class StartsWithSeq_byte : StartsWithSeq<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public sealed class StartsWithSeq_char : StartsWithSeq<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public sealed class StartsWithSeq_int : StartsWithSeq<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, Action<int, int> onCompare) => value;
        protected override int NewTValue(int value, Action<int, int> onCompare) => value;
    }

    public sealed class StartsWithSeq_string : StartsWithSeq<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, Action<string, string> onCompare) => value;
        protected override string NewTValue(string value, Action<string, string> onCompare) => value;
    }

    public sealed class StartsWithSeq_intEE : StartsWithSeq<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public sealed class StartsWithSeq_intEO : StartsWithSeq<int, TEquatable<int>, TObject<int>>
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

    public sealed class StartsWithSeq_intOE : StartsWithSeq<int, TObject<int>, TEquatable<int>>
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

    public sealed class StartsWithSeq_intOO : StartsWithSeq<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public sealed class StartsWithSeq_stringEE : StartsWithSeq<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public sealed class StartsWithSeq_stringEO : StartsWithSeq<string, TEquatable<string>, TObject<string>>
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

    public sealed class StartsWithSeq_stringOE : StartsWithSeq<string, TObject<string>, TEquatable<string>>
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

    public sealed class StartsWithSeq_stringOO : StartsWithSeq<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
    }
}
