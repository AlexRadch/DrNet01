using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;


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
            var rnd = new Random(42);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NextT(rnd);
            T[] t2 = t.AsSpan().ToArray();

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

                uSpan.Fill(default);
                for (var i = 0; i < length; i++)
                {
                    Assert.Equal(default, span[i]);
                    Assert.Equal(default, uSpan[i]);
                    Assert.Equal(default, urSpan[i]);
                }

                T item = NextT(rnd);
                uSpan.Fill(item);

                for (var i = 0; i < length; i++)
                {
                    Assert.Equal(item, span[i]);
                    Assert.Equal(item, uSpan[i]);
                    Assert.Equal(item, urSpan[i]);
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

    public sealed class Fill_intE : Fill<TEquatableInt>
    {
        protected override TEquatableInt NewT(int value) => new TEquatableInt(value, 0);
    }
}
