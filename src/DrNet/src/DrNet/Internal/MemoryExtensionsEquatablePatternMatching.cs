using System;
using System.Runtime.CompilerServices;

namespace DrNet.Internal
{
    ///// See https://github.com/dotnet/csharplang/issues/1840#issuecomment-419456424
    //public abstract class MemoryExtensionsEquatablePatternMatching<T>
    //{
    //    public static MemoryExtensionsEquatablePatternMatching<T> Instance = 
    //        (MemoryExtensionsEquatablePatternMatching<T>)Activator.CreateInstance(
    //            typeof(MemoryExtensionsEquatablePatternMatchingImplementation<>).MakeGenericType(typeof(T)));

    //    /// Pattern matching for System.MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, T) where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int IndexOf(ReadOnlySpan<T> span, T value);
    //    /// Pattern matching for System.MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int IndexOf(ReadOnlySpan<T> span, ReadOnlySpan<T> value);

    //    /// Pattern matching for System.MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, T) where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int LastIndexOf(ReadOnlySpan<T> span, T value);
    //    /// Pattern matching for System.MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int LastIndexOf(ReadOnlySpan<T> span, ReadOnlySpan<T> value);

    //    /// Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T) where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1);
    //    /// Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T, T) where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2);
    //    /// Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int IndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values);

    //    /// Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1);
    //    /// Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T, T) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2);
    //    /// Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) 
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract int LastIndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values);

    //    /// Pattern matching for System.MemoryExtensions.SequenceEqual{T}(ReadOnlySpan{T}, ReadOnlySpan{T})
    //    /// where T : IEquatable{T}
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public abstract bool SequenceEqual(ReadOnlySpan<T> span, ReadOnlySpan<T> other);
    //}

    //public sealed class MemoryExtensionsEquatablePatternMatchingImplementation<T> : 
    //    MemoryExtensionsEquatablePatternMatching<T> where T : IEquatable<T>
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int IndexOf(ReadOnlySpan<T> span, T value) => MemoryExtensions.IndexOf(span, value);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int IndexOf(ReadOnlySpan<T> span, ReadOnlySpan<T> value) => 
    //        MemoryExtensions.IndexOf(span, value);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int LastIndexOf(ReadOnlySpan<T> span, T value) => MemoryExtensions.LastIndexOf(span, value);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int LastIndexOf(ReadOnlySpan<T> span, ReadOnlySpan<T> value) => 
    //        MemoryExtensions.LastIndexOf(span, value);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1) => 
    //        MemoryExtensions.IndexOfAny(span, value0, value1);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2) => 
    //        MemoryExtensions.IndexOfAny(span, value0, value1, value2);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int IndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values) => 
    //        MemoryExtensions.IndexOfAny(span, values);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1) => 
    //        MemoryExtensions.LastIndexOfAny(span, value0, value1);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2) => 
    //        MemoryExtensions.LastIndexOfAny(span, value0, value1, value2);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override int LastIndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values) => 
    //        MemoryExtensions.LastIndexOfAny(span, values);
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    public override bool SequenceEqual(ReadOnlySpan<T> span, ReadOnlySpan<T> other) => 
    //        MemoryExtensions.SequenceEqual(span, other);
    //}   
}
