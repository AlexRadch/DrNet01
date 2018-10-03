// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DrNet.Tests
{
    public sealed class TLog<T> : IDisposable
    {
        public TLog(int handle)
        {
            OnCompareActions<T>.Add(handle, Add);
            _handle = handle;
        }

        private int _handle;
        private List<Tuple<T, T>> _log = new List<Tuple<T, T>>();

        private void Add(T x, T y) => _log.Add(Tuple.Create(x, y));

        public int Count => _log.Count;
        public int CountCompares(T x, T y) => _log.Where(t => (t.Item1.Equals(x) && t.Item2.Equals(y)) || (t.Item1.Equals(y) && t.Item2.Equals(x))).Count();
        public void Clear() => _log.Clear();

        #region IDisposable Support

        private bool disposedValue = false; // Для определения избыточных вызовов

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                OnCompareActions<T>.Remove(_handle, Add);
                _handle = 0;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        ~TLog()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(false);
        }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
