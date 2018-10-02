// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TEquatable<T>: IEquatable<TEquatable<T>>, IEquatable<TObject<T>>
    {
        public TEquatable(T value, Action<T, T> onCompare = null)
        {
            Value = value;
            OnCompareTEquatableT = default;
            OnCompareTObjectT = default;

            if (onCompare != null)
            {
                OnCompareTEquatableT += onCompare;
                OnCompareTObjectT += onCompare;
            }
        }

        public bool Equals(TEquatable<T> other)
        {
            OnCompareTEquatableT?.Invoke(Value, other.Value);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other.Value);
            return Value.Equals(other.Value);
        }

        public bool Equals(TObject<T> other)
        {
            OnCompareTObjectT?.Invoke(Value, other.Value);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other.Value);
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            return Value.ToString();
        }

        public T Value { get; }

        public event Action<T, T> OnCompareTEquatableT;
        public event Action<T, T> OnCompareTObjectT;
    }
}
