﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;


namespace DrNet.Tests.DrNet.UnsafeSpan
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
            Assert.Equal(0, uSpan._length);
            Assert.Equal(0, urSpan._length);
        }

        [Fact]
        public void FromDefault()
        {
            Span<T> span = default;
            ReadOnlySpan<T> rspan = default;
            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);
            unsafe
            {
                Assert.True(null == uSpan._pointer);
                Assert.True(null == urSpan._pointer);
            }
            Assert.Equal(0, uSpan._length);
            Assert.Equal(0, urSpan._length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpan(int length)
        {
            var rnd = new Random(40);
            const int guardLength = 50;

            T[] t = new T[guardLength + length + guardLength];

            Span<T> span = new Span<T>(t, guardLength, length);
            ReadOnlySpan<T> rspan = new ReadOnlySpan<T>(t, guardLength, length);

            UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
            UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(rspan);
            unsafe
            {
                Assert.True(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)) == uSpan._pointer);
                Assert.True(Unsafe.AsPointer(ref MemoryMarshal.GetReference(rspan)) == urSpan._pointer);
            }
            Assert.Equal(length, uSpan._length);
            Assert.Equal(length, urSpan._length);
        }
    }

    public sealed class Constructor_byte : Constructor<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);

        [Fact]
        public void FromDefaultFixed()
        {
            Span<byte> span = default;
            ReadOnlySpan<byte> rspan = default;

            unsafe
            {
                fixed(byte* p = span)
                {
                    UnsafeSpan<byte> uSpan = new UnsafeSpan<byte>(p, span.Length);
                    Assert.True(null == uSpan._pointer);
                    Assert.Equal(0, uSpan._length);
                }

                fixed(byte* p = rspan)
                {
                    UnsafeReadOnlySpan<byte> urSpan = new UnsafeReadOnlySpan<byte>(p, rspan.Length);
                    Assert.True(null == urSpan._pointer);
                    Assert.Equal(0, urSpan._length);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpanFixed(int length)
        {
            var rnd = new Random(40);
            const int guardLength = 50;

            byte[] t = new byte[guardLength + length + guardLength];

            Span<byte> span = new Span<byte>(t, guardLength, length);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(t, guardLength, length);

            unsafe
            {
                fixed(byte* p = span)
                {
                    UnsafeSpan<byte> uSpan = new UnsafeSpan<byte>(p, span.Length);
                    Assert.True(Unsafe.AsPointer(ref span.GetPinnableReference()) == uSpan._pointer);
                    Assert.Equal(length, uSpan._length);
                }

                fixed(byte* p = rspan)
                {
                    UnsafeReadOnlySpan<byte> urSpan = new UnsafeReadOnlySpan<byte>(p, rspan.Length);
                    Assert.True(Unsafe.AsPointer(ref Unsafe.AsRef(in rspan.GetPinnableReference())) == urSpan._pointer);
                    Assert.Equal(length, urSpan._length);
                }
            }
        }
    }

    public sealed class Constructor_char : Constructor<char>
    {
        protected override char NewT(int value) => unchecked((char)value);

        [Fact]
        public void FromDefaultFixed()
        {
            Span<char> span = default;
            ReadOnlySpan<char> rspan = default;

            unsafe
            {
                fixed(char* p = span)
                {
                    UnsafeSpan<char> uSpan = new UnsafeSpan<char>(p, span.Length);
                    Assert.True(null == uSpan._pointer);
                    Assert.Equal(0, uSpan._length);
                }

                fixed(char* p = rspan)
                {
                    UnsafeReadOnlySpan<char> urSpan = new UnsafeReadOnlySpan<char>(p, rspan.Length);
                    Assert.True(null == urSpan._pointer);
                    Assert.Equal(0, urSpan._length);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpanFixed(int length)
        {
            var rnd = new Random(40);
            const int guardLength = 50;

            char[] t = new char[guardLength + length + guardLength];

            Span<char> span = new Span<char>(t, guardLength, length);
            ReadOnlySpan<char> rspan = new ReadOnlySpan<char>(t, guardLength, length);

            unsafe
            {
                fixed(char* p = span)
                {
                    UnsafeSpan<char> uSpan = new UnsafeSpan<char>(p, span.Length);
                    Assert.True(Unsafe.AsPointer(ref span.GetPinnableReference()) == uSpan._pointer);
                    Assert.Equal(length, uSpan._length);
                }

                fixed(char* p = rspan)
                {
                    UnsafeReadOnlySpan<char> urSpan = new UnsafeReadOnlySpan<char>(p, rspan.Length);
                    Assert.True(Unsafe.AsPointer(ref Unsafe.AsRef(in rspan.GetPinnableReference())) == urSpan._pointer);
                    Assert.Equal(length, urSpan._length);
                }
            }
        }
    }

    public sealed class Constructor_int : Constructor<int>
    {
        protected override int NewT(int value) => value;

        [Fact]
        public void FromDefaultFixed()
        {
            Span<int> span = default;
            ReadOnlySpan<int> rspan = default;

            unsafe
            {
                fixed(int* p = span)
                {
                    UnsafeSpan<int> uSpan = new UnsafeSpan<int>(p, span.Length);
                    Assert.True(null == uSpan._pointer);
                    Assert.Equal(0, uSpan._length);
                }

                fixed(int* p = rspan)
                {
                    UnsafeReadOnlySpan<int> urSpan = new UnsafeReadOnlySpan<int>(p, rspan.Length);
                    Assert.True(null == urSpan._pointer);
                    Assert.Equal(0, urSpan._length);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void FromSpanFixed(int length)
        {
            var rnd = new Random(40);
            const int guardLength = 50;

            int[] t = new int[guardLength + length + guardLength];

            Span<int> span = new Span<int>(t, guardLength, length);
            ReadOnlySpan<int> rspan = new ReadOnlySpan<int>(t, guardLength, length);

            unsafe
            {
                fixed(int* p = span)
                {
                    UnsafeSpan<int> uSpan = new UnsafeSpan<int>(p, span.Length);
                    Assert.True(Unsafe.AsPointer(ref span.GetPinnableReference()) == uSpan._pointer);
                    Assert.Equal(length, uSpan._length);
                }

                fixed(int* p = rspan)
                {
                    UnsafeReadOnlySpan<int> urSpan = new UnsafeReadOnlySpan<int>(p, rspan.Length);
                    Assert.True(Unsafe.AsPointer(ref Unsafe.AsRef(in rspan.GetPinnableReference())) == urSpan._pointer);
                    Assert.Equal(length, urSpan._length);
                }
            }
        }
    }

    public sealed class Constructor_string : Constructor<string>
    {
        protected override string NewT(int value) => value.ToString();
    }
}