using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class GetEnumerator<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T>.Enumerator uEnumerator = default;
            UnsafeReadOnlySpan<T>.Enumerator urEnumerator = default;

            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<ArgumentOutOfRangeException>(() => uEnumerator.Current);
            Assert.Throws<ArgumentOutOfRangeException>(() => urEnumerator.Current);

            uEnumerator = default(UnsafeSpan<T>).GetEnumerator();
            urEnumerator = default(UnsafeReadOnlySpan<T>).GetEnumerator();

            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<ArgumentOutOfRangeException>(() => uEnumerator.Current);
            Assert.Throws<ArgumentOutOfRangeException>(() => urEnumerator.Current);
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;

            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

            UnsafeSpan<T>.Enumerator uEnumerator = uSpan.GetEnumerator();
            UnsafeReadOnlySpan<T>.Enumerator urEnumerator = urSpan.GetEnumerator();

            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<ArgumentOutOfRangeException>(() => uEnumerator.Current);
            Assert.Throws<ArgumentOutOfRangeException>(() => urEnumerator.Current);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ForEachRead(int length)
        {
            var rnd = new Random(42);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NextT(rnd);

            Span<T> span = new Span<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                int index = 0;
                foreach (ref T item in uSpan)
                    Assert.True(UnsafeIn.AreSame(in span[index++], in item));

                index = 0;
                foreach (ref readonly T item in urSpan)
                    Assert.True(UnsafeIn.AreSame(in span[index++], in item));

                index = 0;
                foreach (T item in uSpan)
                    Assert.Equal(span[index++], item);

                index = 0;
                foreach (T item in urSpan)
                    Assert.Equal(span[index++], item);
            }
            finally
            {
                gch.Free();
            }
        }
    }

    public sealed class GetEnumerator_byte : GetEnumerator<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class GetEnumerator_char : GetEnumerator<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class GetEnumerator_int : GetEnumerator<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class GetEnumerator_intE : GetEnumerator<TEquatableInt>
    {
        protected override TEquatableInt NewT(int value) => new TEquatableInt(value, 0);
    }
}
