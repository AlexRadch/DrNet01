// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet
{
    // A wrapped integer that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TObject<T>
    {
        public TObject(T value)
            : this(value, (Action<T, T>)null)
        {
            // This constructor does not report comparisons but is still useful for catching uses of the boxing Equals().
        }

        public TObject(T value, Action<T, T> onCompare)
        {
            Value = value;
            _onCompare = onCompare;
        }

        public TObject(T value, TLog<T> log)
        {
            Value = value;
            _onCompare = (x, y) => log.Add(x, y);
        }

        public bool Equals(TObject<T> other)
        {
            _onCompare?.Invoke(Value, other.Value);
            return Value.Equals(other.Value);
        }

        public bool Equals(T other)
        {
            _onCompare?.Invoke(Value, Value);
            return Value.Equals(Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is TObject<T> other)
                return Equals(other);
            if (obj is T otherT)
                return Equals(otherT);
            throw new NotImplementedException();
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            return Value.ToString();
        }

        public T Value { get; }

        private Action<T, T> _onCompare;
    }
}
