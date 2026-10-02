using System;
using System.Runtime.CompilerServices;

namespace System.Numerics
{
	// Token: 0x0200000A RID: 10
	public static class Vector
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00014730 File Offset: 0x00012930
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static void Widen(Vector<byte> source, out Vector<ushort> low, out Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			ushort* ptr;
			checked
			{
				ptr = stackalloc ushort[unchecked((UIntPtr)(count / 2)) * 2];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)source[i];
			}
			ushort* ptr2;
			checked
			{
				ptr2 = stackalloc ushort[unchecked((UIntPtr)(count / 2)) * 2];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ushort)source[j + count / 2];
			}
			low = new Vector<ushort>((void*)ptr);
			high = new Vector<ushort>((void*)ptr2);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000147B4 File Offset: 0x000129B4
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static void Widen(Vector<ushort> source, out Vector<uint> low, out Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			uint* ptr;
			checked
			{
				ptr = stackalloc uint[unchecked((UIntPtr)(count / 2)) * 4];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)source[i];
			}
			uint* ptr2;
			checked
			{
				ptr2 = stackalloc uint[unchecked((UIntPtr)(count / 2)) * 4];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (uint)source[j + count / 2];
			}
			low = new Vector<uint>((void*)ptr);
			high = new Vector<uint>((void*)ptr2);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00014838 File Offset: 0x00012A38
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static void Widen(Vector<uint> source, out Vector<ulong> low, out Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			ulong* ptr;
			checked
			{
				ptr = stackalloc ulong[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ulong)source[i];
			}
			ulong* ptr2;
			checked
			{
				ptr2 = stackalloc ulong[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ulong)source[j + count / 2];
			}
			low = new Vector<ulong>((void*)ptr);
			high = new Vector<ulong>((void*)ptr2);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000148BC File Offset: 0x00012ABC
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static void Widen(Vector<sbyte> source, out Vector<short> low, out Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			short* ptr;
			checked
			{
				ptr = stackalloc short[unchecked((UIntPtr)(count / 2)) * 2];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)source[i];
			}
			short* ptr2;
			checked
			{
				ptr2 = stackalloc short[unchecked((UIntPtr)(count / 2)) * 2];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (short)source[j + count / 2];
			}
			low = new Vector<short>((void*)ptr);
			high = new Vector<short>((void*)ptr2);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00014940 File Offset: 0x00012B40
		[JitIntrinsic]
		public unsafe static void Widen(Vector<short> source, out Vector<int> low, out Vector<int> high)
		{
			int count = Vector<short>.Count;
			int* ptr;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)(count / 2)) * 4];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)source[i];
			}
			int* ptr2;
			checked
			{
				ptr2 = stackalloc int[unchecked((UIntPtr)(count / 2)) * 4];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (int)source[j + count / 2];
			}
			low = new Vector<int>((void*)ptr);
			high = new Vector<int>((void*)ptr2);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000149C4 File Offset: 0x00012BC4
		[JitIntrinsic]
		public unsafe static void Widen(Vector<int> source, out Vector<long> low, out Vector<long> high)
		{
			int count = Vector<int>.Count;
			long* ptr;
			checked
			{
				ptr = stackalloc long[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (long)source[i];
			}
			long* ptr2;
			checked
			{
				ptr2 = stackalloc long[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (long)source[j + count / 2];
			}
			low = new Vector<long>((void*)ptr);
			high = new Vector<long>((void*)ptr2);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00014A48 File Offset: 0x00012C48
		[JitIntrinsic]
		public unsafe static void Widen(Vector<float> source, out Vector<double> low, out Vector<double> high)
		{
			int count = Vector<float>.Count;
			double* ptr;
			checked
			{
				ptr = stackalloc double[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (double)source[i];
			}
			double* ptr2;
			checked
			{
				ptr2 = stackalloc double[unchecked((UIntPtr)(count / 2)) * 8];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (double)source[j + count / 2];
			}
			low = new Vector<double>((void*)ptr);
			high = new Vector<double>((void*)ptr2);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00014ACC File Offset: 0x00012CCC
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<byte> Narrow(Vector<ushort> low, Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			byte* ptr;
			checked
			{
				ptr = stackalloc byte[unchecked((UIntPtr)count) * 1];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (byte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (byte)high[j];
			}
			return new Vector<byte>((void*)ptr);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00014B28 File Offset: 0x00012D28
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<ushort> Narrow(Vector<uint> low, Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			ushort* ptr;
			checked
			{
				ptr = stackalloc ushort[unchecked((UIntPtr)count) * 2];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (ushort)high[j];
			}
			return new Vector<ushort>((void*)ptr);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00014B88 File Offset: 0x00012D88
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<uint> Narrow(Vector<ulong> low, Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			uint* ptr;
			checked
			{
				ptr = stackalloc uint[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (uint)high[j];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00014BE8 File Offset: 0x00012DE8
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<sbyte> Narrow(Vector<short> low, Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			sbyte* ptr;
			checked
			{
				ptr = stackalloc sbyte[unchecked((UIntPtr)count) * 1];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (sbyte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (sbyte)high[j];
			}
			return new Vector<sbyte>((void*)ptr);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00014C44 File Offset: 0x00012E44
		[JitIntrinsic]
		public unsafe static Vector<short> Narrow(Vector<int> low, Vector<int> high)
		{
			int count = Vector<short>.Count;
			short* ptr;
			checked
			{
				ptr = stackalloc short[unchecked((UIntPtr)count) * 2];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (short)high[j];
			}
			return new Vector<short>((void*)ptr);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00014CA4 File Offset: 0x00012EA4
		[JitIntrinsic]
		public unsafe static Vector<int> Narrow(Vector<long> low, Vector<long> high)
		{
			int count = Vector<int>.Count;
			int* ptr;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (int)high[j];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00014D04 File Offset: 0x00012F04
		[JitIntrinsic]
		public unsafe static Vector<float> Narrow(Vector<double> low, Vector<double> high)
		{
			int count = Vector<float>.Count;
			float* ptr;
			checked
			{
				ptr = stackalloc float[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (float)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (float)high[j];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00014D64 File Offset: 0x00012F64
		[JitIntrinsic]
		public unsafe static Vector<float> ConvertToSingle(Vector<int> value)
		{
			int count = Vector<float>.Count;
			float* ptr;
			checked
			{
				ptr = stackalloc float[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (float)value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00014DA0 File Offset: 0x00012FA0
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<float> ConvertToSingle(Vector<uint> value)
		{
			int count = Vector<float>.Count;
			float* ptr;
			checked
			{
				ptr = stackalloc float[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00014DE0 File Offset: 0x00012FE0
		[JitIntrinsic]
		public unsafe static Vector<double> ConvertToDouble(Vector<long> value)
		{
			int count = Vector<double>.Count;
			double* ptr;
			checked
			{
				ptr = stackalloc double[unchecked((UIntPtr)count) * 8];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (double)value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00014E1C File Offset: 0x0001301C
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<double> ConvertToDouble(Vector<ulong> value)
		{
			int count = Vector<double>.Count;
			double* ptr;
			checked
			{
				ptr = stackalloc double[unchecked((UIntPtr)count) * 8];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00014E5C File Offset: 0x0001305C
		[JitIntrinsic]
		public unsafe static Vector<int> ConvertToInt32(Vector<float> value)
		{
			int count = Vector<int>.Count;
			int* ptr;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (int)value[i];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00014E98 File Offset: 0x00013098
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<uint> ConvertToUInt32(Vector<float> value)
		{
			int count = Vector<uint>.Count;
			uint* ptr;
			checked
			{
				ptr = stackalloc uint[unchecked((UIntPtr)count) * 4];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (uint)value[i];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00014ED4 File Offset: 0x000130D4
		[JitIntrinsic]
		public unsafe static Vector<long> ConvertToInt64(Vector<double> value)
		{
			int count = Vector<long>.Count;
			long* ptr;
			checked
			{
				ptr = stackalloc long[unchecked((UIntPtr)count) * 8];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (long)value[i];
			}
			return new Vector<long>((void*)ptr);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00014F10 File Offset: 0x00013110
		[CLSCompliant(false)]
		[JitIntrinsic]
		public unsafe static Vector<ulong> ConvertToUInt64(Vector<double> value)
		{
			int count = Vector<ulong>.Count;
			ulong* ptr;
			checked
			{
				ptr = stackalloc ulong[unchecked((UIntPtr)count) * 8];
			}
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (ulong)value[i];
			}
			return new Vector<ulong>((void*)ptr);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00014F4C File Offset: 0x0001314C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right)
		{
			return Vector<float>.ConditionalSelect((Vector<float>)condition, left, right);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00014F5B File Offset: 0x0001315B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right)
		{
			return Vector<double>.ConditionalSelect((Vector<double>)condition, left, right);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00014F6A File Offset: 0x0001316A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.ConditionalSelect(condition, left, right);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00014F74 File Offset: 0x00013174
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Equals(left, right);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00014F7D File Offset: 0x0001317D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.Equals(left, right);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00014F8B File Offset: 0x0001318B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.Equals(left, right);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00014F94 File Offset: 0x00013194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.Equals(left, right);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00014FA2 File Offset: 0x000131A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.Equals(left, right);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00014FAB File Offset: 0x000131AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left == right;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00014FB4 File Offset: 0x000131B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !Vector<T>.Equals(left, right).Equals(Vector<T>.Zero);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00014FD8 File Offset: 0x000131D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThan(left, right);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00014FE1 File Offset: 0x000131E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThan(left, right);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00014FEF File Offset: 0x000131EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThan(left, right);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00014FF8 File Offset: 0x000131F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThan(left, right);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00015006 File Offset: 0x00013206
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThan(left, right);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00015010 File Offset: 0x00013210
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00015038 File Offset: 0x00013238
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00015061 File Offset: 0x00013261
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0001506A File Offset: 0x0001326A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00015078 File Offset: 0x00013278
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00015081 File Offset: 0x00013281
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0001508A File Offset: 0x0001328A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00015098 File Offset: 0x00013298
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000150C0 File Offset: 0x000132C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000150E9 File Offset: 0x000132E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThan(left, right);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000150F2 File Offset: 0x000132F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThan(left, right);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00015100 File Offset: 0x00013300
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThan(left, right);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00015109 File Offset: 0x00013309
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThan(left, right);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00015117 File Offset: 0x00013317
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThan(left, right);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00015120 File Offset: 0x00013320
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00015148 File Offset: 0x00013348
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00015171 File Offset: 0x00013371
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0001517A File Offset: 0x0001337A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00015188 File Offset: 0x00013388
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00015191 File Offset: 0x00013391
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0001519A File Offset: 0x0001339A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000151A8 File Offset: 0x000133A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000151D0 File Offset: 0x000133D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000020B1 File Offset: 0x000002B1
		[JitIntrinsic]
		public static bool IsHardwareAccelerated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000151F9 File Offset: 0x000133F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Abs<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.Abs(value);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00015201 File Offset: 0x00013401
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Min<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Min(left, right);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0001520A File Offset: 0x0001340A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Max<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Max(left, right);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00015213 File Offset: 0x00013413
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Dot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.DotProduct(left, right);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0001521C File Offset: 0x0001341C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> SquareRoot<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.SquareRoot(value);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00015224 File Offset: 0x00013424
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Add<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left + right;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0001522D File Offset: 0x0001342D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left - right;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00015236 File Offset: 0x00013436
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0001523F File Offset: 0x0001343F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, T right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00015248 File Offset: 0x00013448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(T left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00015251 File Offset: 0x00013451
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left / right;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0001525A File Offset: 0x0001345A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Negate<T>(Vector<T> value) where T : struct
		{
			return -value;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00015262 File Offset: 0x00013462
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & right;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0001526B File Offset: 0x0001346B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left | right;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00015274 File Offset: 0x00013474
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> OnesComplement<T>(Vector<T> value) where T : struct
		{
			return ~value;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0001527C File Offset: 0x0001347C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left ^ right;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00015285 File Offset: 0x00013485
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & ~right;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00015293 File Offset: 0x00013493
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<byte> AsVectorByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<byte>)value;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0001529B File Offset: 0x0001349B
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<sbyte>)value;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000152A3 File Offset: 0x000134A3
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<ushort>)value;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000152AB File Offset: 0x000134AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<short> AsVectorInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<short>)value;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000152B3 File Offset: 0x000134B3
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<uint> AsVectorUInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<uint>)value;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000152BB File Offset: 0x000134BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> AsVectorInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<int>)value;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000152C3 File Offset: 0x000134C3
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<ulong>)value;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000152CB File Offset: 0x000134CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> AsVectorInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<long>)value;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000152D3 File Offset: 0x000134D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> AsVectorSingle<T>(Vector<T> value) where T : struct
		{
			return (Vector<float>)value;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000152DB File Offset: 0x000134DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> AsVectorDouble<T>(Vector<T> value) where T : struct
		{
			return (Vector<double>)value;
		}
	}
}
