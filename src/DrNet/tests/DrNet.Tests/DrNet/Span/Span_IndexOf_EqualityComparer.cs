using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class Span_IndexOf_EqualityComparer<T>
    {
        public abstract T CreateValue(int value);

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
        public void ZeroLengthIndexOf()
        {
            Span<T> sp = new Span<T>(Array.Empty<T>());
            int idx = MemoryExt.IndexOf(sp, CreateValue(0), EqualityComparer);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void DefaultFilledIndexOf()
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

            T target0 = default;

            for (int length = 1; length < 32; length++)
            {
                T[] a = new T[length];
                Span<T> span = new Span<T>(a);

                int idx = MemoryExt.IndexOf(span, target0, EqualityComparer);
                Assert.Equal(0, idx);
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
                    a[i] = CreateValue(10 * (i + 1));
                }
                Span<T> span = new Span<T>(a);

                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    T target = a[targetIndex];
                    int idx = MemoryExt.IndexOf(span, target, EqualityComparer);
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
                T target = CreateValue(targetInt);
                for (int i = 0; i < length; i++)
                {
                    T val = CreateValue(i + 1);
                    a[i] = EqualityComparer(val, target) ? CreateValue(targetInt + 1) : val;
                }
                Span<T> span = new Span<T>(a);

                int idx = MemoryExt.IndexOf(span, target, EqualityComparer);
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
                    a[i] = CreateValue(10 * (i + 1));
                }

                a[length - 1] = CreateValue(5555);
                a[length - 2] = CreateValue(5555);

                Span<T> span = new Span<T>(a);
                int idx = MemoryExt.IndexOf(span, CreateValue(5555), EqualityComparer);
                Assert.Equal(length - 2, idx);
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
                    a[i] = CreateValue(10 * (i + 1));
                }
                Span<T> span = new Span<T>(a);
                int idx = MemoryExt.IndexOf(span, CreateValue(9999), EqualityComparer);
                Assert.Equal(-1, idx);

                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for IndexOf to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(a.Length, log.Count);
                foreach (T elem in a)
                {
                    int numCompares = log.CountCompares(elem, CreateValue(9999));
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }
            }
        }

        [Fact]
        public void MakeSureNoChecksGoOutOfRange()
        {
            T GuardValue = CreateValue(77777);
            const int GuardLength = 50;

            Action<T, T> checkForOutOfRangeAccess =
                delegate (T x, T y)
                {
                    if (EqualityComparer(x, GuardValue) || EqualityComparer(y, GuardValue))
                        throw new Exception("Detected out of range access in IndexOf()");
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
                    a[GuardLength + i] = new TEquatable<T>(CreateValue(10 * (i + 1)), checkForOutOfRangeAccess);
                }

                Span<TEquatable<T>> span = new Span<TEquatable<T>>(a, GuardLength, length);
                int idx = MemoryExt.IndexOf(span, new TEquatable<T>(CreateValue(9999), checkForOutOfRangeAccess), EqualityComparer);
                Assert.Equal(-1, idx);
            }
        }
    }

    public class Span_IndexOf_EqualityComparer_int: Span_IndexOf_EqualityComparer<int>
    {
        public override int CreateValue(int value) => value;
    }

    public class Span_IndexOf_EqualityComparer_string: Span_IndexOf_EqualityComparer<string>
    {
        public override string CreateValue(int value) => value.ToString();
    }

}
