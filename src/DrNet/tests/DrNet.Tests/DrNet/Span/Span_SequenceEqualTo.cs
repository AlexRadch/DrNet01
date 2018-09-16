using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class Span_SequenceEqualTo<T, TSpan, TValue>
    {
        public abstract T NewT(int value);

        public abstract TSpan NewTSpan(int value, Action<T, T> onCompare = default);

        public abstract TValue NewTValue(int value, Action<T, T> onCompare = default);

        [Fact]
        public void ZeroLengthSequenceEqual()
        {
            TSpan[] a = new TSpan[3];
            TValue[] b = new TValue[3];

            Span<TSpan> first = new Span<TSpan>(a, 1, 0);
            Span<TValue> second = new Span<TValue>(b, 2, 0);
            bool c = MemoryExt.SequenceEqualTo<TSpan, TValue>(first, second);
            Assert.True(c);
        }

        [Fact]
        public void SameSpanSequenceEqual()
        {
            TSpan[] a = { NewTSpan(4), NewTSpan(5), NewTSpan(6) };
            Span<TSpan> span = new Span<TSpan>(a);
            bool b = MemoryExt.SequenceEqualTo<TSpan, TSpan>(span, span);
            Assert.True(b);
        }

        [Fact]
        public void SequenceEqualArrayImplicit()
        {
            TSpan[] a = { NewTSpan(4), NewTSpan(5), NewTSpan(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };
            Span<TSpan> first = new Span<TSpan>(a, 0, 3);
            bool c = MemoryExt.SequenceEqualTo<TSpan, TValue>(first, b);
            Assert.True(c);
        }

        [Fact]
        public void SequenceEqualArraySegmentImplicit()
        {
            TSpan[] src = { NewTSpan(1), NewTSpan(2), NewTSpan(3) };
            TValue[] dst = { NewTValue(5), NewTValue(1), NewTValue(2), NewTValue(3), NewTValue(10) };
            var segment = new ArraySegment<TValue>(dst, 1, 3);

            Span<TSpan> first = new Span<TSpan>(src, 0, 3);
            bool b = MemoryExt.SequenceEqualTo<TSpan, TValue>(first, segment);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatchSequenceEqual()
        {
            TSpan[] a = { NewTSpan(4), NewTSpan(5), NewTSpan(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };

            Span<TSpan> first = new Span<TSpan>(a, 0, 3);
            Span<TValue> second = new Span<TValue>(b, 0, 2);
            bool c = MemoryExt.SequenceEqualTo<TSpan, TValue>(first, second);
            Assert.False(c);

            first = new Span<TSpan>(a, 0, 2);
            second = new Span<TValue>(b, 0, 3);
            c = MemoryExt.SequenceEqualTo<TSpan, TValue>(first, second);
            Assert.False(c);
        }

        [Fact]
        public void OnSequenceEqualOfEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                T[] items = new T[length];
                TSpan[] first = new TSpan[length];
                TValue[] second = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    items[i] = NewT(10 * (i + 1));
                    first[i] = NewTSpan(10 * (i + 1), log.Add);
                    second[i] = NewTValue(10 * (i + 1), log.Add);
                }

                Span<TSpan> firstSpan = new Span<TSpan>(first);
                Span<TValue> secondSpan = new Span<TValue>(second);
                bool b = MemoryExt.SequenceEqualTo<TSpan, TValue>(firstSpan, secondSpan);
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
        public void SequenceEqualNoMatch()
        {
            for (int length = 1; length < 32; length++)
            {
                for (int mismatchIndex = 0; mismatchIndex < length; mismatchIndex++)
                {
                    TLog<T> log = new TLog<T>();

                    TSpan[] first = new TSpan[length];
                    TValue[] second = new TValue[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = NewTSpan(10 * (i + 1), log.Add);
                        second[i] = NewTValue(10 * (i + 1), log.Add);
                    }

                    T mismatchSpan = NewT(10 * (mismatchIndex + 1));
                    T mismatchValue = NewT(10 * (mismatchIndex + 2));
                    second[mismatchIndex] = NewTValue(10 * (mismatchIndex + 2), log.Add);

                    Span<TSpan> firstSpan = new Span<TSpan>(first);
                    Span<TValue> secondSpan = new Span<TValue>(second);
                    bool b = MemoryExt.SequenceEqualTo<TSpan, TValue>(firstSpan, secondSpan);
                    Assert.False(b);

                    Assert.Equal(mismatchIndex + 1, log.Count);
                    Assert.Equal(1, log.CountCompares(mismatchSpan, mismatchValue));
                }
            }
        }

        [Fact]
        public void MakeSureNoSequenceEqualChecksGoOutOfRange()
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
                TSpan[] first = new TSpan[GuardLength + length + GuardLength];
                TValue[] second = new TValue[GuardLength + length + GuardLength];
                for (int i = 0; i < first.Length; i++)
                {
                    first[i] = NewTSpan(GuardInt, checkForOutOfRangeAccess);
                    second[i] = NewTValue(GuardInt, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    first[GuardLength + i] = NewTSpan(10 * (i + 1), checkForOutOfRangeAccess);
                    second[GuardLength + i] = NewTValue(10 * (i + 1), checkForOutOfRangeAccess);
                }

                Span<TSpan> firstSpan = new Span<TSpan>(first, GuardLength, length);
                Span<TValue> secondSpan = new Span<TValue>(second, GuardLength, length);
                bool b = MemoryExt.SequenceEqualTo<TSpan, TValue>(firstSpan, secondSpan);
                Assert.True(b);
            }
        }
    }

    public class Span_SequenceEqualTo_intEE: Span_SequenceEqualTo<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_SequenceEqualTo_intEO: Span_SequenceEqualTo<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_SequenceEqualTo_intOE: Span_SequenceEqualTo<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_SequenceEqualTo_intOO: Span_SequenceEqualTo<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_SequenceEqualTo_stringEE: Span_SequenceEqualTo<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_SequenceEqualTo_stringEO: Span_SequenceEqualTo<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class Span_SequenceEqualTo_stringOE: Span_SequenceEqualTo<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_SequenceEqualTo_stringOO: Span_SequenceEqualTo<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
