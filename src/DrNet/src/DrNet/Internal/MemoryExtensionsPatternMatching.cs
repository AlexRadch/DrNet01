﻿using System;

namespace DrNet.Internal
{
    // See https://github.com/dotnet/csharplang/issues/1840#issuecomment-419456424
    public abstract class IEquatablePatternMatchingBase<T>
    {
        public static IEquatablePatternMatchingBase<T> Instance = (IEquatablePatternMatchingBase<T>)Activator.CreateInstance(
            typeof(IEquatablePatternMatching<>).MakeGenericType(typeof(T)));

        // Pattern matching for System.MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, T) where T : IEquatable{T}
        public abstract int IndexOf(ReadOnlySpan<T> span, T value);

        // Pattern matching for System.MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, T) where T : IEquatable{T}
        public abstract int LastIndexOf(ReadOnlySpan<T> span, T value);
    
        // Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T) where T : IEquatable{T}
        public abstract int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1);
        // Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, T, T, T) where T : IEquatable{T}
        public abstract int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2);
        // Pattern matching for System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) where T : IEquatable{T}
        public abstract int IndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values);

        // Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T) where T : IEquatable{T}
        public abstract int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1);
        // Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, T, T, T) where T : IEquatable{T}
        public abstract int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2);
        // Pattern matching for System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) where T : IEquatable{T}
        public abstract int LastIndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values);

        // Pattern matching for System.MemoryExtensions.SequenceEqual{T}(ReadOnlySpan{T}, ReadOnlySpan{T}) where T : IEquatable{T}
        public abstract bool SequenceEqual(ReadOnlySpan<T> span, ReadOnlySpan<T> other);
    }

    public sealed class IEquatablePatternMatching<T> : IEquatablePatternMatchingBase<T> where T : IEquatable<T>
    {
        public override int IndexOf(ReadOnlySpan<T> span, T value) => System.MemoryExtensions.IndexOf(span, value);
        public override int LastIndexOf(ReadOnlySpan<T> span, T value) => System.MemoryExtensions.LastIndexOf(span, value);
        public override int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1) => System.MemoryExtensions.IndexOfAny(span, value0, value1);
        public override int IndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2) => System.MemoryExtensions.IndexOfAny(span, value0, value1, value2);
        public override int IndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values) => System.MemoryExtensions.IndexOfAny(span, values);
        public override int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1) => System.MemoryExtensions.LastIndexOfAny(span, value0, value1);
        public override int LastIndexOfAny(ReadOnlySpan<T> span, T value0, T value1, T value2) => System.MemoryExtensions.LastIndexOfAny(span, value0, value1, value2);
        public override int LastIndexOfAny(ReadOnlySpan<T> span, ReadOnlySpan<T> values) => System.MemoryExtensions.LastIndexOfAny(span, values);
        public override bool SequenceEqual(ReadOnlySpan<T> span, ReadOnlySpan<T> other) => System.MemoryExtensions.SequenceEqual(span, other);
    }   
}
