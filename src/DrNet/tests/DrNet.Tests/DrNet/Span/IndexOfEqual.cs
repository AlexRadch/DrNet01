using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class IndexOfEqual<T, TSource, TValue>
    {
        protected abstract T NewT(int value);

        protected abstract TSource NewTSource(T value, Action<T, T> onCompare = default);

        protected abstract TValue NewTValue(T value, Action<T, T> onCompare = default);

        private bool EqualityCompare(T v1, T v2)
        {
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        private bool EqualityCompare(TSource sValue, TValue vValue)
        {
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            return sValue.Equals(vValue);
        }

        private bool EqualityCompare(TValue vValue, TSource sValue)
        {
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            return vValue.Equals(sValue);
        }

        [Fact]
        public void ZeroLength()
        {
            Span<T> sp = new Span<T>(Array.Empty<T>());
            int idx = MemoryExt.IndexOfEqual(sp, NewTValue(NewT(0), null));
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
                if (!EqualityCompare(default(TSource), default(TValue)))
                    return;
                if (!EqualityCompare(default(TValue), default(TSource)))
                    return;
            }
            catch
            {
                return;
            }

            TSource[] a = new TSource[length];
            Span<TSource> span = new Span<TSource>(a);

            int idx = MemoryExt.IndexOfEqual(span, default(TValue));
            Assert.Equal(0, idx);
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
                } while (EqualityCompare(item, target) || EqualityCompare(item, target));

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);

            for (int targetIndex = 0; targetIndex < length; targetIndex++)
            {
                TSource temp = s[targetIndex];
                s[targetIndex] = NewTSource(target);

                int idx = MemoryExt.IndexOfEqual(span, target);
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
                } while (EqualityCompare(item, target) || EqualityCompare(item, target));

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);

            int idx = MemoryExt.IndexOfEqual(span, target);
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
                } while (EqualityCompare(item, target) || EqualityCompare(item, target));

                s[i] = NewTSource(item);
            }
            Span<TSource> span = new Span<TSource>(s);

            for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
            {
                TSource temp0 = s[targetIndex + 0];
                TSource temp1 = s[targetIndex + 1];
                s[targetIndex + 0] = s[targetIndex + 1] = NewTSource(target);

                int idx = MemoryExt.IndexOfEqual(span, target);
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
            var rnd = new Random(44);
            TLog<T> log = new TLog<T>();
            {
                T temp = NewT(rnd.Next());
                EqualityCompare(NewTSource(temp, log.Add), NewTValue(temp, log.Add));
                EqualityCompare(NewTValue(temp, log.Add), NewTSource(temp, log.Add));

            }
            bool logSupported = log.Count == 2;
            if (!logSupported)
            {
                bool sourceWithLog = typeof(TSource) == typeof(TObject<T>) && typeof(TSource) == typeof(TEquatable<T>);
                bool valueWithLog = typeof(TValue) == typeof(TObject<T>) && typeof(TValue) == typeof(TEquatable<T>);
                Assert.False(sourceWithLog && valueWithLog);
            }

            TSource[] a = new TSource[length];
            T[] b = new T[length];
            for (int i = 0; i < length; i++)
            {
                a[i] = NewTSource(10 * (i + 1), log.Add);
                b[i] = NewT(10 * (i + 1));
            }
            Span<TSource> span = new Span<TSource>(a);
            int idx = MemoryExt.IndexOfEqual(span, NewTValue(9999, log.Add));
            Assert.Equal(-1, idx);

            if (checkLog)
            {
                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for IndexOfEqual to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(a.Length, log.Count);
                foreach (T elem in b)
                {
                    int numCompares = log.CountCompares(elem, NewT(9999));
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }
            }
        }

        [Fact]
        public void MakeSureNoChecksGoOutOfRange()
        {
            T GuardValue = NewT(77777);
            const int GuardLength = 50;

            Action<T, T> checkForOutOfRangeAccess =
                delegate (T x, T y)
                {
                    if (EqualityComparer(x, GuardValue) || EqualityComparer(y, GuardValue))
                        throw new Exception("Detected out of range access in IndexOfEqual()");
                };

            for (int length = 0; length < 100; length++)
            {
                TSource[] a = new TSource[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSource(77777, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = NewTSource(10 * (i + 1), checkForOutOfRangeAccess);
                }

                Span<TSource> span = new Span<TSource>(a, GuardLength, length);
                int idx = MemoryExt.IndexOfEqual(span, NewTValue(9999, checkForOutOfRangeAccess));
                Assert.Equal(-1, idx);
            }
        }
    }

    public class IndexOfEqual_byte : IndexOfEqual<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public class IndexOfEqual_char : IndexOfEqual<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public class IndexOfEqual_intEE : IndexOfEqual<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
    }

    public class IndexOfEqual_intEO : IndexOfEqual<int, TEquatable<int>, TObject<int>>
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

    public class IndexOfEqual_intOE : IndexOfEqual<int, TObject<int>, TEquatable<int>>
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

    public class IndexOfEqual_intOO : IndexOfEqual<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
    }

    public class IndexOfEqual_stringEE : IndexOfEqual<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) => 
            new TEquatable<string>(ToString(), onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) => 
            new TEquatable<string>(ToString(), onCompare);
    }

    public class IndexOfEqual_stringEO : IndexOfEqual<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
        protected override TObject<string> NewTValue(int value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value.ToString(), onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class IndexOfEqual_stringOE : IndexOfEqual<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(int value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value.ToString(), onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        protected override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class IndexOfEqual_stringOO : IndexOfEqual<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
        protected override TObject<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
    }
}
