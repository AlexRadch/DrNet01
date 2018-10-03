using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;


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

            UnsafeSpan<T> uSpan;
            UnsafeReadOnlySpan<T> urSpan;
            unsafe
            {
                uSpan = new UnsafeSpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)), span.Length);
                urSpan = new UnsafeReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(rspan)),
                    rspan.Length);
            }
            Assert.True(span == uSpan.AsSpan());
            Assert.True(rspan == urSpan.AsSpan());

            uSpan = new UnsafeSpan<T>(span);
            urSpan = new UnsafeReadOnlySpan<T>(rspan);
            Assert.True(span == uSpan.AsSpan());
            Assert.True(rspan == urSpan.AsSpan());

            uSpan = new UnsafeSpan<T>(ref MemoryMarshal.GetReference(span), span.Length);
            urSpan = new UnsafeReadOnlySpan<T>(in MemoryMarshal.GetReference(rspan), span.Length);
            Assert.True(span == uSpan.AsSpan());
            Assert.True(rspan == urSpan.AsSpan());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan =  new ReadOnlySpan<T>(t, guardLength + 1, length);

            GCHandle gch = GCHandle.Alloc(t, GCHandleType.Pinned);
            try
            {
                UnsafeSpan<T> uSpan;
                UnsafeReadOnlySpan<T> urSpan;
                unsafe
                {
                    uSpan = new UnsafeSpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)), span.Length);
                    urSpan = new UnsafeReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(rspan)),
                        rspan.Length);
                }
                Assert.True(span == uSpan.AsSpan());
                Assert.True(rspan == urSpan.AsSpan());

                uSpan = new UnsafeSpan<T>(span);
                urSpan = new UnsafeReadOnlySpan<T>(rspan);
                Assert.True(span == uSpan.AsSpan());
                Assert.True(rspan == urSpan.AsSpan());

                uSpan = new UnsafeSpan<T>(ref MemoryMarshal.GetReference(span), span.Length);
                urSpan = new UnsafeReadOnlySpan<T>(in MemoryMarshal.GetReference(rspan), span.Length);
                Assert.True(span == uSpan.AsSpan());
                Assert.True(rspan == urSpan.AsSpan());
            }
            finally
            {
                gch.Free();
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

    public sealed class AsSpan_intT : AsSpan<Tuple<int>>
    {
        protected override Tuple<int> NewT(int value) => new Tuple<int>(value);
    }

    //public sealed class AsSpan_string : AsSpan<string>
    //{
    //    protected override string NewT(int value) => value.ToString();
    //}

    //public sealed class AsSpan_stringE : AsSpan<TEquatable<string>>
    //{
    //    protected override TEquatable<string> NewT(int value) => new TEquatable<string>(value.ToString());
    //}
}
