using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class EndsWithSeq<T, TSource, TValue>
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

        [Fact]
        public void ZeroLength()
        {
            TSource[] f = new TSource[3];
            TValue[] s = new TValue[3];

            Span<TSource> first = new Span<TSource>(f, 1, 0);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s, 2, 0);

            bool b = MemoryExt.EndsWithSeq(first, second);
            Assert.True(b);
        }

        [Fact]
        public void SameSpan()
        {
            TSource[] f = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] s = { NewTValue(4), NewTValue(5), NewTValue(6) };

            Span<TSource> first = new Span<TSource>(f);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s);

            bool b = MemoryExt.EndsWithSeq(first, second);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatch()
        {
            TSource[] f = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] s = { NewTValue(4), NewTValue(5), NewTValue(6) };

            Span<TSource> first = new Span<TSource>(f, 0, 2);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s, 0, 3);

            bool b = MemoryExt.EndsWithSeq(first, second);
            Assert.False(b);
        }

        [Fact]
        public void Match()
        {
            TSource[] f = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] s = { NewTValue(4), NewTValue(5), NewTValue(6) };

            Span<TSource> span = new Span<TSource>(f, 0, 3);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s, 1, 2);

            bool b = MemoryExt.EndsWithSeq(span, second);
            Assert.True(b);
        }

        [Fact]
        public void MatchDifferentSpans()
        {
            TSource[] f = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] s = { NewTValue(4), NewTValue(5), NewTValue(6) };

            Span<TSource> span = new Span<TSource>(f, 0, 3);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s, 0, 3);

            bool b = MemoryExt.EndsWithSeq(span, second);
            Assert.True(b);
        }

        [Fact]
        public void OnEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                T[] a = new T[length];
                TSource[] f = new TSource[length];
                TValue[] s = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(10 * (i + 1));
                    f[i] = NewTSource(10 * (i + 1), log.Add);
                    s[i] = NewTValue(10 * (i + 1), log.Add);
                }

                Span<TSource> first = new Span<TSource>(f);
                ReadOnlySpan<TValue> secondSpan = new ReadOnlySpan<TValue>(s);

                bool b = MemoryExt.EndsWithSeq(first, secondSpan);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EndsWith to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (T elem in a)
                {
                    int numCompares = log.CountCompares(elem, elem);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }
            }
        }

        [Fact]
        public void NoMatch()
        {
            for (int length = 1; length < 32; length++)
            {
                for (int mismatchIndex = 0; mismatchIndex < length; mismatchIndex++)
                {
                    TLog<T> log = new TLog<T>();

                    TSource[] f = new TSource[length];
                    TValue[] s = new TValue[length];
                    for (int i = 0; i < length; i++)
                    {
                        f[i] = NewTSource(10 * (i + 1), log.Add);
                        s[i] = NewTValue(10 * (i + 1), log.Add);
                    }

                    T mismatchS = NewT(10 * (mismatchIndex + 1));
                    T mismatchV = NewT(10 * (mismatchIndex + 1) + 1);
                    s[mismatchIndex] = NewTValue(10 * (mismatchIndex + 1) + 1, log.Add);

                    Span<TSource> first = new Span<TSource>(f);
                    ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s);

                    bool b = MemoryExt.EndsWithSeq(first, second);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(mismatchS, mismatchV));
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
                        throw new Exception("Detected out of range access in EndsWithSeq()");
                };

            for (int length = 0; length < 100; length++)
            {
                TSource[] f = new TSource[GuardLength + length + GuardLength];
                TValue[] s = new TValue[GuardLength + length + GuardLength];
                for (int i = 0; i < f.Length; i++)
                {
                    f[i] = NewTSource(77777, checkForOutOfRangeAccess);
                    s[i] = NewTValue(77777, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    f[GuardLength + i] = NewTSource(10 * (i + 1), checkForOutOfRangeAccess);
                    s[GuardLength + i] = NewTValue(10 * (i + 1), checkForOutOfRangeAccess);
                }

                Span<TSource> first = new Span<TSource>(f, GuardLength, length);
                ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(s, GuardLength, length);

                bool b = MemoryExt.EndsWithSeq(first, second);
                Assert.True(b);
            }
        }
    }

    public class EndsWithSeq_intEE : EndsWithSeq<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class EndsWithSeq_intEO : EndsWithSeq<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class EndsWithSeq_intOE : EndsWithSeq<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class EndsWithSeq_intOO : EndsWithSeq<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public class EndsWithSeq_stringEE : EndsWithSeq<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class EndsWithSeq_stringEO : EndsWithSeq<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value.ToString(), onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class EndsWithSeq_stringOE : EndsWithSeq<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value.ToString(), onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class EndsWithSeq_stringOO : EndsWithSeq<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) =>
            new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) =>
            new TObject<string>(value.ToString(), onCompare);
    }
}
