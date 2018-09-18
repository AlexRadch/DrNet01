﻿using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_LastIndexOfNotEqual<T, TSpan, TValue>
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
        public void ZeroLength()
        {
            ReadOnlySpan<T> sp = new ReadOnlySpan<T>(Array.Empty<T>());
            int idx = MemoryExt.LastIndexOfNotEqual(sp, NewTValue(0, null));
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void DefaultFilled()
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

            for (int length = 1; length < 32; length++)
            {
                TSpan[] a = new TSpan[length];
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);

                int idx = MemoryExt.LastIndexOfNotEqual(span, default(TValue));
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

                    TSpan[] a = new TSpan[length];
                    for (int i = 0; i < length; i++)
                    {
                        a[i] = NewTSpan(targetInt1);
                    }

                    a[targetIndex] = NewTSpan(targetInt2);

                    ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                    int idx = MemoryExt.LastIndexOfNotEqual(span, NewTValue(targetInt1));
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

                TSpan[] a = new TSpan[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(targetInt);
                }

                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                int idx = MemoryExt.LastIndexOfNotEqual(span, NewTValue(targetInt));
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

                TSpan[] a = new TSpan[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(targetInt1);
                }

                a[0] = NewTSpan(targetInt2);
                a[1] = NewTSpan(targetInt2);

                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                int idx = MemoryExt.LastIndexOfNotEqual(span, NewTSpan(targetInt1));
                Assert.Equal(1, idx);
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

                TSpan[] a = new TSpan[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewTSpan(targetInt, log.Add);
                }
                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a);
                int idx = MemoryExt.LastIndexOfNotEqual(span, NewTValue(targetInt, log.Add));
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

                TSpan[] a = new TSpan[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSpan(77777, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = NewTSpan(targetInt, checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TSpan> span = new ReadOnlySpan<TSpan>(a, GuardLength, length);
                int idx = MemoryExt.LastIndexOfNotEqual(span, NewTValue(targetInt, checkForOutOfRangeAccess));
                Assert.Equal(-1, idx);
            }
        }
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_intEE: ReadOnlySpan_LastIndexOfNotEqual<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_intEO: ReadOnlySpan_LastIndexOfNotEqual<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSpan(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_intOE: ReadOnlySpan_LastIndexOfNotEqual<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_intOO: ReadOnlySpan_LastIndexOfNotEqual<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSpan(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_stringEE: ReadOnlySpan_LastIndexOfNotEqual<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_stringEO: ReadOnlySpan_LastIndexOfNotEqual<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSpan(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_stringOE: ReadOnlySpan_LastIndexOfNotEqual<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqual_stringOO: ReadOnlySpan_LastIndexOfNotEqual<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSpan(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => new TObject<string>(value.ToString(), onCompare);
    }
}
