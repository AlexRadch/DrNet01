using System;
using System.Linq;

using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class EqualsToSeq<T, TSource, TValue> : SpanTest<T, TSource, TValue>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);
            T NextT() => NewT(rnd.Next());
            TSource NextTSource() => NewTSource(NextT());
            TValue NextTValue() => NewTValue(NextT());

            Span<TSource> span = new TSource[] { NextTSource(), NextTSource(), NextTSource() }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NextTSource(), NextTSource(), NextTSource() }.
                AsReadOnlySpan(2, 0);
            ReadOnlySpan<TValue> values = new TValue[] { NextTValue(), NextTValue(), NextTValue() }.
                AsReadOnlySpan(3, 0);

            bool b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(c);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(c);

            values = default;

            b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(c);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.True(c);

            span = default;
            rspan = default;
            values = new TValue[] { NextTValue(), NextTValue(), NextTValue() }.AsReadOnlySpan(3, 0);

            b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(c);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.True(c);

            values = default;

            b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(c);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            //c = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.True(c);
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

            bool b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeq(span, values, EqualityCompareS);
            //Assert.True(b);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareS);
            //Assert.True(b);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ArrayImplicit(int length)
        {
            var rnd = new Random(42 * (length + 1));

            TSource[] s = new TSource[length];
            TValue[] v = new TValue[length];
            for (int i = 0; i < length; i++)
            {
                T item = NewT(rnd.Next());
                s[i] = NewTSource(item);
                v[i] = NewTValue(item);
            }

            Span<TSource> span = new Span<TSource>(s, 0, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, 0, length);

            bool b = MemoryExt.EqualsToSeq<TSource, TValue>(span, v);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<TSource, TValue>(span, v, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom<TSource, TValue>(span, v, EqualityCompareFrom);
            //Assert.True(b);

            b = MemoryExt.EqualsToSeq<TSource, TValue>(rspan, v);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<TSource, TValue>(rspan, v, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom<TSource, TValue>(rspan, v, EqualityCompareFrom);
            //Assert.True(b);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ArraySegmentImplicit(int length)
        {
            var rnd = new Random(43 * (length + 1));

            TSource[] s = new TSource[length];
            TValue[] v = new TValue[length + 1];
            v[0] = NewTValue(NewT(rnd.Next()));
            for (int i = 0; i < length; i++)
            {
                T item = NewT(rnd.Next());
                s[i] = NewTSource(item);
                v[i + 1] = NewTValue(item);
            }

            Span<TSource> span = new Span<TSource>(s, 0, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, 0, length);
            var segment = new ArraySegment<TValue>(v, 1, length);

            bool b = MemoryExt.EqualsToSeq<TSource, TValue>(span, segment);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<TSource, TValue>(span, segment, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom<TSource, TValue>(span, segment, EqualityCompareFrom);
            //Assert.True(b);

            b = MemoryExt.EqualsToSeq<TSource, TValue>(rspan, segment);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<TSource, TValue>(rspan, segment, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom<TSource, TValue>(rspan, segment, EqualityCompareFrom);
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

            bool c = MemoryExt.EqualsToSeq(span, values);
            Assert.False(c);
            c = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.False(c);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.False(c);

            c = MemoryExt.EqualsToSeq(rspan, values);
            Assert.False(c);
            c = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.False(c);
            //c = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.False(c);

            span = new Span<TSource>(s, 0, length + 1);
            rspan = new ReadOnlySpan<TSource>(s, 0, length + 1);
            values = new ReadOnlySpan<TValue>(v, 0, length);

            c = MemoryExt.EqualsToSeq(span, values);
            Assert.False(c);
            c = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.False(c);
            //c = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.False(c);

            c = MemoryExt.EqualsToSeq(rspan, values);
            Assert.False(c);
            c = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.False(c);
            //c = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.False(c);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void OnEqualSpansMakeSureEveryElementIsCompared(int length)
        {
            handle = OnCompareActions<T>.CreateHandler(null);
            TLog<T> log = new TLog<T>(handle);

            var rnd = new Random(45 * (length + 1));

            T[] t = new T[length];
            TSource[] s = new TSource[length];
            TValue[] v = new TValue[length];
            for (int i = 0; i < length; i++)
            {
                t[i] = NewT(rnd.Next());
                s[i] = NewTSource(t[i], handle);
                v[i] = NewTValue(t[i], handle);
            }

            // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
            // EqualToSeq to compare an element more than once but that would be a non-optimal implementation and 
            // a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(length, log.Count);
                foreach (T item in t)
                {
                    int itemCount = t.Where(x => EqualityCompareT(item, x, true) || EqualityCompareT(x, item, true)).
                        Count();
                    int numCompares = log.CountCompares(item, item);
                    Assert.True(itemCount == numCompares, $"Expected {itemCount} == {numCompares} for element {item}.");
                }
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v);

            bool logSupported = IsLogSupported();

            log.Clear();
            bool b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            if (logSupported)
                CheckCompares();

            log.Clear();
            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            if (logSupported)
                CheckCompares();

            //if (!logSupported)
            //    OnCompare += log.Add;

            log.Clear();
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            CheckCompares();

            //log.Clear();
            //b = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(b);
            //CheckCompares();

            log.Clear();
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            CheckCompares();

            //log.Clear();
            //b = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.True(b);
            //CheckCompares();

            log.Dispose();
            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatch(int length)
        {
            handle = OnCompareActions<T>.CreateHandler(null);
            TLog<T> log = new TLog<T>(handle);

            var rnd = new Random(46 * (length + 1));
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            TValue[] v = new TValue[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target, true) || EqualityCompareT(target, item, true));

                s[i] = NewTSource(item, handle);
                v[i] = NewTValue(item, handle);
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v);

            bool logSupported = IsLogSupported();

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource tempS = s[targetIndex];
                s[targetIndex] = NewTSource(target, handle);

                log.Clear();
                bool b = MemoryExt.EqualsToSeq(span, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.EqualsToSeq(rspan, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                s[targetIndex] = tempS;
                TValue tempV = v[targetIndex];
                v[targetIndex] = NewTValue(target, handle);

                log.Clear();
                b = MemoryExt.EqualsToSeq(span, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.EqualsToSeq(rspan, values);
                Assert.False(b);
                if (logSupported)
                    Assert.Equal(targetIndex + 1, log.Count);

                v[targetIndex] = tempV;
            }

            //if (!logSupported)
            //    OnCompare += log.Add;

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource tempS = s[targetIndex];
                s[targetIndex] = NewTSource(target, handle);

                log.Clear();
                bool b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                //log.Clear();
                //b = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
                //Assert.False(b);
                //Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                //log.Clear();
                //b = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
                //Assert.False(b);
                //Assert.Equal(targetIndex + 1, log.Count);

                s[targetIndex] = tempS;
                TValue tempV = v[targetIndex];
                v[targetIndex] = NewTValue(target, handle);

                log.Clear();
                b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                //log.Clear();
                //b = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
                //Assert.False(b);
                //Assert.Equal(targetIndex + 1, log.Count);

                log.Clear();
                b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
                Assert.False(b);
                Assert.Equal(targetIndex + 1, log.Count);

                //log.Clear();
                //b = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
                //Assert.False(b);
                //Assert.Equal(targetIndex + 1, log.Count);

                v[targetIndex] = tempV;
            }

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

            var rnd = new Random(47 * (length + 1));
            T target = NewT(rnd.Next());
            const int guardLength = 50;

            T guard;
            do
            {
                guard = NewT(rnd.Next());
            } while (EqualityCompareT(guard, target, true) || EqualityCompareT(target, guard, true));

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard, true) || EqualityCompareT(guard, x, true) ||
                    EqualityCompareT(y, guard, true) || EqualityCompareT(guard, y, true))
                    throw new Exception("Detected out of range access in EqualsToSeq()");
            }
            OnCompareActions<T>.Add(handle, checkForOutOfRangeAccess);

            TSource[] s = new TSource[guardLength + length + guardLength];
            TValue[] v = new TValue[guardLength + length + guardLength];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = NewTSource(guard, handle);
                v[i] = NewTValue(guard, handle);
            }

            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target, true) || EqualityCompareT(target, item, true) ||
                    EqualityCompareT(item, guard, true) || EqualityCompareT(guard, item, true));

                s[guardLength + i] = NewTSource(item, handle);
                v[guardLength + i] = NewTValue(item, handle);
            }

            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);
            ReadOnlySpan<TValue> values = new ReadOnlySpan<TValue>(v, guardLength, length);

            //OnCompare += checkForOutOfRangeAccess;

            bool b = MemoryExt.EqualsToSeq(span, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(span, values, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom(span, values, EqualityCompareFrom);
            //Assert.True(b);

            b = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq(rspan, values, EqualityCompareSV);
            Assert.True(b);
            //b = MemoryExt.EqualsToSeqFrom(rspan, values, EqualityCompareFrom);
            //Assert.True(b);

            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }
    }

    public sealed class EqualsToSeq_byte : EqualsToSeq<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, int handle = 0) => value;
        protected override byte NewTValue(byte value, int handle = 0) => value;
    }

    public sealed class EqualsToSeq_char : EqualsToSeq<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, int handle = 0) => value;
        protected override char NewTValue(char value, int handle = 0) => value;
    }

    public sealed class EqualsToSeq_int : EqualsToSeq<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, int handle = 0) => value;
        protected override int NewTValue(int value, int handle = 0) => value;
    }

    public sealed class EqualsToSeq_string : EqualsToSeq<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, int handle = 0) => value;
        protected override string NewTValue(string value, int handle = 0) => value;
    }

    public sealed class EqualsToSeq_intEE : EqualsToSeq<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class EqualsToSeq_intEO : EqualsToSeq<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = -1) => new TObject<int>(value, -1);
    }

    public sealed class EqualsToSeq_intOE : EqualsToSeq<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = -1) => new TObject<int>(value, -1);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class EqualsToSeq_intOO : EqualsToSeq<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = 0) => new TObject<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = 0) => new TObject<int>(value, handle);
    }

    public sealed class EqualsToSeq_stringEE : EqualsToSeq<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
    }

    public sealed class EqualsToSeq_stringEO : EqualsToSeq<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = -1) => new TObject<string>(value, -1);
    }

    public sealed class EqualsToSeq_stringOE : EqualsToSeq<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = -1) => new TObject<string>(value, -1);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
    }

    public sealed class EqualsToSeq_stringOO : EqualsToSeq<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = 0) => 
            new TObject<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = 0) =>
            new TObject<string>(value, handle);
    }
}
