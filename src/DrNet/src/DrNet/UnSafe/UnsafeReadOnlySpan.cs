using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using DrNet.Internal;
using DrNet.Internal.UnSafe;

namespace DrNet.UnSafe
{
    [DebuggerTypeProxy(typeof(UnsafeSpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    public readonly unsafe struct UnsafeReadOnlySpan<T> :
        IList<T>, IReadOnlyList<T>, ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable,
        IEquatable<UnsafeSpan<T>>, IEquatable<UnsafeReadOnlySpan<T>>//,
        //IComparable<UnsafeSpan<T>>, IComparable<UnsafeReadOnlySpan<T>>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly void* _pointer;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeReadOnlySpan(void* pointer, int length)
        {
            if (length < 0 || pointer == null && length > 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            _pointer = pointer;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeReadOnlySpan(ReadOnlySpan<T> span) : this(in DrNetMarshal.GetReference(span), span.Length) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeReadOnlySpan(in T reference, int length) : this(UnsafeIn.AsPointer(in reference), length) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsSpan() => DrNetMarshal.CreateReadOnlySpan(in UnsafeIn.AsRef<T>(_pointer), _length);

        public ref readonly T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return ref Unsafe.Add(ref Unsafe.AsRef<T>(_pointer), index);
            }
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 0 >= (uint)_length; // Workaround for https://github.com/dotnet/coreclr/issues/19620
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void Clear() => AsSpan().Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(Span<T> destination) => AsSpan().CopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(UnsafeSpan<T> other) => _pointer == other._pointer && _length == other.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(UnsafeReadOnlySpan<T> other) => _pointer == other._pointer && _length == other.Length;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (obj is UnsafeSpan<T> oSpan)
                return Equals(oSpan);
            if (obj is UnsafeReadOnlySpan<T> orSpan)
                return Equals(orSpan);
            return false;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void Fill(T value) => AsSpan().Fill(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => 
            DrNetFastHashCode.CombineHashCodes(((IntPtr)_pointer).GetHashCode(), _length.GetHashCode());

        /// <summary>
        /// Returns a reference to the 0th element of the UnsafeSpan. If the Span is empty, returns null reference.
        /// It can be used for pinning and is required to support the use of span within a fixed statement.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref readonly T GetPinnableReference() => ref (_length != 0) ? ref Unsafe.AsRef<T>(_pointer) :
            ref Unsafe.AsRef<T>(null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeReadOnlySpan<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new UnsafeReadOnlySpan<T>(in Unsafe.Add(ref Unsafe.AsRef<T>(_pointer), start),
                _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeReadOnlySpan<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                throw new ArgumentOutOfRangeException(nameof(start));

            return new UnsafeReadOnlySpan<T>(in Unsafe.Add(ref Unsafe.AsRef<T>(_pointer), start), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => AsSpan().ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            if (typeof(T) == typeof(char))
                return new string((char*)_pointer, 0, _length);
            return string.Format("DrNet.UnsafeReadOnlySpan<{0}>[{1}]", typeof(T).Name, _length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryCopyTo(Span<T> destination) => AsSpan().TryCopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnsafeReadOnlySpan<T> left, UnsafeReadOnlySpan<T> right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnsafeReadOnlySpan<T> left, UnsafeReadOnlySpan<T> right) => !left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnsafeReadOnlySpan<T>(UnsafeSpan<T> span) => 
            new UnsafeReadOnlySpan<T>(span._pointer, span._length);

        T IList<T>.this[int index] { get => this[index]; set => throw new InvalidOperationException(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            if (typeof(T) == typeof(byte))
                return MemoryExtensions.IndexOf(DrNetMarshal.CreateReadOnlySpan(in UnsafeIn.AsRef<byte>(_pointer),
                    _length), UnsafeIn.As<T, byte>(in item));
            if (typeof(T) == typeof(char))
                return MemoryExtensions.IndexOf(DrNetMarshal.CreateReadOnlySpan(in UnsafeIn.AsRef<char>(_pointer),
                    _length), UnsafeIn.As<T, char>(in item));
            if (item is IEquatable<T> vEquatable)
                return DrNetSpanHelpers.IndexOfEqualValueComparer(in UnsafeIn.AsRef<T>(_pointer), _length,
                    vEquatable, (eValue, sValue) => eValue.Equals(sValue));
            return DrNetSpanHelpers.IndexOfEqualValueComparer(in UnsafeIn.AsRef<T>(_pointer), _length, item,
                (vValue, sValue) => vValue.Equals(sValue));
        }

        void IList<T>.Insert(int index, T item) => throw new InvalidOperationException();

        void IList<T>.RemoveAt(int index) => throw new InvalidOperationException();

        T IReadOnlyList<T>.this[int index] => this[index];

        int ICollection<T>.Count { get => _length; }

        bool ICollection<T>.IsReadOnly { get => false; }

        void ICollection<T>.Add(T item) => throw new InvalidOperationException();

        void ICollection<T>.Clear() => throw new InvalidOperationException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => IndexOf(item) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array.AsSpan(arrayIndex));

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();

        int IReadOnlyCollection<T>.Count { get => _length; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator: IEnumerator<T>, IEnumerator, IDisposable
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public readonly UnsafeReadOnlySpan<T> _span;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public int _index;

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Enumerator(UnsafeReadOnlySpan<T> span)
            {
                _span = span;
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int index = _index + 1;
                if (index < _span.Length)
                {
                    _index = index;
                    return true;
                }

                return false;
            }

            public ref readonly T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _span[_index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                _index = -1;
            }

            T IEnumerator<T>.Current => Current;

            object IEnumerator.Current => Current;

            void IDisposable.Dispose() { }
        }

    }
}
