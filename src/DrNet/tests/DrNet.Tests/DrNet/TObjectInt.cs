// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace DrNet.Tests
{
    // A wrapped T that invokes a custom delegate every time Object.Equals() is invoked.
    public struct TObjectInt
    {
        private readonly int _handle;

        public TObjectInt(int value, int handle = 0)
        {
            _handle = handle;
            Value = value;
        }

        public int Value { get; }

        private bool Equals(int other)
        {
            if (_handle < 0)
                throw new Exception("Detected Object.Equals comparition call");
            OnCompareActions<int>.OnCompare(_handle, Value, other);
            return Value.Equals(other);
        }

        private bool Equals(TObjectInt other) => Equals(other.Value);

        private bool Equals(TEquatableInt other) => Equals(other.Value);

        public override bool Equals(object obj)
        {
            if (obj is TObjectInt otherO)
                return Equals(otherO);
            if (obj is TEquatableInt otherE)
                return Equals(otherE);
            throw new NotImplementedException();
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
