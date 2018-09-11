using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DrNet.Internal
{
    public static class SpanHelpers
    {
        #region IndexOf

        public static unsafe int IndexOfObjectEquals<T>(ref T searchSpace, T value, int length)
        {
            Debug.Assert(length >= 0);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 4)))
                    goto Found4;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 5)))
                    goto Found5;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 6)))
                    goto Found6;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 7)))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return (int)(byte*)index;
        Found1:
            return (int)(byte*)(index + 1);
        Found2:
            return (int)(byte*)(index + 2);
        Found3:
            return (int)(byte*)(index + 3);
        Found4:
            return (int)(byte*)(index + 4);
        Found5:
            return (int)(byte*)(index + 5);
        Found6:
            return (int)(byte*)(index + 6);
        Found7:
            return (int)(byte*)(index + 7);
        }

        public static unsafe int IndexOf<T>(ref T searchSpace, IEquatable<T> value, int length)
        {
            Debug.Assert(length >= 0);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 4)))
                    goto Found4;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 5)))
                    goto Found5;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 6)))
                    goto Found6;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 7)))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return (int)(byte*)index;
        Found1:
            return (int)(byte*)(index + 1);
        Found2:
            return (int)(byte*)(index + 2);
        Found3:
            return (int)(byte*)(index + 3);
        Found4:
            return (int)(byte*)(index + 4);
        Found5:
            return (int)(byte*)(index + 5);
        Found6:
            return (int)(byte*)(index + 6);
        Found7:
            return (int)(byte*)(index + 7);
        }

        public static unsafe int IndexOf<T>(ref T searchSpace, T value, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 4)))
                    goto Found4;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 5)))
                    goto Found5;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 6)))
                    goto Found6;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 7)))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, index)))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return (int)(byte*)index;
        Found1:
            return (int)(byte*)(index + 1);
        Found2:
            return (int)(byte*)(index + 2);
        Found3:
            return (int)(byte*)(index + 3);
        Found4:
            return (int)(byte*)(index + 4);
        Found5:
            return (int)(byte*)(index + 5);
        Found6:
            return (int)(byte*)(index + 6);
        Found7:
            return (int)(byte*)(index + 7);
        }

        #endregion

        #region LastIndexOf

        public static unsafe int LastIndexOfObjectEquals<T>(ref T searchSpace, T value, int length)
        {
            Debug.Assert(length >= 0);

            while (length >= 8)
            {
                length -= 8;

                if (value.Equals(Unsafe.Add(ref searchSpace, length + 7)))
                    goto Found7;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 6)))
                    goto Found6;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 5)))
                    goto Found5;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 4)))
                    goto Found4;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (value.Equals(Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;

        }

        public static unsafe int LastIndexOf<T>(ref T searchSpace, T value, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            while (length >= 8)
            {
                length -= 8;

                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 7)))
                    goto Found7;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 6)))
                    goto Found6;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 5)))
                    goto Found5;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 4)))
                    goto Found4;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (equalityComparer.Equals(value, Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;

        }

        public static unsafe int LastIndexOf<T>(ref T searchSpace, IEquatable<T> value, int length)
        {
            Debug.Assert(length >= 0);

            while (length >= 8)
            {
                length -= 8;

                if (value.Equals(Unsafe.Add(ref searchSpace, length + 7)))
                    goto Found7;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 6)))
                    goto Found6;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 5)))
                    goto Found5;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 4)))
                    goto Found4;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (value.Equals(Unsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (value.Equals(Unsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (value.Equals(Unsafe.Add(ref searchSpace, length)))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;

        }

        #endregion

        #region IndexOfAny

        public static int IndexOfAnyObjectEquals<T>(ref T searchSpace, T value0, T value1, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, IEquatable<T> value0, IEquatable<T> value1, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAnyObjectEquals<T>(ref T searchSpace, T value0, T value1, T value2, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAny<T>(ref T searchSpace, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            int index = 0;
            while ((length - index) >= 8)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, index + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, index + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, index + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, index + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found7;

                index += 8;
            }

            if ((length - index) >= 4)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
                lookUp = Unsafe.Add(ref searchSpace, index + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, index + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, index + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;

                index += 4;
            }

            while (index < length)
            {
                lookUp = Unsafe.Add(ref searchSpace, index);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;

                index++;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return index;
        Found1:
            return index + 1;
        Found2:
            return index + 2;
        Found3:
            return index + 3;
        Found4:
            return index + 4;
        Found5:
            return index + 5;
        Found6:
            return index + 6;
        Found7:
            return index + 7;
        }

        public static int IndexOfAnyObjectEquals<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOfObjectEquals(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
                if ((uint)tempIndex < (uint)index)
                {
                    index = tempIndex;
                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    searchSpaceLength = tempIndex;

                    if (index == 0)
                        break;
                }
            }
            return index;
        }

        public static int IndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength, IEqualityComparer<T> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOf(ref searchSpace, Unsafe.Add(ref value, i), equalityComparer, searchSpaceLength);
                if ((uint)tempIndex < (uint)index)
                {
                    index = tempIndex;
                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    searchSpaceLength = tempIndex;

                    if (index == 0)
                        break;
                }
            }
            return index;
        }

        public static int IndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref IEquatable<T> value, int valueLength)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
                if ((uint)tempIndex < (uint)index)
                {
                    index = tempIndex;
                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    searchSpaceLength = tempIndex;

                    if (index == 0)
                        break;
                }
            }
            return index;
        }

        #endregion

        #region LastIndexOfAny

        public static int LastIndexOfAnyObjectEquals<T>(ref T searchSpace, T value0, T value1, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, IEquatable<T> value0, IEquatable<T> value1, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAnyObjectEquals<T>(ref T searchSpace, T value0, T value1, T value2, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (equalityComparer.Equals(value0, lookUp) || equalityComparer.Equals(value1, lookUp) || equalityComparer.Equals(value2, lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2, int length)
        {
            Debug.Assert(length >= 0);

            T lookUp;
            while (length >= 8)
            {
                length -= 8;

                lookUp = Unsafe.Add(ref searchSpace, length + 7);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found7;
                lookUp = Unsafe.Add(ref searchSpace, length + 6);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found6;
                lookUp = Unsafe.Add(ref searchSpace, length + 5);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found5;
                lookUp = Unsafe.Add(ref searchSpace, length + 4);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found4;
                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                lookUp = Unsafe.Add(ref searchSpace, length + 3);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found3;
                lookUp = Unsafe.Add(ref searchSpace, length + 2);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found2;
                lookUp = Unsafe.Add(ref searchSpace, length + 1);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found1;
                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                lookUp = Unsafe.Add(ref searchSpace, length);
                if (value0.Equals(lookUp) || value1.Equals(lookUp) || value2.Equals(lookUp))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;
        }

        public static int LastIndexOfAnyObjectEquals<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOfObjectEquals(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
                if (tempIndex > index)
                    index = tempIndex;
            }
            return index;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength, IEqualityComparer<T> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOf(ref searchSpace, Unsafe.Add(ref value, i), equalityComparer, searchSpaceLength);
                if (tempIndex > index)
                    index = tempIndex;
            }
            return index;
        }

        public static int LastIndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref IEquatable<T> value, int valueLength)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
                if (tempIndex > index)
                    index = tempIndex;
            }
            return index;
        }

        #endregion
   
        #region SequenceEqual

        public static bool SequenceEqualObjectEquals<T>(ref T first, ref T second, int length)
        {
            Debug.Assert(length >= 0);

            if (Unsafe.AreSame(ref first, ref second))
                goto Equal;

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 1).Equals(Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 2).Equals(Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 3).Equals(Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 4).Equals(Unsafe.Add(ref second, index + 4)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 5).Equals(Unsafe.Add(ref second, index + 5)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 6).Equals(Unsafe.Add(ref second, index + 6)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 7).Equals(Unsafe.Add(ref second, index + 7)))
                    goto NotEqual;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 1).Equals(Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 2).Equals(Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 3).Equals(Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;

                index += 4;
            }

            while (length > 0)
            {
                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                index += 1;
                length--;
            }

        Equal:
            return true;

        NotEqual: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return false;
        }

        public static bool SequenceEqual<T>(ref T first, ref T second, int length, IEqualityComparer<T> equalityComparer)
        {
            Debug.Assert(length >= 0);

            if (Unsafe.AreSame(ref first, ref second))
                goto Equal;

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (!equalityComparer.Equals(Unsafe.Add(ref first, index), Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 1), Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 2), Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 3), Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 4), Unsafe.Add(ref second, index + 4)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 5), Unsafe.Add(ref second, index + 5)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 6), Unsafe.Add(ref second, index + 6)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 7), Unsafe.Add(ref second, index + 7)))
                    goto NotEqual;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (!equalityComparer.Equals(Unsafe.Add(ref first, index), Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 1), Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 2), Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index + 3), Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;

                index += 4;
            }

            while (length > 0)
            {
                if (!equalityComparer.Equals(Unsafe.Add(ref first, index), Unsafe.Add(ref second, index)))
                    goto NotEqual;
                index += 1;
                length--;
            }

        Equal:
            return true;

        NotEqual: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return false;
        }

        public static bool SequenceEqual<T>(ref T first, ref IEquatable<T> second, int length)
        {
            Debug.Assert(length >= 0);

            //if (typeof(IEquatable<T>) == typeof(T) && Unsafe.AreSame(ref first, ref Unsafe.As<IEquatable<T>, T>(ref second)))
            //    goto Equal;

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 1).Equals(Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 2).Equals(Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 3).Equals(Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 4).Equals(Unsafe.Add(ref second, index + 4)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 5).Equals(Unsafe.Add(ref second, index + 5)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 6).Equals(Unsafe.Add(ref second, index + 6)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 7).Equals(Unsafe.Add(ref second, index + 7)))
                    goto NotEqual;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 1).Equals(Unsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 2).Equals(Unsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!Unsafe.Add(ref first, index + 3).Equals(Unsafe.Add(ref second, index + 3)))
                    goto NotEqual;

                index += 4;
            }

            while (length > 0)
            {
                if (!Unsafe.Add(ref first, index).Equals(Unsafe.Add(ref second, index)))
                    goto NotEqual;
                index += 1;
                length--;
            }

        //Equal:
            return true;

        NotEqual: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return false;
        }

        #endregion
    }
}
