using System;
using System.Collections.Generic;

using DrNet.Unsafe;

namespace DrNet.Tests.UnsafeSpan
{
    public abstract class UnsafeSpan_IList_Tests<T> : IList_Tests<T>
    {
        #region IList<T> Helper Methods

        /// <summary>
        /// Creates an instance of an IList{T} that can be used for testing.
        /// </summary>
        /// <param name="count">The number of unique items that the returned IList{T} contains.</param>
        /// <returns>An instance of an IList{T} that can be used for testing.</returns>
        protected override IList<T> GenericIListFactory(int count)
        {
            T[] array = GenericArrayFactory(count);
            return new UnsafeSpan<T>(array.AsSpan());
        }

        #endregion

    }

    public class UnsafeSpan_IList_Tests_byte : UnsafeSpan_IList_Tests<byte>
    {
        #region TestBase<T> Helper Methods

        protected override byte CreateT(int seed) => unchecked((byte)new Random(seed).Next(0, 256));

        #endregion
    }
}
