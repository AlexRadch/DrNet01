// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time IEquatable.Equals() is invoked.
    public struct TEquatableInt: IEquatable<TEquatableInt>, IEquatable<TObjectInt>
    {
        private readonly int _handle;

        public TEquatableInt(int value, int handle)
        {
            _handle = handle;
            Value = value;
        }

        public int Value { get; }

        private bool Equals(int other)
        {
            if (_handle < -1)
                throw new Exception("Detected Object.Equals comparition call");
            OnCompareActions<int>.OnCompare(_handle, Value, other);
            return Value.Equals(other);
        }

        public bool Equals(TEquatableInt other) => Equals(other.Value);

        public bool Equals(TObjectInt other) => Equals(other.Value);

        public override bool Equals(object obj) => throw new NotImplementedException();

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
