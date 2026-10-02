using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200009E RID: 158
	public static class RandomOwnerExtensions
	{
		// Token: 0x06001339 RID: 4921 RVA: 0x00057DFE File Offset: 0x00055FFE
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomIntWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00057E0C File Offset: 0x0005600C
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int max)
		{
			return obj.RandomIntWithSeed(seed, 0, max);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00057E17 File Offset: 0x00056017
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomIntWithSeed(seed), min, max);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00057E27 File Offset: 0x00056027
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomFloatWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00057E35 File Offset: 0x00056035
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float max)
		{
			return obj.RandomFloatWithSeed(seed, 0f, max);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00057E44 File Offset: 0x00056044
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloatWithSeed(seed), min, max);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00057E54 File Offset: 0x00056054
		public static int RandomInt(this IRandomOwner obj)
		{
			return obj.RandomValue;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00057E5C File Offset: 0x0005605C
		public static int RandomInt(this IRandomOwner obj, int max)
		{
			return obj.RandomInt(0, max);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00057E66 File Offset: 0x00056066
		public static int RandomInt(this IRandomOwner obj, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomInt(), min, max);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00057E75 File Offset: 0x00056075
		public static float RandomFloat(this IRandomOwner obj)
		{
			return (float)obj.RandomValue / 2.1474836E+09f;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00057E84 File Offset: 0x00056084
		public static float RandomFloat(this IRandomOwner obj, float max)
		{
			return obj.RandomFloat(0f, max);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00057E92 File Offset: 0x00056092
		public static float RandomFloat(this IRandomOwner obj, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloat(), min, max);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00057EA4 File Offset: 0x000560A4
		private static int Random(int randomValue, int min, int max)
		{
			int num = max - min;
			if (num == 0)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 79);
				return 0;
			}
			return min + randomValue % num;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00057ED8 File Offset: 0x000560D8
		private static float Random(float randomValue, float min, float max)
		{
			float num = max - min;
			if (num <= 1E-45f)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 91);
				return min;
			}
			return min + randomValue * num;
		}
	}
}
