// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped integer that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TEquatable<T>: IEquatable<T>, IEquatable<TEquatable<T>>, IEquatable<TObject<T>>
    {
        public TEquatable(T value)
            : this(value, (Action<T, T>)null)
        {
            // This constructor does not report comparisons but is still useful for catching uses of the boxing Equals().
        }

        public TEquatable(T value, Action<T, T> onCompare)
        {
            Value = value;
            OnCompareT = default;
            OnCompareTEquatableT = default;
            OnCompareTObjectT = default;

            if (onCompare != null)
            {
                OnCompareT += onCompare;
                OnCompareTEquatableT += onCompare;
                OnCompareTObjectT += onCompare;
            }
        }

        public TEquatable(T value, TLog<T> log) : this (value, log.Add) { }

        public bool Equals(T other)
        {
            OnCompareT?.Invoke(Value, other);
            if (Value is IEquatable<T> equatable)
                return equatable.Equals(other);
            return Value.Equals(other);
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

        public event Action<T, T> OnCompareT;
        public event Action<T, T> OnCompareTEquatableT;
        public event Action<T, T> OnCompareTObjectT;
    }
}
