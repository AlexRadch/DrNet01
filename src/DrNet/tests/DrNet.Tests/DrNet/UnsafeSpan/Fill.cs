using System;
using System.Linq;

using Xunit;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Fill<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            uSpan.Fill(NextT(new Random(40)));
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            uSpan.Fill(NextT(new Random(41)));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            var rnd = new Random(42 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();
            T[] t2 = t.ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);

                    uSpan.Fill(default);
                    for (var i = 0; i < length; i++)
                    {
                        Assert.Equal(default, span[i]);
                        Assert.Equal(default, uSpan[i]);
                    }

                    T item = NextNotEqualT(rnd, default);
                    uSpan.Fill(item);
                    for (var i = 0; i < length; i++)
                    {
                        Assert.Equal(item, span[i]);
                        Assert.Equal(item, uSpan[i]);
                    }
                }
            }

            Assert.True(t2.AsReadOnlySpan(0, guardLength).EqualsToSeq(t.AsReadOnlySpan(0, guardLength)));
            Assert.True(t2.AsReadOnlySpan(guardLength + length, guardLength).EqualsToSeq(
                t.AsReadOnlySpan(guardLength + length, guardLength)));
        }
    }

    public sealed class Fill_byte : Fill<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Fill_char : Fill<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Fill_int : Fill<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Fill_string : Fill<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Fill_Tuple : Fill<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Fill_ValueTuple : Fill<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
