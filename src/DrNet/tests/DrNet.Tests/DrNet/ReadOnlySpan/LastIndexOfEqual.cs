using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class LastIndexOfEqual<T, TSource, TValue>
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
            var rnd = new Random(42);
            T target = NewT(rnd.Next());

            ReadOnlySpan<TSource> sp = new ReadOnlySpan<TSource>(Array.Empty<TSource>());

            int idx = MemoryExt.LastIndexOfEqual(sp, NewTValue(target));
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqual(sp, NewTValue(target), EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualFrom(sp, NewTValue(target), EqualityCompareFrom);
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

            TSource[] s = new TSource[length];
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s);

            int idx = MemoryExt.LastIndexOfEqual(span, default(TValue));
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqual(span, default(TValue), EqualityCompare);
            Assert.Equal(length - 1, idx);
            idx = MemoryExt.LastIndexOfEqualFrom(span, default(TValue), EqualityCompareFrom);
            Assert.Equal(length - 1, idx);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(42);
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
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(target);

                int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target), EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualFrom(span, NewTValue(target), EqualityCompareFrom);
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
            var rnd = new Random(43);
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
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s);

            int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target));
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target), EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualFrom(span, NewTValue(target), EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(44);
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
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s);

            for (int targetIndex = 1; targetIndex < length; targetIndex++)
            {
                TSource temp0 = s[targetIndex - 0];
                TSource temp1 = s[targetIndex - 1];
                s[targetIndex - 0] = s[targetIndex - 1] = NewTSource(target);

                int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target));
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target), EqualityCompare);
                Assert.Equal(targetIndex, idx);
                idx = MemoryExt.LastIndexOfEqualFrom(span, NewTValue(target), EqualityCompareFrom);
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
            var rnd = new Random(45);
            T target = NewT(rnd.Next());
            TLog<T> log = new TLog<T>();

            T[] t = new T[length];
            TSource[] s = new TSource[length];
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item) || 
                    Array.IndexOf(t, item, 0, i) >= 0);

                t[i] = item;
                s[i] = NewTSource(item, log.Add);
            }
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s);

            {
                T temp = NewT(rnd.Next());
                EqualityCompare(NewTSource(temp, log.Add), NewTValue(temp, log.Add));
                EqualityCompareFrom(NewTValue(temp, log.Add), NewTSource(temp, log.Add));
            }
            bool logSupported = log.Count == 2;
            if (!logSupported)
            {
                bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) && typeof(TSource) == typeof(TEquatable<T>);
                bool valueWithLog = typeof(TValue) == typeof(TObject<T>) && typeof(TValue) == typeof(TEquatable<T>);
                Assert.False(sourceWithLog && valueWithLog);
            }

            log.Clear();
            int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target, log.Add));
            Assert.Equal(-1, idx);
            if (logSupported)
            {
                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for LastIndexOfEqual to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(s.Length, log.Count);
                foreach (T item in t)
                {
                    int numCompares = log.CountCompares(item, target);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {item}.");
                }
            }

            log.Clear();
            if (!logSupported)
                onCompare = log.Add;

            idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target, log.Add), EqualityCompare);
            Assert.Equal(-1, idx);

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for LastIndexOfEqual to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            Assert.Equal(s.Length, log.Count);
            foreach (T item in t)
            {
                int numCompares = log.CountCompares(item, target);
                Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {item}.");
            }

            log.Clear();
            idx = MemoryExt.LastIndexOfEqualFrom(span, NewTValue(target, log.Add), EqualityCompareFrom);
            Assert.Equal(-1, idx);

            // Since we asked for a non-existent value, make sure each element of the array was compared once.
            // (Strictly speaking, it would not be illegal for LastIndexOfEqual to compare an element more than once but
            // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
            Assert.Equal(s.Length, log.Count);
            foreach (T item in t)
            {
                int numCompares = log.CountCompares(item, target);
                Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {item}.");
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void MakeSureNoChecksGoOutOfRange(int length)
        {
            var rnd = new Random(46);
            T target = NewT(rnd.Next());
            T guard = NewT(rnd.Next());
            const int guardLength = 50;

            void checkForOutOfRangeAccess(T x, T y)
            {
                if (EqualityCompareT(x, guard) || EqualityCompareT(guard, x) ||
                    EqualityCompareT(y, guard) || EqualityCompareT(guard, y))
                    throw new Exception("Detected out of range access in LastIndexOfEqual()");
            }

            TSource[] s = new TSource[guardLength + length + guardLength];
            Array.Fill(s, NewTSource(guard));
            for (int i = 0; i < length; i++)
            {
                T item;
                do
                {
                    item = NewT(rnd.Next());
                } while (EqualityCompareT(item, target) || EqualityCompareT(target, item) ||
                    EqualityCompareT(item, guard) || EqualityCompareT(guard, item));

                s[i + guardLength] = NewTSource(item, checkForOutOfRangeAccess);
            }
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(s, guardLength, length);

            int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target, checkForOutOfRangeAccess));
            Assert.Equal(-1, idx);

            onCompare = checkForOutOfRangeAccess;
            idx = MemoryExt.LastIndexOfEqual(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompare);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfEqualFrom(span, NewTValue(target, checkForOutOfRangeAccess), EqualityCompareFrom);
            Assert.Equal(-1, idx);
        }
    }

    public class LastIndexOfEqual_byte : LastIndexOfEqual<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public class LastIndexOfEqual_char : LastIndexOfEqual<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public class LastIndexOfEqual_intEE : LastIndexOfEqual<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
    }

    public class LastIndexOfEqual_intEO : LastIndexOfEqual<int, TEquatable<int>, TObject<int>>
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

    public class LastIndexOfEqual_intOE : LastIndexOfEqual<int, TObject<int>, TEquatable<int>>
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

    public class LastIndexOfEqual_intOO : LastIndexOfEqual<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
    }

    public class LastIndexOfEqual_stringEE : LastIndexOfEqual<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) => 
            new TEquatable<string>(value, onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) => 
            new TEquatable<string>(value, onCompare);
    }

    public class LastIndexOfEqual_stringEO : LastIndexOfEqual<string, TEquatable<string>, TObject<string>>
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

    public class LastIndexOfEqual_stringOE : LastIndexOfEqual<string, TObject<string>, TEquatable<string>>
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

    public class LastIndexOfEqual_stringOO : LastIndexOfEqual<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare) => 
            new TObject<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare) => 
            new TObject<string>(value, onCompare);
    }
}
