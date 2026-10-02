using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000B4 RID: 180
	public static class MBRandom
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001EB62 File Offset: 0x0001CD62
		private static MBFastRandom Random
		{
			get
			{
				if (Game.Current != null)
				{
					return Game.Current.RandomGenerator;
				}
				if (MBRandom._internalRandom == null)
				{
					MBRandom._internalRandom = new MBFastRandom();
				}
				return MBRandom._internalRandom;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0001EB8C File Offset: 0x0001CD8C
		public static float RandomFloat
		{
			get
			{
				return MBRandom.Random.NextFloat();
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001EB98 File Offset: 0x0001CD98
		public static float RandomFloatRanged(float maxVal)
		{
			return MBRandom.RandomFloat * maxVal;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001EBA1 File Offset: 0x0001CDA1
		public static float RandomFloatRanged(float minVal, float maxVal)
		{
			return minVal + MBRandom.RandomFloat * (maxVal - minVal);
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		public static float RandomFloatNormal
		{
			get
			{
				int num = 4;
				float num2;
				float num4;
				do
				{
					num2 = 2f * MBRandom.RandomFloat - 1f;
					float num3 = 2f * MBRandom.RandomFloat - 1f;
					num4 = num2 * num2 + num3 * num3;
					num--;
				}
				while (num4 >= 1f || (num4 == 0f && num > 0));
				return num2 * num4 * 1f;
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001EC0C File Offset: 0x0001CE0C
		public static int RandomInt()
		{
			return MBRandom.Random.Next();
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001EC18 File Offset: 0x0001CE18
		public static int RandomInt(int maxValue)
		{
			return MBRandom.Random.Next(maxValue);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001EC25 File Offset: 0x0001CE25
		public static int RandomInt(int minValue, int maxValue)
		{
			return MBRandom.Random.Next(minValue, maxValue);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001EC34 File Offset: 0x0001CE34
		public static int RoundRandomized(float f)
		{
			int num = MathF.Floor(f);
			float num2 = f - (float)num;
			if (MBRandom.RandomFloat < num2)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001EC5C File Offset: 0x0001CE5C
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList)
		{
			int num;
			return MBRandom.ChooseWeighted<T>(weightList, out num);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001EC74 File Offset: 0x0001CE74
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList, out int chosenIndex)
		{
			chosenIndex = -1;
			float num = weightList.Sum<ValueTuple<T, float>>((ValueTuple<T, float> x) => x.Item2);
			float num2 = MBRandom.RandomFloat * num;
			for (int i = 0; i < weightList.Count; i++)
			{
				num2 -= weightList[i].Item2;
				if (num2 <= 0f)
				{
					chosenIndex = i;
					return weightList[i].Item1;
				}
			}
			if (weightList.Count > 0)
			{
				chosenIndex = 0;
				return weightList[0].Item1;
			}
			chosenIndex = -1;
			return default(T);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001ED10 File Offset: 0x0001CF10
		public static float RandomFloatGaussian(float center, float spread, float min, float max)
		{
			float num = 1f - MBRandom.RandomFloat;
			float num2 = 1f - MBRandom.RandomFloat;
			float num3 = MathF.Sqrt(-2f * MathF.Log(num)) * MathF.Sin(6.2831855f * num2);
			return MathF.Clamp(center + spread * num3, min, max);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001ED60 File Offset: 0x0001CF60
		public static void SetSeed(uint seed, uint seed2)
		{
			MBRandom.Random.SetSeed(seed, seed2);
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001ED6E File Offset: 0x0001CF6E
		public static float NondeterministicRandomFloat
		{
			get
			{
				return MBRandom.NondeterministicRandom.NextFloat();
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001ED7A File Offset: 0x0001CF7A
		public static int NondeterministicRandomInt
		{
			get
			{
				return MBRandom.NondeterministicRandom.Next();
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001ED86 File Offset: 0x0001CF86
		public static int RandomIntWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomInt(seed, seed2);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001ED8F File Offset: 0x0001CF8F
		public static float RandomFloatWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomFloat(seed, seed2);
		}

		// Token: 0x0400052F RID: 1327
		public const int MaxSeed = 2000;

		// Token: 0x04000530 RID: 1328
		private static MBFastRandom _internalRandom = null;

		// Token: 0x04000531 RID: 1329
		private static readonly MBFastRandom NondeterministicRandom = new MBFastRandom();
	}
}
