using System;
using System.Runtime.CompilerServices;

using Xunit;

using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class ImplicitUnsafeReadOnlySpan<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = uSpan;

            Assert.Equal(default, urSpan);
        }

        [Fact]
        public void FromDefault()
        {
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(default);
            UnsafeReadOnlySpan<T> urSpan = uSpan;

            Assert.Equal(default, urSpan);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            var rnd = new Random(41 * (length + 1));
            const int guardLength = 50;
            
            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = uSpan;

                    Assert.Equal(uSpan, urSpan);
                }
            }
        }
    }

    public sealed class ImplicitUnsafeReadOnlySpan_byte : ImplicitUnsafeReadOnlySpan<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class ImplicitUnsafeReadOnlySpan_char : ImplicitUnsafeReadOnlySpan<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class ImplicitUnsafeReadOnlySpan_int : ImplicitUnsafeReadOnlySpan<int>
    {
        protected override int NewT(int value) => value;

    }

    public sealed class ImplicitUnsafeReadOnlySpan_string : ImplicitUnsafeReadOnlySpan<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class ImplicitUnsafeReadOnlySpan_Tuple : ImplicitUnsafeReadOnlySpan<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class ImplicitUnsafeReadOnlySpan_ValueTuple : ImplicitUnsafeReadOnlySpan<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
