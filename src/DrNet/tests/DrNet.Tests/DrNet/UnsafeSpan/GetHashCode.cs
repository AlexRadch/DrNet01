using System;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class GetHashCode<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpanL = default;
            UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(default);
            UnsafeReadOnlySpan<T> urSpanL = default;
            UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(default);

            Assert.Equal(uSpanL.GetHashCode(), uSpanR.GetHashCode());
            Assert.Equal(urSpanL.GetHashCode(), urSpanR.GetHashCode());

            Assert.Equal(uSpanL.GetHashCode(), urSpanR.GetHashCode());
            Assert.Equal(urSpanL.GetHashCode(), uSpanR.GetHashCode());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void EqualityTrue(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(span);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(rspan);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(rspan);

                    Assert.Equal(uSpanL.GetHashCode(), uSpanR.GetHashCode());
                    Assert.Equal(urSpanL.GetHashCode(), urSpanR.GetHashCode());

                    Assert.Equal(uSpanL.GetHashCode(), urSpanR.GetHashCode());
                    Assert.Equal(urSpanL.GetHashCode(), uSpanR.GetHashCode());
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void IncludesLength(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + 1 + guardLength];

            unsafe
            {
                Span<T> spanL = new Span<T>(t, guardLength, length + 1);
                Span<T> spanR = new Span<T>(t, guardLength, length);
                ReadOnlySpan<T> rspanL = new ReadOnlySpan<T>(t, guardLength, length + 1);
                ReadOnlySpan<T> rspanR = new ReadOnlySpan<T>(t, guardLength, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(spanL))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(spanL);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(spanR);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(spanL);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(spanR);

                    Assert.NotEqual(uSpanL.GetHashCode(), uSpanR.GetHashCode());
                    Assert.NotEqual(urSpanL.GetHashCode(), urSpanR.GetHashCode());

                    Assert.NotEqual(uSpanL.GetHashCode(), urSpanR.GetHashCode());
                    Assert.NotEqual(urSpanL.GetHashCode(), uSpanR.GetHashCode());
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void IncludesIncludesBase(int length)
        {
            const int guardLength = 50;

            T[] tL = new T[guardLength + length + guardLength];
            T[] tR = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> spanL = new Span<T>(tL, guardLength, length);
                Span<T> spanR = new Span<T>(tR, guardLength, length);
                ReadOnlySpan<T> rspanL = new ReadOnlySpan<T>(tL, guardLength, length);
                ReadOnlySpan<T> rspanR = new ReadOnlySpan<T>(tR, guardLength, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(spanL))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(spanL);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(spanR);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(spanL);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(spanR);

                    Assert.NotEqual(uSpanL.GetHashCode(), uSpanR.GetHashCode());
                    Assert.NotEqual(urSpanL.GetHashCode(), urSpanR.GetHashCode());

                    Assert.NotEqual(uSpanL.GetHashCode(), urSpanR.GetHashCode());
                    Assert.NotEqual(urSpanL.GetHashCode(), uSpanR.GetHashCode());
                }
            }
        }
    }

    public sealed class GetHashCode_byte : GetHashCode<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class GetHashCode_char : GetHashCode<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class GetHashCode_int : GetHashCode<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class GetHashCode_string : GetHashCode<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class GetHashCode_Tuple : GetHashCode<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class GetHashCode_ValueTuple : GetHashCode<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
