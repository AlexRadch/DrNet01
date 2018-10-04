using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;


namespace DrNet.Tests.UnsafeSpan
{
    public abstract class IsEmpty<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            Assert.True(uSpan.IsEmpty);
            Assert.True(urSpan.IsEmpty);
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;

            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);
            Assert.True(uSpan.IsEmpty);
            Assert.True(urSpan.IsEmpty);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan =  new ReadOnlySpan<T>(t, guardLength + 1, length);

            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);
            Assert.False(uSpan.IsEmpty);
            Assert.False(urSpan.IsEmpty);
        }
    }

    public sealed class IsEmpty_byte : IsEmpty<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class IsEmpty_char : IsEmpty<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class IsEmpty_int : IsEmpty<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class IsEmpty_intE : IsEmpty<TEquatableInt>
    {
        protected override TEquatableInt NewT(int value) => new TEquatableInt(value, 0);
    }
}
