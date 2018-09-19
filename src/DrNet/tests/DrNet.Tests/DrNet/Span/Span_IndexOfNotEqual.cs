using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class Span_IndexOfNotEqual<T, TSource, TValue>
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
            int idx = MemoryExt.IndexOfNotEqual(sp, NewTValue(0, null));
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

                int idx = MemoryExt.IndexOfNotEqual(span, default(TValue));
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            var rnd = new Random(41);
            for (int length = 1; length < 32; length++)
            {
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    int targetInt1;
                    int targetInt2;
                    do 
                    {
                        targetInt1 = rnd.Next(0, 256);
                        targetInt2 = rnd.Next(0, 256);

                    } while (targetInt2 == targetInt1);

                    TSource[] a = new TSource[length];
                    for (int i = 0; i < length; i++)
                    {
                        a[i] = NewTSource(targetInt1);
                    }

                    a[targetIndex] = NewTSource(targetInt2);

                    Span<TSource> span = new Span<TSource>(a);
                    int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(targetInt1));
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
                int targetInt = rnd.Next(0, 256);

                TSource[] a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSource(targetInt);
                }

                Span<TSource> span = new Span<TSource>(a);
                int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(targetInt));
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            var rnd = new Random(43);
            for (int length = 2; length < 32; length++)
            {
                int targetInt1;
                int targetInt2;
                do 
                {
                    targetInt1 = rnd.Next(0, 256);
                    targetInt2 = rnd.Next(0, 256);

                } while (targetInt2 == targetInt1);

                TSource[] a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSource(targetInt1);
                }

                a[length - 1] = NewTSource(targetInt2);
                a[length - 2] = NewTSource(targetInt2);

                Span<TSource> span = new Span<TSource>(a);
                int idx = MemoryExt.IndexOfNotEqual(span, NewTSource(targetInt1));
                Assert.Equal(length - 2, idx);
            }
        }

        [Fact]
        public void OnNoMatchMakeSureEveryElementIsCompared()
        {
            var rnd = new Random(43);
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                int targetInt = rnd.Next(0, 256);

                TSource[] a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSource(targetInt, log.Add);
                }
                Span<TSource> span = new Span<TSource>(a);
                int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(targetInt, log.Add));
                Assert.Equal(-1, idx);

                Assert.Equal(a.Length, log.Count);
                Assert.Equal(a.Length, log.CountCompares(NewT(targetInt), NewT(targetInt)));
            }
        }

        [Fact]
        public void MakeSureNoChecksGoOutOfRange()
        {
            var rnd = new Random(44);
            T GuardValue = NewT(77777);
            const int GuardLength = 50;

            Action<T, T> checkForOutOfRangeAccess =
                delegate (T x, T y)
                {
                    if (EqualityComparer(x, GuardValue) || EqualityComparer(y, GuardValue))
                        throw new Exception("Detected out of range access in IndexOfNotEqual()");
                };

            for (int length = 0; length < 100; length++)
            {
                int targetInt = rnd.Next(0, 256);

                TSource[] a = new TSource[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSource(77777, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = NewTSource(targetInt, checkForOutOfRangeAccess);
                }

                Span<TSource> span = new Span<TSource>(a, GuardLength, length);
                int idx = MemoryExt.IndexOfNotEqual(span, NewTValue(targetInt, checkForOutOfRangeAccess));
                Assert.Equal(-1, idx);
            }
        }
    }

    public class Span_IndexOfNotEqual_intEE: Span_IndexOfNotEqual<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_IndexOfNotEqual_intEO: Span_IndexOfNotEqual<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_IndexOfNotEqual_intOE: Span_IndexOfNotEqual<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class Span_IndexOfNotEqual_intOO: Span_IndexOfNotEqual<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class Span_IndexOfNotEqual_stringEE: Span_IndexOfNotEqual<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfNotEqual_stringEO: Span_IndexOfNotEqual<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfNotEqual_stringOE: Span_IndexOfNotEqual<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_IndexOfNotEqual_stringOO: Span_IndexOfNotEqual<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
