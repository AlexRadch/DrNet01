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
            TValue NextTValue() => NewTValue(NewT(rnd.Next(1, int.MaxValue)));

            Span<TSource> span = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsReadOnlySpan(2, 0);

            int idx = MemoryExt.IndexOfNotEqual(span, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(span, NextTValue());
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NextTValue(), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NextTValue(), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, NextTValue());
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, NextTValue(), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NextTValue(), EqualityCompareFrom);
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

            var rnd = new Random(41);
            TValue NextTValue() => NewTValue(NewT(rnd.Next(1, int.MaxValue)));

            TSource[] s = new TSource[length];
            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            int idx = MemoryExt.IndexOfNotEqual(span, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, default(TValue), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, default(TValue), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(span, NextTValue());
            Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NextTValue(), EqualityCompare);
            //Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NextTValue(), EqualityCompareFrom);
            //Assert.Equal(0, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, NextTValue());
            Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqual(rspan, NextTValue(), EqualityCompare);
            //Assert.Equal(0, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NextTValue(), EqualityCompareFrom);
            //Assert.Equal(0, idx);
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
            Array.Fill(s, NewTSource(target));

            T item;
            do
            {
                item = NewT(rnd.Next());
            } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(item);

                int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
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
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target));

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target));
            Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
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
            T target = NewT(rnd.Next());

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target));

            T item;
            do
            {
                item = NewT(rnd.Next());
            } while (EqualityCompareT(item, target) || EqualityCompareT(target, item));

            Span<TSource> span = new Span<TSource>(s);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
            {
                TSource temp0 = s[targetIndex + 0];
                TSource temp1 = s[targetIndex + 1];
                s[targetIndex + 0] = s[targetIndex + 1] = NewTSource(item);

                int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target), EqualityCompare);
                //Assert.Equal(targetIndex, idx);
                //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target), EqualityCompareFrom);
                //Assert.Equal(targetIndex, idx);

                idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target));
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
            var rnd = new Random(45 * (length + 1));
            T target = NewT(rnd.Next());
            TLog<T> log = new TLog<T>();

            TSource[] s = new TSource[length];
            Array.Fill(s, NewTSource(target, log.Add));

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
            int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target, log.Add));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            log.Clear();
            idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target, log.Add));
            Assert.Equal(-1, idx);
            if (logSupported)
                CheckCompares();

            if (!logSupported)
                OnCompare += log.Add;

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
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void MakeSureNoChecksGoOutOfRange(int length)
        {
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
                    throw new Exception("Detected out of range access in IndexOfNotEqual()");
            }

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard, checkForOutOfRangeAccess));
            Array.Fill(s, NewTSource(target, checkForOutOfRangeAccess), guardLength, length);

            Span<TSource> span = new Span<TSource>(s, guardLength, length);
            ReadOnlySpan<TSource> rspan = new ReadOnlySpan<TSource>(s, guardLength, length);

            int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target, checkForOutOfRangeAccess));
            Assert.Equal(-1, idx);

            idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target, checkForOutOfRangeAccess));
            Assert.Equal(-1, idx);

            OnCompare += checkForOutOfRangeAccess;

            //idx = MemoryExt.IndexOfNotEqual(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompareFrom);
            //Assert.Equal(-1, idx);

            //idx = MemoryExt.IndexOfNotEqual(rspan, NewTValue(target, checkForOutOfRangeAccess), EqualityCompare);
            //Assert.Equal(-1, idx);
            //idx = MemoryExt.IndexOfNotEqualFrom(rspan, NewTValue(target, checkForOutOfRangeAccess), EqualityCompareFrom);
            //Assert.Equal(-1, idx);
        }
    }

    public sealed class IndexOfNotEqual_byte : IndexOfNotEqual<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public sealed class IndexOfNotEqual_char : IndexOfNotEqual<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public sealed class IndexOfNotEqual_int : IndexOfNotEqual<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, Action<int, int> onCompare) => value;
        protected override int NewTValue(int value, Action<int, int> onCompare) => value;
    }

    public sealed class IndexOfNotEqual_string : IndexOfNotEqual<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, Action<string, string> onCompare) => value;
        protected override string NewTValue(string value, Action<string, string> onCompare) => value;
    }

    public sealed class IndexOfNotEqual_intEE : IndexOfNotEqual<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public sealed class IndexOfNotEqual_intEO : IndexOfNotEqual<int, TEquatable<int>, TObject<int>>
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

    public sealed class IndexOfNotEqual_intOE : IndexOfNotEqual<int, TObject<int>, TEquatable<int>>
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

    public sealed class IndexOfNotEqual_intOO : IndexOfNotEqual<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public sealed class IndexOfNotEqual_stringEE : IndexOfNotEqual<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public sealed class IndexOfNotEqual_stringEO : IndexOfNotEqual<string, TEquatable<string>, TObject<string>>
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

    public sealed class IndexOfNotEqual_stringOE : IndexOfNotEqual<string, TObject<string>, TEquatable<string>>
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

    public sealed class IndexOfNotEqual_stringOO : IndexOfNotEqual<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
    }
}
