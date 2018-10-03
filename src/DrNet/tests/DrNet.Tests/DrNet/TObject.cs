// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TObject<T>
    {
        private readonly int _handle;

        public TObject(T value, int handle = 0)
        {
            _handle = handle;
            Value = value;
        }

        public T Value { get; }

        private bool Equals(T other)
        {
            if (_handle < 0)
                throw new Exception("Detected Object.Equals comparition call");
            OnCompareActions<T>.OnCompare(_handle, Value, other);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other);
            return Value.Equals(other);
        }

        private bool Equals(TObject<T> other) => Equals(other.Value);

        private bool Equals(TEquatable<T> other) => Equals(other.Value);

        public override bool Equals(object obj)
        {
            if (obj is T otherT)
                return Equals(otherT);
            if (obj is TObject<T> otherO)
                return Equals(otherO);
            if (obj is TEquatable<T> otherE)
                return Equals(otherE);
            throw new NotImplementedException();
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
