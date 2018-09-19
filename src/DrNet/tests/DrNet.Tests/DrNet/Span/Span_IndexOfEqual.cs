using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class Span_IndexOfEqual<T, TSource, TValue>
    {
        public abstract T NewT(int value);

        public abstract TSource NewTSource(int value, Action<T, T> onCompare = default);

        public abstract TValue NewTValue(int value, Action<T, T> onCompare = default);

        public bool EqualityComparer(T v1, T v2)
        {
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        public bool EqualityComparer(TSource sValue, TValue vValue)
        {
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            return vValue.Equals(sValue);
        }

        public bool EqualityComparer(TValue vValue, TSource sValue)
        {
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            return sValue.Equals(vValue);
        }

        [Fact]
        public void ZeroLength()
        {
            Span<T> sp = new Span<T>(Array.Empty<T>());
            int idx = MemoryExt.IndexOfEqual(sp, NewTValue(0, null));
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void DefaultFilled()
        {
            try
            {
                if (!EqualityComparer(default(TSource), default(TValue)))
                    return;
                if (!EqualityComparer(default(TValue), default(TSource)))
                    return;
            }
            catch
            {
                return;
            }

            for (int length = 1; length < 32; length++)
            {
                TSource[] a = new TSource[length];
                Span<TSource> span = new Span<TSource>(a);

                int idx = MemoryExt.IndexOfEqual(span, default(TValue));
                Assert.Equal(0, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            for (int length = 0; length < 32; length++)
            {
                TSource[] a = new TSource[length];
                TValue[] b = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSource(10 * (i + 1));
                    b[i] = NewTValue(10 * (i + 1));
                }
                Span<TSource> span = new Span<TSource>(a);

                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    TValue target = b[targetIndex];
                    int idx = MemoryExt.IndexOfEqual(span, target);
                    Assert.Equal(targetIndex, idx);
                }
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            var rnd = new Random(42);
            for (int length = 0; length < 32; length++)
            {
                TSource[] a = new TSource[length];
                int targetInt = rnd.Next(0, 256);
                TValue target = NewTValue(targetInt);
                for (int i = 0; i < length; i++)
                {
                    TSource val = NewTSource(i + 1);
                    a[i] = EqualityComparer(val, target) ? NewTSource(targetInt + 1) : val;
                }
                Span<TSource> span = new Span<TSource>(a);

                int idx = MemoryExt.IndexOfEqual(span, target);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 2; length < 32; length++)
            {
                TSource[] a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSource(10 * (i + 1));
                }

                a[length - 1] = NewTSource(5555);
                a[length - 2] = NewTSource(5555);

                Span<TSource> span = new Span<TSource>(a);
                int idx = MemoryExt.IndexOfEqual(span, NewTSource(5555));
                Assert.Equal(length - 2, idx);
            }
        }

        [Fact]
        public void OnNoMatchMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

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

    public class Span_IndexOfEqual_intEE: Span_IndexOfEqual<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_IndexOfEqual_intEO: Span_IndexOfEqual<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_IndexOfEqual_intOE: Span_IndexOfEqual<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_IndexOfEqual_intOO: Span_IndexOfEqual<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_IndexOfEqual_stringEE: Span_IndexOfEqual<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfEqual_stringEO: Span_IndexOfEqual<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfEqual_stringOE: Span_IndexOfEqual<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfEqual_stringOO: Span_IndexOfEqual<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
