using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002EF RID: 751
	public class CompressionInfo
	{
		// Token: 0x020005D1 RID: 1489
		[EngineStruct("Integer_compression_info", false, null)]
		public struct Integer
		{
			// Token: 0x06003F40 RID: 16192 RVA: 0x000F96B8 File Offset: 0x000F78B8
			public Integer(int minimumValue, int maximumValue, bool maximumValueGiven)
			{
				this.maximumValue = maximumValue;
				this.minimumValue = minimumValue;
				uint num = (uint)(maximumValue - minimumValue);
				this.numberOfBits = MBMath.GetNumberOfBitsToRepresentNumber(num);
			}

			// Token: 0x06003F41 RID: 16193 RVA: 0x000F96E3 File Offset: 0x000F78E3
			public Integer(int minimumValue, int numberOfBits)
			{
				this.minimumValue = minimumValue;
				this.numberOfBits = numberOfBits;
				if (minimumValue == -2147483648 && numberOfBits == 32)
				{
					this.maximumValue = int.MaxValue;
					return;
				}
				this.maximumValue = minimumValue + (1 << numberOfBits) - 1;
			}

			// Token: 0x06003F42 RID: 16194 RVA: 0x000F971C File Offset: 0x000F791C
			public int GetNumBits()
			{
				return this.numberOfBits;
			}

			// Token: 0x06003F43 RID: 16195 RVA: 0x000F9724 File Offset: 0x000F7924
			public int GetMaximumValue()
			{
				return this.maximumValue;
			}

			// Token: 0x04001FB6 RID: 8118
			[CustomEngineStructMemberData("min_value")]
			private readonly int minimumValue;

			// Token: 0x04001FB7 RID: 8119
			[CustomEngineStructMemberData("max_value")]
			private readonly int maximumValue;

			// Token: 0x04001FB8 RID: 8120
			[CustomEngineStructMemberData("num_bits")]
			private readonly int numberOfBits;
		}

		// Token: 0x020005D2 RID: 1490
		[EngineStruct("Unsigned_integer_compression_info", false, null)]
		public struct UnsignedInteger
		{
			// Token: 0x06003F44 RID: 16196 RVA: 0x000F972C File Offset: 0x000F792C
			public UnsignedInteger(uint minimumValue, uint maximumValue, bool maximumValueGiven)
			{
				this.minimumValue = minimumValue;
				this.maximumValue = maximumValue;
				uint num = maximumValue - minimumValue;
				this.numberOfBits = MBMath.GetNumberOfBitsToRepresentNumber(num);
			}

			// Token: 0x06003F45 RID: 16197 RVA: 0x000F9757 File Offset: 0x000F7957
			public UnsignedInteger(uint minimumValue, int numberOfBits)
			{
				this.minimumValue = minimumValue;
				this.numberOfBits = numberOfBits;
				if (minimumValue == 0U && numberOfBits == 32)
				{
					this.maximumValue = uint.MaxValue;
					return;
				}
				this.maximumValue = (uint)((ulong)minimumValue + (1UL << numberOfBits) - 1UL);
			}

			// Token: 0x06003F46 RID: 16198 RVA: 0x000F978B File Offset: 0x000F798B
			public int GetNumBits()
			{
				return this.numberOfBits;
			}

			// Token: 0x04001FB9 RID: 8121
			[CustomEngineStructMemberData("min_value")]
			private readonly uint minimumValue;

			// Token: 0x04001FBA RID: 8122
			[CustomEngineStructMemberData("max_value")]
			private readonly uint maximumValue;

			// Token: 0x04001FBB RID: 8123
			[CustomEngineStructMemberData("num_bits")]
			private readonly int numberOfBits;
		}

		// Token: 0x020005D3 RID: 1491
		[EngineStruct("Integer64_compression_info", false, null)]
		public struct LongInteger
		{
			// Token: 0x06003F47 RID: 16199 RVA: 0x000F9794 File Offset: 0x000F7994
			public LongInteger(long minimumValue, long maximumValue, bool maximumValueGiven)
			{
				this.maximumValue = maximumValue;
				this.minimumValue = minimumValue;
				ulong num = (ulong)(maximumValue - minimumValue);
				this.numberOfBits = MBMath.GetNumberOfBitsToRepresentNumber(num);
			}

			// Token: 0x06003F48 RID: 16200 RVA: 0x000F97C0 File Offset: 0x000F79C0
			public LongInteger(long minimumValue, int numberOfBits)
			{
				this.minimumValue = minimumValue;
				this.numberOfBits = numberOfBits;
				if (minimumValue == -9223372036854775808L && numberOfBits == 64)
				{
					this.maximumValue = long.MaxValue;
					return;
				}
				this.maximumValue = minimumValue + (1L << numberOfBits) - 1L;
			}

			// Token: 0x06003F49 RID: 16201 RVA: 0x000F980E File Offset: 0x000F7A0E
			public int GetNumBits()
			{
				return this.numberOfBits;
			}

			// Token: 0x04001FBC RID: 8124
			[CustomEngineStructMemberData("min_value")]
			private readonly long minimumValue;

			// Token: 0x04001FBD RID: 8125
			[CustomEngineStructMemberData("max_value")]
			private readonly long maximumValue;

			// Token: 0x04001FBE RID: 8126
			[CustomEngineStructMemberData("num_bits")]
			private readonly int numberOfBits;
		}

		// Token: 0x020005D4 RID: 1492
		[EngineStruct("Unsigned_integer64_compression_info", false, null)]
		public struct UnsignedLongInteger
		{
			// Token: 0x06003F4A RID: 16202 RVA: 0x000F9818 File Offset: 0x000F7A18
			public UnsignedLongInteger(ulong minimumValue, ulong maximumValue, bool maximumValueGiven)
			{
				this.minimumValue = minimumValue;
				this.maximumValue = maximumValue;
				ulong num = maximumValue - minimumValue;
				this.numberOfBits = MBMath.GetNumberOfBitsToRepresentNumber(num);
			}

			// Token: 0x06003F4B RID: 16203 RVA: 0x000F9843 File Offset: 0x000F7A43
			public UnsignedLongInteger(ulong minimumValue, int numberOfBits)
			{
				this.minimumValue = minimumValue;
				this.numberOfBits = numberOfBits;
				if (minimumValue == 0UL && numberOfBits == 64)
				{
					this.maximumValue = ulong.MaxValue;
					return;
				}
				this.maximumValue = minimumValue + (1UL << numberOfBits) - 1UL;
			}

			// Token: 0x06003F4C RID: 16204 RVA: 0x000F9876 File Offset: 0x000F7A76
			public int GetNumBits()
			{
				return this.numberOfBits;
			}

			// Token: 0x04001FBF RID: 8127
			[CustomEngineStructMemberData("min_value")]
			private readonly ulong minimumValue;

			// Token: 0x04001FC0 RID: 8128
			[CustomEngineStructMemberData("max_value")]
			private readonly ulong maximumValue;

			// Token: 0x04001FC1 RID: 8129
			[CustomEngineStructMemberData("num_bits")]
			private readonly int numberOfBits;
		}

		// Token: 0x020005D5 RID: 1493
		[EngineStruct("Float_compression_info", false, null)]
		public struct Float
		{
			// Token: 0x17000AA7 RID: 2727
			// (get) Token: 0x06003F4D RID: 16205 RVA: 0x000F987E File Offset: 0x000F7A7E
			public static CompressionInfo.Float FullPrecision { get; } = new CompressionInfo.Float(true);

			// Token: 0x06003F4E RID: 16206 RVA: 0x000F9888 File Offset: 0x000F7A88
			public Float(float minimumValue, float maximumValue, int numberOfBits)
			{
				this.minimumValue = minimumValue;
				this.maximumValue = maximumValue;
				this.numberOfBits = numberOfBits;
				float num = maximumValue - minimumValue;
				int num2 = (1 << numberOfBits) - 1;
				this.precision = num / (float)num2;
			}

			// Token: 0x06003F4F RID: 16207 RVA: 0x000F98C4 File Offset: 0x000F7AC4
			public Float(float minimumValue, int numberOfBits, float precision)
			{
				this.minimumValue = minimumValue;
				this.precision = precision;
				this.numberOfBits = numberOfBits;
				int num = (1 << numberOfBits) - 1;
				float num2 = precision * (float)num;
				this.maximumValue = num2 + minimumValue;
			}

			// Token: 0x06003F50 RID: 16208 RVA: 0x000F98FD File Offset: 0x000F7AFD
			private Float(bool isFullPrecision)
			{
				this.minimumValue = float.MinValue;
				this.maximumValue = float.MaxValue;
				this.precision = 0f;
				this.numberOfBits = 32;
			}

			// Token: 0x06003F51 RID: 16209 RVA: 0x000F9928 File Offset: 0x000F7B28
			public int GetNumBits()
			{
				return this.numberOfBits;
			}

			// Token: 0x06003F52 RID: 16210 RVA: 0x000F9930 File Offset: 0x000F7B30
			public float GetMaximumValue()
			{
				return this.maximumValue;
			}

			// Token: 0x06003F53 RID: 16211 RVA: 0x000F9938 File Offset: 0x000F7B38
			public float GetMinimumValue()
			{
				return this.minimumValue;
			}

			// Token: 0x06003F54 RID: 16212 RVA: 0x000F9940 File Offset: 0x000F7B40
			public float GetPrecision()
			{
				return this.precision;
			}

			// Token: 0x06003F55 RID: 16213 RVA: 0x000F9948 File Offset: 0x000F7B48
			public void ClampValueAccordingToLimits(ref float x)
			{
				x = MathF.Clamp(x, this.minimumValue, this.maximumValue);
			}

			// Token: 0x04001FC3 RID: 8131
			[CustomEngineStructMemberData("min_value")]
			private readonly float minimumValue;

			// Token: 0x04001FC4 RID: 8132
			[CustomEngineStructMemberData("max_value")]
			private readonly float maximumValue;

			// Token: 0x04001FC5 RID: 8133
			[CustomEngineStructMemberData(true)]
			private readonly float precision;

			// Token: 0x04001FC6 RID: 8134
			[CustomEngineStructMemberData("num_bits")]
			private readonly int numberOfBits;
		}
	}
}
