using System;
using System.Collections.Generic;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class IList_UnsafeReadOnlySpan_Tests<T> : IList_Tests<T>
    {
        #region ICollection<T> Helper Methods

        protected override bool IsReadOnly => true;

        #endregion

        #region IList<T> Helper Methods

        /// <summary>
        /// Creates an instance of an IList{T} that can be used for testing.
        /// </summary>
        /// <param name="count">The number of unique items that the returned IList{T} contains.</param>
        /// <returns>An instance of an IList{T} that can be used for testing.</returns>
        protected override IList<T> GenericIListFactory(int count)
        {
            Span<T> span = GenericPinnedFactory(count);
            return new UnsafeReadOnlySpan<T>(span);
        }

        #endregion
    }

    public class IList_UnsafeReadOnlySpan_Tests_byte : IList_UnsafeReadOnlySpan_Tests<byte>
    {
        #region TestBase<T> Helper Methods

        protected override byte CreateT(int seed) => unchecked((byte)new Random(seed).Next(int.MinValue, int.MaxValue));

        #endregion
    }

    public class IList_UnsafeReadOnlySpan_Tests_char : IList_UnsafeReadOnlySpan_Tests<char>
    {
        #region TestBase<T> Helper Methods

        protected override char CreateT(int seed) => unchecked((char)new Random(seed).Next(int.MinValue, int.MaxValue));

        #endregion
    }

    public class IList_UnsafeReadOnlySpan_Tests_int : IList_UnsafeReadOnlySpan_Tests<int>
    {
        #region TestBase<T> Helper Methods

        protected override int CreateT(int seed) => new Random(seed).Next(int.MinValue, int.MaxValue);

        #endregion
    }
}
