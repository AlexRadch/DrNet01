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

        private bool disposedValue = false; // ��� ����������� ���������� �������

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: ���������� ����������� ��������� (����������� �������).
                }

                // TODO: ���������� ������������� ������� (������������� �������) � �������������� ���� ����� ����������.
                // TODO: ������ ������� ����� �������� NULL.

                OnCompareActions<T>.Remove(_handle, Add);
                _handle = 0;
            }
        }

        // TODO: �������������� ����� ����������, ������ ���� Dispose(bool disposing) ���� �������� ��� ��� ������������ ������������� ��������.
        ~TLog()
        {
            // �� ��������� ���� ���. ���������� ��� ������� ����, � ������ Dispose(bool disposing).
            Dispose(false);
        }

        // ���� ��� �������� ��� ���������� ���������� ������� ��������������� ������.
        public void Dispose()
        {
            // �� ��������� ���� ���. ���������� ��� ������� ����, � ������ Dispose(bool disposing).
            Dispose(true);
            // TODO: ����������������� ��������� ������, ���� ����� ���������� ������������� ����.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
