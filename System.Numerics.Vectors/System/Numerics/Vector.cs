using System;
using System.Globalization;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics
{
	// Token: 0x02000009 RID: 9
	public struct Vector<T> : IEquatable<Vector<T>>, IFormattable where T : struct
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000022FA File Offset: 0x000004FA
		[JitIntrinsic]
		public static int Count
		{
			get
			{
				return Vector<T>.s_count;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002301 File Offset: 0x00000501
		[JitIntrinsic]
		public static Vector<T> Zero
		{
			get
			{
				return Vector<T>.zero;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002308 File Offset: 0x00000508
		[JitIntrinsic]
		public static Vector<T> One
		{
			get
			{
				return Vector<T>.one;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000230F File Offset: 0x0000050F
		internal static Vector<T> AllOnes
		{
			get
			{
				return Vector<T>.allOnes;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002318 File Offset: 0x00000518
		private unsafe static int InitializeCount()
		{
			Vector<T>.VectorSizeHelper vectorSizeHelper;
			byte* ptr = &vectorSizeHelper._placeholder.register.byte_0;
			byte* ptr2 = &vectorSizeHelper._byte;
			int num = (int)((long)(ptr2 - ptr));
			int num2;
			if (typeof(T) == typeof(byte))
			{
				num2 = 1;
			}
			else if (typeof(T) == typeof(sbyte))
			{
				num2 = 1;
			}
			else if (typeof(T) == typeof(ushort))
			{
				num2 = 2;
			}
			else if (typeof(T) == typeof(short))
			{
				num2 = 2;
			}
			else if (typeof(T) == typeof(uint))
			{
				num2 = 4;
			}
			else if (typeof(T) == typeof(int))
			{
				num2 = 4;
			}
			else if (typeof(T) == typeof(ulong))
			{
				num2 = 8;
			}
			else if (typeof(T) == typeof(long))
			{
				num2 = 8;
			}
			else if (typeof(T) == typeof(float))
			{
				num2 = 4;
			}
			else
			{
				if (!(typeof(T) == typeof(double)))
				{
					throw new NotSupportedException(SR.Arg_TypeNotSupported);
				}
				num2 = 8;
			}
			return num / num2;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000024B0 File Offset: 0x000006B0
		[JitIntrinsic]
		public unsafe Vector(T value)
		{
			this = default(Vector<T>);
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					fixed (byte* ptr = &this.register.byte_0)
					{
						for (int i = 0; i < Vector<T>.Count; i++)
						{
							ptr[i] = (byte)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					fixed (sbyte* ptr2 = &this.register.sbyte_0)
					{
						for (int j = 0; j < Vector<T>.Count; j++)
						{
							ptr2[j] = (sbyte)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					fixed (ushort* ptr3 = &this.register.uint16_0)
					{
						for (int k = 0; k < Vector<T>.Count; k++)
						{
							ptr3[k] = (ushort)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(short))
				{
					fixed (short* ptr4 = &this.register.int16_0)
					{
						for (int l = 0; l < Vector<T>.Count; l++)
						{
							ptr4[l] = (short)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					fixed (uint* ptr5 = &this.register.uint32_0)
					{
						for (int m = 0; m < Vector<T>.Count; m++)
						{
							ptr5[m] = (uint)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(int))
				{
					fixed (int* ptr6 = &this.register.int32_0)
					{
						for (int n = 0; n < Vector<T>.Count; n++)
						{
							ptr6[n] = (int)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					fixed (ulong* ptr7 = &this.register.uint64_0)
					{
						for (int num = 0; num < Vector<T>.Count; num++)
						{
							ptr7[num] = (ulong)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(long))
				{
					fixed (long* ptr8 = &this.register.int64_0)
					{
						for (int num2 = 0; num2 < Vector<T>.Count; num2++)
						{
							ptr8[num2] = (long)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(float))
				{
					fixed (float* ptr9 = &this.register.single_0)
					{
						for (int num3 = 0; num3 < Vector<T>.Count; num3++)
						{
							ptr9[num3] = (float)((object)value);
						}
					}
					return;
				}
				if (typeof(T) == typeof(double))
				{
					fixed (double* ptr10 = &this.register.double_0)
					{
						for (int num4 = 0; num4 < Vector<T>.Count; num4++)
						{
							ptr10[num4] = (double)((object)value);
						}
					}
					return;
				}
			}
			else
			{
				if (typeof(T) == typeof(byte))
				{
					this.register.byte_0 = (byte)((object)value);
					this.register.byte_1 = (byte)((object)value);
					this.register.byte_2 = (byte)((object)value);
					this.register.byte_3 = (byte)((object)value);
					this.register.byte_4 = (byte)((object)value);
					this.register.byte_5 = (byte)((object)value);
					this.register.byte_6 = (byte)((object)value);
					this.register.byte_7 = (byte)((object)value);
					this.register.byte_8 = (byte)((object)value);
					this.register.byte_9 = (byte)((object)value);
					this.register.byte_10 = (byte)((object)value);
					this.register.byte_11 = (byte)((object)value);
					this.register.byte_12 = (byte)((object)value);
					this.register.byte_13 = (byte)((object)value);
					this.register.byte_14 = (byte)((object)value);
					this.register.byte_15 = (byte)((object)value);
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					this.register.sbyte_0 = (sbyte)((object)value);
					this.register.sbyte_1 = (sbyte)((object)value);
					this.register.sbyte_2 = (sbyte)((object)value);
					this.register.sbyte_3 = (sbyte)((object)value);
					this.register.sbyte_4 = (sbyte)((object)value);
					this.register.sbyte_5 = (sbyte)((object)value);
					this.register.sbyte_6 = (sbyte)((object)value);
					this.register.sbyte_7 = (sbyte)((object)value);
					this.register.sbyte_8 = (sbyte)((object)value);
					this.register.sbyte_9 = (sbyte)((object)value);
					this.register.sbyte_10 = (sbyte)((object)value);
					this.register.sbyte_11 = (sbyte)((object)value);
					this.register.sbyte_12 = (sbyte)((object)value);
					this.register.sbyte_13 = (sbyte)((object)value);
					this.register.sbyte_14 = (sbyte)((object)value);
					this.register.sbyte_15 = (sbyte)((object)value);
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					this.register.uint16_0 = (ushort)((object)value);
					this.register.uint16_1 = (ushort)((object)value);
					this.register.uint16_2 = (ushort)((object)value);
					this.register.uint16_3 = (ushort)((object)value);
					this.register.uint16_4 = (ushort)((object)value);
					this.register.uint16_5 = (ushort)((object)value);
					this.register.uint16_6 = (ushort)((object)value);
					this.register.uint16_7 = (ushort)((object)value);
					return;
				}
				if (typeof(T) == typeof(short))
				{
					this.register.int16_0 = (short)((object)value);
					this.register.int16_1 = (short)((object)value);
					this.register.int16_2 = (short)((object)value);
					this.register.int16_3 = (short)((object)value);
					this.register.int16_4 = (short)((object)value);
					this.register.int16_5 = (short)((object)value);
					this.register.int16_6 = (short)((object)value);
					this.register.int16_7 = (short)((object)value);
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					this.register.uint32_0 = (uint)((object)value);
					this.register.uint32_1 = (uint)((object)value);
					this.register.uint32_2 = (uint)((object)value);
					this.register.uint32_3 = (uint)((object)value);
					return;
				}
				if (typeof(T) == typeof(int))
				{
					this.register.int32_0 = (int)((object)value);
					this.register.int32_1 = (int)((object)value);
					this.register.int32_2 = (int)((object)value);
					this.register.int32_3 = (int)((object)value);
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					this.register.uint64_0 = (ulong)((object)value);
					this.register.uint64_1 = (ulong)((object)value);
					return;
				}
				if (typeof(T) == typeof(long))
				{
					this.register.int64_0 = (long)((object)value);
					this.register.int64_1 = (long)((object)value);
					return;
				}
				if (typeof(T) == typeof(float))
				{
					this.register.single_0 = (float)((object)value);
					this.register.single_1 = (float)((object)value);
					this.register.single_2 = (float)((object)value);
					this.register.single_3 = (float)((object)value);
					return;
				}
				if (typeof(T) == typeof(double))
				{
					this.register.double_0 = (double)((object)value);
					this.register.double_1 = (double)((object)value);
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002EE6 File Offset: 0x000010E6
		[JitIntrinsic]
		public Vector(T[] values)
		{
			this = new Vector<T>(values, 0);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002EF0 File Offset: 0x000010F0
		public unsafe Vector(T[] values, int index)
		{
			this = default(Vector<T>);
			if (values == null)
			{
				throw new NullReferenceException(SR.Arg_NullArgumentNullRef);
			}
			if (index < 0 || values.Length - index < Vector<T>.Count)
			{
				throw new IndexOutOfRangeException();
			}
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					fixed (byte* ptr = &this.register.byte_0)
					{
						for (int i = 0; i < Vector<T>.Count; i++)
						{
							ptr[i] = (byte)((object)values[i + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					fixed (sbyte* ptr2 = &this.register.sbyte_0)
					{
						for (int j = 0; j < Vector<T>.Count; j++)
						{
							ptr2[j] = (sbyte)((object)values[j + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					fixed (ushort* ptr3 = &this.register.uint16_0)
					{
						for (int k = 0; k < Vector<T>.Count; k++)
						{
							ptr3[k] = (ushort)((object)values[k + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(short))
				{
					fixed (short* ptr4 = &this.register.int16_0)
					{
						for (int l = 0; l < Vector<T>.Count; l++)
						{
							ptr4[l] = (short)((object)values[l + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					fixed (uint* ptr5 = &this.register.uint32_0)
					{
						for (int m = 0; m < Vector<T>.Count; m++)
						{
							ptr5[m] = (uint)((object)values[m + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(int))
				{
					fixed (int* ptr6 = &this.register.int32_0)
					{
						for (int n = 0; n < Vector<T>.Count; n++)
						{
							ptr6[n] = (int)((object)values[n + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					fixed (ulong* ptr7 = &this.register.uint64_0)
					{
						for (int num = 0; num < Vector<T>.Count; num++)
						{
							ptr7[num] = (ulong)((object)values[num + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(long))
				{
					fixed (long* ptr8 = &this.register.int64_0)
					{
						for (int num2 = 0; num2 < Vector<T>.Count; num2++)
						{
							ptr8[num2] = (long)((object)values[num2 + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(float))
				{
					fixed (float* ptr9 = &this.register.single_0)
					{
						for (int num3 = 0; num3 < Vector<T>.Count; num3++)
						{
							ptr9[num3] = (float)((object)values[num3 + index]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(double))
				{
					fixed (double* ptr10 = &this.register.double_0)
					{
						for (int num4 = 0; num4 < Vector<T>.Count; num4++)
						{
							ptr10[num4] = (double)((object)values[num4 + index]);
						}
					}
					return;
				}
			}
			else
			{
				if (typeof(T) == typeof(byte))
				{
					fixed (byte* ptr11 = &this.register.byte_0)
					{
						*ptr11 = (byte)((object)values[index]);
						ptr11[1] = (byte)((object)values[1 + index]);
						ptr11[2] = (byte)((object)values[2 + index]);
						ptr11[3] = (byte)((object)values[3 + index]);
						ptr11[4] = (byte)((object)values[4 + index]);
						ptr11[5] = (byte)((object)values[5 + index]);
						ptr11[6] = (byte)((object)values[6 + index]);
						ptr11[7] = (byte)((object)values[7 + index]);
						ptr11[8] = (byte)((object)values[8 + index]);
						ptr11[9] = (byte)((object)values[9 + index]);
						ptr11[10] = (byte)((object)values[10 + index]);
						ptr11[11] = (byte)((object)values[11 + index]);
						ptr11[12] = (byte)((object)values[12 + index]);
						ptr11[13] = (byte)((object)values[13 + index]);
						ptr11[14] = (byte)((object)values[14 + index]);
						ptr11[15] = (byte)((object)values[15 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					fixed (sbyte* ptr12 = &this.register.sbyte_0)
					{
						*ptr12 = (sbyte)((object)values[index]);
						ptr12[1] = (sbyte)((object)values[1 + index]);
						ptr12[2] = (sbyte)((object)values[2 + index]);
						ptr12[3] = (sbyte)((object)values[3 + index]);
						ptr12[4] = (sbyte)((object)values[4 + index]);
						ptr12[5] = (sbyte)((object)values[5 + index]);
						ptr12[6] = (sbyte)((object)values[6 + index]);
						ptr12[7] = (sbyte)((object)values[7 + index]);
						ptr12[8] = (sbyte)((object)values[8 + index]);
						ptr12[9] = (sbyte)((object)values[9 + index]);
						ptr12[10] = (sbyte)((object)values[10 + index]);
						ptr12[11] = (sbyte)((object)values[11 + index]);
						ptr12[12] = (sbyte)((object)values[12 + index]);
						ptr12[13] = (sbyte)((object)values[13 + index]);
						ptr12[14] = (sbyte)((object)values[14 + index]);
						ptr12[15] = (sbyte)((object)values[15 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					fixed (ushort* ptr13 = &this.register.uint16_0)
					{
						*ptr13 = (ushort)((object)values[index]);
						ptr13[1] = (ushort)((object)values[1 + index]);
						ptr13[2] = (ushort)((object)values[2 + index]);
						ptr13[3] = (ushort)((object)values[3 + index]);
						ptr13[4] = (ushort)((object)values[4 + index]);
						ptr13[5] = (ushort)((object)values[5 + index]);
						ptr13[6] = (ushort)((object)values[6 + index]);
						ptr13[7] = (ushort)((object)values[7 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(short))
				{
					fixed (short* ptr14 = &this.register.int16_0)
					{
						*ptr14 = (short)((object)values[index]);
						ptr14[1] = (short)((object)values[1 + index]);
						ptr14[2] = (short)((object)values[2 + index]);
						ptr14[3] = (short)((object)values[3 + index]);
						ptr14[4] = (short)((object)values[4 + index]);
						ptr14[5] = (short)((object)values[5 + index]);
						ptr14[6] = (short)((object)values[6 + index]);
						ptr14[7] = (short)((object)values[7 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					fixed (uint* ptr15 = &this.register.uint32_0)
					{
						*ptr15 = (uint)((object)values[index]);
						ptr15[1] = (uint)((object)values[1 + index]);
						ptr15[2] = (uint)((object)values[2 + index]);
						ptr15[3] = (uint)((object)values[3 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(int))
				{
					fixed (int* ptr16 = &this.register.int32_0)
					{
						*ptr16 = (int)((object)values[index]);
						ptr16[1] = (int)((object)values[1 + index]);
						ptr16[2] = (int)((object)values[2 + index]);
						ptr16[3] = (int)((object)values[3 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					fixed (ulong* ptr17 = &this.register.uint64_0)
					{
						*ptr17 = (ulong)((object)values[index]);
						ptr17[1] = (ulong)((object)values[1 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(long))
				{
					fixed (long* ptr18 = &this.register.int64_0)
					{
						*ptr18 = (long)((object)values[index]);
						ptr18[1] = (long)((object)values[1 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(float))
				{
					fixed (float* ptr19 = &this.register.single_0)
					{
						*ptr19 = (float)((object)values[index]);
						ptr19[1] = (float)((object)values[1 + index]);
						ptr19[2] = (float)((object)values[2 + index]);
						ptr19[3] = (float)((object)values[3 + index]);
					}
					return;
				}
				if (typeof(T) == typeof(double))
				{
					fixed (double* ptr20 = &this.register.double_0)
					{
						*ptr20 = (double)((object)values[index]);
						ptr20[1] = (double)((object)values[1 + index]);
					}
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003B3C File Offset: 0x00001D3C
		internal unsafe Vector(void* dataPointer)
		{
			this = new Vector<T>(dataPointer, 0);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003B48 File Offset: 0x00001D48
		internal unsafe Vector(void* dataPointer, int offset)
		{
			this = default(Vector<T>);
			if (typeof(T) == typeof(byte))
			{
				byte* ptr = (byte*)dataPointer + offset;
				fixed (byte* ptr2 = &this.register.byte_0)
				{
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr2[i] = ptr[i];
					}
				}
				return;
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte* ptr3 = (sbyte*)((byte*)dataPointer + offset);
				fixed (sbyte* ptr4 = &this.register.sbyte_0)
				{
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr4[j] = ptr3[j];
					}
				}
				return;
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort* ptr5 = (ushort*)((byte*)dataPointer + (IntPtr)offset * 2);
				fixed (ushort* ptr6 = &this.register.uint16_0)
				{
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr6[k] = ptr5[k];
					}
				}
				return;
			}
			if (typeof(T) == typeof(short))
			{
				short* ptr7 = (short*)((byte*)dataPointer + (IntPtr)offset * 2);
				fixed (short* ptr8 = &this.register.int16_0)
				{
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr8[l] = ptr7[l];
					}
				}
				return;
			}
			if (typeof(T) == typeof(uint))
			{
				uint* ptr9 = (uint*)((byte*)dataPointer + (IntPtr)offset * 4);
				fixed (uint* ptr10 = &this.register.uint32_0)
				{
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr10[m] = ptr9[m];
					}
				}
				return;
			}
			if (typeof(T) == typeof(int))
			{
				int* ptr11 = (int*)((byte*)dataPointer + (IntPtr)offset * 4);
				fixed (int* ptr12 = &this.register.int32_0)
				{
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr12[n] = ptr11[n];
					}
				}
				return;
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong* ptr13 = (ulong*)((byte*)dataPointer + (IntPtr)offset * 8);
				fixed (ulong* ptr14 = &this.register.uint64_0)
				{
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr14[num] = ptr13[num];
					}
				}
				return;
			}
			if (typeof(T) == typeof(long))
			{
				long* ptr15 = (long*)((byte*)dataPointer + (IntPtr)offset * 8);
				fixed (long* ptr16 = &this.register.int64_0)
				{
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr16[num2] = ptr15[num2];
					}
				}
				return;
			}
			if (typeof(T) == typeof(float))
			{
				float* ptr17 = (float*)((byte*)dataPointer + (IntPtr)offset * 4);
				fixed (float* ptr18 = &this.register.single_0)
				{
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr18[num3] = ptr17[num3];
					}
				}
				return;
			}
			if (typeof(T) == typeof(double))
			{
				double* ptr19 = (double*)((byte*)dataPointer + (IntPtr)offset * 8);
				fixed (double* ptr20 = &this.register.double_0)
				{
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr20[num4] = ptr19[num4];
					}
				}
				return;
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003F03 File Offset: 0x00002103
		private Vector(ref Register existingRegister)
		{
			this.register = existingRegister;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003F11 File Offset: 0x00002111
		[JitIntrinsic]
		public void CopyTo(T[] destination)
		{
			this.CopyTo(destination, 0);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003F1C File Offset: 0x0000211C
		[JitIntrinsic]
		public unsafe void CopyTo(T[] destination, int startIndex)
		{
			if (destination == null)
			{
				throw new NullReferenceException(SR.Arg_NullArgumentNullRef);
			}
			if (startIndex < 0 || startIndex >= destination.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", SR.Format(SR.Arg_ArgumentOutOfRangeException, startIndex));
			}
			if (destination.Length - startIndex < Vector<T>.Count)
			{
				throw new ArgumentException(SR.Format(SR.Arg_ElementsInSourceIsGreaterThanDestination, startIndex));
			}
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte[] array = (byte[])destination;
					fixed (byte* ptr = array)
					{
						for (int i = 0; i < Vector<T>.Count; i++)
						{
							ptr[startIndex + i] = (byte)((object)this[i]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte[] array2 = (sbyte[])destination;
					fixed (sbyte* ptr2 = array2)
					{
						for (int j = 0; j < Vector<T>.Count; j++)
						{
							ptr2[startIndex + j] = (sbyte)((object)this[j]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort[] array3 = (ushort[])destination;
					fixed (ushort* ptr3 = array3)
					{
						for (int k = 0; k < Vector<T>.Count; k++)
						{
							ptr3[startIndex + k] = (ushort)((object)this[k]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(short))
				{
					short[] array4 = (short[])destination;
					fixed (short* ptr4 = array4)
					{
						for (int l = 0; l < Vector<T>.Count; l++)
						{
							ptr4[startIndex + l] = (short)((object)this[l]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					uint[] array5 = (uint[])destination;
					fixed (uint* ptr5 = array5)
					{
						for (int m = 0; m < Vector<T>.Count; m++)
						{
							ptr5[startIndex + m] = (uint)((object)this[m]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(int))
				{
					int[] array6 = (int[])destination;
					fixed (int* ptr6 = array6)
					{
						for (int n = 0; n < Vector<T>.Count; n++)
						{
							ptr6[startIndex + n] = (int)((object)this[n]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong[] array7 = (ulong[])destination;
					fixed (ulong* ptr7 = array7)
					{
						for (int num = 0; num < Vector<T>.Count; num++)
						{
							ptr7[startIndex + num] = (ulong)((object)this[num]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(long))
				{
					long[] array8 = (long[])destination;
					fixed (long* ptr8 = array8)
					{
						for (int num2 = 0; num2 < Vector<T>.Count; num2++)
						{
							ptr8[startIndex + num2] = (long)((object)this[num2]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(float))
				{
					float[] array9 = (float[])destination;
					fixed (float* ptr9 = array9)
					{
						for (int num3 = 0; num3 < Vector<T>.Count; num3++)
						{
							ptr9[startIndex + num3] = (float)((object)this[num3]);
						}
					}
					return;
				}
				if (typeof(T) == typeof(double))
				{
					double[] array10 = (double[])destination;
					fixed (double* ptr10 = array10)
					{
						for (int num4 = 0; num4 < Vector<T>.Count; num4++)
						{
							ptr10[startIndex + num4] = (double)((object)this[num4]);
						}
					}
					return;
				}
			}
			else
			{
				if (typeof(T) == typeof(byte))
				{
					byte[] array11 = (byte[])destination;
					fixed (byte* ptr11 = array11)
					{
						ptr11[startIndex] = this.register.byte_0;
						ptr11[startIndex + 1] = this.register.byte_1;
						ptr11[startIndex + 2] = this.register.byte_2;
						ptr11[startIndex + 3] = this.register.byte_3;
						ptr11[startIndex + 4] = this.register.byte_4;
						ptr11[startIndex + 5] = this.register.byte_5;
						ptr11[startIndex + 6] = this.register.byte_6;
						ptr11[startIndex + 7] = this.register.byte_7;
						ptr11[startIndex + 8] = this.register.byte_8;
						ptr11[startIndex + 9] = this.register.byte_9;
						ptr11[startIndex + 10] = this.register.byte_10;
						ptr11[startIndex + 11] = this.register.byte_11;
						ptr11[startIndex + 12] = this.register.byte_12;
						ptr11[startIndex + 13] = this.register.byte_13;
						ptr11[startIndex + 14] = this.register.byte_14;
						ptr11[startIndex + 15] = this.register.byte_15;
					}
					return;
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte[] array12 = (sbyte[])destination;
					fixed (sbyte* ptr12 = array12)
					{
						ptr12[startIndex] = this.register.sbyte_0;
						ptr12[startIndex + 1] = this.register.sbyte_1;
						ptr12[startIndex + 2] = this.register.sbyte_2;
						ptr12[startIndex + 3] = this.register.sbyte_3;
						ptr12[startIndex + 4] = this.register.sbyte_4;
						ptr12[startIndex + 5] = this.register.sbyte_5;
						ptr12[startIndex + 6] = this.register.sbyte_6;
						ptr12[startIndex + 7] = this.register.sbyte_7;
						ptr12[startIndex + 8] = this.register.sbyte_8;
						ptr12[startIndex + 9] = this.register.sbyte_9;
						ptr12[startIndex + 10] = this.register.sbyte_10;
						ptr12[startIndex + 11] = this.register.sbyte_11;
						ptr12[startIndex + 12] = this.register.sbyte_12;
						ptr12[startIndex + 13] = this.register.sbyte_13;
						ptr12[startIndex + 14] = this.register.sbyte_14;
						ptr12[startIndex + 15] = this.register.sbyte_15;
					}
					return;
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort[] array13 = (ushort[])destination;
					fixed (ushort* ptr13 = array13)
					{
						ptr13[startIndex] = this.register.uint16_0;
						ptr13[startIndex + 1] = this.register.uint16_1;
						ptr13[startIndex + 2] = this.register.uint16_2;
						ptr13[startIndex + 3] = this.register.uint16_3;
						ptr13[startIndex + 4] = this.register.uint16_4;
						ptr13[startIndex + 5] = this.register.uint16_5;
						ptr13[startIndex + 6] = this.register.uint16_6;
						ptr13[startIndex + 7] = this.register.uint16_7;
					}
					return;
				}
				if (typeof(T) == typeof(short))
				{
					short[] array14 = (short[])destination;
					fixed (short* ptr14 = array14)
					{
						ptr14[startIndex] = this.register.int16_0;
						ptr14[startIndex + 1] = this.register.int16_1;
						ptr14[startIndex + 2] = this.register.int16_2;
						ptr14[startIndex + 3] = this.register.int16_3;
						ptr14[startIndex + 4] = this.register.int16_4;
						ptr14[startIndex + 5] = this.register.int16_5;
						ptr14[startIndex + 6] = this.register.int16_6;
						ptr14[startIndex + 7] = this.register.int16_7;
					}
					return;
				}
				if (typeof(T) == typeof(uint))
				{
					uint[] array15 = (uint[])destination;
					fixed (uint* ptr15 = array15)
					{
						ptr15[startIndex] = this.register.uint32_0;
						ptr15[startIndex + 1] = this.register.uint32_1;
						ptr15[startIndex + 2] = this.register.uint32_2;
						ptr15[startIndex + 3] = this.register.uint32_3;
					}
					return;
				}
				if (typeof(T) == typeof(int))
				{
					int[] array16 = (int[])destination;
					fixed (int* ptr16 = array16)
					{
						ptr16[startIndex] = this.register.int32_0;
						ptr16[startIndex + 1] = this.register.int32_1;
						ptr16[startIndex + 2] = this.register.int32_2;
						ptr16[startIndex + 3] = this.register.int32_3;
					}
					return;
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong[] array17 = (ulong[])destination;
					fixed (ulong* ptr17 = array17)
					{
						ptr17[startIndex] = this.register.uint64_0;
						ptr17[startIndex + 1] = this.register.uint64_1;
					}
					return;
				}
				if (typeof(T) == typeof(long))
				{
					long[] array18 = (long[])destination;
					fixed (long* ptr18 = array18)
					{
						ptr18[startIndex] = this.register.int64_0;
						ptr18[startIndex + 1] = this.register.int64_1;
					}
					return;
				}
				if (typeof(T) == typeof(float))
				{
					float[] array19 = (float[])destination;
					fixed (float* ptr19 = array19)
					{
						ptr19[startIndex] = this.register.single_0;
						ptr19[startIndex + 1] = this.register.single_1;
						ptr19[startIndex + 2] = this.register.single_2;
						ptr19[startIndex + 3] = this.register.single_3;
					}
					return;
				}
				if (typeof(T) == typeof(double))
				{
					double[] array20 = (double[])destination;
					fixed (double* ptr20 = array20)
					{
						ptr20[startIndex] = this.register.double_0;
						ptr20[startIndex + 1] = this.register.double_1;
					}
				}
			}
		}

		// Token: 0x1700000B RID: 11
		[JitIntrinsic]
		public unsafe T this[int index]
		{
			get
			{
				if (index >= Vector<T>.Count || index < 0)
				{
					throw new IndexOutOfRangeException(SR.Format(SR.Arg_ArgumentOutOfRangeException, index));
				}
				if (typeof(T) == typeof(byte))
				{
					fixed (byte* ptr = &this.register.byte_0)
					{
						return (T)((object)ptr[index]);
					}
				}
				if (typeof(T) == typeof(sbyte))
				{
					fixed (sbyte* ptr2 = &this.register.sbyte_0)
					{
						return (T)((object)ptr2[index]);
					}
				}
				if (typeof(T) == typeof(ushort))
				{
					fixed (ushort* ptr3 = &this.register.uint16_0)
					{
						return (T)((object)ptr3[index]);
					}
				}
				if (typeof(T) == typeof(short))
				{
					fixed (short* ptr4 = &this.register.int16_0)
					{
						return (T)((object)ptr4[index]);
					}
				}
				if (typeof(T) == typeof(uint))
				{
					fixed (uint* ptr5 = &this.register.uint32_0)
					{
						return (T)((object)ptr5[index]);
					}
				}
				if (typeof(T) == typeof(int))
				{
					fixed (int* ptr6 = &this.register.int32_0)
					{
						return (T)((object)ptr6[index]);
					}
				}
				if (typeof(T) == typeof(ulong))
				{
					fixed (ulong* ptr7 = &this.register.uint64_0)
					{
						return (T)((object)ptr7[index]);
					}
				}
				if (typeof(T) == typeof(long))
				{
					fixed (long* ptr8 = &this.register.int64_0)
					{
						return (T)((object)ptr8[index]);
					}
				}
				if (typeof(T) == typeof(float))
				{
					fixed (float* ptr9 = &this.register.single_0)
					{
						return (T)((object)ptr9[index]);
					}
				}
				if (typeof(T) == typeof(double))
				{
					fixed (double* ptr10 = &this.register.double_0)
					{
						return (T)((object)ptr10[index]);
					}
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004EAB File Offset: 0x000030AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is Vector<T> && this.Equals((Vector<T>)obj);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004EC4 File Offset: 0x000030C4
		[JitIntrinsic]
		public bool Equals(Vector<T> other)
		{
			if (Vector.IsHardwareAccelerated)
			{
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					if (!Vector<T>.ScalarEquals(this[i], other[i]))
					{
						return false;
					}
				}
				return true;
			}
			if (typeof(T) == typeof(byte))
			{
				return this.register.byte_0 == other.register.byte_0 && this.register.byte_1 == other.register.byte_1 && this.register.byte_2 == other.register.byte_2 && this.register.byte_3 == other.register.byte_3 && this.register.byte_4 == other.register.byte_4 && this.register.byte_5 == other.register.byte_5 && this.register.byte_6 == other.register.byte_6 && this.register.byte_7 == other.register.byte_7 && this.register.byte_8 == other.register.byte_8 && this.register.byte_9 == other.register.byte_9 && this.register.byte_10 == other.register.byte_10 && this.register.byte_11 == other.register.byte_11 && this.register.byte_12 == other.register.byte_12 && this.register.byte_13 == other.register.byte_13 && this.register.byte_14 == other.register.byte_14 && this.register.byte_15 == other.register.byte_15;
			}
			if (typeof(T) == typeof(sbyte))
			{
				return this.register.sbyte_0 == other.register.sbyte_0 && this.register.sbyte_1 == other.register.sbyte_1 && this.register.sbyte_2 == other.register.sbyte_2 && this.register.sbyte_3 == other.register.sbyte_3 && this.register.sbyte_4 == other.register.sbyte_4 && this.register.sbyte_5 == other.register.sbyte_5 && this.register.sbyte_6 == other.register.sbyte_6 && this.register.sbyte_7 == other.register.sbyte_7 && this.register.sbyte_8 == other.register.sbyte_8 && this.register.sbyte_9 == other.register.sbyte_9 && this.register.sbyte_10 == other.register.sbyte_10 && this.register.sbyte_11 == other.register.sbyte_11 && this.register.sbyte_12 == other.register.sbyte_12 && this.register.sbyte_13 == other.register.sbyte_13 && this.register.sbyte_14 == other.register.sbyte_14 && this.register.sbyte_15 == other.register.sbyte_15;
			}
			if (typeof(T) == typeof(ushort))
			{
				return this.register.uint16_0 == other.register.uint16_0 && this.register.uint16_1 == other.register.uint16_1 && this.register.uint16_2 == other.register.uint16_2 && this.register.uint16_3 == other.register.uint16_3 && this.register.uint16_4 == other.register.uint16_4 && this.register.uint16_5 == other.register.uint16_5 && this.register.uint16_6 == other.register.uint16_6 && this.register.uint16_7 == other.register.uint16_7;
			}
			if (typeof(T) == typeof(short))
			{
				return this.register.int16_0 == other.register.int16_0 && this.register.int16_1 == other.register.int16_1 && this.register.int16_2 == other.register.int16_2 && this.register.int16_3 == other.register.int16_3 && this.register.int16_4 == other.register.int16_4 && this.register.int16_5 == other.register.int16_5 && this.register.int16_6 == other.register.int16_6 && this.register.int16_7 == other.register.int16_7;
			}
			if (typeof(T) == typeof(uint))
			{
				return this.register.uint32_0 == other.register.uint32_0 && this.register.uint32_1 == other.register.uint32_1 && this.register.uint32_2 == other.register.uint32_2 && this.register.uint32_3 == other.register.uint32_3;
			}
			if (typeof(T) == typeof(int))
			{
				return this.register.int32_0 == other.register.int32_0 && this.register.int32_1 == other.register.int32_1 && this.register.int32_2 == other.register.int32_2 && this.register.int32_3 == other.register.int32_3;
			}
			if (typeof(T) == typeof(ulong))
			{
				return this.register.uint64_0 == other.register.uint64_0 && this.register.uint64_1 == other.register.uint64_1;
			}
			if (typeof(T) == typeof(long))
			{
				return this.register.int64_0 == other.register.int64_0 && this.register.int64_1 == other.register.int64_1;
			}
			if (typeof(T) == typeof(float))
			{
				return this.register.single_0 == other.register.single_0 && this.register.single_1 == other.register.single_1 && this.register.single_2 == other.register.single_2 && this.register.single_3 == other.register.single_3;
			}
			if (typeof(T) == typeof(double))
			{
				return this.register.double_0 == other.register.double_0 && this.register.double_1 == other.register.double_1;
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000056BC File Offset: 0x000038BC
		public override int GetHashCode()
		{
			int num = 0;
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						num = HashHelpers.Combine(num, ((byte)((object)this[i])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(sbyte))
				{
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						num = HashHelpers.Combine(num, ((sbyte)((object)this[j])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(ushort))
				{
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						num = HashHelpers.Combine(num, ((ushort)((object)this[k])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(short))
				{
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						num = HashHelpers.Combine(num, ((short)((object)this[l])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(uint))
				{
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						num = HashHelpers.Combine(num, ((uint)((object)this[m])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(int))
				{
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						num = HashHelpers.Combine(num, ((int)((object)this[n])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(ulong))
				{
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						num = HashHelpers.Combine(num, ((ulong)((object)this[num2])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(long))
				{
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						num = HashHelpers.Combine(num, ((long)((object)this[num3])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(float))
				{
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						num = HashHelpers.Combine(num, ((float)((object)this[num4])).GetHashCode());
					}
					return num;
				}
				if (typeof(T) == typeof(double))
				{
					for (int num5 = 0; num5 < Vector<T>.Count; num5++)
					{
						num = HashHelpers.Combine(num, ((double)((object)this[num5])).GetHashCode());
					}
					return num;
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				if (typeof(T) == typeof(byte))
				{
					num = HashHelpers.Combine(num, this.register.byte_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_2.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_3.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_4.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_5.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_6.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_7.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_8.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_9.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_10.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_11.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_12.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_13.GetHashCode());
					num = HashHelpers.Combine(num, this.register.byte_14.GetHashCode());
					return HashHelpers.Combine(num, this.register.byte_15.GetHashCode());
				}
				if (typeof(T) == typeof(sbyte))
				{
					num = HashHelpers.Combine(num, this.register.sbyte_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_2.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_3.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_4.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_5.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_6.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_7.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_8.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_9.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_10.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_11.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_12.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_13.GetHashCode());
					num = HashHelpers.Combine(num, this.register.sbyte_14.GetHashCode());
					return HashHelpers.Combine(num, this.register.sbyte_15.GetHashCode());
				}
				if (typeof(T) == typeof(ushort))
				{
					num = HashHelpers.Combine(num, this.register.uint16_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_2.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_3.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_4.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_5.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint16_6.GetHashCode());
					return HashHelpers.Combine(num, this.register.uint16_7.GetHashCode());
				}
				if (typeof(T) == typeof(short))
				{
					num = HashHelpers.Combine(num, this.register.int16_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_2.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_3.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_4.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_5.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int16_6.GetHashCode());
					return HashHelpers.Combine(num, this.register.int16_7.GetHashCode());
				}
				if (typeof(T) == typeof(uint))
				{
					num = HashHelpers.Combine(num, this.register.uint32_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint32_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.uint32_2.GetHashCode());
					return HashHelpers.Combine(num, this.register.uint32_3.GetHashCode());
				}
				if (typeof(T) == typeof(int))
				{
					num = HashHelpers.Combine(num, this.register.int32_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int32_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.int32_2.GetHashCode());
					return HashHelpers.Combine(num, this.register.int32_3.GetHashCode());
				}
				if (typeof(T) == typeof(ulong))
				{
					num = HashHelpers.Combine(num, this.register.uint64_0.GetHashCode());
					return HashHelpers.Combine(num, this.register.uint64_1.GetHashCode());
				}
				if (typeof(T) == typeof(long))
				{
					num = HashHelpers.Combine(num, this.register.int64_0.GetHashCode());
					return HashHelpers.Combine(num, this.register.int64_1.GetHashCode());
				}
				if (typeof(T) == typeof(float))
				{
					num = HashHelpers.Combine(num, this.register.single_0.GetHashCode());
					num = HashHelpers.Combine(num, this.register.single_1.GetHashCode());
					num = HashHelpers.Combine(num, this.register.single_2.GetHashCode());
					return HashHelpers.Combine(num, this.register.single_3.GetHashCode());
				}
				if (typeof(T) == typeof(double))
				{
					num = HashHelpers.Combine(num, this.register.double_0.GetHashCode());
					return HashHelpers.Combine(num, this.register.double_1.GetHashCode());
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00006139 File Offset: 0x00004339
		public override string ToString()
		{
			return this.ToString("G", CultureInfo.CurrentCulture);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000614B File Offset: 0x0000434B
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000615C File Offset: 0x0000435C
		public string ToString(string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			stringBuilder.Append('<');
			for (int i = 0; i < Vector<T>.Count - 1; i++)
			{
				stringBuilder.Append(((IFormattable)((object)this[i])).ToString(format, formatProvider));
				stringBuilder.Append(numberGroupSeparator);
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(((IFormattable)((object)this[Vector<T>.Count - 1])).ToString(format, formatProvider));
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000061FC File Offset: 0x000043FC
		public unsafe static Vector<T>operator +(Vector<T> left, Vector<T> right)
		{
			if (!Vector.IsHardwareAccelerated)
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = left.register.byte_0 + right.register.byte_0;
					vector.register.byte_1 = left.register.byte_1 + right.register.byte_1;
					vector.register.byte_2 = left.register.byte_2 + right.register.byte_2;
					vector.register.byte_3 = left.register.byte_3 + right.register.byte_3;
					vector.register.byte_4 = left.register.byte_4 + right.register.byte_4;
					vector.register.byte_5 = left.register.byte_5 + right.register.byte_5;
					vector.register.byte_6 = left.register.byte_6 + right.register.byte_6;
					vector.register.byte_7 = left.register.byte_7 + right.register.byte_7;
					vector.register.byte_8 = left.register.byte_8 + right.register.byte_8;
					vector.register.byte_9 = left.register.byte_9 + right.register.byte_9;
					vector.register.byte_10 = left.register.byte_10 + right.register.byte_10;
					vector.register.byte_11 = left.register.byte_11 + right.register.byte_11;
					vector.register.byte_12 = left.register.byte_12 + right.register.byte_12;
					vector.register.byte_13 = left.register.byte_13 + right.register.byte_13;
					vector.register.byte_14 = left.register.byte_14 + right.register.byte_14;
					vector.register.byte_15 = left.register.byte_15 + right.register.byte_15;
				}
				else if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = left.register.sbyte_0 + right.register.sbyte_0;
					vector.register.sbyte_1 = left.register.sbyte_1 + right.register.sbyte_1;
					vector.register.sbyte_2 = left.register.sbyte_2 + right.register.sbyte_2;
					vector.register.sbyte_3 = left.register.sbyte_3 + right.register.sbyte_3;
					vector.register.sbyte_4 = left.register.sbyte_4 + right.register.sbyte_4;
					vector.register.sbyte_5 = left.register.sbyte_5 + right.register.sbyte_5;
					vector.register.sbyte_6 = left.register.sbyte_6 + right.register.sbyte_6;
					vector.register.sbyte_7 = left.register.sbyte_7 + right.register.sbyte_7;
					vector.register.sbyte_8 = left.register.sbyte_8 + right.register.sbyte_8;
					vector.register.sbyte_9 = left.register.sbyte_9 + right.register.sbyte_9;
					vector.register.sbyte_10 = left.register.sbyte_10 + right.register.sbyte_10;
					vector.register.sbyte_11 = left.register.sbyte_11 + right.register.sbyte_11;
					vector.register.sbyte_12 = left.register.sbyte_12 + right.register.sbyte_12;
					vector.register.sbyte_13 = left.register.sbyte_13 + right.register.sbyte_13;
					vector.register.sbyte_14 = left.register.sbyte_14 + right.register.sbyte_14;
					vector.register.sbyte_15 = left.register.sbyte_15 + right.register.sbyte_15;
				}
				else if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = left.register.uint16_0 + right.register.uint16_0;
					vector.register.uint16_1 = left.register.uint16_1 + right.register.uint16_1;
					vector.register.uint16_2 = left.register.uint16_2 + right.register.uint16_2;
					vector.register.uint16_3 = left.register.uint16_3 + right.register.uint16_3;
					vector.register.uint16_4 = left.register.uint16_4 + right.register.uint16_4;
					vector.register.uint16_5 = left.register.uint16_5 + right.register.uint16_5;
					vector.register.uint16_6 = left.register.uint16_6 + right.register.uint16_6;
					vector.register.uint16_7 = left.register.uint16_7 + right.register.uint16_7;
				}
				else if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = left.register.int16_0 + right.register.int16_0;
					vector.register.int16_1 = left.register.int16_1 + right.register.int16_1;
					vector.register.int16_2 = left.register.int16_2 + right.register.int16_2;
					vector.register.int16_3 = left.register.int16_3 + right.register.int16_3;
					vector.register.int16_4 = left.register.int16_4 + right.register.int16_4;
					vector.register.int16_5 = left.register.int16_5 + right.register.int16_5;
					vector.register.int16_6 = left.register.int16_6 + right.register.int16_6;
					vector.register.int16_7 = left.register.int16_7 + right.register.int16_7;
				}
				else if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = left.register.uint32_0 + right.register.uint32_0;
					vector.register.uint32_1 = left.register.uint32_1 + right.register.uint32_1;
					vector.register.uint32_2 = left.register.uint32_2 + right.register.uint32_2;
					vector.register.uint32_3 = left.register.uint32_3 + right.register.uint32_3;
				}
				else if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = left.register.int32_0 + right.register.int32_0;
					vector.register.int32_1 = left.register.int32_1 + right.register.int32_1;
					vector.register.int32_2 = left.register.int32_2 + right.register.int32_2;
					vector.register.int32_3 = left.register.int32_3 + right.register.int32_3;
				}
				else if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = left.register.uint64_0 + right.register.uint64_0;
					vector.register.uint64_1 = left.register.uint64_1 + right.register.uint64_1;
				}
				else if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = left.register.int64_0 + right.register.int64_0;
					vector.register.int64_1 = left.register.int64_1 + right.register.int64_1;
				}
				else if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = left.register.single_0 + right.register.single_0;
					vector.register.single_1 = left.register.single_1 + right.register.single_1;
					vector.register.single_2 = left.register.single_2 + right.register.single_2;
					vector.register.single_3 = left.register.single_3 + right.register.single_3;
				}
				else if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = left.register.double_0 + right.register.double_0;
					vector.register.double_1 = left.register.double_1 + right.register.double_1;
				}
				return vector;
			}
			if (typeof(T) == typeof(byte))
			{
				byte* ptr;
				checked
				{
					ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					ptr[i] = (byte)((object)Vector<T>.ScalarAdd(left[i], right[i]));
				}
				return new Vector<T>((void*)ptr);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte* ptr2;
				checked
				{
					ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int j = 0; j < Vector<T>.Count; j++)
				{
					ptr2[j] = (sbyte)((object)Vector<T>.ScalarAdd(left[j], right[j]));
				}
				return new Vector<T>((void*)ptr2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort* ptr3;
				checked
				{
					ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int k = 0; k < Vector<T>.Count; k++)
				{
					ptr3[k] = (ushort)((object)Vector<T>.ScalarAdd(left[k], right[k]));
				}
				return new Vector<T>((void*)ptr3);
			}
			if (typeof(T) == typeof(short))
			{
				short* ptr4;
				checked
				{
					ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int l = 0; l < Vector<T>.Count; l++)
				{
					ptr4[l] = (short)((object)Vector<T>.ScalarAdd(left[l], right[l]));
				}
				return new Vector<T>((void*)ptr4);
			}
			if (typeof(T) == typeof(uint))
			{
				uint* ptr5;
				checked
				{
					ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int m = 0; m < Vector<T>.Count; m++)
				{
					ptr5[m] = (uint)((object)Vector<T>.ScalarAdd(left[m], right[m]));
				}
				return new Vector<T>((void*)ptr5);
			}
			if (typeof(T) == typeof(int))
			{
				int* ptr6;
				checked
				{
					ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int n = 0; n < Vector<T>.Count; n++)
				{
					ptr6[n] = (int)((object)Vector<T>.ScalarAdd(left[n], right[n]));
				}
				return new Vector<T>((void*)ptr6);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong* ptr7;
				checked
				{
					ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num = 0; num < Vector<T>.Count; num++)
				{
					ptr7[num] = (ulong)((object)Vector<T>.ScalarAdd(left[num], right[num]));
				}
				return new Vector<T>((void*)ptr7);
			}
			if (typeof(T) == typeof(long))
			{
				long* ptr8;
				checked
				{
					ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num2 = 0; num2 < Vector<T>.Count; num2++)
				{
					ptr8[num2] = (long)((object)Vector<T>.ScalarAdd(left[num2], right[num2]));
				}
				return new Vector<T>((void*)ptr8);
			}
			if (typeof(T) == typeof(float))
			{
				float* ptr9;
				checked
				{
					ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int num3 = 0; num3 < Vector<T>.Count; num3++)
				{
					ptr9[num3] = (float)((object)Vector<T>.ScalarAdd(left[num3], right[num3]));
				}
				return new Vector<T>((void*)ptr9);
			}
			if (typeof(T) == typeof(double))
			{
				double* ptr10;
				checked
				{
					ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num4 = 0; num4 < Vector<T>.Count; num4++)
				{
					ptr10[num4] = (double)((object)Vector<T>.ScalarAdd(left[num4], right[num4]));
				}
				return new Vector<T>((void*)ptr10);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000070DC File Offset: 0x000052DC
		public unsafe static Vector<T>operator -(Vector<T> left, Vector<T> right)
		{
			if (!Vector.IsHardwareAccelerated)
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = left.register.byte_0 - right.register.byte_0;
					vector.register.byte_1 = left.register.byte_1 - right.register.byte_1;
					vector.register.byte_2 = left.register.byte_2 - right.register.byte_2;
					vector.register.byte_3 = left.register.byte_3 - right.register.byte_3;
					vector.register.byte_4 = left.register.byte_4 - right.register.byte_4;
					vector.register.byte_5 = left.register.byte_5 - right.register.byte_5;
					vector.register.byte_6 = left.register.byte_6 - right.register.byte_6;
					vector.register.byte_7 = left.register.byte_7 - right.register.byte_7;
					vector.register.byte_8 = left.register.byte_8 - right.register.byte_8;
					vector.register.byte_9 = left.register.byte_9 - right.register.byte_9;
					vector.register.byte_10 = left.register.byte_10 - right.register.byte_10;
					vector.register.byte_11 = left.register.byte_11 - right.register.byte_11;
					vector.register.byte_12 = left.register.byte_12 - right.register.byte_12;
					vector.register.byte_13 = left.register.byte_13 - right.register.byte_13;
					vector.register.byte_14 = left.register.byte_14 - right.register.byte_14;
					vector.register.byte_15 = left.register.byte_15 - right.register.byte_15;
				}
				else if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = left.register.sbyte_0 - right.register.sbyte_0;
					vector.register.sbyte_1 = left.register.sbyte_1 - right.register.sbyte_1;
					vector.register.sbyte_2 = left.register.sbyte_2 - right.register.sbyte_2;
					vector.register.sbyte_3 = left.register.sbyte_3 - right.register.sbyte_3;
					vector.register.sbyte_4 = left.register.sbyte_4 - right.register.sbyte_4;
					vector.register.sbyte_5 = left.register.sbyte_5 - right.register.sbyte_5;
					vector.register.sbyte_6 = left.register.sbyte_6 - right.register.sbyte_6;
					vector.register.sbyte_7 = left.register.sbyte_7 - right.register.sbyte_7;
					vector.register.sbyte_8 = left.register.sbyte_8 - right.register.sbyte_8;
					vector.register.sbyte_9 = left.register.sbyte_9 - right.register.sbyte_9;
					vector.register.sbyte_10 = left.register.sbyte_10 - right.register.sbyte_10;
					vector.register.sbyte_11 = left.register.sbyte_11 - right.register.sbyte_11;
					vector.register.sbyte_12 = left.register.sbyte_12 - right.register.sbyte_12;
					vector.register.sbyte_13 = left.register.sbyte_13 - right.register.sbyte_13;
					vector.register.sbyte_14 = left.register.sbyte_14 - right.register.sbyte_14;
					vector.register.sbyte_15 = left.register.sbyte_15 - right.register.sbyte_15;
				}
				else if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = left.register.uint16_0 - right.register.uint16_0;
					vector.register.uint16_1 = left.register.uint16_1 - right.register.uint16_1;
					vector.register.uint16_2 = left.register.uint16_2 - right.register.uint16_2;
					vector.register.uint16_3 = left.register.uint16_3 - right.register.uint16_3;
					vector.register.uint16_4 = left.register.uint16_4 - right.register.uint16_4;
					vector.register.uint16_5 = left.register.uint16_5 - right.register.uint16_5;
					vector.register.uint16_6 = left.register.uint16_6 - right.register.uint16_6;
					vector.register.uint16_7 = left.register.uint16_7 - right.register.uint16_7;
				}
				else if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = left.register.int16_0 - right.register.int16_0;
					vector.register.int16_1 = left.register.int16_1 - right.register.int16_1;
					vector.register.int16_2 = left.register.int16_2 - right.register.int16_2;
					vector.register.int16_3 = left.register.int16_3 - right.register.int16_3;
					vector.register.int16_4 = left.register.int16_4 - right.register.int16_4;
					vector.register.int16_5 = left.register.int16_5 - right.register.int16_5;
					vector.register.int16_6 = left.register.int16_6 - right.register.int16_6;
					vector.register.int16_7 = left.register.int16_7 - right.register.int16_7;
				}
				else if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = left.register.uint32_0 - right.register.uint32_0;
					vector.register.uint32_1 = left.register.uint32_1 - right.register.uint32_1;
					vector.register.uint32_2 = left.register.uint32_2 - right.register.uint32_2;
					vector.register.uint32_3 = left.register.uint32_3 - right.register.uint32_3;
				}
				else if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = left.register.int32_0 - right.register.int32_0;
					vector.register.int32_1 = left.register.int32_1 - right.register.int32_1;
					vector.register.int32_2 = left.register.int32_2 - right.register.int32_2;
					vector.register.int32_3 = left.register.int32_3 - right.register.int32_3;
				}
				else if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = left.register.uint64_0 - right.register.uint64_0;
					vector.register.uint64_1 = left.register.uint64_1 - right.register.uint64_1;
				}
				else if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = left.register.int64_0 - right.register.int64_0;
					vector.register.int64_1 = left.register.int64_1 - right.register.int64_1;
				}
				else if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = left.register.single_0 - right.register.single_0;
					vector.register.single_1 = left.register.single_1 - right.register.single_1;
					vector.register.single_2 = left.register.single_2 - right.register.single_2;
					vector.register.single_3 = left.register.single_3 - right.register.single_3;
				}
				else if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = left.register.double_0 - right.register.double_0;
					vector.register.double_1 = left.register.double_1 - right.register.double_1;
				}
				return vector;
			}
			if (typeof(T) == typeof(byte))
			{
				byte* ptr;
				checked
				{
					ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					ptr[i] = (byte)((object)Vector<T>.ScalarSubtract(left[i], right[i]));
				}
				return new Vector<T>((void*)ptr);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte* ptr2;
				checked
				{
					ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int j = 0; j < Vector<T>.Count; j++)
				{
					ptr2[j] = (sbyte)((object)Vector<T>.ScalarSubtract(left[j], right[j]));
				}
				return new Vector<T>((void*)ptr2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort* ptr3;
				checked
				{
					ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int k = 0; k < Vector<T>.Count; k++)
				{
					ptr3[k] = (ushort)((object)Vector<T>.ScalarSubtract(left[k], right[k]));
				}
				return new Vector<T>((void*)ptr3);
			}
			if (typeof(T) == typeof(short))
			{
				short* ptr4;
				checked
				{
					ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int l = 0; l < Vector<T>.Count; l++)
				{
					ptr4[l] = (short)((object)Vector<T>.ScalarSubtract(left[l], right[l]));
				}
				return new Vector<T>((void*)ptr4);
			}
			if (typeof(T) == typeof(uint))
			{
				uint* ptr5;
				checked
				{
					ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int m = 0; m < Vector<T>.Count; m++)
				{
					ptr5[m] = (uint)((object)Vector<T>.ScalarSubtract(left[m], right[m]));
				}
				return new Vector<T>((void*)ptr5);
			}
			if (typeof(T) == typeof(int))
			{
				int* ptr6;
				checked
				{
					ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int n = 0; n < Vector<T>.Count; n++)
				{
					ptr6[n] = (int)((object)Vector<T>.ScalarSubtract(left[n], right[n]));
				}
				return new Vector<T>((void*)ptr6);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong* ptr7;
				checked
				{
					ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num = 0; num < Vector<T>.Count; num++)
				{
					ptr7[num] = (ulong)((object)Vector<T>.ScalarSubtract(left[num], right[num]));
				}
				return new Vector<T>((void*)ptr7);
			}
			if (typeof(T) == typeof(long))
			{
				long* ptr8;
				checked
				{
					ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num2 = 0; num2 < Vector<T>.Count; num2++)
				{
					ptr8[num2] = (long)((object)Vector<T>.ScalarSubtract(left[num2], right[num2]));
				}
				return new Vector<T>((void*)ptr8);
			}
			if (typeof(T) == typeof(float))
			{
				float* ptr9;
				checked
				{
					ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int num3 = 0; num3 < Vector<T>.Count; num3++)
				{
					ptr9[num3] = (float)((object)Vector<T>.ScalarSubtract(left[num3], right[num3]));
				}
				return new Vector<T>((void*)ptr9);
			}
			if (typeof(T) == typeof(double))
			{
				double* ptr10;
				checked
				{
					ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num4 = 0; num4 < Vector<T>.Count; num4++)
				{
					ptr10[num4] = (double)((object)Vector<T>.ScalarSubtract(left[num4], right[num4]));
				}
				return new Vector<T>((void*)ptr10);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00007FBC File Offset: 0x000061BC
		public unsafe static Vector<T>operator *(Vector<T> left, Vector<T> right)
		{
			if (!Vector.IsHardwareAccelerated)
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = left.register.byte_0 * right.register.byte_0;
					vector.register.byte_1 = left.register.byte_1 * right.register.byte_1;
					vector.register.byte_2 = left.register.byte_2 * right.register.byte_2;
					vector.register.byte_3 = left.register.byte_3 * right.register.byte_3;
					vector.register.byte_4 = left.register.byte_4 * right.register.byte_4;
					vector.register.byte_5 = left.register.byte_5 * right.register.byte_5;
					vector.register.byte_6 = left.register.byte_6 * right.register.byte_6;
					vector.register.byte_7 = left.register.byte_7 * right.register.byte_7;
					vector.register.byte_8 = left.register.byte_8 * right.register.byte_8;
					vector.register.byte_9 = left.register.byte_9 * right.register.byte_9;
					vector.register.byte_10 = left.register.byte_10 * right.register.byte_10;
					vector.register.byte_11 = left.register.byte_11 * right.register.byte_11;
					vector.register.byte_12 = left.register.byte_12 * right.register.byte_12;
					vector.register.byte_13 = left.register.byte_13 * right.register.byte_13;
					vector.register.byte_14 = left.register.byte_14 * right.register.byte_14;
					vector.register.byte_15 = left.register.byte_15 * right.register.byte_15;
				}
				else if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = left.register.sbyte_0 * right.register.sbyte_0;
					vector.register.sbyte_1 = left.register.sbyte_1 * right.register.sbyte_1;
					vector.register.sbyte_2 = left.register.sbyte_2 * right.register.sbyte_2;
					vector.register.sbyte_3 = left.register.sbyte_3 * right.register.sbyte_3;
					vector.register.sbyte_4 = left.register.sbyte_4 * right.register.sbyte_4;
					vector.register.sbyte_5 = left.register.sbyte_5 * right.register.sbyte_5;
					vector.register.sbyte_6 = left.register.sbyte_6 * right.register.sbyte_6;
					vector.register.sbyte_7 = left.register.sbyte_7 * right.register.sbyte_7;
					vector.register.sbyte_8 = left.register.sbyte_8 * right.register.sbyte_8;
					vector.register.sbyte_9 = left.register.sbyte_9 * right.register.sbyte_9;
					vector.register.sbyte_10 = left.register.sbyte_10 * right.register.sbyte_10;
					vector.register.sbyte_11 = left.register.sbyte_11 * right.register.sbyte_11;
					vector.register.sbyte_12 = left.register.sbyte_12 * right.register.sbyte_12;
					vector.register.sbyte_13 = left.register.sbyte_13 * right.register.sbyte_13;
					vector.register.sbyte_14 = left.register.sbyte_14 * right.register.sbyte_14;
					vector.register.sbyte_15 = left.register.sbyte_15 * right.register.sbyte_15;
				}
				else if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = left.register.uint16_0 * right.register.uint16_0;
					vector.register.uint16_1 = left.register.uint16_1 * right.register.uint16_1;
					vector.register.uint16_2 = left.register.uint16_2 * right.register.uint16_2;
					vector.register.uint16_3 = left.register.uint16_3 * right.register.uint16_3;
					vector.register.uint16_4 = left.register.uint16_4 * right.register.uint16_4;
					vector.register.uint16_5 = left.register.uint16_5 * right.register.uint16_5;
					vector.register.uint16_6 = left.register.uint16_6 * right.register.uint16_6;
					vector.register.uint16_7 = left.register.uint16_7 * right.register.uint16_7;
				}
				else if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = left.register.int16_0 * right.register.int16_0;
					vector.register.int16_1 = left.register.int16_1 * right.register.int16_1;
					vector.register.int16_2 = left.register.int16_2 * right.register.int16_2;
					vector.register.int16_3 = left.register.int16_3 * right.register.int16_3;
					vector.register.int16_4 = left.register.int16_4 * right.register.int16_4;
					vector.register.int16_5 = left.register.int16_5 * right.register.int16_5;
					vector.register.int16_6 = left.register.int16_6 * right.register.int16_6;
					vector.register.int16_7 = left.register.int16_7 * right.register.int16_7;
				}
				else if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = left.register.uint32_0 * right.register.uint32_0;
					vector.register.uint32_1 = left.register.uint32_1 * right.register.uint32_1;
					vector.register.uint32_2 = left.register.uint32_2 * right.register.uint32_2;
					vector.register.uint32_3 = left.register.uint32_3 * right.register.uint32_3;
				}
				else if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = left.register.int32_0 * right.register.int32_0;
					vector.register.int32_1 = left.register.int32_1 * right.register.int32_1;
					vector.register.int32_2 = left.register.int32_2 * right.register.int32_2;
					vector.register.int32_3 = left.register.int32_3 * right.register.int32_3;
				}
				else if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = left.register.uint64_0 * right.register.uint64_0;
					vector.register.uint64_1 = left.register.uint64_1 * right.register.uint64_1;
				}
				else if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = left.register.int64_0 * right.register.int64_0;
					vector.register.int64_1 = left.register.int64_1 * right.register.int64_1;
				}
				else if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = left.register.single_0 * right.register.single_0;
					vector.register.single_1 = left.register.single_1 * right.register.single_1;
					vector.register.single_2 = left.register.single_2 * right.register.single_2;
					vector.register.single_3 = left.register.single_3 * right.register.single_3;
				}
				else if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = left.register.double_0 * right.register.double_0;
					vector.register.double_1 = left.register.double_1 * right.register.double_1;
				}
				return vector;
			}
			if (typeof(T) == typeof(byte))
			{
				byte* ptr;
				checked
				{
					ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					ptr[i] = (byte)((object)Vector<T>.ScalarMultiply(left[i], right[i]));
				}
				return new Vector<T>((void*)ptr);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte* ptr2;
				checked
				{
					ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int j = 0; j < Vector<T>.Count; j++)
				{
					ptr2[j] = (sbyte)((object)Vector<T>.ScalarMultiply(left[j], right[j]));
				}
				return new Vector<T>((void*)ptr2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort* ptr3;
				checked
				{
					ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int k = 0; k < Vector<T>.Count; k++)
				{
					ptr3[k] = (ushort)((object)Vector<T>.ScalarMultiply(left[k], right[k]));
				}
				return new Vector<T>((void*)ptr3);
			}
			if (typeof(T) == typeof(short))
			{
				short* ptr4;
				checked
				{
					ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int l = 0; l < Vector<T>.Count; l++)
				{
					ptr4[l] = (short)((object)Vector<T>.ScalarMultiply(left[l], right[l]));
				}
				return new Vector<T>((void*)ptr4);
			}
			if (typeof(T) == typeof(uint))
			{
				uint* ptr5;
				checked
				{
					ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int m = 0; m < Vector<T>.Count; m++)
				{
					ptr5[m] = (uint)((object)Vector<T>.ScalarMultiply(left[m], right[m]));
				}
				return new Vector<T>((void*)ptr5);
			}
			if (typeof(T) == typeof(int))
			{
				int* ptr6;
				checked
				{
					ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int n = 0; n < Vector<T>.Count; n++)
				{
					ptr6[n] = (int)((object)Vector<T>.ScalarMultiply(left[n], right[n]));
				}
				return new Vector<T>((void*)ptr6);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong* ptr7;
				checked
				{
					ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num = 0; num < Vector<T>.Count; num++)
				{
					ptr7[num] = (ulong)((object)Vector<T>.ScalarMultiply(left[num], right[num]));
				}
				return new Vector<T>((void*)ptr7);
			}
			if (typeof(T) == typeof(long))
			{
				long* ptr8;
				checked
				{
					ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num2 = 0; num2 < Vector<T>.Count; num2++)
				{
					ptr8[num2] = (long)((object)Vector<T>.ScalarMultiply(left[num2], right[num2]));
				}
				return new Vector<T>((void*)ptr8);
			}
			if (typeof(T) == typeof(float))
			{
				float* ptr9;
				checked
				{
					ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int num3 = 0; num3 < Vector<T>.Count; num3++)
				{
					ptr9[num3] = (float)((object)Vector<T>.ScalarMultiply(left[num3], right[num3]));
				}
				return new Vector<T>((void*)ptr9);
			}
			if (typeof(T) == typeof(double))
			{
				double* ptr10;
				checked
				{
					ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num4 = 0; num4 < Vector<T>.Count; num4++)
				{
					ptr10[num4] = (double)((object)Vector<T>.ScalarMultiply(left[num4], right[num4]));
				}
				return new Vector<T>((void*)ptr10);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00008E9C File Offset: 0x0000709C
		public static Vector<T>operator *(Vector<T> value, T factor)
		{
			if (Vector.IsHardwareAccelerated)
			{
				return new Vector<T>(factor) * value;
			}
			Vector<T> vector = default(Vector<T>);
			if (typeof(T) == typeof(byte))
			{
				vector.register.byte_0 = value.register.byte_0 * (byte)((object)factor);
				vector.register.byte_1 = value.register.byte_1 * (byte)((object)factor);
				vector.register.byte_2 = value.register.byte_2 * (byte)((object)factor);
				vector.register.byte_3 = value.register.byte_3 * (byte)((object)factor);
				vector.register.byte_4 = value.register.byte_4 * (byte)((object)factor);
				vector.register.byte_5 = value.register.byte_5 * (byte)((object)factor);
				vector.register.byte_6 = value.register.byte_6 * (byte)((object)factor);
				vector.register.byte_7 = value.register.byte_7 * (byte)((object)factor);
				vector.register.byte_8 = value.register.byte_8 * (byte)((object)factor);
				vector.register.byte_9 = value.register.byte_9 * (byte)((object)factor);
				vector.register.byte_10 = value.register.byte_10 * (byte)((object)factor);
				vector.register.byte_11 = value.register.byte_11 * (byte)((object)factor);
				vector.register.byte_12 = value.register.byte_12 * (byte)((object)factor);
				vector.register.byte_13 = value.register.byte_13 * (byte)((object)factor);
				vector.register.byte_14 = value.register.byte_14 * (byte)((object)factor);
				vector.register.byte_15 = value.register.byte_15 * (byte)((object)factor);
			}
			else if (typeof(T) == typeof(sbyte))
			{
				vector.register.sbyte_0 = value.register.sbyte_0 * (sbyte)((object)factor);
				vector.register.sbyte_1 = value.register.sbyte_1 * (sbyte)((object)factor);
				vector.register.sbyte_2 = value.register.sbyte_2 * (sbyte)((object)factor);
				vector.register.sbyte_3 = value.register.sbyte_3 * (sbyte)((object)factor);
				vector.register.sbyte_4 = value.register.sbyte_4 * (sbyte)((object)factor);
				vector.register.sbyte_5 = value.register.sbyte_5 * (sbyte)((object)factor);
				vector.register.sbyte_6 = value.register.sbyte_6 * (sbyte)((object)factor);
				vector.register.sbyte_7 = value.register.sbyte_7 * (sbyte)((object)factor);
				vector.register.sbyte_8 = value.register.sbyte_8 * (sbyte)((object)factor);
				vector.register.sbyte_9 = value.register.sbyte_9 * (sbyte)((object)factor);
				vector.register.sbyte_10 = value.register.sbyte_10 * (sbyte)((object)factor);
				vector.register.sbyte_11 = value.register.sbyte_11 * (sbyte)((object)factor);
				vector.register.sbyte_12 = value.register.sbyte_12 * (sbyte)((object)factor);
				vector.register.sbyte_13 = value.register.sbyte_13 * (sbyte)((object)factor);
				vector.register.sbyte_14 = value.register.sbyte_14 * (sbyte)((object)factor);
				vector.register.sbyte_15 = value.register.sbyte_15 * (sbyte)((object)factor);
			}
			else if (typeof(T) == typeof(ushort))
			{
				vector.register.uint16_0 = value.register.uint16_0 * (ushort)((object)factor);
				vector.register.uint16_1 = value.register.uint16_1 * (ushort)((object)factor);
				vector.register.uint16_2 = value.register.uint16_2 * (ushort)((object)factor);
				vector.register.uint16_3 = value.register.uint16_3 * (ushort)((object)factor);
				vector.register.uint16_4 = value.register.uint16_4 * (ushort)((object)factor);
				vector.register.uint16_5 = value.register.uint16_5 * (ushort)((object)factor);
				vector.register.uint16_6 = value.register.uint16_6 * (ushort)((object)factor);
				vector.register.uint16_7 = value.register.uint16_7 * (ushort)((object)factor);
			}
			else if (typeof(T) == typeof(short))
			{
				vector.register.int16_0 = value.register.int16_0 * (short)((object)factor);
				vector.register.int16_1 = value.register.int16_1 * (short)((object)factor);
				vector.register.int16_2 = value.register.int16_2 * (short)((object)factor);
				vector.register.int16_3 = value.register.int16_3 * (short)((object)factor);
				vector.register.int16_4 = value.register.int16_4 * (short)((object)factor);
				vector.register.int16_5 = value.register.int16_5 * (short)((object)factor);
				vector.register.int16_6 = value.register.int16_6 * (short)((object)factor);
				vector.register.int16_7 = value.register.int16_7 * (short)((object)factor);
			}
			else if (typeof(T) == typeof(uint))
			{
				vector.register.uint32_0 = value.register.uint32_0 * (uint)((object)factor);
				vector.register.uint32_1 = value.register.uint32_1 * (uint)((object)factor);
				vector.register.uint32_2 = value.register.uint32_2 * (uint)((object)factor);
				vector.register.uint32_3 = value.register.uint32_3 * (uint)((object)factor);
			}
			else if (typeof(T) == typeof(int))
			{
				vector.register.int32_0 = value.register.int32_0 * (int)((object)factor);
				vector.register.int32_1 = value.register.int32_1 * (int)((object)factor);
				vector.register.int32_2 = value.register.int32_2 * (int)((object)factor);
				vector.register.int32_3 = value.register.int32_3 * (int)((object)factor);
			}
			else if (typeof(T) == typeof(ulong))
			{
				vector.register.uint64_0 = value.register.uint64_0 * (ulong)((object)factor);
				vector.register.uint64_1 = value.register.uint64_1 * (ulong)((object)factor);
			}
			else if (typeof(T) == typeof(long))
			{
				vector.register.int64_0 = value.register.int64_0 * (long)((object)factor);
				vector.register.int64_1 = value.register.int64_1 * (long)((object)factor);
			}
			else if (typeof(T) == typeof(float))
			{
				vector.register.single_0 = value.register.single_0 * (float)((object)factor);
				vector.register.single_1 = value.register.single_1 * (float)((object)factor);
				vector.register.single_2 = value.register.single_2 * (float)((object)factor);
				vector.register.single_3 = value.register.single_3 * (float)((object)factor);
			}
			else if (typeof(T) == typeof(double))
			{
				vector.register.double_0 = value.register.double_0 * (double)((object)factor);
				vector.register.double_1 = value.register.double_1 * (double)((object)factor);
			}
			return vector;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00009950 File Offset: 0x00007B50
		public static Vector<T>operator *(T factor, Vector<T> value)
		{
			if (Vector.IsHardwareAccelerated)
			{
				return new Vector<T>(factor) * value;
			}
			Vector<T> vector = default(Vector<T>);
			if (typeof(T) == typeof(byte))
			{
				vector.register.byte_0 = value.register.byte_0 * (byte)((object)factor);
				vector.register.byte_1 = value.register.byte_1 * (byte)((object)factor);
				vector.register.byte_2 = value.register.byte_2 * (byte)((object)factor);
				vector.register.byte_3 = value.register.byte_3 * (byte)((object)factor);
				vector.register.byte_4 = value.register.byte_4 * (byte)((object)factor);
				vector.register.byte_5 = value.register.byte_5 * (byte)((object)factor);
				vector.register.byte_6 = value.register.byte_6 * (byte)((object)factor);
				vector.register.byte_7 = value.register.byte_7 * (byte)((object)factor);
				vector.register.byte_8 = value.register.byte_8 * (byte)((object)factor);
				vector.register.byte_9 = value.register.byte_9 * (byte)((object)factor);
				vector.register.byte_10 = value.register.byte_10 * (byte)((object)factor);
				vector.register.byte_11 = value.register.byte_11 * (byte)((object)factor);
				vector.register.byte_12 = value.register.byte_12 * (byte)((object)factor);
				vector.register.byte_13 = value.register.byte_13 * (byte)((object)factor);
				vector.register.byte_14 = value.register.byte_14 * (byte)((object)factor);
				vector.register.byte_15 = value.register.byte_15 * (byte)((object)factor);
			}
			else if (typeof(T) == typeof(sbyte))
			{
				vector.register.sbyte_0 = value.register.sbyte_0 * (sbyte)((object)factor);
				vector.register.sbyte_1 = value.register.sbyte_1 * (sbyte)((object)factor);
				vector.register.sbyte_2 = value.register.sbyte_2 * (sbyte)((object)factor);
				vector.register.sbyte_3 = value.register.sbyte_3 * (sbyte)((object)factor);
				vector.register.sbyte_4 = value.register.sbyte_4 * (sbyte)((object)factor);
				vector.register.sbyte_5 = value.register.sbyte_5 * (sbyte)((object)factor);
				vector.register.sbyte_6 = value.register.sbyte_6 * (sbyte)((object)factor);
				vector.register.sbyte_7 = value.register.sbyte_7 * (sbyte)((object)factor);
				vector.register.sbyte_8 = value.register.sbyte_8 * (sbyte)((object)factor);
				vector.register.sbyte_9 = value.register.sbyte_9 * (sbyte)((object)factor);
				vector.register.sbyte_10 = value.register.sbyte_10 * (sbyte)((object)factor);
				vector.register.sbyte_11 = value.register.sbyte_11 * (sbyte)((object)factor);
				vector.register.sbyte_12 = value.register.sbyte_12 * (sbyte)((object)factor);
				vector.register.sbyte_13 = value.register.sbyte_13 * (sbyte)((object)factor);
				vector.register.sbyte_14 = value.register.sbyte_14 * (sbyte)((object)factor);
				vector.register.sbyte_15 = value.register.sbyte_15 * (sbyte)((object)factor);
			}
			else if (typeof(T) == typeof(ushort))
			{
				vector.register.uint16_0 = value.register.uint16_0 * (ushort)((object)factor);
				vector.register.uint16_1 = value.register.uint16_1 * (ushort)((object)factor);
				vector.register.uint16_2 = value.register.uint16_2 * (ushort)((object)factor);
				vector.register.uint16_3 = value.register.uint16_3 * (ushort)((object)factor);
				vector.register.uint16_4 = value.register.uint16_4 * (ushort)((object)factor);
				vector.register.uint16_5 = value.register.uint16_5 * (ushort)((object)factor);
				vector.register.uint16_6 = value.register.uint16_6 * (ushort)((object)factor);
				vector.register.uint16_7 = value.register.uint16_7 * (ushort)((object)factor);
			}
			else if (typeof(T) == typeof(short))
			{
				vector.register.int16_0 = value.register.int16_0 * (short)((object)factor);
				vector.register.int16_1 = value.register.int16_1 * (short)((object)factor);
				vector.register.int16_2 = value.register.int16_2 * (short)((object)factor);
				vector.register.int16_3 = value.register.int16_3 * (short)((object)factor);
				vector.register.int16_4 = value.register.int16_4 * (short)((object)factor);
				vector.register.int16_5 = value.register.int16_5 * (short)((object)factor);
				vector.register.int16_6 = value.register.int16_6 * (short)((object)factor);
				vector.register.int16_7 = value.register.int16_7 * (short)((object)factor);
			}
			else if (typeof(T) == typeof(uint))
			{
				vector.register.uint32_0 = value.register.uint32_0 * (uint)((object)factor);
				vector.register.uint32_1 = value.register.uint32_1 * (uint)((object)factor);
				vector.register.uint32_2 = value.register.uint32_2 * (uint)((object)factor);
				vector.register.uint32_3 = value.register.uint32_3 * (uint)((object)factor);
			}
			else if (typeof(T) == typeof(int))
			{
				vector.register.int32_0 = value.register.int32_0 * (int)((object)factor);
				vector.register.int32_1 = value.register.int32_1 * (int)((object)factor);
				vector.register.int32_2 = value.register.int32_2 * (int)((object)factor);
				vector.register.int32_3 = value.register.int32_3 * (int)((object)factor);
			}
			else if (typeof(T) == typeof(ulong))
			{
				vector.register.uint64_0 = value.register.uint64_0 * (ulong)((object)factor);
				vector.register.uint64_1 = value.register.uint64_1 * (ulong)((object)factor);
			}
			else if (typeof(T) == typeof(long))
			{
				vector.register.int64_0 = value.register.int64_0 * (long)((object)factor);
				vector.register.int64_1 = value.register.int64_1 * (long)((object)factor);
			}
			else if (typeof(T) == typeof(float))
			{
				vector.register.single_0 = value.register.single_0 * (float)((object)factor);
				vector.register.single_1 = value.register.single_1 * (float)((object)factor);
				vector.register.single_2 = value.register.single_2 * (float)((object)factor);
				vector.register.single_3 = value.register.single_3 * (float)((object)factor);
			}
			else if (typeof(T) == typeof(double))
			{
				vector.register.double_0 = value.register.double_0 * (double)((object)factor);
				vector.register.double_1 = value.register.double_1 * (double)((object)factor);
			}
			return vector;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000A404 File Offset: 0x00008604
		public unsafe static Vector<T>operator /(Vector<T> left, Vector<T> right)
		{
			if (!Vector.IsHardwareAccelerated)
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = left.register.byte_0 / right.register.byte_0;
					vector.register.byte_1 = left.register.byte_1 / right.register.byte_1;
					vector.register.byte_2 = left.register.byte_2 / right.register.byte_2;
					vector.register.byte_3 = left.register.byte_3 / right.register.byte_3;
					vector.register.byte_4 = left.register.byte_4 / right.register.byte_4;
					vector.register.byte_5 = left.register.byte_5 / right.register.byte_5;
					vector.register.byte_6 = left.register.byte_6 / right.register.byte_6;
					vector.register.byte_7 = left.register.byte_7 / right.register.byte_7;
					vector.register.byte_8 = left.register.byte_8 / right.register.byte_8;
					vector.register.byte_9 = left.register.byte_9 / right.register.byte_9;
					vector.register.byte_10 = left.register.byte_10 / right.register.byte_10;
					vector.register.byte_11 = left.register.byte_11 / right.register.byte_11;
					vector.register.byte_12 = left.register.byte_12 / right.register.byte_12;
					vector.register.byte_13 = left.register.byte_13 / right.register.byte_13;
					vector.register.byte_14 = left.register.byte_14 / right.register.byte_14;
					vector.register.byte_15 = left.register.byte_15 / right.register.byte_15;
				}
				else if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = left.register.sbyte_0 / right.register.sbyte_0;
					vector.register.sbyte_1 = left.register.sbyte_1 / right.register.sbyte_1;
					vector.register.sbyte_2 = left.register.sbyte_2 / right.register.sbyte_2;
					vector.register.sbyte_3 = left.register.sbyte_3 / right.register.sbyte_3;
					vector.register.sbyte_4 = left.register.sbyte_4 / right.register.sbyte_4;
					vector.register.sbyte_5 = left.register.sbyte_5 / right.register.sbyte_5;
					vector.register.sbyte_6 = left.register.sbyte_6 / right.register.sbyte_6;
					vector.register.sbyte_7 = left.register.sbyte_7 / right.register.sbyte_7;
					vector.register.sbyte_8 = left.register.sbyte_8 / right.register.sbyte_8;
					vector.register.sbyte_9 = left.register.sbyte_9 / right.register.sbyte_9;
					vector.register.sbyte_10 = left.register.sbyte_10 / right.register.sbyte_10;
					vector.register.sbyte_11 = left.register.sbyte_11 / right.register.sbyte_11;
					vector.register.sbyte_12 = left.register.sbyte_12 / right.register.sbyte_12;
					vector.register.sbyte_13 = left.register.sbyte_13 / right.register.sbyte_13;
					vector.register.sbyte_14 = left.register.sbyte_14 / right.register.sbyte_14;
					vector.register.sbyte_15 = left.register.sbyte_15 / right.register.sbyte_15;
				}
				else if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = left.register.uint16_0 / right.register.uint16_0;
					vector.register.uint16_1 = left.register.uint16_1 / right.register.uint16_1;
					vector.register.uint16_2 = left.register.uint16_2 / right.register.uint16_2;
					vector.register.uint16_3 = left.register.uint16_3 / right.register.uint16_3;
					vector.register.uint16_4 = left.register.uint16_4 / right.register.uint16_4;
					vector.register.uint16_5 = left.register.uint16_5 / right.register.uint16_5;
					vector.register.uint16_6 = left.register.uint16_6 / right.register.uint16_6;
					vector.register.uint16_7 = left.register.uint16_7 / right.register.uint16_7;
				}
				else if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = left.register.int16_0 / right.register.int16_0;
					vector.register.int16_1 = left.register.int16_1 / right.register.int16_1;
					vector.register.int16_2 = left.register.int16_2 / right.register.int16_2;
					vector.register.int16_3 = left.register.int16_3 / right.register.int16_3;
					vector.register.int16_4 = left.register.int16_4 / right.register.int16_4;
					vector.register.int16_5 = left.register.int16_5 / right.register.int16_5;
					vector.register.int16_6 = left.register.int16_6 / right.register.int16_6;
					vector.register.int16_7 = left.register.int16_7 / right.register.int16_7;
				}
				else if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = left.register.uint32_0 / right.register.uint32_0;
					vector.register.uint32_1 = left.register.uint32_1 / right.register.uint32_1;
					vector.register.uint32_2 = left.register.uint32_2 / right.register.uint32_2;
					vector.register.uint32_3 = left.register.uint32_3 / right.register.uint32_3;
				}
				else if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = left.register.int32_0 / right.register.int32_0;
					vector.register.int32_1 = left.register.int32_1 / right.register.int32_1;
					vector.register.int32_2 = left.register.int32_2 / right.register.int32_2;
					vector.register.int32_3 = left.register.int32_3 / right.register.int32_3;
				}
				else if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = left.register.uint64_0 / right.register.uint64_0;
					vector.register.uint64_1 = left.register.uint64_1 / right.register.uint64_1;
				}
				else if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = left.register.int64_0 / right.register.int64_0;
					vector.register.int64_1 = left.register.int64_1 / right.register.int64_1;
				}
				else if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = left.register.single_0 / right.register.single_0;
					vector.register.single_1 = left.register.single_1 / right.register.single_1;
					vector.register.single_2 = left.register.single_2 / right.register.single_2;
					vector.register.single_3 = left.register.single_3 / right.register.single_3;
				}
				else if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = left.register.double_0 / right.register.double_0;
					vector.register.double_1 = left.register.double_1 / right.register.double_1;
				}
				return vector;
			}
			if (typeof(T) == typeof(byte))
			{
				byte* ptr;
				checked
				{
					ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					ptr[i] = (byte)((object)Vector<T>.ScalarDivide(left[i], right[i]));
				}
				return new Vector<T>((void*)ptr);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte* ptr2;
				checked
				{
					ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
				}
				for (int j = 0; j < Vector<T>.Count; j++)
				{
					ptr2[j] = (sbyte)((object)Vector<T>.ScalarDivide(left[j], right[j]));
				}
				return new Vector<T>((void*)ptr2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort* ptr3;
				checked
				{
					ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int k = 0; k < Vector<T>.Count; k++)
				{
					ptr3[k] = (ushort)((object)Vector<T>.ScalarDivide(left[k], right[k]));
				}
				return new Vector<T>((void*)ptr3);
			}
			if (typeof(T) == typeof(short))
			{
				short* ptr4;
				checked
				{
					ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
				}
				for (int l = 0; l < Vector<T>.Count; l++)
				{
					ptr4[l] = (short)((object)Vector<T>.ScalarDivide(left[l], right[l]));
				}
				return new Vector<T>((void*)ptr4);
			}
			if (typeof(T) == typeof(uint))
			{
				uint* ptr5;
				checked
				{
					ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int m = 0; m < Vector<T>.Count; m++)
				{
					ptr5[m] = (uint)((object)Vector<T>.ScalarDivide(left[m], right[m]));
				}
				return new Vector<T>((void*)ptr5);
			}
			if (typeof(T) == typeof(int))
			{
				int* ptr6;
				checked
				{
					ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int n = 0; n < Vector<T>.Count; n++)
				{
					ptr6[n] = (int)((object)Vector<T>.ScalarDivide(left[n], right[n]));
				}
				return new Vector<T>((void*)ptr6);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong* ptr7;
				checked
				{
					ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num = 0; num < Vector<T>.Count; num++)
				{
					ptr7[num] = (ulong)((object)Vector<T>.ScalarDivide(left[num], right[num]));
				}
				return new Vector<T>((void*)ptr7);
			}
			if (typeof(T) == typeof(long))
			{
				long* ptr8;
				checked
				{
					ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num2 = 0; num2 < Vector<T>.Count; num2++)
				{
					ptr8[num2] = (long)((object)Vector<T>.ScalarDivide(left[num2], right[num2]));
				}
				return new Vector<T>((void*)ptr8);
			}
			if (typeof(T) == typeof(float))
			{
				float* ptr9;
				checked
				{
					ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
				}
				for (int num3 = 0; num3 < Vector<T>.Count; num3++)
				{
					ptr9[num3] = (float)((object)Vector<T>.ScalarDivide(left[num3], right[num3]));
				}
				return new Vector<T>((void*)ptr9);
			}
			if (typeof(T) == typeof(double))
			{
				double* ptr10;
				checked
				{
					ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
				}
				for (int num4 = 0; num4 < Vector<T>.Count; num4++)
				{
					ptr10[num4] = (double)((object)Vector<T>.ScalarDivide(left[num4], right[num4]));
				}
				return new Vector<T>((void*)ptr10);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000B2E1 File Offset: 0x000094E1
		public static Vector<T>operator -(Vector<T> value)
		{
			return Vector<T>.Zero - value;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000B2F0 File Offset: 0x000094F0
		[JitIntrinsic]
		public unsafe static Vector<T>operator &(Vector<T> left, Vector<T> right)
		{
			Vector<T> vector = default(Vector<T>);
			if (Vector.IsHardwareAccelerated)
			{
				long* ptr = &vector.register.int64_0;
				long* ptr2 = &left.register.int64_0;
				long* ptr3 = &right.register.int64_0;
				for (int i = 0; i < Vector<long>.Count; i++)
				{
					ptr[i] = ptr2[i] & ptr3[i];
				}
			}
			else
			{
				vector.register.int64_0 = left.register.int64_0 & right.register.int64_0;
				vector.register.int64_1 = left.register.int64_1 & right.register.int64_1;
			}
			return vector;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000B3AC File Offset: 0x000095AC
		[JitIntrinsic]
		public unsafe static Vector<T>operator |(Vector<T> left, Vector<T> right)
		{
			Vector<T> vector = default(Vector<T>);
			if (Vector.IsHardwareAccelerated)
			{
				long* ptr = &vector.register.int64_0;
				long* ptr2 = &left.register.int64_0;
				long* ptr3 = &right.register.int64_0;
				for (int i = 0; i < Vector<long>.Count; i++)
				{
					ptr[i] = ptr2[i] | ptr3[i];
				}
			}
			else
			{
				vector.register.int64_0 = left.register.int64_0 | right.register.int64_0;
				vector.register.int64_1 = left.register.int64_1 | right.register.int64_1;
			}
			return vector;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000B468 File Offset: 0x00009668
		[JitIntrinsic]
		public unsafe static Vector<T>operator ^(Vector<T> left, Vector<T> right)
		{
			Vector<T> vector = default(Vector<T>);
			if (Vector.IsHardwareAccelerated)
			{
				long* ptr = &vector.register.int64_0;
				long* ptr2 = &left.register.int64_0;
				long* ptr3 = &right.register.int64_0;
				for (int i = 0; i < Vector<long>.Count; i++)
				{
					ptr[i] = ptr2[i] ^ ptr3[i];
				}
			}
			else
			{
				vector.register.int64_0 = left.register.int64_0 ^ right.register.int64_0;
				vector.register.int64_1 = left.register.int64_1 ^ right.register.int64_1;
			}
			return vector;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000B524 File Offset: 0x00009724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T>operator ~(Vector<T> value)
		{
			return Vector<T>.allOnes ^ value;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000B531 File Offset: 0x00009731
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector<T> left, Vector<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000B53B File Offset: 0x0000973B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector<T> left, Vector<T> right)
		{
			return !(left == right);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000B547 File Offset: 0x00009747
		[JitIntrinsic]
		public static explicit operator Vector<byte>(Vector<T> value)
		{
			return new Vector<byte>(ref value.register);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000B555 File Offset: 0x00009755
		[CLSCompliant(false)]
		[JitIntrinsic]
		public static explicit operator Vector<sbyte>(Vector<T> value)
		{
			return new Vector<sbyte>(ref value.register);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000B563 File Offset: 0x00009763
		[CLSCompliant(false)]
		[JitIntrinsic]
		public static explicit operator Vector<ushort>(Vector<T> value)
		{
			return new Vector<ushort>(ref value.register);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000B571 File Offset: 0x00009771
		[JitIntrinsic]
		public static explicit operator Vector<short>(Vector<T> value)
		{
			return new Vector<short>(ref value.register);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000B57F File Offset: 0x0000977F
		[CLSCompliant(false)]
		[JitIntrinsic]
		public static explicit operator Vector<uint>(Vector<T> value)
		{
			return new Vector<uint>(ref value.register);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000B58D File Offset: 0x0000978D
		[JitIntrinsic]
		public static explicit operator Vector<int>(Vector<T> value)
		{
			return new Vector<int>(ref value.register);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000B59B File Offset: 0x0000979B
		[CLSCompliant(false)]
		[JitIntrinsic]
		public static explicit operator Vector<ulong>(Vector<T> value)
		{
			return new Vector<ulong>(ref value.register);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000B5A9 File Offset: 0x000097A9
		[JitIntrinsic]
		public static explicit operator Vector<long>(Vector<T> value)
		{
			return new Vector<long>(ref value.register);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000B5B7 File Offset: 0x000097B7
		[JitIntrinsic]
		public static explicit operator Vector<float>(Vector<T> value)
		{
			return new Vector<float>(ref value.register);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000B5C5 File Offset: 0x000097C5
		[JitIntrinsic]
		public static explicit operator Vector<double>(Vector<T> value)
		{
			return new Vector<double>(ref value.register);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000B5D4 File Offset: 0x000097D4
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static Vector<T> Equals(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (Vector<T>.ScalarEquals(left[i], right[i]) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (Vector<T>.ScalarEquals(left[j], right[j]) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (Vector<T>.ScalarEquals(left[k], right[k]) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (Vector<T>.ScalarEquals(left[l], right[l]) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (Vector<T>.ScalarEquals(left[m], right[m]) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (Vector<T>.ScalarEquals(left[n], right[n]) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (Vector<T>.ScalarEquals(left[num], right[num]) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (Vector<T>.ScalarEquals(left[num2], right[num2]) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (Vector<T>.ScalarEquals(left[num3], right[num3]) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = (Vector<T>.ScalarEquals(left[num4], right[num4]) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				Register register = default(Register);
				if (typeof(T) == typeof(byte))
				{
					register.byte_0 = ((left.register.byte_0 == right.register.byte_0) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_1 = ((left.register.byte_1 == right.register.byte_1) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_2 = ((left.register.byte_2 == right.register.byte_2) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_3 = ((left.register.byte_3 == right.register.byte_3) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_4 = ((left.register.byte_4 == right.register.byte_4) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_5 = ((left.register.byte_5 == right.register.byte_5) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_6 = ((left.register.byte_6 == right.register.byte_6) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_7 = ((left.register.byte_7 == right.register.byte_7) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_8 = ((left.register.byte_8 == right.register.byte_8) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_9 = ((left.register.byte_9 == right.register.byte_9) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_10 = ((left.register.byte_10 == right.register.byte_10) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_11 = ((left.register.byte_11 == right.register.byte_11) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_12 = ((left.register.byte_12 == right.register.byte_12) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_13 = ((left.register.byte_13 == right.register.byte_13) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_14 = ((left.register.byte_14 == right.register.byte_14) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_15 = ((left.register.byte_15 == right.register.byte_15) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(sbyte))
				{
					register.sbyte_0 = ((left.register.sbyte_0 == right.register.sbyte_0) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_1 = ((left.register.sbyte_1 == right.register.sbyte_1) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_2 = ((left.register.sbyte_2 == right.register.sbyte_2) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_3 = ((left.register.sbyte_3 == right.register.sbyte_3) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_4 = ((left.register.sbyte_4 == right.register.sbyte_4) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_5 = ((left.register.sbyte_5 == right.register.sbyte_5) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_6 = ((left.register.sbyte_6 == right.register.sbyte_6) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_7 = ((left.register.sbyte_7 == right.register.sbyte_7) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_8 = ((left.register.sbyte_8 == right.register.sbyte_8) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_9 = ((left.register.sbyte_9 == right.register.sbyte_9) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_10 = ((left.register.sbyte_10 == right.register.sbyte_10) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_11 = ((left.register.sbyte_11 == right.register.sbyte_11) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_12 = ((left.register.sbyte_12 == right.register.sbyte_12) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_13 = ((left.register.sbyte_13 == right.register.sbyte_13) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_14 = ((left.register.sbyte_14 == right.register.sbyte_14) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_15 = ((left.register.sbyte_15 == right.register.sbyte_15) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ushort))
				{
					register.uint16_0 = ((left.register.uint16_0 == right.register.uint16_0) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_1 = ((left.register.uint16_1 == right.register.uint16_1) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_2 = ((left.register.uint16_2 == right.register.uint16_2) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_3 = ((left.register.uint16_3 == right.register.uint16_3) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_4 = ((left.register.uint16_4 == right.register.uint16_4) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_5 = ((left.register.uint16_5 == right.register.uint16_5) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_6 = ((left.register.uint16_6 == right.register.uint16_6) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_7 = ((left.register.uint16_7 == right.register.uint16_7) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(short))
				{
					register.int16_0 = ((left.register.int16_0 == right.register.int16_0) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_1 = ((left.register.int16_1 == right.register.int16_1) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_2 = ((left.register.int16_2 == right.register.int16_2) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_3 = ((left.register.int16_3 == right.register.int16_3) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_4 = ((left.register.int16_4 == right.register.int16_4) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_5 = ((left.register.int16_5 == right.register.int16_5) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_6 = ((left.register.int16_6 == right.register.int16_6) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_7 = ((left.register.int16_7 == right.register.int16_7) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(uint))
				{
					register.uint32_0 = ((left.register.uint32_0 == right.register.uint32_0) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_1 = ((left.register.uint32_1 == right.register.uint32_1) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_2 = ((left.register.uint32_2 == right.register.uint32_2) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_3 = ((left.register.uint32_3 == right.register.uint32_3) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(int))
				{
					register.int32_0 = ((left.register.int32_0 == right.register.int32_0) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_1 = ((left.register.int32_1 == right.register.int32_1) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_2 = ((left.register.int32_2 == right.register.int32_2) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_3 = ((left.register.int32_3 == right.register.int32_3) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ulong))
				{
					register.uint64_0 = ((left.register.uint64_0 == right.register.uint64_0) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					register.uint64_1 = ((left.register.uint64_1 == right.register.uint64_1) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(long))
				{
					register.int64_0 = ((left.register.int64_0 == right.register.int64_0) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					register.int64_1 = ((left.register.int64_1 == right.register.int64_1) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(float))
				{
					register.single_0 = ((left.register.single_0 == right.register.single_0) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_1 = ((left.register.single_1 == right.register.single_1) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_2 = ((left.register.single_2 == right.register.single_2) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_3 = ((left.register.single_3 == right.register.single_3) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(double))
				{
					register.double_0 = ((left.register.double_0 == right.register.double_0) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					register.double_1 = ((left.register.double_1 == right.register.double_1) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					return new Vector<T>(ref register);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static Vector<T> LessThan(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (Vector<T>.ScalarLessThan(left[i], right[i]) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (Vector<T>.ScalarLessThan(left[j], right[j]) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (Vector<T>.ScalarLessThan(left[k], right[k]) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (Vector<T>.ScalarLessThan(left[l], right[l]) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (Vector<T>.ScalarLessThan(left[m], right[m]) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (Vector<T>.ScalarLessThan(left[n], right[n]) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (Vector<T>.ScalarLessThan(left[num], right[num]) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (Vector<T>.ScalarLessThan(left[num2], right[num2]) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (Vector<T>.ScalarLessThan(left[num3], right[num3]) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = (Vector<T>.ScalarLessThan(left[num4], right[num4]) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				Register register = default(Register);
				if (typeof(T) == typeof(byte))
				{
					register.byte_0 = ((left.register.byte_0 < right.register.byte_0) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_1 = ((left.register.byte_1 < right.register.byte_1) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_2 = ((left.register.byte_2 < right.register.byte_2) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_3 = ((left.register.byte_3 < right.register.byte_3) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_4 = ((left.register.byte_4 < right.register.byte_4) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_5 = ((left.register.byte_5 < right.register.byte_5) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_6 = ((left.register.byte_6 < right.register.byte_6) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_7 = ((left.register.byte_7 < right.register.byte_7) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_8 = ((left.register.byte_8 < right.register.byte_8) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_9 = ((left.register.byte_9 < right.register.byte_9) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_10 = ((left.register.byte_10 < right.register.byte_10) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_11 = ((left.register.byte_11 < right.register.byte_11) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_12 = ((left.register.byte_12 < right.register.byte_12) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_13 = ((left.register.byte_13 < right.register.byte_13) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_14 = ((left.register.byte_14 < right.register.byte_14) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_15 = ((left.register.byte_15 < right.register.byte_15) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(sbyte))
				{
					register.sbyte_0 = ((left.register.sbyte_0 < right.register.sbyte_0) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_1 = ((left.register.sbyte_1 < right.register.sbyte_1) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_2 = ((left.register.sbyte_2 < right.register.sbyte_2) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_3 = ((left.register.sbyte_3 < right.register.sbyte_3) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_4 = ((left.register.sbyte_4 < right.register.sbyte_4) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_5 = ((left.register.sbyte_5 < right.register.sbyte_5) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_6 = ((left.register.sbyte_6 < right.register.sbyte_6) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_7 = ((left.register.sbyte_7 < right.register.sbyte_7) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_8 = ((left.register.sbyte_8 < right.register.sbyte_8) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_9 = ((left.register.sbyte_9 < right.register.sbyte_9) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_10 = ((left.register.sbyte_10 < right.register.sbyte_10) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_11 = ((left.register.sbyte_11 < right.register.sbyte_11) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_12 = ((left.register.sbyte_12 < right.register.sbyte_12) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_13 = ((left.register.sbyte_13 < right.register.sbyte_13) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_14 = ((left.register.sbyte_14 < right.register.sbyte_14) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_15 = ((left.register.sbyte_15 < right.register.sbyte_15) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ushort))
				{
					register.uint16_0 = ((left.register.uint16_0 < right.register.uint16_0) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_1 = ((left.register.uint16_1 < right.register.uint16_1) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_2 = ((left.register.uint16_2 < right.register.uint16_2) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_3 = ((left.register.uint16_3 < right.register.uint16_3) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_4 = ((left.register.uint16_4 < right.register.uint16_4) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_5 = ((left.register.uint16_5 < right.register.uint16_5) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_6 = ((left.register.uint16_6 < right.register.uint16_6) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_7 = ((left.register.uint16_7 < right.register.uint16_7) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(short))
				{
					register.int16_0 = ((left.register.int16_0 < right.register.int16_0) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_1 = ((left.register.int16_1 < right.register.int16_1) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_2 = ((left.register.int16_2 < right.register.int16_2) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_3 = ((left.register.int16_3 < right.register.int16_3) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_4 = ((left.register.int16_4 < right.register.int16_4) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_5 = ((left.register.int16_5 < right.register.int16_5) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_6 = ((left.register.int16_6 < right.register.int16_6) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_7 = ((left.register.int16_7 < right.register.int16_7) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(uint))
				{
					register.uint32_0 = ((left.register.uint32_0 < right.register.uint32_0) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_1 = ((left.register.uint32_1 < right.register.uint32_1) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_2 = ((left.register.uint32_2 < right.register.uint32_2) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_3 = ((left.register.uint32_3 < right.register.uint32_3) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(int))
				{
					register.int32_0 = ((left.register.int32_0 < right.register.int32_0) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_1 = ((left.register.int32_1 < right.register.int32_1) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_2 = ((left.register.int32_2 < right.register.int32_2) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_3 = ((left.register.int32_3 < right.register.int32_3) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ulong))
				{
					register.uint64_0 = ((left.register.uint64_0 < right.register.uint64_0) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					register.uint64_1 = ((left.register.uint64_1 < right.register.uint64_1) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(long))
				{
					register.int64_0 = ((left.register.int64_0 < right.register.int64_0) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					register.int64_1 = ((left.register.int64_1 < right.register.int64_1) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(float))
				{
					register.single_0 = ((left.register.single_0 < right.register.single_0) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_1 = ((left.register.single_1 < right.register.single_1) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_2 = ((left.register.single_2 < right.register.single_2) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_3 = ((left.register.single_3 < right.register.single_3) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(double))
				{
					register.double_0 = ((left.register.double_0 < right.register.double_0) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					register.double_1 = ((left.register.double_1 < right.register.double_1) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					return new Vector<T>(ref register);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		[JitIntrinsic]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static Vector<T> GreaterThan(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (Vector<T>.ScalarGreaterThan(left[i], right[i]) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (Vector<T>.ScalarGreaterThan(left[j], right[j]) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (Vector<T>.ScalarGreaterThan(left[k], right[k]) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (Vector<T>.ScalarGreaterThan(left[l], right[l]) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (Vector<T>.ScalarGreaterThan(left[m], right[m]) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (Vector<T>.ScalarGreaterThan(left[n], right[n]) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (Vector<T>.ScalarGreaterThan(left[num], right[num]) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (Vector<T>.ScalarGreaterThan(left[num2], right[num2]) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (Vector<T>.ScalarGreaterThan(left[num3], right[num3]) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = (Vector<T>.ScalarGreaterThan(left[num4], right[num4]) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				Register register = default(Register);
				if (typeof(T) == typeof(byte))
				{
					register.byte_0 = ((left.register.byte_0 > right.register.byte_0) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_1 = ((left.register.byte_1 > right.register.byte_1) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_2 = ((left.register.byte_2 > right.register.byte_2) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_3 = ((left.register.byte_3 > right.register.byte_3) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_4 = ((left.register.byte_4 > right.register.byte_4) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_5 = ((left.register.byte_5 > right.register.byte_5) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_6 = ((left.register.byte_6 > right.register.byte_6) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_7 = ((left.register.byte_7 > right.register.byte_7) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_8 = ((left.register.byte_8 > right.register.byte_8) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_9 = ((left.register.byte_9 > right.register.byte_9) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_10 = ((left.register.byte_10 > right.register.byte_10) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_11 = ((left.register.byte_11 > right.register.byte_11) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_12 = ((left.register.byte_12 > right.register.byte_12) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_13 = ((left.register.byte_13 > right.register.byte_13) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_14 = ((left.register.byte_14 > right.register.byte_14) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					register.byte_15 = ((left.register.byte_15 > right.register.byte_15) ? ConstantHelper.GetByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(sbyte))
				{
					register.sbyte_0 = ((left.register.sbyte_0 > right.register.sbyte_0) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_1 = ((left.register.sbyte_1 > right.register.sbyte_1) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_2 = ((left.register.sbyte_2 > right.register.sbyte_2) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_3 = ((left.register.sbyte_3 > right.register.sbyte_3) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_4 = ((left.register.sbyte_4 > right.register.sbyte_4) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_5 = ((left.register.sbyte_5 > right.register.sbyte_5) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_6 = ((left.register.sbyte_6 > right.register.sbyte_6) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_7 = ((left.register.sbyte_7 > right.register.sbyte_7) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_8 = ((left.register.sbyte_8 > right.register.sbyte_8) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_9 = ((left.register.sbyte_9 > right.register.sbyte_9) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_10 = ((left.register.sbyte_10 > right.register.sbyte_10) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_11 = ((left.register.sbyte_11 > right.register.sbyte_11) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_12 = ((left.register.sbyte_12 > right.register.sbyte_12) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_13 = ((left.register.sbyte_13 > right.register.sbyte_13) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_14 = ((left.register.sbyte_14 > right.register.sbyte_14) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					register.sbyte_15 = ((left.register.sbyte_15 > right.register.sbyte_15) ? ConstantHelper.GetSByteWithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ushort))
				{
					register.uint16_0 = ((left.register.uint16_0 > right.register.uint16_0) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_1 = ((left.register.uint16_1 > right.register.uint16_1) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_2 = ((left.register.uint16_2 > right.register.uint16_2) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_3 = ((left.register.uint16_3 > right.register.uint16_3) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_4 = ((left.register.uint16_4 > right.register.uint16_4) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_5 = ((left.register.uint16_5 > right.register.uint16_5) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_6 = ((left.register.uint16_6 > right.register.uint16_6) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					register.uint16_7 = ((left.register.uint16_7 > right.register.uint16_7) ? ConstantHelper.GetUInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(short))
				{
					register.int16_0 = ((left.register.int16_0 > right.register.int16_0) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_1 = ((left.register.int16_1 > right.register.int16_1) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_2 = ((left.register.int16_2 > right.register.int16_2) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_3 = ((left.register.int16_3 > right.register.int16_3) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_4 = ((left.register.int16_4 > right.register.int16_4) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_5 = ((left.register.int16_5 > right.register.int16_5) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_6 = ((left.register.int16_6 > right.register.int16_6) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					register.int16_7 = ((left.register.int16_7 > right.register.int16_7) ? ConstantHelper.GetInt16WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(uint))
				{
					register.uint32_0 = ((left.register.uint32_0 > right.register.uint32_0) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_1 = ((left.register.uint32_1 > right.register.uint32_1) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_2 = ((left.register.uint32_2 > right.register.uint32_2) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					register.uint32_3 = ((left.register.uint32_3 > right.register.uint32_3) ? ConstantHelper.GetUInt32WithAllBitsSet() : 0U);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(int))
				{
					register.int32_0 = ((left.register.int32_0 > right.register.int32_0) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_1 = ((left.register.int32_1 > right.register.int32_1) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_2 = ((left.register.int32_2 > right.register.int32_2) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					register.int32_3 = ((left.register.int32_3 > right.register.int32_3) ? ConstantHelper.GetInt32WithAllBitsSet() : 0);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(ulong))
				{
					register.uint64_0 = ((left.register.uint64_0 > right.register.uint64_0) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					register.uint64_1 = ((left.register.uint64_1 > right.register.uint64_1) ? ConstantHelper.GetUInt64WithAllBitsSet() : 0UL);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(long))
				{
					register.int64_0 = ((left.register.int64_0 > right.register.int64_0) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					register.int64_1 = ((left.register.int64_1 > right.register.int64_1) ? ConstantHelper.GetInt64WithAllBitsSet() : 0L);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(float))
				{
					register.single_0 = ((left.register.single_0 > right.register.single_0) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_1 = ((left.register.single_1 > right.register.single_1) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_2 = ((left.register.single_2 > right.register.single_2) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					register.single_3 = ((left.register.single_3 > right.register.single_3) ? ConstantHelper.GetSingleWithAllBitsSet() : 0f);
					return new Vector<T>(ref register);
				}
				if (typeof(T) == typeof(double))
				{
					register.double_0 = ((left.register.double_0 > right.register.double_0) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					register.double_1 = ((left.register.double_1 > right.register.double_1) ? ConstantHelper.GetDoubleWithAllBitsSet() : 0.0);
					return new Vector<T>(ref register);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000E603 File Offset: 0x0000C803
		[JitIntrinsic]
		internal static Vector<T> GreaterThanOrEqual(Vector<T> left, Vector<T> right)
		{
			return Vector<T>.Equals(left, right) | Vector<T>.GreaterThan(left, right);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000E618 File Offset: 0x0000C818
		[JitIntrinsic]
		internal static Vector<T> LessThanOrEqual(Vector<T> left, Vector<T> right)
		{
			return Vector<T>.Equals(left, right) | Vector<T>.LessThan(left, right);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000E62D File Offset: 0x0000C82D
		[JitIntrinsic]
		internal static Vector<T> ConditionalSelect(Vector<T> condition, Vector<T> left, Vector<T> right)
		{
			return (left & condition) | Vector.AndNot<T>(right, condition);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000E644 File Offset: 0x0000C844
		[JitIntrinsic]
		internal unsafe static Vector<T> Abs(Vector<T> value)
		{
			if (typeof(T) == typeof(byte))
			{
				return value;
			}
			if (typeof(T) == typeof(ushort))
			{
				return value;
			}
			if (typeof(T) == typeof(uint))
			{
				return value;
			}
			if (typeof(T) == typeof(ulong))
			{
				return value;
			}
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr;
					checked
					{
						ptr = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (sbyte)Math.Abs((sbyte)((object)value[i]));
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr2;
					checked
					{
						ptr2 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (short)Math.Abs((short)((object)value[j]));
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr3;
					checked
					{
						ptr3 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (int)Math.Abs((int)((object)value[k]));
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr4;
					checked
					{
						ptr4 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (long)Math.Abs((long)((object)value[l]));
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr5;
					checked
					{
						ptr5 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (float)Math.Abs((float)((object)value[m]));
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr6;
					checked
					{
						ptr6 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (double)Math.Abs((double)((object)value[n]));
					}
					return new Vector<T>((void*)ptr6);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				if (typeof(T) == typeof(sbyte))
				{
					value.register.sbyte_0 = Math.Abs(value.register.sbyte_0);
					value.register.sbyte_1 = Math.Abs(value.register.sbyte_1);
					value.register.sbyte_2 = Math.Abs(value.register.sbyte_2);
					value.register.sbyte_3 = Math.Abs(value.register.sbyte_3);
					value.register.sbyte_4 = Math.Abs(value.register.sbyte_4);
					value.register.sbyte_5 = Math.Abs(value.register.sbyte_5);
					value.register.sbyte_6 = Math.Abs(value.register.sbyte_6);
					value.register.sbyte_7 = Math.Abs(value.register.sbyte_7);
					value.register.sbyte_8 = Math.Abs(value.register.sbyte_8);
					value.register.sbyte_9 = Math.Abs(value.register.sbyte_9);
					value.register.sbyte_10 = Math.Abs(value.register.sbyte_10);
					value.register.sbyte_11 = Math.Abs(value.register.sbyte_11);
					value.register.sbyte_12 = Math.Abs(value.register.sbyte_12);
					value.register.sbyte_13 = Math.Abs(value.register.sbyte_13);
					value.register.sbyte_14 = Math.Abs(value.register.sbyte_14);
					value.register.sbyte_15 = Math.Abs(value.register.sbyte_15);
					return value;
				}
				if (typeof(T) == typeof(short))
				{
					value.register.int16_0 = Math.Abs(value.register.int16_0);
					value.register.int16_1 = Math.Abs(value.register.int16_1);
					value.register.int16_2 = Math.Abs(value.register.int16_2);
					value.register.int16_3 = Math.Abs(value.register.int16_3);
					value.register.int16_4 = Math.Abs(value.register.int16_4);
					value.register.int16_5 = Math.Abs(value.register.int16_5);
					value.register.int16_6 = Math.Abs(value.register.int16_6);
					value.register.int16_7 = Math.Abs(value.register.int16_7);
					return value;
				}
				if (typeof(T) == typeof(int))
				{
					value.register.int32_0 = Math.Abs(value.register.int32_0);
					value.register.int32_1 = Math.Abs(value.register.int32_1);
					value.register.int32_2 = Math.Abs(value.register.int32_2);
					value.register.int32_3 = Math.Abs(value.register.int32_3);
					return value;
				}
				if (typeof(T) == typeof(long))
				{
					value.register.int64_0 = Math.Abs(value.register.int64_0);
					value.register.int64_1 = Math.Abs(value.register.int64_1);
					return value;
				}
				if (typeof(T) == typeof(float))
				{
					value.register.single_0 = Math.Abs(value.register.single_0);
					value.register.single_1 = Math.Abs(value.register.single_1);
					value.register.single_2 = Math.Abs(value.register.single_2);
					value.register.single_3 = Math.Abs(value.register.single_3);
					return value;
				}
				if (typeof(T) == typeof(double))
				{
					value.register.double_0 = Math.Abs(value.register.double_0);
					value.register.double_1 = Math.Abs(value.register.double_1);
					return value;
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000EE10 File Offset: 0x0000D010
		[JitIntrinsic]
		internal unsafe static Vector<T> Min(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (Vector<T>.ScalarLessThan(left[i], right[i]) ? ((byte)((object)left[i])) : ((byte)((object)right[i])));
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (Vector<T>.ScalarLessThan(left[j], right[j]) ? ((sbyte)((object)left[j])) : ((sbyte)((object)right[j])));
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (Vector<T>.ScalarLessThan(left[k], right[k]) ? ((ushort)((object)left[k])) : ((ushort)((object)right[k])));
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (Vector<T>.ScalarLessThan(left[l], right[l]) ? ((short)((object)left[l])) : ((short)((object)right[l])));
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (Vector<T>.ScalarLessThan(left[m], right[m]) ? ((uint)((object)left[m])) : ((uint)((object)right[m])));
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (Vector<T>.ScalarLessThan(left[n], right[n]) ? ((int)((object)left[n])) : ((int)((object)right[n])));
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (Vector<T>.ScalarLessThan(left[num], right[num]) ? ((ulong)((object)left[num])) : ((ulong)((object)right[num])));
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (Vector<T>.ScalarLessThan(left[num2], right[num2]) ? ((long)((object)left[num2])) : ((long)((object)right[num2])));
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (Vector<T>.ScalarLessThan(left[num3], right[num3]) ? ((float)((object)left[num3])) : ((float)((object)right[num3])));
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = (Vector<T>.ScalarLessThan(left[num4], right[num4]) ? ((double)((object)left[num4])) : ((double)((object)right[num4])));
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = ((left.register.byte_0 < right.register.byte_0) ? left.register.byte_0 : right.register.byte_0);
					vector.register.byte_1 = ((left.register.byte_1 < right.register.byte_1) ? left.register.byte_1 : right.register.byte_1);
					vector.register.byte_2 = ((left.register.byte_2 < right.register.byte_2) ? left.register.byte_2 : right.register.byte_2);
					vector.register.byte_3 = ((left.register.byte_3 < right.register.byte_3) ? left.register.byte_3 : right.register.byte_3);
					vector.register.byte_4 = ((left.register.byte_4 < right.register.byte_4) ? left.register.byte_4 : right.register.byte_4);
					vector.register.byte_5 = ((left.register.byte_5 < right.register.byte_5) ? left.register.byte_5 : right.register.byte_5);
					vector.register.byte_6 = ((left.register.byte_6 < right.register.byte_6) ? left.register.byte_6 : right.register.byte_6);
					vector.register.byte_7 = ((left.register.byte_7 < right.register.byte_7) ? left.register.byte_7 : right.register.byte_7);
					vector.register.byte_8 = ((left.register.byte_8 < right.register.byte_8) ? left.register.byte_8 : right.register.byte_8);
					vector.register.byte_9 = ((left.register.byte_9 < right.register.byte_9) ? left.register.byte_9 : right.register.byte_9);
					vector.register.byte_10 = ((left.register.byte_10 < right.register.byte_10) ? left.register.byte_10 : right.register.byte_10);
					vector.register.byte_11 = ((left.register.byte_11 < right.register.byte_11) ? left.register.byte_11 : right.register.byte_11);
					vector.register.byte_12 = ((left.register.byte_12 < right.register.byte_12) ? left.register.byte_12 : right.register.byte_12);
					vector.register.byte_13 = ((left.register.byte_13 < right.register.byte_13) ? left.register.byte_13 : right.register.byte_13);
					vector.register.byte_14 = ((left.register.byte_14 < right.register.byte_14) ? left.register.byte_14 : right.register.byte_14);
					vector.register.byte_15 = ((left.register.byte_15 < right.register.byte_15) ? left.register.byte_15 : right.register.byte_15);
					return vector;
				}
				if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = ((left.register.sbyte_0 < right.register.sbyte_0) ? left.register.sbyte_0 : right.register.sbyte_0);
					vector.register.sbyte_1 = ((left.register.sbyte_1 < right.register.sbyte_1) ? left.register.sbyte_1 : right.register.sbyte_1);
					vector.register.sbyte_2 = ((left.register.sbyte_2 < right.register.sbyte_2) ? left.register.sbyte_2 : right.register.sbyte_2);
					vector.register.sbyte_3 = ((left.register.sbyte_3 < right.register.sbyte_3) ? left.register.sbyte_3 : right.register.sbyte_3);
					vector.register.sbyte_4 = ((left.register.sbyte_4 < right.register.sbyte_4) ? left.register.sbyte_4 : right.register.sbyte_4);
					vector.register.sbyte_5 = ((left.register.sbyte_5 < right.register.sbyte_5) ? left.register.sbyte_5 : right.register.sbyte_5);
					vector.register.sbyte_6 = ((left.register.sbyte_6 < right.register.sbyte_6) ? left.register.sbyte_6 : right.register.sbyte_6);
					vector.register.sbyte_7 = ((left.register.sbyte_7 < right.register.sbyte_7) ? left.register.sbyte_7 : right.register.sbyte_7);
					vector.register.sbyte_8 = ((left.register.sbyte_8 < right.register.sbyte_8) ? left.register.sbyte_8 : right.register.sbyte_8);
					vector.register.sbyte_9 = ((left.register.sbyte_9 < right.register.sbyte_9) ? left.register.sbyte_9 : right.register.sbyte_9);
					vector.register.sbyte_10 = ((left.register.sbyte_10 < right.register.sbyte_10) ? left.register.sbyte_10 : right.register.sbyte_10);
					vector.register.sbyte_11 = ((left.register.sbyte_11 < right.register.sbyte_11) ? left.register.sbyte_11 : right.register.sbyte_11);
					vector.register.sbyte_12 = ((left.register.sbyte_12 < right.register.sbyte_12) ? left.register.sbyte_12 : right.register.sbyte_12);
					vector.register.sbyte_13 = ((left.register.sbyte_13 < right.register.sbyte_13) ? left.register.sbyte_13 : right.register.sbyte_13);
					vector.register.sbyte_14 = ((left.register.sbyte_14 < right.register.sbyte_14) ? left.register.sbyte_14 : right.register.sbyte_14);
					vector.register.sbyte_15 = ((left.register.sbyte_15 < right.register.sbyte_15) ? left.register.sbyte_15 : right.register.sbyte_15);
					return vector;
				}
				if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = ((left.register.uint16_0 < right.register.uint16_0) ? left.register.uint16_0 : right.register.uint16_0);
					vector.register.uint16_1 = ((left.register.uint16_1 < right.register.uint16_1) ? left.register.uint16_1 : right.register.uint16_1);
					vector.register.uint16_2 = ((left.register.uint16_2 < right.register.uint16_2) ? left.register.uint16_2 : right.register.uint16_2);
					vector.register.uint16_3 = ((left.register.uint16_3 < right.register.uint16_3) ? left.register.uint16_3 : right.register.uint16_3);
					vector.register.uint16_4 = ((left.register.uint16_4 < right.register.uint16_4) ? left.register.uint16_4 : right.register.uint16_4);
					vector.register.uint16_5 = ((left.register.uint16_5 < right.register.uint16_5) ? left.register.uint16_5 : right.register.uint16_5);
					vector.register.uint16_6 = ((left.register.uint16_6 < right.register.uint16_6) ? left.register.uint16_6 : right.register.uint16_6);
					vector.register.uint16_7 = ((left.register.uint16_7 < right.register.uint16_7) ? left.register.uint16_7 : right.register.uint16_7);
					return vector;
				}
				if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = ((left.register.int16_0 < right.register.int16_0) ? left.register.int16_0 : right.register.int16_0);
					vector.register.int16_1 = ((left.register.int16_1 < right.register.int16_1) ? left.register.int16_1 : right.register.int16_1);
					vector.register.int16_2 = ((left.register.int16_2 < right.register.int16_2) ? left.register.int16_2 : right.register.int16_2);
					vector.register.int16_3 = ((left.register.int16_3 < right.register.int16_3) ? left.register.int16_3 : right.register.int16_3);
					vector.register.int16_4 = ((left.register.int16_4 < right.register.int16_4) ? left.register.int16_4 : right.register.int16_4);
					vector.register.int16_5 = ((left.register.int16_5 < right.register.int16_5) ? left.register.int16_5 : right.register.int16_5);
					vector.register.int16_6 = ((left.register.int16_6 < right.register.int16_6) ? left.register.int16_6 : right.register.int16_6);
					vector.register.int16_7 = ((left.register.int16_7 < right.register.int16_7) ? left.register.int16_7 : right.register.int16_7);
					return vector;
				}
				if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = ((left.register.uint32_0 < right.register.uint32_0) ? left.register.uint32_0 : right.register.uint32_0);
					vector.register.uint32_1 = ((left.register.uint32_1 < right.register.uint32_1) ? left.register.uint32_1 : right.register.uint32_1);
					vector.register.uint32_2 = ((left.register.uint32_2 < right.register.uint32_2) ? left.register.uint32_2 : right.register.uint32_2);
					vector.register.uint32_3 = ((left.register.uint32_3 < right.register.uint32_3) ? left.register.uint32_3 : right.register.uint32_3);
					return vector;
				}
				if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = ((left.register.int32_0 < right.register.int32_0) ? left.register.int32_0 : right.register.int32_0);
					vector.register.int32_1 = ((left.register.int32_1 < right.register.int32_1) ? left.register.int32_1 : right.register.int32_1);
					vector.register.int32_2 = ((left.register.int32_2 < right.register.int32_2) ? left.register.int32_2 : right.register.int32_2);
					vector.register.int32_3 = ((left.register.int32_3 < right.register.int32_3) ? left.register.int32_3 : right.register.int32_3);
					return vector;
				}
				if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = ((left.register.uint64_0 < right.register.uint64_0) ? left.register.uint64_0 : right.register.uint64_0);
					vector.register.uint64_1 = ((left.register.uint64_1 < right.register.uint64_1) ? left.register.uint64_1 : right.register.uint64_1);
					return vector;
				}
				if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = ((left.register.int64_0 < right.register.int64_0) ? left.register.int64_0 : right.register.int64_0);
					vector.register.int64_1 = ((left.register.int64_1 < right.register.int64_1) ? left.register.int64_1 : right.register.int64_1);
					return vector;
				}
				if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = ((left.register.single_0 < right.register.single_0) ? left.register.single_0 : right.register.single_0);
					vector.register.single_1 = ((left.register.single_1 < right.register.single_1) ? left.register.single_1 : right.register.single_1);
					vector.register.single_2 = ((left.register.single_2 < right.register.single_2) ? left.register.single_2 : right.register.single_2);
					vector.register.single_3 = ((left.register.single_3 < right.register.single_3) ? left.register.single_3 : right.register.single_3);
					return vector;
				}
				if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = ((left.register.double_0 < right.register.double_0) ? left.register.double_0 : right.register.double_0);
					vector.register.double_1 = ((left.register.double_1 < right.register.double_1) ? left.register.double_1 : right.register.double_1);
					return vector;
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00010464 File Offset: 0x0000E664
		[JitIntrinsic]
		internal unsafe static Vector<T> Max(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (Vector<T>.ScalarGreaterThan(left[i], right[i]) ? ((byte)((object)left[i])) : ((byte)((object)right[i])));
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (Vector<T>.ScalarGreaterThan(left[j], right[j]) ? ((sbyte)((object)left[j])) : ((sbyte)((object)right[j])));
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (Vector<T>.ScalarGreaterThan(left[k], right[k]) ? ((ushort)((object)left[k])) : ((ushort)((object)right[k])));
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (Vector<T>.ScalarGreaterThan(left[l], right[l]) ? ((short)((object)left[l])) : ((short)((object)right[l])));
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (Vector<T>.ScalarGreaterThan(left[m], right[m]) ? ((uint)((object)left[m])) : ((uint)((object)right[m])));
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (Vector<T>.ScalarGreaterThan(left[n], right[n]) ? ((int)((object)left[n])) : ((int)((object)right[n])));
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (Vector<T>.ScalarGreaterThan(left[num], right[num]) ? ((ulong)((object)left[num])) : ((ulong)((object)right[num])));
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (Vector<T>.ScalarGreaterThan(left[num2], right[num2]) ? ((long)((object)left[num2])) : ((long)((object)right[num2])));
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (Vector<T>.ScalarGreaterThan(left[num3], right[num3]) ? ((float)((object)left[num3])) : ((float)((object)right[num3])));
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = (Vector<T>.ScalarGreaterThan(left[num4], right[num4]) ? ((double)((object)left[num4])) : ((double)((object)right[num4])));
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				Vector<T> vector = default(Vector<T>);
				if (typeof(T) == typeof(byte))
				{
					vector.register.byte_0 = ((left.register.byte_0 > right.register.byte_0) ? left.register.byte_0 : right.register.byte_0);
					vector.register.byte_1 = ((left.register.byte_1 > right.register.byte_1) ? left.register.byte_1 : right.register.byte_1);
					vector.register.byte_2 = ((left.register.byte_2 > right.register.byte_2) ? left.register.byte_2 : right.register.byte_2);
					vector.register.byte_3 = ((left.register.byte_3 > right.register.byte_3) ? left.register.byte_3 : right.register.byte_3);
					vector.register.byte_4 = ((left.register.byte_4 > right.register.byte_4) ? left.register.byte_4 : right.register.byte_4);
					vector.register.byte_5 = ((left.register.byte_5 > right.register.byte_5) ? left.register.byte_5 : right.register.byte_5);
					vector.register.byte_6 = ((left.register.byte_6 > right.register.byte_6) ? left.register.byte_6 : right.register.byte_6);
					vector.register.byte_7 = ((left.register.byte_7 > right.register.byte_7) ? left.register.byte_7 : right.register.byte_7);
					vector.register.byte_8 = ((left.register.byte_8 > right.register.byte_8) ? left.register.byte_8 : right.register.byte_8);
					vector.register.byte_9 = ((left.register.byte_9 > right.register.byte_9) ? left.register.byte_9 : right.register.byte_9);
					vector.register.byte_10 = ((left.register.byte_10 > right.register.byte_10) ? left.register.byte_10 : right.register.byte_10);
					vector.register.byte_11 = ((left.register.byte_11 > right.register.byte_11) ? left.register.byte_11 : right.register.byte_11);
					vector.register.byte_12 = ((left.register.byte_12 > right.register.byte_12) ? left.register.byte_12 : right.register.byte_12);
					vector.register.byte_13 = ((left.register.byte_13 > right.register.byte_13) ? left.register.byte_13 : right.register.byte_13);
					vector.register.byte_14 = ((left.register.byte_14 > right.register.byte_14) ? left.register.byte_14 : right.register.byte_14);
					vector.register.byte_15 = ((left.register.byte_15 > right.register.byte_15) ? left.register.byte_15 : right.register.byte_15);
					return vector;
				}
				if (typeof(T) == typeof(sbyte))
				{
					vector.register.sbyte_0 = ((left.register.sbyte_0 > right.register.sbyte_0) ? left.register.sbyte_0 : right.register.sbyte_0);
					vector.register.sbyte_1 = ((left.register.sbyte_1 > right.register.sbyte_1) ? left.register.sbyte_1 : right.register.sbyte_1);
					vector.register.sbyte_2 = ((left.register.sbyte_2 > right.register.sbyte_2) ? left.register.sbyte_2 : right.register.sbyte_2);
					vector.register.sbyte_3 = ((left.register.sbyte_3 > right.register.sbyte_3) ? left.register.sbyte_3 : right.register.sbyte_3);
					vector.register.sbyte_4 = ((left.register.sbyte_4 > right.register.sbyte_4) ? left.register.sbyte_4 : right.register.sbyte_4);
					vector.register.sbyte_5 = ((left.register.sbyte_5 > right.register.sbyte_5) ? left.register.sbyte_5 : right.register.sbyte_5);
					vector.register.sbyte_6 = ((left.register.sbyte_6 > right.register.sbyte_6) ? left.register.sbyte_6 : right.register.sbyte_6);
					vector.register.sbyte_7 = ((left.register.sbyte_7 > right.register.sbyte_7) ? left.register.sbyte_7 : right.register.sbyte_7);
					vector.register.sbyte_8 = ((left.register.sbyte_8 > right.register.sbyte_8) ? left.register.sbyte_8 : right.register.sbyte_8);
					vector.register.sbyte_9 = ((left.register.sbyte_9 > right.register.sbyte_9) ? left.register.sbyte_9 : right.register.sbyte_9);
					vector.register.sbyte_10 = ((left.register.sbyte_10 > right.register.sbyte_10) ? left.register.sbyte_10 : right.register.sbyte_10);
					vector.register.sbyte_11 = ((left.register.sbyte_11 > right.register.sbyte_11) ? left.register.sbyte_11 : right.register.sbyte_11);
					vector.register.sbyte_12 = ((left.register.sbyte_12 > right.register.sbyte_12) ? left.register.sbyte_12 : right.register.sbyte_12);
					vector.register.sbyte_13 = ((left.register.sbyte_13 > right.register.sbyte_13) ? left.register.sbyte_13 : right.register.sbyte_13);
					vector.register.sbyte_14 = ((left.register.sbyte_14 > right.register.sbyte_14) ? left.register.sbyte_14 : right.register.sbyte_14);
					vector.register.sbyte_15 = ((left.register.sbyte_15 > right.register.sbyte_15) ? left.register.sbyte_15 : right.register.sbyte_15);
					return vector;
				}
				if (typeof(T) == typeof(ushort))
				{
					vector.register.uint16_0 = ((left.register.uint16_0 > right.register.uint16_0) ? left.register.uint16_0 : right.register.uint16_0);
					vector.register.uint16_1 = ((left.register.uint16_1 > right.register.uint16_1) ? left.register.uint16_1 : right.register.uint16_1);
					vector.register.uint16_2 = ((left.register.uint16_2 > right.register.uint16_2) ? left.register.uint16_2 : right.register.uint16_2);
					vector.register.uint16_3 = ((left.register.uint16_3 > right.register.uint16_3) ? left.register.uint16_3 : right.register.uint16_3);
					vector.register.uint16_4 = ((left.register.uint16_4 > right.register.uint16_4) ? left.register.uint16_4 : right.register.uint16_4);
					vector.register.uint16_5 = ((left.register.uint16_5 > right.register.uint16_5) ? left.register.uint16_5 : right.register.uint16_5);
					vector.register.uint16_6 = ((left.register.uint16_6 > right.register.uint16_6) ? left.register.uint16_6 : right.register.uint16_6);
					vector.register.uint16_7 = ((left.register.uint16_7 > right.register.uint16_7) ? left.register.uint16_7 : right.register.uint16_7);
					return vector;
				}
				if (typeof(T) == typeof(short))
				{
					vector.register.int16_0 = ((left.register.int16_0 > right.register.int16_0) ? left.register.int16_0 : right.register.int16_0);
					vector.register.int16_1 = ((left.register.int16_1 > right.register.int16_1) ? left.register.int16_1 : right.register.int16_1);
					vector.register.int16_2 = ((left.register.int16_2 > right.register.int16_2) ? left.register.int16_2 : right.register.int16_2);
					vector.register.int16_3 = ((left.register.int16_3 > right.register.int16_3) ? left.register.int16_3 : right.register.int16_3);
					vector.register.int16_4 = ((left.register.int16_4 > right.register.int16_4) ? left.register.int16_4 : right.register.int16_4);
					vector.register.int16_5 = ((left.register.int16_5 > right.register.int16_5) ? left.register.int16_5 : right.register.int16_5);
					vector.register.int16_6 = ((left.register.int16_6 > right.register.int16_6) ? left.register.int16_6 : right.register.int16_6);
					vector.register.int16_7 = ((left.register.int16_7 > right.register.int16_7) ? left.register.int16_7 : right.register.int16_7);
					return vector;
				}
				if (typeof(T) == typeof(uint))
				{
					vector.register.uint32_0 = ((left.register.uint32_0 > right.register.uint32_0) ? left.register.uint32_0 : right.register.uint32_0);
					vector.register.uint32_1 = ((left.register.uint32_1 > right.register.uint32_1) ? left.register.uint32_1 : right.register.uint32_1);
					vector.register.uint32_2 = ((left.register.uint32_2 > right.register.uint32_2) ? left.register.uint32_2 : right.register.uint32_2);
					vector.register.uint32_3 = ((left.register.uint32_3 > right.register.uint32_3) ? left.register.uint32_3 : right.register.uint32_3);
					return vector;
				}
				if (typeof(T) == typeof(int))
				{
					vector.register.int32_0 = ((left.register.int32_0 > right.register.int32_0) ? left.register.int32_0 : right.register.int32_0);
					vector.register.int32_1 = ((left.register.int32_1 > right.register.int32_1) ? left.register.int32_1 : right.register.int32_1);
					vector.register.int32_2 = ((left.register.int32_2 > right.register.int32_2) ? left.register.int32_2 : right.register.int32_2);
					vector.register.int32_3 = ((left.register.int32_3 > right.register.int32_3) ? left.register.int32_3 : right.register.int32_3);
					return vector;
				}
				if (typeof(T) == typeof(ulong))
				{
					vector.register.uint64_0 = ((left.register.uint64_0 > right.register.uint64_0) ? left.register.uint64_0 : right.register.uint64_0);
					vector.register.uint64_1 = ((left.register.uint64_1 > right.register.uint64_1) ? left.register.uint64_1 : right.register.uint64_1);
					return vector;
				}
				if (typeof(T) == typeof(long))
				{
					vector.register.int64_0 = ((left.register.int64_0 > right.register.int64_0) ? left.register.int64_0 : right.register.int64_0);
					vector.register.int64_1 = ((left.register.int64_1 > right.register.int64_1) ? left.register.int64_1 : right.register.int64_1);
					return vector;
				}
				if (typeof(T) == typeof(float))
				{
					vector.register.single_0 = ((left.register.single_0 > right.register.single_0) ? left.register.single_0 : right.register.single_0);
					vector.register.single_1 = ((left.register.single_1 > right.register.single_1) ? left.register.single_1 : right.register.single_1);
					vector.register.single_2 = ((left.register.single_2 > right.register.single_2) ? left.register.single_2 : right.register.single_2);
					vector.register.single_3 = ((left.register.single_3 > right.register.single_3) ? left.register.single_3 : right.register.single_3);
					return vector;
				}
				if (typeof(T) == typeof(double))
				{
					vector.register.double_0 = ((left.register.double_0 > right.register.double_0) ? left.register.double_0 : right.register.double_0);
					vector.register.double_1 = ((left.register.double_1 > right.register.double_1) ? left.register.double_1 : right.register.double_1);
					return vector;
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		[JitIntrinsic]
		internal static T DotProduct(Vector<T> left, Vector<T> right)
		{
			if (Vector.IsHardwareAccelerated)
			{
				T t = Vector<T>.GetZeroValue();
				for (int i = 0; i < Vector<T>.Count; i++)
				{
					t = Vector<T>.ScalarAdd(t, Vector<T>.ScalarMultiply(left[i], right[i]));
				}
				return t;
			}
			if (typeof(T) == typeof(byte))
			{
				byte b = 0;
				b += left.register.byte_0 * right.register.byte_0;
				b += left.register.byte_1 * right.register.byte_1;
				b += left.register.byte_2 * right.register.byte_2;
				b += left.register.byte_3 * right.register.byte_3;
				b += left.register.byte_4 * right.register.byte_4;
				b += left.register.byte_5 * right.register.byte_5;
				b += left.register.byte_6 * right.register.byte_6;
				b += left.register.byte_7 * right.register.byte_7;
				b += left.register.byte_8 * right.register.byte_8;
				b += left.register.byte_9 * right.register.byte_9;
				b += left.register.byte_10 * right.register.byte_10;
				b += left.register.byte_11 * right.register.byte_11;
				b += left.register.byte_12 * right.register.byte_12;
				b += left.register.byte_13 * right.register.byte_13;
				b += left.register.byte_14 * right.register.byte_14;
				b += left.register.byte_15 * right.register.byte_15;
				return (T)((object)b);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte b2 = 0;
				b2 += left.register.sbyte_0 * right.register.sbyte_0;
				b2 += left.register.sbyte_1 * right.register.sbyte_1;
				b2 += left.register.sbyte_2 * right.register.sbyte_2;
				b2 += left.register.sbyte_3 * right.register.sbyte_3;
				b2 += left.register.sbyte_4 * right.register.sbyte_4;
				b2 += left.register.sbyte_5 * right.register.sbyte_5;
				b2 += left.register.sbyte_6 * right.register.sbyte_6;
				b2 += left.register.sbyte_7 * right.register.sbyte_7;
				b2 += left.register.sbyte_8 * right.register.sbyte_8;
				b2 += left.register.sbyte_9 * right.register.sbyte_9;
				b2 += left.register.sbyte_10 * right.register.sbyte_10;
				b2 += left.register.sbyte_11 * right.register.sbyte_11;
				b2 += left.register.sbyte_12 * right.register.sbyte_12;
				b2 += left.register.sbyte_13 * right.register.sbyte_13;
				b2 += left.register.sbyte_14 * right.register.sbyte_14;
				b2 += left.register.sbyte_15 * right.register.sbyte_15;
				return (T)((object)b2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort num = 0;
				num += left.register.uint16_0 * right.register.uint16_0;
				num += left.register.uint16_1 * right.register.uint16_1;
				num += left.register.uint16_2 * right.register.uint16_2;
				num += left.register.uint16_3 * right.register.uint16_3;
				num += left.register.uint16_4 * right.register.uint16_4;
				num += left.register.uint16_5 * right.register.uint16_5;
				num += left.register.uint16_6 * right.register.uint16_6;
				num += left.register.uint16_7 * right.register.uint16_7;
				return (T)((object)num);
			}
			if (typeof(T) == typeof(short))
			{
				short num2 = 0;
				num2 += left.register.int16_0 * right.register.int16_0;
				num2 += left.register.int16_1 * right.register.int16_1;
				num2 += left.register.int16_2 * right.register.int16_2;
				num2 += left.register.int16_3 * right.register.int16_3;
				num2 += left.register.int16_4 * right.register.int16_4;
				num2 += left.register.int16_5 * right.register.int16_5;
				num2 += left.register.int16_6 * right.register.int16_6;
				num2 += left.register.int16_7 * right.register.int16_7;
				return (T)((object)num2);
			}
			if (typeof(T) == typeof(uint))
			{
				uint num3 = 0U;
				num3 += left.register.uint32_0 * right.register.uint32_0;
				num3 += left.register.uint32_1 * right.register.uint32_1;
				num3 += left.register.uint32_2 * right.register.uint32_2;
				num3 += left.register.uint32_3 * right.register.uint32_3;
				return (T)((object)num3);
			}
			if (typeof(T) == typeof(int))
			{
				int num4 = 0;
				num4 += left.register.int32_0 * right.register.int32_0;
				num4 += left.register.int32_1 * right.register.int32_1;
				num4 += left.register.int32_2 * right.register.int32_2;
				num4 += left.register.int32_3 * right.register.int32_3;
				return (T)((object)num4);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong num5 = 0UL;
				num5 += left.register.uint64_0 * right.register.uint64_0;
				num5 += left.register.uint64_1 * right.register.uint64_1;
				return (T)((object)num5);
			}
			if (typeof(T) == typeof(long))
			{
				long num6 = 0L;
				num6 += left.register.int64_0 * right.register.int64_0;
				num6 += left.register.int64_1 * right.register.int64_1;
				return (T)((object)num6);
			}
			if (typeof(T) == typeof(float))
			{
				float num7 = 0f;
				num7 += left.register.single_0 * right.register.single_0;
				num7 += left.register.single_1 * right.register.single_1;
				num7 += left.register.single_2 * right.register.single_2;
				num7 += left.register.single_3 * right.register.single_3;
				return (T)((object)num7);
			}
			if (typeof(T) == typeof(double))
			{
				double num8 = 0.0;
				num8 += left.register.double_0 * right.register.double_0;
				num8 += left.register.double_1 * right.register.double_1;
				return (T)((object)num8);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00012438 File Offset: 0x00010638
		[JitIntrinsic]
		internal unsafe static Vector<T> SquareRoot(Vector<T> value)
		{
			if (Vector.IsHardwareAccelerated)
			{
				if (typeof(T) == typeof(byte))
				{
					byte* ptr;
					checked
					{
						ptr = stackalloc byte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int i = 0; i < Vector<T>.Count; i++)
					{
						ptr[i] = (byte)Math.Sqrt((double)((byte)((object)value[i])));
					}
					return new Vector<T>((void*)ptr);
				}
				if (typeof(T) == typeof(sbyte))
				{
					sbyte* ptr2;
					checked
					{
						ptr2 = stackalloc sbyte[unchecked((UIntPtr)Vector<T>.Count) * 1];
					}
					for (int j = 0; j < Vector<T>.Count; j++)
					{
						ptr2[j] = (sbyte)Math.Sqrt((double)((sbyte)((object)value[j])));
					}
					return new Vector<T>((void*)ptr2);
				}
				if (typeof(T) == typeof(ushort))
				{
					ushort* ptr3;
					checked
					{
						ptr3 = stackalloc ushort[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int k = 0; k < Vector<T>.Count; k++)
					{
						ptr3[k] = (ushort)Math.Sqrt((double)((ushort)((object)value[k])));
					}
					return new Vector<T>((void*)ptr3);
				}
				if (typeof(T) == typeof(short))
				{
					short* ptr4;
					checked
					{
						ptr4 = stackalloc short[unchecked((UIntPtr)Vector<T>.Count) * 2];
					}
					for (int l = 0; l < Vector<T>.Count; l++)
					{
						ptr4[l] = (short)Math.Sqrt((double)((short)((object)value[l])));
					}
					return new Vector<T>((void*)ptr4);
				}
				if (typeof(T) == typeof(uint))
				{
					uint* ptr5;
					checked
					{
						ptr5 = stackalloc uint[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int m = 0; m < Vector<T>.Count; m++)
					{
						ptr5[m] = (uint)Math.Sqrt((uint)((object)value[m]));
					}
					return new Vector<T>((void*)ptr5);
				}
				if (typeof(T) == typeof(int))
				{
					int* ptr6;
					checked
					{
						ptr6 = stackalloc int[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int n = 0; n < Vector<T>.Count; n++)
					{
						ptr6[n] = (int)Math.Sqrt((double)((int)((object)value[n])));
					}
					return new Vector<T>((void*)ptr6);
				}
				if (typeof(T) == typeof(ulong))
				{
					ulong* ptr7;
					checked
					{
						ptr7 = stackalloc ulong[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num = 0; num < Vector<T>.Count; num++)
					{
						ptr7[num] = (ulong)Math.Sqrt((ulong)((object)value[num]));
					}
					return new Vector<T>((void*)ptr7);
				}
				if (typeof(T) == typeof(long))
				{
					long* ptr8;
					checked
					{
						ptr8 = stackalloc long[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num2 = 0; num2 < Vector<T>.Count; num2++)
					{
						ptr8[num2] = (long)Math.Sqrt((double)((long)((object)value[num2])));
					}
					return new Vector<T>((void*)ptr8);
				}
				if (typeof(T) == typeof(float))
				{
					float* ptr9;
					checked
					{
						ptr9 = stackalloc float[unchecked((UIntPtr)Vector<T>.Count) * 4];
					}
					for (int num3 = 0; num3 < Vector<T>.Count; num3++)
					{
						ptr9[num3] = (float)Math.Sqrt((double)((float)((object)value[num3])));
					}
					return new Vector<T>((void*)ptr9);
				}
				if (typeof(T) == typeof(double))
				{
					double* ptr10;
					checked
					{
						ptr10 = stackalloc double[unchecked((UIntPtr)Vector<T>.Count) * 8];
					}
					for (int num4 = 0; num4 < Vector<T>.Count; num4++)
					{
						ptr10[num4] = Math.Sqrt((double)((object)value[num4]));
					}
					return new Vector<T>((void*)ptr10);
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
			else
			{
				if (typeof(T) == typeof(byte))
				{
					value.register.byte_0 = (byte)Math.Sqrt((double)value.register.byte_0);
					value.register.byte_1 = (byte)Math.Sqrt((double)value.register.byte_1);
					value.register.byte_2 = (byte)Math.Sqrt((double)value.register.byte_2);
					value.register.byte_3 = (byte)Math.Sqrt((double)value.register.byte_3);
					value.register.byte_4 = (byte)Math.Sqrt((double)value.register.byte_4);
					value.register.byte_5 = (byte)Math.Sqrt((double)value.register.byte_5);
					value.register.byte_6 = (byte)Math.Sqrt((double)value.register.byte_6);
					value.register.byte_7 = (byte)Math.Sqrt((double)value.register.byte_7);
					value.register.byte_8 = (byte)Math.Sqrt((double)value.register.byte_8);
					value.register.byte_9 = (byte)Math.Sqrt((double)value.register.byte_9);
					value.register.byte_10 = (byte)Math.Sqrt((double)value.register.byte_10);
					value.register.byte_11 = (byte)Math.Sqrt((double)value.register.byte_11);
					value.register.byte_12 = (byte)Math.Sqrt((double)value.register.byte_12);
					value.register.byte_13 = (byte)Math.Sqrt((double)value.register.byte_13);
					value.register.byte_14 = (byte)Math.Sqrt((double)value.register.byte_14);
					value.register.byte_15 = (byte)Math.Sqrt((double)value.register.byte_15);
					return value;
				}
				if (typeof(T) == typeof(sbyte))
				{
					value.register.sbyte_0 = (sbyte)Math.Sqrt((double)value.register.sbyte_0);
					value.register.sbyte_1 = (sbyte)Math.Sqrt((double)value.register.sbyte_1);
					value.register.sbyte_2 = (sbyte)Math.Sqrt((double)value.register.sbyte_2);
					value.register.sbyte_3 = (sbyte)Math.Sqrt((double)value.register.sbyte_3);
					value.register.sbyte_4 = (sbyte)Math.Sqrt((double)value.register.sbyte_4);
					value.register.sbyte_5 = (sbyte)Math.Sqrt((double)value.register.sbyte_5);
					value.register.sbyte_6 = (sbyte)Math.Sqrt((double)value.register.sbyte_6);
					value.register.sbyte_7 = (sbyte)Math.Sqrt((double)value.register.sbyte_7);
					value.register.sbyte_8 = (sbyte)Math.Sqrt((double)value.register.sbyte_8);
					value.register.sbyte_9 = (sbyte)Math.Sqrt((double)value.register.sbyte_9);
					value.register.sbyte_10 = (sbyte)Math.Sqrt((double)value.register.sbyte_10);
					value.register.sbyte_11 = (sbyte)Math.Sqrt((double)value.register.sbyte_11);
					value.register.sbyte_12 = (sbyte)Math.Sqrt((double)value.register.sbyte_12);
					value.register.sbyte_13 = (sbyte)Math.Sqrt((double)value.register.sbyte_13);
					value.register.sbyte_14 = (sbyte)Math.Sqrt((double)value.register.sbyte_14);
					value.register.sbyte_15 = (sbyte)Math.Sqrt((double)value.register.sbyte_15);
					return value;
				}
				if (typeof(T) == typeof(ushort))
				{
					value.register.uint16_0 = (ushort)Math.Sqrt((double)value.register.uint16_0);
					value.register.uint16_1 = (ushort)Math.Sqrt((double)value.register.uint16_1);
					value.register.uint16_2 = (ushort)Math.Sqrt((double)value.register.uint16_2);
					value.register.uint16_3 = (ushort)Math.Sqrt((double)value.register.uint16_3);
					value.register.uint16_4 = (ushort)Math.Sqrt((double)value.register.uint16_4);
					value.register.uint16_5 = (ushort)Math.Sqrt((double)value.register.uint16_5);
					value.register.uint16_6 = (ushort)Math.Sqrt((double)value.register.uint16_6);
					value.register.uint16_7 = (ushort)Math.Sqrt((double)value.register.uint16_7);
					return value;
				}
				if (typeof(T) == typeof(short))
				{
					value.register.int16_0 = (short)Math.Sqrt((double)value.register.int16_0);
					value.register.int16_1 = (short)Math.Sqrt((double)value.register.int16_1);
					value.register.int16_2 = (short)Math.Sqrt((double)value.register.int16_2);
					value.register.int16_3 = (short)Math.Sqrt((double)value.register.int16_3);
					value.register.int16_4 = (short)Math.Sqrt((double)value.register.int16_4);
					value.register.int16_5 = (short)Math.Sqrt((double)value.register.int16_5);
					value.register.int16_6 = (short)Math.Sqrt((double)value.register.int16_6);
					value.register.int16_7 = (short)Math.Sqrt((double)value.register.int16_7);
					return value;
				}
				if (typeof(T) == typeof(uint))
				{
					value.register.uint32_0 = (uint)Math.Sqrt(value.register.uint32_0);
					value.register.uint32_1 = (uint)Math.Sqrt(value.register.uint32_1);
					value.register.uint32_2 = (uint)Math.Sqrt(value.register.uint32_2);
					value.register.uint32_3 = (uint)Math.Sqrt(value.register.uint32_3);
					return value;
				}
				if (typeof(T) == typeof(int))
				{
					value.register.int32_0 = (int)Math.Sqrt((double)value.register.int32_0);
					value.register.int32_1 = (int)Math.Sqrt((double)value.register.int32_1);
					value.register.int32_2 = (int)Math.Sqrt((double)value.register.int32_2);
					value.register.int32_3 = (int)Math.Sqrt((double)value.register.int32_3);
					return value;
				}
				if (typeof(T) == typeof(ulong))
				{
					value.register.uint64_0 = (ulong)Math.Sqrt(value.register.uint64_0);
					value.register.uint64_1 = (ulong)Math.Sqrt(value.register.uint64_1);
					return value;
				}
				if (typeof(T) == typeof(long))
				{
					value.register.int64_0 = (long)Math.Sqrt((double)value.register.int64_0);
					value.register.int64_1 = (long)Math.Sqrt((double)value.register.int64_1);
					return value;
				}
				if (typeof(T) == typeof(float))
				{
					value.register.single_0 = (float)Math.Sqrt((double)value.register.single_0);
					value.register.single_1 = (float)Math.Sqrt((double)value.register.single_1);
					value.register.single_2 = (float)Math.Sqrt((double)value.register.single_2);
					value.register.single_3 = (float)Math.Sqrt((double)value.register.single_3);
					return value;
				}
				if (typeof(T) == typeof(double))
				{
					value.register.double_0 = Math.Sqrt(value.register.double_0);
					value.register.double_1 = Math.Sqrt(value.register.double_1);
					return value;
				}
				throw new NotSupportedException(SR.Arg_TypeNotSupported);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00013138 File Offset: 0x00011338
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ScalarEquals(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (byte)((object)left) == (byte)((object)right);
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (sbyte)((object)left) == (sbyte)((object)right);
			}
			if (typeof(T) == typeof(ushort))
			{
				return (ushort)((object)left) == (ushort)((object)right);
			}
			if (typeof(T) == typeof(short))
			{
				return (short)((object)left) == (short)((object)right);
			}
			if (typeof(T) == typeof(uint))
			{
				return (uint)((object)left) == (uint)((object)right);
			}
			if (typeof(T) == typeof(int))
			{
				return (int)((object)left) == (int)((object)right);
			}
			if (typeof(T) == typeof(ulong))
			{
				return (ulong)((object)left) == (ulong)((object)right);
			}
			if (typeof(T) == typeof(long))
			{
				return (long)((object)left) == (long)((object)right);
			}
			if (typeof(T) == typeof(float))
			{
				return (float)((object)left) == (float)((object)right);
			}
			if (typeof(T) == typeof(double))
			{
				return (double)((object)left) == (double)((object)right);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00013358 File Offset: 0x00011558
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ScalarLessThan(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (byte)((object)left) < (byte)((object)right);
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (sbyte)((object)left) < (sbyte)((object)right);
			}
			if (typeof(T) == typeof(ushort))
			{
				return (ushort)((object)left) < (ushort)((object)right);
			}
			if (typeof(T) == typeof(short))
			{
				return (short)((object)left) < (short)((object)right);
			}
			if (typeof(T) == typeof(uint))
			{
				return (uint)((object)left) < (uint)((object)right);
			}
			if (typeof(T) == typeof(int))
			{
				return (int)((object)left) < (int)((object)right);
			}
			if (typeof(T) == typeof(ulong))
			{
				return (ulong)((object)left) < (ulong)((object)right);
			}
			if (typeof(T) == typeof(long))
			{
				return (long)((object)left) < (long)((object)right);
			}
			if (typeof(T) == typeof(float))
			{
				return (float)((object)left) < (float)((object)right);
			}
			if (typeof(T) == typeof(double))
			{
				return (double)((object)left) < (double)((object)right);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00013578 File Offset: 0x00011778
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ScalarGreaterThan(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (byte)((object)left) > (byte)((object)right);
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (sbyte)((object)left) > (sbyte)((object)right);
			}
			if (typeof(T) == typeof(ushort))
			{
				return (ushort)((object)left) > (ushort)((object)right);
			}
			if (typeof(T) == typeof(short))
			{
				return (short)((object)left) > (short)((object)right);
			}
			if (typeof(T) == typeof(uint))
			{
				return (uint)((object)left) > (uint)((object)right);
			}
			if (typeof(T) == typeof(int))
			{
				return (int)((object)left) > (int)((object)right);
			}
			if (typeof(T) == typeof(ulong))
			{
				return (ulong)((object)left) > (ulong)((object)right);
			}
			if (typeof(T) == typeof(long))
			{
				return (long)((object)left) > (long)((object)right);
			}
			if (typeof(T) == typeof(float))
			{
				return (float)((object)left) > (float)((object)right);
			}
			if (typeof(T) == typeof(double))
			{
				return (double)((object)left) > (double)((object)right);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00013798 File Offset: 0x00011998
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T ScalarAdd(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (T)((object)((byte)((object)left) + (byte)((object)right)));
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (T)((object)((sbyte)((object)left) + (sbyte)((object)right)));
			}
			if (typeof(T) == typeof(ushort))
			{
				return (T)((object)((ushort)((object)left) + (ushort)((object)right)));
			}
			if (typeof(T) == typeof(short))
			{
				return (T)((object)((short)((object)left) + (short)((object)right)));
			}
			if (typeof(T) == typeof(uint))
			{
				return (T)((object)((uint)((object)left) + (uint)((object)right)));
			}
			if (typeof(T) == typeof(int))
			{
				return (T)((object)((int)((object)left) + (int)((object)right)));
			}
			if (typeof(T) == typeof(ulong))
			{
				return (T)((object)((ulong)((object)left) + (ulong)((object)right)));
			}
			if (typeof(T) == typeof(long))
			{
				return (T)((object)((long)((object)left) + (long)((object)right)));
			}
			if (typeof(T) == typeof(float))
			{
				return (T)((object)((float)((object)left) + (float)((object)right)));
			}
			if (typeof(T) == typeof(double))
			{
				return (T)((object)((double)((object)left) + (double)((object)right)));
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00013A18 File Offset: 0x00011C18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T ScalarSubtract(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (T)((object)((byte)((object)left) - (byte)((object)right)));
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (T)((object)((sbyte)((object)left) - (sbyte)((object)right)));
			}
			if (typeof(T) == typeof(ushort))
			{
				return (T)((object)((ushort)((object)left) - (ushort)((object)right)));
			}
			if (typeof(T) == typeof(short))
			{
				return (T)((object)((short)((object)left) - (short)((object)right)));
			}
			if (typeof(T) == typeof(uint))
			{
				return (T)((object)((uint)((object)left) - (uint)((object)right)));
			}
			if (typeof(T) == typeof(int))
			{
				return (T)((object)((int)((object)left) - (int)((object)right)));
			}
			if (typeof(T) == typeof(ulong))
			{
				return (T)((object)((ulong)((object)left) - (ulong)((object)right)));
			}
			if (typeof(T) == typeof(long))
			{
				return (T)((object)((long)((object)left) - (long)((object)right)));
			}
			if (typeof(T) == typeof(float))
			{
				return (T)((object)((float)((object)left) - (float)((object)right)));
			}
			if (typeof(T) == typeof(double))
			{
				return (T)((object)((double)((object)left) - (double)((object)right)));
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00013C98 File Offset: 0x00011E98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T ScalarMultiply(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (T)((object)((byte)((object)left) * (byte)((object)right)));
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (T)((object)((sbyte)((object)left) * (sbyte)((object)right)));
			}
			if (typeof(T) == typeof(ushort))
			{
				return (T)((object)((ushort)((object)left) * (ushort)((object)right)));
			}
			if (typeof(T) == typeof(short))
			{
				return (T)((object)((short)((object)left) * (short)((object)right)));
			}
			if (typeof(T) == typeof(uint))
			{
				return (T)((object)((uint)((object)left) * (uint)((object)right)));
			}
			if (typeof(T) == typeof(int))
			{
				return (T)((object)((int)((object)left) * (int)((object)right)));
			}
			if (typeof(T) == typeof(ulong))
			{
				return (T)((object)((ulong)((object)left) * (ulong)((object)right)));
			}
			if (typeof(T) == typeof(long))
			{
				return (T)((object)((long)((object)left) * (long)((object)right)));
			}
			if (typeof(T) == typeof(float))
			{
				return (T)((object)((float)((object)left) * (float)((object)right)));
			}
			if (typeof(T) == typeof(double))
			{
				return (T)((object)((double)((object)left) * (double)((object)right)));
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00013F18 File Offset: 0x00012118
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T ScalarDivide(T left, T right)
		{
			if (typeof(T) == typeof(byte))
			{
				return (T)((object)((byte)((object)left) / (byte)((object)right)));
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (T)((object)((sbyte)((object)left) / (sbyte)((object)right)));
			}
			if (typeof(T) == typeof(ushort))
			{
				return (T)((object)((ushort)((object)left) / (ushort)((object)right)));
			}
			if (typeof(T) == typeof(short))
			{
				return (T)((object)((short)((object)left) / (short)((object)right)));
			}
			if (typeof(T) == typeof(uint))
			{
				return (T)((object)((uint)((object)left) / (uint)((object)right)));
			}
			if (typeof(T) == typeof(int))
			{
				return (T)((object)((int)((object)left) / (int)((object)right)));
			}
			if (typeof(T) == typeof(ulong))
			{
				return (T)((object)((ulong)((object)left) / (ulong)((object)right)));
			}
			if (typeof(T) == typeof(long))
			{
				return (T)((object)((long)((object)left) / (long)((object)right)));
			}
			if (typeof(T) == typeof(float))
			{
				return (T)((object)((float)((object)left) / (float)((object)right)));
			}
			if (typeof(T) == typeof(double))
			{
				return (T)((object)((double)((object)left) / (double)((object)right)));
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00014198 File Offset: 0x00012398
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T GetZeroValue()
		{
			if (typeof(T) == typeof(byte))
			{
				byte b = 0;
				return (T)((object)b);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte b2 = 0;
				return (T)((object)b2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort num = 0;
				return (T)((object)num);
			}
			if (typeof(T) == typeof(short))
			{
				short num2 = 0;
				return (T)((object)num2);
			}
			if (typeof(T) == typeof(uint))
			{
				uint num3 = 0U;
				return (T)((object)num3);
			}
			if (typeof(T) == typeof(int))
			{
				int num4 = 0;
				return (T)((object)num4);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong num5 = 0UL;
				return (T)((object)num5);
			}
			if (typeof(T) == typeof(long))
			{
				long num6 = 0L;
				return (T)((object)num6);
			}
			if (typeof(T) == typeof(float))
			{
				float num7 = 0f;
				return (T)((object)num7);
			}
			if (typeof(T) == typeof(double))
			{
				double num8 = 0.0;
				return (T)((object)num8);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00014364 File Offset: 0x00012564
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T GetOneValue()
		{
			if (typeof(T) == typeof(byte))
			{
				byte b = 1;
				return (T)((object)b);
			}
			if (typeof(T) == typeof(sbyte))
			{
				sbyte b2 = 1;
				return (T)((object)b2);
			}
			if (typeof(T) == typeof(ushort))
			{
				ushort num = 1;
				return (T)((object)num);
			}
			if (typeof(T) == typeof(short))
			{
				short num2 = 1;
				return (T)((object)num2);
			}
			if (typeof(T) == typeof(uint))
			{
				uint num3 = 1U;
				return (T)((object)num3);
			}
			if (typeof(T) == typeof(int))
			{
				int num4 = 1;
				return (T)((object)num4);
			}
			if (typeof(T) == typeof(ulong))
			{
				ulong num5 = 1UL;
				return (T)((object)num5);
			}
			if (typeof(T) == typeof(long))
			{
				long num6 = 1L;
				return (T)((object)num6);
			}
			if (typeof(T) == typeof(float))
			{
				float num7 = 1f;
				return (T)((object)num7);
			}
			if (typeof(T) == typeof(double))
			{
				double num8 = 1.0;
				return (T)((object)num8);
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00014530 File Offset: 0x00012730
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T GetAllBitsSetValue()
		{
			if (typeof(T) == typeof(byte))
			{
				return (T)((object)ConstantHelper.GetByteWithAllBitsSet());
			}
			if (typeof(T) == typeof(sbyte))
			{
				return (T)((object)ConstantHelper.GetSByteWithAllBitsSet());
			}
			if (typeof(T) == typeof(ushort))
			{
				return (T)((object)ConstantHelper.GetUInt16WithAllBitsSet());
			}
			if (typeof(T) == typeof(short))
			{
				return (T)((object)ConstantHelper.GetInt16WithAllBitsSet());
			}
			if (typeof(T) == typeof(uint))
			{
				return (T)((object)ConstantHelper.GetUInt32WithAllBitsSet());
			}
			if (typeof(T) == typeof(int))
			{
				return (T)((object)ConstantHelper.GetInt32WithAllBitsSet());
			}
			if (typeof(T) == typeof(ulong))
			{
				return (T)((object)ConstantHelper.GetUInt64WithAllBitsSet());
			}
			if (typeof(T) == typeof(long))
			{
				return (T)((object)ConstantHelper.GetInt64WithAllBitsSet());
			}
			if (typeof(T) == typeof(float))
			{
				return (T)((object)ConstantHelper.GetSingleWithAllBitsSet());
			}
			if (typeof(T) == typeof(double))
			{
				return (T)((object)ConstantHelper.GetDoubleWithAllBitsSet());
			}
			throw new NotSupportedException(SR.Arg_TypeNotSupported);
		}

		// Token: 0x04000046 RID: 70
		private Register register;

		// Token: 0x04000047 RID: 71
		private static readonly int s_count = Vector<T>.InitializeCount();

		// Token: 0x04000048 RID: 72
		private static readonly Vector<T> zero = new Vector<T>(Vector<T>.GetZeroValue());

		// Token: 0x04000049 RID: 73
		private static readonly Vector<T> one = new Vector<T>(Vector<T>.GetOneValue());

		// Token: 0x0400004A RID: 74
		private static readonly Vector<T> allOnes = new Vector<T>(Vector<T>.GetAllBitsSetValue());

		// Token: 0x02000013 RID: 19
		private struct VectorSizeHelper
		{
			// Token: 0x04000073 RID: 115
			internal Vector<T> _placeholder;

			// Token: 0x04000074 RID: 116
			internal byte _byte;
		}
	}
}
