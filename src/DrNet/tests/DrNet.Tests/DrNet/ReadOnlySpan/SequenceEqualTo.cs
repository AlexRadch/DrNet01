using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class EqualToSeq<T, TSource, TValue>
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
            bool c = MemoryExt.EqualToSeq<TSource, TValue>(first, second);
            Assert.True(c);
        }

        [Fact]
        public void SameSpan()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(a);
            bool b = MemoryExt.EqualToSeq<TSource, TSource>(span, span);
            Assert.True(b);
        }

        [Fact]
        public void ArrayImplicit()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };
            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(a, 0, 3);
            bool c = MemoryExt.EqualToSeq<TSource, TValue>(first, b);
            Assert.True(c);
        }

        [Fact]
        public void ArraySegmentImplicit()
        {
            TSource[] src = { NewTSource(1), NewTSource(2), NewTSource(3) };
            TValue[] dst = { NewTValue(5), NewTValue(1), NewTValue(2), NewTValue(3), NewTValue(10) };
            var segment = new ArraySegment<TValue>(dst, 1, 3);

            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(src, 0, 3);
            bool b = MemoryExt.EqualToSeq<TSource, TValue>(first, segment);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatch()
        {
            TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
            TValue[] b = { NewTValue(4), NewTValue(5), NewTValue(6) };

            ReadOnlySpan<TSource> first = new ReadOnlySpan<TSource>(a, 0, 3);
            ReadOnlySpan<TValue> second = new ReadOnlySpan<TValue>(b, 0, 2);
            bool c = MemoryExt.EqualToSeq<TSource, TValue>(first, second);
            Assert.False(c);

            first = new ReadOnlySpan<TSource>(a, 0, 2);
            second = new ReadOnlySpan<TValue>(b, 0, 3);
            c = MemoryExt.EqualToSeq<TSource, TValue>(first, second);
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
                bool b = MemoryExt.EqualToSeq<TSource, TValue>(firstSpan, secondSpan);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // EqualToSeq to compare an element more than once but that would be a non-optimal implementation and 
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
                    bool b = MemoryExt.EqualToSeq<TSource, TValue>(firstSpan, secondSpan);
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
                bool b = MemoryExt.EqualToSeq<TSource, TValue>(firstSpan, secondSpan);
                Assert.True(b);
            }
        }
    }

    public class EqualToSeq_intEE : EqualToSeq<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class EqualToSeq_intEO : EqualToSeq<int, TEquatable<int>, TObject<int>>
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

    public class EqualToSeq_intOE : EqualToSeq<int, TObject<int>, TEquatable<int>>
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

    public class EqualToSeq_intOO : EqualToSeq<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public class EqualToSeq_stringEE : EqualToSeq<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) =>
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class EqualToSeq_stringEO : EqualToSeq<string, TEquatable<string>, TObject<string>>
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

    public class EqualToSeq_stringOE : EqualToSeq<string, TObject<string>, TEquatable<string>>
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

    public class EqualToSeq_stringOO : EqualToSeq<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) =>
            new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) =>
            new TObject<string>(value.ToString(), onCompare);
    }
}
