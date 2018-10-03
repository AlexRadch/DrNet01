// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time IEquatable.Equals() is invoked.
    public struct TEquatable<T>: IEquatable<TEquatable<T>>, IEquatable<TObject<T>>
    {
        private readonly int _handle;

        public TEquatable(T value, int handle)
        {
            _handle = handle;
            Value = value;
        }

        public T Value { get; }

        private bool Equals(T other)
        {
            if (_handle < -1)
                throw new Exception("Detected Object.Equals comparition call");
            OnCompareActions<T>.OnCompare(_handle, Value, other);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other);
            return Value.Equals(other);
        }

        public bool Equals(TEquatable<T> other) => Equals(other.Value);

        public bool Equals(TObject<T> other) => Equals(other.Value);

        public override bool Equals(object obj) => throw new NotImplementedException();

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
