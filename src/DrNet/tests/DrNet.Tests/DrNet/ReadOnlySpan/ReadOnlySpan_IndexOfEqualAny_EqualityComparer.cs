using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_IndexOfEqualAny_EqualityComparer<T>
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
            var sp = new ReadOnlySpan<T>(Array.Empty<T>());
            var values = new ReadOnlySpan<T>(new T[] { default, default, default, default });
            int idx = MemoryExt.IndexOfEqualAnySourceComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyValueComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<T>(new T[] { });
            idx = MemoryExt.IndexOfEqualAnySourceComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfEqualAnyValueComparer(sp, values, EqualityComparer);
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

            for (int length = 1; length < 100; length++)
            {
                var a = new T[length];
                var span = new ReadOnlySpan<T>(a);

                var values = new ReadOnlySpan<T>(new T[] { default, NewT(99), NewT(98), NewT(0) });
                int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(0, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(0, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            var rnd = new Random(42);
            for (int length = 0; length < 100; length++)
            {
                var a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(i + 1);
                }
                var span = new ReadOnlySpan<T>(a);
                T[] v;
                ReadOnlySpan<T> values;

                v = new T[] { NewT(0), NewT(0), NewT(0), NewT(0) };
                values = new ReadOnlySpan<T>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    v[0] = a[targetIndex];
                    int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                }

                v = new T[4];
                values = new ReadOnlySpan<T>(v);
                for (int targetIndex = 0; targetIndex < length - 3; targetIndex++)
                {
                    int index = rnd.Next(0, 4);
                    v[0] = a[targetIndex + (index + 0) % 4];
                    v[1] = a[targetIndex + (index + 1) % 4];
                    v[2] = a[targetIndex + (index + 2) % 4];
                    v[3] = a[targetIndex + (index + 3) % 4];
                    int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                }

                v = new T[] { NewT(0), NewT(0), NewT(0), NewT(0) };
                values = new ReadOnlySpan<T>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    v[v.Length - 1] = a[targetIndex];
                    int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                }
            }
        }

        [Fact]
        public void TestMatchValuesLarger()
        {
            var rnd = new Random(43);
            for (int length = 2; length < 100; length++)
            {
                var a = new T[length];
                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    if (i == expectedIndex)
                    {
                        a[i] = NewT(0);
                        continue;
                    }
                    a[i] = NewT(255);
                }
                var span = new ReadOnlySpan<T>(a);

                var targets = new T[length * 2];
                for (int i = 0; i < targets.Length; i++)
                {
                    if (i == length + 1)
                    {
                        targets[i] = NewT(0);
                        continue;
                    }
                    targets[i] = NewT(rnd.Next(1, 255));
                }

                var values = new ReadOnlySpan<T>(targets);
                int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(expectedIndex, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(expectedIndex, idx);
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            var rnd = new Random(44);
            for (int length = 1; length < 100; length++)
            {
                var a = new T[length];
                var targets = new T[length];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewT(0);
                }
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i] = NewT(rnd.Next(1, 256));
                }
                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);

                int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);

                values = new ReadOnlySpan<T>();
                idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
            }

        }

        [Fact]
        public void TestNoMatchValuesLarger()
        {
            var rnd = new Random(45);
            for (int length = 1; length < 100; length++)
            {
                var a = new T[length];
                var targets = new T[length * 2];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewT(0);
                }
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i] = NewT(rnd.Next(1, 256));
                }
                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);

                int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 5; length < 100; length++)
            {
                var a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    int val = i + 1;
                    a[i] = NewT(val == 200 ? 201 : val);
                }

                a[length - 1] = NewT(200);
                a[length - 2] = NewT(200);
                a[length - 3] = NewT(200);
                a[length - 4] = NewT(200);
                a[length - 5] = NewT(200);

                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(new T[] { NewT(200), NewT(200), NewT(200), NewT(200), NewT(200), NewT(200), NewT(200), NewT(200), NewT(200) });
                int idx = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(length - 5, idx);
                idx = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(length - 5, idx);
            }
        }

        //[Fact]
        //public void OnNoMatchMakeSureEveryElementIsCompared()
        //{
        //    for (int length = 0; length < 100; length++)
        //    {
        //        TLog<T> log = new TLog<T>();
        //        onCompare = log.Add;

        //        T[] a = new T[length];
        //        for (int i = 0; i < length; i++)
        //        {
        //            a[i] = NewT(10 * (i + 1));
        //        }
        //        ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);

        //        int idx = MemoryExt.IndexOfSourceComparer(span, NewT(9999), EqualityComparer);
        //        Assert.Equal(-1, idx);

        //        // Since we asked for a non-existent value, make sure each element of the array was compared once.
        //        // (Strictly speaking, it would not be illegal for IndexOf to compare an element more than once but
        //        // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
        //        Assert.Equal(a.Length, log.Count);
        //        foreach (T elem in a)
        //        {
        //            int numCompares = log.CountCompares(elem, NewT(9999));
        //            Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
        //        }

        //        log.Clear();
        //        idx = MemoryExt.IndexOfValueComparer(span, NewT(9999), EqualityComparer);
        //        Assert.Equal(-1, idx);

        //        // Since we asked for a non-existent value, make sure each element of the array was compared once.
        //        // (Strictly speaking, it would not be illegal for IndexOf to compare an element more than once but
        //        // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
        //        Assert.Equal(a.Length, log.Count);
        //        foreach (T elem in a)
        //        {
        //            int numCompares = log.CountCompares(elem, NewT(9999));
        //            Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
        //        }
        //    }
        //}

        [Fact]
        public void MakeSureNoChecksGoOutOfRange()
        {
            for (int length = 1; length < 100; length++)
            {
                var a = new T[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewT(0);
                a[0] = NewT(99);
                a[length + 1] = NewT(98);
                var span = new ReadOnlySpan<T>(a, 1, length - 1);
                var values = new ReadOnlySpan<T>(new T[] { NewT(99), NewT(98), NewT(99), NewT(98), NewT(99), NewT(98) });
                int index = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
                index = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
            }

            for (int length = 1; length < 100; length++)
            {
                var a = new T[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewT(0);
                a[0] = NewT(99);
                a[length + 1] = NewT(99);
                var span = new ReadOnlySpan<T>(a, 1, length - 1);
                var values = new ReadOnlySpan<T>(new T[] { NewT(99), NewT(99), NewT(99), NewT(99), NewT(99), NewT(99) });
                int index = MemoryExt.IndexOfEqualAnySourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
                index = MemoryExt.IndexOfEqualAnyValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
            }
        }
    }

    public class ReadOnlySpan_IndexOfEqualAny_EqualityComparer_int: ReadOnlySpan_IndexOfEqualAny_EqualityComparer<int>
    {
        public override int NewT(int value) => value;
    }

    public class ReadOnlySpan_IndexOfEqualAny_EqualityComparer_string: ReadOnlySpan_IndexOfEqualAny_EqualityComparer<string>
    {
        public override string NewT(int value) => value.ToString();
    }

}
