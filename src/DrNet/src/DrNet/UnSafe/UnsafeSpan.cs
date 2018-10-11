using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;

using DrNet.Internal;
using DrNet.Internal.Unsafe;

namespace DrNet.Unsafe
{
    [DebuggerTypeProxy(typeof(UnsafeSpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    public readonly unsafe struct UnsafeSpan<T> :
        IList<T>, IReadOnlyList<T>, ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable,
        IEquatable<UnsafeSpan<T>>, IEquatable<UnsafeReadOnlySpan<T>>
        //IComparable<UnsafeSpan<T>>, IComparable<UnsafeReadOnlySpan<T>>
    {
        // NOTE: With the current implementation, UnsafeSpan<T> and UnsafeReadOnlySpan<T> must have the same layout,
        // as code uses Unsafe.As to cast between them.

        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly void* _pointer;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeSpan(void* pointer, int length)
        {
            if (length < 0 || pointer == null && length > 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            _pointer = pointer;
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeSpan(Span<T> span) : this(ref DrNetMarshal.GetReference(span), span.Length) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeSpan(ref T reference, int length) : this(UnsafeRef.AsPointer(ref reference), length) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan() => DrNetMarshal.CreateSpan(ref UnsafeRef.AsRef<T>(_pointer), _length);

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return ref UnsafeRef.Add(ref UnsafeRef.AsRef<T>(_pointer), index);
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

        public void Clear() => AsSpan().Clear();

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

        public void Fill(T value) => AsSpan().Fill(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            DrNetFastHashCode.CombineHashCodes(((IntPtr)_pointer).GetHashCode(), _length.GetHashCode());

        /// <summary>
        /// Returns a reference to the 0th element of the UnsafeSpan. If the Span is empty, returns null reference.
        /// It can be used for pinning and is required to support the use of span within a fixed statement.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetPinnableReference() => ref (_length != 0) ? ref UnsafeRef.AsRef<T>(_pointer) :
            ref UnsafeRef.AsRef<T>(null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeSpan<T> Slice(int start)
        {
            if ((uint)start > (uint)_length)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new UnsafeSpan<T>(ref UnsafeRef.Add(ref UnsafeRef.AsRef<T>(_pointer), start), _length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnsafeSpan<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
                throw new ArgumentOutOfRangeException(nameof(start));

            return new UnsafeSpan<T>(ref UnsafeRef.Add(ref UnsafeRef.AsRef<T>(_pointer), start), length);
        }

        public T[] ToArray() => AsSpan().ToArray();

        public override string ToString()
        {
            if (typeof(T) == typeof(char))
                return new string((char*)_pointer, 0, _length);
            return string.Format("DrNet.UnsafeSpan<{0}>[{1}]", typeof(T).Name, _length);
        }

        public bool TryCopyTo(Span<T> destination) => AsSpan().TryCopyTo(destination);

        #region operator == !=

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnsafeSpan<T> left, UnsafeSpan<T> right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnsafeSpan<T> left, UnsafeReadOnlySpan<T> right) => left.Equals(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnsafeSpan<T> left, Span<T> right) =>
            left._pointer == UnsafeIn.AsPointer(in DrNetMarshal.GetReference(right)) && left._length == right.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnsafeSpan<T> left, ReadOnlySpan<T> right) =>
            left._pointer == UnsafeIn.AsPointer(in DrNetMarshal.GetReference(right)) && left._length == right.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Span<T> left, UnsafeSpan<T> right) => right == left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ReadOnlySpan<T> left, UnsafeSpan<T> right) => right == left;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnsafeSpan<T> left, UnsafeSpan<T> right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnsafeSpan<T> left, UnsafeReadOnlySpan<T> right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnsafeSpan<T> left, Span<T> right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnsafeSpan<T> left, ReadOnlySpan<T> right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Span<T> left, UnsafeSpan<T> right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ReadOnlySpan<T> left, UnsafeSpan<T> right) => !(left == right);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnsafeReadOnlySpan<T>(UnsafeSpan<T> span) =>
            UnsafeIn.As<UnsafeSpan<T>, UnsafeReadOnlySpan<T>>(in span);

        T IList<T>.this[int index] { get => this[index]; set => this[index] = value; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            if (typeof(T) == typeof(byte))
                return MemoryExtensions.IndexOf(DrNetMarshal.CreateSpan(ref UnsafeRef.AsRef<byte>(_pointer), _length),
                    UnsafeIn.As<T, byte>(in item));
            if (typeof(T) == typeof(char) )
                return MemoryExtensions.IndexOf(DrNetMarshal.CreateSpan(ref UnsafeRef.AsRef<char>(_pointer), _length),
                    UnsafeIn.As<T, char>(in item));
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

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array.AsSpan(arrayIndex));

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException();

        int IReadOnlyCollection<T>.Count { get => _length; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator: IEnumerator<T>, IEnumerator, IDisposable
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public readonly UnsafeSpan<T> _span;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public int _index;

            [EditorBrowsable(EditorBrowsableState.Never)]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(UnsafeSpan<T> span)
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

            public ref T Current
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
