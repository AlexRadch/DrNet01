using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Xunit;

using DrNet.UnSafe;


namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Items<T> : SpanTest<T>
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Read(int length)
        {
            var rnd = new Random(40);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

                for (var i = 0; i < length; i++)
                {
                    Assert.True(Unsafe.AreSame(ref span[i], ref uSpan[i]));
                    Assert.True(Unsafe.AreSame(ref span[i], ref Unsafe.AsRef(in urSpan[i])));

                    Assert.Equal(span[i], uSpan[i]);
                    Assert.Equal(span[i], urSpan[i]);
                }
            }
            finally
            {
                gch.Free();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ReadWrited(int length)
        {
            var rnd = new Random(41);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

                for (var i = 0; i < length; i++)
                {
                    Assert.True(Unsafe.AreSame(ref span[i], ref uSpan[i]));
                    Assert.True(Unsafe.AreSame(ref span[i], ref Unsafe.AsRef(in urSpan[i])));

                    T item = NewT(rnd.Next());
                    span[i] = item;
                    Assert.Equal(item, uSpan[i]);
                    Assert.Equal(item, urSpan[i]);
                }
            }
            finally
            {
                gch.Free();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Write(int length)
        {
            var rnd = new Random(42);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());
            T[] t2 = t.AsSpan().ToArray();

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

                for (var i = 0; i < length; i++)
                {
                    Assert.True(Unsafe.AreSame(ref span[i], ref uSpan[i]));
                    Assert.True(Unsafe.AreSame(ref span[i], ref Unsafe.AsRef(in urSpan[i])));

                    T item = NewT(rnd.Next());
                    uSpan[i] = item;
                    Assert.Equal(item, span[i]);
                    Assert.Equal(item, uSpan[i]);
                    Assert.Equal(item, urSpan[i]);
                }

                span.EqualsToSeq<T, T>(uSpan.AsSpan());
                span.EqualsToSeq(urSpan.AsSpan());
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

    public sealed class Items_byte : Items<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Items_char : Items<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Items_int : Items<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Items_intE : Items<TEquatableInt>
    {
        protected override TEquatableInt NewT(int value) => new TEquatableInt(value, 0);
    }
}
