using System;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.UnSafe;
using System.Linq;

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

            Assert.Equal(-1, uSpan.IndexOf(NextNotDefaultT(rnd)));
            Assert.Equal(-1, urSpan.IndexOf(NextNotDefaultT(rnd)));

            uSpan = new UnsafeSpan<T>(default);
            urSpan = new UnsafeReadOnlySpan<T>(default);

            Assert.Equal(-1, uSpan.IndexOf(default));
            Assert.Equal(-1, urSpan.IndexOf(default));

            Assert.Equal(-1, uSpan.IndexOf(NextNotDefaultT(rnd)));
            Assert.Equal(-1, urSpan.IndexOf(NextNotDefaultT(rnd)));

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

                    Assert.Equal(-1, uSpan.IndexOf(NextNotDefaultT(rnd)));
                    Assert.Equal(-1, urSpan.IndexOf(NextNotDefaultT(rnd)));
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
