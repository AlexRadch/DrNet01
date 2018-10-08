using System;
using System.Runtime.InteropServices;

using Xunit;

using DrNet.UnSafe;


namespace DrNet.Tests.UnsafeSpan
{
    public abstract class ToArray<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            T[] copy = uSpan.ToArray();
            Assert.Empty(copy);

            copy = urSpan.ToArray();
            Assert.Empty(copy);
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

            T[] copy = uSpan.ToArray();
            Assert.Empty(copy);

            copy = urSpan.ToArray();
            Assert.Empty(copy);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Test(int length)
        {
            var rnd = new Random(41);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());

            T[] copy;

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                T[] expected = span.ToArray();

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    copy = uSpan.ToArray();
                    Assert.Equal<T>(expected, copy);

                    copy = urSpan.ToArray();
                    Assert.Equal<T>(expected, copy);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void NotSame(int length)
        {
            var rnd = new Random(42);

            T[] t = new T[length];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());

            T[] copy;

            unsafe
            {
                Span<T> span = new Span<T>(t);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    copy = uSpan.ToArray();
                    Assert.Equal<T>(t, copy);
                    Assert.NotSame(t, copy);

                    copy = urSpan.ToArray();
                    Assert.Equal<T>(t, copy);
                    Assert.NotSame(t, copy);
                }
            }
        }
    }

    public sealed class ToArray_byte : ToArray<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class ToArray_char : ToArray<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class ToArray_int : ToArray<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class ToArray_string : ToArray<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class ToArray_Tuple : ToArray<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class ToArray_ValueTuple : ToArray<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
