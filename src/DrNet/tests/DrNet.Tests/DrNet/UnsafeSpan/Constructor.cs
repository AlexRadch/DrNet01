using System;
using System.Runtime.CompilerServices;

using Xunit;

using DrNet.UnSafe;
using System.Linq;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Constructor<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;
            unsafe
            {
                Assert.True(null == uSpan._pointer);
                Assert.True(null == urSpan._pointer);
            }
            Assert.Equal(0, uSpan.Length);
            Assert.Equal(0, urSpan.Length);
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;

            UnsafeSpan<T> uSpan;
            UnsafeReadOnlySpan<T> urSpan;
            unsafe
            {
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    uSpan = new UnsafeSpan<T>(Unsafe.AsPointer(ref DrNetMarshal.GetReference(span)), 0);
                    urSpan = new UnsafeReadOnlySpan<T>(Unsafe.AsPointer(ref DrNetMarshal.GetReference(span)), 0);

                    Assert.True(null == uSpan._pointer);
                    Assert.True(null == urSpan._pointer);
                    Assert.Equal(0, uSpan.Length);
                    Assert.Equal(0, urSpan.Length);

                }

                uSpan = new UnsafeSpan<T>(span);
                urSpan = new UnsafeReadOnlySpan<T>(span);

                Assert.True(null == uSpan._pointer);
                Assert.True(null == urSpan._pointer);
                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);

                uSpan = new UnsafeSpan<T>(ref DrNetMarshal.GetReference(span), 0);
                urSpan = new UnsafeReadOnlySpan<T>(in DrNetMarshal.GetReference(span), 0);

                Assert.True(null == uSpan._pointer);
                Assert.True(null == urSpan._pointer);
                Assert.Equal(0, uSpan.Length);
                Assert.Equal(0, urSpan.Length);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromPointer(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(bytePtr, length);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(bytePtr, length);

                    Assert.True(UnsafeIn.AsPointer(in span.GetPinnableReference()) == uSpan._pointer);
                    Assert.True(UnsafeIn.AsPointer(in span.GetPinnableReference()) == urSpan._pointer);
                    Assert.Equal(length, uSpan.Length);
                    Assert.Equal(length, urSpan.Length);
                }
            }
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

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    Assert.True(Unsafe.AsPointer(ref DrNetMarshal.GetReference(span)) == uSpan._pointer);
                    Assert.True(UnsafeIn.AsPointer(in DrNetMarshal.GetReference(span)) == urSpan._pointer);
                    Assert.Equal(length, uSpan.Length);
                    Assert.Equal(length, urSpan.Length);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromRef(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(ref DrNetMarshal.GetReference(span), length);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(in DrNetMarshal.GetReference(span), length);

                    Assert.True(Unsafe.AsPointer(ref DrNetMarshal.GetReference(span)) == uSpan._pointer);
                    Assert.True(UnsafeIn.AsPointer(in DrNetMarshal.GetReference(span)) == urSpan._pointer);
                    Assert.Equal(length, uSpan.Length);
                    Assert.Equal(length, urSpan.Length);
                }
            }
        }

        [Fact]
        public void RangeCheck()
        {
            unsafe
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeSpan<T>(null, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeReadOnlySpan<T>(null, -1));

                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeSpan<T>(null, 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeReadOnlySpan<T>(null, 1));

                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeSpan<T>(ref Unsafe.AsRef<T>(null), -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeReadOnlySpan<T>(in Unsafe.AsRef<T>(null),
                    -1));

                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeSpan<T>(ref Unsafe.AsRef<T>(null), 1));
                Assert.Throws<ArgumentOutOfRangeException>(() => new UnsafeReadOnlySpan<T>(in Unsafe.AsRef<T>(null), 
                    1));
            }

            var rnd = new Random(40);
            T[] t = RepeatT(rnd).Take(3).ToArray();

            Span<T> span = new Span<T>(t, 1, 1);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, 1, 1);
            unsafe
            {
                TestHelpers.AssertThrows<ArgumentOutOfRangeException, T>(span, (aSpan) => 
                    new UnsafeSpan<T>(Unsafe.AsPointer(ref DrNetMarshal.GetReference(aSpan)), -1));
                TestHelpers.AssertThrows<ArgumentOutOfRangeException, T>(rspan, (aSpan) => 
                    new UnsafeReadOnlySpan<T>(UnsafeIn.AsPointer(in DrNetMarshal.GetReference(aSpan)), -1));

                TestHelpers.AssertThrows<ArgumentOutOfRangeException, T>(span, (aSpan) => 
                    new UnsafeSpan<T>(ref DrNetMarshal.GetReference(aSpan), -1));
                TestHelpers.AssertThrows<ArgumentOutOfRangeException, T>(rspan, (aSpan) => 
                    new UnsafeReadOnlySpan<T>(in DrNetMarshal.GetReference(aSpan), -1));
            }
        }
    }

    public sealed class Constructor_byte : Constructor<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Constructor_char : Constructor<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Constructor_int : Constructor<int>
    {
        protected override int NewT(int value) => value;

    }

    public sealed class Constructor_string : Constructor<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Constructor_Tuple : Constructor<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Constructor_ValueTuple : Constructor<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
