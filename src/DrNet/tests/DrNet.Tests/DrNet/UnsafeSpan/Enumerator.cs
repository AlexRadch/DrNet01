using System;
using System.Linq;

using Xunit;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class Enumerator<T> : SpanTest<T>
    {
        [Fact]
        public void Default()
        {
            UnsafeSpan<T>.Enumerator uEnumerator = default;
            UnsafeReadOnlySpan<T>.Enumerator urEnumerator = default;

            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);

            uEnumerator = default(UnsafeSpan<T>).GetEnumerator();
            urEnumerator = default(UnsafeReadOnlySpan<T>).GetEnumerator();

            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
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

            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
            Assert.False(uEnumerator.MoveNext());
            Assert.False(urEnumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
            Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ForEachRead(int length)
        {
            var rnd = new Random(42 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
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
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ManualRead(int length)
        {
            var rnd = new Random(43 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    UnsafeSpan<T>.Enumerator uEnumerator = uSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
                    int index = 0;
                    while (uEnumerator.MoveNext())
                        Assert.True(UnsafeIn.AreSame(in span[index++], in uEnumerator.Current));
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);

                    uEnumerator.Reset();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
                    index = 0;
                    while (uEnumerator.MoveNext())
                        Assert.True(UnsafeIn.AreSame(in span[index++], in uEnumerator.Current));
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);

                    UnsafeReadOnlySpan<T>.Enumerator urEnumerator = urSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                    index = 0;
                    while (urEnumerator.MoveNext())
                        Assert.True(UnsafeIn.AreSame(in span[index++], in urEnumerator.Current));
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);

                    urEnumerator.Reset();
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                    index = 0;
                    while (urEnumerator.MoveNext())
                        Assert.True(UnsafeIn.AreSame(in span[index++], in urEnumerator.Current));
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);

                    uEnumerator = uSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
                    index = 0;
                    while (uEnumerator.MoveNext())
                        Assert.Equal(span[index++], uEnumerator.Current);
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);

                    uEnumerator.Reset();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
                    index = 0;
                    while (uEnumerator.MoveNext())
                        Assert.Equal(span[index++], uEnumerator.Current);
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);

                    urEnumerator = urSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                    index = 0;
                    while (urEnumerator.MoveNext())
                        Assert.Equal(span[index++], urEnumerator.Current);
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);

                    urEnumerator.Reset();
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                    index = 0;
                    while (urEnumerator.MoveNext())
                        Assert.Equal(span[index++], urEnumerator.Current);
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ForEachReadWrited(int length)
        {
            var rnd = new Random(44 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    int index = 0;
                    foreach (ref T item in uSpan)
                    {
                        T newItem = NextT(rnd);
                        span[index++] = newItem;
                        Assert.Equal(newItem, item);
                    }

                    index = 0;
                    foreach (ref readonly T item in urSpan)
                    {
                        T newItem = NextT(rnd);
                        span[index++] = newItem;
                        Assert.Equal(newItem, item);
                    }
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ManualReadWrited(int length)
        {
            var rnd = new Random(45 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);
                    UnsafeReadOnlySpan<T> urSpan = new UnsafeReadOnlySpan<T>(span);

                    UnsafeSpan<T>.Enumerator uEnumerator = uSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);
                    int index = 0;
                    while (uEnumerator.MoveNext())
                    {
                        T newItem = NextT(rnd);
                        span[index++] = newItem;
                        Assert.Equal(newItem, uEnumerator.Current);
                    }
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current);

                    UnsafeReadOnlySpan<T>.Enumerator urEnumerator = urSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                    index = 0;
                    while (urEnumerator.MoveNext())
                    {
                        T newItem = NextT(rnd);
                        span[index++] = newItem;
                        Assert.Equal(newItem, urEnumerator.Current);
                    }
                    Assert.Throws<InvalidOperationException>(() => urEnumerator.Current);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ForEachWrite(int length)
        {
            var rnd = new Random(46 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();
            T[] t2 = t.ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);

                    int index = 0;
                    foreach (ref T item in uSpan)
                    {
                        T newItem = NextT(rnd);
                        item = newItem;
                        Assert.Equal(newItem, span[index++]);
                    }
                }
            }

            Assert.True(t2.AsReadOnlySpan(0, guardLength).EqualsToSeq(t.AsReadOnlySpan(0, guardLength)));
            Assert.True(t2.AsReadOnlySpan(guardLength + length, guardLength).EqualsToSeq(
                t.AsReadOnlySpan(guardLength + length, guardLength)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void ManualWrite(int length)
        {
            var rnd = new Random(47 * (length + 1));
            const int guardLength = 50;

            T[] t = RepeatT(rnd).Take(guardLength + length + guardLength).ToArray();
            T[] t2 = t.ToArray();

            unsafe
            {
                Span<T> span = new Span<T>(t, guardLength, length);
                fixed (byte* bytePtr = DrNetMarshal.UnsafeCastBytes(span))
                {
                    UnsafeSpan<T> uSpan = new UnsafeSpan<T>(span);

                    UnsafeSpan<T>.Enumerator uEnumerator = uSpan.GetEnumerator();
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current = NextT(rnd));
                    int index = 0;
                    while (uEnumerator.MoveNext())
                    {
                        T newItem = NextT(rnd);
                        uEnumerator.Current = newItem;
                        Assert.Equal(newItem, span[index++]);
                    }
                    Assert.Throws<InvalidOperationException>(() => uEnumerator.Current = NextT(rnd));
                }
            }

            Assert.True(t2.AsReadOnlySpan(0, guardLength).EqualsToSeq(t.AsReadOnlySpan(0, guardLength)));
            Assert.True(t2.AsReadOnlySpan(guardLength + length, guardLength).EqualsToSeq(
                t.AsReadOnlySpan(guardLength + length, guardLength)));
        }
    }

    public sealed class Enumerator_byte : Enumerator<byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
    }

    public sealed class Enumerator_char : Enumerator<char>
    {
        protected override char NewT(int value) => unchecked((char)value);
    }

    public sealed class Enumerator_int : Enumerator<int>
    {
        protected override int NewT(int value) => value;
    }

    public sealed class Enumerator_string : Enumerator<string>
    {
        protected override string NewT(int value) => value.ToString();
    }

    public sealed class Enumerator_Tuple : Enumerator<Tuple<byte, char, int, string>>
    {
        protected override Tuple<byte, char, int, string> NewT(int value) => 
            new Tuple<byte, char, int, string>(unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }

    public sealed class Enumerator_ValueTuple : Enumerator<(byte, char, int, string)>
    {
        protected override (byte, char, int, string) NewT(int value) => 
            (unchecked((byte)value), unchecked((char)value), value, value.ToString());
    }
}
