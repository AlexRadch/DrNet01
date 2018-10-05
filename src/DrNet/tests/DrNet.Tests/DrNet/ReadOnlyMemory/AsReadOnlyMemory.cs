using System;
using Xunit;

namespace DrNet.Tests.ReadOnlyMemory
{
    public static class AsReadOnlyMemory
    {
        #region Array

        [Fact]
        public static void ArrayDefault()
        {
            int[] a = default;
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(a);
            Assert.Equal(default, m);
            m = DrNetMemoryExt.AsReadOnlyMemory(a, 0);
            Assert.Equal(default, m);
            m = DrNetMemoryExt.AsReadOnlyMemory(a, 0, 0);
            Assert.Equal(default, m);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(10)]
        public static void Array(int length)
        {
            int[] a = new int[length];
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(a);
            Assert.Equal(length, m.Length);
            if (length > 0)
            {
                a[0] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 0)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(3, 3)]
        [InlineData(10, 0)]
        [InlineData(10, 3)]
        [InlineData(10, 10)]
        public static void ArrayWithStart(int length, int start)
        {
            int[] a = new int[length];
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(a, start);
            Assert.Equal(length - start, m.Length);
            if (start != length)
            {
                a[start] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(3, 0, 3)]
        [InlineData(3, 1, 2)]
        [InlineData(3, 2, 1)]
        [InlineData(3, 3, 0)]
        [InlineData(10, 0, 5)]
        [InlineData(10, 3, 2)]
        public static void ArrayWithStartAndLength(int length, int start, int subLength)
        {
            int[] a = new int[length];
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(a, start, subLength);
            Assert.Equal(subLength, m.Length);
            if (subLength != 0)
            {
                a[start] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public static void ArrayDefaultWithStartNegative(int start)
        {
            int[] a = default;
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(a, start));
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(0, 1)]
        [InlineData(5, 6)]
        public static void ArrayWithStartNegative(int length, int start)
        {
            int[] a = new int[length];
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(a, start));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(1, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 1)]
        public static void ArrayDefaultWithStartAndLengthNegative(int start, int subLength)
        {
            int[] a = default;
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(a, start, subLength));
        }

        [Theory]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, -1)]
        [InlineData(0, 0, 1)]
        [InlineData(5, 6, 0)]
        [InlineData(5, 3, 3)]
        public static void ArrayWithStartAndLengthNegative(int length, int start, int subLength)
        {
            int[] a = new int[length];
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(a, start, subLength));
        }

        #endregion

        #region ArraySegment

        [Fact]
        public static void ArraySegmentDefault()
        {
            ArraySegment<int> segment = default;
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(segment);
            Assert.Equal(default, m);
            m = DrNetMemoryExt.AsReadOnlyMemory(segment, 0);
            Assert.Equal(default, m);
            m = DrNetMemoryExt.AsReadOnlyMemory(segment, 0, 0);
            Assert.Equal(default, m);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(10)]
        public static void ArraySegment(int length)
        {
            const int segmentOffset = 5;

            int[] a = new int[length + segmentOffset];
            ArraySegment<int> segment = new ArraySegment<int>(a, 5, length);
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(segment);
            Assert.Equal(length, m.Length);
            if (m.Length != 0)
            {
                a[segmentOffset] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 0)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(3, 3)]
        [InlineData(10, 0)]
        [InlineData(10, 3)]
        [InlineData(10, 10)]
        public static void ArraySegmentWithStart(int length, int start)
        {
            const int segmentOffset = 5;

            int[] a = new int[length + segmentOffset];
            ArraySegment<int> segment = new ArraySegment<int>(a, 5, length);
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(segment, start);
            Assert.Equal(length - start, m.Length);
            if (m.Length != 0)
            {
                a[segmentOffset + start] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(3, 0, 3)]
        [InlineData(3, 1, 2)]
        [InlineData(3, 2, 1)]
        [InlineData(3, 3, 0)]
        [InlineData(10, 0, 5)]
        [InlineData(10, 3, 2)]
        public static void ArraySegmentWithStartAndLength(int length, int start, int subLength)
        {
            const int segmentOffset = 5;

            int[] a = new int[length + segmentOffset];
            ArraySegment<int> segment = new ArraySegment<int>(a, segmentOffset, length);
            ReadOnlyMemory<int> m = DrNetMemoryExt.AsReadOnlyMemory(segment, start, subLength);
            Assert.Equal(subLength, m.Length);
            if (subLength != 0)
            {
                a[segmentOffset + start] = 42;
                Assert.Equal(42, m.Span[0]);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public static void ArraySegmentDefaultWithStartNegative(int start)
        {
            ArraySegment<int> segment = default;
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(segment, start));
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(0, 1)]
        [InlineData(5, 6)]
        public static void ArraySegmentWithStartNegative(int length, int start)
        {
            const int segmentOffset = 5;

            int[] a = new int[length + segmentOffset];
            ArraySegment<int> segment = new ArraySegment<int>(a, segmentOffset, length);
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(segment, start));
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(1, 0)]
        [InlineData(0, -1)]
        [InlineData(0, 1)]
        public static void ArraySegmentDefaultWithStartAndLengthNegative(int start, int subLength)
        {
            ArraySegment<int> segment = default;
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(segment, start, subLength));
        }

        [Theory]
        [InlineData(0, -1, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, -1)]
        [InlineData(0, 0, 1)]
        [InlineData(5, 6, 0)]
        [InlineData(5, 3, 3)]
        public static void ArraySegmentWithStartAndLengthNegative(int length, int start, int subLength)
        {
            const int segmentOffset = 5;

            int[] a = new int[length + segmentOffset];
            ArraySegment<int> segment = new ArraySegment<int>(a, segmentOffset, length);
            Assert.Throws<ArgumentOutOfRangeException>(() => DrNetMemoryExt.AsReadOnlyMemory(segment, start, subLength));
        }

        #endregion
    }
}
