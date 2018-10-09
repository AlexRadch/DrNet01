using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class AsSpan<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            Assert.True(default == uSpan.AsSpan());
            Assert.True(default == urSpan.AsSpan());
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;

            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);
            Assert.True(span == uSpan.AsSpan());
            Assert.True(rspan == urSpan.AsSpan());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    Assert.True(span == uSpan.AsSpan());
                    Assert.True(span == urSpan.AsSpan());
                }
            }
        }
    }

    public sealed class AsSpan_byte : AsSpan<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class AsSpan_char : AsSpan<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class AsSpan_int : AsSpan<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class AsSpan_string : AsSpan<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class AsSpan_Tuple : AsSpan<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class AsSpan_ValueTuple : AsSpan<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
