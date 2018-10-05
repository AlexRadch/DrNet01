using System.Runtime.CompilerServices;

namespace DrNet.Internal
{
    public static class DrNetTypeExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTypeComparableAsBytes<T>()
        {
            return typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) ||
                typeof(T) == typeof(char) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(int) || typeof(T) == typeof(uint) ||
                typeof(T) == typeof(long) || typeof(T) == typeof(ulong);
        }
    }
}
