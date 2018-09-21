using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class IndexOfNotEqualAll_EqualityComparer<T>
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
            int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfNotEqualAllValueComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<T>(new T[] { });
            idx = MemoryExt.IndexOfNotEqualAllSourceComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);
            idx = MemoryExt.IndexOfNotEqualAllValueComparer(sp, values, EqualityComparer);
            Assert.Equal(-1, idx);

            sp = new ReadOnlySpan<T>(new T[] { default, default, default, default });
            idx = MemoryExt.IndexOfNotEqualAllSourceComparer(sp, values, EqualityComparer);
            Assert.Equal(0, idx);
            idx = MemoryExt.IndexOfNotEqualAllValueComparer(sp, values, EqualityComparer);
            Assert.Equal(0, idx);
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

                var values = new ReadOnlySpan<T>(new T[] { NewT(99), NewT(98), NewT(0), default });
                int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);

                values = new ReadOnlySpan<T>(new T[] { NewT(99), NewT(98) });
                idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(0, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                Assert.Equal(0, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            for (int length = 1; length < 100; length++)
            {
                var a = new T[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = NewT(i + 1);
                }
                var span = new ReadOnlySpan<T>(a);

                T[] v = a.AsReadOnlySpan().ToArray();
                Array.Reverse(v);
                ReadOnlySpan<T> values = new ReadOnlySpan<T>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    T temp = v[length - targetIndex - 1];
                    v[length - targetIndex - 1] = NewT(0);

                    int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);

                    v[length - targetIndex - 1] = temp;
                }

                values = new ReadOnlySpan<T>(v);
                for (int targetIndex = 0; targetIndex < length - 3; targetIndex++)
                {
                    T temp1 = v[length - targetIndex - 1];
                    T temp2 = v[length - targetIndex - 2];
                    T temp3 = v[length - targetIndex - 3];
                    T temp4 = v[length - targetIndex - 4];
                    v[length - targetIndex - 1] = NewT(0);
                    v[length - targetIndex - 2] = NewT(0);
                    v[length - targetIndex - 3] = NewT(0);
                    v[length - targetIndex - 4] = NewT(0);

                    int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);
                    idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                    Assert.Equal(targetIndex, idx);

                    v[length - targetIndex - 1] = temp1;
                    v[length - targetIndex - 2] = temp2;
                    v[length - targetIndex - 3] = temp3;
                    v[length - targetIndex - 4] = temp4;
                }
            }
        }

        [Fact]
        public void TestMatchValuesLarger()
        {
            var rnd = new Random(43);
            for (int length = 1; length < 100; length++)
            {
                var a = new T[length];
                var targets = new T[length * 2];

                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    targets[i] = NewT(1);
                    if (i == expectedIndex)
                    {
                        a[i] = NewT(0);
                        targets[length * 2 - 1 - i] = NewT(1);
                    }
                    else
                    {
                        a[i] = NewT(rnd.Next(2, 255));
                        targets[length * 2 - 1 - i] = a[i];
                    }
                }
                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);

                int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(expectedIndex, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                Assert.Equal(expectedIndex, idx);
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            var rnd = new Random(44);
            for (int length = 0; length < 100; length++)
            {
                var a = new T[length];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewT(rnd.Next(2, 256));
                }
                T[] targets = a.AsReadOnlySpan().ToArray();
                Array.Reverse(targets);
                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);

                int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
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

                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    targets[i] = NewT(1);
                    a[i] = NewT(rnd.Next(2, 255));
                    targets[length * 2 - 1 - i] = a[i];
                }
                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);

                int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
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
                T[] targets = a.AsReadOnlySpan().ToArray();
                Array.Reverse(targets);

                a[length - 1] = NewT(200);
                a[length - 2] = NewT(200);
                a[length - 3] = NewT(200);
                a[length - 4] = NewT(200);
                a[length - 5] = NewT(200);

                var span = new ReadOnlySpan<T>(a);
                var values = new ReadOnlySpan<T>(targets);
                int idx = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(length - 5, idx);
                idx = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
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
            for (int length = 0; length < 100; length++)
            {
                var a = new T[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewT(0);
                a[0] = NewT(99);
                a[1] = NewT(99);
                var span = new ReadOnlySpan<T>(a, 2, length);
                var values = new ReadOnlySpan<T>(new T[] { NewT(0), NewT(0), NewT(0), NewT(0), NewT(0), NewT(0) });
                int index = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
                index = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
            }

            for (int length = 0; length < 100; length++)
            {
                var a = new T[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewT(0);
                a[length + 0] = NewT(99);
                a[length + 1] = NewT(99);
                var span = new ReadOnlySpan<T>(a, 0, length);
                var values = new ReadOnlySpan<T>(new T[] { NewT(0), NewT(0), NewT(0), NewT(0), NewT(0), NewT(0) });
                int index = MemoryExt.IndexOfNotEqualAllSourceComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
                index = MemoryExt.IndexOfNotEqualAllValueComparer(span, values, EqualityComparer);
                Assert.Equal(-1, index);
            }
        }
    }

    public class IndexOfNotEqualAll_EqualityComparer_int : IndexOfNotEqualAll_EqualityComparer<int>
    {
        public override int NewT(int value) => value;
    }

    public class IndexOfNotEqualAll_EqualityComparer_string : IndexOfNotEqualAll_EqualityComparer<string>
    {
        public override string NewT(int value) => value.ToString();
    }

}
