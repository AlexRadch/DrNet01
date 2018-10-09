using System;
using System.Runtime.InteropServices;
using Xunit;

using DrNet.UnSafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Equality<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;

#pragma warning disable CS1718 // Выполнено сравнение с той же переменной
            Assert.True(uSpan.Equals(uSpan));
            Assert.True(uSpan.Equals(urSpan));
            Assert.True(uSpan.Equals((object)uSpan));
            Assert.True(uSpan.Equals((object)urSpan));
            Assert.True(uSpan == uSpan);
            Assert.True(uSpan == urSpan);

            Assert.True(urSpan.Equals(uSpan));
            Assert.True(urSpan.Equals(urSpan));
            Assert.True(urSpan.Equals((object)uSpan));
            Assert.True(urSpan.Equals((object)urSpan));
            Assert.True(urSpan == uSpan);
            Assert.True(urSpan == urSpan);
#pragma warning restore CS1718 // Выполнено сравнение с той же переменной

            UnsafeSpan<T> uSpan2 = default;
            UnsafeReadOnlySpan<T> urSpan2 = default;

            Assert.True(uSpan.Equals(uSpan2));
            Assert.True(uSpan.Equals(urSpan2));
            Assert.True(uSpan.Equals((object)uSpan2));
            Assert.True(uSpan.Equals((object)urSpan2));
            Assert.True(uSpan == uSpan2);
            Assert.True(uSpan == urSpan2);

            Assert.True(urSpan.Equals(uSpan2));
            Assert.True(urSpan.Equals(urSpan2));
            Assert.True(urSpan.Equals((object)uSpan2));
            Assert.True(urSpan.Equals((object)urSpan2));
            Assert.True(urSpan == uSpan2);
            Assert.True(urSpan == urSpan2);

            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;

            Assert.True(uSpan == span);
            Assert.True(span == uSpan);
            Assert.True(uSpan == rspan);
            Assert.True(rspan == uSpan);

            Assert.True(urSpan == span);
            Assert.True(span == urSpan);
            Assert.True(urSpan == rspan);
            Assert.True(rspan == urSpan);

            uSpan2 = new UnsafeSpan<T>(span);
            urSpan2 = new UnsafeReadOnlySpan<T>(span);

            Assert.True(uSpan.Equals(uSpan2));
            Assert.True(uSpan.Equals(urSpan2));
            Assert.True(uSpan.Equals((object)uSpan2));
            Assert.True(uSpan.Equals((object)urSpan2));
            Assert.True(uSpan == uSpan2);
            Assert.True(uSpan == urSpan2);

            Assert.True(urSpan.Equals(uSpan2));
            Assert.True(urSpan.Equals(urSpan2));
            Assert.True(urSpan.Equals((object)uSpan2));
            Assert.True(urSpan.Equals((object)urSpan2));
            Assert.True(urSpan == uSpan2);
            Assert.True(urSpan == urSpan2);

            Assert.True(uSpan2.Equals(uSpan));
            Assert.True(uSpan2.Equals(urSpan));
            Assert.True(uSpan2.Equals((object)uSpan));
            Assert.True(uSpan2.Equals((object)urSpan));
            Assert.True(uSpan2 == uSpan);
            Assert.True(uSpan2 == urSpan);

            Assert.True(urSpan2.Equals(uSpan));
            Assert.True(urSpan2.Equals(urSpan));
            Assert.True(urSpan2.Equals((object)uSpan));
            Assert.True(urSpan2.Equals((object)urSpan));
            Assert.True(urSpan2 == uSpan);
            Assert.True(urSpan2 == urSpan);

            span = new T[0];
            rspan = new T[0];

            Assert.False(uSpan == span);
            Assert.False(span == uSpan);
            Assert.False(uSpan == rspan);
            Assert.False(rspan == uSpan);

            Assert.False(urSpan == span);
            Assert.False(span == urSpan);
            Assert.False(urSpan == rspan);
            Assert.False(rspan == urSpan);

            uSpan2 = new UnsafeSpan<T>(span);
            urSpan2 = new UnsafeReadOnlySpan<T>(rspan);

            Assert.False(uSpan.Equals(uSpan2));
            Assert.False(uSpan.Equals(urSpan2));
            Assert.False(uSpan.Equals((object)uSpan2));
            Assert.False(uSpan.Equals((object)urSpan2));
            Assert.False(uSpan == uSpan2);
            Assert.False(uSpan == urSpan2);

            Assert.False(urSpan.Equals(uSpan2));
            Assert.False(urSpan.Equals(urSpan2));
            Assert.False(urSpan.Equals((object)uSpan2));
            Assert.False(urSpan.Equals((object)urSpan2));
            Assert.False(urSpan == uSpan2);
            Assert.False(urSpan == urSpan2);

            Assert.False(uSpan2.Equals(uSpan));
            Assert.False(uSpan2.Equals(urSpan));
            Assert.False(uSpan2.Equals((object)uSpan));
            Assert.False(uSpan2.Equals((object)urSpan));
            Assert.False(uSpan2 == uSpan);
            Assert.False(uSpan2 == urSpan);

            Assert.False(urSpan2.Equals(uSpan));
            Assert.False(urSpan2.Equals(urSpan));
            Assert.False(urSpan2.Equals((object)uSpan));
            Assert.False(urSpan2.Equals((object)urSpan));
            Assert.False(urSpan2 == uSpan);
            Assert.False(urSpan2 == urSpan);
        }

        [Fact]
        public void FromDefault()
        {
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(default);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(default);

#pragma warning disable CS1718 // Выполнено сравнение с той же переменной
            Assert.True(uSpan.Equals(uSpan));
            Assert.True(uSpan.Equals(urSpan));
            Assert.True(uSpan.Equals((object)uSpan));
            Assert.True(uSpan.Equals((object)urSpan));
            Assert.True(uSpan == uSpan);
            Assert.True(uSpan == urSpan);

            Assert.True(urSpan.Equals(uSpan));
            Assert.True(urSpan.Equals(urSpan));
            Assert.True(urSpan.Equals((object)uSpan));
            Assert.True(urSpan.Equals((object)urSpan));
            Assert.True(urSpan == uSpan);
            Assert.True(urSpan == urSpan);
#pragma warning restore CS1718 // Выполнено сравнение с той же переменной

            UnsafeSpan<T> uSpan2 = default;
            UnsafeReadOnlySpan<T> urSpan2 = default;

            Assert.True(uSpan.Equals(uSpan2));
            Assert.True(uSpan.Equals(urSpan2));
            Assert.True(uSpan.Equals((object)uSpan2));
            Assert.True(uSpan.Equals((object)urSpan2));
            Assert.True(uSpan == uSpan2);
            Assert.True(uSpan == urSpan2);

            Assert.True(urSpan.Equals(uSpan2));
            Assert.True(urSpan.Equals(urSpan2));
            Assert.True(urSpan.Equals((object)uSpan2));
            Assert.True(urSpan.Equals((object)urSpan2));
            Assert.True(urSpan == uSpan2);
            Assert.True(urSpan == urSpan2);

            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;

            Assert.True(uSpan == span);
            Assert.True(span == uSpan);
            Assert.True(uSpan == rspan);
            Assert.True(rspan == uSpan);

            Assert.True(urSpan == span);
            Assert.True(span == urSpan);
            Assert.True(urSpan == rspan);
            Assert.True(rspan == urSpan);

            uSpan2 = new UnsafeSpan<T>(span);
            urSpan2 = new UnsafeReadOnlySpan<T>(span);

            Assert.True(uSpan.Equals(uSpan2));
            Assert.True(uSpan.Equals(urSpan2));
            Assert.True(uSpan.Equals((object)uSpan2));
            Assert.True(uSpan.Equals((object)urSpan2));
            Assert.True(uSpan == uSpan2);
            Assert.True(uSpan == urSpan2);

            Assert.True(urSpan.Equals(uSpan2));
            Assert.True(urSpan.Equals(urSpan2));
            Assert.True(urSpan.Equals((object)uSpan2));
            Assert.True(urSpan.Equals((object)urSpan2));
            Assert.True(urSpan == uSpan2);
            Assert.True(urSpan == urSpan2);

            span = new T[0];
            rspan = new T[0];

            Assert.False(uSpan == span);
            Assert.False(span == uSpan);
            Assert.False(uSpan == rspan);
            Assert.False(rspan == uSpan);

            Assert.False(urSpan == span);
            Assert.False(span == urSpan);
            Assert.False(urSpan == rspan);
            Assert.False(rspan == urSpan);

            uSpan2 = new UnsafeSpan<T>(span);
            urSpan2 = new UnsafeReadOnlySpan<T>(rspan);

            Assert.False(uSpan.Equals(uSpan2));
            Assert.False(uSpan.Equals(urSpan2));
            Assert.False(uSpan.Equals((object)uSpan2));
            Assert.False(uSpan.Equals((object)urSpan2));
            Assert.False(uSpan == uSpan2);
            Assert.False(uSpan == urSpan2);

            Assert.False(urSpan.Equals(uSpan2));
            Assert.False(urSpan.Equals(urSpan2));
            Assert.False(urSpan.Equals((object)uSpan2));
            Assert.False(urSpan.Equals((object)urSpan2));
            Assert.False(urSpan == uSpan2);
            Assert.False(urSpan == urSpan2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void EqualityTrue(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(span);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(rspan);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(rspan);

#pragma warning disable CS1718 // Выполнено сравнение с той же переменной
                    Assert.True(uSpanL.Equals(uSpanL));
                    Assert.True(uSpanL.Equals(urSpanL));
                    Assert.True(uSpanL.Equals((object)uSpanL));
                    Assert.True(uSpanL.Equals((object)urSpanL));
                    Assert.True(uSpanL == uSpanL);
                    Assert.True(uSpanL == urSpanL);

                    Assert.True(urSpanL.Equals(uSpanL));
                    Assert.True(urSpanL.Equals(urSpanL));
                    Assert.True(urSpanL.Equals((object)uSpanL));
                    Assert.True(urSpanL.Equals((object)urSpanL));
                    Assert.True(urSpanL == uSpanL);
                    Assert.True(urSpanL == urSpanL);
#pragma warning restore CS1718 // Выполнено сравнение с той же переменной

                    Assert.True(uSpanL.Equals(uSpanR));
                    Assert.True(uSpanL.Equals(urSpanR));
                    Assert.True(uSpanL.Equals((object)uSpanR));
                    Assert.True(uSpanL.Equals((object)urSpanR));
                    Assert.True(uSpanL == uSpanR);
                    Assert.True(uSpanL == urSpanR);

                    Assert.True(urSpanL.Equals(uSpanR));
                    Assert.True(urSpanL.Equals(urSpanR));
                    Assert.True(urSpanL.Equals((object)uSpanR));
                    Assert.True(urSpanL.Equals((object)urSpanR));
                    Assert.True(urSpanL == uSpanR);
                    Assert.True(urSpanL == urSpanR);

                    Assert.True(uSpanL == span);
                    Assert.True(span == uSpanL);
                    Assert.True(uSpanL == rspan);
                    Assert.True(rspan == uSpanL);

                    Assert.True(urSpanL == span);
                    Assert.True(span == urSpanL);
                    Assert.True(urSpanL == rspan);
                    Assert.True(rspan == urSpanL);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void IncludesLength(int length)
        {
            const int guardLength = 50;

            T[] t = new T[guardLength + length + 1 + guardLength];

            unsafe
            {
                Span<T> spanL = new Span<T>(t, guardLength, length + 1);
                ReadOnlySpan<T> rspanL = new ReadOnlySpan<T>(t, guardLength, length + 1);

                Span<T> spanR = new Span<T>(t, guardLength, length);
                ReadOnlySpan<T> rspanR = new ReadOnlySpan<T>(t, guardLength, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(spanL))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(spanL);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(spanR);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(spanL);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(spanR);

                    Assert.False(uSpanL.Equals(uSpanR));
                    Assert.False(uSpanL.Equals(urSpanR));
                    Assert.False(uSpanL.Equals((object)uSpanR));
                    Assert.False(uSpanL.Equals((object)urSpanR));
                    Assert.False(uSpanL == uSpanR);
                    Assert.False(uSpanL == urSpanR);

                    Assert.False(urSpanL.Equals(uSpanR));
                    Assert.False(urSpanL.Equals(urSpanR));
                    Assert.False(urSpanL.Equals((object)uSpanR));
                    Assert.False(urSpanL.Equals((object)urSpanR));
                    Assert.False(urSpanL == uSpanR);
                    Assert.False(urSpanL == urSpanR);

                    Assert.False(uSpanL == spanR);
                    Assert.False(spanR == uSpanL);
                    Assert.False(uSpanL == rspanR);
                    Assert.False(rspanR == uSpanL);

                    Assert.False(urSpanL == spanR);
                    Assert.False(spanR == urSpanL);
                    Assert.False(urSpanL == rspanR);
                    Assert.False(rspanR == urSpanL);

                    Assert.False(uSpanR.Equals(uSpanL));
                    Assert.False(uSpanR.Equals(urSpanL));
                    Assert.False(uSpanR.Equals((object)uSpanL));
                    Assert.False(uSpanR.Equals((object)urSpanL));
                    Assert.False(uSpanR == uSpanL);
                    Assert.False(uSpanR == urSpanL);

                    Assert.False(urSpanR.Equals(uSpanL));
                    Assert.False(urSpanR.Equals(urSpanL));
                    Assert.False(urSpanR.Equals((object)uSpanL));
                    Assert.False(urSpanR.Equals((object)urSpanL));
                    Assert.False(urSpanR == uSpanL);
                    Assert.False(urSpanR == urSpanL);

                    Assert.False(uSpanR == spanL);
                    Assert.False(spanL == uSpanR);
                    Assert.False(uSpanR == rspanL);
                    Assert.False(rspanL == uSpanR);

                    Assert.False(urSpanR == spanL);
                    Assert.False(spanL == urSpanR);
                    Assert.False(urSpanR == rspanL);
                    Assert.False(rspanL == urSpanR);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void IncludesIncludesBase(int length)
        {
            const int guardLength = 50;

            T[] tL = new T[guardLength + length + guardLength];
            T[] tR = new T[guardLength + length + guardLength];

            unsafe
            {
                Span<T> spanL = new Span<T>(tL, guardLength, length);
                ReadOnlySpan<T> rspanL = new ReadOnlySpan<T>(tL, guardLength, length);

                Span<T> spanR = new Span<T>(tR, guardLength, length);
                ReadOnlySpan<T> rspanR = new ReadOnlySpan<T>(tR, guardLength, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(spanL))
                {
                    UnsafeSpan<T> uSpanL = new UnsafeSpan<T>(spanL);
                    UnsafeSpan<T> uSpanR = new UnsafeSpan<T>(spanR);
                    UnsafeReadOnlySpan<T> urSpanL = new UnsafeReadOnlySpan<T>(spanL);
                    UnsafeReadOnlySpan<T> urSpanR = new UnsafeReadOnlySpan<T>(spanR);

                    Assert.False(uSpanL.Equals(uSpanR));
                    Assert.False(uSpanL.Equals(urSpanR));
                    Assert.False(uSpanL.Equals((object)uSpanR));
                    Assert.False(uSpanL.Equals((object)urSpanR));
                    Assert.False(uSpanL == uSpanR);
                    Assert.False(uSpanL == urSpanR);

                    Assert.False(urSpanL.Equals(uSpanR));
                    Assert.False(urSpanL.Equals(urSpanR));
                    Assert.False(urSpanL.Equals((object)uSpanR));
                    Assert.False(urSpanL.Equals((object)urSpanR));
                    Assert.False(urSpanL == uSpanR);
                    Assert.False(urSpanL == urSpanR);

                    Assert.False(uSpanL == spanR);
                    Assert.False(spanR == uSpanL);
                    Assert.False(uSpanL == rspanR);
                    Assert.False(rspanR == uSpanL);

                    Assert.False(urSpanL == spanR);
                    Assert.False(spanR == urSpanL);
                    Assert.False(urSpanL == rspanR);
                    Assert.False(rspanR == urSpanL);

                    Assert.False(uSpanR.Equals(uSpanL));
                    Assert.False(uSpanR.Equals(urSpanL));
                    Assert.False(uSpanR.Equals((object)uSpanL));
                    Assert.False(uSpanR.Equals((object)urSpanL));
                    Assert.False(uSpanR == uSpanL);
                    Assert.False(uSpanR == urSpanL);

                    Assert.False(urSpanR.Equals(uSpanL));
                    Assert.False(urSpanR.Equals(urSpanL));
                    Assert.False(urSpanR.Equals((object)uSpanL));
                    Assert.False(urSpanR.Equals((object)urSpanL));
                    Assert.False(urSpanR == uSpanL);
                    Assert.False(urSpanR == urSpanL);

                    Assert.False(uSpanR == spanL);
                    Assert.False(spanL == uSpanR);
                    Assert.False(uSpanR == rspanL);
                    Assert.False(rspanL == uSpanR);

                    Assert.False(urSpanR == spanL);
                    Assert.False(spanL == urSpanR);
                    Assert.False(urSpanR == rspanL);
                    Assert.False(rspanL == urSpanR);
                }
            }
        }
    }

    public sealed class Equality_byte : Equality<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Equality_char : Equality<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Equality_int : Equality<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Equality_string : Equality<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Equality_Tuple : Equality<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Equality_ValueTuple : Equality<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
