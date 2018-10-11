using System;
using System.Runtime.CompilerServices;

using Xunit;

using DrNet.Unsafe;
using System.Linq;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class ToString<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;
            if (typeof(T) == typeof(char))
            {
                Assert.Equal("", uSpan.ToString());
                Assert.Equal("", urSpan.ToString());
            }
            else
            {
                Assert.Equal($"DrNet.UnsafeSpan<{typeof(T).Name}>[0]", uSpan.ToString());
                Assert.Equal($"DrNet.UnsafeReadOnlySpan<{typeof(T).Name}>[0]", urSpan.ToString());
            }
        }

        [Fact]
        public void FromDefault()
        {
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(default);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(default);
            if (typeof(T) == typeof(char))
            {
                Assert.Equal("", uSpan.ToString());
                Assert.Equal("", urSpan.ToString());
            }
            else
            {
                Assert.Equal($"DrNet.UnsafeSpan<{typeof(T).Name}>[0]", uSpan.ToString());
                Assert.Equal($"DrNet.UnsafeReadOnlySpan<{typeof(T).Name}>[0]", urSpan.ToString());
            }
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

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    if (typeof(T) == typeof(char))
                    {
                        Assert.Equal(span.ToString(), uSpan.ToString());
                        Assert.Equal(span.ToString(), urSpan.ToString());
                    }
                    else
                    {
                        Assert.Equal($"DrNet.UnsafeSpan<{typeof(T).Name}>[{length}]", uSpan.ToString());
                        Assert.Equal($"DrNet.UnsafeReadOnlySpan<{typeof(T).Name}>[{length}]", urSpan.ToString());
                    }
                }
            }
        }
    }

    public sealed class ToString_byte : ToString<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class ToString_char : ToString<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class ToString_int : ToString<int>
    {
        protected override int NewT(int value) => value;

    }

    public sealed class ToString_string : ToString<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class ToString_Tuple : ToString<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class ToString_ValueTuple : ToString<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
