using System;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class Span_LastIndexOfEqualAny<T, TSource, TValue>
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
            var sp = new Span<TSource>(Array.Empty<TSource>());

            var values = new ReadOnlySpan<TValue>(new TValue[] { default, default, default, default });
            int idx = MemoryExt.LastIndexOfEqualAny(sp, values);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { });
            idx = MemoryExt.LastIndexOfEqualAny(sp, values);
            Assert.Equal(-1, idx);

            sp = new Span<TSource>(new TSource[] { default, default, default, default });
            idx = MemoryExt.LastIndexOfEqualAny(sp, values);
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

            for (int length = 0; length < 100; length++)
            {
                var a = new TSource[length];
                var span = new Span<TSource>(a);

                var values = new ReadOnlySpan<TValue>(new TValue[] 
                    { NewTValue(99), NewTValue(98), NewTValue(0), default });
                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(length - 1, idx);

                values = new ReadOnlySpan<TValue>(new TValue[] { NewTValue(99), NewTValue(98) });
                idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            var rnd = new Random(42);
            for (int length = 1; length < 100; length++)
            {
                var ai = new int[length];
                var a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    ai[i] = i + 1;
                    a[i] = NewTSource(ai[i]);
                }
                var span = new Span<TSource>(a);
                TValue[] v;
                ReadOnlySpan<TValue> values;

                v = new TValue[] { NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0) };
                values = new ReadOnlySpan<TValue>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    v[0] = NewTValue(ai[targetIndex]);
                    int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                    Assert.Equal(targetIndex, idx);
                }

                v = new TValue[4];
                values = new ReadOnlySpan<TValue>(v);
                for (int targetIndex = 3; targetIndex < length; targetIndex++)
                {
                    int index = rnd.Next(0, 4);
                    v[0] = NewTValue(ai[targetIndex - (index + 0) % 4]);
                    v[1] = NewTValue(ai[targetIndex - (index + 1) % 4]);
                    v[2] = NewTValue(ai[targetIndex - (index + 2) % 4]);
                    v[3] = NewTValue(ai[targetIndex - (index + 3) % 4]);
                    int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                    Assert.Equal(targetIndex, idx);
                }

                v = new TValue[] { NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0) };
                values = new ReadOnlySpan<TValue>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    v[v.Length - 1] = NewTValue(ai[targetIndex]);
                    int idx = MemoryExt.LastIndexOfEqualAny(span, values);
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
                var a = new TSource[length];
                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    if (i == expectedIndex)
                    {
                        a[i] = NewTSource(0);
                        continue;
                    }
                    a[i] = NewTSource(255);
                }
                var span = new Span<TSource>(a);

                var targets = new TValue[length * 2];
                for (int i = 0; i < targets.Length; i++)
                {
                    if (i == length + 1)
                    {
                        targets[i] = NewTValue(0);
                        continue;
                    }
                    targets[i] = NewTValue(rnd.Next(1, 255));
                }

                var values = new ReadOnlySpan<TValue>(targets);
                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(expectedIndex, idx);
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            var rnd = new Random(44);
            for (int length = 1; length < 100; length++)
            {
                var a = new TSource[length];
                var targets = new TValue[length];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSource(0);
                }
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i] = NewTValue(rnd.Next(1, 256));
                }
                var span = new Span<TSource>(a);

                var values = new ReadOnlySpan<TValue>(targets);
                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, idx);

                values = new ReadOnlySpan<TValue>();
                idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestNoMatchValuesLarger()
        {
            var rnd = new Random(45);
            for (int length = 1; length < 100; length++)
            {
                var a = new TSource[length];
                var targets = new TValue[length * 2];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = NewTSource(0);
                }
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i] = NewTValue(rnd.Next(1, 256));
                }
                var span = new Span<TSource>(a);
                var values = new ReadOnlySpan<TValue>(targets);

                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 5; length < 100; length++)
            {
                var a = new TSource[length];
                for (int i = 0; i < length; i++)
                {
                    int val = i + 1;
                    a[i] = NewTSource(val == 200 ? 201 : val);
                }

                a[0] = NewTSource(200);
                a[1] = NewTSource(200);
                a[2] = NewTSource(200);
                a[3] = NewTSource(200);
                a[4] = NewTSource(200);

                var span = new Span<TSource>(a);
                var values = new ReadOnlySpan<TValue>(new TValue[] { NewTValue(200), NewTValue(200), NewTValue(200),
                    NewTValue(200), NewTValue(200), NewTValue(200), NewTValue(200), NewTValue(200), NewTValue(200) });
                int idx = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(4, idx);
            }
        }

        //[Fact]
        //public void OnNoMatchMakeSureEveryElementIsCompared()
        //{
        //    for (int length = 0; length < 100; length++)
        //    {
        //        TLog<T> log = new TLog<T>();

        //        TSource[] a = new TSource[length];
        //        T[] b = new T[length];
        //        for (int i = 0; i < length; i++)
        //        {
        //            a[i] = NewTSource(10 * (i + 1), log.Add);
        //            b[i] = NewT(10 * (i + 1));
        //        }
        //        Span<TSource> span = new Span<TSource>(a);
        //        int idx = MemoryExt.IndexOfEqual(span, NewTValue(9999, log.Add));
        //        Assert.Equal(-1, idx);

        //        // Since we asked for a non-existent value, make sure each element of the array was compared once.
        //        // (Strictly speaking, it would not be illegal for IndexOfEqual to compare an element more than once but
        //        // that would be a non-optimal implementation and a red flag. So we'll stick with the stricter test.)
        //        Assert.Equal(a.Length, log.Count);
        //        foreach (T elem in b)
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
                var a = new TSource[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewTSource(0);
                a[0] = NewTSource(99);
                a[length + 1] = NewTSource(98);
                var span = new Span<TSource>(a, 1, length);
                var values = new ReadOnlySpan<TValue>(new TValue[] { NewTValue(99), NewTValue(98), NewTValue(99),
                    NewTValue(98), NewTValue(99), NewTValue(98) });
                int index = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, index);
            }

            for (int length = 0; length < 100; length++)
            {
                var a = new TSource[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewTSource(0);
                a[0] = NewTSource(99);
                a[length + 1] = NewTSource(99);
                var span = new Span<TSource>(a, 1, length);
                var values = new ReadOnlySpan<TValue>(new TValue[] { NewTValue(99), NewTValue(99), NewTValue(99),
                    NewTValue(99), NewTValue(99), NewTValue(99) });
                int index = MemoryExt.LastIndexOfEqualAny(span, values);
                Assert.Equal(-1, index);
            }
        }
    }

    public class Span_LastIndexOfEqualAny_intEE : Span_LastIndexOfEqualAny<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
    }

    public class Span_LastIndexOfEqualAny_intEO : Span_LastIndexOfEqualAny<int, TEquatable<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
    }

    public class Span_LastIndexOfEqualAny_intOE : Span_LastIndexOfEqualAny<int, TObject<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
    }

    public class Span_LastIndexOfEqualAny_intOO : Span_LastIndexOfEqualAny<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
    }

    public class Span_LastIndexOfEqualAny_stringEE : 
        Span_LastIndexOfEqualAny<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_LastIndexOfEqualAny_stringEO : 
        Span_LastIndexOfEqualAny<string, TEquatable<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
    }

    public class Span_LastIndexOfEqualAny_stringOE : 
        Span_LastIndexOfEqualAny<string, TObject<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class Span_LastIndexOfEqualAny_stringOO : Span_LastIndexOfEqualAny<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
    }
}
