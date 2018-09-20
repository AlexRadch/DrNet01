using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_LastIndexOfNotEqualAll<T, TSource, TValue>
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
            var sp = new ReadOnlySpan<TSource>(Array.Empty<TSource>());

            var values = new ReadOnlySpan<TValue>(new TValue[] { default, default, default, default });
            int idx = MemoryExt.LastIndexOfNotEqualAll(sp, values);
            Assert.Equal(-1, idx);

            values = new ReadOnlySpan<TValue>(new TValue[] { });
            idx = MemoryExt.LastIndexOfNotEqualAll(sp, values);
            Assert.Equal(-1, idx);

            sp = new ReadOnlySpan<TSource>(new TSource[] { default, default, default, default });
            idx = MemoryExt.LastIndexOfNotEqualAll(sp, values);
            Assert.Equal(3, idx);
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

            for (int length = 1; length < 100; length++)
            {
                var a = new TSource[length];
                var span = new ReadOnlySpan<TSource>(a);

                var values = new ReadOnlySpan<TValue>(new TValue[] 
                    { NewTValue(99), NewTValue(98), NewTValue(0), default});
                int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(-1, idx);

                values = new ReadOnlySpan<TValue>(new TValue[] { NewTValue(99), NewTValue(98)});
                idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(length - 1, idx);
            }
        }

        [Fact]
        public void TestMatch()
        {
            var rnd = new Random(42);
            for (int length = 1; length < 100; length++)
            {
                //var ai = new int[length];
                var a = new TSource[length];
                var v = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    //ai[i] = i + 1;
                    a[i] = NewTSource(i + 1);
                    v[length - i - 1] = NewTValue(i + 1);
                }
                var span = new ReadOnlySpan<TSource>(a);

                var values = new ReadOnlySpan<TValue>(v);
                for (int targetIndex = 0; targetIndex < length; targetIndex++)
                {
                    TValue temp = v[length - targetIndex - 1];
                    v[length - targetIndex - 1] = NewTValue(0);

                    int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                    Assert.Equal(targetIndex, idx);

                    v[length - targetIndex - 1] = temp;
                }

                for (int targetIndex = 3; targetIndex < length; targetIndex++)
                {
                    TValue temp1 = v[length - targetIndex - 1];
                    TValue temp2 = v[length - targetIndex + 0];
                    TValue temp3 = v[length - targetIndex + 1];
                    TValue temp4 = v[length - targetIndex + 2];
                    v[length - targetIndex - 1] = NewTValue(0);
                    v[length - targetIndex + 0] = NewTValue(0);
                    v[length - targetIndex + 1] = NewTValue(0);
                    v[length - targetIndex + 2] = NewTValue(0);

                    int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                    Assert.Equal(targetIndex, idx);

                    v[length - targetIndex - 1] = temp1;
                    v[length - targetIndex + 0] = temp2;
                    v[length - targetIndex + 1] = temp3;
                    v[length - targetIndex + 2] = temp4;
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
                var targets = new TValue[length * 2];

                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    targets[i] = NewTValue(1);
                    if (i == expectedIndex)
                    {
                        a[i] = NewTSource(0);
                        targets[length * 2 - 1 - i] = NewTValue(1);
                    }
                    else
                    {
                        var intValue = rnd.Next(2, 255);
                        a[i] = NewTSource(intValue);
                        targets[length * 2 - 1 - i] = NewTValue(intValue);
                    }
                }
                var span = new ReadOnlySpan<TSource>(a);
                var values = new ReadOnlySpan<TValue>(targets);

                int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(expectedIndex, idx);
            }
        }

        [Fact]
        public void TestNoMatch()
        {
            var rnd = new Random(44);
            for (int length = 0; length < 100; length++)
            {
                var a = new TSource[length];
                var targets = new TValue[length];
                for (int i = 0; i < a.Length; i++)
                {
                    int intValue = rnd.Next(2, 256);
                    a[i] = NewTSource(intValue);
                    targets[length - i - 1] = NewTValue(intValue);
                }
                var span = new ReadOnlySpan<TSource>(a);
                var values = new ReadOnlySpan<TValue>(targets);

                int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
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

                int expectedIndex = length / 2;
                for (int i = 0; i < length; i++)
                {
                    targets[i] = NewTValue(1);

                    int intValue = rnd.Next(2, 256);
                    a[i] = NewTSource(intValue);
                    targets[length * 2 - 1 - i] = NewTValue(intValue);
                }
                var span = new ReadOnlySpan<TSource>(a);
                var values = new ReadOnlySpan<TValue>(targets);

                int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(-1, idx);
            }
        }

        [Fact]
        public void TestMultipleMatch()
        {
            for (int length = 5; length < 100; length++)
            {
                var a = new TSource[length];
                var targets = new TValue[length];
                for (int i = 0; i < length; i++)
                {
                    int val = i + 1;
                    if (val == 200)
                        val = 201;
                    a[i] = NewTSource(val);
                    targets[length - i - 1] = NewTValue(val);
                }

                a[0] = NewTSource(200);
                a[1] = NewTSource(200);
                a[2] = NewTSource(200);
                a[3] = NewTSource(200);
                a[4] = NewTSource(200);

                var span = new ReadOnlySpan<TSource>(a);
                var values = new ReadOnlySpan<TValue>(targets);
                int idx = MemoryExt.LastIndexOfNotEqualAll(span, values);
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
        //        ReadOnlySpan<TSource> span = new ReadOnlySpan<TSource>(a);
        //        int idx = MemoryExt.LastIndexOfNotEqualAll(span, NewTValue(9999, log.Add));
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
                a[1] = NewTSource(99);
                var span = new ReadOnlySpan<TSource>(a, 2, length);
                var values = new ReadOnlySpan<TValue>(new TValue[]
                    { NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0) });

                int index = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(-1, index);
            }

            for (int length = 0; length < 100; length++)
            {
                var a = new TSource[length + 2];
                for (int i = 0; i < a.Length; i++)
                    a[i] = NewTSource(0);
                a[length + 0] = NewTSource(99);
                a[length + 1] = NewTSource(99);
                var span = new ReadOnlySpan<TSource>(a, 0, length);
                var values = new ReadOnlySpan<TValue>(new TValue[]
                    { NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0), NewTValue(0) });

                int index = MemoryExt.LastIndexOfNotEqualAll(span, values);
                Assert.Equal(-1, index);
            }

        }
    }

    public class ReadOnlySpan_LastIndexOfNotEqualAll_intEE : 
        ReadOnlySpan_LastIndexOfNotEqualAll<int, TEquatable<int>, TEquatable<int>>
    {
        public override int NewT(int value) => value;
        public override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        public override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TEquatable<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqualAll_intEO : 
        ReadOnlySpan_LastIndexOfNotEqualAll<int, TEquatable<int>, TObject<int>>
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

    public class ReadOnlySpan_LastIndexOfNotEqualAll_intOE : 
        ReadOnlySpan_LastIndexOfNotEqualAll<int, TObject<int>, TEquatable<int>>
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

    public class ReadOnlySpan_LastIndexOfNotEqualAll_intOO : 
        ReadOnlySpan_LastIndexOfNotEqualAll<int, TObject<int>, TObject<int>>
    {
        public override int NewT(int value) => value;
        public override TObject<int> NewTSource(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
        public override TObject<int> NewTValue(int value, Action<int, int> onCompare) => 
            new TObject<int>(value, onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqualAll_stringEE : 
        ReadOnlySpan_LastIndexOfNotEqualAll<string, TEquatable<string>, TEquatable<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TEquatable<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
        public override TEquatable<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TEquatable<string>(value.ToString(), onCompare);
    }

    public class ReadOnlySpan_LastIndexOfNotEqualAll_stringEO : 
        ReadOnlySpan_LastIndexOfNotEqualAll<string, TEquatable<string>, TObject<string>>
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

    public class ReadOnlySpan_LastIndexOfNotEqualAll_stringOE : 
        ReadOnlySpan_LastIndexOfNotEqualAll<string, TObject<string>, TEquatable<string>>
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

    public class ReadOnlySpan_LastIndexOfNotEqualAll_stringOO : 
        ReadOnlySpan_LastIndexOfNotEqualAll<string, TObject<string>, TObject<string>>
    {
        public override string NewT(int value) => value.ToString();
        public override TObject<string> NewTSource(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
        public override TObject<string> NewTValue(int value, Action<string, string> onCompare) => 
            new TObject<string>(value.ToString(), onCompare);
    }
}
