using System;
using DrNet;
using Xunit;

namespace Tests.DrNet
{
    public delegate int IndexOfFunc<T>(Span<T> span, T value);

    public abstract class MemoryExtensionsTests_IndexOfEqual<T>
    {
        public IndexOfFunc<T> IndexOf { get; private set; }

        public MemoryExtensionsTests_IndexOfEqual(IndexOfFunc<T> indexOf)
        {
            IndexOf = indexOf;
        }

        public abstract T CreateValue(int value);

        [Fact]
        public void ZeroLengthIndexOf()
        {
            Span<T> sp = new Span<T>(Array.Empty<T>());
            int idx = IndexOf(sp, CreateValue(0));
            Assert.Equal(-1, idx);
        }

        [Fact]
        public void TestMatch()
        {
            for (int length = 0; length < 32; length++)
            {
                T[] a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = CreateValue(2 * (i + 1));
                }
                Span<T> span = new Span<T>(a);

                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    T target = a[targetIndex];
                    int idx = IndexOf(span, target);
                    Assert.Equal(targetIndex, idx);
                }
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
                    a[i] = CreateValue(2 * (i + 1));
                }

                a[length - 1] = CreateValue(55);
                a[length - 2] = CreateValue(55);

                Span<T> span = new Span<T>(a);
                int idx = span.IndexOfEqual(CreateValue(55));
                Assert.Equal(length - 2, idx);
            }
        }

        [Fact]
        public static void OnNoMatchMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();

                TInt[] a = new TInt[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = new TInt(10 * (i + 1), log);
                }
                Span<TInt> span = new Span<TInt>(a);
                int idx = span.IndexOfEqual(new TInt(9999, log));
                Assert.Equal(-1, idx);

                // Since we asked for a non-existent value, make sure each element of the array was compared once.
                // (Strictly speaking, it would not be illegal for IndexOfEqual to compare an element more than once but
                // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
                Assert.Equal(a.Length, log.Count);
                foreach (TInt elem in a)
                {
                    int numCompares = log.CountCompares(elem.Value, 9999);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem.Value}.");
                }
            }
        }

        [Fact]
        public static void MakeSureNoChecksGoOutOfRange()
        {
            const int GuardValue = 77777;
            const int GuardLength = 50;

            Action<int, int> checkForOutOfRangeAccess =
                delegate (int x, int y)
                {
                    if (x == GuardValue || y == GuardValue)
                        throw new Exception("Detected out of range access in IndexOfEqual()");
                };

            for (int length = 0; length < 100; length++)
            {
                TInt[] a = new TInt[GuardLength + length + GuardLength];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = new TInt(GuardValue, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    a[GuardLength + i] = new TInt(10 * (i + 1), checkForOutOfRangeAccess);
                }

                Span<TInt> span = new Span<TInt>(a, GuardLength, length);
                int idx = span.IndexOfEqual(new TInt(9999, checkForOutOfRangeAccess));
                Assert.Equal(-1, idx);
            }
        }
    }
}
