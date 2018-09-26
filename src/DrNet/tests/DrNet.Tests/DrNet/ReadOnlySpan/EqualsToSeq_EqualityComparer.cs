using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class EqualsToSeq_EqualityComparer<T>
    {
        public abstract T NewT(int value);

        protected Action<T, T> onCompare;

        public bool EqualityComparer(T v1, T v2)
        {
            onCompare?.Invoke(v1, v2);
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        public bool EqualityComparer(TEquatable<T> v1, TEquatable<T> v2) => EqualityComparer(v1.Value, v2.Value);

        [Fact]
        public void ZeroLength()
        {
            T[] a = new T[3];

            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 1, 0);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 2, 0);

            bool b = MemoryExt.EqualsToSeq<T, T>(first, second, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<T, T>(second, first, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(first, second, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(second, first, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void SameSpan()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);
            bool b = MemoryExt.EqualsToSeq<T, T>(span, span, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(span, span, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void ArrayImplicit()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 0, 3);
            bool b = MemoryExt.EqualsToSeq<T, T>(first, a, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<T, T>(a, first, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(first, a, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(a, first, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void ArraySegmentImplicit()
        {
            T[] src = { NewT(1), NewT(2), NewT(3) };
            T[] dst = { NewT(5), NewT(1), NewT(2), NewT(3), NewT(10) };
            var segment = new ArraySegment<T>(dst, 1, 3);

            ReadOnlySpan<T> first = new ReadOnlySpan<T>(src, 0, 3);

            bool b = MemoryExt.EqualsToSeq<T, T>(first, segment, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsToSeq<T, T>(segment, first, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(first, segment, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EqualsFromSeq<T, T>(segment, first, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatch()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 0, 3);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 0, 2);
            bool b = MemoryExt.EqualsToSeq<T, T>(first, second, EqualityComparer);
            Assert.False(b);
            b = MemoryExt.EqualsToSeq<T, T>(second, first, EqualityComparer);
            Assert.False(b);
            b = MemoryExt.EqualsFromSeq<T, T>(first, second, EqualityComparer);
            Assert.False(b);
            b = MemoryExt.EqualsFromSeq<T, T>(second, first, EqualityComparer);
            Assert.False(b);
        }

        [Fact]
        public void OnEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();
                onCompare = log.Add;

                T[] first = new T[length];
                T[] second = new T[length];
                for (int i = 0; i < length; i++)
                {
                    first[i] = second[i] = NewT(10 * (i + 1));
                }

                ReadOnlySpan<T> firstSpan = new ReadOnlySpan<T>(first);
                ReadOnlySpan<T> secondSpan = new ReadOnlySpan<T>(second);

                bool b = MemoryExt.EqualsToSeq<T, T>(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EqualToSeq to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (T elem in first)
                {
                    int numCompares = log.CountCompares(elem, elem);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }

                log.Clear();
                b = MemoryExt.EqualsFromSeq<T, T>(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EqualFromSeq to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (T elem in first)
                {
                    int numCompares = log.CountCompares(elem, elem);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            for (int length = 1; length < 32; length++)
            {
                for (int mismatchIndex = 0; mismatchIndex < length; mismatchIndex++)
                {
                    TLog<T> log = new TLog<T>();
                    onCompare = log.Add;

                    T[] first = new T[length];
                    T[] second = new T[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = second[i] = NewT(10 * (i + 1));
                    }

                    second[mismatchIndex] = NewT(10 * (mismatchIndex + 2));

                    ReadOnlySpan<T> firstSpan = new ReadOnlySpan<T>(first);
                    ReadOnlySpan<T> secondSpan = new ReadOnlySpan<T>(second);

                    bool b = MemoryExt.EqualsToSeq<T, T>(firstSpan, secondSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex], second[mismatchIndex]));

                    log.Clear();
                    b = MemoryExt.EqualsToSeq<T, T>(secondSpan, firstSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex], second[mismatchIndex]));

                    log.Clear();
                    b = MemoryExt.EqualsFromSeq<T, T>(firstSpan, secondSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex], second[mismatchIndex]));

                    log.Clear();
                    b = MemoryExt.EqualsFromSeq<T, T>(secondSpan, firstSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex], second[mismatchIndex]));
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
                    if (GuardValue.Equals(x) || GuardValue.Equals(y))
                        throw new Exception("Detected out of range access in IndexOf()");
                };

            for (int length = 0; length < 100; length++)
            {
                TEquatable<T>[] first = new TEquatable<T>[GuardLength + length + GuardLength];
                TEquatable<T>[] second = new TEquatable<T>[GuardLength + length + GuardLength];
                for (int i = 0; i < first.Length; i++)
                {
                    first[i] = second[i] = new TEquatable<T>(GuardValue, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    first[GuardLength + i] = second[GuardLength + i] = new TEquatable<T>(NewT(10 * (i + 1)), checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TEquatable<T>> firstSpan = new ReadOnlySpan<TEquatable<T>>(first, GuardLength, length);
                ReadOnlySpan<TEquatable<T>> secondSpan = new ReadOnlySpan<TEquatable<T>>(second, GuardLength, length);

                bool b = MemoryExt.EqualsToSeq<TEquatable<T>, TEquatable<T>>(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);
                b = MemoryExt.EqualsFromSeq<TEquatable<T>, TEquatable<T>>(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);
            }
        }
    }

    public class EqualsToSeq_EqualityComparer_int : EqualsToSeq_EqualityComparer<int>
    {
        public override int NewT(int value) => value;
    }

    public class EqualsToSeq_EqualityComparer_string : EqualsToSeq_EqualityComparer<string>
    {
        public override string NewT(int value) => value.ToString();
    }
}
