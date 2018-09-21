using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class LastIndexOf_EqualityComparer<T>
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
            ReadOnlySpan<T> sp = new ReadOnlySpan<T>(Array.Empty<T>());
            int idx = MemoryExt.LastIndexOfSourceComparer(sp, NewT(0), EqualityComparer);
            Assert.Equal(-1, idx);
            idx = MemoryExt.LastIndexOfValueComparer(sp, NewT(0), EqualityComparer);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void DefaultFilled()
        {
            try
            {
                if (!EqualityComparer(default(T), default))
                    return;
            }
            catch
            {
                return;
            }

            for (int length = 1; length < 32; length++)
            {
                T[] a = new T[length];
                ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

                int idx = MemoryExt.LastIndexOfSourceComparer(span, default(T), EqualityComparer);
                Assert.Equal(length - 1, idx);
                idx = MemoryExt.LastIndexOfValueComparer(span, default(T), EqualityComparer);
                Assert.Equal(length - 1, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            for (int length = 0; length < 32; length++)
            {
                T[] a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(10 * (i + 1));
                }
                ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    T target = a[targetIndex];
                    int idx = MemoryExt.LastIndexOfSourceComparer(span, target, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.LastIndexOfValueComparer(span, target, EqualityComparer);
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
                T[] a = new T[length];
                int targetInt = rnd.Next(0, 256);
                T target = NewT(targetInt);
                for (int i = 0; i < length; i++)
                {
                    T val = NewT(i + 1);
                    a[i] = EqualityComparer(val, target) ? NewT(targetInt + 1) : val;
                }
                ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

                int idx = MemoryExt.LastIndexOfSourceComparer(span, target, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.LastIndexOfValueComparer(span, target, EqualityComparer);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 2; length < 32; length++)
            {
                T[] a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(10 * (i + 1));
                }

                a[0] = NewT(5555);
                a[1] = NewT(5555);

                ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);
                int idx = MemoryExt.LastIndexOfSourceComparer(span, NewT(5555), EqualityComparer);
                Assert.Equal(1, idx);
                idx = MemoryExt.LastIndexOfValueComparer(span, NewT(5555), EqualityComparer);
                Assert.Equal(1, idx);
            }
        }

        [Fact]
        public void OnNoMatchMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();
                onCompare = log.Add;

                T[] a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(10 * (i + 1));
                }
                ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

                int idx = MemoryExt.LastIndexOfSourceComparer(span, NewT(9999), EqualityComparer);
                Assert.Equal(-1, idx);

                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for LastIndexOf to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(a.Length, log.Count);
                foreach (T elem in a)
                {
                    int numCompares = log.CountCompares(elem, NewT(9999));
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }

                log.Clear();
                idx = MemoryExt.LastIndexOfValueComparer(span, NewT(9999), EqualityComparer);
                Assert.Equal(-1, idx);

                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for LastIndexOf to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(a.Length, log.Count);
                foreach (T elem in a)
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
                        throw new Exception("Detected out of range access in LastIndexOf()");
                };

            for (int length = 0; length < 100; length++)
            {
                TEquatable<T>[] a = new TEquatable<T>[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = new TEquatable<T>(GuardValue, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = new TEquatable<T>(NewT(10 * (i + 1)), checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TEquatable<T>> span = new ReadOnlySpan<TEquatable<T>>(a, GuardLength, length);
                int idx = MemoryExt.LastIndexOfSourceComparer(span, new TEquatable<T>(NewT(9999), checkForOutOfRangeAccess), EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.LastIndexOfValueComparer(span, new TEquatable<T>(NewT(9999), checkForOutOfRangeAccess), EqualityComparer);
                Assert.Equal(-1, idx);
            }
        }
    }

    public class LastIndexOf_EqualityComparer_int : LastIndexOf_EqualityComparer<int>
    {
        public override int NewT(int value) => value;
    }

    public class LastIndexOf_EqualityComparer_string : LastIndexOf_EqualityComparer<string>
    {
        public override string NewT(int value) => value.ToString();
    }

}
