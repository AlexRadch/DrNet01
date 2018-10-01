using System;
using System.Collections.Generic;
using System.Text;

namespace DrNet.Internal.Unsafe
{
    public readonly unsafe struct SpanInternal
    {
        public readonly void* _pointer;
        public readonly int _length;

        public SpanInternal(void* pointer, int length)
        {
            _pointer = pointer;
            _length = length;
        }
    }
}
