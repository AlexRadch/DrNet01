// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TObject<T>
    {
        public TObject(T value, Action<T, T> onCompare = null)
        {
            Value = value;
            OnCompare = default;

            if (onCompare != null)
                OnCompare += onCompare;
        }

        private bool Equals(T other)
        {
            OnCompare?.Invoke(Value, other);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other);
            return Value.Equals(other);
        }

        private bool Equals(TObject<T> other)
        {
            return Equals(other.Value);
        }

        private bool Equals(TEquatable<T> other)
        {
            return Equals(other.Value);
        }

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

        public override string ToString()
        {
            return Value.ToString();
        }

        public T Value { get; }

        public event Action<T, T> OnCompare;
    }
}
