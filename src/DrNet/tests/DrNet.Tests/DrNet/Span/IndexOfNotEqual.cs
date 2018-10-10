using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class IndexOfNotEqual<T, TSource, TValue> : SpanTest<T, TSource, TValue>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);

            Span<TSource> span = new TSource[] { NextS(rnd), NextS(rnd), NextS(rnd) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NextS(rnd), NextS(rnd), NextS(rnd) }.AsReadOnlySpan(2, 0);

            int idx = DrNetMemoryExt.IndexOfNotEqual(span, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(span, NextNotDefaultV(rnd));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NextV(rnd), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NextV(rnd), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NextNotDefaultV(rnd));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, NextV(rnd), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NextV(rnd), EqualityCompareFrom);
            //Assert.Equal(-1, idx);
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

            var rnd = new Random(41 * (length + 1));

            TSource[] s = new TSource[length];
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            int idx = DrNetMemoryExt.IndexOfNotEqual(span, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(span, NextNotDefaultV(rnd));
            Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NextV(rnd), EqualityCompare);
            //Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NextV(rnd), EqualityCompareFrom);
            //Assert.Equal(0, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NextNotDefaultV(rnd));
            Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, NextV(rnd), EqualityCompare);
            //Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NextV(rnd), EqualityCompareFrom);
            //Assert.Equal(0, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(42 * (length + 1));
            T target = NextT(rnd);

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target));

            T item = NextNotEqualT(rnd, target);

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(item);

                int idx = DrNetMemoryExt.IndexOfNotEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

                idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

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
            T target = NextT(rnd);

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target));

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            int idx = DrNetMemoryExt.IndexOfNotEqual(span, NewTValue(target));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target), EqualityCompareFrom);
            //Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(44 * (length + 1));
            T target = NextT(rnd);

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target));

            T item = NextNotEqualT(rnd, target);

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
            {
                TSource temp0 = s[targetIndex + 0];
                TSource temp1 = s[targetIndex + 1];
                s[targetIndex + 0] = s[targetIndex + 1] = NewTSource(item);

                int idx = DrNetMemoryExt.IndexOfNotEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

                idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

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
            T target = NextT(rnd);

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target, handle));

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for IndexOfNotEqual to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            void CheckCompares()
            {
                Assert.Equal(s.Length, log.Count);
                int itemCount = length;
                int numCompares = log.CountCompares(target, target);
                Assert.True(numCompares == itemCount, $"Expected {itemCount} == {numCompares} for element {target}.");
            }

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            bool logSupported = IsLogSupported();

            log.Clear();
            int idx = DrNetMemoryExt.IndexOfNotEqual(span, NewTValue(target, handle));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NewTValue(target, handle));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            //if (!logSupported)
            //    OnCompare += log.Add;

            //log.Clear();
            //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target, log.Add), EqualityCompare);
            //Assert.Equal(-1, idx);
            //CheckCompares();

            //log.Clear();
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target, log.Add), EqualityCompareFrom);
            //Assert.Equal(-1, idx);
            //CheckCompares();

            //log.Clear();
            //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target, log.Add), EqualityCompare);
            //Assert.Equal(-1, idx);
            //CheckCompares();

            //log.Clear();
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target, log.Add), EqualityCompareFrom);
            //Assert.Equal(-1, idx);
            //CheckCompares();

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
            T target = NextT(rnd);
            const int guardLength = 50;

            T guard;
            do
            {
                guard = NextT(rnd);
            } while (EqualityCompareT(guard, target, true) || EqualityCompareT(target, guard, true));

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard, true) || EqualityCompareT(guard, x, true) ||
                    EqualityCompareT(y, guard, true) || EqualityCompareT(guard, y, true))
                    throw new Exception("Detected out of range access in IndexOfNotEqual()");
            }
            OnCompareActions<T>.Add(handle, checkForOutOfRangeAccess);

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard, handle));
            Array.Fill(s, NewTSource(target, handle), guardLength, length);

            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);

            int idx = DrNetMemoryExt.IndexOfNotEqual(span, NewTValue(target, handle));
            Assert.Equal(-1, idx);

            idx = DrNetMemoryExt.IndexOfNotEqual(rspan, NewTValue(target, handle));
            Assert.Equal(-1, idx);

            //OnCompare += checkForOutOfRangeAccess;

            //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target, checkForOutOfRangeAccess), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target, checkForOutOfRangeAccess), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            OnCompareActions<T>.RemoveHandler(handle);
            handle = 0;
        }
    }

    public sealed class IndexOfNotEqual_byte : IndexOfNotEqual<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, int handle = 0) => value;
        protected override byte NewTValue(byte value, int handle = 0) => value;
    }

    public sealed class IndexOfNotEqual_char : IndexOfNotEqual<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, int handle = 0) => value;
        protected override char NewTValue(char value, int handle = 0) => value;
    }

    public sealed class IndexOfNotEqual_int : IndexOfNotEqual<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, int handle = 0) => value;
        protected override int NewTValue(int value, int handle = 0) => value;
    }

    public sealed class IndexOfNotEqual_string : IndexOfNotEqual<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, int handle = 0) => value;
        protected override string NewTValue(string value, int handle = 0) => value;
    }

    public sealed class IndexOfNotEqual_intEE : IndexOfNotEqual<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class IndexOfNotEqual_intEO : IndexOfNotEqual<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, int handle = 0) => new TEquatable<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = -1) => new TObject<int>(value, -1);
    }

    public sealed class IndexOfNotEqual_intOE : IndexOfNotEqual<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = -1) => new TObject<int>(value, -1);
        protected override TEquatable<int> NewTValue(int value, int handle = 0) => new TEquatable<int>(value, handle);
    }

    public sealed class IndexOfNotEqual_intOO : IndexOfNotEqual<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, int handle = 0) => new TObject<int>(value, handle);
        protected override TObject<int> NewTValue(int value, int handle = 0) => new TObject<int>(value, handle);
    }

    public sealed class IndexOfNotEqual_stringEE : IndexOfNotEqual<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) => 
            new TEquatable<string>(value, handle);
    }

    public sealed class IndexOfNotEqual_stringEO : IndexOfNotEqual<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = -1) => new TObject<string>(value, -1);
    }

    public sealed class IndexOfNotEqual_stringOE : IndexOfNotEqual<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = -1) => new TObject<string>(value, -1);
        protected override TEquatable<string> NewTValue(string value, int handle = 0) =>
            new TEquatable<string>(value, handle);
    }

    public sealed class IndexOfNotEqual_stringOO : IndexOfNotEqual<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, int handle = 0) =>
            new TObject<string>(value, handle);
        protected override TObject<string> NewTValue(string value, int handle = 0) => 
            new TObject<string>(value, handle);
    }
}
