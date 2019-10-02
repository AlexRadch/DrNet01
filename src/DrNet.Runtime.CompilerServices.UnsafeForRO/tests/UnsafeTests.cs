using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace DrNet.Runtime.CompilerServices
{
    public class UnsafeTests
    {
        [Fact]
        public static unsafe void ReadInt32()
        {
            int expected = 10;
            void* address = UnsafeForRO.AsPointer(expected);
            int ret = Unsafe.Read<int>(address);
            Assert.Equal(expected, ret);
        }

        [Fact]
        public static unsafe void WriteInt32()
        {
            int value = 10;
            int* address = (int*)UnsafeForRO.AsPointer(value);
            int expected = 20;
            Unsafe.Write(address, expected);

            Assert.Equal(expected, value);
            Assert.Equal(expected, *address);
            Assert.Equal(expected, Unsafe.Read<int>(address));
        }

        [Fact]
        public static unsafe void WriteBytesIntoInt32()
        {
            int value = 20;
            int* intAddress = (int*)UnsafeForRO.AsPointer(value);
            byte* byteAddress = (byte*)intAddress;
            for (int i = 0; i < 4; i++)
            {
                Unsafe.Write(byteAddress + i, (byte)i);
            }

            Assert.Equal(0, Unsafe.Read<byte>(byteAddress));
            Assert.Equal(1, Unsafe.Read<byte>(byteAddress + 1));
            Assert.Equal(2, Unsafe.Read<byte>(byteAddress + 2));
            Assert.Equal(3, Unsafe.Read<byte>(byteAddress + 3));

            Byte4 b4 = Unsafe.Read<Byte4>(byteAddress);
            Assert.Equal(0, b4.B0);
            Assert.Equal(1, b4.B1);
            Assert.Equal(2, b4.B2);
            Assert.Equal(3, b4.B3);

            int expected = (b4.B3 << 24) + (b4.B2 << 16) + (b4.B1 << 8) + (b4.B0);
            Assert.Equal(expected, value);
        }

        [Fact]
        public static unsafe void LongIntoCompoundStruct()
        {
            long value = 1234567891011121314L;
            long* longAddress = (long*)UnsafeForRO.AsPointer(value);
            Byte4Short2 b4s2 = Unsafe.Read<Byte4Short2>(longAddress);
            Assert.Equal(162, b4s2.B0);
            Assert.Equal(48, b4s2.B1);
            Assert.Equal(210, b4s2.B2);
            Assert.Equal(178, b4s2.B3);
            Assert.Equal(4340, b4s2.S4);
            Assert.Equal(4386, b4s2.S6);

            b4s2.B0 = 1;
            b4s2.B1 = 1;
            b4s2.B2 = 1;
            b4s2.B3 = 1;
            b4s2.S4 = 1;
            b4s2.S6 = 1;
            Unsafe.Write(longAddress, b4s2);

            long expected = 281479288520961;
            Assert.Equal(expected, value);
            Assert.Equal(expected, Unsafe.Read<long>(longAddress));
        }

        [Fact]
        public static unsafe void ReadWriteDoublePointer()
        {
            int value1 = 10;
            int value2 = 20;
            int* valueAddress = (int*)UnsafeForRO.AsPointer(value1);
            int** valueAddressPtr = &valueAddress;
            Unsafe.Write(valueAddressPtr, new IntPtr(&value2));

            Assert.Equal(20, *(*valueAddressPtr));
            Assert.Equal(20, Unsafe.Read<int>(valueAddress));
            Assert.Equal(new IntPtr(valueAddress), Unsafe.Read<IntPtr>(valueAddressPtr));
            Assert.Equal(20, Unsafe.Read<int>(Unsafe.Read<IntPtr>(valueAddressPtr).ToPointer()));
        }

        [Fact]
        public static unsafe void CopyToRef()
        {
            int value = 10;
            int destination = -1;
            Unsafe.Copy(ref destination, UnsafeForRO.AsPointer(value));
            Assert.Equal(10, destination);
            Assert.Equal(10, value);

            int destination2 = -1;
            Unsafe.Copy(ref destination2, &value);
            Assert.Equal(10, destination2);
            Assert.Equal(10, value);
        }

        [Fact]
        public static unsafe void CopyToVoidPtr()
        {
            int value = 10;
            int destination = -1;
            UnsafeForRO.Copy(UnsafeForRO.AsPointer(destination), value);
            Assert.Equal(10, destination);
            Assert.Equal(10, value);

            int destination2 = -1;
            UnsafeForRO.Copy(&destination2, value);
            Assert.Equal(10, destination2);
            Assert.Equal(10, value);
        }

        //[Fact]
        //public static unsafe void SizeOf()
        //{
        //    Assert.Equal(1, UnsafeForRO.SizeOf<sbyte>());
        //    Assert.Equal(1, UnsafeForRO.SizeOf<byte>());
        //    Assert.Equal(2, UnsafeForRO.SizeOf<short>());
        //    Assert.Equal(2, UnsafeForRO.SizeOf<ushort>());
        //    Assert.Equal(4, UnsafeForRO.SizeOf<int>());
        //    Assert.Equal(4, UnsafeForRO.SizeOf<uint>());
        //    Assert.Equal(8, UnsafeForRO.SizeOf<long>());
        //    Assert.Equal(8, UnsafeForRO.SizeOf<ulong>());
        //    Assert.Equal(4, UnsafeForRO.SizeOf<float>());
        //    Assert.Equal(8, UnsafeForRO.SizeOf<double>());
        //    Assert.Equal(4, UnsafeForRO.SizeOf<Byte4>());
        //    Assert.Equal(8, UnsafeForRO.SizeOf<Byte4Short2>());
        //    Assert.Equal(512, UnsafeForRO.SizeOf<Byte512>());
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockStack(int numBytes, byte value)
        //{
        //    byte* stackPtr = stackalloc byte[numBytes];
        //    UnsafeForRO.InitBlock(stackPtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(stackPtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockUnmanaged(int numBytes, byte value)
        //{
        //    IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes);
        //    byte* bytePtr = (byte*)allocatedMemory.ToPointer();
        //    UnsafeForRO.InitBlock(bytePtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(bytePtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockRefStack(int numBytes, byte value)
        //{
        //    byte* stackPtr = stackalloc byte[numBytes];
        //    UnsafeForRO.InitBlock(ref *stackPtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(stackPtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockRefUnmanaged(int numBytes, byte value)
        //{
        //    IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes);
        //    byte* bytePtr = (byte*)allocatedMemory.ToPointer();
        //    UnsafeForRO.InitBlock(ref *bytePtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(bytePtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockUnalignedStack(int numBytes, byte value)
        //{
        //    byte* stackPtr = stackalloc byte[numBytes + 1];
        //    stackPtr += 1; // +1 = make unaligned
        //    UnsafeForRO.InitBlockUnaligned(stackPtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(stackPtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockUnalignedUnmanaged(int numBytes, byte value)
        //{
        //    IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes + 1);
        //    byte* bytePtr = (byte*)allocatedMemory.ToPointer() + 1; // +1 = make unaligned
        //    UnsafeForRO.InitBlockUnaligned(bytePtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(bytePtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockUnalignedRefStack(int numBytes, byte value)
        //{
        //    byte* stackPtr = stackalloc byte[numBytes + 1];
        //    stackPtr += 1; // +1 = make unaligned
        //    UnsafeForRO.InitBlockUnaligned(ref *stackPtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(stackPtr[i], value);
        //    }
        //}

        //[Theory]
        //[MemberData(nameof(InitBlockData))]
        //public static unsafe void InitBlockUnalignedRefUnmanaged(int numBytes, byte value)
        //{
        //    IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes + 1);
        //    byte* bytePtr = (byte*)allocatedMemory.ToPointer() + 1; // +1 = make unaligned
        //    UnsafeForRO.InitBlockUnaligned(ref *bytePtr, value, (uint)numBytes);
        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        Assert.Equal(bytePtr[i], value);
        //    }
        //}

        //public static IEnumerable<object[]> InitBlockData()
        //{
        //    yield return new object[] { 0, 1 };
        //    yield return new object[] { 1, 1 };
        //    yield return new object[] { 10, 0 };
        //    yield return new object[] { 10, 2 };
        //    yield return new object[] { 10, 255 };
        //    yield return new object[] { 10000, 255 };
        //}

        //[Theory]
        //[MemberData(nameof(CopyBlockData))]
        //public static unsafe void CopyBlock(int numBytes)
        //{
        //    byte* source = stackalloc byte[numBytes];
        //    byte* destination = stackalloc byte[numBytes];

        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        byte value = (byte)(i % 255);
        //        source[i] = value;
        //    }

        //    UnsafeForRO.CopyBlock(destination, source, (uint)numBytes);

        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        byte value = (byte)(i % 255);
        //        Assert.Equal(value, destination[i]);
        //        Assert.Equal(source[i], destination[i]);
        //    }
        //}

        [Theory]
        [MemberData(nameof(CopyBlockData))]
        public static unsafe void CopyBlockRef(int numBytes)
        {
            byte* source = stackalloc byte[numBytes];
            byte* destination = stackalloc byte[numBytes];

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            UnsafeForRO.CopyBlock(ref destination[0], source[0], (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.Equal(value, destination[i]);
                Assert.Equal(source[i], destination[i]);
            }
        }

        //[Theory]
        //[MemberData(nameof(CopyBlockData))]
        //public static unsafe void CopyBlockUnaligned(int numBytes)
        //{
        //    byte* source = stackalloc byte[numBytes + 1];
        //    byte* destination = stackalloc byte[numBytes + 1];
        //    source += 1;      // +1 = make unaligned
        //    destination += 1; // +1 = make unaligned

        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        byte value = (byte)(i % 255);
        //        source[i] = value;
        //    }

        //    UnsafeForRO.CopyBlockUnaligned(destination, source, (uint)numBytes);

        //    for (int i = 0; i < numBytes; i++)
        //    {
        //        byte value = (byte)(i % 255);
        //        Assert.Equal(value, destination[i]);
        //        Assert.Equal(source[i], destination[i]);
        //    }
        //}

        [Theory]
        [MemberData(nameof(CopyBlockData))]
        public static unsafe void CopyBlockUnalignedRef(int numBytes)
        {
            byte* source = stackalloc byte[numBytes + 1];
            byte* destination = stackalloc byte[numBytes + 1];
            source += 1;      // +1 = make unaligned
            destination += 1; // +1 = make unaligned

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            UnsafeForRO.CopyBlockUnaligned(ref destination[0], source[0], (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.Equal(value, destination[i]);
                Assert.Equal(source[i], destination[i]);
            }
        }

        public static IEnumerable<object[]> CopyBlockData()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 10 };
            yield return new object[] { 100 };
            yield return new object[] { 100000 };
        }

        //[Fact]
        //public static void As()
        //{
        //    object o = "Hello";
        //    Assert.Equal("Hello", UnsafeForRO.As<string>(o));
        //}

        //[Fact]
        //public static void DangerousAs()
        //{
        //    // Verify that As does not perform type checks
        //    object o = new object();
        //    Assert.IsType<object>(UnsafeForRO.As<string>(o));
        //}

        [Fact]
        public static void ByteOffsetArray()
        {
            var a = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            Assert.Equal(new IntPtr(0), UnsafeForRO.ByteOffset(a[0], a[0]));
            Assert.Equal(new IntPtr(1), UnsafeForRO.ByteOffset(a[0], a[1]));
            Assert.Equal(new IntPtr(-1), UnsafeForRO.ByteOffset(a[1], a[0]));
            Assert.Equal(new IntPtr(2), UnsafeForRO.ByteOffset(a[0], a[2]));
            Assert.Equal(new IntPtr(-2), UnsafeForRO.ByteOffset(a[2], a[0]));
            Assert.Equal(new IntPtr(3), UnsafeForRO.ByteOffset(a[0], a[3]));
            Assert.Equal(new IntPtr(4), UnsafeForRO.ByteOffset(a[0], a[4]));
            Assert.Equal(new IntPtr(5), UnsafeForRO.ByteOffset(a[0], a[5]));
            Assert.Equal(new IntPtr(6), UnsafeForRO.ByteOffset(a[0], a[6]));
            Assert.Equal(new IntPtr(7), UnsafeForRO.ByteOffset(a[0], a[7]));
        }

        [Fact]
        public static void ByteOffsetStackByte4()
        {
            var byte4 = new Byte4();

            Assert.Equal(new IntPtr(0), UnsafeForRO.ByteOffset(byte4.B0, byte4.B0));
            Assert.Equal(new IntPtr(1), UnsafeForRO.ByteOffset(byte4.B0, byte4.B1));
            Assert.Equal(new IntPtr(-1), UnsafeForRO.ByteOffset(byte4.B1, byte4.B0));
            Assert.Equal(new IntPtr(2), UnsafeForRO.ByteOffset(byte4.B0, byte4.B2));
            Assert.Equal(new IntPtr(-2), UnsafeForRO.ByteOffset(byte4.B2, byte4.B0));
            Assert.Equal(new IntPtr(3), UnsafeForRO.ByteOffset(byte4.B0, byte4.B3));
            Assert.Equal(new IntPtr(-3), UnsafeForRO.ByteOffset(byte4.B3, byte4.B0));
        }

        [Fact]
        public static unsafe void AsRef()
        {
            byte[] b = new byte[4] { 0x42, 0x42, 0x42, 0x42 };
            fixed (byte* p = b)
            {
                ref readonly int r = ref UnsafeForRO.AsRef<int>(p);
                Assert.Equal(0x42424242, r);

                Unsafe.AsRef(r) = 0x0EF00EF0;
                Assert.Equal(0xFE, b[0] | b[1] | b[2] | b[3]);
            }
        }

        //[Fact]
        //public static void InAsRef()
        //{
        //    int[] a = new int[] { 0x123, 0x234, 0x345, 0x456 };

        //    ref int r = ref UnsafeForRO.AsRef<int>(a[0]);
        //    Assert.Equal(0x123, r);

        //    r = 0x42;
        //    Assert.Equal(0x42, a[0]);
        //}

        [Fact]
        public static void RefAs()
        {
            byte[] b = new byte[4] { 0x42, 0x42, 0x42, 0x42 };

            ref readonly int r = ref UnsafeForRO.As<byte, int>(b[0]);
            Assert.Equal(0x42424242, r);

            Unsafe.AsRef(r) = 0x0EF00EF0;
            Assert.Equal(0xFE, b[0] | b[1] | b[2] | b[3]);
        }

        [Fact]
        public static void RefAdd()
        {
            int[] a = new int[] { 0x123, 0x234, 0x345, 0x456 };

            ref readonly int r1 = ref UnsafeForRO.Add(a[0], 1);
            Assert.Equal(0x234, r1);

            ref readonly int r2 = ref UnsafeForRO.Add(r1, 2);
            Assert.Equal(0x456, r2);

            ref readonly int r3 = ref UnsafeForRO.Add(r2, -3);
            Assert.Equal(0x123, r3);
        }

        //[Fact]
        //public static unsafe void VoidPointerAdd()
        //{
        //    int[] a = new int[] { 0x123, 0x234, 0x345, 0x456 };

        //    fixed (void* ptr = a)
        //    {
        //        void* r1 = UnsafeForRO.Add<int>(ptr, 1);
        //        Assert.Equal(0x234, *(int*)r1);

        //        void* r2 = UnsafeForRO.Add<int>(r1, 2);
        //        Assert.Equal(0x456, *(int*)r2);

        //        void* r3 = UnsafeForRO.Add<int>(r2, -3);
        //        Assert.Equal(0x123, *(int*)r3);
        //    }

        //    fixed (void* ptr = &a[1])
        //    {
        //        void* r0 = UnsafeForRO.Add<int>(ptr, -1);
        //        Assert.Equal(0x123, *(int*)r0);

        //        void* r3 = UnsafeForRO.Add<int>(ptr, 2);
        //        Assert.Equal(0x456, *(int*)r3);
        //    }
        //}

        [Fact]
        public static void RefAddIntPtr()
        {
            int[] a = new int[] { 0x123, 0x234, 0x345, 0x456 };

            ref readonly int r1 = ref UnsafeForRO.Add(a[0], (IntPtr)1);
            Assert.Equal(0x234, r1);

            ref readonly int r2 = ref UnsafeForRO.Add(r1, (IntPtr)2);
            Assert.Equal(0x456, r2);

            ref readonly int r3 = ref UnsafeForRO.Add(r2, (IntPtr)(-3));
            Assert.Equal(0x123, r3);
        }

        [Fact]
        public static void RefAddByteOffset()
        {
            byte[] a = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            ref readonly byte r1 = ref UnsafeForRO.AddByteOffset(a[0], (IntPtr)1);
            Assert.Equal(0x34, r1);

            ref readonly byte r2 = ref UnsafeForRO.AddByteOffset(r1, (IntPtr)2);
            Assert.Equal(0x78, r2);

            ref readonly byte r3 = ref UnsafeForRO.AddByteOffset(r2, (IntPtr)(-3));
            Assert.Equal(0x12, r3);
        }

        [Fact]
        public static void RefSubtract()
        {
            string[] a = new string[] { "abc", "def", "ghi", "jkl" };

            ref readonly string r1 = ref UnsafeForRO.Subtract(a[0], -2);
            Assert.Equal("ghi", r1);

            ref readonly string r2 = ref UnsafeForRO.Subtract(r1, -1);
            Assert.Equal("jkl", r2);

            ref readonly string r3 = ref UnsafeForRO.Subtract(r2, 3);
            Assert.Equal("abc", r3);
        }

        //[Fact]
        //public static unsafe void VoidPointerSubtract()
        //{
        //    int[] a = new int[] { 0x123, 0x234, 0x345, 0x456 };

        //    fixed (void* ptr = a)
        //    {
        //        void* r1 = UnsafeForRO.Subtract<int>(ptr, -2);
        //        Assert.Equal(0x345, *(int*)r1);

        //        void* r2 = UnsafeForRO.Subtract<int>(r1, -1);
        //        Assert.Equal(0x456, *(int*)r2);

        //        void* r3 = UnsafeForRO.Subtract<int>(r2, 3);
        //        Assert.Equal(0x123, *(int*)r3);
        //    }

        //    fixed (void* ptr = &a[1])
        //    {
        //        void* r0 = UnsafeForRO.Subtract<int>(ptr, 1);
        //        Assert.Equal(0x123, *(int*)r0);

        //        void* r3 = UnsafeForRO.Subtract<int>(ptr, -2);
        //        Assert.Equal(0x456, *(int*)r3);
        //    }
        //}

        [Fact]
        public static void RefSubtractIntPtr()
        {
            string[] a = new string[] { "abc", "def", "ghi", "jkl" };

            ref readonly string r1 = ref UnsafeForRO.Subtract(a[0], (IntPtr)(-2));
            Assert.Equal("ghi", r1);

            ref readonly string r2 = ref UnsafeForRO.Subtract(r1, (IntPtr)(-1));
            Assert.Equal("jkl", r2);

            ref readonly string r3 = ref UnsafeForRO.Subtract(r2, (IntPtr)3);
            Assert.Equal("abc", r3);
        }

        [Fact]
        public static void RefSubtractByteOffset()
        {
            byte[] a = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            ref readonly byte r1 = ref UnsafeForRO.SubtractByteOffset(a[0], (IntPtr)(-1));
            Assert.Equal(0x34, r1);

            ref readonly byte r2 = ref UnsafeForRO.SubtractByteOffset(r1, (IntPtr)(-2));
            Assert.Equal(0x78, r2);

            ref readonly byte r3 = ref UnsafeForRO.SubtractByteOffset(r2, (IntPtr)3);
            Assert.Equal(0x12, r3);
        }

        [Fact]
        public static void RefAreSame()
        {
            long[] a = new long[2];

            Assert.True(UnsafeForRO.AreSame(a[0], a[0]));
            Assert.False(UnsafeForRO.AreSame(a[0], a[1]));
        }

        [Fact]
        public static unsafe void RefIsAddressGreaterThan()
        {
            int[] a = new int[2];

            Assert.False(UnsafeForRO.IsAddressGreaterThan(a[0], a[0]));
            Assert.False(UnsafeForRO.IsAddressGreaterThan(a[0], a[1]));
            Assert.True(UnsafeForRO.IsAddressGreaterThan(a[1], a[0]));
            Assert.False(UnsafeForRO.IsAddressGreaterThan(a[1], a[1]));

            // The following tests ensure that we're using unsigned comparison logic

            Assert.False(UnsafeForRO.IsAddressGreaterThan(UnsafeForRO.AsRef<byte>((void*)(1)), UnsafeForRO.AsRef<byte>((void*)(-1))));
            Assert.True(UnsafeForRO.IsAddressGreaterThan(UnsafeForRO.AsRef<byte>((void*)(-1)), UnsafeForRO.AsRef<byte>((void*)(1))));
            Assert.True(UnsafeForRO.IsAddressGreaterThan(UnsafeForRO.AsRef<byte>((void*)(int.MinValue)), UnsafeForRO.AsRef<byte>((void*)(int.MaxValue))));
            Assert.False(UnsafeForRO.IsAddressGreaterThan(UnsafeForRO.AsRef<byte>((void*)(int.MaxValue)), UnsafeForRO.AsRef<byte>((void*)(int.MinValue))));
            Assert.False(UnsafeForRO.IsAddressGreaterThan(UnsafeForRO.AsRef<byte>(null), UnsafeForRO.AsRef<byte>(null)));
        }

        [Fact]
        public static unsafe void RefIsAddressLessThan()
        {
            int[] a = new int[2];

            Assert.False(UnsafeForRO.IsAddressLessThan(a[0], a[0]));
            Assert.True(UnsafeForRO.IsAddressLessThan(a[0], a[1]));
            Assert.False(UnsafeForRO.IsAddressLessThan(a[1], a[0]));
            Assert.False(UnsafeForRO.IsAddressLessThan(a[1], a[1]));

            // The following tests ensure that we're using unsigned comparison logic

            Assert.True(UnsafeForRO.IsAddressLessThan(UnsafeForRO.AsRef<byte>((void*)(1)), UnsafeForRO.AsRef<byte>((void*)(-1))));
            Assert.False(UnsafeForRO.IsAddressLessThan(UnsafeForRO.AsRef<byte>((void*)(-1)), UnsafeForRO.AsRef<byte>((void*)(1))));
            Assert.False(UnsafeForRO.IsAddressLessThan(UnsafeForRO.AsRef<byte>((void*)(int.MinValue)), UnsafeForRO.AsRef<byte>((void*)(int.MaxValue))));
            Assert.True(UnsafeForRO.IsAddressLessThan(UnsafeForRO.AsRef<byte>((void*)(int.MaxValue)), UnsafeForRO.AsRef<byte>((void*)(int.MinValue))));
            Assert.False(UnsafeForRO.IsAddressLessThan(UnsafeForRO.AsRef<byte>(null), UnsafeForRO.AsRef<byte>(null)));
        }

        [Fact]
        public static unsafe void ReadUnaligned_ByRef_Int32()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            int actual = UnsafeForRO.ReadUnaligned<int>(unaligned[1]);

            Assert.Equal(123456789, actual);
        }

        [Fact]
        public static unsafe void ReadUnaligned_ByRef_Double()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            double actual = UnsafeForRO.ReadUnaligned<double>(unaligned[9]);

            Assert.Equal(3.42, actual);
        }

        [Fact]
        public static unsafe void ReadUnaligned_ByRef_Struct()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            Int32Double actual = UnsafeForRO.ReadUnaligned<Int32Double>(unaligned[1]);

            Assert.Equal(123456789, actual.Int32);
            Assert.Equal(3.42, actual.Double);
        }

        //[Fact]
        //public static unsafe void ReadUnaligned_Ptr_Int32()
        //{
        //    byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

        //    fixed (byte* p = unaligned)
        //    {
        //        int actual = UnsafeForRO.ReadUnaligned<int>(p + 1);

        //        Assert.Equal(123456789, actual);
        //    }
        //}

        //[Fact]
        //public static unsafe void ReadUnaligned_Ptr_Double()
        //{
        //    byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

        //    fixed (byte* p = unaligned)
        //    {
        //        double actual = UnsafeForRO.ReadUnaligned<double>(p + 9);

        //        Assert.Equal(3.42, actual);
        //    }
        //}

        //[Fact]
        //public static unsafe void ReadUnaligned_Ptr_Struct()
        //{
        //    byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

        //    fixed (byte* p = unaligned)
        //    {
        //        Int32Double actual = UnsafeForRO.ReadUnaligned<Int32Double>(p + 1);

        //        Assert.Equal(123456789, actual.Int32);
        //        Assert.Equal(3.42, actual.Double);
        //    }
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_ByRef_Int32()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    UnsafeForRO.WriteUnaligned(ref unaligned[1], 123456789);

        //    int actual = Int32Double.Aligned(unaligned).Int32;
        //    Assert.Equal(123456789, actual);
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_ByRef_Double()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    UnsafeForRO.WriteUnaligned(ref unaligned[9], 3.42);

        //    double actual = Int32Double.Aligned(unaligned).Double;
        //    Assert.Equal(3.42, actual);
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_ByRef_Struct()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    UnsafeForRO.WriteUnaligned(ref unaligned[1], new Int32Double { Int32 = 123456789, Double = 3.42 });

        //    Int32Double actual = Int32Double.Aligned(unaligned);
        //    Assert.Equal(123456789, actual.Int32);
        //    Assert.Equal(3.42, actual.Double);
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_Ptr_Int32()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    fixed (byte* p = unaligned)
        //    {
        //        UnsafeForRO.WriteUnaligned(p + 1, 123456789);
        //    }

        //    int actual = Int32Double.Aligned(unaligned).Int32;
        //    Assert.Equal(123456789, actual);
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_Ptr_Double()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    fixed (byte* p = unaligned)
        //    {
        //        UnsafeForRO.WriteUnaligned(p + 9, 3.42);
        //    }

        //    double actual = Int32Double.Aligned(unaligned).Double;
        //    Assert.Equal(3.42, actual);
        //}

        //[Fact]
        //public static unsafe void WriteUnaligned_Ptr_Struct()
        //{
        //    byte[] unaligned = new byte[sizeof(Int32Double) + 1];

        //    fixed (byte* p = unaligned)
        //    {
        //        UnsafeForRO.WriteUnaligned(p + 1, new Int32Double { Int32 = 123456789, Double = 3.42 });
        //    }

        //    Int32Double actual = Int32Double.Aligned(unaligned);
        //    Assert.Equal(123456789, actual.Int32);
        //    Assert.Equal(3.42, actual.Double);
        //}

        [Fact]
        public static void Unbox_Int32()
        {
            object box = 42;

            Assert.True(UnsafeForRO.AreSame(UnsafeForRO.Unbox<int>(box), UnsafeForRO.Unbox<int>(box)));

            Assert.Equal(42, (int)box);
            Assert.Equal(42, UnsafeForRO.Unbox<int>(box));

            ref readonly int value = ref UnsafeForRO.Unbox<int>(box);
            Unsafe.AsRef(value) = 84;
            Assert.Equal(84, (int)box);
            Assert.Equal(84, UnsafeForRO.Unbox<int>(box));

            Assert.Throws<InvalidCastException>(() => UnsafeForRO.Unbox<Byte4>(box));
        }

        [Fact]
        public static void Unbox_CustomValueType()
        {
            object box = new Int32Double();

            Assert.Equal(0, ((Int32Double)box).Double);
            Assert.Equal(0, ((Int32Double)box).Int32);

            ref readonly Int32Double value = ref UnsafeForRO.Unbox<Int32Double>(box);
            Unsafe.AsRef(value).Double = 42;
            Unsafe.AsRef(value).Int32 = 84;

            Assert.Equal(42, ((Int32Double)box).Double);
            Assert.Equal(84, ((Int32Double)box).Int32);

            Assert.Throws<InvalidCastException>(() => UnsafeForRO.Unbox<bool>(box));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Byte4
    {
        [FieldOffset(0)]
        public byte B0;
        [FieldOffset(1)]
        public byte B1;
        [FieldOffset(2)]
        public byte B2;
        [FieldOffset(3)]
        public byte B3;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Byte4Short2
    {
        [FieldOffset(0)]
        public byte B0;
        [FieldOffset(1)]
        public byte B1;
        [FieldOffset(2)]
        public byte B2;
        [FieldOffset(3)]
        public byte B3;
        [FieldOffset(4)]
        public short S4;
        [FieldOffset(6)]
        public short S6;
    }

    public unsafe struct Byte512
    {
        public fixed byte Bytes[512];
    }

    [StructLayout(LayoutKind.Explicit, Size=16)]
    public unsafe struct Int32Double
    {
        [FieldOffset(0)]
        public int Int32;
        [FieldOffset(8)]
        public double Double;

        public static unsafe byte[] Unaligned(int i, double d)
        {
            var aligned = new Int32Double { Int32 = i, Double = d };
            var unaligned = new byte[sizeof(Int32Double) + 1];

            fixed (byte* p = unaligned)
            {
                Buffer.MemoryCopy(&aligned, p + 1, sizeof(Int32Double), sizeof(Int32Double));
            }

            return unaligned;
        }

        public static unsafe Int32Double Aligned(byte[] unaligned)
        {
            var aligned = new Int32Double();

            fixed (byte* p = unaligned)
            {
                Buffer.MemoryCopy(p + 1, &aligned, sizeof(Int32Double), sizeof(Int32Double));
            }

            return aligned;
        }
    }
}
