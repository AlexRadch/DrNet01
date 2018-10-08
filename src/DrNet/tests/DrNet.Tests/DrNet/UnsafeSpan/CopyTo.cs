﻿using System;
using System.Runtime.InteropServices;

using Xunit;

using DrNet.UnSafe;


namespace DrNet.Tests.UnsafeSpan
{
    public abstract class CopyTo<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            var rnd = new Random(40);

            UnsafeSpan<T> uSpan = default;
            UnsafeReadOnlySpan<T> urSpan = default;
            uSpan.CopyTo(default);
            urSpan.CopyTo(default);

            T[] d = new T[] { NextT(rnd), NextT(rnd), NextT(rnd) };
            T[] d2 = d.AsReadOnlySpan().ToArray();

            uSpan.CopyTo(d);
            Assert.Equal(d, d2);

            urSpan.CopyTo(d);
            Assert.Equal(d, d2);

            uSpan.CopyTo(d.AsSpan(2));
            Assert.Equal(d, d2);

            urSpan.CopyTo(d.AsSpan(2));
            Assert.Equal(d, d2);

            uSpan.CopyTo(d.AsSpan(3));
            Assert.Equal(d, d2);

            urSpan.CopyTo(d.AsSpan(3));
            Assert.Equal(d, d2);
        }

        [Fact]
        public void FromDefault()
        {
            var rnd = new Random(41);

            Span<T> span = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

            uSpan.CopyTo(default);
            urSpan.CopyTo(default);

            T[] d = new T[] { NextT(rnd), NextT(rnd), NextT(rnd) };
            T[] d2 = d.AsReadOnlySpan().ToArray();

            uSpan.CopyTo(d);
            Assert.Equal(d, d2);

            urSpan.CopyTo(d);
            Assert.Equal(d, d2);

            uSpan.CopyTo(d.AsSpan(2));
            Assert.Equal(d, d2);

            urSpan.CopyTo(d.AsSpan(2));
            Assert.Equal(d, d2);

            uSpan.CopyTo(d.AsSpan(3));
            Assert.Equal(d, d2);

            urSpan.CopyTo(d.AsSpan(3));
            Assert.Equal(d, d2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Shorter(int length)
        {
            var rnd = new Random(42);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());

            T[] d = new T[guardLength + length - 1 + guardLength];

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    Assert.Throws<ArgumentException>(() => uSpan.CopyTo(new Span<T>(d, guardLength, length - 1)));
                    Assert.Throws<ArgumentException>(() => urSpan.CopyTo(new Span<T>(d, guardLength, length - 1)));
                }
            }
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public void OverlappingP1(int length)
        {
            var rnd = new Random(43);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());
            T[] t2 = t.AsReadOnlySpan().ToArray();

            {
                Span<T> span = new Span<T>(t2, guardLength, length);
                ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t2, guardLength, length);
                Span<T> dspan = new Span<T>(t2, guardLength + 1, length);

                span.CopyTo(dspan);
                rspan.CopyTo(dspan);
            }

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                Span<T> dspan = new Span<T>(t, guardLength + 1, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    uSpan.CopyTo(dspan);
                    urSpan.CopyTo(dspan);
                }
            }

            Assert.True(t2.AsReadOnlySpan().EqualsToSeq(t.AsReadOnlySpan()));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        public void OverlappingM1(int length)
        {
            var rnd = new Random(44);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];
            for (var i = 0; i < t.Length; i++)
                t[i] = NewT(rnd.Next());
            T[] t2 = t.AsReadOnlySpan().ToArray();

            {
                Span<T> span = new Span<T>(t2, guardLength, length);
                ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t2, guardLength, length);
                Span<T> dspan = new Span<T>(t2, guardLength - 1, length);

                span.CopyTo(dspan);
                rspan.CopyTo(dspan);
            }

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                Span<T> dspan = new Span<T>(t, guardLength - 1, length);

                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    uSpan.CopyTo(dspan);
                    urSpan.CopyTo(dspan);
                }
            }

            Assert.True(t2.AsReadOnlySpan().EqualsToSeq(t.AsReadOnlySpan()));
        }
    }

    public sealed class CopyTo_byte : CopyTo<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class CopyTo_char : CopyTo<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class CopyTo_int : CopyTo<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class CopyTo_string : CopyTo<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class CopyTo_Tuple : CopyTo<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class CopyTo_ValueTuple : CopyTo<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
