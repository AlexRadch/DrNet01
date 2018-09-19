﻿using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_SequenceEqualTo<T, TSource, TValue>
    {
        public abstract T NewT(int value);

        public abstract TSource NewTSource(int value, Action<T, T> onCompare = default);

        public abstract TValue NewTValue(int value, Action<T, T> onCompare = default);

        [Fact]
        public void ZeroLength()
        {
            TSource[] a = new TSource[3];
            TValue[] b = new TValue[3];

            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(a, 1, 0);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(b, 2, 0);
            bool c = MemoryExt.SequenceEqualTo<TSource, TValue>(first, second);
            Assert.True(c);
        }

        [Fact]
        public void SameSpan()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(a);
            bool b = MemoryExt.SequenceEqualTo<TSource, TSource>(span, span);
            Assert.True(b);
        }

        [Fact]
        public void ArrayImplicit()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };
            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(a, 0, 3);
            bool c = MemoryExt.SequenceEqualTo<TSource, TValue>(first, b);
            Assert.True(c);
        }

        [Fact]
        public void ArraySegmentImplicit()
        {
            TSource[] src = { NewTSource(1), NewTSource(2), NewTSource(3) };
            TValue[] dst = { NewTValue(5), NewTValue(1), NewTValue(2), NewTValue(3), NewTValue(10) };
            var segment = new ArraySegment<TValue>(dst, 1, 3);

            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(src, 0, 3);
            bool b = MemoryExt.SequenceEqualTo<TSource, TValue>(first, segment);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatch()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };

            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(a, 0, 3);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(b, 0, 2);
            bool c = MemoryExt.SequenceEqualTo<TSource, TValue>(first, second);
            Assert.False(c);

            first = new ReadOnlySpan<TSource>(a, 0, 2);
            second = new ReadOnlySpan<TValue>(b, 0, 3);
            c = MemoryExt.SequenceEqualTo<TSource, TValue>(first, second);
            Assert.False(c);
        }

        [Fact]
        public void OnEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                T[] items = new T[length];
                TSource[] first = new TSource[length];
                TValue[] second = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    items[i] = NewT(10 * (i + 1));
                    first[i] = NewTSource(10 * (i + 1), log.Add);
                    second[i] = NewTValue(10 * (i + 1), log.Add);
                }

                ReadOnlySpan<TSource> firstSpan = new ReadOnlySpan<TSource>(first);
                ReadOnlySpan<TValue> secondSpan = new ReadOnlySpan<TValue>(second);
                bool b = MemoryExt.SequenceEqualTo<TSource, TValue>(firstSpan, secondSpan);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // SequenceEqualTo to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (T elem in items)
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

                    TSource[] first = new TSource[length];
                    TValue[] second = new TValue[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = NewTSource(10 * (i + 1), log.Add);
                        second[i] = NewTValue(10 * (i + 1), log.Add);
                    }

                    T mismatchSpan = NewT(10 * (mismatchIndex + 1));
                    T mismatchValue = NewT(10 * (mismatchIndex + 2));
                    second[mismatchIndex] = NewTValue(10 * (mismatchIndex + 2), log.Add);

                    ReadOnlySpan<TSource> firstSpan = new ReadOnlySpan<TSource>(first);
                    ReadOnlySpan<TValue> secondSpan = new ReadOnlySpan<TValue>(second);
                    bool b = MemoryExt.SequenceEqualTo<TSource, TValue>(firstSpan, secondSpan);
                    Assert.False(b);

                    Assert.Equal(mismatchIndex + 1, log.Count);
                    Assert.Equal(1, log.CountCompares(mismatchSpan, mismatchValue));
                }
            }
        }

        [Fact]
        public void MakeSureNoChecksGoOutOfRange()
        {
            int GuardInt = 77777;
            T GuardValue = NewT(GuardInt);
            const int GuardLength = 50;

            Action<T, T> checkForOutOfRangeAccess =
                delegate (T x, T y)
                {
                    if (GuardValue.Equals(x) || GuardValue.Equals(y))
                        throw new Exception("Detected out of range access in IndexOf()");
                };

            for (int length = 0; length < 100; length++)
            {
                TSource[] first = new TSource[GuardLength + length + GuardLength];
                TValue[] second = new TValue[GuardLength + length + GuardLength];
                for (int i = 0; i < first.Length; i++)
                {
                    first[i] = NewTSource(GuardInt, checkForOutOfRangeAccess);
                    second[i] = NewTValue(GuardInt, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    first[GuardLength + i] = NewTSource(10 * (i + 1), checkForOutOfRangeAccess);
                    second[GuardLength + i] = NewTValue(10 * (i + 1), checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TSource> firstSpan = new ReadOnlySpan<TSource>(first, GuardLength, length);
                ReadOnlySpan<TValue> secondSpan = new ReadOnlySpan<TValue>(second, GuardLength, length);
                bool b = MemoryExt.SequenceEqualTo<TSource, TValue>(firstSpan, secondSpan);
                Assert.True(b);
            }
        }
    }

    public class ReadOnlySpan_SequenceEqualTo_intEE: ReadOnlySpan_SequenceEqualTo<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_intEO: ReadOnlySpan_SequenceEqualTo<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_intOE: ReadOnlySpan_SequenceEqualTo<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_intOO: ReadOnlySpan_SequenceEqualTo<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_stringEE: ReadOnlySpan_SequenceEqualTo<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_stringEO: ReadOnlySpan_SequenceEqualTo<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_stringOE: ReadOnlySpan_SequenceEqualTo<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_SequenceEqualTo_stringOO: ReadOnlySpan_SequenceEqualTo<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
