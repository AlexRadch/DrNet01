using System;
using System.Linq;
using Xunit;

namespace DrNet.Tests.Span
{
    public abstract class EqualsToSeq<T, TSource, TValue>
    {
        protected abstract T NewT(int value);

        protected abstract TSource NewTSource(T value, Action<T, T> onCompare = default);

        protected abstract TValue NewTValue(T value, Action<T, T> onCompare = default);

        private Action<T, T> onCompare;

        private bool EqualityCompareT(T v1, T v2)
        {
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        private bool EqualityCompare(TSource sValue, TValue vValue)
        {
            if (onCompare != null && sValue is T tSource && vValue is T tValue)
                onCompare(tSource, tValue);

            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            return sValue.Equals(vValue);
        }

        private bool EqualityCompareFrom(TValue vValue, TSource sValue)
        {
            if (onCompare != null && vValue is T tValue && sValue is T tSource)
                onCompare(tValue, tSource);

            if (vValue is IEquatable<TSource> vEquatable)
                return vEquatable.Equals(sValue);
            if (sValue is IEquatable<TValue> sEquatable)
                return sEquatable.Equals(vValue);
            return vValue.Equals(sValue);
        }

        [Fact]
        public void ZeroLength()
        {
            var rnd = new Random(40);

            Span<TSource> span = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsSpan(1, 0);
            ReadOnlySpan<TSource> rspan = new TSource[] { NewTSource(NewT(rnd.Next())), NewTSource(NewT(rnd.Next())),
                NewTSource(NewT(rnd.Next())) }.AsReadOnlySpan(2, 0);
            ReadOnlySpan<TValue> values = new TValue[] { NewTValue(NewT(rnd.Next())), NewTValue(NewT(rnd.Next())),
                NewTValue(NewT(rnd.Next())) }.AsReadOnlySpan(3, 0);

            bool c = MemoryExt.EqualsToSeq(span, values);
            Assert.True(c);
            c = MemoryExt.EqualsToSeq(span, values, EqualityCompare);
            Assert.True(c);

            c = MemoryExt.EqualsToSeq(rspan, values);
            Assert.True(c);
            c = MemoryExt.EqualsToSeq(rspan, values, EqualityCompare);
            Assert.True(c);
        }

        //[Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //[InlineData(10)]
        //[InlineData(100)]
        //public void SameSpan(int length)
        //{
        //    TSource[] a = { NewTSource(4), NewTSource(5), NewTSource(6) };
        //    Span<TSource> span = new Span<TSource>(a);
        //    bool b = MemoryExt.EqualsToSeq<TSource, TSource>(span, span);
        //    Assert.True(b);
        //}
    }

    public class EqualsToSeq_byte : EqualsToSeq<byte, byte, byte>
    {
        protected override byte NewT(int value) => unchecked((byte)value);
        protected override byte NewTSource(byte value, Action<byte, byte> onCompare) => value;
        protected override byte NewTValue(byte value, Action<byte, byte> onCompare) => value;
    }

    public class EqualsToSeq_char : EqualsToSeq<char, char, char>
    {
        protected override char NewT(int value) => unchecked((char)value);
        protected override char NewTSource(char value, Action<char, char> onCompare) => value;
        protected override char NewTValue(char value, Action<char, char> onCompare) => value;
    }

    public class EqualsToSeq_int : EqualsToSeq<int, int, int>
    {
        protected override int NewT(int value) => value;
        protected override int NewTSource(int value, Action<int, int> onCompare) => value;
        protected override int NewTValue(int value, Action<int, int> onCompare) => value;
    }

    public class EqualsToSeq_string : EqualsToSeq<string, string, string>
    {
        protected override string NewT(int value) => value.ToString();
        protected override string NewTSource(string value, Action<string, string> onCompare) => value;
        protected override string NewTValue(string value, Action<string, string> onCompare) => value;
    }

    public class EqualsToSeq_intEE : EqualsToSeq<int, TEquatable<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class EqualsToSeq_intEO : EqualsToSeq<int, TEquatable<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TEquatable<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class EqualsToSeq_intOE : EqualsToSeq<int, TObject<int>, TEquatable<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare)
        {
            var result = new TObject<int>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        protected override TEquatable<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TEquatable<int>(value, onCompare);
    }

    public class EqualsToSeq_intOO : EqualsToSeq<int, TObject<int>, TObject<int>>
    {
        protected override int NewT(int value) => value;
        protected override TObject<int> NewTSource(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
        protected override TObject<int> NewTValue(int value, Action<int, int> onCompare) =>
            new TObject<int>(value, onCompare);
    }

    public class EqualsToSeq_stringEE : EqualsToSeq<string, TEquatable<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public class EqualsToSeq_stringEO : EqualsToSeq<string, TEquatable<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TEquatable<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
    }

    public class EqualsToSeq_stringOE : EqualsToSeq<string, TObject<string>, TEquatable<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare)
        {
            var result = new TObject<string>(value, onCompare);
            result.OnCompare += (x, y) => { throw new Exception("Detected Object.Equals comparition call"); };
            return result;
        }
        protected override TEquatable<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TEquatable<string>(value, onCompare);
    }

    public class EqualsToSeq_stringOO : EqualsToSeq<string, TObject<string>, TObject<string>>
    {
        protected override string NewT(int value) => value.ToString();
        protected override TObject<string> NewTSource(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
        protected override TObject<string> NewTValue(string value, Action<string, string> onCompare) =>
            new TObject<string>(value, onCompare);
    }
}
