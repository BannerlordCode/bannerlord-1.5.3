using System;
using System.Globalization;

namespace TaleWorlds.Core
{
	// Token: 0x020000C1 RID: 193
	public class MountCreationKey
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00022B2A File Offset: 0x00020D2A
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00022B32 File Offset: 0x00020D32
		public byte _leftFrontLegColorIndex { get; private set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00022B3B File Offset: 0x00020D3B
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x00022B43 File Offset: 0x00020D43
		public byte _rightFrontLegColorIndex { get; private set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00022B4C File Offset: 0x00020D4C
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x00022B54 File Offset: 0x00020D54
		public byte _leftBackLegColorIndex { get; private set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00022B5D File Offset: 0x00020D5D
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00022B65 File Offset: 0x00020D65
		public byte _rightBackLegColorIndex { get; private set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00022B6E File Offset: 0x00020D6E
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00022B76 File Offset: 0x00020D76
		public byte MaterialIndex { get; private set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00022B7F File Offset: 0x00020D7F
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00022B87 File Offset: 0x00020D87
		public byte MeshMultiplierIndex { get; private set; }

		// Token: 0x06000ABF RID: 2751 RVA: 0x00022B90 File Offset: 0x00020D90
		public MountCreationKey(byte leftFrontLegColorIndex, byte rightFrontLegColorIndex, byte leftBackLegColorIndex, byte rightBackLegColorIndex, byte materialIndex, byte meshMultiplierIndex)
		{
			if (leftFrontLegColorIndex == 3 || rightFrontLegColorIndex == 3)
			{
				leftFrontLegColorIndex = 3;
				rightFrontLegColorIndex = 3;
			}
			this._leftFrontLegColorIndex = leftFrontLegColorIndex;
			this._rightFrontLegColorIndex = rightFrontLegColorIndex;
			this._leftBackLegColorIndex = leftBackLegColorIndex;
			this._rightBackLegColorIndex = rightBackLegColorIndex;
			this.MaterialIndex = materialIndex;
			this.MeshMultiplierIndex = meshMultiplierIndex;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00022BE0 File Offset: 0x00020DE0
		public static MountCreationKey FromString(string str)
		{
			if (str != null)
			{
				uint num = uint.Parse(str, NumberStyles.HexNumber);
				int bitsFromKey = MountCreationKey.GetBitsFromKey(num, 0, 2);
				int bitsFromKey2 = MountCreationKey.GetBitsFromKey(num, 2, 2);
				int bitsFromKey3 = MountCreationKey.GetBitsFromKey(num, 4, 2);
				int bitsFromKey4 = MountCreationKey.GetBitsFromKey(num, 6, 2);
				int bitsFromKey5 = MountCreationKey.GetBitsFromKey(num, 8, 2);
				int bitsFromKey6 = MountCreationKey.GetBitsFromKey(num, 10, 2);
				return new MountCreationKey((byte)bitsFromKey, (byte)bitsFromKey2, (byte)bitsFromKey3, (byte)bitsFromKey4, (byte)bitsFromKey5, (byte)bitsFromKey6);
			}
			return new MountCreationKey(0, 0, 0, 0, 0, 0);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00022C54 File Offset: 0x00020E54
		public override string ToString()
		{
			uint num = 0U;
			this.SetBits(ref num, (int)this._leftFrontLegColorIndex, 0);
			this.SetBits(ref num, (int)this._rightFrontLegColorIndex, 2);
			this.SetBits(ref num, (int)this._leftBackLegColorIndex, 4);
			this.SetBits(ref num, (int)this._rightBackLegColorIndex, 6);
			this.SetBits(ref num, (int)this.MaterialIndex, 8);
			this.SetBits(ref num, (int)this.MeshMultiplierIndex, 10);
			return num.ToString("X");
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00022CCC File Offset: 0x00020ECC
		private static int GetBitsFromKey(uint numericKey, int startingBit, int numBits)
		{
			int num = (int)(numericKey >> startingBit);
			uint num2 = (uint)(numBits * numBits - 1);
			return num & (int)num2;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00022CE8 File Offset: 0x00020EE8
		private void SetBits(ref uint numericKey, int value, int startingBit)
		{
			uint num = (uint)((uint)value << startingBit);
			numericKey |= num;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00022D04 File Offset: 0x00020F04
		public static string GetRandomMountKeyString(ItemObject mountItem, int randomSeed)
		{
			return MountCreationKey.GetRandomMountKey(mountItem, randomSeed).ToString();
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00022D14 File Offset: 0x00020F14
		public static MountCreationKey GetRandomMountKey(ItemObject mountItem, int randomSeed)
		{
			MBFastRandom mbfastRandom = new MBFastRandom((uint)randomSeed);
			if (mountItem == null)
			{
				return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), 0, 0);
			}
			HorseComponent horseComponent = mountItem.HorseComponent;
			if (horseComponent.HorseMaterialNames != null && horseComponent.HorseMaterialNames.Count > 0)
			{
				int num = mbfastRandom.Next(horseComponent.HorseMaterialNames.Count);
				float num2 = mbfastRandom.NextFloat();
				int num3 = 0;
				float num4 = 0f;
				HorseComponent.MaterialProperty materialProperty = horseComponent.HorseMaterialNames[num];
				for (int i = 0; i < materialProperty.MeshMultiplier.Count; i++)
				{
					num4 += materialProperty.MeshMultiplier[i].Item2;
					if (num2 <= num4)
					{
						num3 = i;
						break;
					}
				}
				return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)num, (byte)num3);
			}
			return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), 0, 0);
		}

		// Token: 0x040005F0 RID: 1520
		private const int NumLegColors = 4;
	}
}
