using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;


namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Clear<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            uSpan.Clear();
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            uSpan.Clear();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            var rnd = new Random(42);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NextT(rnd);
            T[] t2 = t.AsSpan().ToArray();

            Span<T> span = new Span<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                uSpan.Clear();

                for (var i = 0; i < length; i++)
                {
                    Assert.Equal(default, span[i]);
                    Assert.Equal(default, uSpan[i]);
                }
            }
            finally
            {
                gch.Free();
            }

            Assert.True(t2.AsReadOnlySpan(0, guardLength).EqualsToSeq(t2.AsReadOnlySpan(0, guardLength)));
            Assert.True(t2.AsReadOnlySpan(guardLength + length, guardLength).EqualsToSeq(
                t2.AsReadOnlySpan(guardLength + length, guardLength)));
        }
    }

    public sealed class Clear_byte : Clear<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Clear_char : Clear<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Clear_int : Clear<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Clear_intE : Clear<TEquatableInt>
    {
        protected override TEquatableInt NewT(int value) => new TEquatableInt(value, 0);
    }
}
