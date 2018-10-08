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
        [InlineData(0)]
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

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    for (var i = 0; i < length; i++)
                    {
                        Assert.True(Unsafe.AreSame(ref span[i], ref uSpan[i]));
                        Assert.Equal(span[i], uSpan[i]);

                        Assert.True(UnsafeIn.AreSame(in span[i], in urSpan[i]));
                        Assert.Equal(span[i], urSpan[i]);
                    }
                }
            }
        }

        [Theory]
        [InlineData(0)]
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

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    for (var i = 0; i < length; i++)
                    {
                        T item = NewT(rnd.Next());
                        span[i] = item;
                        Assert.Equal(item, uSpan[i]);
                        Assert.Equal(item, urSpan[i]);
                    }
                }
            }
        }

        [Theory]
        [InlineData(0)]
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

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    for (var i = 0; i < length; i++)
                    {
                        T item = NewT(rnd.Next());
                        uSpan[i] = item;
                        Assert.Equal(item, span[i]);
                    }
                }
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

    public sealed class Items_string : Items<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Items_Tuple : Items<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Items_ValueTuple : Items<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
