using System;
using System.Linq;

using Xunit;

using DrNet.Linq;
using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class IndexOf<T> : SpanTest<T>
    {
        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(41);

            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            Assert.Equal(-1, uSpan.IndexOf(default));
            Assert.Equal(-1, urSpan.IndexOf(default));

            Assert.Equal(-1, uSpan.IndexOf(NextNotEqualT(rnd, default)));
            Assert.Equal(-1, urSpan.IndexOf(NextNotEqualT(rnd, default)));

            uSpan = new UnsafeSpan<T>(default);
            urSpan = new UnsafeReadOnlySpan<T>(default);

            Assert.Equal(-1, uSpan.IndexOf(default));
            Assert.Equal(-1, urSpan.IndexOf(default));

            Assert.Equal(-1, uSpan.IndexOf(NextNotEqualT(rnd, default)));
            Assert.Equal(-1, urSpan.IndexOf(NextNotEqualT(rnd, default)));

            T[] targets = RepeatT(rnd).Take(4).ToArray();

            uSpan = new UnsafeSpan<T>(targets.AsSpan(1, 0));
            urSpan = new UnsafeReadOnlySpan<T>(targets.AsReadOnlySpan(2, 0));

            Assert.Equal(-1, uSpan.IndexOf(default));
            Assert.Equal(-1, urSpan.IndexOf(default));

            foreach (T target in targets)
            {
                Assert.Equal(-1, uSpan.IndexOf(target));
                Assert.Equal(-1, urSpan.IndexOf(target));
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void DefaultFilled(int length)
        {
            try
            {
                if (!EqualityCompareT(default, default))
                    return;
                if (!EqualityCompareT(default, default))
                    return;
            }
            catch
            {
                return;
            }

            var rnd = new Random(42 * (length + 1));
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    Assert.Equal(0, uSpan.IndexOf(default));
                    Assert.Equal(0, urSpan.IndexOf(default));

                    Assert.Equal(-1, uSpan.IndexOf(NextNotEqualT(rnd, default)));
                    Assert.Equal(-1, urSpan.IndexOf(NextNotEqualT(rnd, default)));
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMatch(int length)
        {
            var rnd = new Random(43 * (length + 1));
            const int guardLength = 50;

            T target = NextT(rnd);
            T[] t = Enumerable.Repeat(target, guardLength + length + guardLength).ToArray();
            WhereNotEqualT(RepeatT(rnd), target).Take(length).CopyTo(t.AsSpan(guardLength, length)).
                CheckOperationAllToEnd();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    for (int targetIndex = 0; targetIndex < length; targetIndex++)
                    {
                        T temp = urSpan[targetIndex];
                        uSpan[targetIndex] = target;

                        int idx = uSpan.IndexOf(target);
                        Assert.Equal(targetIndex, idx);
                        idx = urSpan.IndexOf(target);
                        Assert.Equal(targetIndex, idx);

                        uSpan[targetIndex] = temp;
                    }
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestNoMatch(int length)
        {
            var rnd = new Random(44 * (length + 1));
            const int guardLength = 50;

            T target = NextT(rnd);
            T[] t = Enumerable.Repeat(target, guardLength + length + guardLength).ToArray();
            WhereNotEqualT(RepeatT(rnd), target).Take(length).CopyTo(t.AsSpan(guardLength, length)).
                CheckOperationAllToEnd();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    int idx = uSpan.IndexOf(target);
                    Assert.Equal(-1, idx);
                    idx = urSpan.IndexOf(target);
                    Assert.Equal(-1, idx);
                }
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void TestMultipleMatch(int length)
        {
            var rnd = new Random(45 * (length + 1));
            const int guardLength = 50;

            T target = NextT(rnd);
            T[] t = Enumerable.Repeat(target, guardLength + length + guardLength).ToArray();
            WhereNotEqualT(RepeatT(rnd), target).Take(length).CopyTo(t.AsSpan(guardLength, length)).
                CheckOperationAllToEnd();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    for (int targetIndex = 0; targetIndex < length - 1; targetIndex++)
                    {
                        T temp0 = urSpan[targetIndex + 0];
                        T temp1 = urSpan[targetIndex + 1];

                        uSpan[targetIndex + 0] = uSpan[targetIndex + 1] = target;

                        int idx = uSpan.IndexOf(target);
                        Assert.Equal(targetIndex, idx);
                        idx = urSpan.IndexOf(target);
                        Assert.Equal(targetIndex, idx);

                        uSpan[targetIndex + 0] = temp0;
                        uSpan[targetIndex + 1] = temp1;
                    }
                }
            }
        }
    }

    public sealed class IndexOf_byte : IndexOf<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class IndexOf_char : IndexOf<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class IndexOf_int : IndexOf<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class IndexOf_string : IndexOf<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class IndexOf_Tuple : IndexOf<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class IndexOf_ValueTuple : IndexOf<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
