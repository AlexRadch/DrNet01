using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_LastIndexOfEqual<T, TSpan, TValue>
    {
        public abstract T NewT(int value);

        public abstract TSpan NewTSpan(int value, Action<T, T> onCompare = default);

        public abstract TValue NewTValue(int value, Action<T, T> onCompare = default);

        public bool EqualityComparer(T v1, T v2)
        {
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        public bool EqualityComparer(TSpan sValue, TValue vValue)
        {
            if (vValue is IEquatable<TSpan> vEquatable)
                return vEquatable.Equals(sValue);
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            return vValue.Equals(sValue);
        }

        public bool EqualityComparer(TValue vValue, TSpan sValue)
        {
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            if (vValue is IEquatable<TSpan> vEquatable)
                return vEquatable.Equals(sValue);
            return sValue.Equals(vValue);
        }

        [Fact]
        public void ZeroLengthIndexOf()
        {
            ReadOnlySpan<T> sp = new ReadOnlySpan<T>(Array.Empty<T>());
            int idx = MemoryExt.LastIndexOfEqual(sp, NewTValue(0, null));
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void DefaultFilledIndexOf()
        {
            try
            {
                if (!EqualityComparer(default(TSpan), default(TValue)))
                    return;
                if (!EqualityComparer(default(TValue), default(TSpan)))
                    return;
            }
            catch
            {
                return;
            }

            TValue target0 = default;

            for (int length = 1; length < 32; length++)
            {
                TSpan[] a = new TSpan[length];
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);

                int idx = MemoryExt.LastIndexOfEqual(span, target0);
                Assert.Equal(length - 1, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            for (int length = 0; length < 32; length++)
            {
                TSpan[] a = new TSpan[length];
                TValue[] b = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(10 * (i + 1));
                    b[i] = NewTValue(10 * (i + 1));
                }
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);

                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    TValue target = b[targetIndex];
                    int idx = MemoryExt.LastIndexOfEqual(span, target);
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
                TSpan[] a = new TSpan[length];
                int targetInt = rnd.Next(0, 256);
                TValue target = NewTValue(targetInt);
                for (int i = 0; i < length; i++)
                {
                    TSpan val = NewTSpan(i + 1);
                    a[i] = EqualityComparer(val, target) ? NewTSpan(targetInt + 1) : val;
                }
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);

                int idx = MemoryExt.LastIndexOfEqual(span, target);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 2; length < 32; length++)
            {
                TSpan[] a = new TSpan[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(10 * (i + 1));
                }

                a[0] = NewTSpan(5555);
                a[1] = NewTSpan(5555);

                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                int idx = MemoryExt.LastIndexOfEqual(span, NewTSpan(5555));
                Assert.Equal(1, idx);
            }
        }

        [Fact]
        public void OnNoMatchMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                TSpan[] a = new TSpan[length];
                T[] b = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(10 * (i + 1), log.Add);
                    b[i] = NewT(10 * (i + 1));
                }
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(9999, log.Add));
                Assert.Equal(-1, idx);

                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for LastIndexOfEqual to compare an element more than once but
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
                        throw new Exception("Detected out of range access in LastIndexOfEqual()");
                };

            for (int length = 0; length < 100; length++)
            {
                TSpan[] a = new TSpan[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSpan(77777, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = NewTSpan(10 * (i + 1), checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a, GuardLength, length);
                int idx = MemoryExt.LastIndexOfEqual(span, NewTValue(9999, checkForOutOfRangeAccess));
                Assert.Equal(-1, idx);
            }
        }
    }

    public class ReadOnlySpan_LastIndexOfEqual_intEE: ReadOnlySpan_LastIndexOfEqual<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_intEO: ReadOnlySpan_LastIndexOfEqual<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_intOE: ReadOnlySpan_LastIndexOfEqual<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_intOO: ReadOnlySpan_LastIndexOfEqual<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_stringEE: ReadOnlySpan_LastIndexOfEqual<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_stringEO: ReadOnlySpan_LastIndexOfEqual<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_stringOE: ReadOnlySpan_LastIndexOfEqual<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfEqual_stringOO: ReadOnlySpan_LastIndexOfEqual<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
