// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DrNet.Tests
{
    public sealed class TLog<T>
    {
        public void Add(T x, T y) => _log.Add(Tuple.Create(x, y));
        public int Count => _log.Count;
        public int CountCompares(T x, T y) => _log.Where(t => (t.Item1.Equals(x) && t.Item2.Equals(y)) || (t.Item1.Equals(y) && t.Item2.Equals(x))).Count();

        private List<Tuple<T, T>> _log = new List<Tuple<T, T>>();
    }
}
