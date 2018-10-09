using System;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Slice<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

            unsafe
            {
                uSpan = uSpan.Slice(0);
                urSpan = urSpan.Slice(0);

                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);

                ref readonly T expected = ref UnsafeIn.AsRef<T>(null);
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));

                uSpan = uSpan.Slice(0, 0);
                urSpan = urSpan.Slice(0, 0);

                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);

                expected = ref UnsafeIn.AsRef<T>(null);
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
            }
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

            unsafe
            {
                uSpan = uSpan.Slice(0);
                urSpan = urSpan.Slice(0);

                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);

                ref readonly T expected = ref UnsafeIn.AsRef<T>(null);
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));

                uSpan = uSpan.Slice(0, 0);
                urSpan = urSpan.Slice(0, 0);

                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);

                expected = ref UnsafeIn.AsRef<T>(null);
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Start(int length)
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

                    uSpan = uSpan.Slice(length / 2);
                    urSpan = urSpan.Slice(length / 2);

                    Assert.Equal(length - length / 2, uSpan.Length);
                    Assert.Equal(length - length / 2, urSpan.Length);

                    ref readonly T expected = ref UnsafeIn.Add(in DrNetMarshal.GetReference(span), length / 2);
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void StartPastEnd(int length)
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

                    uSpan = uSpan.Slice(length);
                    urSpan = urSpan.Slice(length);

                    Assert.Equal(0, uSpan.Length);
                    Assert.Equal(0, urSpan.Length);

                    ref readonly T expected = ref UnsafeIn.Add(in DrNetMarshal.GetReference(span), length);
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void StartLength(int length)
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

                    uSpan = uSpan.Slice(length / 3, length / 2);
                    urSpan = urSpan.Slice(length / 3, length / 2);

                    Assert.Equal(length / 2, uSpan.Length);
                    Assert.Equal(length / 2, urSpan.Length);

                    ref readonly T expected = ref UnsafeIn.Add(in DrNetMarshal.GetReference(span), length / 3);
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void StartLengthUpToEnd(int length)
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

                    uSpan = uSpan.Slice(length / 2, length - length / 2);
                    urSpan = urSpan.Slice(length / 2, length - length / 2);

                    Assert.Equal(length - length / 2, uSpan.Length);
                    Assert.Equal(length - length / 2, urSpan.Length);

                    ref readonly T expected = ref UnsafeIn.Add(in DrNetMarshal.GetReference(span), length / 2);
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void StartLengthPastEnd(int length)
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

                    uSpan = uSpan.Slice(length, 0);
                    urSpan = urSpan.Slice(length, 0);

                    Assert.Equal(0, uSpan.Length);
                    Assert.Equal(0, urSpan.Length);

                    ref readonly T expected = ref UnsafeIn.Add(in DrNetMarshal.GetReference(span), length);
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                    Assert.True(UnsafeIn.AreSame(expected, in DrNetMarshal.GetReference(uSpan)));
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void RangeCheck(int length)
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

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length + 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length + 1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1, 0));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1, 0));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1, 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1, 1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1, length));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1, length));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(-1, length + 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(-1, length + 1));
                    
                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(0, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(0, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(0, length + 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(0, length + 1));
                    
                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(1, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(1, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(1, length));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(1, length));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length - 1, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length - 1, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length - 1, 2));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length - 1, 2));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length, 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length, 1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length + 1, -1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length + 1, -1));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length + 1, 0));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length + 1, 0));

                    Assert.Throws<ArgumentOutOfRangeException>(() => uSpan.Slice(length + 1, 1));
                    Assert.Throws<ArgumentOutOfRangeException>(() => urSpan.Slice(length + 1, 1));
                }
            }
        }
    }

    public sealed class Slice_byte : Slice<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Slice_char : Slice<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Slice_int : Slice<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Slice_string : Slice<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Slice_Tuple : Slice<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Slice_ValueTuple : Slice<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
