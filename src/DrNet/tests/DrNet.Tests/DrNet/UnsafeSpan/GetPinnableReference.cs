using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;
using System.Runtime.InteropServices;

using Xunit;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class GetPinnableReference<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            ref T uRef = ref uSpan.GetPinnableReference();
            ref readonly T urRef = ref urSpan.GetPinnableReference();
            unsafe
            {
                Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in uRef));
                Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in urRef));

                fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(uSpan))
                {
                    Assert.True(ptr == null);
                }
                fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(urSpan))
                {
                    Assert.True(ptr == null);
                }
            }
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);

            ref T uRef = ref uSpan.GetPinnableReference();
            ref readonly T urRef = ref urSpan.GetPinnableReference();
            unsafe
            {
                Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in uRef));
                Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in urRef));

                fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(uSpan))
                {
                    Assert.True(ptr == null);
                }
                fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(urSpan))
                {
                    Assert.True(ptr == null);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromPastEnd(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span.Slice(length));
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span.Slice(length));

                    ref T uRef = ref uSpan.GetPinnableReference();
                    ref readonly T urRef = ref urSpan.GetPinnableReference();

                    Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in uRef));
                    Assert.True(UnsafeIn.AreSame(in UnsafeIn.AsRef<T>(null), in urRef));

                    fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(uSpan))
                    {
                        Assert.True(ptr == null);
                    }
                    fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(urSpan))
                    {
                        Assert.True(ptr == null);
                    }
                }
            }
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

                    ref T uRef = ref uSpan.GetPinnableReference();
                    ref readonly T urRef = ref urSpan.GetPinnableReference();

                    Assert.True(UnsafeIn.AreSame(in span[0], in uRef));
                    Assert.True(UnsafeIn.AreSame(in span[0], in urRef));

                    fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(uSpan))
                    {
                        void* spanPtr = UnsafeRef.AsPointer(ref span[0]);
                        Assert.True(spanPtr == ptr);
                    }
                    fixed (byte* ptr = DrNetMarshal.UnsafeCastBytes(urSpan))
                    {
                        void* spanPtr = UnsafeIn.AsPointer(in span[0]);
                        Assert.True(spanPtr == ptr);
                    }
                }
            }
        }
    }

    public sealed class GetPinnableReference_byte : GetPinnableReference<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class GetPinnableReference_char : GetPinnableReference<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class GetPinnableReference_int : GetPinnableReference<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class GetPinnableReference_string : GetPinnableReference<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class GetPinnableReference_Tuple : GetPinnableReference<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class GetPinnableReference_ValueTuple : GetPinnableReference<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
