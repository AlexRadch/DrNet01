using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class EndsWithSeq_EqualityComparer<T>
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

        public bool EqualityComparer(TEquatable<T> v1, TEquatable<T> v2)
        {
            onCompare?.Invoke(v1.Value, v2.Value);
            if (v1 is IEquatable<TEquatable<T>> equatable)
                return equatable.Equals(v2);
            return v1.Value.Equals(v2.Value);
        }

        [Fact]
        public void ZeroLength()
        {
            T[] a = new T[3];

            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 1, 0);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 2, 0);

            bool b = MemoryExt.EndsWithSeqSourceComparer(first, second, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EndsWithSeqValueComparer(first, second, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void SameSpan()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

            bool b = MemoryExt.EndsWithSeqSourceComparer<T, T>(span, span, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EndsWithSeqValueComparer<T, T>(span, span, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatch()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 0, 2);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 0, 3);

            bool b = MemoryExt.EndsWithSeqSourceComparer(first, second, EqualityComparer);
            Assert.False(b);
            b = MemoryExt.EndsWithSeqValueComparer(first, second, EqualityComparer);
            Assert.False(b);
        }

        [Fact]
        public void Match()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };

            ReadOnlySpan<T> span = new ReadOnlySpan<T>(a, 0, 3);
            ReadOnlySpan<T> slice = new ReadOnlySpan<T>(a, 1, 2);

            bool b = MemoryExt.EndsWithSeqSourceComparer(span, slice, EqualityComparer);
            Assert.True(b);
            b = MemoryExt.EndsWithSeqValueComparer(span, slice, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void MatchDifferentSpans()
        {
            T[] a = { NewT(4), NewT(5), NewT(6) };
            T[] b = { NewT(4), NewT(5), NewT(6) };

            ReadOnlySpan<T> span = new ReadOnlySpan<T>(a, 0, 3);
            ReadOnlySpan<T> slice = new ReadOnlySpan<T>(b, 0, 3);

            bool c = MemoryExt.EndsWithSeqSourceComparer(span, slice, EqualityComparer);
            Assert.True(c);
            c = MemoryExt.EndsWithSeqValueComparer(span, slice, EqualityComparer);
            Assert.True(c);
        }

        [Fact]
        public void OnEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                TEquatable<T>[] first = new TEquatable<T>[length];
                TEquatable<T>[] second = new TEquatable<T>[length];
                for (int i = 0; i < length; i++)
                {
                    first[i] = second[i] = new TEquatable<T>(NewT(10 * (i + 1)), log.Add);
                }

                ReadOnlySpan<TEquatable<T>> firstSpan = new ReadOnlySpan<TEquatable<T>>(first);
                ReadOnlySpan<TEquatable<T>> secondSpan = new ReadOnlySpan<TEquatable<T>>(second);

                bool b = MemoryExt.EndsWithSeqSourceComparer(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EndsWith to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (TEquatable<T> elem in first)
                {
                    int numCompares = log.CountCompares(elem.Value, elem.Value);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem.Value}.");
                }

                log.Clear();
                b = MemoryExt.EndsWithSeqValueComparer(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EndsWith to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (TEquatable<T> elem in first)
                {
                    int numCompares = log.CountCompares(elem.Value, elem.Value);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem.Value}.");
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

                    TEquatable<T>[] first = new TEquatable<T>[length];
                    TEquatable<T>[] second = new TEquatable<T>[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = second[i] = new TEquatable<T>(NewT(10 * (i + 1)), log.Add);
                    }

                    second[mismatchIndex] = new TEquatable<T>(NewT(10 * (mismatchIndex + 1) + 1), log.Add);

                    ReadOnlySpan<TEquatable<T>> firstSpan = new ReadOnlySpan<TEquatable<T>>(first);
                    ReadOnlySpan<TEquatable<T>> secondSpan = new ReadOnlySpan<TEquatable<T>>(second);

                    bool b = MemoryExt.EndsWithSeqSourceComparer(firstSpan, secondSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex].Value, second[mismatchIndex].Value));


                    log.Clear();
                    b = MemoryExt.EndsWithSeqValueComparer(firstSpan, secondSpan, EqualityComparer);
                    Assert.False(b);
                    Assert.Equal(1, log.CountCompares(first[mismatchIndex].Value, second[mismatchIndex].Value));
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
                TEquatable<T>[] first = new TEquatable<T>[GuardLength + length + GuardLength];
                TEquatable<T>[] second = new TEquatable<T>[GuardLength + length + GuardLength];
                for (int i = 0; i < first.Length; i++)
                {
                    first[i] = second[i] = new TEquatable<T>(GuardValue, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    first[GuardLength + i] = second[GuardLength + i] = new TEquatable<T>(NewT(10 * (i + 1)),
                        checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TEquatable<T>> firstSpan = new ReadOnlySpan<TEquatable<T>>(first, GuardLength, length);
                ReadOnlySpan<TEquatable<T>> secondSpan = new ReadOnlySpan<TEquatable<T>>(second, GuardLength, length);

                bool b = MemoryExt.EndsWithSeqSourceComparer(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);

                b = MemoryExt.EndsWithSeqValueComparer(firstSpan, secondSpan, EqualityComparer);
                Assert.True(b);
            }
        }
    }

    public class EndsWithSeq_EqualityComparer_int : EndsWithSeq_EqualityComparer<int>
    {
        public override int NewT(int value) => value;
    }

    public class EndsWithSeq_EqualityComparer_string : EndsWithSeq_EqualityComparer<string>
    {
        public override string NewT(int value) => value.ToString();
    }
}
